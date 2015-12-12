using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using Microsoft.VisualBasic.FileIO;
using Microsoft.Web.Mvc;
using TeacherAppreciation.Infrastructure;
using TeacherAppreciation.Models;

namespace TeacherAppreciation.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Entries(HttpPostedFileBase inputFile)
        {
            var entries = BuildEntriesDataTable();

            var tempFilePath = WriteDataToTempFile(inputFile);

            CleanUpBadlyFormattedCsvFile(tempFilePath);

            var modelList = GetEntries(tempFilePath, entries);

            return View(modelList);
        }

        /// <summary>
        /// Convenience Method for using MVC Futures RedirectToAction without this
        /// </summary>
        /// <typeparam name="TController"></typeparam>
        /// <param name="action"></param>
        /// <returns></returns>
        protected ActionResult RedirectToAction<TController>(Expression<Action<TController>> action)
           where TController : Controller
        {
            return ControllerExtensions.RedirectToAction(this, action);
        }

        #region Private Methods
        /// <summary>
        /// Parse the csv file fields
        /// </summary>
        /// <param name="tempFilePath"></param>
        /// <param name="entries"></param>
        /// <returns></returns>
        private List<TeacherAppreciationForm> GetEntries(string tempFilePath, DataTable entries)
        {
            using (TextFieldParser parser = new TextFieldParser(tempFilePath))
            {
                parser.TextFieldType = Microsoft.VisualBasic.FileIO.FieldType.Delimited;
                parser.SetDelimiters(",");
                parser.HasFieldsEnclosedInQuotes = false;
                parser.TrimWhiteSpace = true;

                var count = 0;
                while (parser.PeekChars(1) != null)
                {
                    string[] rowCells = parser.ReadFields();

                    if (count != 0)
                    {
                        entries.Rows.Add(Utilities.CheckNotNull(rowCells));
                    }
                    count++;
                }
            }
            var modelList = Utilities.ConvertToList<TeacherAppreciationForm>(entries);
            return modelList;
        }

        /// <summary>
        /// Clean up badly formatted csv file
        /// </summary>
        /// <param name="tempFilePath"></param>
        private void CleanUpBadlyFormattedCsvFile(string tempFilePath)
        {
            var lineId = "(^[0-9]{4,})";
            var makeOneLine = "\\r?\\n";
            var splitLinesOnId = "%([0-9]{4,})";
            var replaceMultCommas = ",{2,}";
            var removeQuotes = "\"?";
            System.IO.File.WriteAllText(tempFilePath,
                Regex.Replace(System.IO.File.ReadAllText(tempFilePath), lineId, "%$1", RegexOptions.Multiline));
            System.IO.File.WriteAllText(tempFilePath,
                Regex.Replace(System.IO.File.ReadAllText(tempFilePath), makeOneLine, "", RegexOptions.Multiline));
            System.IO.File.WriteAllText(tempFilePath,
                Regex.Replace(System.IO.File.ReadAllText(tempFilePath), splitLinesOnId, "\r\n$1",
                    RegexOptions.Multiline));
            System.IO.File.WriteAllText(tempFilePath,
                Regex.Replace(System.IO.File.ReadAllText(tempFilePath), replaceMultCommas, "", RegexOptions.Multiline));
            System.IO.File.WriteAllText(tempFilePath,
                Regex.Replace(System.IO.File.ReadAllText(tempFilePath), removeQuotes, "", RegexOptions.Multiline));
        }

        /// <summary>
        /// Write file inputstream to temp file in users Temporary file path
        /// </summary>
        /// <param name="inputFile"></param>
        /// <returns></returns>
        private string WriteDataToTempFile(HttpPostedFileBase inputFile)
        {
            var file = Path.GetFileName(inputFile.FileName);

            BinaryReader br = new BinaryReader(inputFile.InputStream);
            byte[] binaryData = br.ReadBytes(Convert.ToInt32(inputFile.InputStream.Length));
            System.IO.File.WriteAllBytes(Path.GetTempPath() + file, binaryData);
            return Path.GetTempPath() + file;
        }

        /// <summary>
        /// Build datatable to hold submission entries
        /// </summary>
        /// <returns></returns>
        private DataTable BuildEntriesDataTable()
        {
            var entries = new DataTable();
            entries.Columns.Add("SubmissionId");
            entries.Columns.Add("DateSubmitted");
            entries.Columns.Add("TeachersFullName");
            entries.Columns.Add("TeachersEmailAddress");
            entries.Columns.Add("TeachersPhone");
            entries.Columns.Add("School");
            entries.Columns.Add("District");
            entries.Columns.Add("CurrentTeachingPosition");
            entries.Columns.Add("CurrentMember");
            entries.Columns.Add("Question1");
            entries.Columns.Add("Question2");
            entries.Columns.Add("SubmittersFullName");
            entries.Columns.Add("SubmittersEmailAddress");
            entries.Columns.Add("SubmittersPhone");
            return entries;
        }

        #endregion
        
    }
}