using System;
using Secyud.Ugf.DataManager;
using Volo.Abp.Domain.Entities;

namespace Ugf.DataManager.ClassManagement
{
    public class ClassProperty : Entity<Guid>
    {
        private ClassProperty()
        {
        }

        public ClassProperty(Guid id, 
            Guid classId,string propertyName)
            : base(id)
        {
            ClassId = classId;
            PropertyName = propertyName;
        }

        public Guid ClassId { get; set; }
        public string PropertyName { get; set; }
        public string Name { get; set; }

        public string Description { get; set; }
    }
}