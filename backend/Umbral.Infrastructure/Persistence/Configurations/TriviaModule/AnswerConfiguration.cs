using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Umbral.Domain.TriviaModule.Entities;

namespace Umbral.Infrastructure.Persistence.Configurations.TriviaModule;

public class AnswerConfiguration : IEntityTypeConfiguration<Answer>
{
    public void Configure(EntityTypeBuilder<Answer> builder)
    {
        builder.ToTable("Answers");

        builder.HasKey(a => a.Id);

        builder.Property(a => a.SelectedOption)
            .IsRequired();

        builder.Property(a => a.TimeElapsedMs)
            .IsRequired();
    }
}