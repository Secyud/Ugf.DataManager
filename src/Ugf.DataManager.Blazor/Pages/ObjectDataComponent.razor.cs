using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Secyud.Ugf;
using Secyud.Ugf.DataManager;
using Ugf.DataManager.Blazor.ClassManagement;
using Ugf.DataManager.ClassManagement;

namespace Ugf.DataManager.Blazor.Pages
{
    public partial class ObjectDataComponent
    {
        [Inject] public IClassContainerAppService AppService { get; set; }
        [Parameter] public object Object { get; set; }

        private ObjectDataView _data;

        protected override async Task OnInitializedAsync()
        {
            _data = await ObjectDataView.CreateAsync(Object, AppService);
        }

    }
}