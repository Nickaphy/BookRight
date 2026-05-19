// Erik's work.

using BookRight.UI.Components;
using BookRight.Application.Commands.BookingCommands;
using BookRight.Application.Repositories;
using BookRight.Application.Services;
// Below we have the DB context usings:
using BookRight.Infrastructure.Persistence.Repositories;
using BookRight.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
// Below we have the using we need in order to have the Fake-files in TestDoubles to be active.
//using BookRight.UI.TestDoubles;
using BookRight.Facade.Commands;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Service to BookingConflictChecker - builder
builder.Services.AddScoped<IBookingConflictChecker,BookingConflictChecker>();


// Fake repository builder.
//builder.Services.AddScoped<IBookingRepository, FakeBookingRepository>();

//"Real" DB builder.
//builder.Services.AddScoped<IBookingRepository, BookingRepository>();

//builder.Services.AddScoped<ICustomerRepository, FakeCustomerRepository>();
//builder.Services.AddScoped<IPractitionerRepository, FakePractitionerRepository>();
//builder.Services.AddScoped<IClinicRepository, FakeClinicRepository>();
//builder.Services.AddScoped<ITreatmentTypeRepository, FakeTreatmentTypeRepository>();


builder.Services.AddDbContext<BookRightDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("BookRightDb")));



// Fake repositories are registered here to test the full UI → Application → Domain flow
// before the real EF Core repositories are implemented.

builder.Services.AddScoped<CreateBookingCommandHandler>();
builder.Services.AddScoped<ICreateBookingUseCase, BookingFacade>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
