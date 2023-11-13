using System.Diagnostics;
using OpenTelemetry.Trace;

namespace Products.Instrumentation;

public class Instrumentation
{
    public static readonly string ActivitySourceName = "eShopLite.Products";

    public ActivitySource ActivitySource { get; } = new ActivitySource(ActivitySourceName);
}

public static class InstrumentationExtensions
{
    public static TracerProviderBuilder AddWorkerInstrumentation(this TracerProviderBuilder tracerProviderBuilder)
    {
        return tracerProviderBuilder.AddSource(Instrumentation.ActivitySourceName);
    }
}
