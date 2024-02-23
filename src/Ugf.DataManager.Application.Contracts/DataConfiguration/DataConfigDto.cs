using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Application.Dtos;

namespace Ugf.DataManager.DataConfiguration
{
    public class DataConfigDto:EntityDto<Guid>
    {
        [Required]
        public string Name { get;  set; }
        public List<DataConfigItemDto> DataConfigItems { get; set; }
    }
}