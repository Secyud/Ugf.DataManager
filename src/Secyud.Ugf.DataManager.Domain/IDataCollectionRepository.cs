using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Secyud.Ugf.DataManager
{
    public interface IDataCollectionRepository : IRepository<DataCollection, Guid>
    {
        Task<List<DataCollection>> GetListAsync(
            int skipCount, int maxResultCount, string sorting, string name,
            bool includeDetails = false, CancellationToken token = default);

        Task<IQueryable<DataCollection>> FilteredQueryableAsync(
            IQueryable<DataCollection> query, string name);
    }
}