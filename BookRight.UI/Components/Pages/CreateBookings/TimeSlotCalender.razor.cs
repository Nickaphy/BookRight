using BookRight.Facade.Querries.PractitionerQuerries;
using Microsoft.AspNetCore.Components;
using BookRight.Facade.Dtos.PractitionerQuerry;

namespace BookRight.UI.Components.Pages.CreateBookings
{
    public partial class TimeSlotCalender : ComponentBase
    {
        [Inject]
        private IPractitionerQuerries PractitionerQueries { get; set; } = default!;

        [Parameter]
        public Guid PractitionerId { get; set; }

        [Parameter]
        public int DurationMinutes { get; set; }

        [Parameter]
        public EventCallback<PractitionerAvailableSlotDto> OnSlotSelected { get; set; }

        private DateOnly _currentWeek = DateOnly.FromDateTime(
        DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek + 1));

        private IReadOnlyList<PractitionerAvailableSlotDto> _slots = [];
        private PractitionerAvailableSlotDto? _selectedSlot;

        protected override async Task OnParametersSetAsync()
        {
            await LoadSlots();
        }
        private async Task LoadSlots()
        {
            if (PractitionerId != Guid.Empty)
            {
                _slots = await PractitionerQueries.GetAvailableSlotsAsync(
                    PractitionerId,
                    _currentWeek,
                    DurationMinutes);
            }
        }
        private async Task SelectSlot(PractitionerAvailableSlotDto slot)
        {
            _selectedSlot = slot;
            await OnSlotSelected.InvokeAsync(slot);
        }

        private void ClearSlot()
        {
            _selectedSlot = null;
        }

        private async Task GoToPreviousWeek()
        {
            _currentWeek = _currentWeek.AddDays(-7);
            await LoadSlots();
        }

        private async Task GoToNextWeek()
        {
            _currentWeek = _currentWeek.AddDays(7);
            await LoadSlots();
        }

        private IEnumerable<DateOnly> GetWeekDays()
        {
            var monday = _currentWeek.AddDays(-(int)_currentWeek.DayOfWeek + 1);
            return Enumerable.Range(0, 5).Select(i => monday.AddDays(i));
        }

        private IEnumerable<TimeOnly> GetTimeLabels()
        {
            var times = new List<TimeOnly>();
            var current = new TimeOnly(8, 0);
            while (current < new TimeOnly(17, 0))
            {
                times.Add(current);
                current = current.AddMinutes(30);
            }
            return times;
        }
    }
}
