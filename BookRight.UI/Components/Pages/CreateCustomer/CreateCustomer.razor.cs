using BookRight.Facade.Commands.CustomerCommands;
using BookRight.Facade.Dtos.CommandDto.CustomerCommands;
using Microsoft.AspNetCore.Components;

namespace BookRight.UI.Components.Pages.CreateCustomers;

public partial class CreateCustomer : ComponentBase
{
    [Inject] private ICreateCustomer CreateCustomerUseCase { get; set; } = default!;
    [Inject] private NavigationManager Navigation { get; set; } = default!;

    // ── Form fields ───────────────────────────────────────────────────────────
    private string _name = string.Empty;
    private string _phoneNumber = string.Empty;
    private string _email = string.Empty;
    private DateTime _dateOfBirth = DateTime.Today.AddYears(-30);
    private string? _street;
    private string _zipcode = string.Empty;
    private string _city = string.Empty;
    private string? _note;

    // ── State ─────────────────────────────────────────────────────────────────
    private bool _isSubmitting = false;
    private bool _saved = false;
    private string? _savedName;
    private string? _errorMessage;

    private async Task SubmitAsync()
    {
        _errorMessage = null;

        // Basic client-side guard — the domain validates fully on Create()
        if (string.IsNullOrWhiteSpace(_name) ||
            string.IsNullOrWhiteSpace(_phoneNumber) ||
            string.IsNullOrWhiteSpace(_email) ||
            string.IsNullOrWhiteSpace(_city) ||
            string.IsNullOrWhiteSpace(_zipcode))
        {
            _errorMessage = "Udfyld venligst alle påkrævede felter (markeret med *).";
            return;
        }

        _isSubmitting = true;

        try
        {
            // LoyaltyLevel is always None for new customers —
            // it is derived automatically from booking history.
            var request = new CreateCustomerRequest(
                Name: _name.Trim(),
                PhoneNumber: _phoneNumber.Trim(),
                Email: _email.Trim(),
                LoyaltyLevel: 0, // None
                DateOfBirth: _dateOfBirth,
                Note: string.IsNullOrWhiteSpace(_note) ? null : _note.Trim(),
                Street: string.IsNullOrWhiteSpace(_street) ? null : _street.Trim(),
                City: _city.Trim(),
                Zipcode: _zipcode.Trim());

            await CreateCustomerUseCase.HandleAsync(request);

            _savedName = _name;
            _saved = true;
        }
        catch (Exception ex)
        {
            // Domain validation errors (e.g. phone number format) surface here
            _errorMessage = ex.Message;
        }
        finally
        {
            _isSubmitting = false;
        }
    }

    private void ResetForm()
    {
        _name = string.Empty;
        _phoneNumber = string.Empty;
        _email = string.Empty;
        _dateOfBirth = DateTime.Today.AddYears(-30);
        _street = null;
        _zipcode = string.Empty;
        _city = string.Empty;
        _note = null;
        _errorMessage = null;
        _saved = false;
        _savedName = null;
    }
}