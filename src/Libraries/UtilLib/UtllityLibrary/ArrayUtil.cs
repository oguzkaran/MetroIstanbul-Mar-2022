namespace MetroIstanbul.Util;

public static  class ArrayUtil
{
    public static int[] GenerateRandomArray(Random random, int count, int min, int bound)
    {
        if (random == null)
            throw new ArgumentNullException(nameof(Random));

        if (count <= 0 || min >= bound)
            throw new ArgumentException("Invalid values");

        var result = new int[count];

        for (var i = 0; i < count; ++i)
            result[i] = random.Next(min, bound);

        return result;
    }
}
