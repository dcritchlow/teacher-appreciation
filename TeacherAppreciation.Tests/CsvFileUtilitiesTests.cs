using System;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TeacherAppreciation.Core;

namespace TeacherAppreciation.Tests
{
  [TestClass]
  public class CsvFileUtilitiesTests
  {
    private static string TestSubmissionsFilePath => @"Test Files\TestSubmissions.csv";
    private static string TwoLineFormatTestFile => @"Test Files\TwoLineFormatTestFile.csv";

    [TestMethod]
    [DeploymentItem(@"Test Files\TestSubmissions.csv", "Test Files")]
    public void process_file_test()
    {
      CsvFileUtilities.ProcessCsvFile(TestSubmissionsFilePath);
      var lines = File.ReadAllLines(TestSubmissionsFilePath).Length;

      Assert.IsTrue(lines == 29);
    }

    [TestMethod]
    [DeploymentItem(@"Test Files\TwoLineFormatTestFile.csv", "Test Files")]
    public void add_percent_symbol_to_line_ids()
    {
      CsvFileUtilities.AddPercentSignToLineIds(TwoLineFormatTestFile);
      var lines = File.ReadAllLines(TwoLineFormatTestFile);
      var parts = lines[0].Split(',');

      Assert.AreEqual(parts[0], "%4929");
    }

    [TestMethod]
    [DeploymentItem(@"Test Files\TwoLineFormatTestFile.csv", "Test Files")]
    public void move_all_text_to_one_line()
    {
      CsvFileUtilities.MoveTextToOneLineForCleanup(TwoLineFormatTestFile);
      var lines = File.ReadAllLines(TwoLineFormatTestFile).Length;

      Assert.IsTrue(lines == 1);
    }

    [TestMethod]
    [DeploymentItem(@"Test Files\TwoLineFormatTestFile.csv", "Test Files")]
    public void split_lines_on_id_with_percent_sign()
    {
      CsvFileUtilities.AddPercentSignToLineIds(TwoLineFormatTestFile);
      CsvFileUtilities.MoveTextToOneLineForCleanup(TwoLineFormatTestFile);
      CsvFileUtilities.SplitLinesOnId(TwoLineFormatTestFile);

      var lineCount = File.ReadAllLines(TwoLineFormatTestFile).Where(l => l != "").ToArray().Length;

      Assert.IsTrue(lineCount == 2);
    }

    [TestMethod]
    [DeploymentItem(@"Test Files\TwoLineFormatTestFile.csv", "Test Files")]
    public void correct_superfluous_commas_in_lines()
    {
      CsvFileUtilities.AddPercentSignToLineIds(TwoLineFormatTestFile);
      CsvFileUtilities.MoveTextToOneLineForCleanup(TwoLineFormatTestFile);
      CsvFileUtilities.SplitLinesOnId(TwoLineFormatTestFile);
      CsvFileUtilities.RemoveSuperfluousCommas(TwoLineFormatTestFile);

      var lines = File.ReadAllLines(TwoLineFormatTestFile).Where(l => l != "").ToArray();
      var parts = lines[0].Split(',');

      Assert.IsTrue(parts.Length == 14);
    }

    [TestMethod]
    [DeploymentItem(@"Test Files\TwoLineFormatTestFile.csv", "Test Files")]
    public void remove_quotation_marks()
    {
      CsvFileUtilities.AddPercentSignToLineIds(TwoLineFormatTestFile);
      CsvFileUtilities.MoveTextToOneLineForCleanup(TwoLineFormatTestFile);
      CsvFileUtilities.SplitLinesOnId(TwoLineFormatTestFile);
      CsvFileUtilities.RemoveSuperfluousCommas(TwoLineFormatTestFile);
      CsvFileUtilities.RemoveQuotes(TwoLineFormatTestFile);

      var lines = File.ReadAllLines(TwoLineFormatTestFile).Where(l => l != "").ToArray();
      var quote = lines[0].IndexOf("\"", StringComparison.Ordinal);

      Assert.IsTrue(quote == -1);
    }
  }
}
