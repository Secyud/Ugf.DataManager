using Microsoft.AspNetCore.Components;

namespace Ugf.DataManager.Blazor.Pages.FieldComponents;

public class RootObjectField:ObjectFieldObject
{
    [Parameter] public object RootObject { get; set; }
    
    protected override void OnInitialized()
    {
        SetValue(RootObject);
    }

    protected override void SetValue(object value)
    {
        Cache = value;
    }

    protected override object GetValue()
    {
        return Cache;
    }
}