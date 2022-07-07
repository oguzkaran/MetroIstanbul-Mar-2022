/*---------------------------------------------------------------------------------------------------------------------
    
----------------------------------------------------------------------------------------------------------------------*/
using System;

using CSD.Util.Function;

namespace CSD;

class App
{
    public static void Main()
    {
        var list = new List<int> { 6, 1, 2, 3, 4, 5, 7};
        var evens = new List<int>();

        var query = (from a in list
                    where a % 2 == 0
                    select a * a).ToList();

        query.ToList().ForEach(Console.WriteLine);
       
        //sConsole.WriteLine(query.GetType().Name);   

       // list.Where(a => a % 2 == 0).Select(a => a * a).ToList().ForEach(Console.WriteLine);
    } 
}

class NUmberUtil
{
    public static bool IsEven(int a) => a % 2 == 0;
    public static int Square(int a) => a * a;
}