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

        public async Task<byte[]> GenerateDataAsync(Guid id)
        {
           return await manager.GenerateBinaryAsync(id);
        }
    }
}