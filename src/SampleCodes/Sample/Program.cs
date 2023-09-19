/*---------------------------------------------------------------------------------------------------------------------
    
----------------------------------------------------------------------------------------------------------------------*/

using static System.Math;
namespace CSD
{
    class App
    {
        public static void Main()
        {
            
        }
    }

    class Point
    {
        public int X { get; set; }
        public int Y { get; set; }

        public Point(int x = 0, int y = 0)
        {
            X = x;
            Y = y;
        }

        public double Distance(int x = 0, int y = 0) => Sqrt(Pow(X - x, 2) + Pow(Y - y, 2));

        public double Distance(Point other) => Distance(other.X, other.Y);

        public override string ToString() => $"({X}, {Y}";

        //...

    }
}