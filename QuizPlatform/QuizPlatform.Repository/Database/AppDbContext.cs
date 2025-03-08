using QuizPlatform.Repository.EntityConfigurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using QuizPlatform.Domain.Models.Entities;
using QuizPlatform.Identity.Models;


namespace QuizPlatform.Repository.Database;

public sealed class AppDbContext : IdentityDbContext<ApplicationUser>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Answer> Answers { get; set; }
    public DbSet<Question> Questions { get; set; }
    public DbSet<Test> Tests { get; set; }
    public DbSet<Quiz> Quizzes { get; set; }
    public DbSet<QuizResult> QuizResults { get; set; }

    //public DbSet<QuizUser> QuizUsers{ get; set; }
    public DbSet<QuizTestLink> QuizzesTestsLink{ get; set; }
    public DbSet<Config> Configs{ get; set; }



    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfiguration(new AnswerConfiguration());
        modelBuilder.ApplyConfiguration(new QuestionConfiguration());
        modelBuilder.ApplyConfiguration(new TestConfiguration());
        modelBuilder.ApplyConfiguration(new QuizConfiguration());
        modelBuilder.ApplyConfiguration(new QuizTestLinkConfiguration());
        modelBuilder.ApplyConfiguration(new ConfigConfiguration());


        modelBuilder.Entity<ApplicationUser>()
            .Property(p => p.Id)
            .HasColumnOrder(0);
        modelBuilder.Entity<ApplicationUser>()
                .Property(p => p.UserName)
                .HasColumnOrder(1);    
        modelBuilder.Entity<ApplicationUser>()
                .Property(p => p.IsActive)
                .HasColumnOrder(2);
            
            

    }
}