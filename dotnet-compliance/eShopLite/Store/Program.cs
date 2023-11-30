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

// Add logging
#pragma warning disable EXTEXP0002 // HMac redactor is experimental so we need to disable this warning given we are using it.
builder.Services.AddRedaction(configure =>
{
    // For Private Data, we will configure to use the HMac redactor which will allow correlation between log entries.
    configure.SetHmacRedactor(configureHmac =>
    {
        // This key should be kept SECRET! It should be fetched from keyvault or some other secure store.
        configureHmac.Key = "YWFhYWFhYWFhYWFhYWFhYWFhYWFhYWFhYWFhYWFhYWFh";
        // Some discriminator to differentiate between different deployments of a service.
        configureHmac.KeyId = 19;
    }, new DataClassificationSet(DataClassifications.PrivateDataClassification));

    // For Other Data, we will configure to use a custom redactor which will replace the data with ****.
    configure.SetRedactor<EShopCustomRedactor>(new DataClassificationSet(DataClassifications.OtherDataClassification));
});
#pragma warning restore EXTEXP0002 // HMac redactor is experimental so we need to disable this warning given we are using it.

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
