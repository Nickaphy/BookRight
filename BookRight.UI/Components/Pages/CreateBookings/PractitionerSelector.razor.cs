using BookRight.Facade.Dtos.PractitionerQuerry;
using BookRight.Facade.Querries.PractitionerQuerries;
using Microsoft.AspNetCore.Components;

namespace BookRight.UI.Components.Pages.CreateBookings
{
    public partial class PractitionerSelector : ComponentBase
    {
        [Inject]
        private IPractitionerQuerries PractitionerQueries { get; set; } = default!;

        [Parameter]
        public string AuthorizationType { get; set; } = string.Empty;

        [Parameter]
        public EventCallback<PractitionerDto> OnPractitionerSelected { get; set; }

        [Parameter]
        public EventCallback OnCleared { get; set; }

        private IReadOnlyList<PractitionerDto> _practitioners = [];
        private PractitionerDto? _selectedPractitioner;
        private string _previousAuthorizationType = string.Empty;

        protected override async Task OnParametersSetAsync()
        {
            if (!string.IsNullOrEmpty(AuthorizationType)
                && AuthorizationType != _previousAuthorizationType)
            {
                _previousAuthorizationType = AuthorizationType;

                // Treatment type changed — clear the old practitioner selection
                // so the user isn't left with a practitioner from a different
                // treatment type still appearing as chosen.
                _selectedPractitioner = null;

                _practitioners = await PractitionerQueries.GetByAuthorizationType(AuthorizationType);
            }
        }

        private async Task SelectPractitioner(PractitionerDto practitioner)
        {
            _selectedPractitioner = practitioner;
            await OnPractitionerSelected.InvokeAsync(practitioner);
        }

        private async Task ClearPractitioner()
        {
            _selectedPractitioner = null;
            await OnCleared.InvokeAsync();
        }

        private string GetInitials(string name)
        {
            var parts = name.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            return parts.Length >= 2
                ? $"{parts[0][0]}{parts[1][0]}"
                : name[..1].ToUpper();
        }
    }
}