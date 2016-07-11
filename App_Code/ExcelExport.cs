using System;
using System.Data;
using System.Diagnostics;
using System.IO;
using ExportImplementation;

/// <summary>
/// Summary description for ExcelExport
/// </summary>


namespace EggwiseLib
{

    public static class ExcelExport
    {

        public static void exportDataTable(DataTable table)
        {


            string path = "C:\\Users\\windowsucks\\Desktop\\hub_excel_test";
            Debug.WriteLine("current directory" + path);

            try
            {
                var data = ExportFactory.ExportDataFromDataTable(table, ExportToFormat.Excel2007);
                File.WriteAllBytes(path + "/test.xlsx", data);
            }
            catch (Exception e)
            {
                Debug.WriteLine("error: " + e.Message);
                throw;
            }


        }
      
    }

}

