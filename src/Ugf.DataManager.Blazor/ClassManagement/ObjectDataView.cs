﻿using System;
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

        private ObjectDataView(object obj)
        {
            Obj = obj;
        }

        public static async Task<ObjectDataView> CreateAsync(object obj,IClassContainerAppService appService)
        {
            ObjectDataView view = new(obj);
            PropertyDescriptor descriptor = U.Tm.GetProperty(obj.GetType()).Properties;

            while (descriptor is not null)
            {
                List<ClassPropertyDto> properties = await appService.GetPropertiesAsync(U.Tm[descriptor.Type]);
                view.AddProperty(descriptor.Attributes, properties);
                descriptor = descriptor.BaseProperty;
            }

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