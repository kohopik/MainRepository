using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace IndieProjects.Model
{
    public class IndieContext : IdentityDbContext<User>
    {
        public DbSet<Article> Articles { get; set; }

        public DbSet<ChatMessage> ChatMessages { get; set; }

        public DbSet<ArticleCommentaries> ArticleCommentaries { get; set; }

        public DbSet<Like> Likes { get; set; }

        public DbSet<Tag> Tags { get; set; }

        public DbSet<ProjectCommentaries> ProjectCommentaries { get; set; }

        public DbSet<Message> Messages { get; set; }

        public DbSet<Project> Projects { get; set; }

        public DbSet<Summary> Summaries { get; set; }

        public DbSet<Vakanci> Vakancies { get; set; }

        public IndieContext(DbContextOptions<IndieContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //Проект - пользователь
            modelBuilder.Entity<DeveloperProject>()
            .HasKey(t => new { t.UserId, t.ProjectID });

            modelBuilder.Entity<DeveloperProject>()
                .HasOne(pt => pt.User)
                .WithMany(p => p.Projects)
                .HasForeignKey(pt => pt.UserId);

            modelBuilder.Entity<DeveloperProject>()
                .HasOne(pt => pt.Project)
                .WithMany(t => t.Team)
                .HasForeignKey(pt => pt.ProjectID);
        }
    }
}
