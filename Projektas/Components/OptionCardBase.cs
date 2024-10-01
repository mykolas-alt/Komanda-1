using Microsoft.AspNetCore.Components;

namespace Projektas.Components
{
    public class OptionCardBase : ComponentBase
    {
        [Parameter]
        public int Option { get; set; }
        [Parameter]
        public EventCallback<int> OnOptionSelected { get; set; }

        [Parameter]
        public bool IsDisabled { get; set; }
        protected async void SelectOption()
        {
            if (IsDisabled == false)
            {
                await OnOptionSelected.InvokeAsync(Option);
            }
            
        }
    }
}
