using QuizPlatform.Domain.Models;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuizPlatform.Domain.Models.Entities;
namespace QuizPlatform.Repository.EntityConfigurations
{
    public class QuestionConfiguration : IEntityTypeConfiguration<Question>
    {
        public void Configure(EntityTypeBuilder<Question> builder)
        {
            // Specify the table name (optional, defaults to the class name)
            builder.ToTable("Questions");

            // Primary key
            builder.HasKey(a => a.QuestionId);

            // Properties
            builder.Property(a => a.QuestionText)
                .IsRequired()
                .HasMaxLength(200);

            builder.HasMany(e => e.Answers)
               .WithOne(e => e.Question)
               .HasForeignKey(e => e.QuestionId)
               .HasPrincipalKey(e => e.QuestionId)
               .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
