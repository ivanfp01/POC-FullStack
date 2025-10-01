using Domain.Entities;

namespace Application.Repositories
{
    public interface IAutomovilRepository
    {
        Task<Automovil?> GetByIdAsync(int id);
        Task<Automovil?> GetByVinAsync(string vin);
        Task<IReadOnlyList<Automovil>> GetAllAsync();
        Task<int> AddAsync(Automovil automovil);
        Task UpdateAsync(Automovil automovil);
        Task DeleteAsync(Automovil automovil);
        Task<bool> ExistsByVinAsync(string vin, int? excludeId = null);
        Task<bool> ExistsByMotorNumberAsync(string numeroMotor, int? excludeId = null);
    }
}