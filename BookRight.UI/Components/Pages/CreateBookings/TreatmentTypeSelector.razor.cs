using Microsoft.AspNetCore.Components;

using BookRight.Facade.Querries.TreatmentTypeQuerries;
using BookRight.Facade.Dtos.QuerryDto.TreatmentTypeDtos;

namespace BookRight.UI.Components.Pages.CreateBookings
{
    public partial class TreatmentTypeSelector : ComponentBase
    {
        [Inject]
        private ITreatmentTypeQuerry TreatmentTypeQuerry { get; set; } = default!;

        [Parameter]
        public EventCallback<TreatmentTypeDto> OnTreatmentTypeSelected { get; set; }

        private IReadOnlyList<TreatmentTypeDto> _treatmentTypes = [];
        private TreatmentTypeDto? _selectedTreatmentType;

        protected override async Task OnInitializedAsync()
        {
            _treatmentTypes = await TreatmentTypeQuerry.GetAllAsync();
        }

        private async Task SelectTreatmentType(TreatmentTypeDto treatmentType)
        {
            _selectedTreatmentType = treatmentType;
            await OnTreatmentTypeSelected.InvokeAsync(treatmentType);
        }

        private string GetEmoji(string authorizationType) => authorizationType switch
        {
            "Physiotherapist" => "🏃",
            "Masseur" => "🤲",
            "Acupuncturist" => "🪡",
            "Nutritionist" => "🥗",
            _ => "🏥"
        };
    }
}
