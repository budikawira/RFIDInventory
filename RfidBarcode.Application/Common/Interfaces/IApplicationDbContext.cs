using RfidBarcode.Domain.Entities;
using RfidBarcode.Domain.Entities.Identities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace RfidBarcode.Application.Common.Interfaces
{
    public interface IApplicationDbContext
    {
        public DbSet<ApplicationUser> Users { get; set; }
        public DbSet<ApplicationRole> Roles { get; set; }
        public DbSet<ApplicationUserRole> UserRoles { get; set; }
        public DbSet<AccessMenu> AccessMenus { get; set; }
        public DbSet<AccessMenuRole> AccessMenuRoles { get; set; }
        public DbSet<Status> Status { get; set; }

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

        public DbSet<SuratJalanP1> SuratJalanP1s { get; set; }
        public DbSet<DailyReport> DailyReports { get; set; }

        public DatabaseFacade Db { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);

        public void Refresh(object entity);

        public string GetConnectionString();
    }
}
