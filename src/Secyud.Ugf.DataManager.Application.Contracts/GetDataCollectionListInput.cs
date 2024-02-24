using Volo.Abp.Application.Dtos;

namespace Secyud.Ugf.DataManager
{
    public class GetDataCollectionListInput :
        PagedAndSortedResultRequestDto
    {
        public string Name { get; set; }
    }
}