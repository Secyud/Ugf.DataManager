using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Blazorise;
using Ugf.DataManager.DataConfiguration;
using Ugf.DataManager.Localization;
using Volo.Abp.AspNetCore.Components.Web.Extensibility.EntityActions;
using Volo.Abp.AspNetCore.Components.Web.Extensibility.TableColumns;
using Volo.Abp.AspNetCore.Components.Web.Theming.PageToolbars;

namespace Ugf.DataManager.Blazor.Pages
{
    public partial class ConfigManagement
    {
        private Modal ItemModal { get; set; }
        private ConfigSetting Setting { get; set; }
        
        protected PageToolbar Toolbar { get; } = new();
        protected List<TableColumn> ConfigTableColumns => TableColumns.Get<ConfigManagement>();

        public ConfigManagement()
        {
            LocalizationResource = typeof(DataManagerResource);
        }

        protected override ValueTask SetBreadcrumbItemsAsync()
        {
            BreadcrumbItems.Add(new Volo.Abp.BlazoriseUI.BreadcrumbItem(L["Menu:Class"].Value));
            BreadcrumbItems.Add(new Volo.Abp.BlazoriseUI.BreadcrumbItem(L["DataConfig"].Value));
            return base.SetBreadcrumbItemsAsync();
        }

        protected override ValueTask SetEntityActionsAsync()
        {
            EntityActions
                .Get<ConfigManagement>()
                .AddRange(new EntityAction[]
                {
                    new()
                    {
                        Text = L["Edit"],
                        Clicked = async data => { await OpenEditModalAsync(data.As<DataConfigDto>()); }
                    },
                    new()
                    {
                        Text = L["Item"],
                        Clicked = async data => { await OpenItemModalAsync(data.As<DataConfigDto>()); }
                    },
                    new()
                    {
                        Text = L["Delete"],
                        Clicked = async data => await DeleteEntityAsync(data.As<DataConfigDto>()),
                        ConfirmationMessage = data => GetDeleteConfirmationMessage(data.As<DataConfigDto>())
                    },
                    new()
                    {
                        Text = L["Generate"],
                        Clicked = async data => await GenerateConfigAsync(data.As<DataConfigDto>()),
                    }
                });

            return base.SetEntityActionsAsync();
        }

        protected override ValueTask SetTableColumnsAsync()
        {
            ConfigTableColumns
                .AddRange(new TableColumn[]
                {
                    new()
                    {
                        Title = L["Actions"],
                        Actions = EntityActions.Get<ConfigManagement>(),
                    },
                    new()
                    {
                        Title = L["Name"],
                        Sortable = true,
                        Data = nameof(DataConfig.Name)
                    }
                });

            return base.SetTableColumnsAsync();
        }

        protected override ValueTask SetToolbarItemsAsync()
        {
            Toolbar.AddButton(L["NewObject"],
                OpenCreateModalAsync,
                IconName.Add);

            return base.SetToolbarItemsAsync();
        }

        private async Task GenerateConfigAsync(DataConfigDto dataConfig)
        {
            await AppService.GenerateDataAsync(dataConfig.Id);
        }

        private Guid ItemConfigId { get; set; }
        
        private async Task OpenItemModalAsync(DataConfigDto dto)
        {
            ItemConfigId = dto.Id;
            await Setting.GetConfigAsync(dto.Id);
            await ItemModal.Show();
        }
        private async Task CloseItemModalAsync()
        {
            await ItemModal.Hide();
            await InvokeAsync(StateHasChanged);
        }
    }
}