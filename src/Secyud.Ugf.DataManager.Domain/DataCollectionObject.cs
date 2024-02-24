using System;
using Volo.Abp.Domain.Entities;

namespace Secyud.Ugf.DataManager
{
    public class DataCollectionObject:Entity
    {
        public Guid ConfigId { get; set; }
        public Guid ObjectId { get; set; }
        
        public override object[] GetKeys()
        {
            return [ConfigId, ObjectId];
        }
    }
}