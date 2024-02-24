using Volo.Abp.Modularity;

namespace Secyud.Ugf.DataManager;

public abstract class DataManagerApplicationTestBase<TStartupModule> : DataManagerTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
