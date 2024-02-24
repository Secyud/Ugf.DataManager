using System.Collections.Generic;
using System.Threading.Tasks;
using Secyud.Ugf.DataManager.Localization;
using Secyud.Ugf.DataManager.MultiTenancy;
using Volo.Abp.Identity.Blazor;
using Volo.Abp.SettingManagement.Blazor.Menus;
using Volo.Abp.TenantManagement.Blazor.Navigation;
using Volo.Abp.UI.Navigation;

namespace Secyud.Ugf.DataManager.Blazor.Menus;

public class DataManagerMenuContributor : IMenuContributor
{
    public async Task ConfigureMenuAsync(MenuConfigurationContext context)
    {
        if (context.Menu.Name == StandardMenus.Main)
        {
            await ConfigureMainMenuAsync(context);
        }
    }

    private Task ConfigureMainMenuAsync(MenuConfigurationContext context)
    {
        var administration = context.Menu.GetAdministration();
        var l = context.GetLocalizer<DataManagerResource>();

        context.Menu.Items.InsertRange(
            0, new List<ApplicationMenuItem>
            {
                new ApplicationMenuItem(
                    DataManagerMenus.Home,
                    l["Menu:Home"],
                    "/",
                    icon: "fas fa-home",
                    order: 0
                ),
                new(
                    DataManagerMenus.DataObject,
                    l["Menu:DataObject"],
                    "/data-object",
                    groupName: DataManagerMenus.DataObject,
                    icon: "fas fa-database",
                    order: 1
                ),
                new(
                    DataManagerMenus.DataCollection,
                    l["Menu:DataCollection"],
                    "/data-collection",
                    groupName: DataManagerMenus.DataCollection,
                    icon: "fas fa-wrench",
                    order: 2
                ),
            }
        );

        if (MultiTenancyConsts.IsEnabled)
        {
            administration.SetSubItemOrder(TenantManagementMenuNames.GroupName, 1);
        }
        else
        {
            administration.TryRemoveMenuItem(TenantManagementMenuNames.GroupName);
        }

        administration.SetSubItemOrder(IdentityMenuNames.GroupName, 2);
        administration.SetSubItemOrder(SettingManagementMenus.GroupName, 3);

        return Task.CompletedTask;
    }
}