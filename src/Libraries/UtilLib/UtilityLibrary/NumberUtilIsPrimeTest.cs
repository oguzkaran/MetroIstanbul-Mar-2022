/*
    IsPrime metodu için true test ve false testleri ayırınız. Verileri şu şekilde bir dosyadan okuyunuz. Örneğin
    trues.txt dosyası şu şekilde olacaktır
    6 -> input sayısı
    19
    2
    3
    71
    1000003
    17    
 */
namespace UtilityLibrary;

[TestFixture]
public class NumberUtilIsPrimeTest
{ 
    public class DataInfo
    {
        public long Input { get; set; }
        public bool Expected { get; set; }
    }

    public static IEnumerable<DataInfo> SourceSupplier()
    {
        yield return new DataInfo { Input = 19, Expected = true};
        yield return new DataInfo { Input = 2, Expected = true };
        yield return new DataInfo { Input = 1, Expected = false };
    }

    [Author("Halil İbrahim Usta")]
    [Description("IsPrime general test")]    
    [Test]
    [TestCaseSource(nameof(SourceSupplier))]
    public void GivenValue_WhenNumber_ThenReturnsResult(DataInfo dataInfo)
    {        
        That(dataInfo.Expected, Is.EqualTo(NumberUtil.IsPrime(dataInfo.Input)));
    }
}
