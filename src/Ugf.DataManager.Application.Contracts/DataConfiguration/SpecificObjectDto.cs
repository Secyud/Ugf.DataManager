using System;
using Volo.Abp.Application.Dtos;

namespace Ugf.DataManager.DataConfiguration
{
    public class SpecificObjectDto : EntityDto<Guid>
    {
        public string Name { get; set; }
        public Guid ClassId { get; set; }
        public int ResourceId { get; set; }
        public int BundleId { get; set; }
        public byte[] Data { get; set; }
    }
}