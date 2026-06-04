using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Umbral.Domain.TriviaModule.Entities;

namespace Umbral.Infrastructure.Persistence.Configurations.TriviaModule;

public class GameRoundConfiguration : IEntityTypeConfiguration<GameRound>
{
    public void Configure(EntityTypeBuilder<GameRound> builder)
    {
        builder.ToTable("GameRounds");

        builder.HasKey(g => g.Id);

        builder.Property(g => g.RoundNumber)
            .IsRequired();

        builder.HasMany(g => g.Answers)
            .WithOne()
            .HasForeignKey(a => a.RoundId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}