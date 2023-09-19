using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Ugf.DataManager.ClassManagement
{
    public interface IDataConfigRepository : IRepository<DataConfig, Guid>
    {
        Task<List<DataConfig>> GetListAsync(
            int skipCount, int maxResultCount, string sorting, string name,
            bool includeDetails = false, CancellationToken token = default);

        Task<IQueryable<DataConfig>> FilteredQueryableAsync(
            IQueryable<DataConfig> query, string name);
    }
}