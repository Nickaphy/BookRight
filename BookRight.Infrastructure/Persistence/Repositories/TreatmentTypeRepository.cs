using BookRight.Application.Repositories;
using BookRight.Domain.Entities.Treatments;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookRight.Infrastructure.Persistence.Repositories
{
    public class TreatmentTypeRepository : ITreatmentTypeRepository
    {
        private readonly AppDbContext _dbContext;

        public TreatmentTypeRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<TreatmentType?> GetByIdAsync(Guid treatmentTypeId,
                                                        CancellationToken cancellationToken = default)
        {
            return await _dbContext.Treatments.FindAsync(treatmentTypeId, cancellationToken);
        }
    }
}
