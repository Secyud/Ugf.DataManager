using System;
using Volo.Abp.Application.Dtos;

namespace Ugf.DataManager.ClassManagement
{
    public class GetConfigListInput:PagedAndSortedResultRequestDto
    {
        public string Name { get; set; }
    }
}