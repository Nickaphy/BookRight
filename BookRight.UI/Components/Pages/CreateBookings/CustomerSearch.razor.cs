using BookRight.Application;
using BookRight.Facade.Dtos.CustomerDtos;
using BookRight.Facade.Querries.CustomerQuerries;
using Microsoft.AspNetCore.Components;

namespace BookRight.UI.Components.Pages.CreateBookings
{
    public partial class CustomerSearch : ComponentBase
    {
        [Inject]
        private ICustomerQuerries CustomerQueries { get; set; } = default!;

        [Parameter]
        public EventCallback<CustomerDto> OnCustomerSelected { get; set; }

        [Parameter]
        public EventCallback OnCleared { get; set; }

        private string _searchTerm = string.Empty;
        private IReadOnlyList<CustomerDto> _searchResults = [];
        private CustomerDto? _selectedCustomer;
        private bool _isSearching = false;

        private async Task OnSearchChanged(ChangeEventArgs e)
        {
            _searchTerm = e.Value?.ToString() ?? string.Empty;
            if (_searchTerm.Length < 2)
            {
                _searchResults = [];
                return;
            }
            _isSearching = true;
            _searchResults = await CustomerQueries.SearchCustomersAsync(_searchTerm);
            _isSearching = false;
        }

        private async Task SelectCustomer(CustomerDto customer)
        {
            _selectedCustomer = customer;
            _searchResults = [];
            _searchTerm = string.Empty;
            await OnCustomerSelected.InvokeAsync(customer);
        }

        private async Task ClearCustomer()
        {
            _selectedCustomer = null;
            _searchTerm = string.Empty;
            _searchResults = [];
            await OnCleared.InvokeAsync();
        }

        private string GetInitials(string name)
        {
            var parts = name.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            return parts.Length >= 2
                ? $"{parts[0][0]}{parts[1][0]}"
                : name[..1].ToUpper();
        }

        private string GetLoyaltyStyle(CustomerLoyaltyLevel level) => level switch
        {
            CustomerLoyaltyLevel.Bronze => "background-color: #fef3c7; color: #92400e; padding: 4px 10px; border-radius: 999px; font-size: 0.75rem; font-weight: 600;",
            CustomerLoyaltyLevel.Silver => "background-color: #f3f4f6; color: #374151; padding: 4px 10px; border-radius: 999px; font-size: 0.75rem; font-weight: 600;",
            CustomerLoyaltyLevel.Gold => "background-color: #fef9c3; color: #854d0e; padding: 4px 10px; border-radius: 999px; font-size: 0.75rem; font-weight: 600;",
            _ => "display: none;"
        };
    }
}
