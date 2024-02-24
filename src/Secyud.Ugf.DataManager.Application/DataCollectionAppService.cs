using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace Secyud.Ugf.DataManager
{
    public class DataCollectionAppService(IDataCollectionRepository repository, DataOperationManager manager)
        : CrudAppService<DataCollection, DataCollectionDto, Guid, GetDataCollectionListInput>(repository),
            IDataCollectionAppService
    {
        protected override async Task<IQueryable<DataCollection>> CreateFilteredQueryAsync(GetDataCollectionListInput input)
        {
            return await repository.FilteredQueryableAsync(
                await repository.GetQueryableAsync(),
                input.Name);
        }

        public async Task GenerateDataAsync(Guid id)
        {
            DataCollectionDto data = await GetAsync(id);

            List<Guid> guids = data.DataCollectionObjects
                .Select(u => u.ObjectId).ToList();
            await manager.GenerateConfigAsync(guids, data.Name);
        }
    }
}