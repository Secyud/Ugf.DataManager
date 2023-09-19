using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace Ugf.DataManager.ClassManagement
{
    public class DataConfig : FullAuditedAggregateRoot<Guid>
    {
        private DataConfig()
        {
        }

        public DataConfig(
            Guid id, string name)
            : base(id)
        {
            Name = name;
            DataConfigItems = new List<DataConfigItem>();
        }

        public string Name { get; set; }
        public List<DataConfigItem> DataConfigItems { get; set; }
        
    }
}