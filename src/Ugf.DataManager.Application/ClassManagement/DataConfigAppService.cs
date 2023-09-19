using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace Ugf.DataManager.ClassManagement
{
    public class DataConfigAppService:
        CrudAppService<DataConfig, DataConfigDto, Guid, GetConfigListInput>,
        IDataConfigAppService
    {
        private readonly IDataConfigRepository _repository;
        private readonly ObjectManager _manager;

        public DataConfigAppService(IDataConfigRepository repository,ObjectManager manager) : base(repository)
        {
            _repository = repository;
            _manager = manager;
        }

        protected override async Task<IQueryable<DataConfig>> CreateFilteredQueryAsync(GetConfigListInput input)
        {
            return await _repository.FilteredQueryableAsync(
                await _repository.GetQueryableAsync(),
                input.Name);
        }

        public async Task GenerateDataAsync(Guid id)
        {
            DataConfigDto data = await GetAsync(id);

            List<Guid> guids = data.DataConfigItems
                .Select(u => u.ObjectId).ToList();
            await _manager.GenerateConfigAsync(guids, data.Name);
        }
    }
}