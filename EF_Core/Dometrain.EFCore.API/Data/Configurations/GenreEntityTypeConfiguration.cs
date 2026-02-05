using Dometrain.EFCore.API.Data.ValueGenerators;
using Dometrain.EFCore.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dometrain.EFCore.API.Data.Configurations
{
    public class GenreEntityTypeConfiguration : IEntityTypeConfiguration<Genre>
    {
        public void Configure(EntityTypeBuilder<Genre> builder)
        {
            builder.ToTable("Genres");
            builder.HasKey(g => g.Id);
            builder.Property(g => g.Name)
                .HasColumnType("varchar")
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(x => x.CreatedDate)
                .HasColumnName("CreatedAt")
                .HasValueGenerator<CreatedDateGenerator>();

            // Seed
            builder.HasData(
                new Genre
                {
                    Id = 1,
                    Name = "Drama"
                },
                new Genre
                {
                    Id = 2,
                    Name = "Comedy"
                },
                new Genre
                {
                    Id = 3,
                    Name = "Action"
                });

        }
    }
}
