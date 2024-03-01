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
		public DbSet<Project> Projects { get; set; }
		public DbSet<Province> Provinces { get; set; }
		public DbSet<City> Cities { get; set; }

		public DbSet<LabourTypeInfo> LabourTypeInfos { get; set; }
		public DbSet<BidLabourTypeInfo> BidLabourTypeInfos { get; set; }

		public DbSet<MaterialType> MaterialTypes { get; set; }
		public DbSet<Inventory> Inventories { get; set; }
		public DbSet<BidInventory> BidInventories { get; set; }

		public DbSet<Bid> Bids { get; set; }

		public DbSet<StaffRole> StaffRoles { get; set; }

		public DbSet<Staff> Staffs { get; set; }

		public DbSet<BidStaff> BidsStaffs { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
			modelBuilder.Entity<BidStaff>()
				.HasKey(t => new { t.StaffID, t.BidID });

			modelBuilder.Entity<BidInventory>()
				.HasKey(t => new { t.BidID,t.InventoryID });

			modelBuilder.Entity<BidLabourTypeInfo>()
				.HasKey(t => new { t.LabourTypeInfoID, t.BidID });

			modelBuilder.Entity<Client>()
				.HasMany<Project>(c => c.Projects)
				.WithOne(p => p.Client)
				.HasForeignKey(p => p.ClientID)
				.OnDelete(DeleteBehavior.Restrict);


			modelBuilder.Entity<City>()
			.HasIndex(c => new { c.Name, c.ProvinceID })
			.IsUnique();


			modelBuilder.Entity<Province>()
				.HasMany<City>(d => d.Cities)
				.WithOne(p => p.Province)
				.HasForeignKey(p => p.ProvinceID)
				.OnDelete(DeleteBehavior.Restrict);

			modelBuilder.Entity<LabourTypeInfo>()
			   .HasMany<BidLabourTypeInfo>(d => d.BidLabourTypeInfos)
			   .WithOne(p => p.LabourTypeInfo)
			   .HasForeignKey(p => p.LabourTypeInfoID)
			   .OnDelete(DeleteBehavior.Restrict);


			modelBuilder.Entity<Inventory>()
			  .HasMany<BidInventory>(d => d.BidInventories)
			  .WithOne(p => p.Inventory)
			  .HasForeignKey(p => p.InventoryID)
			  .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<Staff>()
              .HasMany<BidStaff>(d => d.BidStaffs)
              .WithOne(p => p.Staff)
              .HasForeignKey(p => p.StaffID)
              .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<MaterialType>()
			  .HasMany<Inventory>(d => d.Inventories)
			  .WithOne(p => p.MaterialType)
			  .HasForeignKey(p => p.MaterialTypeID)
			  .OnDelete(DeleteBehavior.Restrict);

			modelBuilder.Entity<StaffRole>()
			 .HasMany<Staff>(d => d.Staffs)
			 .WithOne(p => p.StaffRole)
			 .HasForeignKey(p => p.StaffRoleID)
			 .OnDelete(DeleteBehavior.Restrict);



		}

    }
}
