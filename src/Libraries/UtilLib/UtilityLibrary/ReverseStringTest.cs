namespace UtilityLibrary;

[TestFixture]
public class ReverseStringTest
{
    [Author("Ahmet Taçgın")]
    [TestCase("ankara", "arakna")]
    [TestCase("alipapila", "alipapila")]
    [TestCase("", "")]
    [Category("reverse")]
    [Ignore("Just Passed", Until ="2023-06-12 11:34:00")]
    public void GivenString_WhenText_ThenReturnReversed(string input, string expected)
    {
        That(expected, Is.EqualTo(ReverseString(input)));
    }
}
