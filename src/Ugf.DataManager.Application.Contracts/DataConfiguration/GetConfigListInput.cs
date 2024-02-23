using Volo.Abp.Application.Dtos;

namespace Ugf.DataManager.DataConfiguration
{
    public class GetConfigListInput:PagedAndSortedResultRequestDto
    {
        public string Name { get; set; }
    }
}