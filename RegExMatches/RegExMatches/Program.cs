// Yes, use this !
using System.Text.RegularExpressions;

namespace RegExMatches
{
  internal class Program
  {
    static void Main(string[] args)
    {
      Console.WriteLine("Hello, World!");
      // named groups demo
      string sTest = "H2SO4";
      // NO Named groups          CHAR CHAR ?  DIGIT+  // ? none-or-more, + one-or-more
      if (Regex.IsMatch(sTest, @"([A-Z][a-z]?)([0-9]+)*"))
      {
        foreach (Match m in Regex.Matches(sTest, @"([A-Z][a-z]?)([0-9]+)*"))
          Console.WriteLine(m.Value + " : " + m.Groups[1] + ", " + m.Groups[2]);
      }

      //                            NAME   CHAR CHAR?     NAME  DIGIT+ REPEAT
      if (Regex.IsMatch(sTest, @"(?'Symbol'[A-Z][a-z]?)(?'Count'[0-9]+)*"))
      {
        foreach (Match m in Regex.Matches(sTest, @"(?'Symbol'[A-Z][a-z]?)(?'Count'[0-9]+)*"))
          Console.WriteLine(m.Value + " : " + m.Groups["Symbol"] + ", " + m.Groups["Count"]);
      }
    }
  }
}
