using System;

namespace CSD
{
    class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                var client = new Client { Host = "192.168.1.167", Port = 50500 };

                client.Run();
            }
            catch (Exception ex) 
            {
                Console.WriteLine(ex.Message);
            }
            
        }
    }
}
