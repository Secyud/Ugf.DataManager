using Secyud.Ugf.DataManager.Localization;
using Volo.Abp.AspNetCore.Components;

namespace Secyud.Ugf.DataManager.Blazor;

public abstract class DataManagerComponentBase : AbpComponentBase
{
    protected DataManagerComponentBase()
    {
        LocalizationResource = typeof(DataManagerResource);
    }
}
