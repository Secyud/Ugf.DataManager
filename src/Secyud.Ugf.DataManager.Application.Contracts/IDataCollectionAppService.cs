using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace Secyud.Ugf.DataManager
{
    public interface IDataCollectionAppService:
        ICrudAppService<DataCollectionDto, Guid, GetDataCollectionListInput>
    {
        Task<byte[]> GenerateDataAsync(Guid id);
    }
}