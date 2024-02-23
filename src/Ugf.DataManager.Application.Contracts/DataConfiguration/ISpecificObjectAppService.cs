using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace Ugf.DataManager.DataConfiguration
{
    public interface ISpecificObjectAppService :
        ICrudAppService<SpecificObjectDto, Guid, GetObjectListInput>
    {
        Task GenerateConfigAsync(List<Guid> ids, string name);
        Task CheckObjectsValidAsync(int bundle, bool update);
    }
}