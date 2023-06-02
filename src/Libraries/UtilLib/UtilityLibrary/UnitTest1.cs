namespace UtilityLibrary;

[TestFixture]
public class Tests
{
    private static IEnumerable<string[]> SourceSupplier()
    {
        yield return new string[] { "ankara", "arakna" };
        yield return new string[] { "alipapila", "alipapila" };
        yield return new string[] { "", "" };
    }

    [Author("Ahmet Taçkın")]
    [TestCase("ankara", "arakna")]
    [TestCase("alipapila", "alipapila")]
    [TestCase("", "")]
    public void GivenString_WhenText_ThenReturnReversed(string input, string expected)
    {
        Assert.That(expected, Is.EqualTo(StringUtil.ReverseString(input)));
    }

    
    [Author("Ahmet Taçkın")]
    [Test]
    [TestCaseSource(nameof(SourceSupplier))]
    public void GivenString_WhenText_ThenReturnReversed_SourceSupplier(string input, string expected)
    {
        Assert.That(expected, Is.EqualTo(StringUtil.ReverseString(input)));
    }
}
