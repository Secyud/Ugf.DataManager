using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Blazorise;
using Microsoft.AspNetCore.Components;
using Secyud.Ugf;
using Secyud.Ugf.DataManager;
using Ugf.DataManager.Blazor.ClassManagement;
using Ugf.DataManager.ClassManagement;
using Ugf.DataManager.Localization;
using Volo.Abp.AspNetCore.Components.Web.Extensibility.EntityActions;
using Volo.Abp.AspNetCore.Components.Web.Extensibility.TableColumns;
using Volo.Abp.AspNetCore.Components.Web.Theming.PageToolbars;

namespace Ugf.DataManager.Blazor.Pages
{
    public partial class ObjectViewer
    {
        [Inject] public IClassContainerAppService AppService { get; set; }
        [Parameter] public ClassPropertyDto Property { get; set; }
        [Parameter] public SAttribute Attr { get; set; }
        [Parameter] public ObjectDataView Data { get; set; }
        [Parameter] public object Obj { get; set; }

        public Task SetObject(SAttribute attribute, Guid classId)
        {
            Obj = CreateObject(classId);
            attribute.SetValue(Data.Obj,Obj);
            return InvokeAsync(StateHasChanged);
        }

        public object CreateObject(Guid classId)
        {
            return classId == default ? null : U.Get(U.Tm[classId]);
        }
    }
}