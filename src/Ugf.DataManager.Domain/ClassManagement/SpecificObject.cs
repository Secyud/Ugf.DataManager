using System;
using Volo.Abp.Domain.Entities.Auditing;

namespace Ugf.DataManager.ClassManagement
{
    public class SpecificObject:FullAuditedAggregateRoot<Guid>
    {
        private SpecificObject()
        {
        }
    
        public SpecificObject(Guid id, 
            Guid classId, string name, int bundleId, byte[] data) 
            : base(id)
        {
            ClassId = classId;
            Name = name;
            BundleId = bundleId;
            Data = data;
        }

        public Guid ClassId { get; set; }
        public string Name { get; set; }
        public int BundleId { get; set; }
        public byte[] Data { get; set; }
    }
}