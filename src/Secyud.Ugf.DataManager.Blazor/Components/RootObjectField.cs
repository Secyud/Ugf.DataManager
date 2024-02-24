using Microsoft.AspNetCore.Components;

namespace Secyud.Ugf.DataManager.Blazor.Components;

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