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
            
            Param.MenuGroups.Add(groupOps);
            #endregion

            #region Setting
            var groupSetting = new MenuGroup
            {
                Title = "Settings"
            };

            #region User Setting
            var menuSetting = new MenuItem
            {
                Label = "Pengaturan Pengguna",
                Icon = "users",
                Href = "settings"
            };
            if (_user.HasReadAccess(AccessMenu.UserManagement))
            {
                var menuUsers = new MenuItem() { Label = "Manajemen Pengguna", Href = "/Settings/Users/" };
                menuSetting.ChildMenuItems.Add(menuUsers);
            }

            var menuProfile = new MenuItem() { Label = "Ganti Password", Href = "/Settings/Password/" };
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

            #endregion

            Param.MenuGroups.Add(groupSetting);
            #endregion

            

            return await Task.FromResult((IViewComponentResult)View(Param));
        }

    }
}
