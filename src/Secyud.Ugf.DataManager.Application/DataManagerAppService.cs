using Secyud.Ugf.DataManager.Localization;
using Volo.Abp.Application.Services;

namespace Secyud.Ugf.DataManager;

/* Inherit your application services from this class.
 */
public abstract class DataManagerAppService : ApplicationService
{
    protected DataManagerAppService()
    {
        LocalizationResource = typeof(DataManagerResource);
    }
}
