using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Umbral.Domain.TriviaModule.Entities;
using Umbral.Domain.TriviaModule.ValueObjects;

namespace Umbral.Infrastructure.Persistence.Configurations.TriviaModule;

public class SessionConfiguration : IEntityTypeConfiguration<Session>
{
    public void Configure(EntityTypeBuilder<Session> builder)
    {
        builder.ToTable("Sessions");

        builder.HasKey(s => s.Id);

        builder.Property(s => s.Code)
            .HasConversion(
                v => v.ToString(),
                v => SessionCode.Create(v))
            .IsRequired();

        builder.Property(s => s.TimePerQuestion)
            .IsRequired();

        builder.Property(s => s.Status)
            .HasConversion<int>()
            .IsRequired();

        builder.HasMany(s => s.Participants)
            .WithOne()
            .HasForeignKey(p => p.SessionId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}