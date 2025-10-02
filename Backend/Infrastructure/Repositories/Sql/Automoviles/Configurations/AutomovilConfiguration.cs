using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Repositories.Sql.Automoviles.Configurations
{
    public class AutomovilConfiguration : IEntityTypeConfiguration<Automovil>
    {
        public void Configure(EntityTypeBuilder<Automovil> builder)
        {
            builder.ToTable("Automovil", t =>
            {
                t.HasCheckConstraint("CK_Automovil_Año_Rango",
                    $"[Año] >= 1900 AND [Año] <= (YEAR(GETDATE()) + 1)");

                t.HasCheckConstraint("CK_Automovil_Marca_NotBlank",
                    "LEN(LTRIM(RTRIM([Marca]))) > 0");

                t.HasCheckConstraint("CK_Automovil_Modelo_NotBlank",
                    "LEN(LTRIM(RTRIM([Modelo]))) > 0");

                t.HasCheckConstraint("CK_Automovil_Color_NotBlank",
                    "LEN(LTRIM(RTRIM([Color]))) > 0");
            });
            
            builder.HasKey(a => a.Id);

            builder.Property(a => a.Marca).IsRequired().HasMaxLength(60);
            builder.Property(a => a.Modelo).IsRequired().HasMaxLength(60);
            builder.Property(a => a.Año)
                   .HasColumnName("Año")
                   .IsRequired();
            builder.Property(a => a.Color)
                   .HasMaxLength(30)
                   .IsRequired();

            builder.OwnsOne(a => a.NumeroChasis, ch =>
            {
                ch.Property(p => p.Value)
                   .HasColumnName("NumeroChasis")
                   .HasMaxLength(17)
                   .IsRequired();
                
                ch.HasIndex(p => p.Value)
                   .HasDatabaseName("IX_Automovil_NumeroChasis")
                   .IsUnique();
            });

            builder.Navigation(a => a.NumeroChasis).IsRequired();

            builder.Property(a => a.NumeroMotor).IsRequired().HasMaxLength(20);
            builder.Property(a => a.FechaAlta).IsRequired();

            builder.HasIndex(a => a.NumeroMotor)
                   .HasDatabaseName("IX_Automovil_NumeroMotor")
                   .IsUnique();
        }
    }
}