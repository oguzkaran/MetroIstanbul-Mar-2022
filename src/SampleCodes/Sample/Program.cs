namespace CSD
{
    class App
    {
        public static void Main()
        {
            using var fs = new FileStream("tes.dat", FileMode.CreateNew, FileAccess.Read);
            using var ms = new MemoryStream();
            
            //...
        }

        public static async void Foo()
        {
            await FooAsync();
            Console.WriteLine("FooAsync");
        }

        public static void TaskProc()
        {
            Thread.Sleep(3000);
        }
        public static Task FooAsync()
        {
            var t = new Task(TaskProc);

            t.Start();

            return t;
        }
    }

    class Mample
    {
        public void Foo()
        {
            //...
        }

    }
    class Sample
    {
        private Mample m_mample;

        //...
        ~Sample()
        { 
            m_mample.Foo();
        }
    }
}