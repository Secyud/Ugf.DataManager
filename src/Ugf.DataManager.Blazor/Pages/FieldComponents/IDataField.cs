using Blazorise;
using Secyud.Ugf.DataManager;
using Ugf.DataManager.Blazor.ClassManagement;
using Ugf.DataManager.ClassManagement;

namespace Ugf.DataManager.Blazor.Pages.FieldComponents
{
    public interface IDataField
    {
        SAttribute FieldDescriptor { get; }

        ClassPropertyDto FieldTypeMessage { get; }

        IFluentColumn FieldSize { get; }
        
        ObjectDataView ParentObjectData { get; }
    }
}