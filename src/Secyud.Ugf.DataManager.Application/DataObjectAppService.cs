﻿using System;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace Secyud.Ugf.DataManager
{
    public class DataObjectAppService(IDataObjectRepository repository, DataOperationManager manager)
        : CrudAppService<DataObject, DataObjectDto, Guid, GetDataObjectListInput>(repository),
            IDataObjectAppService
    {
        protected override async Task<IQueryable<DataObject>> CreateFilteredQueryAsync(GetDataObjectListInput input)
        {
            return await repository.FilteredQueryableAsync(
                await repository.GetQueryableAsync(),
                input.Name, input.BundleId, input.ClassId);
        }

        public Task CheckObjectsValidAsync(int bundle, bool update)
        {
            return manager.CheckObjectsValidAsync(bundle, update);
        }
    }
}