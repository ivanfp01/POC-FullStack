using Application.DTOs.Automovil;
using Application.UseCases.Automovil;
using Microsoft.AspNetCore.Mvc;

namespace Template_API.Controllers
{
    /// <summary>
    /// Controlador para la gestión de automóviles
    /// </summary>
    [ApiController]
    [Route("api/v1/automovil")]
    [Produces("application/json")]
    public class AutomovilController : ControllerBase
    {
        private readonly IAutomovilService _automovilService;

        public AutomovilController(IAutomovilService automovilService)
        {
            _automovilService = automovilService ?? throw new ArgumentNullException(nameof(automovilService));
        }

        /// <summary>
        /// Crea un nuevo automóvil
        /// </summary>
        /// <param name="dto">Datos del automóvil a crear. El número de chasis (VIN) y número de motor se generan automáticamente si no se proporcionan.</param>
        /// <returns>ID del automóvil creado</returns>
        /// <example>
        /// Ejemplo sin número de chasis/motor (generación automática):
        /// {
        ///   "marca": "AUDI",
        ///   "modelo": "A4", 
        ///   "año": 2025,
        ///   "color": "BLANCO"
        /// }
        /// </example>
        [HttpPost]
        [ProducesResponseType(typeof(int), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Create([FromBody] AutomovilCreateDto dto)
        {
            try
            {
                var id = await _automovilService.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id }, new { Id = id });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new ProblemDetails
                {
                    Title = "Datos inválidos",
                    Detail = ex.Message,
                    Status = StatusCodes.Status400BadRequest
                });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new ProblemDetails
                {
                    Title = "Conflicto de unicidad",
                    Detail = ex.Message,
                    Status = StatusCodes.Status409Conflict
                });
            }
        }

        /// <summary>
        /// Actualiza un automóvil existente
        /// </summary>
        /// <param name="id">ID del automóvil</param>
        /// <param name="dto">Datos a actualizar. El número de chasis (VIN) NO se puede modificar.</param>
        /// <example>
        /// Ejemplo de actualización:
        /// { "color": "ROJO", "año": 2026 }
        /// </example>
        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Update(int id, [FromBody] AutomovilUpdateDto dto)
        {
            try
            {
                await _automovilService.UpdateAsync(id, dto);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new ProblemDetails
                {
                    Title = "Recurso no encontrado",
                    Detail = ex.Message,
                    Status = StatusCodes.Status404NotFound
                });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new ProblemDetails
                {
                    Title = "Datos inválidos",
                    Detail = ex.Message,
                    Status = StatusCodes.Status400BadRequest
                });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new ProblemDetails
                {
                    Title = "Conflicto de unicidad",
                    Detail = ex.Message,
                    Status = StatusCodes.Status409Conflict
                });
            }
        }

        /// <summary>
        /// Elimina un automóvil
        /// </summary>
        /// <param name="id">ID del automóvil</param>
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _automovilService.DeleteAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new ProblemDetails
                {
                    Title = "Recurso no encontrado",
                    Detail = ex.Message,
                    Status = StatusCodes.Status404NotFound
                });
            }
        }

        /// <summary>
        /// Obtiene un automóvil por su ID
        /// </summary>
        /// <param name="id">ID del automóvil</param>
        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(AutomovilReadDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            var automovil = await _automovilService.GetByIdAsync(id);
            if (automovil == null)
            {
                return NotFound(new ProblemDetails
                {
                    Title = "Recurso no encontrado",
                    Detail = $"Automóvil con ID {id} no encontrado",
                    Status = StatusCodes.Status404NotFound
                });
            }

            return Ok(automovil);
        }

        /// <summary>
        /// Obtiene un automóvil por su número de chasis (VIN)
        /// </summary>
        /// <param name="numeroChasis">Número de chasis (VIN)</param>
        [HttpGet("chasis/{numeroChasis}")]
        [ProducesResponseType(typeof(AutomovilReadDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetByChasis(string numeroChasis)
        {
            try
            {
                var automovil = await _automovilService.GetByChasisAsync(numeroChasis);
                if (automovil == null)
                {
                    return NotFound(new ProblemDetails
                    {
                        Title = "Recurso no encontrado",
                        Detail = $"Automóvil con número de chasis {numeroChasis} no encontrado",
                        Status = StatusCodes.Status404NotFound
                    });
                }

                return Ok(automovil);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new ProblemDetails
                {
                    Title = "Número de chasis (VIN) inválido",
                    Detail = ex.Message,
                    Status = StatusCodes.Status400BadRequest
                });
            }
        }

        /// <summary>
        /// Obtiene todos los automóviles
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(IReadOnlyList<AutomovilReadDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            var automoviles = await _automovilService.GetAllAsync();
            return Ok(automoviles);
        }
    }
}