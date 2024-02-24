using Volo.Abp.Modularity;

namespace Secyud.Ugf.DataManager;

[DependsOn(
    typeof(DataManagerDomainModule),
    typeof(DataManagerTestBaseModule)
)]
public class DataManagerDomainTestModule : AbpModule
{

}
