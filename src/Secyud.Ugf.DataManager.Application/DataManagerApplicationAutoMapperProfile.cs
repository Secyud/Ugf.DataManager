using AutoMapper;

namespace Secyud.Ugf.DataManager;

public class DataManagerApplicationAutoMapperProfile : Profile
{
    public DataManagerApplicationAutoMapperProfile()
    {
        CreateMap<DataObject, DataObjectDto>();
        CreateMap<DataObjectDto, DataObject>();
        CreateMap<DataCollection, DataCollectionDto>();
        CreateMap<DataCollectionDto, DataCollection>();
        CreateMap<DataCollectionObject, DataCollectionObjectDto>();
        CreateMap<DataCollectionObjectDto, DataCollectionObject>();
    }
}
