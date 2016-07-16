using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using ExportImplementation;

/// <summary>
/// Summary description for Export
/// </summary>


namespace EggwiseLib
{





    public static class Export
    {


        public struct ExportResult
        {

            public string fileName;
            public string extension;
            public string location;
            public string format;
            public bool success;

            public ExportResult(string format, string fileName, string extension, string location, bool success)
            {
                this.format = format;
                this.fileName = fileName;
                this.extension = extension;
                this.location = location;
                this.success = success;
            }

            public static ExportResult error()
            {
                return new ExportResult("", "", "", "", false);

            }

            public bool isSuccess()
            {
                return success;
            }
        }

        public static ExportResult exportDataTable(DataTable table, string format, string fileName, string path)
        {


            var formatMapping = new Dictionary
                <string, ExportToFormat>()
            {
                {"excel", ExportToFormat.Excel2007},
//                { "html", ExportToFormat.HTML },
                { "pdf", ExportToFormat.PDFiTextSharp4 },
                { "word", ExportToFormat.Word2007 },
                 { "ods", ExportToFormat.ODS },
                  { "odt", ExportToFormat.ODT }


            };
            if (!formatMapping.ContainsKey(format))
            {
         
                return ExportResult.error();
            }

            var extensionMapping = new Dictionary
             <string, string>()
            {
                {"excel", "xlsx"},
//                { "html", "html"},
                { "pdf", "pdf"},
                { "word", "docx"},
                { "ods", "ods"},
                { "odt", "odt"}

            };
            
            
            string extension = extensionMapping[format];
            string uploadPath = path + "\\" + fileName + "." + extensionMapping[format];
            Debug.WriteLine("upload to path" + uploadPath);

            try
            {
                var data = ExportFactory.ExportDataFromDataTable(table, formatMapping[format]);
                File.WriteAllBytes(uploadPath, data);

                return new ExportResult(format, fileName, extension, uploadPath, true);
            }
            catch (Exception e)
            {
                Debug.WriteLine("error: " + e.Message);
                throw;
            }


        }
      
    }

}

