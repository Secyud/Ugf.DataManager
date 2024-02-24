using System;
using Volo.Abp.Application.Dtos;

namespace Secyud.Ugf.DataManager
{
    public class DataObjectDto : EntityDto<Guid>
    {
        public string Name { get; set; }
        public Guid ClassId { get; set; }
        public int ResourceId { get; set; }
        public int BundleId { get; set; }
        public byte[] Data { get; set; }
    }
}