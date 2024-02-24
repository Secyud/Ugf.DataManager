using Xunit;

namespace Secyud.Ugf.DataManager.EntityFrameworkCore;

[CollectionDefinition(DataManagerTestConsts.CollectionDefinitionName)]
public class DataManagerEntityFrameworkCoreCollection : ICollectionFixture<DataManagerEntityFrameworkCoreFixture>
{

}
