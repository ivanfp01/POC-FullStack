using Application.DTOs.Automovil;
using Application.Repositories;
using Application.Services;
using Application.Json.Converters;
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
            var marca = StringSanitizer.NormalizeRequired(dto.Marca).ToUpperInvariant();
            var modelo = StringSanitizer.NormalizeRequired(dto.Modelo).ToUpperInvariant();
            var color = StringSanitizer.NormalizeRequired(dto.Color);
            var anio = dto.Año;
            
            if (anio < 1900 || anio > DateTime.Now.Year + 1)
                throw new ArgumentException($"Año inválido. Debe estar entre 1900 y {DateTime.Now.Year + 1}.");

            var chasisInput = StringSanitizer.NormalizeOrNull(dto.NumeroChasis);
            var motorInput = StringSanitizer.NormalizeOrNull(dto.NumeroMotor);

            var numeroChasis = chasisInput == null
                ? _numeroChasisService.Generate(marca, anio)
                : _numeroChasisService.Validate(chasisInput);

            if (chasisInput != null)
            {
                var existe = await _repository.ExistsByVinAsync(numeroChasis.Value);
                if (existe) throw new InvalidOperationException($"Ya existe un automóvil con el número de chasis {numeroChasis.Value}");
            }

            var numeroMotor = motorInput == null
                ? _motorService.Generate(marca, anio)
                : _motorService.Validate(motorInput);

            if (motorInput != null)
            {
                var existe = await _repository.ExistsByMotorNumberAsync(numeroMotor);
                if (existe) throw new InvalidOperationException($"Ya existe un automóvil con el número de motor {numeroMotor}");
            }

            var automovil = new Domain.Entities.Automovil
            {
                Marca = marca,
                Modelo = modelo,
                Año = anio,
                Color = color,
                NumeroChasis = numeroChasis,
                NumeroMotor = numeroMotor,
                FechaAlta = DateTime.Now
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

            if (dto.Color != null)
            {
                var color = StringSanitizer.NormalizeOrNull(dto.Color);
                if (color == null) throw new ArgumentException("Color no puede estar vacío");
                automovil.Color = color;
            }

            if (!string.IsNullOrEmpty(dto.NumeroMotor))
            {
                var motorInput = StringSanitizer.NormalizeOrNull(dto.NumeroMotor);
                if (motorInput == null) throw new ArgumentException("Número de motor no puede estar vacío");
                var nuevoMotor = _motorService.Validate(motorInput);

                var existeMotor = await _repository.ExistsByMotorNumberAsync(nuevoMotor, id);
                if (existeMotor) throw new InvalidOperationException($"Ya existe un automóvil con el número de motor {nuevoMotor}");

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
                Año = automovil.Año,
                Color = automovil.Color,
                NumeroChasis = automovil.NumeroChasis.Value,
                NumeroMotor = automovil.NumeroMotor,
                FechaAlta = automovil.FechaAlta
            };
        }
    }
}