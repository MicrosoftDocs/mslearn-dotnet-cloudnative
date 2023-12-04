using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace DataEntities;

public class Product
{
    [Key]
    [OIIData]
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("description")]
    public string? Description { get; set; }

    [OIIData]
    [JsonPropertyName("price")]
    public decimal Price { get; set; }

    [JsonPropertyName("imageUrl")]
    public string? ImageUrl { get; set; }

    [OIIData]
    [JsonPropertyName("stock")]
    public int Stock { get; set; }

    [JsonIgnore]
    public List<Order>? Orders { get; set; }
}


[JsonSerializable(typeof(List<Product>))]
public sealed partial class ProductSerializerContext : JsonSerializerContext
{
}