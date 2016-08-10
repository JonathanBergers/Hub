using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

// added
using System.Xml;
using System.Xml.Schema;
using System.Text;
using System.Net.Mail;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace EggwiseLib
{






    public static class Tools
    {

        public static void addJavascript(Control htmlObject, string function_name, string parameter_string  )
        {
            if (string.IsNullOrEmpty(htmlObject.ID))
            {
            
                htmlObject.ID ="" +  System.Guid.NewGuid();

            }

            if (parameter_string == null)
            {
                parameter_string = "";
            }
            else
            {
                parameter_string = ", " + parameter_string;
            }
       
            htmlObject.Controls.Add(new LiteralControl("<script> var element = $('#" + htmlObject.ID + "'); " + function_name + "(element" + parameter_string + ");</script>" ));


        }

    }

    public static class ValidateXML
    {
        public static string ValidatingXML(string xmlFile, string xsdFile)
        {
            string m = null;
            try
            {
                XmlReaderSettings settings = new XmlReaderSettings();
                settings.ValidationType = ValidationType.Schema;
                settings.Schemas.Add("", xsdFile);
                XmlReader objXmlReader = XmlReader.Create(xmlFile, settings);

                while (objXmlReader.Read())
                {
                }

                objXmlReader.Close();
            }
            catch (XmlSchemaValidationException e)
            {
                m = e.Message.ToString();
            }

            return m;
        }

    }

    public static class Alert
    {
        public static void Show(string message)
        {
            string cleanMessage = message.Replace("'", "\\'");
            string script = "alert('" + cleanMessage + "');";
            Page page = HttpContext.Current.CurrentHandler as Page;
            Type cstype = page.GetType();
            ClientScriptManager cs = page.ClientScript;

            if (!cs.IsStartupScriptRegistered(cstype, "Alert"))
            {
                cs.RegisterStartupScript(cstype, "Alert", script, true);
            }

        }

        public static void ShowAndReload(string message)
        {
            string cleanMessage = message.Replace("'", "\\'");
            string script = "alert('" + cleanMessage + "'); window.location = window.location.href;";
            Page page = HttpContext.Current.CurrentHandler as Page;
            Type cstype = page.GetType();
            ClientScriptManager cs = page.ClientScript;

            
            if (!cs.IsStartupScriptRegistered(cstype, "Alert"))
            {
                cs.RegisterStartupScript(cstype, "Alert", script, true);
            }

        }

        public static void ShowNotie(string message)
        {
            
            string cleanMessage = message.Replace("'", "\\'");
            string script = "notie.alert('success', " + cleanMessage + "', 3)";
//            string script = "alert('" + cleanMessage + "');";
            Page page = HttpContext.Current.CurrentHandler as Page;
            Type cstype = page.GetType();
            ClientScriptManager cs = page.ClientScript;

            if (!cs.IsStartupScriptRegistered(cstype, "Alert"))
            {
                cs.RegisterStartupScript(cstype, "Alert", script, true);
            }



        }
    }

    public static class MailHandler
    {
        public static bool SendMail(string from, string to, string message, string file)
        {
            SmtpClient SmtpMail = new SmtpClient();
            string str = string.Empty;
            //SmtpMail.EnableSsl = true;

            try
            {
                MailMessage mail = new MailMessage(
                from,
                to,
                "Interface result",
                message);

                Attachment attach = new Attachment(file);
                mail.Attachments.Add(attach);
                SmtpMail.Send(mail);
                attach.Dispose();
                 
            }
            catch (System.Exception ex)
            {
                str = "Source:" + ex.Source;
                str += "\n" + "Message:" + ex.Message;
                MessageHandler.errorMsg = str;
                MessageHandler.throwError();
                return false;
            }

            return true;
        }

    }

    public static class FileName
    {
        public static string MakeFileName(string file, List<string> parameters)
        {
            file = file.ToLower();
            file = file.Replace("{user}", SessionHandler.Usr.User);
            file = file.Replace("{day}", System.DateTime.Now.Day.ToString());
            file = file.Replace("{month}", System.DateTime.Now.Month.ToString());
            file = file.Replace("{year}", System.DateTime.Now.Year.ToString());
            file = file.Replace("{module}", SessionHandler.Qstring.Pageform);
            file = file.Replace("{security}", SessionHandler.accesslevel);
            for (int i = 0; i < parameters.Count; i++)
            {
                file = file.Replace("{parameter" + (i + 1) + "}", parameters[i]);
            }


            return file; // ;
        }

    }


   


    public static class MessageHandler
    {

        private static string _errorMsg = "errorMsg";

        public static string errorMsg
        {

            get
            {
                if (HttpContext.Current.Session[MessageHandler._errorMsg] == null)
                {
                    return string.Empty;
                }
                else
                {
                    return HttpContext.Current.Session[MessageHandler._errorMsg].ToString();
                }
            }
            set
            {
                if (value == null)
                {
                    HttpContext.Current.Session[MessageHandler._errorMsg] = "Generic Error";
                }

                if (value != null && value != "clear")
                {
                    HttpContext.Current.Session[MessageHandler._errorMsg] = value + "\n" + HttpContext.Current.Session[MessageHandler._errorMsg];
                }
                else // ckear
                {
                    HttpContext.Current.Session[MessageHandler._errorMsg] = string.Empty;
                }
            }

        }

        public static void throwError()
        {
            HttpContext.Current.Response.Redirect("error.aspx", true);
        }

        public static void clearError()
        {
            errorMsg = "clear";  

        }

        public static void showError()
        {

            var error = errorMsg.Trim();

            if (error.Length == 0)
            {
                return;
            }
            HtmlGenericControl d = new HtmlGenericControl("div");
            Page page = (Page)HttpContext.Current.Handler;

            d = (HtmlGenericControl)page.FindControl("debugpanel");

            d.InnerHtml = "<script>var message = `\n" + error + "\n`\nnotie.alert(3, message, 2.5);</script>";




//            d.InnerHtml = "<script>var message = '"+ error+ "'; notie.alert(3, message, 2.5);</script>";
//            d.InnerHtml = "<span id=\"debugpanelspan\" style=\"font-family: Arial, Helvetica, sans-serif; color: red; font-size: 10pt;\">" + 
//            errorMsg + "</span>";
        }


        public static void showErrorClickToHide()
        {

            var error = errorMsg.Trim();

            if (error.Length == 0)
            {
                return;
            }
            HtmlGenericControl d = new HtmlGenericControl("div");
            Page page = (Page)HttpContext.Current.Handler;

            d = (HtmlGenericControl)page.FindControl("debugpanel");

            var messageScript = "<script>var message = `\n" + error + "\n`\nnotie.alert(3, message);</script>";
            d.InnerHtml = "<script>var message = `\n" + error + "\n`\nnotie.alert(3, message);</script>";
            //            d.InnerHtml = "<span id=\"debugpanelspan\" style=\"font-family: Arial, Helvetica, sans-serif; color: red; font-size: 10pt;\">" + 
            //            errorMsg + "</span>";
        }

        public static void showSuccess(string message)
        {

            message = message.Trim();

            if (message.Length == 0)
            {
                return;
            }
            HtmlGenericControl d = new HtmlGenericControl("div");
            Page page = (Page)HttpContext.Current.Handler;

            d = (HtmlGenericControl)page.FindControl("debugpanel");
            


            d.InnerHtml = "<script>var message = `\n" + message + "\n`\nnotie.alert(1, message, 2.5); +\n" + "</script>";


           


        }


        public static void showSuccessImport()
        {


//            var script = @"<script>
////    notie.confirm('Import succesfull!', 'See results', 'Stay here', function() {
////        window.location.reload(true);
////    });
////    </script>";
//
//            HtmlGenericControl d = new HtmlGenericControl("div");
//            Page page = (Page)HttpContext.Current.Handler;
//
//            d = (HtmlGenericControl)page.FindControl("debugpanel");
//
//
//
//            d.InnerHtml = script;



        }

    }

    public static class CleanInput
    {
        public static string Sanitize(string text)
        {
            string returnvalue = text;
            if (returnvalue.ToLower().Contains("drop "))
            {
                returnvalue = returnvalue.Replace("drop ", "");
            }

           // if (returnvalue.ToLower().Contains("select "))
           // {
           //    returnvalue = returnvalue.Replace("select ", "");
           // }

            if (returnvalue.ToLower().Contains("alter "))
            {
                returnvalue = returnvalue.Replace("alter ", "");
            }

            if (returnvalue.ToLower().Contains("truncate "))
            {
                returnvalue = returnvalue.Replace("truncate ", "");
            }

            if (returnvalue.ToLower().Contains("exec "))
            {
                returnvalue = returnvalue.Replace("exec ", "");
            }

            if (returnvalue.ToLower().Contains("execute "))
            {
                returnvalue = returnvalue.Replace("execute ", "");
            }

            if (returnvalue.ToLower().Contains(";"))
            {
                returnvalue = returnvalue.Replace(";", ",");
            }

            return returnvalue;
        }
    }

    public static class CreateHTMLObjects
    {
        public static HtmlLink CreateCssLink(string cssFilePath, string media)
        {
            HtmlLink link = new HtmlLink();
            link.Attributes.Add("type", "text/css");
            link.Attributes.Add("rel", "stylesheet");
            link.Href = link.ResolveUrl(cssFilePath);
            if (string.IsNullOrEmpty(media))
            {
                media = "all";
            }

            link.Attributes.Add("media", media);
            return link;
        }

        public static HtmlGenericControl CreateJavaScriptLink(string scriptFilePath)
        {
            HtmlGenericControl script = new HtmlGenericControl();
            script.TagName = "script";
            script.Attributes.Add("type", "text/javascript");
            script.Attributes.Add("src", script.ResolveUrl(scriptFilePath));

            return script;
        }

        public static LiteralControl CreateBreak()
        {
            LiteralControl br = new LiteralControl("<br />\n");
            return br;
        }

        //public static HtmlGenericControl CreateDiv(string id)
        //{
        //    HtmlGenericControl div = new HtmlGenericControl("div");
        //    div.ID = id;
        //    return div;
        //}

        public static LiteralControl CreateLiteral(string value)
        {
            LiteralControl l = new LiteralControl(value + "\n");
            return l;
        }

        public static LiteralControl CreateNewLine(int n, int space)
        {

            string sp = new String(' ', space);

            StringBuilder s = new StringBuilder();
            for (int i = 1; i <= n; i++)
            {
                s.Append("\n" + sp);
            }
            LiteralControl nl = new LiteralControl(s.ToString());
            return nl;
        }

        public static LiteralControl CreateNonBreakingSpace(int n)
        {

            string sp = "&nbsp;";

            StringBuilder s = new StringBuilder();
            for (int i = 1; i <= n; i++)
            {
                s.Append(sp);
            }
            LiteralControl nb = new LiteralControl(s.ToString());
            return nb;

        }

        public static LiteralControl CreatePageComments()
        {
            # region comment

            string a = string.Empty;
            string d = string.Empty;
            string v = string.Empty;
            string c = string.Empty;

            if (HttpContext.Current.Application["version"] != null)
            {
                XmlDocument vd = (XmlDocument)HttpContext.Current.Application["version"];
                VersionInfo vs = new VersionInfo(vd);
                a = vs.Author;
                d = vs.Date;
                v = vs.Version;
                c = vs.Copyright;
            }

            StringBuilder s = new StringBuilder();
            s.Append("\n");
            s.Append("<!--");
            s.Append(new String(' ', 14));
            s.Append(new String('-', 85));
            s.Append("\n\n");
            s.Append(new String(' ', 25));
            s.Append("This page is generated automatically from GRM ");
            s.Append(v);
            s.Append(" at ");
            s.Append(System.DateTime.Now.ToString());
            s.Append("\n\n");
            s.Append(new String(' ', 32));
            s.Append("Do not change or modify this code. Copyright ");
            s.Append(c);
            s.Append(" ");
            s.Append(d);
            s.Append("\n\n");
            s.Append(new String(' ', 39));
            s.Append("Author: ");
            s.Append(a);
            s.Append("\n\n");
            s.Append(new String(' ', 60));
            s.Append(",,,, \n");
            s.Append(new String(' ', 59));
            s.Append("/   ' \n");
            s.Append(new String(' ', 58));
            s.Append("/.. / \n");
            s.Append(new String(' ', 57));
            s.Append("( c  D \n");
            s.Append(new String(' ', 58));
            s.Append("\\- '\\_ \n");
            s.Append(new String(' ', 59));
            s.Append("`-'\\)\\ \n");
            s.Append(new String(' ', 62));
            s.Append("|_ \\ \n");
            s.Append(new String(' ', 62));
            s.Append("|U \\ \n");
            s.Append(new String(' ', 61));
            s.Append("(__,// \n");
            s.Append(new String(' ', 60));
            s.Append("|. \\/ \n");
            s.Append(new String(' ', 61));
            s.Append("LL__I \n");
            s.Append(new String(' ', 62));
            s.Append("||| \n");
            s.Append(new String(' ', 62));
            s.Append("||| \n");
            s.Append(new String(' ', 59));
            s.Append(",,-``'\\ \n\n");
            s.Append(new String(' ', 18));
            s.Append(new String('-', 85));
            s.Append(new String(' ', 16));
            s.Append("-->\n");
            s.Append("\n\n\n\n");

            #endregion

            LiteralControl comm = new LiteralControl(s.ToString());
            return comm;

        }

    }

    public class BasePage : Page
    {
        protected override void Render(HtmlTextWriter writer)
        {
            StringBuilder sb = new StringBuilder();
            StringWriter sw = new StringWriter(sb);
            HtmlTextWriter hWriter = new HtmlTextWriter(sw);
            base.Render(hWriter);
            string html = sb.ToString();
            html = Regex.Replace(html, "<input[^>]*id=\"(__VIEWSTATE)\"[^>]*>", string.Empty, RegexOptions.IgnoreCase);          
            writer.Write(html);
        }
    }

    public static class Helper
    {
        public static bool Reverse(bool value)
        {
            bool returnvalue = false;
            if (value == false)
            {
                returnvalue = true;
            }
            return returnvalue;
        }

    }

    public class Navigation
    {
        private List<string[]> _Items = new List<string[]>();

        public Navigation()
        {
            // start with empty values
            string[] s = new string[2];
            s[0] = string.Empty;
            s[1] = string.Empty;
            _Items.Add(s);
        }

        public void Add(string Item, string Title)
        {
            if (Item != _Items[_Items.Count - 1][0])
            {
                // remove first item from list
                if (_Items.Count > 20)
                {
                    _Items.RemoveAt(1);
                }

                string[] s = new string[2];
                s[0] = Item;
                s[1] = Title;

                _Items.Add(s);

            }
        }

        public void Clear()
        {
            _Items.RemoveRange(1, _Items.Count - 1);
        }

        public string GetFirst()
        {
            return _Items[1][0];
        }

        public void RemoveLast()
        {
            _Items.RemoveAt(_Items.Count - 1);
        }

        public string GetPrevious()
        {
            if (_Items.Count >= 2)
            {
                return _Items[_Items.Count - 2][0];
            }
            else
            {
                // empty
                return _Items[0][0];
            }
        }

        public string GetUrlByPage(string item)
        {
            //string output = string.Empty;
            string output = GetPrevious();

            _Items.Reverse();
            foreach (string[] itm in _Items)
            {
                if (itm[1] == item)
                {
                    output = itm[0];
                    break;
                }
            }
            _Items.Reverse();
            return output;
        }
    }


    public class CustomException : Exception
    {
        public CustomException()
        {
        }

        public CustomException(string message)
            : base(message)
        {
        }

        public CustomException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}

