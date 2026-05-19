using BookRight.Facade.Dtos.PractitionerQuerry;
using BookRight.Facade.Querries.PractitionerQuerries;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookRight.Infrastructure.Persistence.QuerryHandlers
{
    public class PractitionerImpl : IPractitionerQuerries
    {
        private readonly AppDbContext _context;

        public PractitionerImpl(AppDbContext context)
        {
            _context = context;
        }

        public async Task<PractitionerDto?> GetByIdAsync(Guid id)
        {
            return await _context.Practitioners
                 .AsNoTracking()
                 .Where(p => p.Id == id)
                 .Select(p => new PractitionerDto(
                     p.Id,
                     p.Name,
                     p.Email,
                     p.PhoneNumber,
                     p.AuthorizationCode,
                     p.AuthorizationType.ToString()))
                 .FirstOrDefaultAsync();
        }

        public async Task<IReadOnlyList<PractitionerDto>> GetAllAsync()
        {
            return await _context.Practitioners
                .AsNoTracking()
                .Select(p => new PractitionerDto(
                    p.Id,
                    p.Name,
                    p.Email,
                    p.PhoneNumber,
                    p.AuthorizationCode,
                    p.AuthorizationType.ToString()))
                .ToListAsync();
        }
    }
}
