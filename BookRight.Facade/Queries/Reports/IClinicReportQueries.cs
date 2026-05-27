using BookRight.Facade.Dtos.Reports;

namespace BookRight.Facade.Queries.Reports;

public interface IClinicReportQueries
{
    Task<IReadOnlyList<ClinicReportDto>> GetClinicReportsAsync(
        DateTime from,
        DateTime to,
        CancellationToken cancellationToken = default);
}