using System;
using Volo.Abp.Domain.Entities;

namespace Ugf.DataManager.ClassManagement
{
    public class DataConfigItem:Entity
    {
        public Guid ConfigId { get; set; }
        public Guid ObjectId { get; set; }
        
        public override object[] GetKeys()
        {
            return new object[] { ConfigId, ObjectId };
        }
    }
}