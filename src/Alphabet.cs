using System.Collections.Immutable;

namespace ffx;

/// <summary>
///     Base of alphabets used in FF3 functions.
/// </summary>
internal sealed class Alphabet
{
    /// <summary>
    ///     Alphabet ordered by insert, with index as value.
    /// </summary>
    internal ImmutableDictionary<char, ulong> OrderedAlphabet { get; set; }
   
    /// <summary>
    ///     Alphabet ordered by insert.
    /// </summary>
    internal ImmutableArray<char> OrderedAlphabetCharacters { get; set; }

    /// <summary>
    ///     Return the radix, which is simply the size of the alphabet.
    /// </summary>
    internal ulong Radix => (ulong)OrderedAlphabet.Count;

    /// <summary>
    ///     Build an <see cref="Alphabet"/> object, based on a <see cref="IEnumerable{char}"/>.
    /// </summary>
    /// <param name="orderedAlphabet">Alphabet used to build the object.</param>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="ArgumentException"></exception>
    private Alphabet(IEnumerable<char> orderedAlphabet)
    {
        OrderedAlphabet = orderedAlphabet.Select((val, index) => (val, index))
                        .ToDictionary(letter => letter.val, letter => (ulong)letter.index)
                        .ToImmutableDictionary();

        OrderedAlphabetCharacters = orderedAlphabet
                                    .Distinct()
                                    .ToImmutableArray();
        
        if (OrderedAlphabet.Count < 2)
            throw new ArgumentException($"{nameof(orderedAlphabet)} needs to be composed of at least 2 distincts characters or more.");
    }

    /// <summary>
    ///     Transform an <see cref="IEnumerable{char}"/> into a <see cref="ImmutableDictionary{char, int}"/> that represents a counter.
    /// </summary>
    /// <param name="orderedAlphabet"></param>
    /// <returns></returns>
    internal static Alphabet Create(IEnumerable<char> orderedAlphabet) =>
        new(orderedAlphabet);
}