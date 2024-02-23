using System.ComponentModel;
using System.Reflection;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using Ugf.DataManager.Localization;

namespace Ugf.DataManager.Blazor.Pages.FieldComponents;

public abstract class ObjectFieldBase : DataFieldBase
{
    [Inject] public IStringLocalizer<DataManagerResource> L { get; set; }
    [Parameter] public DataFieldBase ParentField { get; set; }
    protected IObjectField ObjectField { get; private set; }
    protected string Description { get; set; }

    protected override DataFieldBase ParentFieldReference => ParentField;

    protected override void OnInitialized()
    {
        #region Label

        string str = FieldAttribute.Info.Name;
        if (str.StartsWith('<'))
            str = str[1..^16];
        else if (str.StartsWith('_'))
            str = char.ToUpper(str[1]) + str[2..];
        LabelText = L[str];

        #endregion

        #region Description

        DescriptionAttribute description = FieldAttribute
            .Info.GetCustomAttribute<DescriptionAttribute>();
        if (description is not null)
        {
            Description = L[description.Description];
        }

        #endregion

        if (ParentField is IObjectField field)
        {
            ObjectField = field;
        }
    }

    protected override void SetValue(object value)
    {
        FieldAttribute.SetValue(ObjectField.Reference, value);
    }

    protected override object GetValue()
    {
        return FieldAttribute.GetValue(ObjectField.Reference);
    }
}