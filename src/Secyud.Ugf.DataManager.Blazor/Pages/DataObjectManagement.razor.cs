using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Blazorise;
using Microsoft.Extensions.Logging;
using Secyud.Ugf.DataManager.Localization;
using Volo.Abp.AspNetCore.Components.Web.Extensibility.EntityActions;
using Volo.Abp.AspNetCore.Components.Web.Extensibility.TableColumns;
using Volo.Abp.AspNetCore.Components.Web.Theming.PageToolbars;

namespace Secyud.Ugf.DataManager.Blazor.Pages
{
    public partial class DataObjectManagement
    {
        public object DataObjectEditingObject { get; set; }
        public Modal DataObjectEditModal { get; set; }
        protected PageToolbar Toolbar { get; } = new();
        protected List<TableColumn> ObjectManagementTableColumns => TableColumns.Get<DataObjectManagement>();

        public DataObjectManagement()
        {
            LocalizationResource = typeof(DataManagerResource);
        }

        protected override ValueTask SetBreadcrumbItemsAsync()
        {
            BreadcrumbItems.Add(new Volo.Abp.BlazoriseUI.BreadcrumbItem(L["Menu:Class"].Value));
            BreadcrumbItems.Add(new Volo.Abp.BlazoriseUI.BreadcrumbItem(L["Objects"].Value));
            return base.SetBreadcrumbItemsAsync();
        }

        protected override ValueTask SetEntityActionsAsync()
        {
            EntityActions
                .Get<DataObjectManagement>()
                .AddRange(new EntityAction[]
                {
                    new()
                    {
                        Text = L["Edit"],
                        Clicked = async data => { await OpenEditModalAsync(data.As<DataObjectDto>()); }
                    },
                    new()
                    {
                        Text = L["Data"],
                        Clicked = async data => { await OpenObjectDataModalAsync(data.As<DataObjectDto>()); }
                    },
                    new()
                    {
                        Text = L["Copy"],
                        Clicked = async data => { await CopyObjectAsync(data.As<DataObjectDto>()); }
                    },
                    new()
                    {
                        Text = L["Delete"],
                        Clicked = async data =>
                            await DeleteEntityAsync(data.As<DataObjectDto>()),
                        ConfirmationMessage = data =>
                            GetDeleteConfirmationMessage(data.As<DataObjectDto>())
                    }
                });

            return base.SetEntityActionsAsync();
        }

        protected override ValueTask SetTableColumnsAsync()
        {
            ObjectManagementTableColumns
                .AddRange(new TableColumn[]
                {
                    new()
                    {
                        Title = L["Actions"],
                        Actions = EntityActions.Get<DataObjectManagement>(),
                    },
                    new()
                    {
                        Title = L["Class"],
                        Sortable = true,
                        Data = nameof(DataObjectDto.ClassId),
                        ValueConverter = o =>
                        {
                            Guid classId = ((DataObjectDto)o).ClassId;
                            return classId == default ? string.Empty : TypeManager.Instance[classId]?.Type.Name;
                        }
                    },
                    new()
                    {
                        Title = L["Name"],
                        Sortable = true,
                        Data = nameof(DataObjectDto.Name)
                    },
                    new()
                    {
                        Title = L["ResourceId"],
                        Sortable = true,
                        Data = nameof(DataObjectDto.ResourceId)
                    },
                    new()
                    {
                        Title = L["Bundle"],
                        Sortable = true,
                        Data = nameof(DataObjectDto.BundleId)
                    },
                });

            return base.SetTableColumnsAsync();
        }

        protected override string GetDeleteConfirmationMessage(DataObjectDto entity)
        {
            return string.Format(L["ObjectDeletionConfirmationMessage"], entity.Name);
        }

        protected override ValueTask SetToolbarItemsAsync()
        {
            Toolbar.AddButton(L["NewObject"],
                OpenCreateModalAsync, IconName.Add);

            return base.SetToolbarItemsAsync();
        }

        public async Task CloseObjectDataModalAsync()
        {
            DataObjectEditingObject = null;
            await InvokeAsync(DataObjectEditModal.Hide);
        }

        public async Task OpenObjectDataModalAsync(DataObjectDto dto)
        {
            EditingEntity = await AppService.GetAsync(dto.Id);

            DataResource resource = new()
            {
                Id = 0,
                Type = EditingEntity.ClassId,
                Data = EditingEntity.Data
            };
            try
            {
                DataObjectEditingObject = resource.GetObject();
            }
            catch (Exception e)
            {
                Logger.LogError("{Exception}", e);
            }

            await DataObjectEditModal.Show();
        }


        private Task CloseObjectDataModal(ModalClosingEventArgs arg)
        {
            // cancel close if clicked outside of modal area
            arg.Cancel = arg.CloseReason == CloseReason.FocusLostClosing;
            DataObjectEditingObject = null;
            return Task.CompletedTask;
        }

        private async Task UpdateObjectDataAsync()
        {
            DataResource resource = new();
            resource.SetObject(DataObjectEditingObject);
            EditingEntity.Data = resource.Data;
            await AppService.UpdateAsync(
                EditingEntity.Id, EditingEntity);
            await GetEntitiesAsync();
            await CloseObjectDataModalAsync();
        }

        private async Task CopyObjectAsync(DataObjectDto dto)
        {
            DataObjectDto newEntity = new()
            {
                Name = dto.Name,
                ClassId = dto.ClassId,
                BundleId = dto.BundleId,
                ResourceId = dto.ResourceId,
                Data = dto.Data
            };
            await AppService.CreateAsync(newEntity);
            await GetEntitiesAsync();
            StateHasChanged();
        }
    }
}