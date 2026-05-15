namespace CodingTheory.Models;

public class HuffmanResponse
{
    public List<HuffmanCodeDto> Codes { get; set; } = new();

    public string EncodedText { get; set; } = string.Empty;

    public double Entropy { get; set; }

    public double AverageLength { get; set; }

    public int OriginalBits { get; set; }

    public int CompressedBits { get; set; }
}

public class HuffmanCodeDto
{
    public string Symbol { get; set; } = string.Empty;

    public int Frequency { get; set; }

    public double Probability { get; set; }

    public string Code { get; set; } = string.Empty;
}