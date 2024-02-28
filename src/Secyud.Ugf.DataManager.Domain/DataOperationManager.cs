using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;

namespace Secyud.Ugf.DataManager
{
    public class DataOperationManager(
        IDataObjectRepository objectRepository,
        IDataCollectionRepository collectionRepository,
        IRepository<DataCollectionObject> coRepository)
        : DomainService
    {
        public async Task CheckObjectsValidAsync(int bundle, bool restore = false)
        {
            IQueryable<DataObject> objects = await objectRepository.GetQueryableAsync();
            foreach (DataObject specificObject in objects)
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

        public async Task<byte[]> GenerateBinaryAsync(Guid collectionId)
        {
            Logger.LogInformation("ClassManager: Begin search object");

            IQueryable<DataObject> objectQuery = await objectRepository.GetQueryableAsync();
            IQueryable<DataCollectionObject> coQuery = await coRepository.GetQueryableAsync();
            IQueryable<DataObject> results =
                from o in objectQuery
                join co in coQuery 
                    on o.Id equals co.ObjectId
                where co.ConfigId == collectionId
                    select o;

            List<DataObject> list =  results.ToList();

            await using MemoryStream stream = new();
            await using BinaryWriter writer = new(stream);

            writer.Write(list.Count);

            foreach (DataObject o in list)
            {
                DataResource resource = new()
                {
                    Id = o.ResourceId,
                    Type = o.ClassId,
                    Data = o.Data
                };
                resource.Save(writer);
            }

            Logger.LogInformation("Config successfully write {Count} objects", list.Count);

            return stream.ToArray();
        }
    }
}