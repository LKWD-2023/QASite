using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace QASite.Data
{
    public class QASiteContext : DbContext
    {
        private string _connectionString;

        public QASiteContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //by default Entity Framework sets all foreign key relationship delete rules
            //to be Cascade delete. This code changes it to be Restrict which is more in line
            //of what we're used to in that it will fail deleting a parent, if there are still
            //any children
            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }

            modelBuilder.Entity<QuestionsTags>()
                .HasKey(qt => new { qt.QuestionId, qt.TagId });

            modelBuilder.Entity<QuestionLike>()
                .HasKey(qt => new { qt.QuestionId, qt.UserId });

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Question> Questions { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<QuestionsTags> QuestionsTags { get; set; }
        public DbSet<Answer> Answers { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<QuestionLike> QuestionLikes { get; set; }
    }
}
