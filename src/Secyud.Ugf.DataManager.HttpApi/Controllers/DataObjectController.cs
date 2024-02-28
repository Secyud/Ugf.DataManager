using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Application.Dtos;

namespace Secyud.Ugf.DataManager.Controllers;

[Route("api/[controller]/[action]")]
public class DataObjectController(
    IDataObjectAppService dataObjectAppService)
    : DataManagerController, IDataObjectAppService
{
    [HttpGet]
    public Task<DataObjectDto> GetAsync(Guid id)
    {
        return dataObjectAppService.GetAsync(id);
    }

    [HttpGet]
    public Task<PagedResultDto<DataObjectDto>> GetListAsync(GetDataObjectListInput input)
    {
        return dataObjectAppService.GetListAsync(input);
    }

    [HttpPost]
    public Task<DataObjectDto> CreateAsync(DataObjectDto input)
    {
        return dataObjectAppService.CreateAsync(input);
    }

    [HttpPut]
    public Task<DataObjectDto> UpdateAsync(Guid id, DataObjectDto input)
    {
        return dataObjectAppService.UpdateAsync(id, input);
    }

    [HttpDelete]
    public Task DeleteAsync(Guid id)
    {
        return dataObjectAppService.DeleteAsync(id);
    }

    [HttpPut]
    public Task CheckObjectsValidAsync(int bundle, bool update)
    {
        return dataObjectAppService.CheckObjectsValidAsync(bundle, update);
    }
}