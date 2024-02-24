using Microsoft.AspNetCore.Components;

namespace Secyud.Ugf.DataManager.Blazor.Components;

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
      ParentField.SetValue(Index,value);
   }

   protected void RemoveSelf()
   {
      ParentField.RemoveAt(Index);
   }
}