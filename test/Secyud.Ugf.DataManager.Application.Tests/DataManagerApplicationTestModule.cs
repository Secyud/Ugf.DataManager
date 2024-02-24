using Volo.Abp.Modularity;

namespace Secyud.Ugf.DataManager;

[DependsOn(
    typeof(DataManagerApplicationModule),
    typeof(DataManagerDomainTestModule)
)]
public class DataManagerApplicationTestModule : AbpModule
{

}
