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
using System.Text;

//to be removed
using System.Diagnostics;
using System.Globalization;

namespace EggwiseLib
{
    public static class FormBuilder
    {
        public static HtmlGenericControl BuildTable(DataTable dt, Section section)
        {
            HtmlGenericControl panel = new HtmlGenericControl("div");
            panel.Attributes["class"] += " table_class";
            int firstrowheaders = 0;

            foreach (HtmlObject htmlobject in section.HtmlObjects.Items)
            {
                if (htmlobject.HtmlType == "output" && dt.Rows.Count > 0)
                {
                    #region paging

                    int factor = htmlobject.Rows == 0 ? 999 : htmlobject.Rows;

                    // make the urls
                    int from =
                        Convert.ToInt16(HttpContext.Current.Request.QueryString["start"] == null
                            ? "1"
                            : HttpContext.Current.Request.QueryString["start"]);
                    string url = HttpContext.Current.Request.RawUrl;
                    url = url.Substring(0, url.IndexOf("start") == -1 ? url.Length : url.IndexOf("start") - 1);
                    StringBuilder sbpaging = new StringBuilder();
                    sbpaging.Append("        <a class=\"paging\" href=\"");
                    sbpaging.Append(url);
                    sbpaging.Append("&start=");
                    sbpaging.Append(from == 1 ? 1 : from - 1);
                    sbpaging.Append("\">&nbsp;");
                    sbpaging.Append("<<");
                    sbpaging.Append("</a>\n        ");
                    int maxpages = 0;
                    foreach (DataRow r in dt.Rows)
                    {
                        if (dt.Rows.IndexOf(r)%factor == 0)
                        {
                            sbpaging.Append("<a class=\"paging\" href=\"");
                            sbpaging.Append(url);
                            sbpaging.Append("&start=");
                            sbpaging.Append((dt.Rows.IndexOf(r)/factor + 1));
                            sbpaging.Append("\">&nbsp;");
                            sbpaging.Append(dt.Rows.IndexOf(r)/factor + 1 == from
                                ? "<span style=\"font-weight:bold;\">"
                                : "");
                            sbpaging.Append(dt.Rows.IndexOf(r)/factor + 1);
                            sbpaging.Append(dt.Rows.IndexOf(r)/factor + 1 == from ? "</span>" : "");
                            sbpaging.Append("</a>\n        ");
                            maxpages++;
                        }
                    }

                    sbpaging.Append("<a class=\"paging\" href=\"");
                    sbpaging.Append(url);
                    sbpaging.Append("&start=");
                    sbpaging.Append(from == maxpages ? maxpages : from + 1);
                    sbpaging.Append("\">&nbsp;");
                    sbpaging.Append(">>");
                    sbpaging.Append("</a>");

                    #endregion

                    #region table

                    //Table table = new Table();
                    HtmlGenericControl table = new HtmlGenericControl("table");
                    table.ID = "tbl" + section.ID + "_" + htmlobject.ID;
                    //table.Width = htmlobject.Width;
                    table.Attributes["class"] = htmlobject.CssClass + " striped highlight responsive-table";
                    //table.CssClass = htmlobject.CssClass;

                    #endregion

                    #region headers

                    if (dt.Rows.Count > 0)
                    {
                        HtmlGenericControl thead = new HtmlGenericControl("thead");

                        TableHeaderRow headerrow = new TableHeaderRow();
                        DataRow dr = dt.Rows[0];

                        for (int i = 0; i < dt.Columns.Count; i++)
                        {
                            TableHeaderCell headercell = new TableHeaderCell();
                            headercell.CssClass = "colhead";
                            if (htmlobject.Headers.Items.Count > 0)
                            {
                                if (htmlobject.Headers.Items[0].Name == "yes")
                                {
                                    headercell.Text = dr[i].ToString();
                                    firstrowheaders = 1;
                                }
                                else
                                {
                                    Header h = htmlobject.Headers.Items[i];
                                    headercell.Text = h.Name;
                                    headercell.ID = h.ID;

                                    headerrow.Controls.Add(headercell);
                                }
                            }
                            else
                            {
                                headercell.Text = "Column " + i;
                            }
                            headercell.ID = "hdr_" + section.ID + i;
                            headerrow.Controls.Add(headercell);
                        }
                        thead.Controls.Add(headerrow);
                        table.Controls.Add(thead);

                        #endregion

                        #region rows

                        foreach (DataRow r in dt.Rows)
                            if (dt.Rows.IndexOf(r) >= firstrowheaders)
                            {
                                if (dt.Rows.IndexOf(r) >= (from - 1)*factor & dt.Rows.IndexOf(r) < from*factor)
                                {
                                    {
                                        TableRow tablerow = new TableRow();
                                        // colls

                                        for (int i = 0; i < dt.Columns.Count; i++)
                                        {
                                            TableCell tablecell = new TableCell();
                                            tablecell.Text = r[i].ToString();
                                            tablerow.Controls.Add(tablecell);
                                        }
                                        tablerow.CssClass = dt.Rows.IndexOf(r)%2 == 0 ? "even" : "odd";
                                        table.Controls.Add(tablerow);
                                    }
                                }
                            }
                    }

                    #endregion

                    LiteralControl paging = new LiteralControl(sbpaging.ToString());
                    panel.Controls.Add(paging);
                    panel.Controls.Add(CreateHTMLObjects.CreateNewLine(1, 8));
                    panel.Controls.Add(table);
                }
            }

            return panel;
        }


        private static void addValidation<T>(T input, HtmlObject h,
            string type) where T : HtmlControl
        {
            input.Attributes.Add("data-parsley-error-class", "invalid");
            input.Attributes.Add("data-parsley-trigger", "change");


            if (!string.IsNullOrEmpty(h.Mandatory))
            {
                if (h.Mandatory.ToLower() == "yes")
                {
                    input.Attributes.Add("required", "");
                }
            }


            if (!string.IsNullOrEmpty(h.Length))
            {
                input.Attributes.Add("data-parsley-maxlength", h.Length);
            }


            if (type == "number")
            {
                //input.Attributes.Remove("type");
                input.Attributes.Add("data-parsley-type", "number");
                input.Attributes.Remove("data-parsley-maxlength");
                input.Attributes.Add("data-parsley-max", h.Length);

                // add number validation
            }

            if (type == "date")
            {
                input.Attributes.Add("data-parsley-datevalidation", "MDY");
                //input.Attributes.Add("data-type", "dateIso");
                // add decimal validation
            }
        }


        public static HtmlGenericControl buildPageHeader(XmlDocument xml)
        {
            if (SessionHandler.Usr.Roles == null)
            {
                ErHandler.errorMsg = "Session has expired!";
                ErHandler.throwError();
            }


            PageHeader p = PageHeader.load(xml);


            HtmlGenericControl navWrapper = new HtmlGenericControl("div");
            navWrapper.Attributes["class"] += "nav-wrapper";


            HtmlGenericControl navLogo = new HtmlGenericControl("a");
            if (!string.IsNullOrEmpty(p.Title))
            {
                navLogo.Attributes["class"] += "brand-logo center";
                navLogo.InnerText = p.Title;
                navWrapper.Controls.Add(navLogo);
            }


            // add menu items
            if (p.MenuItms != null)
            {
                HtmlGenericControl menuItemList = new HtmlGenericControl("ul");
                menuItemList.Attributes["class"] += " right hide-on-med-and-down";

                // build main nav menu
                if (!string.IsNullOrEmpty(SessionHandler.Usr.User))
                {
                    string username = SessionHandler.Usr.User.ToString();
                    menuItemList.Controls.Add(
                        new LiteralControl(
                            "<li><a class=\"dropdown-button\" href=\"#!\" data-activates=\"nav_dropdown\">" + username +
                            "<i class=\"material-icons right\">arrow_drop_down</i>"));
                }


                foreach (MenuItm m in p.MenuItms)
                {
                    // security
                    bool allowed = false;


                    if (m.Security != null && m.Security != "")
                    {
                        foreach (string role in SessionHandler.Usr.Roles)
                        {
                            if (m.Security.ToLower() == role.ToLower())
                            {
                                allowed = true;
                                break;
                            }
                        }
                    }
                    else
                    {
                        allowed = true;
                    }


                    allowed = true;
                    if (m.Security == "application" || allowed)
                    {
                        HtmlGenericControl li = new HtmlGenericControl("li");
                        HtmlAnchor a = new HtmlAnchor();
                        a.HRef = "~/index.aspx?page=" + m.Module + "&filter=nofilter";
                        a.InnerText = m.MenuText;

                        li.Controls.Add(a);
                        menuItemList.Controls.Add(li);
                        // create menu item
                    }
                }

                navWrapper.Controls.Add(menuItemList);
            }


            return navWrapper;
        }

        #region builders

        public static String buildToastOnClick(String message, int duration)
        {
            return "Materialize.toast('" + message +
                   " ', " + duration + ")";
        }

        public static HtmlGenericControl buildInputField(HtmlObject htmlObject, Section section, bool enabled,
            int length, HtmlGenericControl icon, String type)
        {
            HtmlGenericControl placehholder = new HtmlGenericControl("div");
            placehholder.Attributes["class"] += "input-field";
            placehholder.Attributes["class"] += " col s" + htmlObject.Width.ToString();
            if (!string.IsNullOrEmpty(htmlObject.CssClass))
            {
                placehholder.Attributes["class"] += "  " + htmlObject.CssClass;
            }


            if (icon != null)
            {
                placehholder.Controls.Add(icon);
            }


            HtmlInputControl inputbox = new HtmlInputText("input");
            inputbox.ID = "ctl" + section.ID + "_" + htmlObject.ID;
            // inputbox.Attributes["class"] += "validate";


            if (!string.IsNullOrEmpty(type))
            {
                if (type == "date")
                {
                    inputbox.Attributes["type"] = "text";
                }
                else
                {
                    inputbox.Attributes["type"] = type;
                }


                if (type == "number")
                {
                    type = "digits";
                }
            }
            else
            {
                inputbox.Attributes["type"] = "text";
            }


            addValidation(inputbox, htmlObject, type);

            if (!string.IsNullOrEmpty(htmlObject.Mandatory))
            {
                if (htmlObject.Mandatory.ToLower() == "yes")
                {
                    inputbox.Attributes["required"] = "";
                }
            }

            if (htmlObject.DefValue != null && htmlObject.DefValue != "")
            {
                inputbox.Attributes["value"] = htmlObject.DefValue;
            }
            inputbox.Disabled = !enabled;


            // label

            // mandatory
//             starr = (htmlobject.Mandatory == "yes" && pageform.ActionType != "search" && enab) ? "<span class=\"message\">&nbsp;*</span>" : "";
//                                           label.InnerHtml = htmlobject.Label + starr;


            HtmlGenericControl label = new HtmlGenericControl("label");
            label.Attributes["for"] = inputbox.ID;
            label.InnerHtml = htmlObject.Label;


            placehholder.Controls.Add(inputbox);
            placehholder.Controls.Add(label);


            return placehholder;
        }


        public static HtmlGenericControl buildDateField(HtmlObject htmlObject, Section section, bool enabled,
            HtmlGenericControl icon)
        {
            HtmlGenericControl placehholder = new HtmlGenericControl("div");
            placehholder.Attributes["class"] += "input-field";
            placehholder.Attributes["class"] += " col s" + htmlObject.Width.ToString();


            if (icon != null)
            {
                placehholder.Controls.Add(icon);
            }
            if (!string.IsNullOrEmpty(htmlObject.CssClass))
            {
                placehholder.Attributes["class"] += "  " + htmlObject.CssClass;
            }


            HtmlInputControl inputbox = new HtmlInputText("input");
            inputbox.Attributes["type"] = "date";
            inputbox.Attributes["class"] += "datepicker";
            inputbox.ID = "ctl" + section.ID + "_" + htmlObject.ID;

            inputbox.Attributes["dateField"] = "";

            if (!string.IsNullOrEmpty(htmlObject.DefValue))
            {
                inputbox.Attributes["value"] = htmlObject.DefValue;
            }
            inputbox.Disabled = !enabled;

            HtmlGenericControl label = new HtmlGenericControl("label");
            label.Attributes["for"] = inputbox.ID;
            label.InnerHtml = htmlObject.Label;


            placehholder.Controls.Add(inputbox);
            placehholder.Controls.Add(label);


            placehholder.Controls.Add(new LiteralControl("<script>$('.datepicker').pickadate({" +
                                                         "selectMonths: true," +
                                                         "selectYears: 20})</script>"));


            return placehholder;
        }

        public static HtmlGenericControl buildTextArea(HtmlObject htmlObject, Section section, bool enabled,
            HtmlGenericControl icon)
        {
            HtmlGenericControl placehholder = new HtmlGenericControl("div");
            placehholder.Attributes["class"] += "input-field";
            placehholder.Attributes["class"] += " col s" + htmlObject.Width.ToString();


            if (icon != null)
            {
                placehholder.Controls.Add(icon);
            }
            if (!string.IsNullOrEmpty(htmlObject.CssClass))
            {
                placehholder.Attributes["class"] += "  " + htmlObject.CssClass;
            }


            HtmlTextArea inputbox = new HtmlTextArea();

            inputbox.Attributes["class"] += "materialize-textarea";
            inputbox.ID = "ctl" + section.ID + "_" + htmlObject.ID;


            addValidation(inputbox, htmlObject, "textarea");
//            if (!string.IsNullOrEmpty(htmlObject.Length))
//            {
//
//                inputbox.Attributes["length"] = htmlObject.Length;
//
//            }

            if (!string.IsNullOrEmpty(htmlObject.DefValue))
            {
                inputbox.Attributes["value"] = htmlObject.DefValue;
            }
            inputbox.Disabled = !enabled;

            HtmlGenericControl label = new HtmlGenericControl("label");
            label.Attributes["for"] = inputbox.ID;
            label.InnerHtml = htmlObject.Label;


            placehholder.Controls.Add(inputbox);
            placehholder.Controls.Add(label);


            return placehholder;
        }


        public static HtmlGenericControl buildFileInput(HtmlObject htmlObject, Section section, bool enabled,
            HtmlGenericControl icon)
        {
            HtmlGenericControl placehholder = new HtmlGenericControl("div");
            placehholder.Attributes["class"] += "input-field file-field";
            placehholder.Attributes["class"] += " col s" + htmlObject.Width.ToString();


            if (!string.IsNullOrEmpty(htmlObject.CssClass))
            {
                placehholder.Attributes["class"] += "  " + htmlObject.CssClass;
            }


            HtmlGenericControl buttonDiv = new HtmlGenericControl("div");
            buttonDiv.Attributes["class"] += " btn";
            buttonDiv.ID = "ctl" + section.ID + "_" + htmlObject.ID;

            HtmlGenericControl buttonText = new HtmlGenericControl("span");
            if (!string.IsNullOrEmpty(htmlObject.Label))
            {
                buttonText.InnerText = htmlObject.Label;
            }

            buttonDiv.Controls.Add(buttonText);

            if (icon != null)
            {
                icon.Attributes["class"] = "material-icons left";
                buttonDiv.Controls.Add(icon);
            }

            HtmlGenericControl fileInput = new HtmlGenericControl("input");
            fileInput.Attributes["type"] = "file";
            buttonDiv.Controls.Add(fileInput);

            placehholder.Controls.Add(buttonDiv);


            HtmlGenericControl fileWrapDiv = new HtmlGenericControl("div");
            fileWrapDiv.Attributes["class"] += " file-path-wrapper";


            placehholder.Controls.Add(fileWrapDiv);

            HtmlGenericControl fileTextInput = new HtmlGenericControl("input");
            fileTextInput.Attributes["class"] += "validate";
            fileTextInput.Attributes["type"] = "text";

            if (!string.IsNullOrEmpty(htmlObject.Length))
            {
                fileTextInput.Attributes["length"] = htmlObject.Length;
            }

            if (!string.IsNullOrEmpty(htmlObject.DefValue))
            {
                fileTextInput.Attributes["value"] = htmlObject.DefValue;
            }

            if (!enabled)
            {
                buttonDiv.Attributes["class"] += " disabled";
                fileTextInput.Disabled = true;
            }

            fileWrapDiv.Controls.Add(fileTextInput);


            placehholder.Controls.Add(fileWrapDiv);


            return placehholder;
        }

        //public static HtmlGenericControl buildTable(List<String> fieldsList, HtmlGenericControl context) 
        //{
        //    HtmlGenericControl table = new HtmlGenericControl("table");
        //    table.Attributes["class"] = "striped highlight responsive-table";
        //    context.Controls.Add(table);
        //    HtmlGenericControl thead = new HtmlGenericControl("thead");
        //    table.Controls.Add(thead);
        //    HtmlGenericControl tbody = new HtmlGenericControl("tbody");
        //    table.Controls.Add(tbody);


        //    HtmlGenericControl tr = new HtmlGenericControl("tr");
        //    thead.Controls.Add(tr);
        //    foreach (String fieldName in fieldsList)
        //    {
        //        HtmlGenericControl th = new HtmlGenericControl("th");
        //        th.Attributes["data-field"] = fieldName;
        //        th.InnerHtml = fieldName;
        //        tr.Controls.Add(tr);
        //    }

        //    return tbody;
        //}


        /**Builds an icon
         * tiny, small medium or large
         * standard = 
         * 
         */

        public static HtmlGenericControl buildIcon(String iconName, String size)
        {
            HtmlGenericControl inputIcon = new HtmlGenericControl("i");
            if (size != null)
            {
                inputIcon.Attributes["class"] += size;
            }
            inputIcon.Attributes["class"] += " material-icons prefix";
            inputIcon.InnerHtml = iconName;
            return inputIcon;
        }


        public static HtmlGenericControl buildCheckBox(HtmlObject htmlObject, Section section, bool enabled,
            HtmlGenericControl icon)
        {
            HtmlGenericControl placehholder = new HtmlGenericControl("div");
            placehholder.Attributes["class"] += "input-field checkbox";
            placehholder.Attributes["class"] += " col s" + htmlObject.Width.ToString();


            if (icon != null)
            {
                placehholder.Controls.Add(icon);
            }

            if (!string.IsNullOrEmpty(htmlObject.CssClass))
            {
                placehholder.Attributes["class"] += "  " + htmlObject.CssClass;
            }


            HtmlInputCheckBox inputbox = new HtmlInputCheckBox();
            inputbox.Attributes["type"] = "checkbox";
            inputbox.ID = "ctl" + section.ID + "_" + htmlObject.ID;

            inputbox.Checked = false;
            if (!string.IsNullOrEmpty(htmlObject.DefValue))
            {
                if (htmlObject.DefValue == "0")
                {
                    inputbox.Checked = false;
                }
                else if (htmlObject.DefValue == "1")
                {
                    inputbox.Checked = true;
                }
            }
            inputbox.Disabled = !enabled;

            addValidation(inputbox, htmlObject, "checkbox");

            HtmlGenericControl label = new HtmlGenericControl("label");
            label.Attributes["for"] = inputbox.ID;
            label.InnerHtml = htmlObject.Label;


            placehholder.Controls.Add(inputbox);
            placehholder.Controls.Add(label);


            return placehholder;
        }


        public static HtmlGenericControl buildDropdown(HtmlObject htmlObject, bool enabled, HtmlGenericControl icon,
            Section section)
        {
            HtmlGenericControl placehholder = new HtmlGenericControl("div");
            placehholder.Attributes["class"] += "input-field";
            placehholder.Attributes["class"] += " col s" + htmlObject.Width.ToString();
            if (!string.IsNullOrEmpty(htmlObject.CssClass))
            {
                placehholder.Attributes["class"] += "  " + htmlObject.CssClass;
            }


            if (icon != null)
            {
                placehholder.Controls.Add(icon);
            }


            HtmlSelect coldropdown = new HtmlSelect();
            coldropdown.Disabled = !enabled;
            coldropdown.ID = "ctl" + section.ID + "_" + htmlObject.ID;


            // add mandatory
     
            if (!string.IsNullOrEmpty(htmlObject.Mandatory) && htmlObject.Mandatory.ToLower() == "yes")
            {
            }
            else
            {
                ListItem firstItem = null;
                if (htmlObject.List.Substring(0, 2).ToLower() == "se" ||
                    htmlObject.List.Substring(0, 2).ToLower() == "ex")
                {
                    firstItem = new ListItem("", "*");
                }
                else
                {
                    firstItem = new ListItem("", "");
                }
                
                firstItem.Selected = true;
                coldropdown.Items.Add(firstItem);
            }


            // list
            if (!string.IsNullOrEmpty(htmlObject.List))
            {
                List<String> items = new List<string>();


                String listString = htmlObject.List;


                if (listString.StartsWith("[") && listString.EndsWith("]"))
                {
                    listString = listString.Substring(1, listString.Length - 2);

                    String[] listItems = listString.Split(',');
                    foreach (string listItem in listItems)
                    {
                        string item = listItem.Trim();
                        if (item.Split(':').Length == 1)
                        {
                            items.Add(item + ":" + item);

                        }
                        else
                        {
                            items.Add(item);
                        }

                    }
                }
                else if (htmlObject.List.Substring(0, 2).ToLower() == "se" ||
                         htmlObject.List.Substring(0, 2).ToLower() == "ex")
                {
                    String sql = htmlObject.List;
                    //            // select or execute
                    //            if (htmlobject.List.Substring(0, 2).ToLower() == "se" || htmlobject.List.Substring(0, 2).ToLower() == "ex")
                    //            {
                    //                // replace with environment variables
                    sql = sql.Replace("@user", "'" + SessionHandler.Usr.User + "'");
                    sql = sql.Replace("@module", "'" + SessionHandler.Qstring.Pageform + "'");
                    sql = sql.Replace("@filter", "'" + SessionHandler.Qstring.Filter.Split('|')[0] + "'");
                    sql = sql.Replace("@role", "'" + SessionHandler.accesslevel + "'");
                    sql = sql.Replace("@department", "'" + SessionHandler.Usr.Department + "'");
                    //

                    ConnectionString cs = new ConnectionString(SessionHandler.connection);
                    DataTable dt = cs.Select(sql);
                    foreach (DataRow r in dt.Rows)
                    {


                        string value = r[0].ToString();
                        string text = dt.Columns.Count > 1 ? r[1].ToString() : r[0].ToString();
                        items.Add(value + ":" + text);
                    }
                }


                foreach (String s in items)
                {

                    string value = s.Split(':')[0];
                    string text = s.Split(':')[1];
                    ListItem listItem = new ListItem(text, value);

                    if (!string.IsNullOrEmpty(htmlObject.DefValue))
                    {
                        if (value.Equals(htmlObject.DefValue))
                        {
                            listItem.Attributes["class"] = "active selected";
                        }
                    }

                    coldropdown.Items.Add(listItem);
                }
                placehholder.Controls.Add(coldropdown);


                // add label
                if (!string.IsNullOrEmpty(htmlObject.Label))
                {
                    HtmlGenericControl label = new HtmlGenericControl("label");
                    label.InnerText = htmlObject.Label;
                    placehholder.Controls.Add(label);
                }



               


            }
            return placehholder;
        }

        public static HtmlGenericControl buildDatePicker(HtmlObject htmlObject)
        {
            return null;
        }

        public static HtmlGenericControl buildTable(HtmlObject htmlObject)
        {
            return null;
        }


        public static HtmlGenericControl buildButton(HtmlObject htmlObject, Section section, HtmlGenericControl icon,
            bool enabled, String onClick)
        {
            //create button


            HtmlAnchor button = new HtmlAnchor();
            button.Attributes["class"] += " waves-effect waves-light btn";


            if (icon != null)
            {
                icon.Attributes["class"] += "left";
                button.Controls.Add(icon);
            }

            button.InnerHtml = htmlObject.Label;
            button.Name = htmlObject.Action;
            button.Attributes["class"] += " " + htmlObject.CssClass;
            button.ID = "ctl" + section.ID + "_" + htmlObject.ID;
            button.Disabled = !enabled;


            //message

            //String message = htmlobject.Action + " Succeeded.";
            //if (htmlobject.Message != null && htmlobject.Message != "")
            //{
            //    message = htmlobject.Message;

            //}


            if (onClick != null && onClick != "")
            {
                button.Attributes[
                    "onclick"] = onClick;
            }

            return null;
        }

        #endregion

        public static HtmlGenericControl BuildForm(XmlDocument xmlDocument, bool postback)
        {
            #region variables

            HtmlGenericControl p = new HtmlGenericControl("div");
            p.ID = "pageform";
            PageForm pageform = new PageForm();
            pageform.Load(xmlDocument);
            pageform.ID = SessionHandler.Qstring.Pageform;


            if (pageform.Database.Connection == null || pageform.Database.Connection == "")
            {
                // default connection
                SessionHandler.connection = "DBConnection";
            }
            else
            {
                // connection from the propfile
                SessionHandler.connection = pageform.Database.Connection;
            }


            ConnectionString cs = new ConnectionString(SessionHandler.connection);
            string sql = string.Empty;
            DataTable dt;
            List<string> searchcolumns = new List<string>();

            #endregion

            string str = string.Empty;
            try // error trap
            {
                foreach (Section section in pageform.Sections.Items)
                {
                    #region variables

                    HtmlGenericControl sectionpanelWrapper = new HtmlGenericControl("div");
                    HtmlGenericControl sectionpanel = new HtmlGenericControl("div");
                    sectionpanelWrapper.Attributes["class"] += " section ";

                    if (!string.IsNullOrEmpty(section.Class))
                    {
                        sectionpanel.Attributes["class"] += " " + section.Class;
                    }
                    if (!string.IsNullOrEmpty(section.Container))
                    {
                        if (section.Container.ToLower() != "no")
                        {
                            sectionpanelWrapper.Attributes["class"] += " container ";
                        }
                    }

                    sectionpanel.ID = "section_" + section.ID;

                    //ROW ADD
                    sectionpanel.Attributes["class"] += " row";
                    sectionpanelWrapper.Controls.Add(sectionpanel);
                    p.Controls.Add(CreateHTMLObjects.CreateNewLine(1, 8));
                    p.Controls.Add(sectionpanelWrapper);
                    string starr = String.Empty;
                    bool enab = false;

                    #endregion

                    foreach (HtmlObject htmlobject in section.HtmlObjects.Items)
                    {
                        // get the searchcolumns
                        if (htmlobject.Target == "search" || htmlobject.Target == "parameter")
                        {
                            searchcolumns.Add(htmlobject.DbField);
                        }

                        // add html objects
                        sectionpanel.Controls.Add(CreateHTMLObjects.CreateNewLine(1, 8));


                        HtmlGenericControl icon = null;
                        if (htmlobject.Icon != "" && htmlobject.Icon != null)
                        {
                            icon = buildIcon(htmlobject.Icon, null);
                        }


                        switch (htmlobject.HtmlType)
                        {
                                #region space

                            case "space":
                                HtmlGenericControl space = new HtmlGenericControl("div");


                                if (htmlobject.Width == 0)
                                {
                                    htmlobject.Width = 12;
                                }

                                if (!string.IsNullOrEmpty(htmlobject.CssClass))
                                {
                                    space.Attributes["class"] += "  " + htmlobject.CssClass;
                                }
                                space.Attributes["class"] += " col s" + htmlobject.Width.ToString();
                                space.Controls.Add(new LiteralControl("&nbsp"));


                                sectionpanel.Controls.Add(space);
                                sectionpanel.Controls.Add(CreateHTMLObjects.CreateNewLine(1, 8));
                                break;

                                #endregion

                            case "output":

                                #region output

                                if (htmlobject.Display == "grid")
                                {
                                    HtmlGenericControl output = new HtmlGenericControl("div");
                                    HtmlGenericControl labelout = new HtmlGenericControl("span");
                                    labelout.InnerHtml = htmlobject.Label;
                                    labelout.Attributes["class"] += " " + "label";
                                    output.ID = "ctl" + section.ID + "_" + htmlobject.ID;
                                    output.Style["width"] = htmlobject.Width + "px";
                                    output.Style["height"] = htmlobject.Height + "px";
                                    sectionpanel.Controls.Add(labelout);
                                    sectionpanel.Controls.Add(CreateHTMLObjects.CreateNewLine(1, 8));
                                    sectionpanel.Controls.Add(output);
                                }
                                else
                                {
                                    HtmlTextArea output = new HtmlTextArea();
                                    HtmlGenericControl labelout = new HtmlGenericControl("span");
                                    labelout.InnerHtml = htmlobject.Label;
                                    labelout.Attributes["class"] += " " + "label";
                                    output.ID = "ctl" + section.ID + "_" + htmlobject.ID;
                                    output.Attributes["class"] += " col s" + htmlobject.Width.ToString();
                                    output.Style["height"] = htmlobject.Height.ToString() + "px";
                                    output.Attributes["tabindex"] = htmlobject.TabOrder.ToString();
                                    // security
                                    enab = htmlobject.Enabled.ToLower() == "no"
                                        ? false
                                        : SessionHandler.Usr.Create || SessionHandler.Usr.Update ||
                                          pageform.ActionType == "login" || pageform.ActionType == "search";
                                    output.Disabled = Helper.Reverse(enab);
                                    output.Attributes["class"] += " " +
                                                                  (enab == false
                                                                      ? htmlobject.CssClass + "disabled"
                                                                      : htmlobject.CssClass);
                                    sectionpanel.Controls.Add(labelout);
                                    sectionpanel.Controls.Add(CreateHTMLObjects.CreateNewLine(1, 8));
                                    sectionpanel.Controls.Add(output);
                                }
                                break;

                                #endregion

                            case "inputbox":

                                #region inputbox

                                enab = htmlobject.Enabled.ToLower() == "no"
                                    ? false
                                    : SessionHandler.Usr.Create || SessionHandler.Usr.Update ||
                                      pageform.ActionType == "login" || pageform.ActionType == "search";
//                               

                                // default value

                                // max chars


                                sectionpanel.Controls.Add(buildInputField(htmlobject, section, enab, 0, icon, "text"));

//                                HtmlGenericControl placehholder = new HtmlGenericControl("div");
//                                placehholder.Attributes["class"] += "input-field";
//                                placehholder.Attributes["class"] += " col s" + htmlobject.Width.ToString();


//                                HtmlInputText inputbox = new HtmlInputText();
//                                inputbox.ID = "ctl" + section.ID + "_" + htmlobject.ID;


//                                HtmlGenericControl label = new HtmlGenericControl("label");
//                                label.Attributes["for"] = inputbox.ID;


//                                placehholder.Controls.Add(inputbox);
//                                placehholder.Controls.Add(label);


//                                //WIDTH
////                                inputbox.Style["width"] = htmlobject.Width.ToString() + "px";

//                                inputbox.Attributes["tabindex"] = htmlobject.TabOrder.ToString();
//                                //inputbox.Attributes.Add("OnKeyUp", "checkElement('" + htmlobject.Datatype + "')");

//                                // security
//                                enab = htmlobject.Enabled.ToLower() == "no" ? false : SessionHandler.Usr.Create || SessionHandler.Usr.Update || pageform.ActionType == "login" || pageform.ActionType == "search";
//                                // inputbox.Disabled = Helper.Reverse(enab);
//                                // readonly not disabled
//                                if (enab == false) { inputbox.Attributes["readonly"] = "readonly"; }

//                                // mandatory
//                                starr = (htmlobject.Mandatory == "yes" && pageform.ActionType != "search" && enab) ? "<span class=\"message\">&nbsp;*</span>" : "";
//                                label.InnerHtml = htmlobject.Label + starr;
//                                inputbox.Attributes["class"] += " " +
//                                                                (enab == false
//                                                                    ? htmlobject.CssClass + "disabled"
//                                                                    : (htmlobject.Mandatory == "yes" &&
//                                                                       pageform.ActionType != "search")
//                                                                        ? htmlobject.CssClass + "mandatory"
//                                                                        : htmlobject.CssClass);

//                                //ADD WIDTH COL
//                                //inputbox.Attributes["class"] += " col s"+ htmlobject.Width.ToString();


//                                sectionpanel.Controls.Add(placehholder);
//                                sectionpanel.Controls.Add(CreateHTMLObjects.CreateNewLine(1, 8));
                                //sectionpanel.Controls.Add(inputbox);
                                break;

                                #endregion

                            case "password":

                                #region password

//
//
//                                HtmlGenericControl passPh = new HtmlGenericControl("div");
//                                passPh.Attributes["class"] += "input-field";
//                                passPh.Attributes["class"] += " col s" + htmlobject.Width.ToString();
//
//
//                                if (htmlobject.Icon != "" && htmlobject.Icon != null)
//                                {
//                                    HtmlGenericControl inputIcon = new HtmlGenericControl("i");
//                                    inputIcon.Attributes["class"] += " material-icons prefix";
//                                    inputIcon.InnerHtml = htmlobject.Icon;
//                                    placehholder.Controls.Add(inputIcon);
//                                }
//
//
//                                HtmlInputText inputbox = new HtmlInputText();
//                                inputbox.ID = "ctl" + section.ID + "_" + htmlobject.ID;
//
//
//                                HtmlGenericControl label = new HtmlGenericControl("label");
//                                label.Attributes["for"] = inputbox.ID;
//
//
//
//
//                                passPh.Controls.Add(inputbox);
//                                passPh.Controls.Add(label);
//
//
//


                                enab = htmlobject.Enabled.ToLower() == "no" ? false : true;
//                                // inputbox.Disabled = Helper.Reverse(enab);
//                                // readonly not disabled
//                                if (enab == false) { inputbox.Attributes["readonly"] = "readonly"; }


                                HtmlGenericControl password = buildInputField(htmlobject, section, enab, 0, icon,
                                    "password");

//                                HtmlGenericControl label0 = new HtmlGenericControl("span");
//                                label0.InnerHtml = htmlobject.Label;
//                                label0.Attributes["class"] = "label";
//                                password.ID = "ctl" + section.ID + "_" + htmlobject.ID;
//
//
//
//                                //password.Style["width"] = htmlobject.Width.ToString() + "px";
//
//                                //WIDTH
//                                password.Attributes["class"] += " col s"+ htmlobject.Width.ToString();
//                                password.Attributes["tabindex"] = htmlobject.TabOrder.ToString();
//                                password.Disabled = htmlobject.Enabled.ToLower() == "yes" ? false : true;
//                                sectionpanel.Controls.Add(label0);
//                                sectionpanel.Controls.Add(CreateHTMLObjects.CreateNewLine(1, 8));
                                sectionpanel.Controls.Add(password);
                                break;

                                #endregion

                            case "table":

                    
                                #region table

                                //HtmlTable table = new HtmlTable();

                                HtmlGenericControl table_wrapper = new HtmlGenericControl("div");
                                table_wrapper.Attributes["class"] += " table_wrapper";
                                HtmlGenericControl table = new HtmlGenericControl("table");
                                table_wrapper.Controls.Add(table);
                                table.Attributes["class"] += " table_class ";

                               


                                HtmlGenericControl label2 = new HtmlGenericControl("span");
                                StringBuilder sb2 = new StringBuilder();
                                DataTable dtaut = new DataTable();
                                DataTable dtc = new DataTable();

                                XmlDocument vd = (XmlDocument) HttpContext.Current.Application["version"];
                                VersionInfo v = new VersionInfo(vd);

                                string unfilteredurl =
                                    HttpContext.Current.Request.Url.ToString()
                                        .Replace("filter=" + SessionHandler.Qstring.Filter, "filter=nofilter");
                                label2.Attributes["class"] = SessionHandler.Qstring.Filter != "nofilter"
                                    ? htmlobject.Label + " - filtered value: " +
                                      SessionHandler.Qstring.Filter.Replace("nofilter", "")
                                          .Replace("|", "")
                                          .Replace("AND", " AND ")
                                          .Replace("OR", " OR ") + " <a class = \"tinylink\" href = \"" + unfilteredurl +
                                      "\">[remove filter]</a>"
                                    : htmlobject.Label;


                                //    label2.Style["width"] = htmlobject.Width.ToString() + "px";
                                // width in columns
                                if (htmlobject.Width < 12)
                                {
                                    table.Attributes["class"] += " col s" + htmlobject.Width.ToString();
                                }
                                else
                                {
                                    table.Style.Add("width", htmlobject.Width.ToString() + "px");
                                }

                                table.ID = "ctl" + section.ID + "_" + htmlobject.ID;
                                //table.Width = htmlobject.Width.ToString();
                                table.Attributes["class"] += htmlobject.CssClass +
                                                             " left striped highlight responsive-table";

                                // set the initial direction
                                if (HttpContext.Current.Request.QueryString["direction"] == null)
                                {
                                    if (htmlobject.Direction != "")
                                    {
                                        SessionHandler.Qstring.Direction = htmlobject.Direction;
                                    }
                                }


                                //======================= HEADERS ======================//


                                // header plus nav


                                HtmlGenericControl thead = new HtmlGenericControl("thead");


                                table.Controls.Add(thead);


                                HtmlTableRow headerrow = new HtmlTableRow();
                                thead.Controls.Add(headerrow);


                                if (htmlobject.Check != "")
                                {
                                    HtmlTableCell authheader = new HtmlTableCell("th");
                                    //authheader.Style["width"] = "40px";
                                    HtmlInputCheckBox chkall = new HtmlInputCheckBox();
                                    chkall.ID = "ctl" + section.ID + "_" + htmlobject.ID + "_all";
                                    // security
                                    chkall.Disabled =
                                        Helper.Reverse(SessionHandler.Usr.Update && SessionHandler.Usr.Authorize);
                                    chkall.Attributes["class"] += " " + "input";
                                    authheader.Controls.Add(chkall);
                                    headerrow.Controls.Add(authheader);
                                }

                                if (htmlobject.DbTable != "")
                                {
                                    HtmlTableCell authheader = new HtmlTableCell("th");
                                    headerrow.Controls.Add(authheader);
                                }


                                // toggle direction
                                string direction = SessionHandler.Qstring.Direction == "asc" ? "desc" : "asc";

                                // build sql for aut
                                if (htmlobject.ColumnCheck == "yes")
                                {
                                    sb2.Append("select top 1 ");
                                    foreach (Header h in htmlobject.Headers.Items)
                                    {
                                        sb2.Append(h.Name + ", ");
                                    }
                                    sb2.Append("@from " + pageform.Database.Table + "_aut ");
                                    sb2.Append("where usr = '" + SessionHandler.Usr.User + "' ");
                                    sb2.Append("order by id desc");
                                    sql = sb2.ToString().Replace(", @", " ");

                                    // load the row into a datatable
                                    if (cs.Parse(sql))
                                    {
                                        dtaut = cs.Select(sql);
                                    }
                                }

                                foreach (Header h in htmlobject.Headers.Items)
                                {
                                    HtmlTableCell headercell = new HtmlTableCell("th");
                                    //headercell.Attributes["class"] += " " + "colhead";
                                    headercell.ID = h.ID;
                                    //HtmlAnchor test = new HtmlAnchor();
                                    //test.HRef = "~/index.aspx?page=" + SessionHandler.Qstring.Pageform + "&filter=" + SessionHandler.Qstring.Filter + "&order=" + (htmlobject.Headers.Items.IndexOf(h) + 1) + "&direction=" + direction;
                                    //test.Style.Add("text-decoration", "underline");
                                    headercell.InnerHtml = h.Name + " ";

                                    HtmlAnchor link = new HtmlAnchor();
                                    link.HRef = "~/index.aspx?page=" + SessionHandler.Qstring.Pageform + "&filter=" +
                                                SessionHandler.Qstring.Filter + "&order=" +
                                                (htmlobject.Headers.Items.IndexOf(h) + 1) + "&direction=" + direction;
                                    link.Style.Add("color", "grey");

                                    HtmlGenericControl sort = new HtmlGenericControl("i");
                                    sort.Attributes["class"] = "fa fa-sort fa-lg";
                                    link.Controls.Add(sort);

                                    // ID column must be named ID
                                    if (htmlobject.ColumnCheck == "yes" && h.Name.ToUpper() != "ID")
                                    {
                                        HtmlInputCheckBox chkauth = new HtmlInputCheckBox();
                                        chkauth.ID = "ctl_a" + section.ID + "_" + htmlobject.ID + "_" + h.ID;
                                        if (dtaut.Rows.Count != 0)
                                        {
                                            DataRow r = dtaut.Rows[0];
                                            // ID must be numeric and sequential
                                            chkauth.Checked = r[Convert.ToInt16(h.ID) - 1].ToString() == "1"
                                                ? true
                                                : false;
                                        }

                                        HtmlInputHidden hidcol = new HtmlInputHidden();
                                        hidcol.ID = "ctl_hc" + section.ID + "_" + htmlobject.ID + "_" + h.ID;
                                        hidcol.Value = h.Name;

                                        headercell.Controls.Add(chkauth);
                                        headercell.Controls.Add(hidcol);
                                        headercell.Controls.Add(CreateHTMLObjects.CreateBreak());
                                    }

                                    headercell.Controls.Add(link);
                                    headerrow.Controls.Add(headercell);
                                }
                                if (htmlobject.Edit != null && htmlobject.Edit != "")
                                {
                                    HtmlTableCell editheader = new HtmlTableCell("th");
                                    //editheader.Style["width"] = "40px";
                                    headerrow.Controls.Add(editheader);
                                }

                                if (htmlobject.Delete != null && htmlobject.Delete != "")
                                {
                                    HtmlTableCell deleteheader = new HtmlTableCell("th");
                                    //deleteheader.Style["width"] = "40px";
                                    headerrow.Controls.Add(deleteheader);
                                }
                                table.Controls.Add(thead);

                                // build sql
                                sql = string.Empty;
                                foreach (Column c in htmlobject.Columns.Items)
                                {
                                    if (c.DbField.StartsWith("##"))
                                    {
                                        // decrypt
                                        sql += "dbo.sf_decrypt(" + c.DbField.Replace("##", "") + ",'" + v.Encryption +
                                               "'), ";
                                    }
                                    else
                                    {
                                        // no decript
                                        sql += c.DbField.Replace("##", "") + ", ";
                                    }
                                }
                                StringBuilder sb = new StringBuilder();


                                sb.Append("select top ");
                                // max rows from version info
                                sb.Append(v.MaxRows.ToString() + " ");
                                sb.Append(sql.Substring(0, sql.Length - 2));
                                sb.Append(" from ");

                                // encryption
                                if (pageform.Database.Table.IndexOf("##") > 0)
                                {
                                    string[] t = pageform.Database.Table.Split(',');
                                    string decrsql = "";
                                    foreach (string vld in t)
                                    {
                                        if (vld.Trim().StartsWith("##"))
                                        {
                                            // decrypt
                                            decrsql += "dbo.sf_decrypt(" + vld.Replace("##", "") + ",'" + v.Encryption +
                                                       "') as " + vld.Replace("##", "") + ", ";
                                        }
                                        else
                                        {
                                            // no decript
                                            decrsql += vld.Replace("##", "") + ", ";
                                        }
                                    }
                                    // remove comma
                                    sb.Append(decrsql.Substring(0, decrsql.Length - 2));
                                }
                                else
                                {
                                    sb.Append(pageform.Database.Table);
                                }

                                sb.Append(" where ");
                                sb.Append(pageform.Database.Condition);
                                sb.Append(" and ");

                                string filter = SessionHandler.Qstring.Filter == "nofilter"
                                    ? (htmlobject.Loaddata == "no" ? "0" : "%")
                                    : SessionHandler.Qstring.Filter;

                                if (SessionHandler.Usr.Read)
                                {
                                    // parse filter
                                    string[] parsedfilter = filter.Split('|');

                                    // save parsed filter to session for future use

                                    for (int i = 0; i < parsedfilter.Length; i++)
                                    {
                                        if ((searchcolumns.Count == 0 || searchcolumns[i] == "") &&
                                            parsedfilter.Length <= 1)
                                        {
                                            sb.Append(pageform.Database.PrimaryKey + " like '" +
                                                      parsedfilter[0].Replace("*", "%") + "%'");
                                        }
                                        else
                                        {
                                            if (searchcolumns[i] == "")
                                            {
                                                sb.Append(" " +
                                                          parsedfilter[i].Replace("nofilter", "").Replace("*", "%") +
                                                          " ");
                                            }
                                            else
                                            {
                                                if (searchcolumns[i].StartsWith("##"))
                                                {
                                                    // with encryption
                                                    sb.Append(searchcolumns[i].Replace("##", "") + " like " +
                                                              "dbo.sf_encrypt('" +
                                                              parsedfilter[i].Replace("nofilter", "").Replace("%", "") +
                                                              "')" + "+'%'");
                                                }
                                                else
                                                {
                                                    // no encryption
                                                    sb.Append(searchcolumns[i] + " like '" +
                                                              parsedfilter[i].Replace("nofilter", "").Replace("*", "%") +
                                                              "%'");
                                                }
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    sb.Append(pageform.Database.PrimaryKey + " is null ");
                                }

                                sb.Append(" order by ");
                                sb.Append(SessionHandler.Qstring.Order == "1"
                                    ? pageform.Database.Order
                                    : SessionHandler.Qstring.Order);
                                sb.Append(" " + SessionHandler.Qstring.Direction);
                                sql = sb.ToString();

                                // dummy query to prevent large selects after postback
                                if (postback && htmlobject.Check == "" && htmlobject.DbTable == "")
                                {
                                    sql = "select 'nope' as nope";
                                }

                                // replace with environment variables
                                sql = sql.Replace("@user", "'" + SessionHandler.Usr.User + "'");
                                sql = sql.Replace("@module", "'" + SessionHandler.Qstring.Pageform + "'");
                                sql = sql.Replace("@filter", "'" + SessionHandler.Qstring.Filter.Split('|')[0] + "'");
                                sql = sql.Replace("@role", "'" + SessionHandler.accesslevel + "'");
                                sql = sql.Replace("@department", "'" + SessionHandler.Usr.Department + "'");

                                dt = cs.Select(sql);


                                SessionHandler.datatable = dt;

                                #region paging

                                HtmlGenericControl paginationWrapper = new HtmlGenericControl("div");


                                HtmlGenericControl paginationUl = new HtmlGenericControl("ul");


                                paginationWrapper.Controls.Add(paginationUl);

                                paginationUl.Attributes["class"] += " pagination";

                                int factor = htmlobject.Rows == 0 ? 999 : htmlobject.Rows;

                                // make the urls
                                int from =
                                    Convert.ToInt16(HttpContext.Current.Request.QueryString["start"] == null
                                        ? "1"
                                        : HttpContext.Current.Request.QueryString["start"]);
                                string url = HttpContext.Current.Request.RawUrl;
                                url = url.Substring(0,
                                    url.IndexOf("start") == -1 ? url.Length : url.IndexOf("start") - 1);


                                // make left arrow

                                StringBuilder leftArrowUrl = new StringBuilder();
                                leftArrowUrl.Append(url);
                                leftArrowUrl.Append("&start=");
                                leftArrowUrl.Append(from == 1 ? 1 : from - 1);


                                paginationUl.Controls.Add(
                                    new LiteralControl(" <li class=\"waves-effect\"><a href=\"" +
                                                       leftArrowUrl.ToString() +
                                                       " \"><i class=\"material-icons\">chevron_left</i></a></li>"));

                                int maxpages = 0;
                                foreach (DataRow r in dt.Rows)
                                {
                                    if (dt.Rows.IndexOf(r)%factor == 0)
                                    {
                                        HtmlGenericControl paginationItem = new HtmlGenericControl("li");

                                        if (dt.Rows.IndexOf(r)/factor + 1 == from)
                                        {
                                            // current page, so set to active
                                            paginationItem.Attributes["class"] += "active";
                                        }
                                        else
                                        {
                                            paginationItem.Attributes["class"] += "waves-effect";
                                        }


                                        StringBuilder pagUrl = new StringBuilder();
                                        pagUrl.Append(url);
                                        pagUrl.Append("&start=");
                                        pagUrl.Append((dt.Rows.IndexOf(r)/factor + 1));

                                        HtmlAnchor paginationLink = new HtmlAnchor();
                                        paginationLink.HRef = pagUrl.ToString();

                                        paginationLink.InnerText = "" + (dt.Rows.IndexOf(r)/factor + 1);
                                        paginationUl.Controls.Add(paginationLink);

                                        paginationItem.Controls.Add(paginationLink);
                                        paginationUl.Controls.Add(paginationItem);

                                        maxpages ++;
                                    }
                                }


                                // make right arrow


                                StringBuilder rightArrowUrl = new StringBuilder();
                                rightArrowUrl.Append(url);
                                rightArrowUrl.Append("&start=");
                                rightArrowUrl.Append(from == maxpages ? maxpages : from + 1);


                                paginationUl.Controls.Add(
                                    new LiteralControl(" <li class=\"waves-effect\"><a href=\"" +
                                                       rightArrowUrl.ToString() +
                                                       " \"><i class=\"material-icons\">chevron_right</i></a></li>"));


                                sectionpanel.Controls.Add(paginationWrapper);

//                                StringBuilder sbpaging = new StringBuilder();
//                                sbpaging.Append("        <a class=\"paging\" href=\"");
//                                sbpaging.Append(url);
//                                sbpaging.Append("&start=");
//                                sbpaging.Append(from == 1 ? 1 : from - 1);
//                                sbpaging.Append("\">&nbsp;");
//                                sbpaging.Append("<<");
//                                sbpaging.Append("</a>\n        ");
//                                int maxpages = 0;
//                                foreach (DataRow r in dt.Rows)
//                                {
//
//                                    //paging
//                                    if (dt.Rows.IndexOf(r) % factor == 0)
//                                    {
//                                        sbpaging.Append("<a class=\"paging\" href=\"");
//                                        sbpaging.Append(url);
//                                        sbpaging.Append("&start=");
//                                        sbpaging.Append((dt.Rows.IndexOf(r) / factor + 1));
//                                        sbpaging.Append("\">&nbsp;");
//                                        sbpaging.Append(dt.Rows.IndexOf(r) / factor + 1 == from ? "<span style=\"font-weight:bold;\">" : "");
//                                        sbpaging.Append(dt.Rows.IndexOf(r) / factor + 1);
//                                        sbpaging.Append(dt.Rows.IndexOf(r) / factor + 1 == from ? "</span>" : "");
//                                        sbpaging.Append("</a>\n        ");
//                                        maxpages++;
//                                    }
//                                }
//
//                                sbpaging.Append("<a class=\"paging\" href=\"");
//                                sbpaging.Append(url);
//                                sbpaging.Append("&start=");
//                                sbpaging.Append(from == maxpages ? maxpages : from + 1);
//                                sbpaging.Append("\">&nbsp;");
//                                sbpaging.Append(">>");
//                                sbpaging.Append("</a>");

                                #endregion

                                #region rows
                                int counter = 1;
                                foreach (DataRow r in dt.Rows)
                                {
                                    if (dt.Rows.IndexOf(r) >= (from - 1)*factor & dt.Rows.IndexOf(r) < from*factor)
                                    {
                                        HtmlTableRow tablerow = new HtmlTableRow();
                                        // colls


                                        if (htmlobject.Check != "")
                                        {
                                            HtmlTableCell authcell = new HtmlTableCell();
                                            authcell.ID = "test" + counter;
                                            HtmlInputCheckBox chkb = new HtmlInputCheckBox();
                                            chkb.ID = "ctl_c" + section.ID + "_" + htmlobject.ID + "_" +
                                                      counter.ToString();
                                            chkb.Attributes["class"] += " " + "checkbox";
                                            // security
                                            chkb.Disabled =
                                                Helper.Reverse(SessionHandler.Usr.Update && SessionHandler.Usr.Authorize);
                                            chkb.Checked = r[htmlobject.Check].ToString() == "1" ? true : false;
                                            HtmlInputHidden hidden = new HtmlInputHidden();
                                            hidden.ID = "ctl_h" + section.ID + "_" + htmlobject.ID + "_" +
                                                        counter.ToString();
                                            hidden.Value = r[0].ToString();
                                            authcell.Controls.Add(chkb);
                                            authcell.Controls.Add(hidden);
                                            tablerow.Controls.Add(authcell);
                                        }

                                        if (htmlobject.DbTable != "")
                                        {
                                            HtmlTableCell authcell = new HtmlTableCell();
                                            authcell.ID = "id" + counter;
                                            HtmlInputHidden hidden = new HtmlInputHidden();
                                            hidden.ID = "ctl_hi" + section.ID + "_" + htmlobject.ID + "_" +
                                                        counter.ToString();
                                            hidden.Value = r[0].ToString();
                                            authcell.Controls.Add(hidden);
                                            tablerow.Controls.Add(authcell);
                                        }


                                        for (int i = 0; i <= dt.Columns.Count - 1; i++)
                                        {
                                            HtmlTableCell tablecell = new HtmlTableCell();

                                            if (!string.IsNullOrEmpty(htmlobject.Columns.Items[i].Width))
                                            {
                                                tablecell.Width = htmlobject.Columns.Items[i].Width;
                                            }


                                            if (htmlobject.RowHeight != 0)
                                            {
                                                tablecell.Height = "" + htmlobject.RowHeight;

                                            }


                                            //HtmlGenericControl span = new HtmlGenericControl("span");
                                            // updatable
                                            if (htmlobject.Columns.Items[i].Type != "")
                                            {
                                                #region security

                                                // security
                                                enab = false;
                                                // check authorized
                                                if (htmlobject.Check != "")
                                                {
                                                    if (SessionHandler.Usr.Update &&
                                                        r[htmlobject.Check].ToString() != "1")
                                                    {
                                                        enab = true;
                                                    }
                                                }
                                                else
                                                {
                                                    // no check
                                                    if (SessionHandler.Usr.Update)
                                                    {
                                                        enab = true;
                                                    }
                                                }

                                                #endregion

                                                #region checkbox

                                                // checkbox
                                                if (htmlobject.Columns.Items[i].Type == "checkbox")
                                                {
                                                    HtmlInputCheckBox inputbox = new HtmlInputCheckBox();
                                                    inputbox.Attributes["type"] = "checkbox";
                                                    inputbox.ID = "ctl_hcv" + section.ID + "_" + htmlobject.ID + "_" +
                                                                  i.ToString() + "_" + counter.ToString();
                                                    inputbox.Attributes.Add("parsley-enabled", "no");

                                                    inputbox.Checked = r[i].ToString() == "1" ? true : false;

                                                    inputbox.Disabled = !enab;


                                                    HtmlGenericControl label = new HtmlGenericControl("label");
                                                    label.Attributes["for"] = inputbox.ID;
                                                    label.InnerHtml = "";


//
//                                                    HtmlInputCheckBox a = new HtmlInputCheckBox();
//                                                    a.Checked = r[i].ToString() == "1" ? true : false;
//                                                    a.Attributes["class"] += " " + "checkbox";
//                                                    a.ID = "ctl_hcv" + section.ID + "_" + htmlobject.ID + "_" + i.ToString() + "_" + counter.ToString();
//                                                    // security
//                                                    if (enab == false) { a.Disabled = true; }
//                                                    a.Attributes.Add("parsley-enabled", "no");
////                                                    tablecell.Controls.Add(a);
                                                    tablecell.Controls.Add(inputbox);
                                                    tablecell.Controls.Add(label);
                                                }

                                                #endregion

                                                #region dropdown

                                                // dropdown
                                                if (htmlobject.Columns.Items[i].Type == "dropdown")
                                                {
                                                    HtmlSelect coldropdown = new HtmlSelect();
                                                    coldropdown.ID = "ctl_hv" + section.ID + "_" + htmlobject.ID + "_" +
                                                                     i.ToString() + "_" + counter.ToString();
                                                    coldropdown.Style["width"] =
                                                        htmlobject.Columns.Items[i].Width.ToString() + "px";
                                                    coldropdown.Attributes.Add("parsley-enabled", "no");
                                                    // security
                                                    if (enab == false)
                                                    {
                                                        coldropdown.Disabled = true;
                                                    }

                                                    // fill the dropdown
                                                    sql = htmlobject.Columns.Items[i].List;
                                                    // select or execute
                                                    if (htmlobject.Columns.Items[i].List.Substring(0, 2).ToLower() ==
                                                        "se" ||
                                                        htmlobject.Columns.Items[i].List.Substring(0, 2).ToLower() ==
                                                        "ex")
                                                    {
                                                        // replace with environment variables
                                                        sql = sql.Replace("@user", "'" + SessionHandler.Usr.User + "'");
                                                        sql = sql.Replace("@module",
                                                            "'" + SessionHandler.Qstring.Pageform + "'");
                                                        sql = sql.Replace("@filter",
                                                            "'" + SessionHandler.Qstring.Filter.Split('|')[0] + "'");
                                                        sql = sql.Replace("@role",
                                                            "'" + SessionHandler.accesslevel + "'");
                                                        sql = sql.Replace("@department",
                                                            "'" + SessionHandler.Usr.Department + "'");

                                                        //if (dtc.Rows.Count == 0)
                                                        //{
                                                        //    // only once
                                                        dtc = cs.Select(sql);
                                                        //}
                                                        foreach (DataRow colr in dtc.Rows)
                                                        {
                                                            ListItem lst = new ListItem();
                                                            lst.Value = colr[0].ToString();
                                                            lst.Text = dtc.Columns.Count > 1
                                                                ? colr[1].ToString()
                                                                : colr[0].ToString();
                                                            coldropdown.Items.Add(lst);
                                                        }
                                                    }
                                                    else
                                                    {
                                                        string[] values =
                                                            htmlobject.Columns.Items[i].List.Substring(1,
                                                                htmlobject.Columns.Items[i].List.Length - 2).Split(',');
                                                        List<string> listvalues = new List<string>(values.Length);
                                                        listvalues.AddRange(values);
                                                        foreach (string s in listvalues)
                                                        {
                                                            coldropdown.Items.Add(s.Trim());
                                                        }
                                                    }
                                                    coldropdown.Value = r[i].ToString();
                                                    tablecell.Controls.Add(coldropdown);
                                                }

                                                #endregion

                                                #region inputbox

                                                if (htmlobject.Columns.Items[i].Type == "inputbox")
                                                {
                                                    // other types default
                                                    HtmlInputText b = new HtmlInputText();
                                                    string value = "";
                                                    if (r[i] is Decimal || r[i] is Double || r[i] is Int32)
                                                    {
                                                        value = String.Format(System.Threading.Thread.CurrentThread.CurrentUICulture,
                                                            "{0:0,0}", r[i]);
                                                    }
                                                    else
                                                    {
                                                        value = r[i].ToString();
                                                    }



                                                    b.Value = value;
                                                           
                                          
                                             
                                                    if (enab == true)
                                                        b.Attributes.Add("OnKeyUp",
                                                            "checkElement('" + htmlobject.Columns.Items[i].DataType +
                                                            "')");
                                                    b.Style["width"] = htmlobject.Columns.Items[i].Width.ToString() +
                                                                       "px";
                                                    b.Attributes["class"] += " " +
                                                                             (enab == false
                                                                                 ? htmlobject.Columns.Items[i].CssClass +
                                                                                   "disabled"
                                                                                 : htmlobject.Columns.Items[i].CssClass);
                                                    b.ID = "ctl_hv" + section.ID + "_" + htmlobject.ID + "_" +
                                                           i.ToString() + "_" + counter.ToString();
                                                    // security
                                                    if (enab == false) b.Attributes["readOnly"] = "readonly";
                                                    b.Attributes.Add("parsley-enabled", "no");
                                                    tablecell.Controls.Add(b);
                                                }

                                                #endregion
                                                #region inputbox-number

                                                if (htmlobject.Columns.Items[i].Type == "inputbox-number")
                                                {
                                                    // other types default
                                                    HtmlInputText b = new HtmlInputText();
                                                    
                                                    string value = "";

                                                    if (r[i] is Decimal || r[i] is Double)
                                                    {
                                                        value = String.Format(System.Threading.Thread.CurrentThread.CurrentUICulture,
                                                            "{0:0,0}", r[i]);
                                                    }
                                                    else if (r[i] is Int32)
                                                    {
                                                        value = String.Format(System.Threading.Thread.CurrentThread.CurrentUICulture,
                                                            "{0:0,0}", r[i]);

//                                                        b.Attributes["step"] = "1";
                                                    }
                                                    else
                                                    {
                                                        value = r[i].ToString();
                                                    }


                                                    b.Value = value;
//                                                    b.Attributes["type"] = "number";
//                                                    b.Attributes["step"] = "any";


                                                    if (enab == true)
                                                        b.Attributes.Add("OnKeyUp",
                                                            "checkElement('" + htmlobject.Columns.Items[i].DataType +
                                                            "')");
                                                    b.Style["width"] = htmlobject.Columns.Items[i].Width.ToString() +
                                                                       "px";
                                                    b.Attributes["class"] += " " +
                                                                             (enab == false
                                                                                 ? htmlobject.Columns.Items[i].CssClass +
                                                                                   "disabled"
                                                                                 : htmlobject.Columns.Items[i].CssClass);
                                                    b.ID = "ctl_hv" + section.ID + "_" + htmlobject.ID + "_" +
                                                           i.ToString() + "_" + counter.ToString();
                                                    // security
                                                    if (enab == false) b.Attributes["readOnly"] = "readonly";
                                                    b.Attributes.Add("parsley-enabled", "no");
                                                    tablecell.Controls.Add(b);
                                                }
                                                #endregion

                                                #region password

                                                if (htmlobject.Columns.Items[i].Type == "password")
                                                {

                                                    tablecell.ID = "ctl_hv" + section.ID + "_" + htmlobject.ID + "_" + i.ToString() + "_" + counter.ToString();

                                                    tablecell.InnerHtml = "******";

                                                }
                                                #endregion

                                                #region textarea

                                                if (htmlobject.Columns.Items[i].Type == "textarea")
                                                {
                                                    // other types default
                                                    HtmlTextArea b = new HtmlTextArea();
                                                    b.Value = r[i].ToString();
                                                    if (enab == true)
                                                        b.Attributes.Add("OnKeyUp",
                                                            "checkElement('" + htmlobject.Columns.Items[i].DataType +
                                                            "')");
                                                    b.Style["width"] = htmlobject.Columns.Items[i].Width.ToString() +
                                                                       "px";
                                                    b.Style["height"] = htmlobject.Columns.Items[i].Height.ToString() +
                                                                        "px";
                                                    b.Attributes["class"] += " " +
                                                                             (enab == false
                                                                                 ? htmlobject.Columns.Items[i].CssClass +
                                                                                   "disabled"
                                                                                 : htmlobject.Columns.Items[i].CssClass);
                                                    b.ID = "ctl_hv" + section.ID + "_" + htmlobject.ID + "_" +
                                                           i.ToString() + "_" + counter.ToString();
                                                    // security
                                                    if (enab == false) b.Attributes["readOnly"] = "readonly";
                                                    tablecell.Controls.Add(b);
                                                }

                                                #endregion
                                            }
                                            else
                                                #region normnal

                                            // not updatable
                                            {

                                                tablecell.ID = "ctl_hv" + section.ID + "_" + htmlobject.ID + "_" + i.ToString() + "_" + counter.ToString();

                                                
                                               
                                                string value = "";
                                                if (r[i] is Decimal || r[i] is Double || r[i] is Int32)
                                                {
                                                    value = String.Format(System.Threading.Thread.CurrentThread.CurrentUICulture,
                                                        "{0:0,0}", r[i]);
                                                }
                                                else
                                                {
                                                    value = r[i].ToString();
                                                }
                                            tablecell.InnerHtml = value;

//                                                tablecell.InnerHtml = r[i].ToString();
//                                                span.InnerHtml = r[i].ToString();
                                                //
                                                if (!string.IsNullOrEmpty(htmlobject.Columns.Items[i].CssClass))
                                                {
                                                    tablecell.Attributes["class"] += " " + htmlobject.Columns.Items[i].CssClass;
                                                }
//                                                if (!string.IsNullOrEmpty(htmlobject.Columns.Items[i].Width))
//                                                {
//                                                    span.Style["width"] = htmlobject.Columns.Items[i].Width.ToString() + "%";
//                                                }
//
//                                                Debug.Print(htmlobject.Columns.Items[i].Type);
//                                                tablecell.Controls.Add(span);
                                            }

                                            #endregion

                                            #region hidden

                                            // hide?
                                            if (htmlobject.Columns.Items[i].Type == "hidden")
                                            {
                                                tablecell.Visible = false;
                                            }

                                            #endregion

                                            tablerow.Controls.Add(tablecell);
                                        }
                                        if (htmlobject.Edit != null && htmlobject.Edit != "")
                                        {
                                            HtmlTableCell editcell = new HtmlTableCell();
                                            HtmlAnchor editlink = new HtmlAnchor();
                                            editlink.HRef = "~/index.aspx?page=" + htmlobject.Edit + "&filter=" + r[0];
                                            editlink.Style.Add("color", "grey");
                                            editlink.Attributes.Add("class", "tinylink");


                                            HtmlGenericControl edit = new HtmlGenericControl("i");
                                            edit.Attributes["class"] = "fa fa-pencil-square-o fa-lg";
                                            editlink.Controls.Add(edit);

                                            editcell.Controls.Add(editlink);
                                            tablerow.Controls.Add(editcell);
                                        }

                                        if (!string.IsNullOrEmpty(htmlobject.Delete))
                                        {
                                            HtmlTableCell deletecell = new HtmlTableCell();
                                            HtmlAnchor deletelink = new HtmlAnchor();
                                            deletelink.HRef = "~/index.aspx?page=" + htmlobject.Delete + "&filter=" +
                                                              r[0];
                                            deletelink.Style.Add("color", "grey");
                                            deletelink.Attributes.Add("class", "tinylink");

                                            HtmlGenericControl delete = new HtmlGenericControl("i");
                                            delete.Attributes["class"] = "fa fa-times fa-lg";

                                            deletelink.Controls.Add(delete);


                                            deletecell.Controls.Add(deletelink);
                                            tablerow.Controls.Add(deletecell);
                                        }

                                        tablerow.Attributes["class"] += " " +
                                                                        (dt.Rows.IndexOf(r)%2 == 0 ? "even" : "odd");
                                        table.Controls.Add(tablerow);

                                        counter++;
                                    }
                                }

#endregion
                                // add label voor filter ???
                                // sectionpanel.Controls.Add(label2);
                                // sectionpanel.Controls.Add(CreateHTMLObjects.CreateBreak());
                                // add paging
                                // LiteralControl paging = new LiteralControl(sbpaging.ToString());
                                //  sectionpanel.Controls.Add(paging);

                                sectionpanel.Controls.Add(CreateHTMLObjects.CreateNewLine(1, 8));
                                sectionpanel.Controls.Add(table_wrapper);
                                break;

                                #endregion

                            case "dropdown":


                                enab = htmlobject.Enabled.ToLower() == "no" ? false : true;
                                sectionpanel.Controls.Add(buildDropdown(htmlobject, enab, icon, section));

                                #region dropdown

//                                HtmlSelect dropdown = new HtmlSelect();
//                                HtmlGenericControl label3 = new HtmlGenericControl("span");
//                                label3.InnerHtml = htmlobject.Label;
//                                label3.Attributes["class"] += " " + "label";
//                                dropdown.ID = "ctl" + section.ID + "_" + htmlobject.ID;
//                                dropdown.Attributes["tabindex"] = htmlobject.TabOrder.ToString();
//                                dropdown.Attributes["class"] += " col s"+ htmlobject.Width.ToString();
//
//                                // security
//                                enab = htmlobject.Enabled.ToLower() == "no" ? false : SessionHandler.Usr.Create || SessionHandler.Usr.Update || pageform.ActionType == "login" || pageform.ActionType == "search";
//
//                                // dropdown.Disabled = Helper.Reverse(enab);
//                                // readonly not disabled
//                                if (enab == false) { dropdown.Attributes["readonly"] = "readonly"; }
//
//                                // mandatory
//                                starr = (htmlobject.Mandatory == "yes" && pageform.ActionType != "search" && enab) ? "<span class=\"message\">&nbsp;*</span>" : "";
//                                label3.InnerHtml = htmlobject.Label + starr;
//                                dropdown.Attributes["class"] += " " + ( enab == false ? htmlobject.CssClass + "disabled" : (htmlobject.Mandatory == "yes" && pageform.ActionType != "search") ? htmlobject.CssClass + "mandatory" : htmlobject.CssClass);
//
//                                //reset for searchfields
//                                if (htmlobject.Target == "search")
//                                {
//                                    dropdown.Disabled = false;
//                                    dropdown.Attributes["class"] += " " +  htmlobject.CssClass;
//                                }
//
//                                // fill the dropdown
//                               sql = htmlobject.List;
//                                // select or execute
//                                if (htmlobject.List.Substring(0, 2).ToLower() == "se" || htmlobject.List.Substring(0, 2).ToLower() == "ex")
//                                {
//                                    // replace with environment variables
//                                    sql = sql.Replace("@user", "'" + SessionHandler.Usr.User + "'");
//                                    sql = sql.Replace("@module", "'" + SessionHandler.Qstring.Pageform + "'");
//                                    sql = sql.Replace("@filter", "'" + SessionHandler.Qstring.Filter.Split('|')[0] + "'");
//                                    sql = sql.Replace("@role", "'" + SessionHandler.accesslevel + "'");
//                                    sql = sql.Replace("@department", "'" + SessionHandler.Usr.Department + "'");
//
//                                    dt = cs.Select(sql);
//                                    foreach (DataRow r in dt.Rows)
//                                    {
//                                        ListItem lst = new ListItem();
//                                        lst.Value = r[0].ToString();
//                                        lst.Text = dt.Columns.Count > 1 ? r[1].ToString() : r[0].ToString();
//                                        dropdown.Items.Add(lst);
//                                    }
//                                }
//                                else
//                                {
//                                    string[] values = htmlobject.List.Substring(1, htmlobject.List.Length - 2).Split(',');
//                                    List<string> listvalues = new List<string>(values.Length);
//                                    listvalues.AddRange(values);
//                                    foreach (string s in listvalues)
//                                    {
//                                        dropdown.Items.Add(s.Trim());
//                                    }
//                                }
//
//                                sectionpanel.Controls.Add(label3);
//                                sectionpanel.Controls.Add(CreateHTMLObjects.CreateNewLine(1, 8));
//                                sectionpanel.Controls.Add(dropdown);

                                break;

                                #endregion

                                #region file

                            case "file":

                                enab = htmlobject.Enabled.ToLower() == "no" ? false : true;
                                sectionpanel.Controls.Add(buildFileInput(htmlobject, section, enab, icon));

                                break;

                                #endregion

                            case "textarea":

                                #region textarea

                                enab = htmlobject.Enabled.ToLower() == "no"
                                    ? false
                                    : SessionHandler.Usr.Create || SessionHandler.Usr.Update ||
                                      pageform.ActionType == "login" || pageform.ActionType == "search";
//                                
                                sectionpanel.Controls.Add(buildTextArea(htmlobject, section, enab, icon));


//                                HtmlTextArea textarea = new HtmlTextArea();
//                                HtmlGenericControl label4 = new HtmlGenericControl("span");
//                                label4.InnerHtml = htmlobject.Label;
//                                label4.Attributes["class"] += " " +  "label";
//                                textarea.ID = "ctl" + section.ID + "_" + htmlobject.ID;
//                                textarea.Attributes["class"] += " col s"+ htmlobject.Width.ToString();
//                                textarea.Style["height"] = htmlobject.Height.ToString() + "px";
//                                textarea.Attributes["tabindex"] = htmlobject.TabOrder.ToString();
//                                textarea.Attributes.Add("OnKeyUp", "checkElement('" + htmlobject.Datatype + "')");
//
//                                // security
//                                enab = htmlobject.Enabled.ToLower() == "no" ? false : SessionHandler.Usr.Create || SessionHandler.Usr.Update || pageform.ActionType == "login" || pageform.ActionType == "search";
//                                // textarea.Disabled = Helper.Reverse(enab);
//                                // readonly not disabled
//                                if (enab == false) { textarea.Attributes["readonly"] = "readonly"; }
//
//                                // mandatory
//                                starr = (htmlobject.Mandatory == "yes" && pageform.ActionType != "search" && enab) ? "<span class=\"message\">&nbsp;*</span>" : "";
//                                label4.InnerHtml = htmlobject.Label + starr;
//                                textarea.Attributes["class"] += " " + (enab == false ? htmlobject.CssClass + "disabled" : (htmlobject.Mandatory == "yes" && pageform.ActionType != "search") ? htmlobject.CssClass + "mandatory" : htmlobject.CssClass);
//
//                                sectionpanel.Controls.Add(label4);
//                                sectionpanel.Controls.Add(CreateHTMLObjects.CreateNewLine(1, 8));
//                                sectionpanel.Controls.Add(textarea);
                                break;

                                #endregion

                            case "button":

                                #region button

                                bool enabled = false;


                                if (string.IsNullOrEmpty(htmlobject.Enabled))
                                {
                                    htmlobject.Enabled = "yes";
                                }
                                if (htmlobject.Enabled != "no")
                                {
                                    htmlobject.Enabled = "yes";
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
                                    if ((htmlobject.Action == "insert" || htmlobject.Action == "file" ||
                                         htmlobject.Action == "copy" || htmlobject.Action == "execute" ||
                                         htmlobject.Action == "mail" || htmlobject.Action == "browse" ||
                                         htmlobject.Action == "upload") && SessionHandler.Usr.Create)
                                    {
                                        enabled = true;
                                    }
                                    if (htmlobject.Action == "search" && SessionHandler.Usr.Read)
                                    {
                                        enabled = true;
                                    }
                                    if (htmlobject.Action == "login" || htmlobject.Action == "back" ||
                                        htmlobject.Action == "logoff" || htmlobject.Action == "cancel")
                                    {
                                        enabled = true;
                                    }



                                    //BUSINES LOGIC FOR ROLES AND EXPORT


                                    if (htmlobject.Action == "export_excel")
                                    {
                                        enabled = true;

                                    }

                                }


                                if (htmlobject.Action == "browse")
                                {
                                    FileUpload fu = new FileUpload();
                                    fu.ID = "fileupload";
                                    fu.Enabled = enabled;
                                    fu.Attributes["tabindex"] = htmlobject.TabOrder.ToString();
                                    fu.Attributes["style"] = "opacity:0";
                                    fu.Width = 40;

                                    HtmlAnchor butd = new HtmlAnchor();
                                    butd.ID = "ctl" + section.ID + "_" + htmlobject.ID;
                                    butd.Attributes["class"] += " " + htmlobject.CssClass;
                                    butd.Controls.Add(fu);
                                    butd.Attributes["class"] += " " + "browse";
                                    sectionpanel.Controls.Add(butd);
                                    sectionpanel.Controls.Add(CreateHTMLObjects.CreateNewLine(1, 8));
                                }
                                else
                                {
                                    //create button


                                    HtmlAnchor button = new HtmlAnchor();
                                    button.Attributes["class"] += " waves-effect waves-light btn";
                                    if (htmlobject.Width != 0)
                                    {
                                        button.Attributes["class"] += " col s" + htmlobject.Width.ToString();
                                    }


                                    button.Attributes["type"] = "submit";
                                    button.Attributes["formsubmit"] = "";


                                    if (htmlobject.Icon != "" && htmlobject.Icon != null)
                                    {
                                        HtmlGenericControl buttonIcon = new HtmlGenericControl("i");
                                        buttonIcon.Attributes["class"] += " material-icons left";
                                        buttonIcon.InnerHtml = htmlobject.Icon;
                                        button.Controls.Add(buttonIcon);
                                    }

                                    button.InnerHtml = htmlobject.Label;
                                    button.Name = htmlobject.Action;
                                    button.Attributes["class"] += " " + htmlobject.CssClass;
                                    button.ID = "ctl" + section.ID + "_" + htmlobject.ID;
                                    button.Disabled = !enabled;


                                    //message

                                    String message = htmlobject.Action + " Succeeded.";
                                    if (htmlobject.Message != null && htmlobject.Message != "")
                                    {
                                        message = htmlobject.Message;
                                    }


                                    String onClick = "if(! ($(this).prop('disabled'))){ Materialize.toast('" + message +
                                                     " ', 2000) };";
                                    button.Attributes[
                                        "onclick"] = onClick;


//                                    HtmlAnchor linkbutton = new HtmlAnchor();
//                                    linkbutton.ID = "ctl" + section.ID + "_" + htmlobject.ID;
//                                    linkbutton.InnerHtml = htmlobject.Label;
//                                    linkbutton.Name = htmlobject.Action;
//                                    linkbutton.Attributes["class"] += " " + htmlobject.CssClass;
//                                    linkbutton.Attributes["tabindex"] = htmlobject.TabOrder.ToString();
//                                    linkbutton.Disabled = Helper.Reverse(enabled);
//                                    // add disabled class


                                    sectionpanel.Controls.Add(button);
                                    sectionpanel.Controls.Add(CreateHTMLObjects.CreateNonBreakingSpace(1));
                                }
                                break;

                                #endregion

                            case "link":

                                #region link

                                //HtmlAnchor link = new HtmlAnchor();
                                //link.Attributes["class"] += " " + htmlobject.CssClass;
                                //link.ID = "ctl" + section.ID + "_" + htmlobject.ID;

                                //// replace with environment variables
                                //string href = htmlobject.Url;
                                //href = href.Replace("@user", SessionHandler.Usr.User);
                                //href = href.Replace("@module", SessionHandler.Qstring.Pageform);
                                //href = href.Replace("@filter", SessionHandler.Qstring.Filter.Split('|')[0]);
                                //href = href.Replace("@role", SessionHandler.accesslevel);
                                //href = href.Replace("@department", SessionHandler.Usr.Department);

                                //string[] hr = href.Split(',');

                                //link.HRef = hr[0];
                                //if (hr.Length == 2)
                                //{
                                //    link.Target = hr[1];
                                //}


                                enabled = true;
                                if (!string.IsNullOrEmpty(htmlobject.Enabled))
                                {
                                    if (htmlobject.Enabled.ToLower() == "no")
                                    {
                                        enabled = false;
                                    }
                                }

                                if (htmlobject.Location == "right")
                                {
                                    HtmlGenericControl buttonDiv = new HtmlGenericControl("div");
                                    buttonDiv.Attributes["class"] += " fixed-action-btn";
                                    buttonDiv.Style.Add("bottom", "45px");
                                    buttonDiv.Style.Add("right", "24px");


                                    HtmlGenericControl button = new HtmlGenericControl("a");
                                    button.Attributes["class"] += " btn-floating btn-large red";
                                    button.ID = "ctl" + section.ID + "_" + htmlobject.ID;

                                    HtmlGenericControl buttonIcon = new HtmlGenericControl("i");
                                    buttonIcon.Attributes["class"] += "large material-icons";


                                    if (htmlobject.Icon != "" && htmlobject.Icon != null)
                                    {
                                        buttonIcon.InnerHtml = htmlobject.Icon;
                                    }
                                    else
                                    {
                                        buttonIcon.InnerHtml = "add";
                                    }


                                    button.Controls.Add(buttonIcon);
                                    buttonDiv.Controls.Add(button);


                                    if (enabled)
                                    {
                                        button.Attributes["href"] = createHref(htmlobject);
                                    }
                                    else
                                    {
                                        button.Attributes["class"] += " disabled";
                                    }


                                    sectionpanel.Controls.Add(buttonDiv);
                                    sectionpanel.Controls.Add(CreateHTMLObjects.CreateNonBreakingSpace(1));
                                }
                                else
                                {
                                    HtmlAnchor link = createLink(htmlobject, section);


                                    link.InnerHtml = htmlobject.Label;
                                    link.Disabled = htmlobject.Enabled.ToLower() == "yes" ? false : true;
                                    link.Attributes["class"] += " col s" + htmlobject.Width.ToString();
                                    // not form jbuttons


                                    sectionpanel.Controls.Add(link);
                                }


                                break;

                                #endregion

                            case "checkbox":

                                #region checkbox

//                                HtmlInputCheckBox checkbox = new HtmlInputCheckBox();
//                                HtmlGenericControl label5 = new HtmlGenericControl("span");
//                                label5.Attributes["class"] += " " + "label";
//                                label5.InnerHtml = htmlobject.Label;
//                                checkbox.ID = "ctl" + section.ID + "_" + htmlobject.ID;
//                                checkbox.Attributes["class"] += " " + htmlobject.CssClass;
//                                checkbox.Attributes["tabindex"] = htmlobject.TabOrder.ToString();
                                //security

                                enab = true;
                                if (!string.IsNullOrEmpty(htmlobject.Enabled))
                                {
                                    if (htmlobject.Enabled.ToLower() == "no")
                                    {
                                        enab = false;
                                    }
                                    else if (htmlobject.Enabled.ToLower() == "yes")
                                    {
                                        if (SessionHandler.Usr.Create || SessionHandler.Usr.Update)
                                        {
                                            enab = true;
                                        }
                                    }
                                }


                                sectionpanel.Controls.Add(buildCheckBox(htmlobject, section, enab, icon));
//
//                                sectionpanel.Controls.Add(label5);
//                                sectionpanel.Controls.Add(CreateHTMLObjects.CreateNewLine(1, 8));
//                                sectionpanel.Controls.Add(checkbox);
//                                sectionpanel.Controls.Add(CreateHTMLObjects.CreateNonBreakingSpace(1));


                                break;

                                #endregion

                                #region date

                            case "date":


                                enab = htmlobject.Enabled.ToLower() == "no" ? false : true;

                                sectionpanel.Controls.Add(buildInputField(htmlobject, section, enab, 0, icon, "date"));
                                //sectionpanel.Controls.Add(buildDateField(htmlobject, section, enab, null));


                                break;

                                #endregion

                                #region number

                            case "number":


                                enab = htmlobject.Enabled.ToLower() == "no" ? false : true;
                                sectionpanel.Controls.Add(buildInputField(htmlobject, section, enab, 0, icon, "number"));


                                break;

                                #endregion

                                #region email

                            case "email":

                                sectionpanel.Controls.Add(buildInputField(htmlobject, section, true, 0, null, "email"));


                                break;

                                #endregion

                            case "literal":

                                #region literal

                                LiteralControl literal = new LiteralControl();
                                literal.Text = htmlobject.Label;

                                literal.ID = "ctl" + section.ID + "_" + htmlobject.ID;
                                sectionpanel.Controls.Add(literal);
                                break;

                                #endregion

                                //case "break":
                            //    #region break
                            //    HtmlGenericControl br = new HtmlGenericControl("div");
                            //    br.Attributes.Add("class", "divider");
                            //    br.Controls.Add(new LiteralControl("&nbsp"));
                            //    sectionpanel.Controls.Add(br);
                            //    break;
                            //    #endregion
//
//                            case "rule":
//                                #region rule
//                                string rule = "<br />\n        <span style=\"display:inline-block;width:" + htmlobject.Width.ToString() + "px;\"><hr /></span><br />";
//                                sectionpanel.Controls.Add(CreateHTMLObjects.CreateLiteral(rule));
//                                break;
//                                #endregion

                            //case "listbox":

                                #region listbox

                                //    ListBox listbox = new ListBox();
                            //    HtmlGenericControl label6 = new HtmlGenericControl("span");
                            //    label6.InnerHtml = htmlobject.Label;
                            //    label6.Attributes["class"] += " " + "label";
                            //    listbox.ID = "ctl" + section.ID + "_" + htmlobject.ID;
                            //    listbox.Width = htmlobject.Width;
                            //    listbox.TabIndex = (short)htmlobject.TabOrder;
                            //    listbox.Enabled = htmlobject.Enabled.ToLower() == "no" ? false : SessionHandler.Usr.Create || SessionHandler.Usr.Update;
                            //    listbox.SelectionMode = ListSelectionMode.Multiple;
                            //    listbox.Rows = htmlobject.ListSize;

                            //    if (htmlobject.Target == "search")
                            //    {
                            //        listbox.Enabled = true;
                            //    }
                            //    // fill the listbox
                            //    sql = htmlobject.List;
                            //    // select or execute
                            //    if (htmlobject.List.Substring(0, 2).ToLower() == "se" || htmlobject.List.Substring(0, 2).ToLower() == "ex")
                            //    {
                            //        // replace with environment variables
                            //        sql = sql.Replace("@user", "'" + SessionHandler.Usr.User + "'");
                            //        sql = sql.Replace("@module", "'" + SessionHandler.Qstring.Pageform + "'");
                            //        sql = sql.Replace("@role", "'" + SessionHandler.accesslevel + "'");
                            //        sql = sql.Replace("@department", "'" + SessionHandler.Usr.Department + "'");

                            //        dt = cs.Select(sql);
                            //        foreach (DataRow r in dt.Rows)
                            //        {
                            //            ListItem lst = new ListItem();
                            //            lst.Value = r[0].ToString();
                            //            lst.Text = dt.Columns.Count > 1 ? r[1].ToString() : r[0].ToString();
                            //            listbox.Items.Add(lst);
                            //        }
                            //    }
                            //    else
                            //    {
                            //        string[] values = htmlobject.List.Substring(1, htmlobject.List.Length - 2).Split(',');
                            //        List<string> listvalues = new List<string>(values.Length);
                            //        listvalues.AddRange(values);
                            //        foreach (string s in listvalues)
                            //        {
                            //            listbox.Items.Add(s.Trim());
                            //        }
                            //    }

                            //    sectionpanel.Controls.Add(label6);
                            //    sectionpanel.Controls.Add(CreateHTMLObjects.CreateNewLine(1, 8));
                            //    sectionpanel.Controls.Add(listbox);
                            //    break;

                                #endregion

                            default:
                                break;
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                str = "Source:" + ex.Source;
                str += "\n" + "Message:" + ex.Message;
                ErHandler.errorMsg = str;
                ErHandler.throwError();
            }


            return p;
        }


        public static String createHref(HtmlObject htmlobject)
        {
            // replace with environment variables
            string href = htmlobject.Url;
            href = href.Replace("@user", SessionHandler.Usr.User);
            href = href.Replace("@module", SessionHandler.Qstring.Pageform);
            href = href.Replace("@filter", SessionHandler.Qstring.Filter.Split('|')[0]);
            href = href.Replace("@role", SessionHandler.accesslevel);
            href = href.Replace("@department", SessionHandler.Usr.Department);

            string[] hr = href.Split(',');

            String link = hr[0];
            if (hr.Length == 2)
            {
                link = hr[1];
            }

            return link;
        }

        public static HtmlAnchor createLink(HtmlObject htmlobject, Section section)
        {
            HtmlAnchor link = new HtmlAnchor();
            link.Attributes["class"] += " " + htmlobject.CssClass;
            link.ID = "ctl" + section.ID + "_" + htmlobject.ID;

            // replace with environment variables
            string href = htmlobject.Url;
            href = href.Replace("@user", SessionHandler.Usr.User);
            href = href.Replace("@module", SessionHandler.Qstring.Pageform);
            href = href.Replace("@filter", SessionHandler.Qstring.Filter.Split('|')[0]);
            href = href.Replace("@role", SessionHandler.accesslevel);
            href = href.Replace("@department", SessionHandler.Usr.Department);

            string[] hr = href.Split(',');

            link.HRef = hr[0];
            if (hr.Length == 2)
            {
                link.Target = hr[1];
            }

            return link;
        }
    }
}