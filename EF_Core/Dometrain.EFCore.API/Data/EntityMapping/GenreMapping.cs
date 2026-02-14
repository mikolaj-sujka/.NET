using Dometrain.EFCore.API.Data.ValueGenerators;
using Dometrain.EFCore.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dometrain.EFCore.API.Data.EntityMapping;

public class GenreMapping : IEntityTypeConfiguration<Genre>
{
    public void Configure(EntityTypeBuilder<Genre> builder)
    {
        builder.Property<DateTime>("CreatedDate")
            .HasColumnName("CreatedAt")
            .HasValueGenerator<CreatedDateGenerator>();

        builder.Property(g => g.Name)
            .HasMaxLength(256)
            .HasColumnType("varchar");

        // unique constraint -> can be target of a foreign key, but not the primary key
        builder.HasAlternateKey(x => x.Name);

        builder.Property(g => g.ConcurrencyToken)
            .IsRowVersion() // concurrency token -> used for optimistic concurrency control, automatically updated by the database on each update
            .HasColumnType("rowversion");

        builder
            .Property<bool>("Deleted")
            .HasDefaultValue(false);

        builder // global query filter -> applied to all queries, but not to raw SQL queries
            .HasQueryFilter(g => EF.Property<bool>(g, "Deleted") == false)
            .HasAlternateKey(g => g.Name);
    }
}