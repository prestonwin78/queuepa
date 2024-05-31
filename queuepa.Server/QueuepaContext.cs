using Microsoft.EntityFrameworkCore;
using queuepa.Server.models;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace queuepa.Server
{
    public class QueuepaContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Queue> Queues { get; set; }
        public DbSet<Song> Songs { get; set; }

        public QueuepaContext(DbContextOptions<QueuepaContext> options) : base(options)
        { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseSqlServer("YourConnectionStringHere");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasMany(u => u.Queues)
                .WithOne(q => q.Streamer)
                .HasForeignKey(q => q.StreamerId);

            modelBuilder.Entity<Queue>()
                .HasMany(q => q.Songs)
                .WithOne(s => s.Queue)
                .HasForeignKey(s => s.QueueId);

            modelBuilder.Entity<User>()
                .HasMany(u => u.Songs)
                .WithOne(s => s.AddedBy)
                .HasForeignKey(s => s.QueueId);
        }
    }

}
