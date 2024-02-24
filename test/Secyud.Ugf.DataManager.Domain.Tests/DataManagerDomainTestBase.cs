using Volo.Abp.Modularity;

namespace Secyud.Ugf.DataManager;

/* Inherit from this class for your domain layer tests. */
public abstract class DataManagerDomainTestBase<TStartupModule> : DataManagerTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
