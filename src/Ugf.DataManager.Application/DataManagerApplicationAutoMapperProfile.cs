using AutoMapper;
using Ugf.DataManager.ClassManagement;

namespace Ugf.DataManager
{
    public class DataManagerApplicationAutoMapperProfile : Profile
    {
        public DataManagerApplicationAutoMapperProfile()
        {
            CreateMap<ClassProperty, ClassPropertyDto>();
            CreateMap<ClassContainer, ClassContainerDto>();
            CreateMap<SpecificObject, SpecificObjectDto>();
            CreateMap<ClassPropertyDto, ClassProperty>();
            CreateMap<ClassContainerDto, ClassContainer>();
            CreateMap<SpecificObjectDto, SpecificObject>();
            CreateMap<DataConfig, DataConfigDto>();
            CreateMap<DataConfigItem, DataConfigItemDto>();
            CreateMap<DataConfigDto, DataConfig>();
            CreateMap<DataConfigItemDto, DataConfigItem>();
        }
    }
}
