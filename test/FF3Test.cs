using AutoFixture.Xunit2;
using ffx;

namespace ff3.Tests;

public class FF3Test
{
    [Theory, AutoData]
    public void NumRadix_should_return_the_same_value_when_using_base_10(ulong expected)
    {
        var ff3 = new FF3(Enumerable.Range(0, 10).Select(number => number.ToString()[0]));

        Assert.Equal(expected, ff3.NumRadix(expected.ToString()));
    }

    [Fact]
    public void NumRadix_should_return_the_value_of_the_example_when_using_base_5()
    {
        var ff3 = new FF3(Enumerable.Range(0, 5).Select(number => number.ToString()[0]));
        var actual = "00011010";
        var expected = 755UL;
        Assert.Equal(expected, ff3.NumRadix(actual));
    }

    [Theory, AutoData]
    public void NumRadix_should_be_equal_to_Num_when_using_base_2(uint expected)
    {
        var ff3 = new FF3(Enumerable.Range(0, 2).Select(number => number.ToString()[0]));
        var actualBase2 = Convert.ToString(expected, 2);
        var actualNumRadixBase2 = ff3.NumRadix(actualBase2);
        var actualNum = FF3.Num(actualBase2);

        Assert.Equal(expected, actualNum);
        Assert.Equal(expected, actualNumRadixBase2);
    }

    [Theory, AutoData]
    public void Rev_should_reverse_string_no_matter_the_alphabet_when_called_on_numeralstring(string numeralString)
    {
        var reversed = FF3.Rev(numeralString);
        Assert.NotEqual(numeralString, reversed);
        Assert.Equal(numeralString, FF3.Rev(reversed));
    }

    [Theory, AutoData]
    public void Revb_should_reverse_string_no_matter_the_alphabet_when_called_on_numeralstring(byte[] byteString)
    {
        var reversed = FF3.Revb(byteString);
        Assert.NotEqual(byteString, reversed);
        Assert.Equal(byteString, FF3.Revb(reversed));
    }

    [Theory, AutoData]
    public void ByteLen_should_give_length_of_array_8_times(byte[] byteString)
    {
        Assert.Equal(byteString.Length * 8, FF3.ByteLen(byteString));
    }

    [Theory, AutoData]
    public void Str_should_return_same_value_padded_with_zero_when_using_base10(ulong value, int stringLength)
    {
        var ff3 = new FF3(Enumerable.Range(0, 10).Select(number => number.ToString()[0]));

        if(value.ToString().Length <= stringLength)
        {
            Assert.Equal(value.ToString().PadLeft(stringLength, '0'), ff3.Str(value, stringLength));
        }
        else
        {
            Assert.Equal(value.ToString()[^stringLength..], ff3.Str(value, stringLength));
        }
    }

    [Theory]
    [InlineData(53UL, 8, 2)]
    [InlineData(349UL, 3, 8)]
    [InlineData(178UL, 2, 16)]
    public void Str_should_return_same_value_Convert_ToString_when_using_the_same_base(ulong value, int stringLength, ulong radix)
    {
        var expected = Convert.ToString(Convert.ToInt32(value), (int)radix).PadLeft(stringLength, '0').ToUpper()[..stringLength];
        var ff3 = new FF3(Enumerable.Range(0, (int)radix).Select(number => Convert.ToString(Convert.ToInt32(number), (int)radix).ToUpper()[0]));
        var actual = ff3.Str(value, stringLength);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Str_should_throw_ArgumentOutOfRangeException_when_value_is_more_than_radix_pow_length()
    {
        var radix = 10UL;
        var ff3 = new FF3(Enumerable.Range(0, (int)radix).Select(number => number.ToString()[0]));

        Assert.Throws<ArgumentOutOfRangeException>(() => ff3.Str(100, 2));
    }
}