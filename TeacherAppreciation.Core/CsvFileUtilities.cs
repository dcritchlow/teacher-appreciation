using System.IO;
using System.Text.RegularExpressions;

namespace TeacherAppreciation.Core
{
  public static class CsvFileUtilities
  {
    public static void ProcessCsvFile(string tempFilePath)
    {
      AddPercentSignToLineIds(tempFilePath);
      MoveTextToOneLineForCleanup(tempFilePath);
      SplitLinesOnId(tempFilePath);
      RemoveSuperfluousCommas(tempFilePath);
      RemoveQuotes(tempFilePath);
    }

    public static void AddPercentSignToLineIds(string tempFilePath)
    {
      const string lineIdRegex = "(^[0-9]{4,})";
      File.WriteAllText(tempFilePath, Regex.Replace(File.ReadAllText(tempFilePath), lineIdRegex, "%$1", RegexOptions.Multiline));
    }

    public static void MoveTextToOneLineForCleanup(string tempFilePath)
    {
      const string makeOneLine = "\\r?\\n";
      File.WriteAllText(tempFilePath, Regex.Replace(File.ReadAllText(tempFilePath), makeOneLine, "", RegexOptions.Multiline));
    }

    public static void SplitLinesOnId(string tempFilePath)
    {
      const string splitLinesOnId = "%([0-9]{4,})";
      File.WriteAllText(tempFilePath, Regex.Replace(File.ReadAllText(tempFilePath), splitLinesOnId, "\r\n$1", RegexOptions.Multiline));
    }

    public static void RemoveSuperfluousCommas(string tempFilePath)
    {
      const string replaceMultCommas = ",{2,}";
      File.WriteAllText(tempFilePath, Regex.Replace(File.ReadAllText(tempFilePath), replaceMultCommas, "", RegexOptions.Multiline));
    }

    public static void RemoveQuotes(string tempFilePath)
    {
      const string removeQuotes = "\"?";
      File.WriteAllText(tempFilePath, Regex.Replace(File.ReadAllText(tempFilePath), removeQuotes, "", RegexOptions.Multiline));
    }
  }
}