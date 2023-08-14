using System.ComponentModel.DataAnnotations;
using AutoFixture.Xunit2;
using ffx;

namespace ff3.Tests;

public class AlphabetTest
{
    [Theory, AutoData]
    public void Create_should_create_alphabet_properly_when_distinct_input_length_is_more_than_1(ulong radix, [Range(0, 255)]int firstChar)
    {
        var input = Enumerable.Range(firstChar, (int)radix)
                                .Select(Convert.ToChar);
        var alphabet = Alphabet.Create(input);

        Assert.Equal(radix, alphabet.Radix);
    }

    [Theory]
    [InlineData('a', 'a')]
    [InlineData('a')]
    public void Create_should_throw_an_exception_when_distinct_input_length_is_less_than_2(params char[] inputs)
    {
        Assert.Throws<ArgumentException>(() => Alphabet.Create(inputs));
    }
}
