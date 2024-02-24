using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace Secyud.Ugf.DataManager
{
    public class DataCollectionDto : EntityDto<Guid>
    {
        public string Name { get; set; }
        public List<DataCollectionObjectDto> DataCollectionObjects { get; set; }
    }
}