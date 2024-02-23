using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace Ugf.DataManager.DataConfiguration
{
    public class DataConfigAppService(IDataConfigRepository repository, ObjectManager manager)
        : CrudAppService<DataConfig, DataConfigDto, Guid, GetConfigListInput>(repository),
            IDataConfigAppService
    {
        protected override async Task<IQueryable<DataConfig>> CreateFilteredQueryAsync(GetConfigListInput input)
        {
            return await repository.FilteredQueryableAsync(
                await repository.GetQueryableAsync(),
                input.Name);
        }

        public async Task GenerateDataAsync(Guid id)
        {
            DataConfigDto data = await GetAsync(id);

            List<Guid> guids = data.DataConfigItems
                .Select(u => u.ObjectId).ToList();
            await manager.GenerateConfigAsync(guids, data.Name);
        }
    }
}