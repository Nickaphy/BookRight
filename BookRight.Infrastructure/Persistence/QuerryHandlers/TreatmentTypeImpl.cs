using BookRight.Facade.Dtos.QuerryDto.TreatmentTypeDtos;
using BookRight.Facade.Querries.TreatmentTypeQuerries;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookRight.Infrastructure.Persistence.QuerryHandlers
{
    public class TreatmentTypeImpl : ITreatmentTypeQuerry
    {
        private readonly IDbContextFactory<AppDbContext> _factory;

        public TreatmentTypeImpl(IDbContextFactory<AppDbContext> factory)
        {
            _factory = factory;
        }

        public async Task<IReadOnlyList<TreatmentTypeDto>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            using var context = _factory.CreateDbContext();
            var treatments = await context.Treatments
                .AsNoTracking()
                .OrderBy(t => t.NeedsAuthorisation)
                .ThenBy(t => t.Name)
                .ThenBy(t => t.DurationMinutes)
                .ToListAsync(cancellationToken);

            return treatments.Select(t => new TreatmentTypeDto(
                t.Id,
                t.Name,
                t.DurationMinutes,
                t.BasePrice.Amount,
                t.NeedsAuthorisation.ToString(),
                t.MaxParticipants))
                .ToList();


        }
    }
}
