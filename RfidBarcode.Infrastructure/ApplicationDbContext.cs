using RfidBarcode.Application.Common.Interfaces;
using RfidBarcode.Domain.Common;
using RfidBarcode.Domain.Entities;
using RfidBarcode.Domain.Entities.Identities;
using RfidBarcode.Infrastructure.Configurations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace RfidBarcode.Infrastructure
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Int64,
        IdentityUserClaim<Int64>, ApplicationUserRole, IdentityUserLogin<Int64>,
        IdentityRoleClaim<Int64>, IdentityUserToken<Int64>>, IApplicationDbContext
    {
        private readonly IUserResolverService _userService;

        public DbSet<AccessMenu> AccessMenus { get; set; } = null!;
        public DbSet<AccessMenuRole> AccessMenuRoles { get; set; } = null!;
        public DbSet<Status> Status { get; set; } = null!;

        public DbSet<TrackingItem> TrackingItems { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<ImportItemLog> ImportItemLogs { get; set; }
        public DbSet<ItemPrintLog> ItemPrintLogs { get; set; }
        public DbSet<ItemMovement> ItemMovements { get; set; }

        public DbSet<Location> Locations { get; set; }
        public DbSet<Gate> Gates { get; set; }
        public DbSet<GateMap> GateMaps { get; set; }
        public DbSet<TagLocation> TagLocations { get; set; }

        public DbSet<StockOpname> StockOpnames { get; set; }
        public DbSet<StockOpnameDetail> StockOpnameDetails { get; set; }

        public DbSet<SuratJalan> SuratJalans { get; set; }
        public DbSet<SuratJalanType> SuratJalanTypes { get; set; }
        public DbSet<DailyReport> DailyReports { get; set; }


        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IUserResolverService userService)
            : base(options)
        {
            _userService = userService;
        }

        public DatabaseFacade Db { get { return this.Database; } }

        public string GetConnectionString()
        {
            return Database.GetDbConnection().ConnectionString;
        }

        public void Refresh(object entity)
        {
            var entry = Entry(entity);
            entry.Reload();
        }

        private void UpdateBaseEntity()
        {

            var user = _userService.GetUser();

            var newEntities = this.ChangeTracker.Entries()
                .Where(
                    x => x.State == EntityState.Added &&
                    x.Entity != null &&
                    x.Entity as IBaseEntity != null
                    )
                .Select(x => x.Entity as IBaseEntity);

            var modifiedEntities = this.ChangeTracker.Entries()
                .Where(
                    x => x.State == EntityState.Modified &&
                    x.Entity != null &&
                    x.Entity as IBaseEntity != null
                    )
                .Select(x => x.Entity as IBaseEntity);

            foreach (var newEntity in newEntities)
            {
                if (newEntity != null)
                {
                    newEntity.CreatedDate = DateTime.Now;
                    newEntity.LastUpdateDate = DateTime.Now;
                    newEntity.CreatedBy = user;
                    newEntity.LastUpdateBy = user;
                }
            }

            foreach (var modifiedEntity in modifiedEntities)
            {
                if (modifiedEntity != null)
                {
                    modifiedEntity.LastUpdateDate = DateTime.Now;
                    modifiedEntity.LastUpdateBy = user;
                }
            }
        }

        public override int SaveChanges()
        {
            UpdateBaseEntity();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            UpdateBaseEntity();
            return base.SaveChangesAsync(cancellationToken);
        }

        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.Properties<decimal>()
                .HavePrecision(18, 2);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new ApplicationRoleConfiguration());
            modelBuilder.ApplyConfiguration(new ApplicationUserConfiguration());
            modelBuilder.ApplyConfiguration(new ApplicationUserRoleConfiguration());
            modelBuilder.ApplyConfiguration(new AccessMenuConfiguration());
            modelBuilder.ApplyConfiguration(new AccessMenuRoleConfiguration());
            modelBuilder.ApplyConfiguration(new ItemConfiguration());
            modelBuilder.ApplyConfiguration(new TrackingItemConfiguration());
            modelBuilder.ApplyConfiguration(new GateConfiguration());
            modelBuilder.ApplyConfiguration(new GateMapConfiguration());
            modelBuilder.ApplyConfiguration(new LocationConfiguration());
            modelBuilder.ApplyConfiguration(new SuratJalanP1Configuration());
            modelBuilder.ApplyConfiguration(new ItemMovementConfiguration());
        }
    }
}
