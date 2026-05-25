using BookRight.Facade.Dtos.ClinicQuerry;
using BookRight.Facade.Querries.ClinicQuerries;
using Microsoft.AspNetCore.Components;

namespace BookRight.UI.Components.Pages.CreateBookings
{
    public partial class ClinicSelector : ComponentBase
    {
        [Inject]
        private IClinicQuerries ClinicQueries { get; set; } = default!;

        [Parameter]
        public Guid PractitionerId { get; set; }

        [Parameter]
        public EventCallback<ClinicDto> OnClinicSelected { get; set; }

        [Parameter]
        public EventCallback OnCleared { get; set; }

        private IReadOnlyList<ClinicDto> _clinics = [];
        private ClinicDto? _selectedClinic;

        protected override async Task OnParametersSetAsync()
        {
            if (PractitionerId != Guid.Empty)
            {
                _clinics = await ClinicQueries.GetByPractitionerAsync(PractitionerId);
                _selectedClinic = null;
            }
        }

        private async Task SelectClinic(ClinicDto clinic)
        {
            _selectedClinic = clinic;
            await OnClinicSelected.InvokeAsync(clinic);
        }

        private async Task ClearClinic()
        {
            _selectedClinic = null;
            await OnCleared.InvokeAsync();
        }
    }
}
