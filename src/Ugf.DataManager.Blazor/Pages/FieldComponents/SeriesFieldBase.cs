using Microsoft.AspNetCore.Components;

namespace Ugf.DataManager.Blazor.Pages.FieldComponents;

public abstract class SeriesFieldBase: DataFieldBase
{
   [Parameter] public int Index { get; set; }
   [Parameter] public ObjectFieldSeries ParentField { get; set; }

   protected override DataFieldBase ParentFieldReference => ParentField;

   protected override void OnInitialized()
   {
      LabelText = Index.ToString();
   }
   
   protected override object GetValue()
   {
      return ParentField.List[Index];
   }

   protected override void SetValue(object value)
   {
      ParentField.List[Index] = value;
   }

   protected void RemoveSelf()
   {
      ParentField.List.RemoveAt(Index);
   }
}