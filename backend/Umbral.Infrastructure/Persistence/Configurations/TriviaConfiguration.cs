using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Umbral.Domain.Entities;

namespace Umbral.Infrastructure.Persistence.Configurations;

public class TriviaConfiguration : IEntityTypeConfiguration<Trivia>
{
    public void Configure(EntityTypeBuilder<Trivia> builder)
    {
        builder.ToTable("Trivias");
        
        builder.HasKey(t => t.Id);
        
        builder.Property(t => t.Name)
            .IsRequired()
            .HasMaxLength(100);
        
        builder.Property(t => t.Description)
            .HasMaxLength(500);
        
        builder.Property(t => t.IsDeleted)
            .HasDefaultValue(false);
        
        builder.HasMany(t => t.Questions)
            .WithOne()
            .HasForeignKey(q => q.TriviaId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}