using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace Ugf.DataManager.DataConfiguration
{
    public class SpecificObjectAppService(ISpecificObjectRepository repository, ObjectManager objectManager)
        : CrudAppService<SpecificObject, SpecificObjectDto, Guid, GetObjectListInput>(repository),
            ISpecificObjectAppService
    {
        protected override async Task<IQueryable<SpecificObject>> CreateFilteredQueryAsync(GetObjectListInput input)
        {
            return await repository.FilteredQueryableAsync(
                await repository.GetQueryableAsync(),
                input.Name, input.BundleId, input.ClassId);
        }

        public Task GenerateConfigAsync(List<Guid> ids, string name)
        {
            return objectManager.GenerateConfigAsync(ids, name);
        }

        public Task CheckObjectsValidAsync(int bundle, bool update)
        {
            return objectManager.CheckObjectsValidAsync(bundle, update);
        }
    }
}