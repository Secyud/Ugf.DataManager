using AutoMapper;
using Ugf.DataManager.DataConfiguration;

namespace Ugf.DataManager
{
    public class DataManagerApplicationAutoMapperProfile : Profile
    {
        public DataManagerApplicationAutoMapperProfile()
        {
            CreateMap<SpecificObject, SpecificObjectDto>();
            CreateMap<SpecificObjectDto, SpecificObject>();
            CreateMap<DataConfig, DataConfigDto>();
            CreateMap<DataConfigItem, DataConfigItemDto>();
            CreateMap<DataConfigDto, DataConfig>();
            CreateMap<DataConfigItemDto, DataConfigItem>();
        }
    }
}
