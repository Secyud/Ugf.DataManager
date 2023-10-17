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
            Guid classId,string propertyName,EditStyle style = EditStyle.Default)
            : base(id)
        {
            ClassId = classId;
            PropertyName = propertyName;
            Style = style;
        }

        public Guid ClassId { get; set; }
        public string PropertyName { get; set; }
        public string Name { get; set; }

        public string Description { get; set; }
        public EditStyle Style { get; set; }
    }
}