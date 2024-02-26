using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Blazorise;
using Secyud.Ugf.DataManager.Localization;
using Volo.Abp.AspNetCore.Components.Web.Extensibility.EntityActions;
using Volo.Abp.AspNetCore.Components.Web.Extensibility.TableColumns;

using Volo.Abp.AspNetCore.Components.Web.Theming.PageToolbars;

namespace Secyud.Ugf.DataManager.Blazor.Pages
{
    public partial class DataCollectionManagement
    {
        private Modal ItemModal { get; set; }
        private DataCollectionObjectManagement Setting { get; set; }
        
        protected PageToolbar Toolbar { get; } = new();
        protected List<TableColumn> ConfigTableColumns => TableColumns.Get<DataCollectionManagement>();

        public DataCollectionManagement()
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
                .Get<DataCollectionManagement>()
                .AddRange(new EntityAction[]
                {
                    new()
                    {
                        Text = L["Edit"],
                        Clicked = async data => { await OpenEditModalAsync(data.As<DataCollectionDto>()); }
                    },
                    new()
                    {
                        Text = L["Item"],
                        Clicked = async data => { await OpenItemModalAsync(data.As<DataCollectionDto>()); }
                    },
                    new()
                    {
                        Text = L["Delete"],
                        Clicked = async data => await DeleteEntityAsync(data.As<DataCollectionDto>()),
                        ConfirmationMessage = data => GetDeleteConfirmationMessage(data.As<DataCollectionDto>())
                    },
                    new()
                    {
                        Text = L["Generate"],
                        Clicked = async data => await GenerateConfigAsync(data.As<DataCollectionDto>()),
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
                        Actions = EntityActions.Get<DataCollectionManagement>(),
                    },
                    new()
                    {
                        Title = L["Name"],
                        Sortable = true,
                        Data = nameof(DataCollectionDto.Name)
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

        private async Task GenerateConfigAsync(DataCollectionDto dataConfig)
        {
            await AppService.GenerateDataAsync(dataConfig.Id);
        }
        private Guid ItemConfigId { get; set; }
        
        private async Task OpenItemModalAsync(DataCollectionDto dto)
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