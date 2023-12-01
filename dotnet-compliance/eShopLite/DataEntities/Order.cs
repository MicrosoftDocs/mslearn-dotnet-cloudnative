using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace DataEntities;

public class Order
{
    [Key]
    [OIIData]
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("products")]
    public List<Product>? Products { get; set; }

    [OIIData]
    [JsonPropertyName("total")] 
    public decimal Total { get; set; }

    [EUIIData]
    [JsonPropertyName("customerName")]
    public string? CustomerName { get; set; }

    [EUPData]
    [JsonPropertyName("customerAddress")]
    public string? CustomerAddress { get; set; }
}

[JsonSerializable(typeof(List<Order>))]
public sealed partial class OrderSerializerContext : JsonSerializerContext
{
}