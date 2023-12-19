using Store.Components;
using Store.Services;
using Microsoft.Extensions.Compliance.Classification;
using Microsoft.Extensions.Compliance.Redaction;

var builder = WebApplication.CreateBuilder(args);
 
builder.Services.AddSingleton<ProductService>();
builder.Services.AddHttpClient<ProductService>(c =>
{
    var url = builder.Configuration["ProductEndpoint"] ?? throw new InvalidOperationException("ProductEndpoint is not set");

    c.BaseAddress = new(url);
});

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Add redaction
builder.Services.AddRedaction(configure =>
{
    configure.SetRedactor<ErasingRedactor>(new DataClassificationSet(DataClassifications.EUPDataClassification));
    configure.SetRedactor<EShopCustomRedactor>(new DataClassificationSet(DataClassifications.EUIIDataClassification));
});

builder.Services.AddLogging(logging => 
{
    logging.EnableRedaction();
    logging.AddJsonConsole(); //Enable structure logs on the console to view the redacted data.
});

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

app.Run();
