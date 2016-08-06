using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

//added
using System.Collections.Generic;
using System.Xml;
using System.Collections;
using System.Diagnostics;

namespace EggwiseLib
{
    public class HtmlObjects
    {
        private List<HtmlObject> oItems = new List<HtmlObject>();

        public List<HtmlObject> Items
        {
            get { return oItems; }
            set { oItems = value; }
        }

        public int Load(XmlNode d)
        {
            XmlNodeList nodes = d.SelectNodes("htmlobject");
            foreach (XmlNode node in nodes)
            {
                HtmlObject htmlobject = new HtmlObject();
                Headers h = new Headers();
                h.Load(node);
                htmlobject.Headers = h;
                Columns c = new Columns();
                c.Load(node);
                htmlobject.Columns = c;
                htmlobject.ID = node.Attributes.GetNamedItem("id") == null ? "" : node.Attributes.GetNamedItem("id").InnerText;
                htmlobject.HtmlType = node.Attributes.GetNamedItem("type").InnerText;
                htmlobject.Enabled = node.SelectSingleNode("enabled") == null ? "" : node.SelectSingleNode("enabled").InnerText;

                if (htmlobject.Enabled.ToLower() != "no")
                {
                    htmlobject.Enabled = "yes";
                }
                htmlobject.Mandatory = node.SelectSingleNode("mandatory") == null ? "" : node.SelectSingleNode("mandatory").InnerText;
                htmlobject.Datatype = node.SelectSingleNode("datatype") == null ? "" : node.SelectSingleNode("datatype").InnerText;
                htmlobject.DefValue = node.SelectSingleNode("defvalue") == null ? "" : node.SelectSingleNode("defvalue").InnerText;
                htmlobject.DbField = node.SelectSingleNode("dbfield") == null ? "" : node.SelectSingleNode("dbfield").InnerText;
                htmlobject.Label = node.SelectSingleNode("label") == null ? htmlobject.DbField : node.SelectSingleNode("label").InnerText;
                htmlobject.CssClass = node.SelectSingleNode("cssclass") == null ? "" : node.SelectSingleNode("cssclass").InnerText;
                htmlobject.List = node.SelectSingleNode("list") == null ? "" : node.SelectSingleNode("list").InnerText;
                htmlobject.Edit = node.SelectSingleNode("edit") == null ? "" : node.SelectSingleNode("edit").InnerText;
                htmlobject.Delete = node.SelectSingleNode("delete") == null ? "" : node.SelectSingleNode("delete").InnerText;
                htmlobject.Create = node.SelectSingleNode("create") == null ? "" : node.SelectSingleNode("create").InnerText; 
                htmlobject.Check = node.SelectSingleNode("check") == null ? "" : node.SelectSingleNode("check").InnerText;
                htmlobject.ColumnCheck = node.SelectSingleNode("columncheck") == null ? "" : node.SelectSingleNode("columncheck").InnerText;
                htmlobject.Direction = node.SelectSingleNode("direction") == null ? "" : node.SelectSingleNode("direction").InnerText;
                htmlobject.Action = node.SelectSingleNode("action") == null ? "" : node.SelectSingleNode("action").InnerText;
                htmlobject.Reload = node.SelectSingleNode("reload") == null ? "" : node.SelectSingleNode("reload").InnerText;
                htmlobject.Rows = node.SelectSingleNode("rows") == null ? 0 : int.Parse(node.SelectSingleNode("rows").InnerText);
                htmlobject.Width = node.SelectSingleNode("width") == null ? 0 : int.Parse(node.SelectSingleNode("width").InnerText);
                htmlobject.Height = node.SelectSingleNode("height") == null ? 0 : int.Parse(node.SelectSingleNode("height").InnerText);
                htmlobject.TabOrder = node.SelectSingleNode("taborder") == null ? 0 : int.Parse(node.SelectSingleNode("taborder").InnerText);
                htmlobject.Url = node.SelectSingleNode("url") == null ? "" : node.SelectSingleNode("url").InnerText;
                htmlobject.Sproc = node.SelectSingleNode("sproc") == null ? "" : node.SelectSingleNode("sproc").InnerText;
                htmlobject.FilePath = node.SelectSingleNode("filepath") == null ? "" : node.SelectSingleNode("filepath").InnerText;
                htmlobject.Delimeter = node.SelectSingleNode("delimeter") == null ? "" : node.SelectSingleNode("delimeter").InnerText;
                htmlobject.Display = node.SelectSingleNode("display") == null ? "" : node.SelectSingleNode("display").InnerText;
                htmlobject.Target = node.SelectSingleNode("target") == null ? "" : node.SelectSingleNode("target").InnerText;
                htmlobject.Loaddata = node.SelectSingleNode("loaddata") == null ? "" : node.SelectSingleNode("loaddata").InnerText;
                htmlobject.DbTable = node.SelectSingleNode("dbtable") == null ? "" : node.SelectSingleNode("dbtable").InnerText;
                htmlobject.DbKey = node.SelectSingleNode("dbkey") == null ? "" : node.SelectSingleNode("dbkey").InnerText;
                htmlobject.Navigate = node.SelectSingleNode("navigate") == null ? "" : node.SelectSingleNode("navigate").InnerText;
                htmlobject.Message = node.SelectSingleNode("message") == null ? "" : node.SelectSingleNode("message").InnerText;
                htmlobject.Length = node.SelectSingleNode("length") == null ? "" : node.SelectSingleNode("length").InnerText;

                htmlobject.Location = node.SelectSingleNode("location") == null ? "normal" : node.SelectSingleNode("location").InnerText;
                htmlobject.Icon = node.SelectSingleNode("icon") == null ? "" : node.SelectSingleNode("icon").InnerText;



                htmlobject.RowHeight = node.SelectSingleNode("rowheight") == null ? 0 : int.Parse(node.SelectSingleNode("rowheight").InnerText);
                //htmlobject.ListSize = node.SelectSingleNode("listsize") == null ? 4 : int.Parse(node.SelectSingleNode("listsize").InnerText);

                htmlobject.ExportFormat = node.SelectSingleNode("exportformat") == null ? "" : node.SelectSingleNode("exportformat").InnerText;
                htmlobject.Entity = node.SelectSingleNode("entity") == null ? "" : node.SelectSingleNode("entity").InnerText;

                oItems.Add(htmlobject);
            }
            return nodes.Count;
        }
    }

    public class Sections
    {
        private List<Section> oItems = new List<Section>();

        public List<Section> Items
        {
            get { return oItems; }
            set { oItems = value; }
        }

        public int Load(XmlDocument xmldoc)
        {
            XmlNodeList nodes = xmldoc.SelectNodes("//section");
            foreach (XmlNode node in nodes)
            {
                Section section = new Section();
                HtmlObjects h = new HtmlObjects();
                h.Load(node);
                section.HtmlObjects = h;
                section.ID = node.Attributes.GetNamedItem("id").InnerText;

                section.Class = null;
                section.Container = "yes";
                XmlNode classNode = node.SelectSingleNode("class");

                if (classNode != null)
                {
                    if (!string.IsNullOrEmpty(classNode.InnerText))
                    {
                       section.Class = classNode.InnerText; 
                    }
                    
                }

                XmlNode containerNode = node.SelectSingleNode("container");
                if (containerNode != null)
                {

                    if (!string.IsNullOrEmpty(containerNode.InnerText))
                    {
                        section.Container = containerNode.InnerText;
                    }
                }
            


                oItems.Add(section);
            }
            return nodes.Count;
        }
    }

    public class Section
    {
        private string sID;
        private HtmlObjects oHtmlObjects;
        private string sClass;
        private string sContainer;

        public string ID
        {
            get { return sID; }
            set { sID = value; }
        }


        public string Class
        {
            get { return sClass; }
            set { sClass = value; }
        }
        public string Container
        {
            get { return sContainer; }
            set { sContainer = value; }
        }

        public HtmlObjects HtmlObjects
        {
            get { return oHtmlObjects; }
            set { oHtmlObjects = value; }
        }
     }


    public class PageHeader
    {
        private List<MenuItm> menuItems;

        public static PageHeader load(XmlDocument xml)
        {
            
            PageHeader ph = new PageHeader();
            XmlNode headerNode = xml.SelectSingleNode("/form/pageheader");
            if (headerNode != null)
            {
                XmlNode titleNode = headerNode.SelectSingleNode("title");
                if (titleNode != null)
                {
                    ph.Title = titleNode.InnerText;
                }


                List<MenuItm> menuItems = new List<MenuItm>();
                XmlNodeList nodes = headerNode.ChildNodes;

                if (nodes != null && nodes.Count != 0)
                {
                    
                     foreach (XmlNode node in nodes)
                {

                    if (node.Name.ToLower().Equals("menuitem"))
                    {
                        MenuItm ret = new MenuItm();
                        menuItems.Add(ret.fromNode(node));

                    }
                    
                    
                }
                ph.MenuItms = menuItems;

                }
               



            }

            return ph;

        }
        private string sTitle;

        public string Title
        {
            get { return sTitle; }
            set { sTitle = value; }
        }

        public List<MenuItm> MenuItms
        {
            get { return menuItems; }
            set { menuItems = value; }
        }

    }

    public class PageForm
    {
        private string sID;
        private Sections oSections;
        private DataBase oDataBase;
        private Audit oAudit;
        private string sActionType;
        private string sPrimaryKey;
        private string sTitle;
        private string sAccess;

        public string ID
        {
            get { return sID; }
            set { sID = value; }
        }

        public string Title
        {
            get { return sTitle; }
            set { sTitle = value; }
        }

        public string Access
        {
            get { return sAccess; }
            set { sAccess = value; }
        }

        public DataBase Database
        {
            get { return oDataBase; }
            set { oDataBase = value; }
        }

        public Audit Audit
        {
            get { return oAudit; }
            set { oAudit = value; }
        }

        public Sections Sections
        {
            get { return oSections; }
            set { oSections = value; }
        }

        public string ActionType
        {
            get { return sActionType; }
            set { sActionType = value; }
        }

        public string PrimaryKey
        {
            get { return sPrimaryKey; }
            set { sPrimaryKey = value; }
        }

        public void Load(XmlDocument xmldoc)
        {
            Sections sections = new Sections();
            DataBase database = new DataBase();
            Audit audit = new Audit();
            string action = string.Empty;
            string title = string.Empty;
            string access = string.Empty;
            string notification = string.Empty;

            sections.Load(xmldoc);

            try
            {
                XmlNode node = xmldoc.SelectSingleNode("form/database");
                if (node != null)
                {
                    database.Table = node.SelectSingleNode("table") == null ? "" : node.SelectSingleNode("table").InnerText;
                    database.Condition = node.SelectSingleNode("condition") == null ? "" : node.SelectSingleNode("condition").InnerText;
                    database.Order = node.SelectSingleNode("order") == null ? "" : node.SelectSingleNode("order").InnerText;
                    database.PrimaryKey = node.SelectSingleNode("primarykey") == null ? "" : node.SelectSingleNode("primarykey").InnerText;
                    database.Connection = node.SelectSingleNode("connection") == null ? "" : node.SelectSingleNode("connection").InnerText;
                }
                action = xmldoc.SelectSingleNode("//actiontype").InnerText;
                title = xmldoc.SelectSingleNode("//title").InnerText;
                access = xmldoc.SelectSingleNode("//access").InnerText;

                node = xmldoc.SelectSingleNode("form/audit");
                audit.Table = node == null ? "" : node.SelectSingleNode("table").InnerText;
                audit.View = node == null ? "" : node.SelectSingleNode("view").InnerText;
                audit.Enabled = node == null ? false : node.SelectSingleNode("enabled").InnerText.ToLower() == "yes" ? true : false;

                sAccess = access;
                sTitle = title;
                sActionType = action;
                oSections = sections;
                oDataBase = database;
                oAudit = audit;
            }
            catch (System.Exception ex)
            {
                ErHandler.errorMsg = "Source:" + ex.Source + "\n" + "Message:" + ex.Message;
                ErHandler.throwError();
            }
        }
    }

    public class HtmlObject
    {
        private string sID;
        private string sHtmlType;
        private string sLabel;
        private string sEnabled;
        private string sMandatory;
        private string sDatatype;
        private string sDefValue;
        private string sDbField;
        private string sCssClass;
        private string sList;
        private int iWidth;
        private int iHeight;
        private int iTabOrder;
        private Headers oHeaders;
        private Columns oColumns;
        private string sEdit;
        private string sDelete;
        private string sCreate;
        private string sCheck;
        private string sColumnCheck;
        private string sDirection;
        private string sAction;
        private string sReload;
        private string sUrl;
        private string sSproc;
        private string sFilePath;
        private string sDelimeter;
        private string sDisplay;
        private string sTarget;
        private int iRows;
        private string sLoaddata;
        private string sDbTable;
        private string sDbKey;
        private string sNavigate;
        private string sMessage;
        private string sLocation;
        private string sIcon;
        private string sLength;
        //private int iListSize;
        private int iRowHeight;


        public string Length
        {
            get { return sLength; }
            set { sLength = value; }
        }


        public string Icon
        {
            get { return sIcon; }
            set { sIcon = value; }
        }

        public string Location
        {
            get { return sLocation; }
            set { sLocation = value; }
        }
        public string ID
        {
            get { return sID; }
            set { sID = value; }
        }
        public string HtmlType
        {
            get { return sHtmlType; }
            set { sHtmlType = value; }
        }

        public string Label
        {
            get { return sLabel; }
            set { sLabel = value; }
        }

        public string Display
        {
            get { return sDisplay; }
            set { sDisplay = value; }
        }

        public string Target
        {
            get { return sTarget; }
            set { sTarget = value; }
        }

        public string Url
        {
            get { return sUrl; }
            set { sUrl = value; }
        }

        public string Delimeter
        {
            get { return sDelimeter; }
            set { sDelimeter = value; }
        }

        public string Direction
        {
            get { return sDirection; }
            set { sDirection = value; }
        }

        public string Enabled
        {
            get { return sEnabled; }
            set { sEnabled = value; }
        }

        public string Loaddata
        {
            get { return sLoaddata; }
            set { sLoaddata = value; }
        }

        public string Mandatory
        {
            get { return sMandatory; }
            set { sMandatory = value; }
        }

        public string Action
        {
            get { return sAction; }
            set { sAction = value; }
        }

        public string Reload
        {
            get { return sReload; }
            set { sReload = value; }
        }

        public string Datatype
        {
            get { return sDatatype; }
            set { sDatatype = value; }
        }

        public string Edit
        {
            get { return sEdit; }
            set { sEdit = value; }
        }

        public string Delete
        {
            get { return sDelete; }
            set { sDelete = value; }
        }
        
        public string Create
        {
            get { return sCreate; }
            set { sCreate = value; }
        }

        public string Check
        {
            get { return sCheck; }
            set { sCheck = value; }
        }

        public string ColumnCheck
        {
            get { return sColumnCheck; }
            set { sColumnCheck = value; }
        }

        public string DefValue
        {
            get { return sDefValue; }
            set { sDefValue = value; }
        }

        public string DbField
        {
            get { return sDbField; }
            set { sDbField = value; }
        }

        public string CssClass
        {
            get { return sCssClass; }
            set { sCssClass = value; }
        }

        public string List
        {
            get { return sList; }
            set { sList = value; }
        }

        public int Width
        {
            get { return iWidth; }
            set { iWidth = value; }
        }

        public int Rows
        {
            get { return iRows; }
            set { iRows = value; }
        }

        public int Height
        {
            get { return iHeight; }
            set { iHeight = value; }
        }


        public int RowHeight
        {
            get { return iRowHeight; }
            set { iRowHeight = value; }
        }

        public int TabOrder
        {
            get { return iTabOrder; }
            set { iTabOrder = value; }
        }

        public Headers Headers
        {
            get { return oHeaders; }
            set { oHeaders = value; }
        }

        public string Sproc
        {
            get { return sSproc; }
            set { sSproc = value; }
        }

        public string FilePath
        {
            get { return sFilePath; }
            set { sFilePath = value; }
        }

        public string DbTable
        {
            get { return sDbTable; }
            set { sDbTable = value; }
        }

        public string DbKey
        {
            get { return sDbKey; }
            set { sDbKey = value; }
        }

        public string Navigate
        {
            get { return sNavigate; }
            set { sNavigate = value; }
        }

        public string Message
        {
            get { return sMessage; }
            set { sMessage = value; }
        }
        public Columns Columns
        {
            get { return oColumns; }
            set { oColumns = value; }
        }

        public string ExportFormat { get; set; }


        public string Entity { get; set; }
        //public int ListSize
        //{
        //    get { return iListSize; }
        //    set { iListSize = value; }
        //}

    }

    public class VersionInfo
    {
        public VersionInfo(XmlDocument xmlDoc)
        {
            doc = xmlDoc;
        }
        private XmlDocument doc;
        private string sVersion;
        private string sAuthor;
        private string sDate;
        private string sCopyright;
        private string sErrorLog;
        private string sMaxRows;
        private string sEncryption;
        private Scripts oScripts;
       

        public Scripts Scripts
        {
            get { return oScripts; }
            set { oScripts = value; }
        }

        public string Version
        {
            get
            {
                XmlNode n = doc.SelectSingleNode("//version");
                return n.InnerText;
            }
            set
            {
                sVersion = value;
            }
        }

        public string ErrorLog
        {
            get
            {
                XmlNode n = doc.SelectSingleNode("//errorlog");
                return n.InnerText;
            }
            set
            {
                sErrorLog = value;
            }
        }

        public string MaxRows
        {
            get
            {
                XmlNode n = doc.SelectSingleNode("//maxrows");
                return n.InnerText;
            }
            set
            {
                sMaxRows = value;
            }
        }

        public string Author
        {
            get
            {
                XmlNode n = doc.SelectSingleNode("//author");
                return n.InnerText;
            }
            set
            {
                sAuthor = value;
            }
        }

        public string Date
        {
            get
            {
                XmlNode n = doc.SelectSingleNode("//date");
                return n.InnerText;
            }
            set
            {
                sDate = value;
            }
        }

        public string Copyright
        {
            get
            {
                XmlNode n = doc.SelectSingleNode("//copyright");
                return n.InnerText;
            }
            set
            {
                sCopyright = value;
            }
        }

        public string Encryption
        {
            get
            {
                XmlNode n = doc.SelectSingleNode("//encryption");
                return n.InnerText;
            }
            set
            {
                sEncryption = value;
            }
        }
    }

    public class DataBase
    {
        private string sConnection;
        private string sTable;
        private string sCondition;
        private string sOrder;
        private string sPrimaryKey;

        public string Table
        {
            get { return sTable; }
            set { sTable = value; }
        }

        public string Connection
        {
            get { return sConnection; }
            set { sConnection = value; }
        }

        public string Condition
        {
            get { return sCondition; }
            set { sCondition = value; }
        }

        public string Order
        {
            get { return sOrder; }
            set { sOrder = value; }
        }

        public string PrimaryKey
        {
            get { return sPrimaryKey; }
            set { sPrimaryKey = value; }
        }
    }

    public class MenuItm
    {

        private string sText;
        private string sModule;
        private int iLevel;
        private string sSecurity;


        public MenuItm fromNode(XmlNode node)
        {

            MenuText = node.SelectSingleNode("menutext") == null ? "" : node.SelectSingleNode("menutext").InnerText;
            Security = node.SelectSingleNode("security") == null ? "" : node.SelectSingleNode("security").InnerText;
            Module = node.SelectSingleNode("module") == null ? "" : node.SelectSingleNode("module").InnerText;
           

           
            foreach(XmlNode n in node.ChildNodes)
            {

        
                if (n.Name.ToLower().Equals("menuitem"))
                {
                    MenuItm m = new MenuItm();
                    oItems.Add(m.fromNode(n));
                }

            }

            return this;

        }

        private List<MenuItm> oItems = new List<MenuItm>();

        public List<MenuItm> Children
        {
            get { return oItems; }
            set { oItems = value; }
        }

        public string MenuText

        {
            get { return sText; }
            set { sText = value; }
        }


        public string Module
        {
            get { return sModule; }
            set { sModule = value; }
        }

        public int Level
        {
            get { return iLevel; }
            set { iLevel = value; }
        }

        public string Security
        {
            get { return sSecurity; }
            set { sSecurity = value; }
        }

    }




    public class MenuItms
    {
        private List<MenuItm> oItems = new List<MenuItm>();

        public List<MenuItm> Items
        {
            get { return oItems; }
            set { oItems = value; }
        }

        public int Load(XmlDocument xmldoc)
        {

            XmlNodeList nodes = xmldoc.SelectNodes("/menu/*");
            foreach (XmlNode node in nodes)
            {
  
                if (node.Name.ToLower().Equals("menuitem"))
                {
                MenuItm ret = new MenuItm();
                oItems.Add(ret.fromNode(node));

                }
    
//                ret.MenuText = node.SelectSingleNode("menutext").InnerText;
//                ret.Module = node.SelectSingleNode("module").InnerText;
//                ret.Level = int.Parse(node.Attributes.GetNamedItem("level").InnerText);
//                ret.Security = node.SelectSingleNode("security").InnerText;
//                oItems.Add(ret);
            }
            return nodes.Count;
        }
    }

    


    public class Header
    {

        private string sID;
        private string sName;


        public string ID
        {
            get { return sID; }
            set { sID = value; }
        }

        public string Name
        {
            get { return sName; }
            set { sName = value; }
        }
    }

    public class Column
    {

        private string sID;
        private string sDbField;
        private string sDataType;
        private string sType;
        private string sWidth;
        private string sHeight;
        private string sCssClass;
        private string sList;

        public string ID
        {
            get { return sID; }
            set { sID = value; }
        }
        
        public string DataType
        {
            get { return sDataType; }
            set { sDataType = value; }
        }

        public string Type
        {
            get { return sType; }
            set { sType = value; }
        }

        public string List
        {
            get { return sList; }
            set { sList = value; }
        }

        public string DbField
        {
            get { return sDbField; }
            set { sDbField = value; }
        }

        public string Width
        {
            get { return sWidth; }
            set { sWidth = value; }
        }

        public string Height
        {
            get { return sHeight; }
            set { sHeight = value; }
        }

        public string CssClass
        {
            get { return sCssClass; }
            set { sCssClass = value; }
        }
    }




    public class Headers
    {
        private List<Header> oItems = new List<Header>();

        public List<Header> Items
        {
            get { return oItems; }
            set { oItems = value; }
        }

        public int Load(XmlNode d)
        {
            XmlNode h = d.SelectSingleNode("headers");
            if (h != null)
            {
                if (h.InnerText == "yes")
                {
                    Header head = new Header();
                    head.Name = "yes";
                    oItems.Add(head);
                }
            }

            XmlNodeList nodes = d.SelectNodes("headers/header");
            foreach (XmlNode node in nodes)
            {
                Header header = new Header();
                header.ID = node.Attributes.GetNamedItem("id") == null ? "" : node.Attributes.GetNamedItem("id").InnerText;
                header.Name = node.SelectSingleNode("name") == null ? "" : node.SelectSingleNode("name").InnerText;
                oItems.Add(header);
            }

            return nodes.Count;
        }
    }

    public class Columns
    {
        private List<Column> oItems = new List<Column>();

        public List<Column> Items
        {
            get { return oItems; }
            set { oItems = value; }
        }

        public int Load(XmlNode d)
        {
            XmlNodeList nodes = d.SelectNodes("columns/column");
            foreach (XmlNode node in nodes)
            {
                Column column = new Column();
                column.ID = node.Attributes.GetNamedItem("id") == null ? "" : node.Attributes.GetNamedItem("id").InnerText;
                column.DbField = node.SelectSingleNode("dbfield") == null ? "NULL" : node.SelectSingleNode("dbfield").InnerText;
                column.DataType = node.SelectSingleNode("datatype") == null ? "" : node.SelectSingleNode("datatype").InnerText;
                column.CssClass = node.SelectSingleNode("cssclass") == null ? "" : node.SelectSingleNode("cssclass").InnerText;
                column.Width = node.SelectSingleNode("width") == null ? "" : node.SelectSingleNode("width").InnerText;
                column.Height = node.SelectSingleNode("height") == null ? "" : node.SelectSingleNode("height").InnerText;
                column.Type = node.Attributes.GetNamedItem("type") == null ? "" : node.Attributes.GetNamedItem("type").InnerText;
                column.List = node.SelectSingleNode("list") == null ? "" : node.SelectSingleNode("list").InnerText;
                oItems.Add(column);
            }
            return nodes.Count;
        }
    }


    public class Script
    {
        private string sType;
        private string sInclude;

        public string Type
        {
            get { return sType; }
            set { sType = value; }
        }

        public string Include
        {
            get { return sInclude; }
            set { sInclude = value; }
        }
    }


    public class Scripts
    {
        private List<Script> oItems = new List<Script>();

        public List<Script> Items
        {
            get { return oItems; }
            set { oItems = value; }
        }

        public int Load(XmlNode d)
        {
            XmlNodeList nodes = d.SelectNodes("//scripts/script");
            foreach (XmlNode node in nodes)
            {
                Script script = new Script();
                script.Type = node.Attributes.GetNamedItem("type") == null ? "" : node.Attributes.GetNamedItem("type").InnerText;
                script.Include = node.SelectSingleNode("include") == null ? "" : node.SelectSingleNode("include").InnerText;
                oItems.Add(script);
            }
            return nodes.Count;
        }
    }

    public class Audit
    {
        private string sTable;
        private string sView;
        private bool bEnabled;

        public string Table
        {
            get { return sTable; }
            set { sTable = value; }
        }

        public string View
        {
            get { return sView; }
            set { sView = value; }
        }

        public bool Enabled
        {
            get { return bEnabled; }
            set { bEnabled = value; }
        }

    }

    public class QString
    {
        private string sPageform;
        private string sFilter;
        private string sOrder;
        private string sDirection;

        public string Pageform
        {
            get { return sPageform; }
            set { sPageform = value; }
        }

        public string Filter
        {
            get { return sFilter; }
            set { sFilter = value; }
        }

        public string Order
        {
            get { return sOrder; }
            set { sOrder = value; }
        }

        public string Direction
        {
            get { return sDirection; }
            set { sDirection = value; }
        }

    }

    public class Usr
    {
        private List<string> lRoles;
        private bool bRead;
        private bool bCreate;
        private bool bDelete;
        private bool bUpdate;
        private bool bAdmin;
        private bool bAllowed;
        private bool bAuthorize;
        private string sUsr;
        private string sDepartment;

        public bool Read
        {
            get { return bRead; }
            set { bRead = value; }
        }

        public bool Create
        {
            get { return bCreate; }
            set { bCreate = value; }
        }

        public bool Delete
        {
            get { return bDelete; }
            set { bDelete = value; }
        }

        public bool Update
        {
            get { return bUpdate; }
            set { bUpdate = value; }
        }

        public bool Authorize
        {
            get { return bAuthorize; }
            set { bAuthorize = value; }
        }

        public bool Admin
        {
            get { return bAdmin; }
            set { bAdmin = value; }
        }

        public bool Allowed
        {
            get { return bAllowed; }
            set { bAllowed = value; }
        }

        public string User
        {
            get { return sUsr; }
            set { sUsr = value; }
        }

        public string Department
        {
            get { return sDepartment; }
            set { sDepartment = value; }
        }

        public List<string> Roles
        {
            get { return lRoles; }
            set { lRoles = value; }
        }        
    }
}
