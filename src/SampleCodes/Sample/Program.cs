namespace CSD
{
    class App
    {
        public static void Main()
        {
            Foo();
            Console.WriteLine("Ahmet Taçgın");
            Console.ReadKey(false);
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
}