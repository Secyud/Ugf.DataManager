﻿using System;
using Volo.Abp.Application.Dtos;

namespace Ugf.DataManager.ClassManagement
{
    public class ClassPropertyDto : EntityDto<Guid>
    {
        public Guid ClassId { get; set; }
        public string PropertyName { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public EditStyle Style { get; set; }
    }
}