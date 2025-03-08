
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuizPlatform.Domain.Models.Entities;

namespace QuizPlatform.Repository.EntityConfigurations
{
    public class QuizTestLinkConfiguration : IEntityTypeConfiguration<QuizTestLink>
    {
        public void Configure(EntityTypeBuilder<QuizTestLink> builder)
        {
            // Specify the table name (optional, defaults to the class name)
            builder.ToTable("QuizzesTestesLink");

            // Primary key
            builder.HasKey(l => l.Id);
            builder.HasIndex(l =>  new { l.QuizId , l.TestId}).IsUnique();

            builder.HasOne<Test>(l=>l.Test)
                   .WithMany(t=>t.QuizzesTests)
                   .HasForeignKey(a => a.TestId)
                   .OnDelete(DeleteBehavior.NoAction);
            builder.HasOne<Quiz>(l=>l.Quiz)
                  .WithMany(q=>q.QuizzesTests)
                  .HasForeignKey(a => a.QuizId)
                  .OnDelete(DeleteBehavior.NoAction);

        }
    }
}
