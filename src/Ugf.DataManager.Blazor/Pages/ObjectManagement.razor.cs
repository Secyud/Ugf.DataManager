using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Blazorise;
using Microsoft.Extensions.Logging;
using Secyud.Ugf.DataManager;
using Ugf.DataManager.DataConfiguration;
using Ugf.DataManager.Localization;
using Volo.Abp.AspNetCore.Components.Web.Extensibility.EntityActions;
using Volo.Abp.AspNetCore.Components.Web.Extensibility.TableColumns;
using Volo.Abp.AspNetCore.Components.Web.Theming.PageToolbars;

namespace Ugf.DataManager.Blazor.Pages
{
    public partial class ObjectManagement
    {
        public object EditingObject { get; set; }
        public Modal ObjectDataModal { get; set; }
        protected PageToolbar Toolbar { get; } = new();
        protected List<TableColumn> ObjectManagementTableColumns => TableColumns.Get<ObjectManagement>();

        public ObjectManagement()
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
                .Get<ObjectManagement>()
                .AddRange(new EntityAction[]
                {
                    new()
                    {
                        Text = L["Edit"],
                        Clicked = async (data) => { await OpenEditModalAsync(data.As<SpecificObjectDto>()); }
                    },
                    new()
                    {
                        Text = L["Data"],
                        Clicked = async (data) => { await OpenObjectDataModalAsync(data.As<SpecificObjectDto>()); }
                    },
                    new()
                    {
                        Text = L["Delete"],
                        Clicked = async data => await DeleteEntityAsync(data.As<SpecificObjectDto>()),
                        ConfirmationMessage = data => GetDeleteConfirmationMessage(data.As<SpecificObjectDto>())
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
                        Actions = EntityActions.Get<ObjectManagement>(),
                    },
                    new()
                    {
                        Title = L["Class"],
                        Sortable = true,
                        Data = nameof(SpecificObjectDto.ClassId),
                        ValueConverter = o =>
                        {
                            Guid classId = ((SpecificObjectDto)o).ClassId;
                            return classId == default ? string.Empty : TypeManager.Instance[classId]?.Type.Name;
                        }
                    },
                    new()
                    {
                        Title = L["Name"],
                        Sortable = true,
                        Data = nameof(SpecificObjectDto.Name)
                    },
                    new()
                    {
                        Title = L["Bundle"],
                        Sortable = true,
                        Data = nameof(SpecificObjectDto.BundleId)
                    },
                });

            return base.SetTableColumnsAsync();
        }

        protected override string GetDeleteConfirmationMessage(SpecificObjectDto entity)
        {
            return string.Format(L["ObjectDeletionConfirmationMessage"], entity.Name);
        }

        protected override ValueTask SetToolbarItemsAsync()
        {
            Toolbar.AddButton(L["NewObject"],
                OpenCreateModalAsync,
                IconName.Add);

            return base.SetToolbarItemsAsync();
        }

        public async Task CloseObjectDataModalAsync()
        {
            EditingObject = null;
            await InvokeAsync(ObjectDataModal.Hide);
        }

        public async Task OpenObjectDataModalAsync(SpecificObjectDto dto)
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
                EditingObject = resource.GetObject();
            }
            catch (Exception e)
            {
                Logger.LogError("{Exception}", e);
            }
            await ObjectDataModal.Show();
        }


        private Task CloseObjectDataModal(ModalClosingEventArgs arg)
        {
            // cancel close if clicked outside of modal area
            arg.Cancel = arg.CloseReason == CloseReason.FocusLostClosing;
            EditingObject = null;
            return Task.CompletedTask;
        }

        private async Task UpdateObjectDataAsync()
        {
            DataResource resource = new();
            resource.SetObject(EditingObject);
            EditingEntity.Data = resource.Data;
            await AppService.UpdateAsync(EditingEntity.Id, EditingEntity);
            await CloseObjectDataModalAsync();
        }
    }
}