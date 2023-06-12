namespace UtilityLibrary;

[TestFixture]
public class NumberUtilIsPrimeTimeTest
{ 
    [Author("Hülya Kantar"), Description("IsPrime time test")]    
    [Test, TestCase(710584055392819667), Timeout(20000)]
    public void GivenValue_WhenNumber_ThenCalculateTime(long input)
    {
        NumberUtil.IsPrime(input);
    }
}
