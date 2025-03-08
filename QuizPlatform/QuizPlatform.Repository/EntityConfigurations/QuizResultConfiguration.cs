using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuizPlatform.Domain.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizPlatform.Repository.EntityConfigurations
{
    public class QuizResultConfiguration : IEntityTypeConfiguration<QuizResult>
    {
        public void Configure(EntityTypeBuilder<QuizResult> builder)
        {
            // Specify the table name (optional, defaults to the class name)
            builder.ToTable("QuizResults");

            // Primary key
            builder.HasKey(q => q.QuizResultId);

            //builder.HasOne(qr=>qr.User)
            //       .WithMany(u=>u.QuizResults)
            //       .OnDelete(DeleteBehavior.SetNull);



        }
    }
}