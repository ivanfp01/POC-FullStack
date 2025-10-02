using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Repositories.Sql.Automoviles.Configurations
{
    public class AutomovilConfiguration : IEntityTypeConfiguration<Automovil>
    {
        public void Configure(EntityTypeBuilder<Automovil> builder)
        {
            builder.ToTable("Automovil");
            builder.HasKey(a => a.Id);

            builder.Property(a => a.Marca).IsRequired().HasMaxLength(60);
            builder.Property(a => a.Modelo).IsRequired().HasMaxLength(60);
            builder.Property(a => a.Año)
                   .HasColumnName("Año")
                   .IsRequired();
            builder.Property(a => a.Color).HasMaxLength(30);

            // Mapeo del Value Object NumeroChasisVo - configuración más explícita
            builder.OwnsOne(a => a.NumeroChasis, ch =>
            {
                ch.Property(p => p.Value)
                   .HasColumnName("NumeroChasis")
                   .HasMaxLength(17)
                   .IsRequired();
                
                // Índice único en la columna
                ch.HasIndex(p => p.Value)
                   .HasDatabaseName("IX_Automovil_NumeroChasis")
                   .IsUnique();
            });

            // Configurar que el owned type es requerido
            builder.Navigation(a => a.NumeroChasis).IsRequired();

            builder.Property(a => a.NumeroMotor).IsRequired().HasMaxLength(20);
            builder.Property(a => a.FechaAlta).IsRequired();

            // Índice único para NumeroMotor
            builder.HasIndex(a => a.NumeroMotor)
                   .HasDatabaseName("IX_Automovil_NumeroMotor")
                   .IsUnique();
        }
    }
}