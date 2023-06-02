namespace UtilityLibrary;

[TestFixture]
public class ReverseStringTest
{
    [Author("Ahmet Taçkın")]
    [TestCase("ankara", "arakna")]
    [TestCase("alipapila", "alipapila")]
    [TestCase("", "")]
    public void GivenString_WhenText_ThenReturnReversed(string input, string expected)
    {
        Assert.That(expected, Is.EqualTo(StringUtil.ReverseString(input)));
    }
}
