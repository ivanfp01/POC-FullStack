using Application.Repositories;
using Domain.Entities;
using Infrastructure.Repositories.Sql.Automoviles;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Sql.Automoviles
{
    public class AutomovilRepository : IAutomovilRepository
    {
        private readonly AutomovilesDbContext _context;

        public AutomovilRepository(AutomovilesDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<Automovil?> GetByIdAsync(int id)
        {
            return await _context.Automoviles.FindAsync(id);
        }

        public async Task<Automovil?> GetByVinAsync(string vin)
        {
            return await _context.Automoviles
                .FirstOrDefaultAsync(a => a.NumeroChasis.Value == vin);
        }

        public async Task<IReadOnlyList<Automovil>> GetAllAsync()
        {
            return await _context.Automoviles
                .OrderBy(a => a.Marca)
                .ThenBy(a => a.Modelo)
                .ToListAsync();
        }

        public async Task<int> AddAsync(Automovil automovil)
        {
            _context.Automoviles.Add(automovil);
            await _context.SaveChangesAsync();
            return automovil.Id;
        }

        public async Task UpdateAsync(Automovil automovil)
        {
            _context.Automoviles.Update(automovil);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Automovil automovil)
        {
            _context.Automoviles.Remove(automovil);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsByVinAsync(string vin, int? excludeId = null)
        {
            var query = _context.Automoviles.Where(a => a.NumeroChasis.Value == vin);
            
            if (excludeId.HasValue)
            {
                query = query.Where(a => a.Id != excludeId.Value);
            }

            return await query.AnyAsync();
        }

        public async Task<bool> ExistsByMotorNumberAsync(string numeroMotor, int? excludeId = null)
        {
            var query = _context.Automoviles.Where(a => a.NumeroMotor == numeroMotor);
            
            if (excludeId.HasValue)
            {
                query = query.Where(a => a.Id != excludeId.Value);
            }

            return await query.AnyAsync();
        }
    }
}