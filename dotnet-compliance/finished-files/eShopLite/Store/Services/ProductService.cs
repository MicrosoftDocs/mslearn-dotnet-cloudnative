using DataEntities;
using System.Text;
using System.Text.Json;

namespace Store.Services;

public class ProductService
{
  HttpClient httpClient;
  private readonly ILogger<ProductService> _logger;

  public ProductService(HttpClient httpClient, ILogger<ProductService> logger)
  {
    _logger = logger;
    this.httpClient = httpClient;
  }

  public async Task<List<Product>> GetProducts()
  {
    List<Product>? products = null;
    try
    {
      var response = await httpClient.GetAsync("/api/Product");
      var responseText = await response.Content.ReadAsStringAsync();

      if (response.IsSuccessStatusCode)
      {
        var options = new JsonSerializerOptions
        {
          PropertyNameCaseInsensitive = true
        };

        products = await response.Content.ReadFromJsonAsync(ProductSerializerContext.Default.ListProduct);
        foreach (var product in products!)
        {
          _logger.LogProduct(product);
        }
        
      }
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "Error during GetProducts.");
    }

    return products ?? new List<Product>();
  }

  public async Task<Product> GetProduct(int productId)
  {
    Product? product = null;
    try
    {
      var response = await httpClient.GetAsync($"/api/Product/{productId}");
      var responseText = await response.Content.ReadAsStringAsync();

      if (response.IsSuccessStatusCode)
      {
        var options = new JsonSerializerOptions
        {
          PropertyNameCaseInsensitive = true
        };

        product = await response.Content.ReadFromJsonAsync<Product>(options);
      }
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "Error during GetProduct.");
    }

    return product ?? new Product();
  }

  public async Task<bool> CreateOrder(Order order)
  {
      try
      {
          _logger.LogOrders(order);
          var response = await httpClient.PostAsync("/api/Order", new StringContent(JsonSerializer.Serialize(order), Encoding.UTF8, "application/json"));

          if (response.IsSuccessStatusCode)
          {
              var responseContent = await response.Content.ReadAsStringAsync();
              return true;
          }

          return false;
      }
      catch (Exception ex)
      {
          // handle error  
          return false;
      }
  }


}

#region Logging Extensions
public static partial class Log
{
    [LoggerMessage(1, LogLevel.Information, "Placed Order: {order}")]
    public static partial void LogOrders(this ILogger logger, [LogProperties] Order order);
}
#endregion