using RfidBarcode.Application.Common.Interfaces;
using RfidBarcode.Domain.Entities;
using RfidBarcode.Domain.Models.Views;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace RFIDTracking.Pages.Shared.Components.SidebarMenu
{
    public class SidebarMenuViewComponent : ViewComponent
    {
        public class MenuGroup
        {
            public string? Title { get; set; }
            public List<MenuItem> MenuItems { get; set; } = new List<MenuItem>();
        }
        public class Model
        {
            public List<MenuGroup> MenuGroups { get; set; } = null!;
            public string BasePath { get; set; } = null!;
        }

        private readonly IUserResolverService _user;

        public Model Param { get; set; } = null!;

        public SidebarMenuViewComponent(IUserResolverService user, IConfiguration config)
        {
            _user = user;
            Param = new Model()
            {
                MenuGroups = new List<MenuGroup>()
            };
        }

        public async Task<IViewComponentResult> InvokeAsync(string path)
        {
            var groupHome = new MenuGroup();

            var menuHome = new MenuItem()
            {
                Label = "Dashboard",
                Icon = "pie-chart",
                Href = "/"
            };
            groupHome.MenuItems.Add(menuHome);
            Param.MenuGroups.Add(groupHome);

            #region Operasional
            var groupOps = new MenuGroup
            {
                Title = "Operasional"
            };

            //var menuTracking = new MenuItem
            //{
            //    Label = "Tracking",
            //    Icon = "send",
            //    Href = "/Trackings/"
            //};
            //groupOps.MenuItems.Add(menuTracking);


            var menuFinish = new MenuItem() { Label = "Finish", Href = "/Finish/", Icon = "check-circle" };
            groupOps.MenuItems.Add(menuFinish);
            var menuStockOpname = new MenuItem() { Label = "Stock Opname", Href = "/StockOpnames/", Icon = "check-square" };
            groupOps.MenuItems.Add(menuStockOpname);
            var menuSuratJalan = new MenuItem() { Label = "Surat Jalan", Href = "suratJalans", Icon = "file" };
            var menuSuratInbound = new MenuItem() { Label = "Inbound", Href = "/Inbounds/", Icon = "file" };
            var menuSuratOutbound = new MenuItem() { Label = "Outbound", Href = "/SuratJalanP1s/", Icon = "file" };
            menuSuratJalan.ChildMenuItems.Add(menuSuratInbound);
            menuSuratJalan.ChildMenuItems.Add(menuSuratOutbound);
            groupOps.MenuItems.Add(menuSuratJalan);

            Param.MenuGroups.Add(groupOps);
            #endregion

            #region Laporan
            var groupReport = new MenuGroup
            {
                Title = "Laporan"
            };
            var menuReportReceived = new MenuItem()
            {
                Label = "Laporan Stok",
                Icon = "check-square",
                Href = "/Reports/Stock/"
            };
            groupReport.MenuItems.Add(menuReportReceived);

            groupReport.MenuItems.Add(new MenuItem() { Label = "Laporan Harian", Icon="file-text", Href = "/Reports/DailyReport/" });
            Param.MenuGroups.Add(groupReport);
            #endregion

            #region Setting
            var groupSetting = new MenuGroup
            {
                Title = "Pengaturan"
            };

            #region User Setting
            var menuSetting = new MenuItem
            {
                Label = "Pengguna",
                Icon = "users",
                Href = "settings"
            };
            if (_user.HasReadAccess(AccessMenu.UserManagement))
            {
                var menuUsers = new MenuItem() { Label = "Manajemen Pengguna", Href = "/Settings/Users/" };
                menuSetting.ChildMenuItems.Add(menuUsers);
            }

            var menuProfile = new MenuItem() { Label = "Ubah Password", Href = "/Settings/Password/" };
            menuSetting.ChildMenuItems.Add(menuProfile);

            groupSetting.MenuItems.Add(menuSetting);
            #endregion

            #region Map Setting

            var menuMap = new MenuItem
            {
                Label = "Denah Lokasi",
                Icon = "box",
                Href = "nav-map"
            };
            menuMap.ChildMenuItems.Add(new MenuItem() { Label = "Ruangan", Href = "/Settings/Locations/" });
            menuMap.ChildMenuItems.Add(new MenuItem() { Label = "RFID Gate", Href = "/Settings/Gates/" });
            groupSetting.MenuItems.Add(menuMap);

            var menuMaster = new MenuItem
            {
                Label = "Master Data",
                Icon = "box",
                Href = "nav-master"
            };
            menuMaster.ChildMenuItems.Add(new MenuItem() { Label = "Tipe Surat Jalan", Href = "/Settings/SuratJalans/" });
            groupSetting.MenuItems.Add(menuMaster);
            #endregion

            Param.MenuGroups.Add(groupSetting);
            #endregion

            

            return await Task.FromResult((IViewComponentResult)View(Param));
        }

    }
}
