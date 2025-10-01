using Application.DTOs.Automovil;

namespace Application.UseCases.Automovil
{
    public interface IAutomovilService
    {
        Task<int> CreateAsync(AutomovilCreateDto dto);
        Task UpdateAsync(int id, AutomovilUpdateDto dto);
        Task DeleteAsync(int id);
        Task<AutomovilReadDto?> GetByIdAsync(int id);
        Task<AutomovilReadDto?> GetByChasisAsync(string numeroChasis);
        Task<IReadOnlyList<AutomovilReadDto>> GetAllAsync();
    }
}