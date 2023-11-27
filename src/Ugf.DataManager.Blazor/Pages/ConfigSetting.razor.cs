using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using Secyud.Ugf;
using Ugf.DataManager.ClassManagement;
using Ugf.DataManager.Localization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Components.Web.Extensibility.EntityActions;
using Volo.Abp.AspNetCore.Components.Web.Extensibility.TableColumns;

namespace Ugf.DataManager.Blazor.Pages
{
    public partial class ConfigSetting
    {
        [Inject] public IDataConfigAppService DataConfigAppService { get; set; }

        public ConfigSetting()
        {
            LocalizationResource = typeof(DataManagerResource);
        }
        
        public Guid ConfigId { get; set; }

        private DataConfigDto Config { get; set; }
        private HashSet<Guid> Guids { get; } = new();

        public List<TableColumn> ObjectTableColumns => TableColumns.Get<ConfigSetting>();
        public List<EntityAction> ObjectEntityActions => EntityActions.Get<ConfigSetting>();

        private Task ChangeCheckAsync(SpecificObjectDto dataConfigDto)
        {
            if (Guids.Contains(dataConfigDto.Id))
            {
                Guids.Remove(dataConfigDto.Id);
            }
            else
            {
                Guids.Add(dataConfigDto.Id);
            }
            return Task.CompletedTask;
        }
        
        protected override ValueTask SetEntityActionsAsync()
        {
            ObjectEntityActions.Add(new EntityAction
            {
                Text = "Select",
                Clicked = async data =>  await ChangeCheckAsync(data.As<SpecificObjectDto>()) 
            });
            return ValueTask.CompletedTask;
        }
        protected override  ValueTask SetTableColumnsAsync()
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
                        Data = nameof(SpecificObjectDto.Id),
                        ValueConverter = o =>
                        {
                            Guid id = ((SpecificObjectDto)o).Id;
                            return Guids.Contains(id) ? "√" :"×" ;
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
                        Title = L["Class"],
                        Sortable = true,
                        Data = nameof(SpecificObjectDto.ClassId),
                        ValueConverter = o =>
                        {
                            Guid classId = ((SpecificObjectDto)o).ClassId;
                            return U.Tm[classId]?.Type.Name;
                        }
                    },
                    new()
                    {
                        Title = L["Bundle"],
                        Sortable = true,
                        Data = nameof(SpecificObjectDto.BundleId)
                    },
                });
            
            return ValueTask.CompletedTask;
        }

        public async Task GetConfigAsync(Guid configId)
        {
            ConfigId = configId;
            if (ConfigId == default)
                return;
            Config = await DataConfigAppService.GetAsync(ConfigId);
            Guids.Clear();
            foreach (DataConfigItemDto item in Config.DataConfigItems)
            {
                Guids.Add(item.ObjectId);
            }
            await InvokeAsync(StateHasChanged);
        }
        

        private async Task UpdateAsync()
        {
            Config = await DataConfigAppService.GetAsync(ConfigId);
            Config.DataConfigItems.Clear();
            Config.DataConfigItems.AddRange(Guids.Select(u=>new DataConfigItemDto()
            {
                ConfigId = ConfigId,
                ObjectId = u
            }));
            await DataConfigAppService.UpdateAsync(ConfigId,Config);
        }

        private void SelectAll()
        {
            foreach (SpecificObjectDto entity in Entities)
            {
                Guids.Add(entity.Id);
            }
        }
        private void DeSelectAll()
        {
            foreach (SpecificObjectDto entity in Entities)
            {
                Guids.Remove(entity.Id);
            }
        }
    }
}