using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Secyud.Ugf.DataManager.EntityFrameworkCore
{
    public class DataObjectRepository :
        EfCoreRepository<DataManagerDbContext, DataObject, Guid>,
        IDataObjectRepository
    {
        public DataObjectRepository(
            IDbContextProvider<DataManagerDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<List<DataObject>> GetListAsync(
            int skipCount, int maxResultCount, string sorting,
            string name, int? bundleId, Guid classId,
            bool includeDetails = false, CancellationToken token = default)
        {
            IQueryable<DataObject> query =
                includeDetails ? await WithDetailsAsync() : await GetQueryableAsync();

            IQueryable<DataObject> results =
                (await FilteredQueryableAsync(query, name, bundleId, classId))
                .OrderBy(sorting)
                .PageBy(skipCount, maxResultCount);

            return await results.ToListAsync(cancellationToken: token);
        }

        public async Task<IQueryable<DataObject>> FilteredQueryableAsync(
            IQueryable<DataObject> query,
            string name, int? bundleId, Guid classId)
        {
            IQueryable<DataObject> results = (await GetQueryableAsync())
                .WhereIf(!name.IsNullOrEmpty(), u => u.Name.Contains(name))
                .WhereIf(bundleId is not null, u => u.BundleId == bundleId);

            if (classId != default)
            {
                results = results.Where(u => u.ClassId == classId);
            }

            return results;
        }
    }
}