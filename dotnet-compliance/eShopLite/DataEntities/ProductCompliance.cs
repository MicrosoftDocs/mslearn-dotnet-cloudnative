using Microsoft.Extensions.Compliance.Classification;
using Microsoft.Extensions.Compliance.Redaction;

#region Data Taxonomy definition
public static class DataClassifications
{
    public static DataClassification PrivateDataClassification {get;} = new DataClassification("PrivateDataTaxonomy", "PrivateData");

    public static DataClassification OtherDataClassification {get;} = new DataClassification("OtherDataTaxonomy", "OtherData");
}

public class PrivateDataAttribute : DataClassificationAttribute
{
    public PrivateDataAttribute() : base(DataClassifications.PrivateDataClassification) { }
}

public class OtherDataAttribute : DataClassificationAttribute
{
    public OtherDataAttribute() : base(DataClassifications.OtherDataClassification) { }
}
#endregion

#region Custom Redactor definition
public class EShopCustomRedactor : Redactor
{
    private const string Stars = "****";

    public override int GetRedactedLength(ReadOnlySpan<char> input) => Stars.Length;

    public override int Redact(ReadOnlySpan<char> source, Span<char> destination)
    {
        Stars.CopyTo(destination);
        return Stars.Length;
    }
}
#endregion