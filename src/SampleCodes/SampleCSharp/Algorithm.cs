using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSD.Util.Function;

// Bu sınıftaki metotların kodlarına değil yapısına odaklanınız
public static class Algorithm
{
    public static void CopyIf<T>(this List<T> src, List<T> dest, Func<T, bool> pred)
    {
        foreach (var t in src)
            if (pred(t))
                dest.Add(t);
    }
}

