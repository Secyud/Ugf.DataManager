using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using Ugf.DataManager.DataConfiguration;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Ugf.DataManager.EntityFrameworkCore
{
    public class DataConfigRepository(IDbContextProvider<DataManagerDbContext> dbContextProvider) :
        EfCoreRepository<DataManagerDbContext, DataConfig, Guid>(dbContextProvider),
        IDataConfigRepository
    {
        public async Task<List<DataConfig>> GetListAsync(int skipCount, int maxResultCount, string sorting, string name,
            bool includeDetails = false,
            CancellationToken token = default)
        {
            IQueryable<DataConfig> query =
                includeDetails ? await WithDetailsAsync() : await GetQueryableAsync();

            IQueryable<DataConfig> results =
                (await FilteredQueryableAsync(query, name))
                .OrderBy(sorting)
                .PageBy(skipCount, maxResultCount);

            List<DataConfig> list = results.ToList();

            return list;
        }

        public Task<IQueryable<DataConfig>> FilteredQueryableAsync(
            IQueryable<DataConfig> query, string name)
        {
            IQueryable<DataConfig> results =
                query.WhereIf(!name.IsNullOrWhiteSpace(), u => u.Name.Contains(name));
            return Task.FromResult(results);
        }
    }
}