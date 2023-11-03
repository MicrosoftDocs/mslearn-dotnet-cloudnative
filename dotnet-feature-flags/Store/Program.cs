using Store.Components;
using Store.Services;
using Microsoft.FeatureManagement;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureAppConfiguration;

var builder = WebApplication.CreateBuilder(args);

// Retrieve the connection string
string connectionString = builder.Configuration.GetConnectionString("AppConfig");

// Load configuration from Azure App Configuration
builder.Configuration.AddAzureAppConfiguration(options => {
  options.Connect(connectionString)
    .UseFeatureFlags();
});

builder.Services.AddFeatureManagement();
builder.Services.AddAzureAppConfiguration();

builder.Services.AddSingleton<ProductService>();
builder.Services.AddHttpClient<ProductService>(c =>
{
    var url = builder.Configuration["ProductEndpoint"] ?? throw new InvalidOperationException("ProductEndpoint is not set");

    c.BaseAddress = new(url);
});

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.UseAzureAppConfiguration();

app.Run();
