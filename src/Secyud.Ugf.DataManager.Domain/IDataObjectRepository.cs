using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Secyud.Ugf.DataManager
{
    public interface IDataObjectRepository : IRepository<DataObject, Guid>
    {
        Task<List<DataObject>> GetListAsync(
            int skipCount, int maxResultCount, string sorting,
            string name, int? bundleId, Guid classId,
            bool includeDetails = false, CancellationToken token = default);

        Task<IQueryable<DataObject>> FilteredQueryableAsync(
            IQueryable<DataObject> query,
            string name, int? bundleId, Guid classId);
    }
}