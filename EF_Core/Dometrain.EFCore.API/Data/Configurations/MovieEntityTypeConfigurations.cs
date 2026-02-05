using Dometrain.EFCore.API.Data.ValueConverters;
using Dometrain.EFCore.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dometrain.EFCore.API.Data.Configurations
{
    public class MovieEntityTypeConfigurations : IEntityTypeConfiguration<Movie>
    {
        public void Configure(EntityTypeBuilder<Movie> builder)
        {
            builder.ToTable("Movies")
                .HasQueryFilter(m => m.ReleaseDate >= new DateTime(2000, 01, 01));

            builder.HasKey(m => m.Id);

            builder.Property(m => m.Title)
                .HasColumnType("varchar")
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(m => m.ReleaseDate)
                .HasColumnType("char(8)")
                .HasConversion(new DateTimeToChar8Converter());

            builder.Property(m => m.Synopsis)
                .HasColumnType("text")
                .IsRequired(false)
                .HasColumnName("Plot");

            builder.Property(m => m.AgeRating)
                .HasConversion<string>()
                .HasColumnType("varchar(32)")
                .IsRequired();

            // Owned entity type configuration - Include is not needed when querying Movie
            builder.OwnsOne(m => m.Director) // Owned entity type configuration for Person as Director 
                .ToTable("Movies_Directors"); // Mapping owned type to separate table

            builder.OwnsMany(x => x.Actors)
                .ToTable("Movies_Actors") // Mapping owned type to separate table
                .WithOwner()
                .HasForeignKey("MovieId"); // FK in Movies_Actors table pointing to PK in Movies table

            builder.HasOne(m => m.Genre)
                .WithMany(genre => genre.Movies)
                .HasPrincipalKey(genre => genre.Id)
                .HasForeignKey(m => m.MainGenreId); // FK in Movie table pointing to PK in Genre table
            // .HasForeignKey<Movie>(m => m.MainGenreId); alternative syntax
        }
    }
}
