namespace UtilityLibrary;

public class Tests
{
    public static IEnumerable<string[]> SourceSupplier()
    {
        yield return new string[] { "ankara", "arakna" };
        yield return new string[] { "alipapila", "alipapila" };
        yield return new string[] { "", "" };
    }

    [TestCase("ankara", "arakna")]
    [TestCase("alipapila", "alipapila")]
    [TestCase("", "")]
    public void GivenString_WhenText_ThenReturnReversed(string input, string expected)
    {
        Assert.AreEqual(expected, StringUtil.ReverseString(input));
    }

    [Author("Ahmet Taçkın")]
    [TestCaseSource(nameof(SourceSupplier))]
    public void GivenString_WhenText_ThenReturnReversed_Source(string input, string expected)
    {
        Assert.AreEqual(expected, StringUtil.ReverseString(input));
    }
}
