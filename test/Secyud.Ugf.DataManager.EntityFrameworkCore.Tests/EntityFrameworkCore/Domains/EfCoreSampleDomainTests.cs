using Secyud.Ugf.DataManager.Samples;
using Xunit;

namespace Secyud.Ugf.DataManager.EntityFrameworkCore.Domains;

[Collection(DataManagerTestConsts.CollectionDefinitionName)]
public class EfCoreSampleDomainTests : SampleDomainTests<DataManagerEntityFrameworkCoreTestModule>
{

}
