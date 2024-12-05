using Microsoft.AspNetCore.Components;

namespace Projektas.Client.Components {
    public class OptionCardBase : ComponentBase {
        [Parameter]
        public int Option {get; set;}
        [Parameter]
        public EventCallback<int> OnOptionSelected {get; set;}
        protected async void SelectOption() {
            await OnOptionSelected.InvokeAsync(Option);
        }
    }
}
