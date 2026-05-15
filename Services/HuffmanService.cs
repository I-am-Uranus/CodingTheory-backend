using CodingTheory.Models;

namespace CodingTheory.Services;

public class HuffmanService
{
    private class HuffmanNode
    {
        public string? Symbol { get; set; }

        public int Frequency { get; set; }

        public HuffmanNode? Left { get; set; }

        public HuffmanNode? Right { get; set; }
    }

    public HuffmanResponse Encode(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            throw new ArgumentException("Text cannot be empty.");
        }

        var frequencies = text
            .GroupBy(character => character)
            .Select(group => new
            {
                Symbol = group.Key.ToString(),
                Frequency = group.Count()
            })
            .ToList();

        var totalCharacters = text.Length;

        var nodes = frequencies
            .Select(item => new HuffmanNode
            {
                Symbol = item.Symbol,
                Frequency = item.Frequency
            })
            .ToList();

        if (nodes.Count == 1)
        {
            var onlySymbol = frequencies[0];

            return new HuffmanResponse
            {
                Codes =
                [
                    new HuffmanCodeDto
                    {
                        Symbol = onlySymbol.Symbol,
                        Frequency = onlySymbol.Frequency,
                        Probability = 1,
                        Code = "0"
                    }
                ],
                EncodedText = new string('0', text.Length),
                Entropy = 0,
                AverageLength = 1,
                OriginalBits = text.Length * 8,
                CompressedBits = text.Length
            };
        }

        while (nodes.Count > 1)
        {
            nodes = nodes
                .OrderBy(node => node.Frequency)
                .ToList();

            var left = nodes[0];
            var right = nodes[1];

            nodes.RemoveAt(0);
            nodes.RemoveAt(0);

            var parent = new HuffmanNode
            {
                Frequency = left.Frequency + right.Frequency,
                Left = left,
                Right = right
            };

            nodes.Add(parent);
        }

        var root = nodes[0];

        var codeMap = new Dictionary<string, string>();

        GenerateCodes(root, string.Empty, codeMap);

        var codes = frequencies
            .Select(item =>
            {
                var probability = (double)item.Frequency / totalCharacters;

                return new HuffmanCodeDto
                {
                    Symbol = item.Symbol,
                    Frequency = item.Frequency,
                    Probability = probability,
                    Code = codeMap[item.Symbol]
                };
            })
            .OrderBy(item => item.Code.Length)
            .ThenByDescending(item => item.Frequency)
            .ToList();

        var encodedText = string.Concat(
            text.Select(character => codeMap[character.ToString()])
        );

        var entropy = codes.Sum(item =>
            -item.Probability * Math.Log2(item.Probability)
        );

        var averageLength = codes.Sum(item =>
            item.Probability * item.Code.Length
        );

        return new HuffmanResponse
        {
            Codes = codes,
            EncodedText = encodedText,
            Entropy = entropy,
            AverageLength = averageLength,
            OriginalBits = text.Length * 8,
            CompressedBits = encodedText.Length
        };
    }

    private void GenerateCodes(
        HuffmanNode node,
        string currentCode,
        Dictionary<string, string> codeMap)
    {
        if (node.Symbol != null)
        {
            codeMap[node.Symbol] = currentCode;
            return;
        }

        if (node.Left != null)
        {
            GenerateCodes(node.Left, currentCode + "0", codeMap);
        }

        if (node.Right != null)
        {
            GenerateCodes(node.Right, currentCode + "1", codeMap);
        }
    }
}