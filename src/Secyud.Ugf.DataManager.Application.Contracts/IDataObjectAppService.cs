using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace Secyud.Ugf.DataManager
{
    public interface IDataObjectAppService :
        ICrudAppService<DataObjectDto, Guid, GetDataObjectListInput>
    {
        Task CheckObjectsValidAsync(int bundle, bool update);
    }
}