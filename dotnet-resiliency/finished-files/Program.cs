using Microsoft.Extensions.Http.Resilience;
using Store.Components;
using Store.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<ProductService>();
builder.Services.AddHttpClient<ProductService>(c =>
{
    var url = builder.Configuration["ProductEndpoint"] ?? throw new InvalidOperationException("ProductEndpoint is not set");

    c.BaseAddress = new(url);
}).AddStandardResilienceHandler( options => 
{  
    options.RetryOptions.RetryCount = 7;
    options.TotalRequestTimeoutOptions.Timeout = TimeSpan.FromSeconds(260);
});

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddServerComponents();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.MapRazorComponents<App>()
    .AddServerRenderMode();

app.Run();
