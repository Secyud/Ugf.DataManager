using System.Threading.Tasks;

namespace Secyud.Ugf.DataManager.Data;

public interface IDataManagerDbSchemaMigrator
{
    Task MigrateAsync();
}
