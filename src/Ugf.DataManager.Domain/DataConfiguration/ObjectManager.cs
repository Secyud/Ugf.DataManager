using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Secyud.Ugf.DataManager;
using Volo.Abp.Domain.Services;

namespace Ugf.DataManager.DataConfiguration
{
    public class ObjectManager(
        ISpecificObjectRepository objectRepository, IConfiguration configuration)
        : DomainService
    {
        public async Task CheckObjectsValidAsync(int bundle, bool restore = false)
        {
            IQueryable<SpecificObject> objects = await objectRepository.GetQueryableAsync();
            foreach (SpecificObject specificObject in objects)
            {
                if (specificObject.BundleId != bundle)
                {
                    continue;
                }
                
                Console.WriteLine($"[Check] {specificObject.Name} for class id: { specificObject.ClassId} ");

                DataResource resource = new();
                object obj = null;
                try
                {
                    resource.Type = specificObject.ClassId;
                    resource.Data = specificObject.Data;
                    resource.Id = specificObject.ResourceId;
                    obj = resource.GetObject();
                }
                catch (Exception e)
                {
                    if (resource.Data is not null && obj is not null)
                    {
                        if (restore)
                        {
                            resource.SetObject(obj);
                            specificObject.Data = resource.Data;
                            await objectRepository.UpdateAsync(specificObject);
                        }
                    }
                    Console.WriteLine(e);
                }
            }
        }

        public async Task GenerateConfigAsync(List<Guid> objectIds, string configName)
        {
            Logger.LogInformation("ClassManager: Begin search object");

            List<SpecificObject> results =
                (await objectRepository.GetQueryableAsync())
                .Where(u =>
                    objectIds.Contains(u.Id) &&
                    !u.IsDeleted)
                .ToList();

            string path = Path.Combine(configuration["ConfigPath"] ?? AppContext.BaseDirectory, "OutConfigs");
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);


            path = Path.Combine(path, $"{configName}.binary");

            Logger.LogInformation("ClassManager: Start write config to path: {Path}", path);
            await using FileStream stream = File.OpenWrite(path);
            await using BinaryWriter writer = new(stream);

            writer.Write(results.Count);

            foreach (SpecificObject result in results)
            {
                DataResource resource = new()
                {
                    Id = result.ResourceId,
                    Type = result.ClassId,
                    Data = result.Data
                };
                resource.Save(writer);
            }

            Logger.LogInformation("Config successfully write {Count} objects", results.Count);
        }
    }
}