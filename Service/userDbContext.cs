using Microsoft.EntityFrameworkCore;

using SmartQuiz_APP.Models;

namespace SmartQuiz_APP.Service
{
    public class userDbContext : DbContext
    {
        public userDbContext(DbContextOptions<userDbContext> options) : base(options) 
        {
            
        }

        public DbSet<login_signup> login_signup { get; set; }

        public DbSet<QuizAttempt> QuizAttempts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure the relationship here:
            modelBuilder.Entity<QuizAttempt>()
                .HasOne(qa => qa.User) // A QuizAttempt has one User
                .WithMany() // A User can have many QuizAttempts
                .HasForeignKey(qa => qa.UserId) // The foreign key in QuizAttempt is UserId
                .OnDelete(DeleteBehavior.Cascade); // If a User is deleted, their QuizAttempts are also deleted
        }

    }
}
