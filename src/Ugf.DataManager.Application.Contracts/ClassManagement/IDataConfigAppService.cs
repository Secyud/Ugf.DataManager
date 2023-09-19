using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace Ugf.DataManager.ClassManagement
{
    public interface IDataConfigAppService:
        ICrudAppService<DataConfigDto, Guid, GetConfigListInput>
    {
        Task GenerateDataAsync(Guid id);
    }
}