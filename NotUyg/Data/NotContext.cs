
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NotUyg.Entity;

namespace NotUyg.Data
{
    public class NotContext : IdentityDbContext<User>
    {
        public NotContext(DbContextOptions<NotContext> options) : base(options)
        {

        }        
        public DbSet<Tag> Tag { get; set; }
        public DbSet<Not> Not { get; set; }
        public DbSet<Option> Option { get; set; }
        public DbSet<Poll> Poll { get; set; }
        public DbSet<UserVote> UserVote { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserVote>()
        .HasOne(uv => uv.User)
        .WithMany()
        .HasForeignKey(uv => uv.UserId)
        .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<UserVote>()
                .HasOne(uv => uv.Poll)
                .WithMany()
                .HasForeignKey(uv => uv.PollId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<UserVote>()
                .HasOne(uv => uv.Option)
                .WithMany()
                .HasForeignKey(uv => uv.OptionId)
                .OnDelete(DeleteBehavior.Restrict);



        }


    }
}
