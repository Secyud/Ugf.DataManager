using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace Secyud.Ugf.DataManager
{
    public class DataCollection : FullAuditedAggregateRoot<Guid>
    {
        private DataCollection()
        {
        }

        public DataCollection(
            Guid id, string name)
            : base(id)
        {
            Name = name;
            DataCollectionObjects = new List<DataCollectionObject>();
        }

        public string Name { get; set; }
        public List<DataCollectionObject> DataCollectionObjects { get; set; }
    }
}