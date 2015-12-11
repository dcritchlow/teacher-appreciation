using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using Microsoft.VisualBasic.FileIO;
using Microsoft.Web.Mvc;
using FieldType = Microsoft.Ajax.Utilities.FieldType;

namespace TeacherAppreciation.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Load(HttpPostedFileBase inputFile)
        {
            var entries = new DataTable();
            entries.Columns.Add("Submission_ID");
            entries.Columns.Add("Date_Submitted");
            entries.Columns.Add("Teacher_Full_Name");
            entries.Columns.Add("Teacher_Email_Address");
            entries.Columns.Add("Teacher_Phone");
            entries.Columns.Add("School");
            entries.Columns.Add("District");
            entries.Columns.Add("Current_Teaching_Position");
            entries.Columns.Add("Current_member");
            entries.Columns.Add("Question1");
            entries.Columns.Add("Question2");
            entries.Columns.Add("Submitter_Full_Name");
            entries.Columns.Add("Submitter_Email_Address");
            entries.Columns.Add("Submitter_Phone");

            var file = Path.GetFileName(inputFile.FileName);

            BinaryReader br = new BinaryReader(inputFile.InputStream);
            byte[] binaryData = br.ReadBytes(Convert.ToInt32(inputFile.InputStream.Length));
            System.IO.File.WriteAllBytes(Path.GetTempPath() + file, binaryData);

            var tempFile = Path.GetTempPath() + file;

            var lineId = "(^[0-9]{4,})";
            var makeOneLine = "\\s\\r?\\n";
            var splitLinesOnId = "%([0-9]{4,})";
            var replaceMultCommasWithOne = ",{2,}";
            System.IO.File.WriteAllText(tempFile, Regex.Replace(System.IO.File.ReadAllText(tempFile), lineId.ToString(), "%$1", RegexOptions.Multiline));
            System.IO.File.WriteAllText(tempFile, Regex.Replace(System.IO.File.ReadAllText(tempFile), makeOneLine.ToString(), "", RegexOptions.Multiline));
            System.IO.File.WriteAllText(tempFile, Regex.Replace(System.IO.File.ReadAllText(tempFile), splitLinesOnId.ToString(), "\r\n$1", RegexOptions.Multiline));
            System.IO.File.WriteAllText(tempFile, Regex.Replace(System.IO.File.ReadAllText(tempFile), replaceMultCommasWithOne.ToString(), ",", RegexOptions.Multiline));


            using (TextFieldParser parser = new TextFieldParser(tempFile))
            {
                parser.TextFieldType = Microsoft.VisualBasic.FileIO.FieldType.Delimited;
                parser.SetDelimiters(",");
                parser.HasFieldsEnclosedInQuotes = false;
                parser.TrimWhiteSpace = true;

                var count = 0;
                while (parser.PeekChars(1) != null)
                {
                    var cleanFieldRowCells = parser.ReadFields().Select(f => f.Trim(new[] { ' ', '"' }));

//                    var line = String.Join(" | ", cleanFieldRowCells);
                    
                    if (count != 0)
                    {
                        entries.Rows.Add(cleanFieldRowCells.ToArray());
                    }
                    count++;
                }
            }


            return PartialView();
        }

        protected ActionResult RedirectToAction<TController>(Expression<Action<TController>> action)
           where TController : Controller
        {
            return ControllerExtensions.RedirectToAction(this, action);
        }
    }
}