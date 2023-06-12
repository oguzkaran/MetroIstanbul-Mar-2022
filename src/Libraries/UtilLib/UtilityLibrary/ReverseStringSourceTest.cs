/*
    Aşağıdaki input ve expected değerleri şu formattaki bir dosyadan okuyacak şekilde SourceSupplier'ı yazınız
    Dosya Formatı:
    ankara  arakna
    alipapila  alipapila
    ...

    Formatta input ve expexted değerler arasında \n dışında herhangi bir whitespace karakter olabilir
 */

namespace UtilityLibrary;

[TestFixture]
public class ReverseStringSourceTest
{
    public static IEnumerable<string[]> SourceSupplier()
    {
        yield return new string[] { "ankara", "arakna" };
        yield return new string[] { "alipapila", "alipapila" };
        yield return new string[] { "", "" };
    }

    [SetUp]
    public void SetUp()
    {
        //...
    }

    [TearDown]
    public void TearDown()
    {
        //...
    }
 
    [Author("Ahmet Taçgın")]
    [Description("This is the test")]
    [Category("reverse")]
    [Test]
    [TestCaseSource(nameof(SourceSupplier))]
    public void GivenString_WhenText_ThenReturnReversed_SourceSupplier(string input, string expected)
    {
        That(expected, Is.EqualTo(ReverseString(input)));
    }
}
