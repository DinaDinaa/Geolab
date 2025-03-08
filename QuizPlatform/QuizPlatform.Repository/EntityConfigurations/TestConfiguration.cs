using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuizPlatform.Domain.Models.Entities;

namespace QuizPlatform.Repository.EntityConfigurations
{
    public class TestConfiguration : IEntityTypeConfiguration<Test>
    {
        public void Configure(EntityTypeBuilder<Test> builder)
        {
            // Specify the table name (optional, defaults to the class name)
            builder.ToTable("Tests");

            // Primary key
            builder.HasKey(a => a.TestId);

            // Properties
            builder.Property(a => a.TestName)
                .IsRequired()
                .HasMaxLength(40);

            builder.Property(a => a.TestTime)
                .IsRequired();
            builder.HasMany(t => t.Questions)
                   .WithOne(q => q.Test)
                   .IsRequired();

            builder.HasMany(e => e.QuizzesTests)
                  .WithOne(e => e.Test)
                  .HasForeignKey(e => e.QuizId)
                  .HasPrincipalKey(e => e.TestId)
                  .OnDelete(DeleteBehavior.Cascade);


        }

    }
}
