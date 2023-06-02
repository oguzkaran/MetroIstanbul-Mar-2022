using System;
using System.Text;

namespace MetroIstanbul.Util
{
	public static class StringUtil
	{
		public static string ReverseString(string s)
		{
			var sb = new StringBuilder();

			for (int i = s.Length - 1; i >= 0; --i)
				sb.Append(s[i]);

			return sb.ToString();
		}
	}

	//...
}

