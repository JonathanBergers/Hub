using System;
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
using EggwiseLib;
using System.Data.OleDb;


//to be removed
using System.Diagnostics;
using System.Globalization;

public partial class index : BasePage  //System.Web.UI.Page
{
    // the configuration files
    private XmlDocument doc = new XmlDocument();
    private XmlDocument ver = new XmlDocument();
    private XmlDocument men = new XmlDocument();

    // refresh prop
    bool pageformchanged = true;
    protected void Page_Init(object sender, EventArgs e)
    {
        // I don't use viewstate variable
        // EnableEventValidation="false" is also put on page

        this.EnableViewState = false;


        // create navigation object
        if (SessionHandler.Navigation == null)
        {
            SessionHandler.Navigation = new Navigation();
        }

        # region query parameters
        // get query parameters

        if (Request.QueryString.Count > 0)
        {
            if (SessionHandler.Qstring.Pageform == Request.QueryString.Get(0))
            {
                pageformchanged = false;
            }
            else
            {
                pageformchanged = true;
            }

            QString qs = new QString();
            qs.Filter = Request.QueryString["filter"] == null ? "nofilter" : Request.QueryString["filter"];
            qs.Order = Request.QueryString["order"] == null ? "1" : Request.QueryString["order"];
            qs.Pageform = Request.QueryString["page"];
            qs.Direction = Request.QueryString["direction"] == null ? "desc" : Request.QueryString["direction"];
            SessionHandler.Qstring = qs;
        }
        else
        {
            // defaults
            QString qs = new QString();
            qs.Filter = "nofilter";
            qs.Order = "1";
            //qs.Pageform = "login";
            qs.Pageform = "home";
            qs.Direction = "desc";
            SessionHandler.Qstring = qs;

            // empty user
            Usr u = new Usr();
            List<string> l = new List<string>();
            l.Add("");
            u.Roles = l;
            u.User = "";
            SessionHandler.Usr = u;

        }

        // feedback execute
        if (Request.QueryString["executed"] != "yes")
        {

            //reset error msg
            ErHandler.clearError();
        }
        else
        {
            // Alert
            if (ErHandler.errorMsg != "")
            {
                Alert.Show(ErHandler.errorMsg.Replace("\n", ""));
                //reset error msg
                ErHandler.clearError();
            }
        }

        if (Request.QueryString["confirm"] == "yes")
        {
            ErHandler.errorMsg = "Are you sure you want to delete this item?";
        }


        # endregion

        # region path
        // version file path
        string versionFile = Server.MapPath(@"~/config/app/version.prop");
        // the config file
        string configFile = Server.MapPath(@"~/config/" + SessionHandler.Qstring.Pageform + ".prop");
        string xsdFile = Server.MapPath(@"~/config/prop.xsd");
        // menufile
        string menuItemFile = Server.MapPath(@"~/config/app/menu.prop");
        //string xsdMenu = Server.MapPath("~/config/menu.xsd");
        # endregion

        #region version
        if (Application["version"] == null)
        {
            // load the version into ver
            ver.Load(versionFile);
            Application["version"] = ver;
        }
        else
        {
            ver = (XmlDocument)Application["version"];
        }
        # endregion

        # region scriptmanager
        // scriptmanager
        Type cstype = Page.GetType();
        ClientScriptManager cs = Page.ClientScript;
        # endregion

        #region add main panels

        // HIER WORDEN DE PANELS GEMAAKT WAARIN DE HTML OBJECTEN WORDEN GEGENEREERD.


        //add comment
        frm.Controls.Add(CreateHTMLObjects.CreatePageComments());

      

        //MENU
        // add menu
        HtmlGenericControl m = new HtmlGenericControl("nav");
        m.ID = "menu";
        m.Attributes["class"] = "menu";
        m.Style.Add("padding-left", "240px");
        frm.Controls.Add(m);

        //sideMenu
        HtmlGenericControl sideMenu = new HtmlGenericControl("div");
        sideMenu.ID = "sideMenu";
        sideMenu.Attributes["class"] = "sideMenu";
        frm.Controls.Add(sideMenu);
    

        //add nav item       class="right
        //HtmlGenericControl nav = new HtmlGenericControl("nav");
        //frm.Controls.Add(CreateHTMLObjects.CreateNewLine(1, 8));
        //nbf.Controls.Add(nav);

        // add menu
        //HtmlGenericControl m = new HtmlGenericControl("div");
        //HtmlGenericControl mWrapper = new HtmlGenericControl("div");
        //mWrapper.Attributes["class"] = "nav-wrapper";
        //m.Controls.Add(mWrapper);
        //m.ID = "menu";
        //m.Attributes["class"] = "menu";
        //m.Attributes["class"] = "nav-wrapper";
        //nav.Controls.Add(CreateHTMLObjects.CreateNewLine(1, 8));
        //nav.Controls.Add(m);

        // add filler
        HtmlGenericControl fl = new HtmlGenericControl("div");
        fl.ID = "filler";
        frm.Controls.Add(CreateHTMLObjects.CreateNewLine(1, 8));
        frm.Controls.Add(fl);

        // main div
        HtmlGenericControl cont = new HtmlGenericControl("div");
        cont.ID = "content";
//        cont.Attributes["class"] = "container";
        cont.Style.Add("padding-left", "240px");
        cont.Style.Add("min-height", "100%");
        frm.Controls.Add(CreateHTMLObjects.CreateNewLine(1, 8));
        frm.Controls.Add(cont);

        // add filler
       // HtmlGenericControl flr = new HtmlGenericControl("div");
      //  flr.ID = "fillerright";
      //  frm.Controls.Add(CreateHTMLObjects.CreateNewLine(1, 8));
      //  frm.Controls.Add(flr);

        // extra div
     //   HtmlGenericControl contr = new HtmlGenericControl("div");
      //  contr.ID = "rightpanel";
     //  contr.Attributes["class"] = "content";
       // frm.Controls.Add(CreateHTMLObjects.CreateNewLine(1, 8));
      //  frm.Controls.Add(contr);

        // extra div
      //  HtmlGenericControl floatingdiv = new HtmlGenericControl("div");
      //  floatingdiv.ID = "floatingdiv";
     //   floatingdiv.Attributes["class"] = "floatingdiv";
      //  frm.Controls.Add(CreateHTMLObjects.CreateNewLine(1, 8));
     //   frm.Controls.Add(floatingdiv);

        // extra div
        //HtmlGenericControl floatingdivcont = new HtmlGenericControl("div");
        //floatingdivcont.ID = "floatingdivcont";
        //floatingdivcont.Attributes["class"] = "floatingdiv";
        //frm.Controls.Add(CreateHTMLObjects.CreateNewLine(1, 8));
        //frm.Controls.Add(floatingdivcont);

        // extra div
        HtmlGenericControl floatingtitle = new HtmlGenericControl("div");
        floatingtitle.ID = "floatingtitle";
        floatingtitle.Attributes["class"] = "floatingdiv";
        frm.Controls.Add(CreateHTMLObjects.CreateNewLine(1, 8));
        frm.Controls.Add(floatingtitle);

        // page title
        Literal pagetitle = new Literal();
        cont.Controls.Add(CreateHTMLObjects.CreateNewLine(1, 8));
        cont.Controls.Add(pagetitle);

        // waitstreen
        HtmlGenericControl wait = new HtmlGenericControl("div");
        wait.ID = "wait";
        wait.Attributes["class"] = "gray";
        frm.Controls.Add(CreateHTMLObjects.CreateNewLine(1, 8));
        frm.Controls.Add(wait);

        HtmlGenericControl sandbox = new HtmlGenericControl("div");
        sandbox.ID = "sandbox";
        sandbox.Attributes["class"] = "sandbox";
        frm.Controls.Add(CreateHTMLObjects.CreateNewLine(1, 8));
        frm.Controls.Add(sandbox);

        # endregion


#region add locale

        cont.Controls.Add(new LiteralControl("<script> var client_locale = " + System.Threading.Thread.CurrentThread.CurrentUICulture + "; </script>"));
#endregion


        # region load files
        try
        {
            if (pageformchanged || SessionHandler.propFile == null)
            {
                using (Stream s = new FileStream(configFile, 
                        FileMode.Open,
                        FileAccess.Read,
                        FileShare.ReadWrite))
                {
                    doc.Load(new StreamReader(s));
                    s.Close();
                }

                //doc.Load(configFile);
                ErHandler.errorMsg = ValidateXML.ValidatingXML(configFile, xsdFile);
                SessionHandler.propFile = doc;
            }
            else
            {
                doc = SessionHandler.propFile;

            }

            if (SessionHandler.menuFile == null)
            {
                men.Load(menuItemFile);
            }
            else
            {
                men = SessionHandler.menuFile;
            }
        }
        catch
        {
            SessionHandler.Qstring.Pageform = "main";
            ErHandler.errorMsg = "Cannot load XML file. Module not configured correctly yet.";
        }

        # endregion




        #region add page header
  


        HtmlGenericControl pageheader = FormBuilder.buildPageHeader(doc);
        
        //        pageheader.Controls.Add(new LiteralControl("<h1>" + System.Threading.Thread.CurrentThread.CurrentUICulture + "</h1>"));
        m.Controls.Add(pageheader);





        #endregion  

        # region add menu
        // hide menu button
        //HtmlAnchor hm = new HtmlAnchor();
        //hm.ID = "hide";
        //hm.InnerHtml = "[Hide me]";
        //hm.HRef = "javascript:showmenu('hide', 'pagemenu')";
        //hm.Style.Add("text-decoration", "underline");
        //m.Controls.Add(hm);
        //m.Controls.Add(CreateHTMLObjects.CreateBreak());

        // get the menu
        sideMenu.Controls.Add(MenuBuilder.BuildMenu(men));

        # endregion




        #region page type
        // type
        PageForm pageform = new PageForm();
        pageform.Load(doc);
        HtmlInputHidden formtype = new HtmlInputHidden();
        formtype.ID = "formtype";
        formtype.Value = pageform.ActionType;
        cont.Controls.Add(CreateHTMLObjects.CreateNewLine(1, 8));
        cont.Controls.Add(formtype);

        #endregion

        # region add form
        // get the form
        cont.Controls.Add(CreateHTMLObjects.CreateNewLine(1, 8));
        cont.Controls.Add(FormBuilder.BuildForm(doc, IsPostBack));
        # endregion

        # region add eventhandlers
        // add eventhandlers
        try
        {

            

            foreach (Section section in pageform.Sections.Items)
            {
                foreach (HtmlObject htmlobject in section.HtmlObjects.Items)
                {
                    // security
                    bool enabled = false;


                    if (htmlobject.HtmlType != "button")
                    {
                        continue;
                    }


                    if (htmlobject.Enabled.ToLower() == "yes")
                    {
                        if (htmlobject.Action == "delete" && SessionHandler.Usr.Delete)
                        {
                            enabled = true;
                        }
                        if ((htmlobject.Action == "update" ||
                            (htmlobject.Action == "check" && SessionHandler.Usr.Authorize) ||
                            htmlobject.Action == "authorize") && SessionHandler.Usr.Update)
                        {
                            enabled = true;
                        }
                        if ((htmlobject.Action == "insert" || htmlobject.Action == "file" || htmlobject.Action == "copy" || htmlobject.Action == "execute" || htmlobject.Action == "mail" || htmlobject.Action == "browse" || htmlobject.Action == "upload") && SessionHandler.Usr.Create)
                        {
                            enabled = true;
                        }
                        if (htmlobject.Action == "search" && SessionHandler.Usr.Read)
                        {
                            enabled = true;
                        }
                        if (htmlobject.Action == "login" || htmlobject.Action == "back" || htmlobject.Action == "logoff" || htmlobject.Action == "cancel" || htmlobject.Action == "export_excel")
                        {
                            enabled = true;
                        }

                    }

                    
                    // upload button separate eventhandler
                    if (htmlobject.Action == "upload" & enabled)
                    {
                        HtmlAnchor linkbutton = new HtmlAnchor();
                        linkbutton = (HtmlAnchor)Page.FindControl("ctl" + section.ID + "_" + htmlobject.ID);
                        linkbutton.InnerHtml = htmlobject.Label;
                        linkbutton.Name = htmlobject.Sproc + "&" + htmlobject.Reload;
                        linkbutton.ServerClick += new EventHandler(Btn_Click_Upload);
                    }

                    // execute button separate eventhandler
                    if (htmlobject.Action == "execute" & enabled)
                    {
                        HtmlAnchor linkbutton = new HtmlAnchor();
                        linkbutton = (HtmlAnchor)Page.FindControl("ctl" + section.ID + "_" + htmlobject.ID);
                        linkbutton.InnerHtml = htmlobject.Label;
                        linkbutton.Name = htmlobject.Sproc + "&" + htmlobject.Reload + "&" + htmlobject.Message;
                        linkbutton.ServerClick += new EventHandler(Btn_Click_Execute);
                    }

                    // browse button seperate event handler ?



                    // export button separate eventhandler
//                    if (htmlobject.Action == "export_excel" & enabled)
//                    {
//                        HtmlAnchor linkbutton = new HtmlAnchor();
//                        linkbutton = (HtmlAnchor)Page.FindControl("ctl" + section.ID + "_" + htmlobject.ID);
//                        linkbutton.InnerHtml = htmlobject.Label;
//                        linkbutton.Name = htmlobject.Sproc + "&" + htmlobject.Reload + "&" + htmlobject.Message;
//                        linkbutton.ServerClick += new EventHandler(Btn_Click_Execute);
//                    }




                    // rest of the buttons
                    if (htmlobject.Action != "execute" && enabled && htmlobject.Action != "browse" && htmlobject.Action != "upload")
                    {
                        HtmlAnchor linkbutton = new HtmlAnchor();
                        linkbutton = (HtmlAnchor)Page.FindControl("ctl" + section.ID + "_" + htmlobject.ID);
                        linkbutton.InnerHtml = htmlobject.Label;
                        linkbutton.Name = htmlobject.Action;
                        //linkbutton.HRef = "javascript:submitForm('" + linkbutton.ID + "','')";
                        linkbutton.ServerClick += new EventHandler(Btn_Click);
                        

                    }


      


                }

            }
        }
        catch (System.Exception ex)
        {
            ErHandler.errorMsg = "Source:" + ex.Source + "\n" + "Message:" + ex.Message;
            ErHandler.throwError();
        }

        #endregion

        #region set accesslevel
        SessionHandler.accesslevel = pageform.Access;
        #endregion

        #region page title
        // page title
       // pagetitle.Text = "<h1>" + pageform.Title + (SessionHandler.Usr.User == "" ? "" : " - ") + SessionHandler.Usr.User.ToUpper() + "</h1>";

        #endregion

        #region header stuff
        // add javascript

        
        Page.Header.Controls.Add(CreateHTMLObjects.CreateJavaScriptLink("https://code.jquery.com/jquery-2.1.4.min.js"));
        //Page.Header.Controls.Add(CreateHTMLObjects.CreateJavaScriptLink("~/javascript/parsley.min.js"));


        Page.Header.Controls.Add(CreateHTMLObjects.CreateNewLine(1, 8));
        Page.Header.Controls.Add(CreateHTMLObjects.CreateJavaScriptLink("~/javascript/generic.js"));
        Page.Header.Controls.Add(CreateHTMLObjects.CreateNewLine(1, 8));
        //Page.Header.Controls.Add(CreateHTMLObjects.CreateJavaScriptLink("~/javascript/menu.js"));
        Page.Header.Controls.Add(CreateHTMLObjects.CreateJavaScriptLink("~/javascript/Main.js"));
        Page.Header.Controls.Add(CreateHTMLObjects.CreateJavaScriptLink("~/javascript/date.min.js"));
        //Page.Header.Controls.Add(CreateHTMLObjects.CreateJavaScriptLink("~/javascript/validation.js"));

        Page.Header.Controls.Add(CreateHTMLObjects.CreateNewLine(1, 8));
        Page.Header.Controls.Add(CreateHTMLObjects.CreateJavaScriptLink("~/javascript/" + SessionHandler.Qstring.Pageform + ".js"));

        //// add css
        //Page.Header.Controls.Add(CreateHTMLObjects.CreateNewLine(1, 8));
        Page.Header.Controls.Add(CreateHTMLObjects.CreateCssLink("~/style/Main.css", "screen"));
        //Page.Header.Controls.Add(CreateHTMLObjects.CreateNewLine(1, 8));
        //Page.Header.Controls.Add(CreateHTMLObjects.CreateCssLink("~/style/" + SessionHandler.Qstring.Pageform + ".css", "screen"));
        //Page.Header.Controls.Add(CreateHTMLObjects.CreateNewLine(1, 8));

        // includes
        Scripts h = new Scripts();

        h.Load(ver);
        Page.Header.Controls.Add(CreateHTMLObjects.CreateNewLine(1, 8));
        if(h.Items.Count > 0)
        {
          Page.Header.Controls.Add(CreateHTMLObjects.CreateLiteral("<!-- includes -->"));
          Page.Header.Controls.Add(CreateHTMLObjects.CreateNewLine(1, 8));
        }
        foreach (Script sc in h.Items)
        {
            if (sc.Type == "css")
            {
                Page.Header.Controls.Add(CreateHTMLObjects.CreateCssLink(sc.Include, "screen"));
                Page.Header.Controls.Add(CreateHTMLObjects.CreateNewLine(1, 8));
            }
            if (sc.Type == "javascript")
            {
                Page.Header.Controls.Add(CreateHTMLObjects.CreateJavaScriptLink(sc.Include));
                Page.Header.Controls.Add(CreateHTMLObjects.CreateNewLine(1, 8));
            }
        }
        Page.Header.Controls.Add(CreateHTMLObjects.CreateNewLine(1, 0));
        // page title
        this.Page.Title = pageform.Title;

        # endregion

        # region add version info
        // add version
        //if (Application["version"] != null)
        //{
        //    XmlDocument versiondoc = (XmlDocument)Application["version"];
        //    HtmlGenericControl div_version = new HtmlGenericControl("div");
        //    VersionInfo versioninfo = new VersionInfo(versiondoc);
        //    div_version.ID = "version";
        //    div_version.Attributes["class"] = "version";
        //    LiteralControl versiontext = new LiteralControl("version:  " + versioninfo.Version);
        //    div_version.Controls.Add(versiontext);
        //    cont.Controls.Add(CreateHTMLObjects.CreateNewLine(1, 8));
        //    cont.Controls.Add(div_version);
        //}
        # endregion

        # region debug info
        // debug info here
        cont.Controls.Add(CreateHTMLObjects.CreateNewLine(1, 8));
        HtmlGenericControl dp = new HtmlGenericControl("div");
        dp.ID = "debugpanel";
        cont.Controls.Add(dp);
        cont.Controls.Add(CreateHTMLObjects.CreateNewLine(1, 8));
        ErHandler.showError();

        # endregion

        #region startup scripts
        // show hide menu after pageload
        if (!cs.IsStartupScriptRegistered(cstype, "showMenu"))
        {
            String cstext1 = "showmenustartup('hide','pagemenu');";
            cs.RegisterStartupScript(cstype, "showmenu", cstext1, true);

            String cstext5 = "showmenustartup('hidecontent','pageform');";
            cs.RegisterStartupScript(cstype, "showcontent", cstext5, true);

            String cstext2 = "showsubmenustartup();";
            cs.RegisterStartupScript(cstype, "showsubmenu", cstext2, true);

            String cstext3 = "showmenustartup('','section_advancedfilter');";
            cs.RegisterStartupScript(cstype, "showfilter", cstext3, true);

            if (pageform.ActionType != "search")
            {
                String cstext4 = "checkMandatory();";
                cs.RegisterStartupScript(cstype, "checkmandatory", cstext4, true);
            }

        }
        #endregion
    }





    protected void Page_Load()
    {
        #region check security

        //#region check security
        bool allowed = false;
        foreach (string role in SessionHandler.Usr.Roles)
        {
            if (SessionHandler.accesslevel.ToLower() == role.ToLower())
            {
                allowed = true;
            }
        }

        if (!allowed && SessionHandler.accesslevel != "application")
        {
            #region find startpage
            string module = string.Empty;
            XmlNodeList nodes = men.SelectNodes("//menuitem");
            foreach (XmlNode itm in nodes)
            {
                if (itm.SelectSingleNode("type") != null)
                {
                    if (itm.SelectSingleNode("type").InnerText == "startpage")
                    {
                        module = itm.SelectSingleNode("module").InnerText;
                        break;
                    }

                    else
                        if (itm.SelectSingleNode("type").InnerText == "startpage")
                        {
                            module = itm.SelectSingleNode("module").InnerText;
                            break;
                        }
                }
            }
            #endregion

            Response.Redirect("~/index.aspx?page=" + module, false);

        }
        #endregion

        if (!IsPostBack)
        {
            PageForm pageform = new PageForm();
            pageform.Load(doc);

            string actiontype = string.Empty;
            actiontype = pageform.ActionType;

            #region clear stored datatable
            if (pageformchanged)
            {
                SessionHandler.datatable = null;
            }
            #endregion


            if (actiontype == "edit" || actiontype == "delete")
            {
                # region create select
                string sql = string.Empty;
                XmlDocument vd = (XmlDocument)HttpContext.Current.Application["version"];
                VersionInfo v = new VersionInfo(vd);

                foreach (Section section in pageform.Sections.Items)
                {
                    foreach (HtmlObject htmlobject in section.HtmlObjects.Items)
                    {
                        if (htmlobject.DbField != "")
                        {
                            if (htmlobject.DbField.StartsWith("##"))
                            {
                                // decrypt
                                sql += "dbo.sf_decrypt(" + htmlobject.DbField.Replace("##", "") + ",'" + v.Encryption + "'), ";
                            }
                            else
                            {
                                // no decript
                                sql += htmlobject.DbField.Replace("##", "") + ", ";
                            }
                        }
                    }
                }

                // build the select
                StringBuilder sb = new StringBuilder();
                sb.Append("select ");
                sb.Append(sql.Substring(0, sql.Length - 2));
                sb.Append(" from ");
                sb.Append(pageform.Database.Table);
                sb.Append(" where ");
                sb.Append(pageform.Database.Condition);
                sb.Append(" and ");
                sb.Append(pageform.Database.PrimaryKey);
                sb.Append(" = ");
                if (SessionHandler.Usr.Read == true)
                {
                    sb.Append("'" + SessionHandler.Qstring.Filter + "'");
                }
                else
                {
                    sb.Append(" null ");
                }
                sql = sb.ToString();

                // replace with environment variables
                sql = sql.Replace("@user", "'" + SessionHandler.Usr.User + "'");
                sql = sql.Replace("@module", "'" + SessionHandler.Qstring.Pageform + "'");
                sql = sql.Replace("@filter", "'" + SessionHandler.Qstring.Filter.Split('|')[0] + "'");
                sql = sql.Replace("@role", "'" + SessionHandler.accesslevel + "'");
                sql = sql.Replace("@department", "'" + SessionHandler.Usr.Department + "'");

                # endregion

                #region fill form
                ConnectionString cs = new ConnectionString(SessionHandler.connection);

                DataTable dt = cs.Select(sql);

                // extra security to avoid messing with querystring
                if (dt.Rows.Count == 0)
                {
                    ErHandler.errorMsg = "Invalid request!";
                    ErHandler.throwError();
                }

                int i = 0;
                HtmlInputText textbox = new HtmlInputText();
                HtmlTextArea textarea = new HtmlTextArea();
                HtmlSelect dropdown = new HtmlSelect();
                HtmlInputCheckBox checkbox = new HtmlInputCheckBox();

                foreach (Section section in pageform.Sections.Items)
                {
                    foreach (HtmlObject htmlobject in section.HtmlObjects.Items)
                    {
                        if (htmlobject.DbField != "")
                        {
                            if (htmlobject.HtmlType == "inputbox")
                            {
                                textbox = (HtmlInputText)FindControl("ctl" + section.ID + "_" + htmlobject.ID);
                                if (dt.Rows.Count > 0)
                                {
                                    textbox.Value = dt.Rows[0][i].ToString();
                                }
                            }

                            if (htmlobject.HtmlType == "textarea")
                            {
                                textarea = (HtmlTextArea)FindControl("ctl" + section.ID + "_" + htmlobject.ID);
                                if (dt.Rows.Count > 0)
                                {
                                    textarea.Value = dt.Rows[0][i].ToString();
                                }
                            }

                            if (htmlobject.HtmlType == "checkbox")
                            {
                                checkbox = (HtmlInputCheckBox)FindControl("ctl" + section.ID + "_" + htmlobject.ID);
                                if (dt.Rows.Count > 0)
                                {
                                    checkbox.Checked = dt.Rows[0][i].ToString() == "0" ? false : true;
                                }
                            }

                            if (htmlobject.HtmlType == "dropdown")
                            {
                                dropdown = (HtmlSelect)FindControl("ctl" + section.ID + "_" + htmlobject.ID);
                                if (dt.Rows.Count > 0)
                                {
                                    dropdown.Value = dt.Rows[0][i].ToString();
                                }
                            }
                            i++;
                        }
                    }
                }

                #endregion
            }

            if (actiontype == "create" || actiontype == "execute")
            {
                #region datatable
                foreach (Section section in pageform.Sections.Items)
                {
                    foreach (HtmlObject htmlobject in section.HtmlObjects.Items)
                    {
                        if (htmlobject.Display == "grid" && SessionHandler.datatable != null)
                        {
                            HtmlGenericControl outputpanel = new HtmlGenericControl("div");
                            outputpanel = (HtmlGenericControl)Page.FindControl("ctl" + section.ID + "_" + htmlobject.ID);
                            outputpanel.Controls.Add(FormBuilder.BuildTable(SessionHandler.datatable, section));
                            // resize
                            outputpanel.Style["height"] = FormBuilder.BuildTable(SessionHandler.datatable, section).Style["height"] + "px";
                        }
                    }
                }
                #endregion

                #region defaults for insert
                HtmlInputText textbox = new HtmlInputText();
                HtmlTextArea textarea = new HtmlTextArea();
                HtmlSelect dropdown = new HtmlSelect();
                HtmlInputCheckBox checkbox = new HtmlInputCheckBox();
                // ListBox listbox = new ListBox();


                foreach (Section section in pageform.Sections.Items)
                {
                    foreach (HtmlObject htmlobject in section.HtmlObjects.Items)
                    {
                        // if value from database
                        string defvalue = htmlobject.DefValue;
                        if (htmlobject.DefValue.Length > 6)
                        {
                            if (htmlobject.DefValue.Substring(0, 6).ToLower() == "select" || htmlobject.DefValue.Substring(0, 4).ToLower() == "exec")
                            {
                                DataTable dt = new DataTable();
                                ConnectionString cs = new ConnectionString(SessionHandler.connection);
                                string sql = htmlobject.DefValue;

                                // replace with environment variables
                                string[] t = SessionHandler.referrer.Split('=');
                                sql = sql.Replace("@user", "'" + SessionHandler.Usr.User + "'");
                                sql = sql.Replace("@module", "'" + SessionHandler.Qstring.Pageform + "'");
                                sql = sql.Replace("@filter", "'" + t[2].Split('|')[0] + "'");
                                sql = sql.Replace("@role", "'" + SessionHandler.accesslevel + "'");
                                sql = sql.Replace("@department", "'" + SessionHandler.Usr.Department + "'");

                                dt = cs.Select(sql);
                                foreach (DataRow r in dt.Rows)
                                {
                                    defvalue = r[0].ToString();
                                }
                            }
                        }


                        if (htmlobject.HtmlType == "inputbox" || htmlobject.HtmlType == "email" || htmlobject.HtmlType == "password" || htmlobject.HtmlType == "date" || htmlobject.HtmlType == "number")
                        {
                            textbox = (HtmlInputText)FindControl("ctl" + section.ID + "_" + htmlobject.ID);
                            textbox.Value = defvalue;
                        }

                        if (htmlobject.HtmlType == "textarea")
                        {
                            textarea = (HtmlTextArea)FindControl("ctl" + section.ID + "_" + htmlobject.ID);
                            textarea.Value = defvalue;
                        }


                        if (htmlobject.HtmlType == "checkbox")
                        {


                            checkbox = (HtmlInputCheckBox)FindControl("ctl" + section.ID + "_" + htmlobject.ID);
                       
                            checkbox.Checked = defvalue == "1" ? true : false ;
                            
                        }

                        if (htmlobject.HtmlType == "dropdown")
                        {
                            dropdown = (HtmlSelect)FindControl("ctl" + section.ID + "_" + htmlobject.ID);


                            dropdown.Value = defvalue;
                        }

                        // if (htmlobject.HtmlType == "listbox")
                        // {
                        //     listbox = (ListBox)FindControl("ctl" + section.ID + "_" + htmlobject.ID);
                        //     listbox.SelectedValue = defvalue;
                        // }


                    }
                }
                #endregion
            }


            if (actiontype == "search" || actiontype == "home" || actiontype == "none")
            {
                #region fill searchboxes
                string[] searchvalues = SessionHandler.Qstring.Filter.Split('|');
                foreach (Section section in pageform.Sections.Items)
                {
                    int i = 0;
                    HtmlInputText textbox = new HtmlInputText();
                    HtmlSelect dropdown = new HtmlSelect();
                    //      ListBox listbox = new ListBox();

                    foreach (HtmlObject htmlobject in section.HtmlObjects.Items)
                    {
                        // if value from database
                        string defvalue = htmlobject.DefValue;
                        if (htmlobject.DefValue.Length > 6)
                        {
                            if (htmlobject.DefValue.Substring(0, 6).ToLower() == "select" || htmlobject.DefValue.Substring(0, 4).ToLower() == "exec")
                            {
                                DataTable dt = new DataTable();
                                ConnectionString cs = new ConnectionString(SessionHandler.connection);
                                string sql = htmlobject.DefValue;

                                // replace with environment variables
                                sql = sql.Replace("@user", "'" + SessionHandler.Usr.User + "'");
                                sql = sql.Replace("@module", "'" + SessionHandler.Qstring.Pageform + "'");
                                sql = sql.Replace("@filter", "'" + SessionHandler.Qstring.Filter.Split('|')[0] + "'");
                                sql = sql.Replace("@role", "'" + SessionHandler.accesslevel + "'");
                                sql = sql.Replace("@department", "'" + SessionHandler.Usr.Department + "'");

                                dt = cs.Select(sql);
                                foreach (DataRow r in dt.Rows)
                                {
                                    defvalue = r[0].ToString();
                                }
                            }
                        }
                        if (htmlobject.HtmlType == "inputbox" && (htmlobject.Target == "search" || htmlobject.Target == "parameter"))
                        {
                            textbox = (HtmlInputText)FindControl("ctl" + section.ID + "_" + htmlobject.ID);
                            textbox.Value = searchvalues[searchvalues.Length - 1 >= i ? i : 0].Replace("nofilter", defvalue);
                            i++;
                            continue;
                        }

                        if (htmlobject.HtmlType == "inputbox" && (htmlobject.Target == "self"))
                        {
                            textbox = (HtmlInputText)FindControl("ctl" + section.ID + "_" + htmlobject.ID);
                            textbox.Value = defvalue;
                            i++;
                            continue;
                        }

                        if (htmlobject.HtmlType == "dropdown" && (htmlobject.Target == "search" || htmlobject.Target == "parameter"))
                        {
                            dropdown = (HtmlSelect)FindControl("ctl" + section.ID + "_" + htmlobject.ID);
                            dropdown.Value = searchvalues[searchvalues.Length - 1 >= i ? i : 0].Replace("nofilter", defvalue);
                            i++;
                            continue;
                        }

                        //      if (htmlobject.HtmlType == "listbox" && htmlobject.Target == "search")
                        //      {
                        //          listbox = (ListBox)FindControl("ctl" + section.ID + "_" + htmlobject.ID);
                        //          listbox.SelectedValue = searchvalues[searchvalues.Length - 1 >= i ? i : 0].Replace("nofilter", defvalue);
                        //          i++;
                        //      }

                    }
                }

                #endregion

                #region referrer
                // referrer
                SessionHandler.referrer = Request.RawUrl;
                #endregion
            }

            // navigation
            SessionHandler.Navigation.Add(Request.RawUrl, SessionHandler.Qstring.Pageform);
        }

    }

    protected void Btn_Click_Execute(object s, EventArgs e)
    {
        HtmlAnchor linkbutton = (HtmlAnchor)s;
        ConnectionString cs = new ConnectionString(SessionHandler.connection);
        string sql = "exec " + linkbutton.Name.Split('&')[0];
        string reload = linkbutton.Name.Split('&')[1];
        string message = linkbutton.Name.Split('&')[2];
        if (message.Length == 0) { message = "Procedure executed!"; }
        PageForm pageform = new PageForm();
        pageform.Load(doc);

        #region get parameters
        StringBuilder sbparams = new StringBuilder();
        StringBuilder sboutput = new StringBuilder();
        StringBuilder sb = new StringBuilder();
        string parameters = string.Empty;
        string delimeter = string.Empty;
        HtmlTextArea outputfile = new HtmlTextArea();
        HtmlGenericControl outputpanel = new HtmlGenericControl("div");
        Section pagesection = new Section();
        string[] loadlist = new string[] { };

        // replace with environment variables
        sql = sql.Replace("@user", "'" + SessionHandler.Usr.User + "'");
        sql = sql.Replace("@module", "'" + SessionHandler.Qstring.Pageform + "'");
        sql = sql.Replace("@filter", "'" + SessionHandler.Qstring.Filter.Split('|')[0] + "'");
        sql = sql.Replace("@role", "'" + SessionHandler.accesslevel + "'");
        sql = sql.Replace("@department", "'" + SessionHandler.Usr.Department + "'");


        sb.Append("'" + System.DateTime.Now.Year.ToString() + "-");
        sb.Append(System.DateTime.Now.Month.ToString() + "-");
        sb.Append(System.DateTime.Now.Day.ToString() + " ");
        sb.Append(System.DateTime.Now.Hour + ":");
        sb.Append(System.DateTime.Now.Minute + ":");
        sb.Append(System.DateTime.Now.Second + "',");

        sql = sql.Replace("@now", sb.ToString());

        sbparams.Append(" ");

        foreach (Section section in pageform.Sections.Items)
        {
            if (section.ID == linkbutton.ID.Substring(0, linkbutton.ID.IndexOf("_")).Replace("ctl", "") || 1 == 1)
            {
                pagesection = section;

                foreach (HtmlObject htmlobject in section.HtmlObjects.Items)
                {
                    // parameters if there are any
                    if (htmlobject.Target == "parameter" && htmlobject.HtmlType == "inputbox")
                    {
                        HtmlInputText parameter = new HtmlInputText();
                        parameter = (HtmlInputText)Page.FindControl("ctl" + section.ID + "_" + htmlobject.ID);
                        sbparams.Append("'" + CleanInput.Sanitize(parameter.Value) + "',");
                    }

                    if (htmlobject.Target == "parameter" && htmlobject.HtmlType == "textarea" && htmlobject.Datatype == "sql")
                    {
                        HtmlTextArea parameterarea = new HtmlTextArea();
                        parameterarea = (HtmlTextArea)Page.FindControl("ctl" + section.ID + "_" + htmlobject.ID);
                        sbparams.Append("'" + parameterarea.Value.Replace("'", "''") + "',");
                    }

                    if (htmlobject.Target == "parameter" && htmlobject.HtmlType == "dropdown")
                    {
                        HtmlSelect dropdown = new HtmlSelect();
                        dropdown = (HtmlSelect)Page.FindControl("ctl" + section.ID + "_" + htmlobject.ID);
                        sbparams.Append("'" + dropdown.Value + "',");
                    }

                    if (htmlobject.Target == "parameter" && htmlobject.HtmlType == "textarea" && htmlobject.Datatype != "sql")
                    {
                        HtmlTextArea parameterlist = new HtmlTextArea();
                        parameterlist = (HtmlTextArea)Page.FindControl("ctl" + section.ID + "_" + htmlobject.ID);
                        char[] delimiters = new char[] { ';' }; //{ '\r', '\n' };
                        loadlist = CleanInput.Sanitize(parameterlist.Value).Split(delimiters, StringSplitOptions.RemoveEmptyEntries); ;
                    }

                    if (htmlobject.HtmlType == "button" && htmlobject.Action == "execute" && htmlobject.Navigate != "")
                    {
                        SessionHandler.referrer = SessionHandler.Navigation.GetUrlByPage(htmlobject.Navigate);
                    }


                    if (htmlobject.HtmlType == "output")
                    {
                        if (htmlobject.Display == "grid")
                        {
                            outputpanel = (HtmlGenericControl)Page.FindControl("ctl" + section.ID + "_" + htmlobject.ID);
                        }
                        else
                        {
                            outputfile = (HtmlTextArea)Page.FindControl("ctl" + section.ID + "_" + htmlobject.ID);
                        }
                        delimeter = htmlobject.Delimeter == "" ? "comma" : htmlobject.Delimeter;
                    }
                }

                parameters = sbparams.ToString().Substring(0, sbparams.ToString().Length - 1);
            }
        }
        delimeter = delimeter.Replace("comma", ",");
        delimeter = delimeter.Replace("tab", "\t");
        delimeter = delimeter.Replace("semicolon", ";");

        #endregion

        if (outputpanel.ID == null && outputfile.ID == null)
        {
            #region exec no output
            if (loadlist.Length > 0)
            {
                foreach (string pm in loadlist)
                {
                    cs.Update(sql + parameters + ",'" + pm + "'");
                }
            }
            else
            {
                cs.Update(sql + parameters);
            }

            #endregion
        }
        else
        {
            #region exec with output

            DataTable dt = new DataTable();
            if (loadlist.Length > 0)
            {
                foreach (string pm in loadlist)
                {
                    dt = cs.Select(sql + parameters + ",'" +
                        //make double quotes
                    pm.Replace("'", "''") + "'");
                }
            }
            else
            {
                dt = cs.Select(sql + parameters);
            }

            SessionHandler.datatable = dt;


            foreach (DataRow r in dt.Rows)
            {
                for (int i = 0; i <= dt.Columns.Count - 1; i++)
                {
                    sboutput.Append(r[i]);
                    sboutput.Append(delimeter);
                }
                sboutput.Append("{n}");
            }
            outputpanel.Controls.Add(FormBuilder.BuildTable(dt, pagesection));
            // resize
            outputpanel.Style["height"] = FormBuilder.BuildTable(dt, pagesection).Style["height"] + "px";
            outputfile.Value = sboutput.ToString().Replace(delimeter + "{n}", "\n");

            #endregion
        }

        #region audit
        if (pageform.Audit.Enabled)
        {
            // create audit table
            cs.CreateAuditTable(pageform.Audit.Table);

            // insert audit record
            string[] url = Request.Url.ToString().Split('?');
            StringBuilder sbaudit = new StringBuilder();
            sbaudit.Append("insert ");
            sbaudit.Append(pageform.Audit.Table);
            sbaudit.Append("(action, usr, page, descr, ts) ");
            sbaudit.Append("values ('");
            sbaudit.Append(pageform.ActionType);
            sbaudit.Append("','");
            sbaudit.Append(SessionHandler.Usr.User);
            sbaudit.Append("','");
            sbaudit.Append(SessionHandler.Qstring.Pageform);
            sbaudit.Append("','");
            sbaudit.Append(sql.Replace(",", ";").Replace("'", "''"));
            sbaudit.Append("','");
            sbaudit.Append(System.DateTime.Now);
            sbaudit.Append("')");
            cs.Update(sbaudit.ToString());

        }
        #endregion

        ErHandler.errorMsg = message;
        ErHandler.showError();
        Alert.Show(message);
       
        if (reload == "yes")
        {

            string exec = SessionHandler.referrer.Split('&')[SessionHandler.referrer.Split('&').Length-1];
            exec = exec == "executed=yes" ? "" : "&executed=yes";
            Response.Redirect(SessionHandler.referrer + exec, false);
        }
    }

    protected void Btn_Click(object s, EventArgs e)
    {
        #region variables
        PageForm pageform = new PageForm();
        HtmlAnchor linkbutton = (HtmlAnchor)s;
        ConnectionString cs = new ConnectionString(SessionHandler.connection);
        StringBuilder sb = new StringBuilder();
        StringBuilder sb2 = new StringBuilder();
        List<string> values = new List<string>();
        List<string> fields = new List<string>();
        List<string> parameters = new List<string>();
        string sql = string.Empty;
        string primarykey = string.Empty;
        string filepath = string.Empty;
        string file = string.Empty;
        string result = string.Empty;
        string email = string.Empty;

        int i = 0;
        DataTable dt;
        int test;
        #endregion

        pageform.Load(doc);
        Debug.WriteLine("button clicked: " + linkbutton);
        Debug.WriteLine("button clicked name: " + linkbutton.Name);
        switch (linkbutton.Name)
        {

            case "export_excel":
                Debug.WriteLine("export test button pressed");
                ExcelExport.exportDataTable(SessionHandler.datatable);
                

                break;

            case "search":
                #region search
                foreach (Section section in pageform.Sections.Items)
                {
                    List<string> filter = new List<string>();
                    string constructedfilter = string.Empty;

                    foreach (HtmlObject htmlobject in section.HtmlObjects.Items)
                    {
                        if ((htmlobject.Target == "search" || htmlobject.Target == "parameter") && (htmlobject.HtmlType == "inputbox" || htmlobject.HtmlType == "email" || htmlobject.HtmlType == "password" || htmlobject.HtmlType == "date" || htmlobject.HtmlType == "number"))
                        {
                            HtmlInputText textbox = new HtmlInputText();
                            textbox = (HtmlInputText)Page.FindControl("ctl" + section.ID + "_" + htmlobject.ID);
                            // we supppose AND when disabled
                            // if (textbox.Disabled)
                            //  {
                            //    textbox.Value = "AND";
                            //  }
                            filter.Add(CleanInput.Sanitize(textbox.Value) == "" ? "nofilter" : CleanInput.Sanitize(textbox.Value));
                        }

                        if ((htmlobject.Target == "search" || htmlobject.Target == "parameter") && htmlobject.HtmlType == "dropdown")
                        {
                            HtmlSelect dropdown = new HtmlSelect();
                            dropdown = (HtmlSelect)Page.FindControl("ctl" + section.ID + "_" + htmlobject.ID);
                            filter.Add(dropdown.Value.Trim() == "" ? "nofilter" : dropdown.Value.Trim());
                        }
                    }

                    if (filter.Count == 0)
                    {
                        continue;
                    }

                    // build the filter
                    // fix the first field
                    constructedfilter = filter[0].ToString() == "AND" ? "nofilter" : filter[0].ToString();
                    for (i = 1; i < filter.Count; i++)
                    {
                        constructedfilter += "|" + filter[i];
                    }
                    // replace &
                    constructedfilter = constructedfilter.Replace("&", "_");

                    // redirect
                    Response.Redirect("~/index.aspx?page=" + SessionHandler.Qstring.Pageform + "&filter=" + constructedfilter, false);

                    // quit
                    break;
                }
                break;
                #endregion

            case "login":
                #region login
                string user = string.Empty;
                string pw = string.Empty;

                foreach (Section section in pageform.Sections.Items)
                {
                    foreach (HtmlObject htmlobject in section.HtmlObjects.Items)
                    {
                        if (htmlobject.HtmlType == "inputbox")
                        {
                            HtmlInputText textbox = new HtmlInputText();
                            textbox = (HtmlInputText)Page.FindControl("ctl" + section.ID + "_" + htmlobject.ID);
                            user = CleanInput.Sanitize(textbox.Value);
                        }
                        if (htmlobject.HtmlType == "password")
                        {
                            HtmlInputText passw = new HtmlInputText();
                            passw = (HtmlInputText)Page.FindControl("ctl" + section.ID + "_" + htmlobject.ID);
                            pw = CleanInput.Sanitize(passw.Value);
                        }
                    }
                }
                // login logic here
                string mapFile = Server.MapPath(@"~/config/app/usrmapping.prop");
                XmlDocument map = new XmlDocument();
                XmlNode node;
                map.Load(mapFile);

                #region parse mapping
                // table
                string table;
                node = map.SelectSingleNode("//table");
                table = node == null ? "" : map.SelectSingleNode("//table").InnerText;
                // usr
                node = map.SelectSingleNode("//user");
                string usr = node == null ? "" : node.SelectSingleNode("dbfield").InnerText;
                //pw
                node = map.SelectSingleNode("//password");
                string password = node == null ? "" : node.SelectSingleNode("dbfield").InnerText;
                //department
                node = map.SelectSingleNode("//department");
                string department = node == null ? "" : node.SelectSingleNode("dbfield").InnerText;
                //read
                node = map.SelectSingleNode("//read");
                string read = node == null ? "" : node.SelectSingleNode("dbfield").InnerText;
                //create
                node = map.SelectSingleNode("//create");
                string create = node == null ? "" : node.SelectSingleNode("dbfield").InnerText;
                //delete
                node = map.SelectSingleNode("//delete");
                string delete = node == null ? "" : node.SelectSingleNode("dbfield").InnerText;
                //update
                node = map.SelectSingleNode("//update");
                string update = node == null ? "" : node.SelectSingleNode("dbfield").InnerText;
                //admin
                node = map.SelectSingleNode("//admin");
                string admin = node == null ? "" : node.SelectSingleNode("dbfield").InnerText;
                //authorize
                node = map.SelectSingleNode("//authorize");
                string authorize = node == null ? "" : node.SelectSingleNode("dbfield").InnerText;

                #endregion

                #region build sql
                // build the sql
                sb.Append("select ");
                sb.Append(usr + ",");
                sb.Append(password + ",");
                sb.Append(department + ",");
                sb.Append(create + ",");
                sb.Append(read + ",");
                sb.Append(update + ",");
                sb.Append(delete + ",");
                sb.Append(authorize + ",");
                sb.Append(admin);
                sb.Append(" from ");
                sb.Append(table);
                sb.Append(" where ");
                sb.Append(usr);
                sb.Append(" = ");
                sb.Append("'" + user + "'");
                sb.Append(" and ");
                sb.Append(password);
                sb.Append(" = ");
                // encryption
                sb.Append("dbo.sf_encrypt('" + pw + "')");
                #endregion

                Usr theuser = new Usr();

                #region reset security
                theuser.Allowed = false;
                theuser.Create = false;
                theuser.Read = false;
                theuser.Update = false;
                theuser.Delete = false;
                theuser.Admin = false;
                theuser.Authorize = false;
                theuser.Department = null;
                List<string> l = new List<string>();
                l.Add("");
                theuser.Roles = l;

                // clear navigation
                SessionHandler.Navigation.Clear();
                #endregion


                dt = cs.Select(sb.ToString());
                if (dt.Rows.Count > 0)
                {
                    // allowed
                    theuser.Allowed = true;

                    // department
                    theuser.Department = dt.Rows[0][2].ToString();

                    if (dt.Rows[0][3].ToString() == "1")
                    {
                        // create
                        theuser.Create = true;
                    }

                    if (dt.Rows[0][4].ToString() == "1")
                    {
                        // read
                        theuser.Read = true;
                    }

                    if (dt.Rows[0][5].ToString() == "1")
                    {
                        // update
                        theuser.Update = true;
                    }

                    if (dt.Rows[0][6].ToString() == "1")
                    {
                        // delete
                        theuser.Delete = true;
                    }

                    if (dt.Rows[0][7].ToString() == "1")
                    {
                        // authorize
                        theuser.Authorize = true;
                    }

                    if (dt.Rows[0][8].ToString() == "1")
                    {
                        // admin
                        theuser.Admin = true;
                    }

                    // here the role logic
                    mapFile = Server.MapPath(@"~/config/app/usrrolemapping.prop");
                    map.Load(mapFile);

                    #region parse rolemapping
                    // table
                    string roletable;
                    node = map.SelectSingleNode("//table");
                    roletable = node == null ? "" : map.SelectSingleNode("//table").InnerText;

                    // usr
                    node = map.SelectSingleNode("//user");
                    string roleusr = node == null ? "" : node.SelectSingleNode("dbfield").InnerText;
                    //role
                    node = map.SelectSingleNode("//role");
                    string role = node == null ? "" : node.SelectSingleNode("dbfield").InnerText;
                    #endregion

                    #region build sql for role
                    StringBuilder rl = new StringBuilder();
                    rl.Append("select ");
                    rl.Append(role);
                    rl.Append(" from ");
                    rl.Append(roletable);
                    rl.Append(" where ");
                    rl.Append(usr);
                    rl.Append(" = ");
                    rl.Append("'" + user + "'");
                    #endregion

                    dt = cs.Select(rl.ToString());
                    List<String> roles = new List<string>();
                    foreach (DataRow r in dt.Rows)
                    {
                        roles.Add(r[0].ToString());
                    }
                    theuser.Roles = roles;
                    theuser.User = user;
                }

                #region audit
                if (pageform.Audit.Enabled)
                {
                    // create audit table
                    cs.CreateAuditTable(pageform.Audit.Table);

                    // insert audit record
                    string[] url = Request.Url.ToString().Split('?');
                    StringBuilder sbaudit = new StringBuilder();
                    sbaudit.Append("insert ");
                    sbaudit.Append(pageform.Audit.Table);
                    sbaudit.Append("(action, usr, page, descr, ts) ");
                    sbaudit.Append("values ('");
                    sbaudit.Append(pageform.ActionType);
                    sbaudit.Append("','");
                    sbaudit.Append(user);
                    sbaudit.Append("','");
                    sbaudit.Append(SessionHandler.Qstring.Pageform);
                    sbaudit.Append("','");
                    sbaudit.Append(theuser.Allowed ? "success" : "failed");
                    sbaudit.Append("','");
                    sbaudit.Append(System.DateTime.Now);
                    sbaudit.Append("')");
                    cs.Update(sbaudit.ToString());

                }
                #endregion

                if (theuser.Allowed)
                {

                    #region find startpage
                    string module = string.Empty;

                    // load the full menu
                    string menuItemFile = Server.MapPath(@"~/config/menu.prop");
                    // some sort of error handling here!
                    men.Load(menuItemFile);
                    SessionHandler.menuFile = men;

                    XmlNodeList nodes = men.SelectNodes("//menuitem");
                    foreach (string role in theuser.Roles)
                    {
                        foreach (XmlNode itm in nodes)
                        {
                            XmlNode item = itm.SelectSingleNode("security");
                            if (item!= null)
                            {
                                if (!string.IsNullOrEmpty(item.InnerText))
                                {

                                    if (item.InnerText.ToLower() == role.ToLower() ||
                                        item.InnerText.ToLower() == "application")
                                    {

                                        if (itm.SelectSingleNode("type") != null)
                                        {
                                            if (itm.SelectSingleNode("type").InnerText == "startpage")
                                            {
                                                module = itm.SelectSingleNode("module").InnerText;
                                                break;
                                            }

                                            else
                                                if (itm.SelectSingleNode("type").InnerText == "startpage")
                                                {
                                                    module = itm.SelectSingleNode("module").InnerText;
                                                    break;
                                                }
                                        }



                                    }

                                }

                            }
                           
                        }
                    }
                    #endregion

                    // save user in session
                    SessionHandler.Usr = theuser;
                    
                    // go to startpage
                    Response.Redirect("~/index.aspx?page=" + module, false);
                }
                else
                {
                    Alert.Show("login not correct");
                }
                break;
                #endregion

            case "update":
                #region update record

                if (pageform.ActionType != "search")
                {
                    foreach (Section section in pageform.Sections.Items)
                    {
                        foreach (HtmlObject htmlobject in section.HtmlObjects.Items)
                        {
                            if ((htmlobject.HtmlType == "inputbox" || htmlobject.HtmlType == "email" || htmlobject.HtmlType == "password" || htmlobject.HtmlType == "date" || htmlobject.HtmlType == "number" ) && htmlobject.Enabled == "yes")
                            {
                                HtmlInputText textbox = new HtmlInputText();
                                textbox = (HtmlInputText)Page.FindControl("ctl" + section.ID + "_" + htmlobject.ID);
                                if (htmlobject.HtmlType == "date" && textbox.Value.Length > 0)
                                {
                                    textbox.Value = Convert.ToDateTime(textbox.Value).ToString("yyyy-MM-dd HH:mm:ss");
                                }
                                values.Add(CleanInput.Sanitize(textbox.Value).Replace("'", "''"));
                            }

                            if (htmlobject.HtmlType == "checkbox" && htmlobject.Enabled == "yes")
                            {
                                HtmlInputCheckBox checkbox = new HtmlInputCheckBox();
                                checkbox = (HtmlInputCheckBox)Page.FindControl("ctl" + section.ID + "_" + htmlobject.ID);
                                values.Add(checkbox.Checked == true ? "1" : "0");
                            }

                            if (htmlobject.HtmlType == "dropdown" && htmlobject.Enabled == "yes")
                            {
                                HtmlSelect dropdown = new HtmlSelect();
                                dropdown = (HtmlSelect)Page.FindControl("ctl" + section.ID + "_" + htmlobject.ID);
                                values.Add(dropdown.Value);
                            }

                            if (htmlobject.HtmlType == "textarea" && htmlobject.Enabled == "yes" && htmlobject.Datatype != "sql")
                            {
                                HtmlTextArea textbox = new HtmlTextArea();
                                textbox = (HtmlTextArea)Page.FindControl("ctl" + section.ID + "_" + htmlobject.ID);
                                values.Add(CleanInput.Sanitize(textbox.Value).Replace("'", "''"));
                            }

                            // this brings a security risk
                            if (htmlobject.HtmlType == "textarea" && htmlobject.Enabled == "yes" && htmlobject.Datatype == "sql")
                            {
                                HtmlTextArea textbox = new HtmlTextArea();
                                textbox = (HtmlTextArea)Page.FindControl("ctl" + section.ID + "_" + htmlobject.ID);
                                values.Add(textbox.Value.Replace("'", "''"));
                            }

                            if (htmlobject.HtmlType == "button" && htmlobject.Action == "update" && htmlobject.Navigate != "")
                            {
                                SessionHandler.referrer = SessionHandler.Navigation.GetUrlByPage(htmlobject.Navigate);
                            }

                        }
                    }
                    foreach (Section section in pageform.Sections.Items)
                    {
                        foreach (HtmlObject htmlobject in section.HtmlObjects.Items)
                        {
                            if (htmlobject.DbField != "" && htmlobject.Enabled == "yes")
                                fields.Add(htmlobject.DbField);
                        }
                    }

                    // construct sql
                    sb.Append("update ");
                    sb.Append(pageform.Database.Table);
                    sb.Append(" set ");
                    i = 0;
                    foreach (string value in values)
                    {
                        if (fields[i].StartsWith("##"))
                        {
                            // if encrytion is required
                            fields[i] = fields[i].Replace("##", "");
                            sb.Append(fields[i] + " = dbo.sf_encrypt('" + value + "'), ");
                        }
                        else
                        {
                            // no encryption
                            sb.Append(fields[i] + " = '" + value + "', ");
                        }
                        i++;
                    }
                    sb.Append("where ");
                    // added condition
                    sb.Append(pageform.Database.Condition);
                    sb.Append(" and ");
                    sb.Append(pageform.Database.PrimaryKey);
                    sb.Append(" = ");
                    sb.Append("'" + SessionHandler.Qstring.Filter + "'");
                    sql = sb.ToString().Replace(", where", " where");

                    // replace with environment variables
                    sql = sql.Replace("@user", "'" + SessionHandler.Usr.User + "'");
                    sql = sql.Replace("@module", "'" + SessionHandler.Qstring.Pageform + "'");
                    sql = sql.Replace("@filter", "'" + SessionHandler.Qstring.Filter.Split('|')[0] + "'");
                    sql = sql.Replace("@role", "'" + SessionHandler.accesslevel + "'");
                    sql = sql.Replace("@department", "'" + SessionHandler.Usr.Department + "'");

                    // update
                    cs.Update(sql);
                }
                #endregion

                else
                    Debug.WriteLine("TEST " + System.Threading.Thread.CurrentThread.CurrentUICulture);
                    #region update table
                    foreach (Section section in pageform.Sections.Items)
                    {
                        foreach (HtmlObject htmlobject in section.HtmlObjects.Items)
                        {
                            if (htmlobject.HtmlType == "table" && htmlobject.DbTable != "")
                            {

                                
                                // rows
                                for (int counter = 1; counter <= htmlobject.Rows; counter++)
                                {
                                    List<string> dbcolumns = new List<string>();
                                    List<string> dbvalues = new List<string>();
                                    List<string> dbkeys = new List<string>();
                                    HtmlInputHidden hiddenid = new HtmlInputHidden();

                                    // empty sb
                                    sb.Remove(0, sb.Length);

                                    foreach (Column c in htmlobject.Columns.Items)
                                    {

                                        if (c.Type != "")
                                        {
                                            HtmlInputCheckBox checkbox = new HtmlInputCheckBox();
                                            HtmlInputText textbox = new HtmlInputText();
                                            HtmlTextArea textarea = new HtmlTextArea();
                                            HtmlSelect dropdown = new HtmlSelect();

                                            hiddenid = (HtmlInputHidden)Page.FindControl("ctl_hi" + section.ID + "_" + htmlobject.ID + "_" + counter);

                                            if (hiddenid != null)
                                            {
                                                dbkeys.Add(hiddenid.Value);

                                                // checkbox
                                                if (c.Type == "checkbox")
                                                {
                                                    checkbox = (HtmlInputCheckBox)Page.FindControl("ctl_hcv" + section.ID + "_" + htmlobject.ID + "_" + htmlobject.Columns.Items.IndexOf(c) + "_" + counter);
                                                    dbcolumns.Add(c.DbField);

                                                    // check the status of the checkbox via Request.Form
                                                    string checkstat = Request.Form["ctl_hcv" + section.ID + "_" + htmlobject.ID + "_" + htmlobject.Columns.Items.IndexOf(c) + "_" + counter];
                                                    dbvalues.Add(checkstat == "on" ? "1" : "0");

                                                }
                                                if (c.Type == "dropdown")
                                                {
                                                    dropdown = (HtmlSelect)Page.FindControl("ctl_hv" + section.ID + "_" + htmlobject.ID + "_" + htmlobject.Columns.Items.IndexOf(c) + "_" + counter);
                                                    dbcolumns.Add(c.DbField);
                                                    dbvalues.Add(dropdown.Value);
                                                }
                                                if (c.Type == "inputbox")
                                                {
                                                    textbox = (HtmlInputText)Page.FindControl("ctl_hv" + section.ID + "_" + htmlobject.ID + "_" + htmlobject.Columns.Items.IndexOf(c) + "_" + counter);
                                                    dbcolumns.Add(c.DbField);
                                                    dbvalues.Add(CleanInput.Sanitize(textbox.Value).Replace("'", "''"));
                                                }
                                                if (c.Type == "inputbox-number")
                                                {
                                                    textbox = (HtmlInputText)Page.FindControl("ctl_hv" + section.ID + "_" + htmlobject.ID + "_" + htmlobject.Columns.Items.IndexOf(c) + "_" + counter);
                                                    dbcolumns.Add(c.DbField);
                                                    dbvalues.Add("CONVERT:"+
                                                                 CleanInput.Sanitize(textbox.Value).Replace("'", "''"));
                                                }

                                                if (c.Type == "textarea")
                                                {
                                                    textarea = (HtmlTextArea)Page.FindControl("ctl_hv" + section.ID + "_" + htmlobject.ID + "_" + htmlobject.Columns.Items.IndexOf(c) + "_" + counter);
                                                    dbcolumns.Add(c.DbField);
                                                    dbvalues.Add(CleanInput.Sanitize(textarea.Value).Replace("'", "''"));
                                                }
                                            }
                                        }
                                    }

                                    // build sql
                                    if (hiddenid != null)
                                    {
                                        int cntr = 0;
                                        sb.Append("update ");
                                        sb.Append(htmlobject.DbTable);
                                        sb.Append(" set ");
                                        foreach (string dbcolumn in dbcolumns)
                                        {
                                            sb.Append(dbcolumn + " = ");
                                            // clean input

                                            if (dbvalues[cntr].StartsWith("CONVERT"))
                                            {
                                                string value = dbvalues[cntr].Split(':')[1];
                                               
                                               double theValue=  Double.Parse(value, NumberStyles.AllowThousands, System.Threading.Thread.CurrentThread.CurrentUICulture);
                                                  
                                                sb.Append(CleanInput.Sanitize(""+theValue) + ", ");


                                            }
                                            else
                                            {
                                                sb.Append("'" + CleanInput.Sanitize(dbvalues[cntr]) + "', ");
                                            }
                                            

                                            cntr++;
                                        }
                                        sb.Append("where ");
                                        sb.Append(htmlobject.DbKey + " = ");
                                        sb.Append("'" + dbkeys[0] + "'");

                                        sql = sb.ToString().Replace(", where", " where ");

                                        Debug.WriteLine("UPDATE TABLE, QUERY = " + sql);
                                        //update
                                        cs.Update(sql);
                                    }

                                }
                            }
                        }
                    }


                #region audit
                if (pageform.Audit.Enabled)
                {
                    // create audit table
                    cs.CreateAuditTable(pageform.Audit.Table);

                    // insert audit record
                    string[] url = Request.Url.ToString().Split('?');
                    StringBuilder sbaudit = new StringBuilder();
                    sbaudit.Append("insert ");
                    sbaudit.Append(pageform.Audit.Table);
                    sbaudit.Append("(action, usr, page, descr, ts) ");
                    sbaudit.Append("values ('");
                    sbaudit.Append(pageform.ActionType);
                    sbaudit.Append("','");
                    sbaudit.Append(SessionHandler.Usr.User);
                    sbaudit.Append("','");
                    sbaudit.Append(SessionHandler.Qstring.Pageform);
                    sbaudit.Append("','");
                    sbaudit.Append(url[0]);
                    sbaudit.Append("?page=");
                    sbaudit.Append(pageform.Audit.View);
                    sbaudit.Append("&filter=");
                    sbaudit.Append(SessionHandler.Qstring.Filter);
                    sbaudit.Append("','");
                    sbaudit.Append(System.DateTime.Now);
                    sbaudit.Append("')");
                    cs.Update(sbaudit.ToString());

                }
                #endregion

                Response.Redirect(SessionHandler.referrer, false);
                break;
                    #endregion

            case "check":
                #region check
                foreach (Section section in pageform.Sections.Items)
                {
                    foreach (HtmlObject htmlobject in section.HtmlObjects.Items)
                    {
                        if (htmlobject.HtmlType == "table")
                        {
                            for (int counter = 1; counter <= htmlobject.Rows; counter++)
                            {
                                HtmlInputCheckBox checkbox = new HtmlInputCheckBox();
                                HtmlInputHidden hidden = new HtmlInputHidden();
                                checkbox = (HtmlInputCheckBox)Page.FindControl("ctl_c" + section.ID + "_" + htmlobject.ID + "_" + counter);
                                hidden = (HtmlInputHidden)Page.FindControl("ctl_h" + section.ID + "_" + htmlobject.ID + "_" + counter);
                                string dbtable;
                                if (hidden != null)
                                {
                                    // empty sb
                                    sb.Remove(0, sb.Length);

                                    // pick the right table
                                    if (htmlobject.DbTable != "")
                                    {
                                        dbtable = htmlobject.DbTable;
                                    }
                                    else
                                    {
                                        dbtable = pageform.Database.Table;
                                    }

                                    // check the status of the checkbox via Request.Form
                                    string checkstat = Request.Form["ctl_c" + section.ID + "_" + htmlobject.ID + "_" + counter];
                                    bool ischecked = checkstat == "on" ? true : false;

                                    // is changed?
                                    sb.Append("select ");
                                    sb.Append(pageform.Database.PrimaryKey);
                                    sb.Append(" from ");
                                    sb.Append(dbtable);
                                    sb.Append(" where ");
                                    sb.Append(pageform.Database.PrimaryKey);
                                    sb.Append(" = ");
                                    sb.Append("'" + hidden.Value + "'");
                                    sb.Append(" and ");
                                    sb.Append(htmlobject.Check);
                                    sb.Append(" <> ");
                                    //sb.Append("'" + (checkbox.Checked == true ? 1 : 0) + "'");
                                    sb.Append("'" + (ischecked == true ? 1 : 0) + "'");

                                    sql = sb.ToString();
                                    dt = cs.Select(sql);
                                    bool changed = dt.Rows.Count > 0 ? true : false;

                                    if (changed)
                                    {
                                        // empty sb
                                        sb.Remove(0, sb.Length);

                                        // construct sql
                                        sb.Append("update ");
                                        sb.Append(dbtable);
                                        sb.Append(" set ");
                                        sb.Append(htmlobject.Check);
                                        sb.Append(" = ");
                                        sb.Append("'" + (ischecked == true ? 1 : 0) + "'");
                                        //sb.Append("'" + (checkbox.Checked == true ? 1 : 0) + "'");
                                        sb.Append(" where ");
                                        sb.Append(pageform.Database.PrimaryKey);
                                        sb.Append(" = ");
                                        sb.Append("'" + hidden.Value + "'");
                                        sql = sb.ToString();
                                        // update
                                        cs.Update(sql);

                                        #region audit
                                        if (pageform.Audit.Enabled)
                                        {
                                            // create audit table
                                            cs.CreateAuditTable(pageform.Audit.Table);

                                            // insert audit record
                                            string[] url = Request.Url.ToString().Split('?');
                                            StringBuilder sbaudit = new StringBuilder();
                                            sbaudit.Append("insert ");
                                            sbaudit.Append(pageform.Audit.Table);
                                            sbaudit.Append("(action, usr, page, descr, ts) ");
                                            sbaudit.Append("values ('");
                                            sbaudit.Append(pageform.ActionType);
                                            sbaudit.Append("','");
                                            sbaudit.Append(SessionHandler.Usr.User);
                                            sbaudit.Append("','");
                                            sbaudit.Append(SessionHandler.Qstring.Pageform);
                                            sbaudit.Append("','");
                                            sbaudit.Append(url[0]);
                                            sbaudit.Append("?page=");
                                            sbaudit.Append(pageform.Audit.View);
                                            sbaudit.Append("&filter=");
                                            sbaudit.Append(hidden.Value);
                                            sbaudit.Append("','");
                                            sbaudit.Append(System.DateTime.Now);
                                            sbaudit.Append("')");
                                            cs.Update(sbaudit.ToString());

                                        }
                                        #endregion
                                    }
                                }

                            }
                        }
                    }
                }

                Response.Redirect(SessionHandler.referrer, false);
                break;
                #endregion

            case "authorize":
                #region authorize
                foreach (Section section in pageform.Sections.Items)
                {
                    foreach (HtmlObject htmlobject in section.HtmlObjects.Items)
                    {
                        if (htmlobject.HtmlType == "table")
                        {
                            // empty sb
                            sb.Remove(0, sb.Length);
                            sb2.Remove(0, sb.Length);

                            // insert statement
                            sb.Append("insert ");
                            sb.Append(pageform.Database.Table + "_aut");
                            sb.Append(" (usr, moddate,");
                            sb2.Append(" values (");
                            sb2.Append("'" + SessionHandler.Usr.User + "',");
                            sb2.Append("'" + System.DateTime.Now.Year.ToString() + "-");
                            sb2.Append(System.DateTime.Now.Month.ToString() + "-");
                            sb2.Append(System.DateTime.Now.Day.ToString() + " ");
                            sb2.Append(System.DateTime.Now.Hour + ":");
                            sb2.Append(System.DateTime.Now.Minute + ":");
                            sb2.Append(System.DateTime.Now.Second + "',");


                            foreach (Header h in htmlobject.Headers.Items)
                            {
                                HtmlInputCheckBox checkbox = new HtmlInputCheckBox();
                                HtmlInputHidden hidden = new HtmlInputHidden();

                                checkbox = (HtmlInputCheckBox)Page.FindControl("ctl_a" + section.ID + "_" + htmlobject.ID + "_" + h.ID);
                                hidden = (HtmlInputHidden)Page.FindControl("ctl_hc" + section.ID + "_" + htmlobject.ID + "_" + h.ID);

                                if (hidden != null)
                                {
                                    // check the status of the checkbox via Request.Form
                                    string checkstat = Request.Form["ctl_a" + section.ID + "_" + htmlobject.ID + "_" + h.ID];
                                    bool ischecked = checkstat == "on" ? true : false;

                                    sb.Append(hidden.Value);
                                    sb.Append(",");
                                    sb2.Append("'" + (ischecked == true ? 1 : 0) + "'");
                                    sb2.Append(",");
                                }

                            }

                            sql = sb.ToString().Substring(0, sb.ToString().Length - 1) + ")";
                            sql += sb2.ToString().Substring(0, sb2.ToString().Length - 1) + ")";

                            // create table the first time
                            Headers hdrs = new Headers();
                            hdrs = htmlobject.Headers;
                            cs.CreateAuthTable(pageform.Database.Table, hdrs);

                            // insert record
                            cs.Update(sql);

                        }
                    }
                }

                Response.Redirect(SessionHandler.referrer, false);
                break;
                #endregion

            case "copy":
                #region copy
                foreach (Section section in pageform.Sections.Items)
                {
                    foreach (HtmlObject htmlobject in section.HtmlObjects.Items)
                    {
                        if ((htmlobject.HtmlType == "inputbox" || htmlobject.HtmlType == "email" || htmlobject.HtmlType == "password" || htmlobject.HtmlType == "date" || htmlobject.HtmlType == "number" )&& (htmlobject.Enabled != "" || htmlobject.DbField.ToLower() == pageform.Database.PrimaryKey.ToLower()))
                        {
                            HtmlInputText textbox = new HtmlInputText();
                            textbox = (HtmlInputText)Page.FindControl("ctl" + section.ID + "_" + htmlobject.ID);
                            if (htmlobject.Datatype == "date" && textbox.Value.Length > 0)
                            {
                                textbox.Value = Convert.ToDateTime(textbox.Value).ToString("yyyy-MM-dd");
                            }
                            values.Add(CleanInput.Sanitize(textbox.Value).Replace("'", "''"));
                        }

                        if (htmlobject.HtmlType == "checkbox" && htmlobject.Enabled != "")
                        {
                            HtmlInputCheckBox checkbox = new HtmlInputCheckBox();
                            checkbox = (HtmlInputCheckBox)Page.FindControl("ctl" + section.ID + "_" + htmlobject.ID);
                            values.Add(checkbox.Checked == true ? "1" : "0");
                        }

                        if (htmlobject.HtmlType == "dropdown" && htmlobject.Enabled != "")
                        {
                            HtmlSelect dropdown = new HtmlSelect();
                            dropdown = (HtmlSelect)Page.FindControl("ctl" + section.ID + "_" + htmlobject.ID);
                            values.Add(dropdown.Value);
                        }

                        if (htmlobject.HtmlType == "textarea" && htmlobject.Enabled != "")
                        {
                            HtmlTextArea textbox = new HtmlTextArea();
                            textbox = (HtmlTextArea)Page.FindControl("ctl" + section.ID + "_" + htmlobject.ID);
                            values.Add(CleanInput.Sanitize(textbox.Value).Replace("'", "''"));
                        }

                        if (htmlobject.HtmlType == "button" && htmlobject.Action == "copy" && htmlobject.Navigate != "")
                        {
                            SessionHandler.referrer = SessionHandler.Navigation.GetUrlByPage(htmlobject.Navigate);
                        }

                    }
                }
                foreach (Section section in pageform.Sections.Items)
                {
                    foreach (HtmlObject htmlobject in section.HtmlObjects.Items)
                    {
                        if (htmlobject.DbField != "" && (htmlobject.Enabled != "" || htmlobject.DbField.ToLower() == pageform.Database.PrimaryKey.ToLower()))
                            fields.Add(htmlobject.DbField);
                    }
                }

                // get the higest primarykey
                sql = "select max(" + pageform.Database.PrimaryKey + ") from " + pageform.Database.Table;
                dt = cs.Select(sql);

                if (int.TryParse(dt.Rows[0][0].ToString(), out test))
                {
                    int pk = int.Parse(dt.Rows[0][0].ToString()) + 1;
                    primarykey = pk.ToString();
                }
                else
                {
                    string pk = "0 " + dt.Rows[0][0].ToString();
                    primarykey = pk;
                }

                // construct sql
                sb.Append("insert ");
                sb.Append(pageform.Database.Table);
                sb.Append(" (");
                foreach (string field in fields)
                {
                    sb.Append(field.Replace("##", "") + ", ");
                }

                sb.Append(") values (");
                i = 0;
                foreach (string value in values)
                {
                    if (fields[i].ToLower() == pageform.Database.PrimaryKey.ToLower())
                    {
                        sb.Append("'" + primarykey + "', ");
                    }
                    else
                    {
                        if (fields[i].StartsWith("##"))
                        {
                            // with encryption
                            sb.Append("dbo.sf_encrypt('" + value + "'), ");
                        }
                        else
                        {
                            // no encryption
                            sb.Append("'" + value + "', ");
                        }
                    }
                    i++;
                }

                sb.Append(")");
                sql = sb.ToString().Replace(", )", ")");

                // update
                cs.Update(sql);

                #region audit
                if (pageform.Audit.Enabled)
                {
                    // create audit table
                    cs.CreateAuditTable(pageform.Audit.Table);

                    // insert audit record
                    string[] url = Request.Url.ToString().Split('?');
                    StringBuilder sbaudit = new StringBuilder();
                    sbaudit.Append("insert ");
                    sbaudit.Append(pageform.Audit.Table);
                    sbaudit.Append("(action, usr, page, descr, ts) ");
                    sbaudit.Append("values ('");
                    sbaudit.Append(pageform.ActionType);
                    sbaudit.Append("','");
                    sbaudit.Append(SessionHandler.Usr.User);
                    sbaudit.Append("','");
                    sbaudit.Append(SessionHandler.Qstring.Pageform);
                    sbaudit.Append("','");
                    sbaudit.Append(url[0]);
                    sbaudit.Append("?page=");
                    sbaudit.Append(pageform.Audit.View);
                    sbaudit.Append("&filter=");
                    sbaudit.Append(primarykey);
                    sbaudit.Append("','");
                    sbaudit.Append(System.DateTime.Now);
                    sbaudit.Append("')");
                    cs.Update(sbaudit.ToString());

                }
                #endregion

                Response.Redirect(SessionHandler.referrer, false);
                break;
                #endregion

            case "insert":
                #region insert
                foreach (Section section in pageform.Sections.Items)
                {
                    foreach (HtmlObject htmlobject in section.HtmlObjects.Items)
                    {
                        if ((htmlobject.HtmlType == "inputbox" || htmlobject.HtmlType == "email" || htmlobject.HtmlType == "password" || htmlobject.HtmlType == "date" || htmlobject.HtmlType == "number") && (htmlobject.Enabled != "" || htmlobject.DbField.ToLower() == pageform.Database.PrimaryKey.ToLower()))
                        {
                            HtmlInputText textbox = new HtmlInputText();
                            textbox = (HtmlInputText)Page.FindControl("ctl" + section.ID + "_" + htmlobject.ID);
                            if (htmlobject.Datatype == "date" && textbox.Value.Length > 0)
                            {
                                textbox.Value = Convert.ToDateTime(textbox.Value).ToString("yyyy-MM-dd HH:mm:ss");
                            }
                            values.Add(CleanInput.Sanitize(textbox.Value).Replace("'", "''"));
                        }

                        if (htmlobject.HtmlType == "checkbox" && htmlobject.Enabled != "")
                        {
                            HtmlInputCheckBox checkbox = new HtmlInputCheckBox();
                            checkbox = (HtmlInputCheckBox)Page.FindControl("ctl" + section.ID + "_" + htmlobject.ID);
                            values.Add(checkbox.Checked == true ? "1" : "0");
                        }

                        if (htmlobject.HtmlType == "dropdown" && htmlobject.Enabled != "")
                        {
                            HtmlSelect dropdown = new HtmlSelect();
                            dropdown = (HtmlSelect)Page.FindControl("ctl" + section.ID + "_" + htmlobject.ID);
                            values.Add(dropdown.Value);
                        }

                        if (htmlobject.HtmlType == "textarea" && htmlobject.Enabled != "")
                        {
                            HtmlTextArea textbox = new HtmlTextArea();
                            textbox = (HtmlTextArea)Page.FindControl("ctl" + section.ID + "_" + htmlobject.ID);
                            values.Add(CleanInput.Sanitize(textbox.Value).Replace("'", "''"));
                        }

                        if (htmlobject.HtmlType == "button" && htmlobject.Action == "update" && htmlobject.Navigate != "")
                        {
                            SessionHandler.referrer = SessionHandler.Navigation.GetUrlByPage(htmlobject.Navigate);
                        }
                    }
                }
                foreach (Section section in pageform.Sections.Items)
                {
                    foreach (HtmlObject htmlobject in section.HtmlObjects.Items)
                    {
                        if (htmlobject.DbField != "" && (htmlobject.Enabled != "" || htmlobject.DbField.ToLower() == pageform.Database.PrimaryKey.ToLower()))
                            fields.Add(htmlobject.DbField);
                    }
                }

                // get the higest primarykey
                sql = "select max(" + pageform.Database.PrimaryKey + ") from " + pageform.Database.Table;
                dt = cs.Select(sql);

                if (int.TryParse(dt.Rows[0][0].ToString(), out test))
                {
                    int pk = int.Parse(dt.Rows[0][0].ToString()) + 1;
                    primarykey = pk.ToString();
                }
                else
                {
                    string pk = "0" + dt.Rows[0][0].ToString();
                    primarykey = pk;
                }

                // construct sql
                sb.Append("insert ");
                sb.Append(pageform.Database.Table);
                sb.Append(" (");
                foreach (string field in fields)
                {
                    sb.Append(field.Replace("##", "") + ", ");
                }

                sb.Append(") values (");
                i = 0;
                foreach (string value in values)
                {
                    if (fields[i].ToLower() == pageform.Database.PrimaryKey.ToLower())
                    {
                        sb.Append("'" + primarykey + "', ");
                    }
                    else
                    {
                        if (fields[i].StartsWith("##"))
                        {
                            // with encryption
                            sb.Append("dbo.sf_encrypt('" + value.Replace("##", "") + "'), ");
                        }
                        else
                        {
                            // no encryption
                            sb.Append("'" + value + "', ");
                        }
                    }
                    i++;
                }

                sb.Append(")");
                sql = sb.ToString().Replace(", )", ")");

                // update
                cs.Update(sql);

                #region audit
                if (pageform.Audit.Enabled)
                {
                    // create audit table
                    cs.CreateAuditTable(pageform.Audit.Table);

                    // insert audit record
                    string[] url = Request.Url.ToString().Split('?');
                    StringBuilder sbaudit = new StringBuilder();
                    sbaudit.Append("insert ");
                    sbaudit.Append(pageform.Audit.Table);
                    sbaudit.Append("(action, usr, page, descr, ts) ");
                    sbaudit.Append("values ('");
                    sbaudit.Append(pageform.ActionType);
                    sbaudit.Append("','");
                    sbaudit.Append(SessionHandler.Usr.User);
                    sbaudit.Append("','");
                    sbaudit.Append(SessionHandler.Qstring.Pageform);
                    sbaudit.Append("','");
                    sbaudit.Append(url[0]);
                    sbaudit.Append("?page=");
                    sbaudit.Append(pageform.Audit.View);
                    sbaudit.Append("&filter=");
                    sbaudit.Append(primarykey);
                    sbaudit.Append("','");
                    sbaudit.Append(System.DateTime.Now);
                    sbaudit.Append("')");
                    cs.Update(sbaudit.ToString());

                }
                #endregion

                Response.Redirect(SessionHandler.referrer, false);
                break;
                #endregion

            case "cancel":
                #region cancel
                foreach (Section section in pageform.Sections.Items)
                {
                    foreach (HtmlObject htmlobject in section.HtmlObjects.Items)
                    {
                        if (htmlobject.HtmlType == "button" && htmlobject.Action == "cancel" && htmlobject.Navigate != "")
                        {
                            SessionHandler.referrer = SessionHandler.Navigation.GetUrlByPage(htmlobject.Navigate);
                        }
                    }
                }

                if (pageform.ActionType == "search")
                {
                    Response.Redirect("~/index.aspx?page=" + SessionHandler.Qstring.Pageform + "&filter=nofilter", false);
                }
                else
                {
                    Response.Redirect(SessionHandler.referrer, false);
                }
                break;
                #endregion

            case "delete":
                #region delete
                if (Request.QueryString.Count == 2)
                {
                    Response.Redirect("~/index.aspx?page=" + SessionHandler.Qstring.Pageform + "&filter=" + SessionHandler.Qstring.Filter + "&confirm=yes", false);
                }
                else
                {
                    if (Request.QueryString.Get(2) == "yes")
                    {
                        sb.Append("delete from ");
                        sb.Append(pageform.Database.Table);
                        sb.Append(" where ");
                        // added condition
                        sb.Append(pageform.Database.Condition);
                        sb.Append(" and ");
                        sb.Append(pageform.Database.PrimaryKey);
                        sb.Append(" = ");
                        sb.Append(SessionHandler.Qstring.Filter);

                        sql = sb.ToString();

                        // replace with environment variables
                        sql = sql.Replace("@user", "'" + SessionHandler.Usr.User + "'");
                        sql = sql.Replace("@module", "'" + SessionHandler.Qstring.Pageform + "'");
                        sql = sql.Replace("@filter", "'" + SessionHandler.Qstring.Filter.Split('|')[0] + "'");
                        sql = sql.Replace("@role", "'" + SessionHandler.accesslevel + "'");
                        sql = sql.Replace("@department", "'" + SessionHandler.Usr.Department + "'");

                        // delete
                        cs.Update(sql);
                    }



                    #region audit
                    if (pageform.Audit.Enabled)
                    {
                        // create audit table
                        cs.CreateAuditTable(pageform.Audit.Table);

                        // insert audit record
                        string[] url = Request.Url.ToString().Split('?');
                        StringBuilder sbaudit = new StringBuilder();
                        sbaudit.Append("insert ");
                        sbaudit.Append(pageform.Audit.Table);
                        sbaudit.Append("(action, usr, page, descr, ts) ");
                        sbaudit.Append("values ('");
                        sbaudit.Append(pageform.ActionType);
                        sbaudit.Append("','");
                        sbaudit.Append(SessionHandler.Usr.User);
                        sbaudit.Append("','");
                        sbaudit.Append(SessionHandler.Qstring.Pageform);
                        sbaudit.Append("','");
                        sbaudit.Append("deleted: ");
                        sbaudit.Append(pageform.Database.PrimaryKey);
                        sbaudit.Append(" ");
                        sbaudit.Append(SessionHandler.Qstring.Filter);
                        sbaudit.Append("','");
                        sbaudit.Append(System.DateTime.Now);
                        sbaudit.Append("')");
                        cs.Update(sbaudit.ToString());

                    }
                    #endregion

                    foreach (Section section in pageform.Sections.Items)
                    {
                        foreach (HtmlObject htmlobject in section.HtmlObjects.Items)
                        {
                            if (htmlobject.HtmlType == "button" && htmlobject.Action == "delete" && htmlobject.Navigate != "")
                            {
                                SessionHandler.referrer = SessionHandler.Navigation.GetUrlByPage(htmlobject.Navigate);
                            }
                        }
                    }

                    Response.Redirect(SessionHandler.referrer, false);
                }
                break;
                #endregion

            case "logoff":
                #region logoff
                Session.Abandon();
                Session.Clear();
                Session.RemoveAll();
                Response.Redirect("~/index.aspx", true);
                break;
                #endregion

            case "mail":
                #region mail

                foreach (Section section in pageform.Sections.Items)
                {

                    if (section.ID == linkbutton.ID.Substring(0, linkbutton.ID.IndexOf("_")).Replace("ctl", ""))
                    {

                        foreach (HtmlObject htmlobject in section.HtmlObjects.Items)
                        {
                            // parameters
                            if (htmlobject.HtmlType == "inputbox" && htmlobject.Target == "parameter")
                            {
                                HtmlInputText parameter = new HtmlInputText();
                                parameter = (HtmlInputText)Page.FindControl("ctl" + section.ID + "_" + htmlobject.ID);
                                parameters.Add(parameter.Value);
                            }

                            if (htmlobject.HtmlType == "dropdown" && htmlobject.Target == "parameter")
                            {
                                HtmlSelect dropdown = new HtmlSelect();
                                dropdown = (HtmlSelect)Page.FindControl("ctl" + section.ID + "_" + htmlobject.ID);
                                parameters.Add(dropdown.Value);
                            }

                            // path
                            if (htmlobject.HtmlType == "button" && htmlobject.Action == "file")
                            {
                                filepath = htmlobject.FilePath;
                            }
                            // filename
                            if (htmlobject.HtmlType == "inputbox" && htmlobject.Target == "file")
                            {
                                HtmlInputText textbox = (HtmlInputText)Page.FindControl("ctl" + section.ID + "_" + htmlobject.ID);
                                file = FileName.MakeFileName(textbox.Value, parameters);
                            }

                            // email
                            if (htmlobject.Target == "email")
                            {
                                HtmlInputText textbox = (HtmlInputText)Page.FindControl("ctl" + section.ID + "_" + htmlobject.ID);
                                email = textbox.Value;
                            }

                            #region datatable
                            if (htmlobject.Display == "grid" && SessionHandler.datatable != null)
                            {
                                HtmlGenericControl outputpanel = new HtmlGenericControl("div");
                                outputpanel = (HtmlGenericControl)Page.FindControl("ctl" + section.ID + "_" + htmlobject.ID);
                                outputpanel.Controls.Add(FormBuilder.BuildTable(SessionHandler.datatable, section));
                                // resize
                                outputpanel.Style["height"] = FormBuilder.BuildTable(SessionHandler.datatable, section).Style["height"] + "px";
                            }
                            #endregion

                        }

                        if (file.Length > 0 && email.Length > 0 && File.Exists(Server.MapPath(@"~/" + filepath + file)))
                        {
                            // mail
                            MailHandler.SendMail("noreply@redevco.com", email, "Your interface results", Server.MapPath(@"~/" + filepath + file));
                            Alert.Show("File is mailed to " + email);

                            // clean up 
                            File.Delete(Server.MapPath(@"~/" + filepath + file));

                            #region audit
                            if (pageform.Audit.Enabled)
                            {
                                // create audit table
                                cs.CreateAuditTable(pageform.Audit.Table);

                                // insert audit record
                                string[] url = Request.Url.ToString().Split('?');
                                StringBuilder sbaudit = new StringBuilder();
                                sbaudit.Append("insert ");
                                sbaudit.Append(pageform.Audit.Table);
                                sbaudit.Append("(action, usr, page, descr, ts) ");
                                sbaudit.Append("values ('");
                                sbaudit.Append(pageform.ActionType);
                                sbaudit.Append("','");
                                sbaudit.Append(SessionHandler.Usr.User);
                                sbaudit.Append("','");
                                sbaudit.Append(SessionHandler.Qstring.Pageform);
                                sbaudit.Append("','");
                                sbaudit.Append("file mailed to ");
                                sbaudit.Append(email);
                                sbaudit.Append("','");
                                sbaudit.Append(System.DateTime.Now);
                                sbaudit.Append("')");
                                cs.Update(sbaudit.ToString());

                            }
                            #endregion

                        }
                        else
                        {
                            Alert.Show("Nothing to mail");
                        }
                    }
                }
                break;
                #endregion

            case "back":
                #region back
                SessionHandler.referrer = SessionHandler.Navigation.GetPrevious();

                foreach (Section section in pageform.Sections.Items)
                {
                    foreach (HtmlObject htmlobject in section.HtmlObjects.Items)
                    {
                        if (htmlobject.HtmlType == "button" && htmlobject.Action == "back" && htmlobject.Navigate != "")
                        {
                            SessionHandler.referrer = SessionHandler.Navigation.GetUrlByPage(htmlobject.Navigate);
                        }
                    }
                }

                Response.Redirect(SessionHandler.referrer, false);
                break;
                #endregion

            case "file":
                #region file
                StringBuilder sboutput = new StringBuilder();
                HtmlTextArea toutput = new HtmlTextArea();
                HtmlInputText fileinput = new HtmlInputText();

                string delimeter = string.Empty;
                bool grid = false;

                foreach (Section section in pageform.Sections.Items)
                {

                    if (section.ID == linkbutton.ID.Substring(0, linkbutton.ID.IndexOf("_")).Replace("ctl", ""))
                    {

                        foreach (HtmlObject htmlobject in section.HtmlObjects.Items)
                        {

                            // parameters
                            if (htmlobject.HtmlType == "inputbox" && htmlobject.Target == "parameter")
                            {
                                HtmlInputText parameter = new HtmlInputText();
                                parameter = (HtmlInputText)Page.FindControl("ctl" + section.ID + "_" + htmlobject.ID);
                                parameters.Add(parameter.Value);
                            }
                            if (htmlobject.HtmlType == "dropdown" && htmlobject.Target == "parameter")
                            {
                                HtmlSelect dropdown = new HtmlSelect();
                                dropdown = (HtmlSelect)Page.FindControl("ctl" + section.ID + "_" + htmlobject.ID);
                                parameters.Add(dropdown.Value);
                            }

                            // delimeter
                            if (htmlobject.HtmlType == "output")
                            {
                                delimeter = htmlobject.Delimeter == "" ? "comma" : htmlobject.Delimeter;
                                grid = htmlobject.Display == "grid" ? true : false;
                                // for textareas only
                                if (!grid)
                                {
                                    toutput = (HtmlTextArea)Page.FindControl("ctl" + section.ID + "_" + htmlobject.ID);
                                }
                            }

                            // path
                            if (htmlobject.HtmlType == "button" && htmlobject.Action == "file")
                            {
                                filepath = htmlobject.FilePath;

                            }
                            // filename
                            if (htmlobject.HtmlType == "inputbox" && htmlobject.Target == "file")
                            {
                                fileinput = (HtmlInputText)Page.FindControl("ctl" + section.ID + "_" + htmlobject.ID);
                                file = FileName.MakeFileName(fileinput.Value, parameters);
                            }

                            #region datatable
                            if (htmlobject.Display == "grid" && SessionHandler.datatable != null)
                            {
                                HtmlGenericControl outputpanel = new HtmlGenericControl("div");
                                outputpanel = (HtmlGenericControl)Page.FindControl("ctl" + section.ID + "_" + htmlobject.ID);
                                outputpanel.Controls.Add(FormBuilder.BuildTable(SessionHandler.datatable, section));
                                // resize
                                outputpanel.Style["height"] = FormBuilder.BuildTable(SessionHandler.datatable, section).Style["height"] + "px";
                            }
                            #endregion

                        }

                        delimeter = delimeter.Replace("comma", ",");
                        delimeter = delimeter.Replace("tab", "\t");
                        delimeter = delimeter.Replace("semicolon", ";");

                        foreach (DataRow r in SessionHandler.datatable.Rows)
                        {
                            for (i = 0; i <= SessionHandler.datatable.Columns.Count - 1; i++)
                            {
                                sboutput.Append(r[i]);
                                sboutput.Append(delimeter);
                            }
                            sboutput.Append("{n}");
                        }

                        // file
                        result = sboutput.ToString().Replace(delimeter + "{n}", "\r\n");

                        // is not disabled and not grid take the value from the textarea
                        if (!toutput.Disabled && !grid)
                        {
                            result = toutput.Value;
                        }

                        // save
                        if (file.Length > 0 && result.Length > 0)
                        {
                            File.WriteAllText(Server.MapPath(@"~/" + filepath + file), result);
                            Alert.Show("File is saved to " + filepath + file);

                            #region audit
                            if (pageform.Audit.Enabled)
                            {
                                // create audit table
                                cs.CreateAuditTable(pageform.Audit.Table);

                                // insert audit record
                                string[] url = Request.Url.ToString().Split('?');
                                StringBuilder sbaudit = new StringBuilder();
                                sbaudit.Append("insert ");
                                sbaudit.Append(pageform.Audit.Table);
                                sbaudit.Append("(action, usr, page, descr, ts) ");
                                sbaudit.Append("values ('");
                                sbaudit.Append(pageform.ActionType);
                                sbaudit.Append("','");
                                sbaudit.Append(SessionHandler.Usr.User);
                                sbaudit.Append("','");
                                sbaudit.Append(SessionHandler.Qstring.Pageform);
                                sbaudit.Append("','");
                                sbaudit.Append("file saved: ");
                                sbaudit.Append(file);
                                sbaudit.Append("','");
                                sbaudit.Append(System.DateTime.Now);
                                sbaudit.Append("')");
                                cs.Update(sbaudit.ToString());

                            }
                            #endregion

                            ErHandler.errorMsg = "File is saved to " + filepath + file;
                            ErHandler.showError();
                        }
                        else
                        {
                            Alert.Show("Nothing to save");
                        }

                        if (!grid)
                        {
                            toutput.Value = result;
                        }
                    }
                }
                break;
                #endregion

            default:
                break;
        }

    }

    protected void Btn_Click_Upload(object s, EventArgs e)
    {
        PageForm pageform = new PageForm();
        pageform.Load(doc);
        HtmlAnchor linkbutton = (HtmlAnchor)s;
        FileUpload fu = new FileUpload();
        string reload = linkbutton.Name.Split('&')[1];
        fu = (FileUpload)Page.FindControl("fileupload");
        ConnectionString cs = new ConnectionString(SessionHandler.connection);

        foreach (Section section in pageform.Sections.Items)
        {

            if (section.ID == linkbutton.ID.Substring(0, linkbutton.ID.IndexOf("_")).Replace("ctl", "") || 1 == 1)
            {

                foreach (HtmlObject htmlobject in section.HtmlObjects.Items)
                {
                    // parameters
                    if ((htmlobject.HtmlType == "inputbox") && htmlobject.Target == "file")
                    {
                        HtmlInputText parameter = new HtmlInputText();
                        parameter = (HtmlInputText)Page.FindControl("ctl" + section.ID + "_" + htmlobject.ID);

                        try
                        {
                            if (fu.HasFile)
                            {

                                #region import excel
                                if (parameter.Value.IndexOf("xls") > -1)
                                {
                                    string excelPath = Server.MapPath("~/files/") + SessionHandler.Usr.User + "_" + DateTime.Now.ToOADate() * 10000000000 + "_" + fu.FileName;
                                    fu.SaveAs(excelPath);

                                    string conString = string.Empty;
                                    string extension = Path.GetExtension(fu.PostedFile.FileName);

                                    if (extension == ".xls" || extension == ".xlsx")
                                    {
                                        switch (extension)
                                        {
                                            case ".xls": //Excel 97-03
                                                conString = ConfigurationManager.ConnectionStrings["Excel03ConString"].ConnectionString;
                                                break;
                                            case ".xlsx": //Excel 07 or higher
                                                conString = ConfigurationManager.ConnectionStrings["Excel07+ConString"].ConnectionString;
                                                break;
                                        }

                                        conString = string.Format(conString, excelPath);

                                        using (OleDbConnection excel_con = new OleDbConnection(conString))
                                        {
                                            excel_con.Open();
                                            string sheet1 = excel_con.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null).Rows[0]["TABLE_NAME"].ToString();
                                            DataTable dtExcelData = new DataTable();
                                            using (OleDbDataAdapter oda = new OleDbDataAdapter("SELECT * FROM [" + sheet1 + "]", excel_con))
                                            {
                                                oda.Fill(dtExcelData);
                                            }
                                            excel_con.Close();

                                            #endregion

                                            #region mapping
                                            Mapping m = new Mapping();
                                            m.Load();
                                            string sql = string.Empty;
                                            StringBuilder fields = new StringBuilder();
                                            StringBuilder values = new StringBuilder();

                                            #endregion

                                            #region load in db

                                            string pl = m.Preload;
                                            if (pl.Length > 0)
                                            {
                                                pl = pl.Replace("@user", "'" + SessionHandler.Usr.User + "'");
                                                pl = pl.Replace("@module", "'" + SessionHandler.Qstring.Pageform + "'");
                                                pl = pl.Replace("@filter", "'" + SessionHandler.Qstring.Filter.Split('|')[0] + "'");
                                                pl = pl.Replace("@role", "'" + SessionHandler.accesslevel + "'");
                                                pl = pl.Replace("@department", "'" + SessionHandler.Usr.Department + "'");
                                                pl = pl.Replace("@file", "'" + parameter.Value + "'");
                                                cs.Update(pl);
                                            }

                                            // loop through the lines is there a header?
                                            for (int i1 = 0; i1 <= dtExcelData.Rows.Count - 1; i1++)
                                            {

                                                fields.Append(m.Table + " (");
                                                fields.Length = 0;
                                                values.Length = 0;
                                                sql = string.Empty;
                                                fields.Append("insert into ");
                                                fields.Append(m.Table + " (");

                                                // mapping columns
                                                foreach (ExcelColumn col in m.Columns)
                                                {
                                                    fields.Append(col.Field + ",");
                                                    if (col.ColumnOrder >= 0)
                                                    {
                                                        if (col.DataType == "character" && col.ColumnOrder <= dtExcelData.Rows.Count - 1)
                                                        {

                                                            // field length
                                                            int l = dtExcelData.Rows[i1][col.ColumnOrder].ToString().Length > col.Length ? col.Length : dtExcelData.Rows[i1][col.ColumnOrder].ToString().Length;
                                                            values.Append("'" + dtExcelData.Rows[i1][col.ColumnOrder].ToString().Substring(0, l).Replace("'", "''") + "',");
                                                        }

                                                        else
                                                        {
                                                            if (col.DataType == "date" && col.ColumnOrder <= dtExcelData.Rows.Count - 1)
                                                            {
                                                                string[] rd = dtExcelData.Rows[i1][col.ColumnOrder].ToString().Replace("/", "-").Replace(".", "-").Replace(" ", "-").Split('-');
                                                                string[] fd = col.Format.Replace("/", "-").Replace(".", "-").Replace(" ", "-").Split('-');
                                                                string tm = "12:00:00";
                                                                if (Array.IndexOf(fd, "hh:mm:ss") > -1)
                                                                {
                                                                    tm = rd[3];
                                                                }
                                                                values.Append("'" +
                                                                    rd[Array.IndexOf(fd, "yyyy")] + "-" +
                                                                    rd[Array.IndexOf(fd, "mm")] + "-" +
                                                                    rd[Array.IndexOf(fd, "dd")] + " " + tm + "',");
                                                            }
                                                            else // numeric
                                                            {
                                                                values.Append("'" + dtExcelData.Rows[i1][col.ColumnOrder].ToString().Replace("'", "''") + "',");
                                                            }
                                                        }
                                                    }
                                                    else
                                                    {
                                                        string dv = col.DefValue;
                                                        dv = dv.Replace("@user", SessionHandler.Usr.User);
                                                        dv = dv.Replace("@module", SessionHandler.Qstring.Pageform);
                                                        dv = dv.Replace("@filter", SessionHandler.Qstring.Filter.Split('|')[0]);
                                                        dv = dv.Replace("@role", SessionHandler.accesslevel);
                                                        dv = dv.Replace("@department", SessionHandler.Usr.Department);
                                                        dv = dv.Replace("@file", parameter.Value);
                                                        values.Append("'" + dv + "',");
                                                    }
                                                }

                                                // make sql
                                                sql = fields.ToString().TrimEnd(',');
                                                sql = sql + ") values (" + values.ToString().TrimEnd(',') + ");";
                                                // write to database
                                                cs.Update(sql);

                                            #endregion
                                            }

                                        }

                                        // success
                                        if (reload == "yes")
                                        {
                                            Alert.ShowAndReload("File successfully uploaded.");
                                        }
                                        else
                                        {
                                            Alert.Show("File successfully uploaded.");
                                        }

                                        ErHandler.errorMsg = "File successfully uploaded.";
                                        ErHandler.showError();
     
                                    }
                                    else
                                    {
                                        throw new CustomException("No valid file format.");
                                    }
                                }
                                else
                                {
                                    throw new CustomException("No valid file format.");
                                }
                            }
                            else
                            {
                                throw new CustomException("No file selected.");
                            }

                        }

                        catch (CustomException ex)
                        {
                            Alert.Show(ex.Message);
                            ErHandler.errorMsg = ex.Message;
                            ErHandler.showError();
                        }

                        catch (System.InvalidOperationException ex)
                        {
                            Alert.Show(ex.Message);
                            ErHandler.errorMsg = ex.Message;
                            ErHandler.showError();
                        }
                        catch (System.Exception ex)
                        {
                            Alert.Show(ex.Message);
                            ErHandler.errorMsg = ex.Message;
                            ErHandler.showError();
                        }
                    }
                }
            }
        }
    }
}