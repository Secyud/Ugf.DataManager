using Secyud.Ugf.DataManager.Samples;
using Xunit;

namespace Secyud.Ugf.DataManager.EntityFrameworkCore.Applications;

[Collection(DataManagerTestConsts.CollectionDefinitionName)]
public class EfCoreSampleAppServiceTests : SampleAppServiceTests<DataManagerEntityFrameworkCoreTestModule>
{

}
