using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Secyud.Ugf.DataManager;
using Secyud.Ugf;
using Volo.Abp.Domain.Services;

namespace Ugf.DataManager.ClassManagement
{
    public class ObjectManager : DomainService
    {
        private readonly ISpecificObjectRepository _objectRepository;
        private readonly IConfiguration _configuration;

        public ObjectManager(
            ISpecificObjectRepository objectRepository,
            IConfiguration configuration)
        {
            _objectRepository = objectRepository;
            _configuration = configuration;
        }

        public async Task CheckObjectsValidAsync(int bundle, bool restore = false)
        {
            IQueryable<SpecificObject> objects = await _objectRepository.GetQueryableAsync();
            foreach (SpecificObject o in objects)
            {
                if (o.BundleId != bundle)
                {
                    continue;
                }
                
                Logger.LogInformation("[Check] {Name} for class id: {ClassId} ", o.Name, o.ClassId);

                ResourceDescriptor resource = null;
                object obj = null;
                try
                {
                    TypeDescriptor descriptor = U.Tm[o.ClassId];
                    obj = U.Get(descriptor.Type);
                    resource = new ResourceDescriptor(o.Name)
                    {
                        Data = o.Data
                    };
                    resource.LoadObject(obj);
                }
                catch (Exception e)
                {
                    if (resource is not null && obj is not null)
                    {
                        if (restore)
                        {
                            resource.SaveObject(obj);
                            o.Data = resource.Data;
                            await _objectRepository.UpdateAsync(o);
                        }
                    }
                    Logger.LogError("{Exception}",e);
                }
            }
        }

        public async Task GenerateConfigAsync(List<Guid> objectIds, string configName)
        {
            Logger.LogInformation("ClassManager: Begin search object");

            List<SpecificObject> results =
                (await _objectRepository.GetQueryableAsync())
                .Where(u =>
                    objectIds.Contains(u.Id) &&
                    !u.IsDeleted &&
                    u.Data != null &&
                    u.Data.Any())
                .ToList();

            string path = Path.Combine(_configuration["ConfigPath"] ?? AppContext.BaseDirectory, "OutConfigs");
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);


            path = Path.Combine(path, $"{configName}.binary");

            Logger.LogInformation("ClassManager: Start write config to path: {Path}", path);
            await using FileStream stream = File.OpenWrite(path);
            await using DataWriter writer = new(stream);

            writer.Write(results.Count);

            foreach (SpecificObject result in results)
            {
                writer.SaveResource(result.ClassId, new ResourceDescriptor(result.Name)
                {
                    Data = result.Data
                });
            }

            Logger.LogInformation("Config successfully write {Count} objects", results.Count);
        }
    }
}