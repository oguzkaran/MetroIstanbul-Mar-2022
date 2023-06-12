using System.Text;

namespace MetroIstanbul.Util;

public static class StringUtil
{
	public static string ReverseString(string s)
	{
		var sb = new StringBuilder();

		for (var i = s.Length - 1; i >= 0; --i)
			sb.Append(s[i]);

		return sb.ToString();
	}

	public static string GenerateRandomTextTR(Random r, int count)
	{
		throw new NotImplementedException("TODO: Write and test");
	}

    public static string GenerateRandomTextEN(Random r, int count)
    {
        throw new NotImplementedException("TODO: Write and test");
    }
}

//...

