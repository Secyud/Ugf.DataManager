using System;
using Volo.Abp.Application.Dtos;

namespace Secyud.Ugf.DataManager
{
    public class GetDataObjectListInput:PagedAndSortedResultRequestDto
    {
        public string Name { get; set; }
        public int? BundleId { get; set; }
        public Guid ClassId { get; set; }
    }
}