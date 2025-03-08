using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using QuizPlatform.Domain.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizPlatform.Repository.EntityConfigurations
{
    public class ConfigConfiguration : IEntityTypeConfiguration<Config>
    {
        public void Configure(EntityTypeBuilder<Config> builder)
        {
            // Specify the table name (optional, defaults to the class name)
            builder.ToTable("Configs");

            // Primary key
            builder.HasKey(c => c.ConfigId);

            // Properties
            builder.Property(a => a.Key)
                .IsRequired()
                .HasMaxLength(40);

            builder.HasIndex(a => a.Key).IsUnique();

        }

    }
}
