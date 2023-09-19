using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Secyud.Ugf;
using Secyud.Ugf.Archiving;
using Volo.Abp.Domain.Services;

namespace Ugf.DataManager.ClassManagement
{
    public class ObjectManager : DomainService
    {
        private readonly ISpecificObjectRepository _objectRepository;

        public ObjectManager(
            ISpecificObjectRepository objectRepository)
        {
            _objectRepository = objectRepository;
        }

        public async Task GenerateConfigAsync(List<Guid> objectIds,string configName)
        {
            Logger.LogInformation("ClassManager: Begin search object");

            List<SpecificObject> results =
                (await _objectRepository.GetQueryableAsync())
                .Where(u => objectIds.Contains(u.Id))
                .ToList();

            string path = Path.Combine(AppContext.BaseDirectory, "OutConfigs");
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

        
            path = Path.Combine(path, $"{configName}.binary");

            Logger.LogInformation("ClassManager: Start write config to path: {Path}", path);
            await using FileStream stream = File.OpenWrite(path);
            await using DefaultArchiveWriter writer = new(stream);

            writer.Write(results.Count);

            foreach (SpecificObject result in results)
            {
                writer.Write(result.ClassId);
                writer.Write(result.Name);
                writer.Write(result.Data.Length);
                writer.Write(result.Data);
            }

            Logger.LogInformation("Config successfully write {Count} objects", results.Count);
        }
    }
}