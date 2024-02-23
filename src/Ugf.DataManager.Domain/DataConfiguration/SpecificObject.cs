using System;
using Volo.Abp.Domain.Entities.Auditing;

namespace Ugf.DataManager.DataConfiguration
{
    public class SpecificObject:FullAuditedAggregateRoot<Guid>
    {
        private SpecificObject()
        {
        }
    
        public SpecificObject(Guid id) 
            : base(id)
        {
        }

        public string Name { get; set; }
        public Guid ClassId { get; set; }
        public int ResourceId { get; set; }
        public int BundleId { get; set; }
        public byte[] Data { get; set; }
    }
}