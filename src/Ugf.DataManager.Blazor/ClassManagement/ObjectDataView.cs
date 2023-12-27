using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Secyud.Ugf;
using Secyud.Ugf.DataManager;
using Ugf.DataManager.ClassManagement;

namespace Ugf.DataManager.Blazor.ClassManagement
{
    public class ObjectDataView
    {
        public List<Tuple<ClassPropertyDto, SAttribute>> Properties { get; } = new();
        public object Obj { get; }
        public ClassContainerDto ClassContainerDto { get; }
        public Type Type { get; }

        private ObjectDataView(object obj, ClassContainerDto classContainerDto)
        {
            Obj = obj;
            Type = obj.GetType();
            ClassContainerDto = classContainerDto;
        }

        public static async Task<ObjectDataView> CreateAsync(object obj,IClassContainerAppService appService)
        {
            Type objectType = obj.GetType();

            if (!objectType.IsClass)
            {
                U.LogError("non-class type is not support for s field!");
                return new ObjectDataView(obj,new ClassContainerDto());
            }
            
            ClassContainerDto classContainer = await appService.GetAsync(objectType.GUID);
            ObjectDataView view = new(obj,classContainer);
            
            
            while (U.Tm.IsRegistered(objectType))
            {
                TypeDescriptor classDesc = U.Tm[objectType];
                if (classDesc is null) break;
                List<ClassPropertyDto> properties = await appService.GetPropertiesAsync(classDesc.Type.GUID);
                view.AddProperty(classDesc.Properties.Attributes, properties);
                objectType = objectType.BaseType; 
            }
            view.Properties.Sort(
                (u,v)=>
                    u.Item2.Id - v.Item2.Id);

            return view;
        }

        private void AddProperty(SAttribute[] attributes, List<ClassPropertyDto> properties)
        {
            foreach (SAttribute attribute in attributes)
            {
                ClassPropertyDto p = properties.FirstOrDefault(u =>
                    u.PropertyName == attribute.Info.Name);
                Properties.Add(new Tuple<ClassPropertyDto, SAttribute>(p, attribute));
            }
        }

        public TValue GetValue<TValue>(SAttribute attr)
        {
            return (TValue)attr.GetValue(Obj);
        }

        public void SetValue(SAttribute attr, object value)
        {
            attr.SetValue(Obj, value);
        }
    }
}