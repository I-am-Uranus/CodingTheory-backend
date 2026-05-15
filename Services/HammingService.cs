using CodingTheory.Models;
using System.Text.RegularExpressions;

namespace CodingTheory.Services;

public class HammingService
{
    public HammingResponse Run(string data, int errorPosition)
    {
        if (string.IsNullOrWhiteSpace(data) || !Regex.IsMatch(data, "^[01]+$"))
        {
            throw new ArgumentException("Data must contain only 0 and 1.");
        }

        var encodedData = Encode(data);

        if (errorPosition <= 0 || errorPosition > encodedData.Length)
        {
            errorPosition = 1;
        }

        var corruptedData = IntroduceError(encodedData, errorPosition);
        var detectedPosition = DetectError(corruptedData);
        var correctedData = CorrectError(corruptedData, detectedPosition);
        var decodedData = Decode(correctedData);

        return new HammingResponse
        {
            OriginalData = data,
            EncodedData = encodedData,
            CorruptedData = corruptedData,
            CorrectedData = correctedData,
            ErrorPosition = errorPosition,
            DetectedPosition = detectedPosition,
            DecodedData = decodedData
        };
    }

    private string Encode(string data)
    {
        var dataBits = data
            .Select(character => character - '0')
            .ToArray();

        var dataBitsCount = dataBits.Length;
        var parityBitsCount = 0;

        while (Math.Pow(2, parityBitsCount) < dataBitsCount + parityBitsCount + 1)
        {
            parityBitsCount++;
        }

        var encoded = new int[dataBitsCount + parityBitsCount + 1];

        var dataIndex = 0;

        for (var position = 1; position < encoded.Length; position++)
        {
            if (IsPowerOfTwo(position))
            {
                encoded[position] = 0;
            }
            else
            {
                encoded[position] = dataBits[dataIndex];
                dataIndex++;
            }
        }

        for (var i = 0; i < parityBitsCount; i++)
        {
            var parityPosition = (int)Math.Pow(2, i);
            var parity = 0;

            for (var position = 1; position < encoded.Length; position++)
            {
                if ((position & parityPosition) != 0)
                {
                    parity ^= encoded[position];
                }
            }

            encoded[parityPosition] = parity;
        }

        return string.Join(string.Empty, encoded.Skip(1));
    }

    private int DetectError(string encodedData)
    {
        var bits = encodedData
            .Select(character => character - '0')
            .ToArray();

        var errorPosition = 0;

        for (var parityPosition = 1; parityPosition <= bits.Length; parityPosition *= 2)
        {
            var parity = 0;

            for (var position = 1; position <= bits.Length; position++)
            {
                if ((position & parityPosition) != 0)
                {
                    parity ^= bits[position - 1];
                }
            }

            if (parity != 0)
            {
                errorPosition += parityPosition;
            }
        }

        return errorPosition;
    }

    private string CorrectError(string encodedData, int position)
    {
        if (position <= 0)
        {
            return encodedData;
        }

        var bits = encodedData.ToCharArray();
        var index = position - 1;

        if (index >= 0 && index < bits.Length)
        {
            bits[index] = bits[index] == '0' ? '1' : '0';
        }

        return new string(bits);
    }

    private string Decode(string encodedData)
    {
        var result = new List<char>();

        for (var position = 1; position <= encodedData.Length; position++)
        {
            if (!IsPowerOfTwo(position))
            {
                result.Add(encodedData[position - 1]);
            }
        }

        return new string(result.ToArray());
    }

    private string IntroduceError(string encodedData, int position)
    {
        var bits = encodedData.ToCharArray();
        var index = position - 1;

        bits[index] = bits[index] == '0' ? '1' : '0';

        return new string(bits);
    }

    private bool IsPowerOfTwo(int value)
    {
        return value > 0 && (value & (value - 1)) == 0;
    }
}