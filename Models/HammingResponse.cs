namespace CodingTheory.Models;

public class HammingResponse
{
    public string OriginalData { get; set; } = string.Empty;

    public string EncodedData { get; set; } = string.Empty;

    public string CorruptedData { get; set; } = string.Empty;

    public string CorrectedData { get; set; } = string.Empty;

    public int ErrorPosition { get; set; }

    public int DetectedPosition { get; set; }

    public string DecodedData { get; set; } = string.Empty;
}