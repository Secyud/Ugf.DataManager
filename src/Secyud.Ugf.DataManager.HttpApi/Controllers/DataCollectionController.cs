using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Application.Dtos;

namespace Secyud.Ugf.DataManager.Controllers;

[Route("api/[controller]/[action]")]
public class DataCollectionController(
    IDataCollectionAppService dataCollectionAppService)
    : DataManagerController, IDataCollectionAppService
{
    [HttpGet]
    public Task<DataCollectionDto> GetAsync(Guid id)
    {
        return dataCollectionAppService.GetAsync(id);
    }

    [HttpGet]
    public Task<PagedResultDto<DataCollectionDto>> GetListAsync(GetDataCollectionListInput input)
    {
        return dataCollectionAppService.GetListAsync(input);
    }

    [HttpPost]
    public Task<DataCollectionDto> CreateAsync(DataCollectionDto input)
    {
        return dataCollectionAppService.CreateAsync(input);
    }

    [HttpPut]
    public Task<DataCollectionDto> UpdateAsync(Guid id, DataCollectionDto input)
    {
        return dataCollectionAppService.UpdateAsync(id, input);
    }

    [HttpDelete]
    public Task DeleteAsync(Guid id)
    {
        return dataCollectionAppService.DeleteAsync(id);
    }

    [HttpGet]
    public Task<byte[]> GenerateDataAsync(Guid id)
    {
        return dataCollectionAppService.GenerateDataAsync(id);
    }

    [HttpGet]
    public async Task<FileResult> DownloadDataAsync(Guid id)
    {
        DataCollectionDto dto = await dataCollectionAppService.GetAsync(id);
        byte[] data = await dataCollectionAppService.GenerateDataAsync(id);

        return new FileContentResult(data, "application/octet-stream")
        {
            FileDownloadName = dto.Name +".binary"
        };
    }
}