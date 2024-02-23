using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Ugf.DataManager.DataConfiguration;

namespace Ugf.DataManager.Blazor.Pages
{
    public partial class Index
    {
        [Inject] public ISpecificObjectAppService ObjectAppService { get; set; }

        private int _bundle;
        private bool _update;

        public async Task CheckBundleAsync()
        {
            if (await Message.Confirm($"Bundle: {_bundle}; Update: {_update}"))
            {
                await ObjectAppService.CheckObjectsValidAsync(_bundle, _update);
            }
        }
    }
}