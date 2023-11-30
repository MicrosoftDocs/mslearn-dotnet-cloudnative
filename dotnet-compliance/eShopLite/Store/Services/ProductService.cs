using DataEntities;
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

  public async Task<bool> UpdateStock(int productId, int stockAmount)
  {
      try
      {
          var response = await httpClient.PutAsync($"/api/Stock/{productId}?stockAmount={stockAmount}", null);

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
    [LoggerMessage(1, LogLevel.Information, "Returned Product: {product}")]
    public static partial void LogProduct(this ILogger logger, [LogProperties] Product product);
}
#endregion