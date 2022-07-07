using System;

namespace CSD
{
    class Program
    {
        public static void Main(string [] args)
        {
            try
            {
                args = new string[] { "50500" };
                var server = new Server { Port = int.Parse(args[0]) };

                server.Run();
            }
            catch (Exception ex) {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
