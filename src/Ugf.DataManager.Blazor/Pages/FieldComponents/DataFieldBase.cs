using Microsoft.AspNetCore.Components;
using Secyud.Ugf.DataManager;

namespace Ugf.DataManager.Blazor.Pages.FieldComponents;

public abstract class DataFieldBase : ComponentBase
{
    [Parameter] public SAttribute FieldAttribute { get; set; }
    protected abstract DataFieldBase ParentFieldReference { get; }
    protected string LabelText { get; set; }
    protected abstract void SetValue(object value);
    protected abstract object GetValue();
}