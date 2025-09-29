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
            builder.Property(a => a.Tipo).IsRequired().HasMaxLength(40);
            builder.Property(a => a.Anio).IsRequired();
            builder.Property(a => a.Color).HasMaxLength(30);
            builder.Property(a => a.NumeroChasis).IsRequired().HasMaxLength(17);
            builder.Property(a => a.NumeroMotor).IsRequired().HasMaxLength(20);
            builder.Property(a => a.FechaAlta).IsRequired();

            builder.HasIndex(a => a.NumeroChasis).IsUnique();
            builder.HasIndex(a => a.NumeroMotor).IsUnique();
        }
    }
}