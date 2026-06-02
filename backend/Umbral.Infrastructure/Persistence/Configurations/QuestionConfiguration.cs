using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Umbral.Domain.Entities;

namespace Umbral.Infrastructure.Persistence.Configurations;

public class QuestionConfiguration : IEntityTypeConfiguration<Question>
{
    public void Configure(EntityTypeBuilder<Question> builder)
    {
        builder.ToTable("Questions");
        
        builder.HasKey(q => q.Id);
        
        builder.Property(q => q.Text)
            .IsRequired()
            .HasMaxLength(500);
        
        builder.Property(q => q.Options)
            .HasConversion(
                v => string.Join('|', v),
                v => v.Split('|', StringSplitOptions.RemoveEmptyEntries))
            .IsRequired();
        
        builder.Property(q => q.CorrectOption)
            .IsRequired();
        
        builder.OwnsOne(q => q.Points, points =>
        {
            points.Property(p => p.Value)
                .HasColumnName("Points")
                .IsRequired();
        });
    }
}