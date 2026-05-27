using BookRight.Domain.Enums;
using BookRight.Facade.Dtos.Reports;
using BookRight.Facade.Queries.Reports;
using BookRight.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BookRight.Infrastructure.Persistence.QuerryHandlers;

public class ClinicReportImpl : IClinicReportQueries
{
    private readonly IDbContextFactory<AppDbContext> _factory;

    public ClinicReportImpl(IDbContextFactory<AppDbContext> factory)
    {
        _factory = factory;
    }

    public async Task<IReadOnlyList<ClinicReportDto>> GetClinicReportsAsync(
        DateTime from,
        DateTime to,
        CancellationToken cancellationToken = default)
    {
        using var ctx = _factory.CreateDbContext();

        var clinics = await ctx.Clinics.AsNoTracking().ToListAsync(cancellationToken);

        var bookings = await ctx.Bookings
            .AsNoTracking()
            .Where(b => b.TimeRange.Start >= from && b.TimeRange.Start <= to)
            .ToListAsync(cancellationToken);

        var treatmentNames = await ctx.Treatments
            .AsNoTracking()
            .ToDictionaryAsync(t => t.Id, t => t.Name, cancellationToken);

        var reports = new List<ClinicReportDto>();

        foreach (var clinic in clinics.OrderBy(c => c.Name))
        {
            var cb = bookings.Where(b => b.ClinicId == clinic.Id).ToList();
            if (cb.Count == 0)
            {
                reports.Add(new ClinicReportDto(
                    clinic.Name, 0, 0, 0, 0, 0, 0, 0, "–", 0));
                continue;
            }

            var completed = cb.Count(b => b.Status == BookingStatus.Completed);
            var cancelled = cb.Count(b => b.Status == BookingStatus.Cancelled);
            var noShow = cb.Count(b => b.Status == BookingStatus.NoShow);
            var pending = cb.Count(b => b.Status == BookingStatus.Created);

            var revenue = cb
                .Where(b => b.Status == BookingStatus.Completed)
                .Sum(b => b.FinalPrice.Amount);

            var avgValue = completed > 0 ? Math.Round(revenue / completed, 0) : 0;

            var mostPopular = cb
                .GroupBy(b => b.TreatmentTypeId)
                .OrderByDescending(g => g.Count())
                .Select(g => treatmentNames.TryGetValue(g.Key, out var n) ? n : "–")
                .FirstOrDefault() ?? "–";

            var uniqueCustomers = cb.Select(b => b.CustomerId).Distinct().Count();

            reports.Add(new ClinicReportDto(
                clinic.Name,
                cb.Count,
                completed,
                cancelled,
                noShow,
                pending,
                revenue,
                avgValue,
                mostPopular,
                uniqueCustomers));
        }

        return reports;
    }
}