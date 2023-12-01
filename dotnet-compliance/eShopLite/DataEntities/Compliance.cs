using Microsoft.Extensions.Compliance.Classification;
using Microsoft.Extensions.Compliance.Redaction;

#region Data Taxonomy definition
public static class DataClassifications
{
    // End User Identifiable Information
    public static DataClassification EUIIDataClassification {get;} = new DataClassification("EUIIDataTaxonomy", "EUIIData");

    // End User Pseudonymous Information
    public static DataClassification EUPDataClassification {get;} = new DataClassification("EUPDataTaxonomy", "EUPData");

    // Organizational Identifiable Information 
    public static DataClassification OIIDataClassification {get;} = new DataClassification("OIIDataTaxonomy", "OIIData");
}

public class EUIIDataAttribute : DataClassificationAttribute
{
    public EUIIDataAttribute() : base(DataClassifications.EUIIDataClassification) { }
}

public class EUPDataAttribute : DataClassificationAttribute
{
    public EUPDataAttribute() : base(DataClassifications.EUPDataClassification) { }
}

public class OIIDataAttribute : DataClassificationAttribute
{
    public OIIDataAttribute() : base(DataClassifications.OIIDataClassification) { }
}
#endregion

#region Custom Redactor definition
public class EShopCustomRedactor : Redactor
{
    private const string Stars = "** hidden **";

    public override int GetRedactedLength(ReadOnlySpan<char> input) => Stars.Length;

    public override int Redact(ReadOnlySpan<char> source, Span<char> destination)
    {
        Stars.CopyTo(destination);
        return Stars.Length;
    }
}
#endregion