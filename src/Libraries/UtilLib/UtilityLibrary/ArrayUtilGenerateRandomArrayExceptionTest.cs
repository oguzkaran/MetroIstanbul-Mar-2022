namespace UtilityLibrary;

[TestFixture]
public class ArrayUtilGenerateRandomArrayExceptionTest
{    
    [Author("Muarrem Tekin")]
    [Description("ArgumentNullException Test")]    
    [Test]    
    public void GivenValues_ThenThrowsArgumentNullException()
    {
        Throws<ArgumentNullException>(() => ArrayUtil.GenerateRandomArray(null, 10, 0, 100));
    }

    [Author("Serdar Aslan")]
    [Description("ArgumentException Test")]
    [Test]
    public void GivenValue_WhenMinimumGreaterOrEqualThanBound_ThenThrowsArgumentException()
    {
        Throws<ArgumentException>(() => ArrayUtil.GenerateRandomArray(new Random(), 10, 100, 100));
    }

    [Author("Uğur Baran")]
    [Description("ArgumentException Test")]
    [Test]
    public void GivenValue_WhenCountNotGreaterThanZero_ThenThrowsArgumentException()
    {
        Throws<ArgumentException>(() => ArrayUtil.GenerateRandomArray(new Random(), 0, 0, 100));
    }
}
