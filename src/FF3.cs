using System.Collections.Immutable;

namespace ffx;

/// <summary>
///     Class that implements FF3-1 methods.
/// </summary>
public sealed class FF3
{
    /// <summary>
    ///     The current alphabet used as a base for the FF3.
    /// </summary>
    private Alphabet Alphabet { get; set; }
    /// <summary>
    ///     Allowed caracters in a "bits string".
    /// </summary>
    private static readonly ImmutableHashSet<char> validBitChar = new HashSet<char>{'0', '1'}.ToImmutableHashSet();

    /// <summary>
    ///     Build FF3 from an enumeration of chars.
    /// </summary>
    /// <param name="alphabet">The ordered alphabet.</param>
    public FF3(IEnumerable<char> alphabet)
    {
        Alphabet = Alphabet.Create(alphabet);
    }

    /// <summary>
    ///     The number that the numeral string X represents in base radix when the<br/>
    ///     numerals are valued in decreasing order of significance.
    /// </summary>
    /// <example>/* context radix = 5 */NumRadix("00011010") = 755UL;</example>
    /// <param name="numeralString">String as a valid numeral</param>
    /// <param name="alphabet">Alphabet used as radix</param>
    /// <returns>The value in long.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when a character is not in the alphabet.</exception>
    private static ulong NumRadix(string numeralString, Alphabet alphabet)
    {
        var res = 0UL;
        foreach (var numeralChar in numeralString)
        {
            res *= alphabet.Radix;
            if (!alphabet.OrderedAlphabet.TryGetValue(numeralChar, out var numeralCharValue))
                throw new ArgumentOutOfRangeException(nameof(numeralString));
            res += numeralCharValue;
        }

        return res;
    }

    /// <summary>
    ///     The number that the numeral string X represents in base radix when the<br/>
    ///     numerals are valued in decreasing order of significance.
    /// </summary>
    /// <example>/* context radix = 5 */NumRadix("00011010") == 755UL;</example>
    /// <param name="numeralString">String as a valid numeral</param>
    /// <returns>The value in long.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when a character is not in the alphabet.</exception>
    internal ulong NumRadix(string numeralString) => NumRadix(numeralString, Alphabet);

    /// <summary>
    ///     Same as <see cref="NumRadix"/> but with a radix of 2, and an alphabet with {0,1}.
    /// </summary>
    /// <param name="bitString">String as a valid bits string</param>
    /// <returns>The value in long.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when a character is not in the alphabet.</exception>
    internal static ulong Num(string bitString) => NumRadix(bitString, Alphabet.Create(validBitChar));

    /// <summary>
    ///     Given a numeral string, X, the numeral string that consists of the numerals of X in reverse order.
    /// </summary>
    /// <example>/* in base ten */REV("13579") == 97531;</example>
    /// <param name="numeralString">The numeral string.</param>
    /// <returns>The reversed numeral string.</returns>
    internal static string Rev(string numeralString) => string.Concat(numeralString.Reverse());

    /// <summary>
    ///     Given a byte string, X, the byte string that consists of the bytes of X in <br/>
    ///     reverse order.
    /// </summary>
    /// <example>/* with x^1 = x on 1 byte */REVB([1]^1 ||[2]^1 ||[3]^1)=[3]^1 ||[2]^1 ||[1]^1;</example>
    /// <param name="byteString">The byte string.</param>
    /// <returns>The reversed byte string.</returns>
    internal static byte[] Revb(byte[] byteString) => byteString.Reverse().ToArray();
    
    /// <summary>
    ///     The number of bytes in a byte string, X, which may be represented as a bit string.
    /// </summary>
    /// <param name="byteString">The byte string.</param>
    /// <example>BYTELEN(new byte[]{0b10111001,0b10101100}) = 2;</example>
    /// <returns>Size of the byte string (in bits, so * 8).</returns>
    /// 
    internal static long ByteLen(byte[] byteString) => byteString.Length * 8;
    
    /// <summary>
    ///     Given a nonnegative integer x less than radixm, the representation of x as a<br/>
    ///     string of m numerals in base radix, in decreasing order of significance.
    /// </summary>
    /// <param name="value"></param>
    /// <param name="stringLength"></param>
    /// <example>(with radix 12), Str(559,4) is the string of four numerals in base 12 that represents 559, namely, 0 3 10 7.</example>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    internal string Str(ulong value, int stringLength)
    {
        if(value >= Math.Pow(Alphabet.Radix, stringLength)) throw new ArgumentOutOfRangeException(nameof(value));

        var res = new char[stringLength];
        for(int charIndex = 0; charIndex < stringLength; charIndex++)
        {
            res[stringLength - charIndex - 1] = Alphabet.OrderedAlphabetCharacters[(int)(value % Alphabet.Radix)];
            value = (ulong)Math.Floor((decimal)value / Alphabet.Radix);
        }
        return new string(res, 0, stringLength);
    }
}