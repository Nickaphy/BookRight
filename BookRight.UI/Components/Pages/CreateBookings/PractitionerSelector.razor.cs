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

        private IReadOnlyList<PractitionerDto> _practitioners = [];
        private PractitionerDto? _selectedPractitioner;

        protected override async Task OnParametersSetAsync()
        {
            if (!string.IsNullOrEmpty(AuthorizationType))
            {
                _practitioners = await PractitionerQueries.GetByAuthorizationType(AuthorizationType);
            }
        }

        private async Task SelectPractitioner(PractitionerDto practitioner)
        {
            _selectedPractitioner = practitioner;
            await OnPractitionerSelected.InvokeAsync(practitioner);
        }

        private void ClearPractitioner()
        {
            _selectedPractitioner = null;
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