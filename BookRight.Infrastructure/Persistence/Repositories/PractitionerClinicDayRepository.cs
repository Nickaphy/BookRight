using BookRight.Application.Repositories;
using BookRight.Domain.Entities.Practitioners;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookRight.Infrastructure.Persistence.Repositories
{
    public class PractitionerClinicDayRepository : IPractitionerClinicDayRepository
    {
        private readonly AppDbContext _context;

        public PractitionerClinicDayRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<int> CountByClinicAndDateAsync(
            Guid clinicId,
            DateTime date,
            CancellationToken cancellationToken = default)
        {
            return await _context.PractitionerClinicDays
                .Where(pc => pc.ClinicId == clinicId && pc.Date == date.Date)
                .CountAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<PractitionerClinicDay>> GetByPractitionerAndClinicInRangeAsync(
            Guid practitionerId,
            Guid clinicId,
            DateTime rangeStart,
            DateTime rangeEnd,
            CancellationToken cancellationToken = default)
    {
        return await _context.PractitionerClinicDays
            .AsNoTracking()
            .Where(pc => pc.PractitionerId == practitionerId
                      && pc.ClinicId == clinicId
                      && pc.Date >= rangeStart
                      && pc.Date < rangeEnd)
            .ToListAsync(cancellationToken);
        }
}
}
