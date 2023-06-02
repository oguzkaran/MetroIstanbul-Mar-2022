/*---------------------------------------------------------------------------------------------------------------------
    Test İşlemleri:
    Yazılımda test süreçleri ürün geliştirmenin önemli bir aşamasını oluşturmaktadır. Bazı yazılımlarda, ürünün
    herşeyiyle doğru olması kritik öneme sahip olabilmektedir. Bzı yazılımlarda hata toleransı olabilir. Gerektiğinde
    düzeltilebilir

    Eskiden yazılım geliştirmede test süreçleri lüks olarak değerlendirildi. Bu nedenle yalnızca büyük firmalar test
    bölümleri barındırıyorlardı. Günümüzde aartık yazılımda kalite (software quality) bilinci daha fazla artmışi ve
    test süreçleri daha önemsenir hale gelmiştir

    Yazılımda test süreçleri için çeşitli stratejiler kullanılabilmektedir. Test işlemi en aşağı düzeyde programcının
    kendi yazdığı kodları test etmesi ile başlar. Buna "birim test (unit testing)" denir. Programcı genel olarak,
    yazmış olduğu bir metodun doğru çalışıp çalışmadığını test eder. İşte burada metot aslında bir birimdir (unit).
    Bir yazılımda aslında parçalar biraraya getirilir. Yani metotlar çağrılarak yazılım geliştirilir. Bu biraraya
    getirme işlemi sonucunda birleştirilen parçalar yeniden test edilir. Buna da "entegrasyon testi (integration testing)"
    denir. Yazılımın önemli parçalarına modül (module) denir. Modüller de ayrı ayrı test edilebilir. Buna da "modül testi (module testing)"
    denir. Nihayet ürün oluşturulur ve bir bütün olarak test edilir. Genelde bu testlere "kabul testleri (acceptance testing)"
    Ürün bir bütün olarak önce kurum içerisinde test bölümleri tarafından test edilir. Genellikle bu testlere
    "alfa testi (alpha testing)" denir. Sonra ürün seçilmiş bazı son kullanıcılara dağıtılır ve gerçek hatay testine
    sokulur Buna da genellikle "beta testi (beta testing)" denir.

    Birim testi için pratikte şu 3 yaklaşımdan biri uygulanır:
    - Hiç birim testi yapmamak: Bu durum yazılım geliştirmede tavsiye edilmese de bir takım özel sebeplerden dolayı
    firmalar tarafından uygulanabilmaktedir. Örneğin geliştirici ekibin sayı olarak azlığı, projenin deadline'ının kısa
    olması, rakip firmalardan önce ürünü çıkarma kaygısı vb. durumlarda karşılaşılabilmektedir. Buradaki yaklaşım
    programcının hiç test yapmaması yaklaşımı değil programcıdan bir test beklentisi olmaması veya özellikle bir test
    yapılmaması anlamındador. Şüphesiz programcı geliştirme sürecinde belirli ölçüde testler yapacaktır.

    - Katı katıya birim test yapmak: Bu yaklaşımda neredeyse her birim test edilir. Bir metodun basit ya da karmaşık
    olmasına bakılmaksızın testi yapılır. Bu durumda zaman kaybının fazla olmaması için birim testi yapan programcıların
    ayrı olması idealdir. Herhangi zaman kısıtı olmadığı ya da zamanın yeterince uzun olduğu durumlarda bu yaklaşım
    uygulanabilir

    - Gereken birimler için birim testi yapmak: Aslında görünürde en ideal durum budur.

    setup, teardown, input, expected, actual vb. 
----------------------------------------------------------------------------------------------------------------------*/
namespace CSD
{
    class App
    {
        public static void Main()
        {
            
        }
    }
}