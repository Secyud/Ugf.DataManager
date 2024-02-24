using System;
using Volo.Abp.Domain.Entities.Auditing;

namespace Secyud.Ugf.DataManager
{
    public class DataObject:FullAuditedAggregateRoot<Guid>
    {
        private DataObject()
        {
        }
    
        public DataObject(Guid id) 
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