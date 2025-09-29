using Application.Services;
using Domain.Entities;
using Domain.ValueObjects;

namespace Domain.Tests
{
    /// <summary>
    /// Clase simple para probar la funcionalidad del Value Object Vin y servicios
    /// </summary>
    public static class AutomovilTestHelper
    {
        public static void TestVinCreation()
        {
            // Test VIN válido
            try
            {
                var vin = Vin.Create("1HGBH41JXMN109186");
                Console.WriteLine($"VIN válido creado: {vin}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creando VIN: {ex.Message}");
            }
        }

        public static void TestServices(IVinService vinService, IMotorService motorService)
        {
            try
            {
                // Generar VIN
                var generatedVin = vinService.Generate("TOYOTA", 2024, "SUV");
                Console.WriteLine($"VIN generado: {generatedVin}");

                // Generar número de motor
                var generatedMotor = motorService.Generate("TOYOTA", 2024);
                Console.WriteLine($"Número de motor generado: {generatedMotor}");

                // Crear automóvil de prueba
                var automovil = new Automovil
                {
                    Marca = "TOYOTA",
                    Modelo = "RAV4",
                    Tipo = "SUV",
                    Anio = 2024,
                    Color = "Rojo",
                    NumeroChasis = generatedVin,
                    NumeroMotor = generatedMotor,
                    FechaAlta = DateTime.UtcNow
                };

                Console.WriteLine($"Automóvil creado: {automovil.Marca} {automovil.Modelo} - VIN: {automovil.NumeroChasis}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en test de servicios: {ex.Message}");
            }
        }
    }
}