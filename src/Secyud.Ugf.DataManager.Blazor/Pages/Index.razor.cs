using System;
using System.IO;
using System.Threading.Tasks;
using Blazorise;
using Microsoft.AspNetCore.Components;
using Secyud.Ugf.Abstraction;

namespace Secyud.Ugf.DataManager.Blazor.Pages
{
    public partial class Index
    {
        [Inject] public IDataObjectAppService DataObjectAppService { get; set; }

        private int _bundle;
        private bool _update;

        public async Task CheckBundleAsync()
        {
            if (await Message.Confirm($"Bundle: {_bundle}; Update: {_update}"))
            {
                await DataObjectAppService.CheckObjectsValidAsync(_bundle, _update);
            }
        }

        public async Task ImportDataAsync(FileUploadEventArgs e)
        {
            try
            {
                MemoryStream stream = new();
                
                await e.File.WriteToStreamAsync(stream);
                stream.Seek( 0, SeekOrigin.Begin );
                
                DataResource[] resources = stream.ReadResources();

                for (int i = 0; i < resources.Length; i++)
                {
                    DataResource resource = resources[i];
                    object obj = resource.GetObject();
                    
                    await DataObjectAppService.CreateAsync(new DataObjectDto
                    {
                        BundleId = _bundle,
                        ClassId = resource.Type,
                        ResourceId = resource.Id,
                        Data = resource.Data,
                        Name = (obj as IHasName)?.Name,
                    });
                }
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.Message);
            }
            finally
            {
                StateHasChanged();
            }
        }
    }
}