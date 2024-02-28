using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Secyud.Ugf.DataManager.Localization;
using Volo.Abp.AspNetCore.Components.Web.Extensibility.EntityActions;
using Volo.Abp.AspNetCore.Components.Web.Extensibility.TableColumns;
using Volo.Abp.BlazoriseUI.Components;

namespace Secyud.Ugf.DataManager.Blazor.Pages
{
    public partial class DataCollectionObjectManagement
    {
        [Inject] public IDataCollectionAppService DataCollectionAppService { get; set; }

        public DataCollectionObjectManagement()
        {
            LocalizationResource = typeof(DataManagerResource);
        }

        public Guid CollectionId { get; set; }

        private DataCollectionDto DataCollection { get; set; }
        private HashSet<Guid> Guids { get; } = [];

        public List<TableColumn> ObjectTableColumns => TableColumns.Get<DataCollectionObjectManagement>();
        public List<EntityAction> ObjectEntityActions => EntityActions.Get<DataCollectionObjectManagement>();

        private Task ChangeCheckAsync(DataObjectDto dataConfigDto)
        {
            if (!Guids.Add(dataConfigDto.Id))
            {
                Guids.Remove(dataConfigDto.Id);
            }

            return Task.CompletedTask;
        }

        protected override ValueTask SetEntityActionsAsync()
        {
            ObjectEntityActions.Add(new EntityAction
            {
                Text = "Select",
                Clicked = async data => await ChangeCheckAsync(
                    data.As<DataObjectDto>())
            });
            return ValueTask.CompletedTask;
        }

        protected override ValueTask SetTableColumnsAsync()
        {
            ObjectTableColumns
                .AddRange(new TableColumn[]
                {
                    new()
                    {
                        Title = L["Actions"],
                        Actions = ObjectEntityActions,
                    },
                    new()
                    {
                        Title = L["Selected"],
                        Data = nameof(DataObjectDto.Id),
                        ValueConverter = o =>
                        {
                            Guid id = ((DataObjectDto)o).Id;
                            return Guids.Contains(id) ? "√" : "×";
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
                        Title = L["Class"],
                        Sortable = true,
                        Data = nameof(DataObjectDto.ClassId),
                        ValueConverter = o =>
                        {
                            Guid classId = ((DataObjectDto)o).ClassId;
                            return TypeManager.Instance[classId]?.Type.Name;
                        }
                    },
                    new()
                    {
                        Title = L["Bundle"],
                        Sortable = true,
                        Data = nameof(DataObjectDto.BundleId)
                    },
                });

            return ValueTask.CompletedTask;
        }

        public async Task GetConfigAsync(Guid configId)
        {
            CollectionId = configId;
            if (CollectionId == default)
            {
                return;
            }

            DataCollection = await DataCollectionAppService.GetAsync(CollectionId);
            Guids.Clear();
            foreach (DataCollectionObjectDto item in DataCollection.DataCollectionObjects)
            {
                Guids.Add(item.ObjectId);
            }

            await InvokeAsync(StateHasChanged);
        }


        private async Task UpdateAsync()
        {
            DataCollection = await DataCollectionAppService.GetAsync(CollectionId);
            DataCollection.DataCollectionObjects.Clear();
            DataCollection.DataCollectionObjects.AddRange(
                Guids.Select(u => new DataCollectionObjectDto()
            {
                CollectionId = CollectionId,
                ObjectId = u
            }));
            await DataCollectionAppService.UpdateAsync(CollectionId, DataCollection);
            await Message.Success("Submit Success!");
        }

        private void SelectAll()
        {
            foreach (DataObjectDto entity in Entities)
            {
                Guids.Add(entity.Id);
            }
        }

        private void DeSelectAll()
        {
            foreach (DataObjectDto entity in Entities)
            {
                Guids.Remove(entity.Id);
            }
        }
    }
}