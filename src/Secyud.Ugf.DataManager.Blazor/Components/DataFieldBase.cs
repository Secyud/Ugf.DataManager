using Microsoft.AspNetCore.Components;

namespace Secyud.Ugf.DataManager.Blazor.Components;

public abstract class DataFieldBase : ComponentBase
{
    [Parameter] public SAttribute FieldAttribute { get; set; }
    protected abstract DataFieldBase ParentFieldReference { get; }
    protected string LabelText { get; set; }
    protected abstract void SetValue(object value);
    protected abstract object GetValue();
}