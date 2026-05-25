using BookRight.Application.DependencyInjection;
using BookRight.Application.Services;
using BookRight.Infrastructure.DependencyInjection;
using BookRight.Infrastructure.Persistence;
using BookRight.UI.Components;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplication();
builder.Services.AddInfrastructure();
// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
// IBookingConflictChecker is already registered in ApplicationDependencyInjection


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

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    DataSeeder.Seed(context);

}

app.Run();