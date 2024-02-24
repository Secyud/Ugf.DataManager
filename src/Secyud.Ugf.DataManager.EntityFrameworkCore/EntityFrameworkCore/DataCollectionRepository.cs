using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Secyud.Ugf.DataManager.EntityFrameworkCore
{
    public class DataCollectionRepository :
        EfCoreRepository<DataManagerDbContext, DataCollection, Guid>,
        IDataCollectionRepository
    {
        public DataCollectionRepository(IDbContextProvider<DataManagerDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }

        public async Task<List<DataCollection>> GetListAsync(int skipCount, int maxResultCount, string sorting, string name,
            bool includeDetails = false,
            CancellationToken token = default)
        {
            IQueryable<DataCollection> query =
                includeDetails ? await WithDetailsAsync() : await GetQueryableAsync();

            IQueryable<DataCollection> results =
                (await FilteredQueryableAsync(query, name))
                .OrderBy(sorting)
                .PageBy(skipCount, maxResultCount);

            List<DataCollection> list = results.ToList();

            return list;
        }

        public Task<IQueryable<DataCollection>> FilteredQueryableAsync(
            IQueryable<DataCollection> query, string name)
        {
            IQueryable<DataCollection> results =
                query.WhereIf(!name.IsNullOrWhiteSpace(), u => u.Name.Contains(name));
            return Task.FromResult(results);
        }
    }
}