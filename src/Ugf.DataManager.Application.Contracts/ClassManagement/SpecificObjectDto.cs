using System;
using Volo.Abp.Application.Dtos;

namespace Ugf.DataManager.ClassManagement
{
    public class SpecificObjectDto:EntityDto<Guid>
    {
        public Guid ClassId { get; set; }
        public string Name { get; set; }
        public int BundleId { get; set; }
        public byte[] Data { get; set; }
    }
}