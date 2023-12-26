using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Secyud.Ugf;
using Secyud.Ugf.DataManager;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Ugf.DataManager.ClassManagement
{
    public class ClassContainerAppService :
        CrudAppService<ClassContainer, ClassContainerDto, Guid,
            GetClassListInput>, IClassContainerAppService
    {
        private readonly IClassPropertyRepository _propertyRepository;
        private readonly IClassContainerRepository _repository;

        public ClassContainerAppService(
            IClassPropertyRepository propertyRepository,
            IClassContainerRepository repository) : base(repository)
        {
            _propertyRepository = propertyRepository;
            _repository = repository;
        }

        protected override async Task<IQueryable<ClassContainer>> CreateFilteredQueryAsync(GetClassListInput input)
        {
            return await _repository.FilteredQueryableAsync(
                await _repository.GetQueryableAsync(),
                input.Name);
        }

        protected override async Task<ClassContainer> GetEntityByIdAsync(Guid id)
        {
            ClassContainer entity =
                await _repository.FindAsync(id) ??
                await CreateNewAsync(id);
            return entity;
        }

        public async Task CheckPropertiesAsync(Guid id)
        {
            List<ClassProperty> deleteProperties = await _propertyRepository
                .GetListAsync(u => u.ClassId == id);
            List<ClassProperty> insertProperties = new();
            List<ClassProperty> updateProperties = new();

            TypeDescriptor descriptor = U.Tm[id];
            if (descriptor is null)
            {
                await DeleteAsync(id);
                return;
            }

            PropertyDescriptor propertyDescriptor = descriptor.Properties;
            foreach (SAttribute attribute in propertyDescriptor.Attributes)
            {
                ClassProperty cp = deleteProperties.Find(u => u.PropertyName == attribute.Info.Name);

                if (cp is null)
                    insertProperties.Add(new ClassProperty(GuidGenerator.Create(), id,
                        attribute.Info.Name));
                else
                {
                    deleteProperties.Remove(cp);
                    updateProperties.Add(cp);
                }
            }

            await _propertyRepository.DeleteManyAsync(deleteProperties, true);
            await _propertyRepository.UpdateManyAsync(updateProperties, true);
            await _propertyRepository.InsertManyAsync(insertProperties, true);
        }

        public async Task CreateThisAndBase(Guid id)
        {
            Type type = U.Tm[id]?.Type;
            while (U.Tm.IsRegistered(type))
            {
                ClassContainer entity =
                    await _repository.FindAsync(type.GUID);

                if (entity is not null)
                {
                    entity.Name = type.Name;
                    await CheckPropertiesAsync(type.GUID);
                }
                else
                {
                    await CreateNewAsync(type.GUID);
                }

                type = type.BaseType;
            }
        }

        public async Task<List<ClassPropertyDto>> GetPropertiesAsync(Guid id)
        {
            return ObjectMapper.Map<List<ClassProperty>, List<ClassPropertyDto>>(
                await GetPropertiesByIdAsync(id));
        }

        public async Task UpdatePropertiesAsync(List<ClassPropertyDto> properties)
        {
            Dictionary<Guid, ClassPropertyDto> dict = properties.ToDictionary(u => u.Id);
            HashSet<Guid> hashSet = dict.Keys.ToHashSet();
            List<ClassProperty> entities =
                (await _propertyRepository.GetQueryableAsync())
                .Where(u => hashSet.Contains(u.Id))
                .ToList();

            foreach (ClassProperty entity in entities)
            {
                ClassPropertyDto dto = dict[entity.Id];
                ObjectMapper.Map(dto, entity);
            }

            await _propertyRepository.UpdateManyAsync(entities, true);
        }

        private async Task<List<ClassProperty>> GetPropertiesByIdAsync(Guid id)
        {
            List<Guid> findIds = [];
            Type type = U.Tm[id]?.Type;

            while (U.Tm.IsRegistered(type))
            {
                findIds.Add(type.GUID);
                type = type.BaseType;
            }

            return
                (await _propertyRepository.GetQueryableAsync())
                .Where(u => findIds.Contains(u.ClassId))
                .ToList();
        }

        private async Task<ClassContainer> CreateNewAsync(Guid classId)
        {
            TypeDescriptor descriptor = U.Tm[classId];

            ClassContainer c = await _repository.InsertAsync(
                new ClassContainer(classId, descriptor.Type.Name));

            await CheckPropertiesAsync(classId);

            return c;
        }

        protected override Task DeleteByIdAsync(Guid id)
        {
            return Repository.HardDeleteAsync(u => u.Id == id);
        }
    }
}