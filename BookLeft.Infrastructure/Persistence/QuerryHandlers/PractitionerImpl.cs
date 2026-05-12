using BookRight.Facade.Dtos.PractitionerQuerry;
using BookRight.Facade.Querries.PractitionerQuerries;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookRight.Infrastructure.Persistence.QuerryHandlers
{
    public class PractitionerImpl : IPractitionerQuerries
    {
        private readonly DbContext _context;

        public PractitionerImpl(DbContext context)
        {
            _context = context;
        }

        public async Task<PractitionerDto?> GetByIdAsync(Guid id)
        {
            return await _context.practitioner
                .AsNoTracking()
                .Where(p => p.Id == id)
                .Select(p => new PractitionerDto(
                    p.Name,
                    p.Email,
                    p.PhoneNumber,
                    p.AuthorizationCode,
                    p.Authorization))
                .FirstOrDefaultAsync();
        }

        public async Task<IReadOnlyList<PractitionerDto>> GetAllAsync()
        {
            return await _context.practitioner
                .AsNoTracking()
                .Select(p => new PractitionerDto(
                    p.Name,
                    p.Email,
                    p.PhoneNumber,
                    p.AuthorizationCode,
                    p.AuthorizationType))
                .ToListAsync();
        }
    }
}
