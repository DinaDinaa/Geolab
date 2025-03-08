using QuizPlatform.Domain.Models;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuizPlatform.Domain.Models.Entities;

public class AnswerConfiguration : IEntityTypeConfiguration<Answer>
{
    public void Configure(EntityTypeBuilder<Answer> builder)
    {
        // Specify the table name (optional, defaults to the class name)
        builder.ToTable("Answers");

        // Primary key
        builder.HasKey(a => a.AnswerId);

        // Properties
        builder.Property(a => a.AnswerText)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(a=>a.QuestionId)
            .IsRequired();  

    }

}