using Microsoft.EntityFrameworkCore;
using NBD4.Models;

namespace NBD4.Data
{
    public class NBDContext : DbContext
    {
        public NBDContext(DbContextOptions<NBDContext>options)
        :base(options)
        {

        }

        public DbSet<Client> Clients { get; set; }
        public DbSet<Project>Projects { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Client>()
                .HasMany<Project>(c => c.Projects)
                .WithOne(p => p.Client)
                .HasForeignKey(p => p.ClientID)
                .OnDelete(DeleteBehavior.Restrict);
        }

    }
}
