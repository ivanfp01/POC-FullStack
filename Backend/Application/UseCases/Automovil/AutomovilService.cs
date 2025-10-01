using Application.DTOs.Automovil;
using Application.Repositories;
using Application.Services;
using Domain.ValueObjects;

namespace Application.UseCases.Automovil
{
    public class AutomovilService : IAutomovilService
    {
        private readonly IAutomovilRepository _repository;
        private readonly INumeroChasisService _numeroChasisService;
        private readonly IMotorService _motorService;

        public AutomovilService(IAutomovilRepository repository, INumeroChasisService numeroChasisService, IMotorService motorService)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _numeroChasisService = numeroChasisService ?? throw new ArgumentNullException(nameof(numeroChasisService));
            _motorService = motorService ?? throw new ArgumentNullException(nameof(motorService));
        }

        public async Task<int> CreateAsync(AutomovilCreateDto dto)
        {
            // Normalizar strings
            var marca = dto.Marca.Trim().ToUpperInvariant();
            var modelo = dto.Modelo.Trim().ToUpperInvariant();
            var color = dto.Color?.Trim();

            // Generar o validar NumeroChasis
            NumeroChasisVo numeroChasis;
            if (string.IsNullOrEmpty(dto.NumeroChasis))
            {
                numeroChasis = _numeroChasisService.Generate(marca, dto.Anio);
            }
            else
            {
                numeroChasis = _numeroChasisService.Validate(dto.NumeroChasis);
                
                // Verificar unicidad del número de chasis (VIN)
                var existeChasis = await _repository.ExistsByVinAsync(numeroChasis.Value);
                if (existeChasis)
                {
                    throw new InvalidOperationException($"Ya existe un automóvil con el número de chasis {numeroChasis.Value}");
                }
            }

            // Generar o validar NumeroMotor
            string numeroMotor;
            if (string.IsNullOrEmpty(dto.NumeroMotor))
            {
                numeroMotor = _motorService.Generate(marca, dto.Anio);
            }
            else
            {
                numeroMotor = _motorService.Validate(dto.NumeroMotor);
                
                // Verificar unicidad del motor
                var existeMotor = await _repository.ExistsByMotorNumberAsync(numeroMotor);
                if (existeMotor)
                {
                    throw new InvalidOperationException($"Ya existe un automóvil con el número de motor {numeroMotor}");
                }
            }

            var automovil = new Domain.Entities.Automovil
            {
                Marca = marca,
                Modelo = modelo,
                Anio = dto.Anio,
                Color = color,
                NumeroChasis = numeroChasis,
                NumeroMotor = numeroMotor,
                FechaAlta = DateTime.UtcNow
            };

            return await _repository.AddAsync(automovil);
        }

        public async Task UpdateAsync(int id, AutomovilUpdateDto dto)
        {
            var automovil = await _repository.GetByIdAsync(id);
            if (automovil == null)
            {
                throw new KeyNotFoundException($"Automóvil con ID {id} no encontrado");
            }

            // Aplicar cambios solo si los valores no son null
            if (dto.Marca != null)
                automovil.Marca = dto.Marca.Trim().ToUpperInvariant();
            
            if (dto.Modelo != null)
                automovil.Modelo = dto.Modelo.Trim().ToUpperInvariant();
           
            if (dto.Anio.HasValue)
                automovil.Anio = dto.Anio.Value;
            
            if (dto.Color != null)
                automovil.Color = dto.Color.Trim();

            // NumeroChasis (VIN) NO se puede actualizar - eliminado completamente

            // Actualizar NumeroMotor si se proporciona
            if (!string.IsNullOrEmpty(dto.NumeroMotor))
            {
                var nuevoMotor = _motorService.Validate(dto.NumeroMotor);
                
                // Verificar unicidad (excluyendo el automóvil actual)
                var existeMotor = await _repository.ExistsByMotorNumberAsync(nuevoMotor, id);
                if (existeMotor)
                {
                    throw new InvalidOperationException($"Ya existe un automóvil con el número de motor {nuevoMotor}");
                }
                
                automovil.NumeroMotor = nuevoMotor;
            }

            await _repository.UpdateAsync(automovil);
        }

        public async Task DeleteAsync(int id)
        {
            var automovil = await _repository.GetByIdAsync(id);
            if (automovil == null)
            {
                throw new KeyNotFoundException($"Automóvil con ID {id} no encontrado");
            }

            await _repository.DeleteAsync(automovil);
        }

        public async Task<AutomovilReadDto?> GetByIdAsync(int id)
        {
            var automovil = await _repository.GetByIdAsync(id);
            if (automovil == null)
                return null;

            return MapToReadDto(automovil);
        }

        public async Task<AutomovilReadDto?> GetByChasisAsync(string numeroChasis)
        {
            // Normalizar y validar el número de chasis (VIN)
            var chasis = _numeroChasisService.Validate(numeroChasis);
            
            var automovil = await _repository.GetByVinAsync(chasis.Value);
            
            if (automovil == null)
                return null;

            return MapToReadDto(automovil);
        }

        public async Task<IReadOnlyList<AutomovilReadDto>> GetAllAsync()
        {
            var automoviles = await _repository.GetAllAsync();
            return automoviles.Select(MapToReadDto).ToList();
        }

        private static AutomovilReadDto MapToReadDto(Domain.Entities.Automovil automovil)
        {
            return new AutomovilReadDto
            {
                Id = automovil.Id,
                Marca = automovil.Marca,
                Modelo = automovil.Modelo,
                Anio = automovil.Anio,
                Color = automovil.Color,
                NumeroChasis = automovil.NumeroChasis.Value,
                NumeroMotor = automovil.NumeroMotor,
                FechaAlta = automovil.FechaAlta
            };
        }
    }
}