namespace MetroIstanbul.Util;

public static class NumberUtil
{
    public static bool IsPrime(long val)
    {        
        if (val <= 1) 
            return false;

        if (val % 2 == 0)
            return val == 2;

        if (val % 3 == 0)
            return val == 3;

        if (val % 5 == 0)
            return val == 5;

        if (val % 7 == 0)
            return val == 7;

        for (var i = 11L; i * i <= val / 2; i += 2)
            if (val % i == 0)
                return false;

        return true;
    }
}
