using Microsoft.EntityFrameworkCore;
using NBD4.Models;

namespace NBD4.Data
{
    public class NBDContext : DbContext
    {
		//To give access to IHttpContextAccessor for Audit Data with IAuditable
		private readonly IHttpContextAccessor _httpContextAccessor;

		//Property to hold the UserName value
		public string UserName
		{
			get; private set;
		}


		public NBDContext(DbContextOptions<NBDContext> options, IHttpContextAccessor httpContextAccessor)
			: base(options)
		{
			_httpContextAccessor = httpContextAccessor;
			if (_httpContextAccessor.HttpContext != null)
			{
				//We have a HttpContext, but there might not be anyone Authenticated
				UserName = _httpContextAccessor.HttpContext?.User.Identity.Name;
				UserName ??= "Unknown";
			}
			else
			{
				//No HttpContext so seeding data
				UserName = "Seed Data";
			}
		}


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

		public DbSet<Employee> Employees { get; set; }
		public DbSet<Subscription> Subscriptions { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
			//Add a unique index to the Employee Email
			modelBuilder.Entity<Employee>()
			.HasIndex(a => new { a.Email })
			.IsUnique();

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
		public override int SaveChanges(bool acceptAllChangesOnSuccess)
		{
			OnBeforeSaving();
			return base.SaveChanges(acceptAllChangesOnSuccess);
		}

		public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken))
		{
			OnBeforeSaving();
			return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
		}

		private void OnBeforeSaving()
		{
			var entries = ChangeTracker.Entries();
			foreach (var entry in entries)
			{
				if (entry.Entity is IAuditable trackable)
				{
					var now = DateTime.UtcNow;
					switch (entry.State)
					{
						case EntityState.Modified:
							trackable.UpdatedOn = now;
							trackable.UpdatedBy = UserName;
							break;

						case EntityState.Added:
							trackable.CreatedOn = now;
							trackable.CreatedBy = UserName;
							trackable.UpdatedOn = now;
							trackable.UpdatedBy = UserName;
							break;
					}
				}
			}
		}


	}
}
