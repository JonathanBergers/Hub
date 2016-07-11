
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

//manually added
using System.Xml;
using System.Xml.Schema;
using System.Text;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Data.SqlClient;

namespace EggwiseLib
{
        public class Mapping
        {

            private List<ExcelColumn> _Columns = new List<ExcelColumn>();
            private string _Table;
            private string _Preload;
            private XmlDocument _doc = new XmlDocument();

            public void Load()
            {

                string f = HttpContext.Current.Server.MapPath(@"~/mapping/" + SessionHandler.Qstring.Pageform + ".map");

                if (File.Exists(f))
                {
                    _doc.Load(f);
                    _Table = _doc.SelectSingleNode("//table") == null ? "" : _doc.SelectSingleNode("//table").InnerText;
                    _Preload = _doc.SelectSingleNode("//preload") == null ? "" : _doc.SelectSingleNode("//preload").InnerText;
  

                    XmlNodeList nodes = _doc.SelectNodes("//columns/column");
                    foreach (XmlNode node in nodes)
                    {
                        ExcelColumn column = new ExcelColumn();
                        column.ColumnOrder = node.SelectSingleNode("columnorderfile") == null ? 0 : int.Parse(node.SelectSingleNode("columnorderfile").InnerText);
                        column.DefValue = node.SelectSingleNode("defvalue") == null ? "" : node.SelectSingleNode("defvalue").InnerText;
                        column.DataType = node.SelectSingleNode("datatype") == null ? "" : node.SelectSingleNode("datatype").InnerText;
                        column.Field = node.SelectSingleNode("field") == null ? "" : node.SelectSingleNode("field").InnerText;
                        column.Length = node.SelectSingleNode("length") == null ? 0 : int.Parse(node.SelectSingleNode("length").InnerText);
                        column.Format = node.SelectSingleNode("format") == null ? "" : node.SelectSingleNode("format").InnerText;
                        _Columns.Add(column);
                    }
                }
            }

            public List<ExcelColumn> Columns
            {
                get { return _Columns; }
                set { _Columns = value; }
            }

            public string Table
            {
                get { return _Table; }
                set { _Table = value; }
            }

            public string Preload
            {
                get { return _Preload; }
                set { _Preload = value; }
            }

        }

        public class ExcelColumn
        {

            private int _ColumnOrder;
            private string _Field;
            private string _DataType;
            private string _Format;
            private string _DefValue;
            private int _Length;

            public int ColumnOrder
            {
                get { return _ColumnOrder; }
                set { _ColumnOrder = value; }
            }

            public string DataType
            {
                get { return _DataType; }
                set { _DataType = value; }
            }

             public string DefValue
            {
                get { return _DefValue; }
                set { _DefValue = value; }
            }
            public string Format
            {
                get { return _Format; }
                set { _Format = value; }
            }

            public string Field
            {
                get { return _Field; }
                set { _Field = value; }
            }

            public int Length
            {
                get { return _Length; }
                set { _Length = value; }
            }

        }
}