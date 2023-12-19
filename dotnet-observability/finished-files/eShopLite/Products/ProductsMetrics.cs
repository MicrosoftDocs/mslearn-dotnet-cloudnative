using System;
using System.Diagnostics.Metrics;

namespace Products.Instrumentation;

public class ProductsMetrics
{
    private readonly Counter<int> _serviceCalls;
    private readonly Counter<int> _stockChange;

    public ProductsMetrics(IMeterFactory meterFactory)
    {
        var meter = meterFactory.Create("eShopLite.Products");
        _serviceCalls = meter.CreateCounter<int>(name: "eshoplite.products.service_calls", unit: "{calls}", description: "Number of times the product service is being called to list all products.");
        _stockChange = meter.CreateCounter<int>("eshoplite.products.stock_change", unit: "{stock}", description: "Amount of stock being changed through the product service.");
    }

    public void ServiceCalls(int quantity)
    {
        _serviceCalls.Add(quantity);
    }

    public void StockChange(int quantity)
    {
        _stockChange.Add(quantity);
    }
}