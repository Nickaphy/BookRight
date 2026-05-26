using BookRight.Application.DependencyInjection;
using BookRight.Infrastructure.DependencyInjection;
using BookRight.Infrastructure.Persistence;
using BookRight.UI.Components;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// ====================
// Dependency Injection
// ====================

builder.Services.AddApplication();

builder.Services.AddInfrastructure();

// ====================
// Blazor Services
// ====================

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// ====================
// Build application
// ====================

var app = builder.Build();

// ====================
// Automatic database migration + seeding
// ====================
//
// Goal:
// - Create database automatically
// - Apply migrations automatically
// - Seed demo/test data automatically
//
// This makes the project easier to run for:
// - Developers
// - Teachers
// - Censors
//
// Startup flow:
// Run project → Database created → Seed data inserted → UI starts

using (var scope = app.Services.CreateScope())
{
    var dbFactory =
        scope.ServiceProvider
            .GetRequiredService<IDbContextFactory<AppDbContext>>();

    using var dbContext =
        dbFactory.CreateDbContext();

    // Apply pending migrations automatically.
    dbContext.Database.Migrate();

    // Seed demo data if database is empty.
    DataSeeder.Seed(dbContext);
}

// ====================
// Configure middleware
// ====================

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler(
        "/Error",
        createScopeForErrors: true);

    app.UseHsts();
}

app.UseStatusCodePagesWithReExecute(
    "/not-found",
    createScopeForStatusCodePages: true);

app.UseHttpsRedirection();

app.UseAntiforgery();

// ====================
// Configure Blazor
// ====================

app.MapStaticAssets();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

// ====================
// Run application
// ====================

app.Run();