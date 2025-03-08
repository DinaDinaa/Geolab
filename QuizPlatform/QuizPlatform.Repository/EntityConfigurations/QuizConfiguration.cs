
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using QuizPlatform.Domain.Models.Entities ;

namespace QuizPlatform.Repository.EntityConfigurations
{
    public class QuizConfiguration : IEntityTypeConfiguration<Quiz>
    {
        public void Configure(EntityTypeBuilder<Quiz> builder)
        {
            // Specify the table name (optional, defaults to the class name)
            builder.ToTable("Quizzes");

            // Primary key
            builder.HasKey(a => a.QuizId);

            // Properties
            builder.Property(a => a.QuizName)
                .IsRequired()
                .HasMaxLength(40);

            builder.Property(a => a.QuizTime)
                .IsRequired();

            builder.HasMany(e => e.QuizzesTests)
               .WithOne(x => x.Quiz)
              .HasForeignKey(e => e.QuizId)
              .HasPrincipalKey(e => e.QuizId)
            .OnDelete(DeleteBehavior.NoAction);



        }

    }
}