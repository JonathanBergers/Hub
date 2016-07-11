using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

// manually added
using EggwiseLib;
using System.Text;
using System.Xml;
using System.Xml.Schema;

public partial class _Default : BasePage  //System.Web.UI.Page
{
    protected void Page_Init(object sender, EventArgs e)
    {
        this.EnableViewState = false;
     
        #region add style sheets

        // add the stylesheet
        Page.Header.Controls.Add(CreateHTMLObjects.CreateNewLine(1, 8));
        Page.Header.Controls.Add(CreateHTMLObjects.CreateCssLink("~/style/generic.css", "screen"));

        #endregion

//        #region add htmlobjects
//        // page title
//        this.Page.Title = "Something went wrong";
//
//        //add comment
//        fmError.Controls.Add(CreateHTMLObjects.CreatePageComments());
//
//        HtmlGenericControl p = new HtmlGenericControl("div");
//        p.ID = "errorbox";
//        p.Attributes["class"] = "errorbox";
//        fmError.Controls.Add(p);
//
//        HtmlGenericControl l = new HtmlGenericControl("span");
//        l.ID = "head";
//        p.Controls.Add(l);
//
//        HtmlGenericControl h = new HtmlGenericControl("span");
//        h.ID = "message";
//        h.Attributes["class"] = "message";
//        p.Controls.Add(h);
//
//        HtmlGenericControl pb = new HtmlGenericControl("div");
//        pb.ID = "buttonholder";
//        p.Controls.Add(pb);
//
//        HtmlAnchor b = new HtmlAnchor();
//        b.ID = "btnOK";
//        b.Attributes["class"] = "button";
//        b.InnerHtml = "OK";
//        b.ServerClick += new EventHandler(btnErrorOK_Click);
//        pb.Controls.Add(b);

//        #endregion

    }

    protected void Page_Load(object sender, EventArgs e)
    {
        #region variables

        HtmlGenericControl h = (HtmlGenericControl) error_message;
//        HtmlGenericControl l = (HtmlGenericControl)error_message.FindControl("message");
        XmlDocument versiondoc = (XmlDocument)Application["version"];
        VersionInfo v = new VersionInfo(versiondoc);

        #endregion

        #region  add error text

//        h.InnerHtml = "<h1>Error:</h1>";

        if (ErHandler.errorMsg == string.Empty)
        {
//            h.InnerHtml = "Generic error";
        }
        else
        {
            h.InnerHtml = "Error occured: " +  ErHandler.errorMsg;
        }

        #endregion


        if (!IsPostBack && SessionHandler.connection != "")
        {
            #region logging
            ConnectionString cs = new ConnectionString(SessionHandler.connection);

            // create log table
            cs.CreateLogTable(v.ErrorLog);

            // insert log record
            StringBuilder sblog = new StringBuilder();
            sblog.Append("insert ");
            sblog.Append(v.ErrorLog);
            sblog.Append("(ver, usr, page, descr, ts) ");
            sblog.Append("values ('");
            sblog.Append(v.Version);
            sblog.Append("','");
            sblog.Append(SessionHandler.Usr.User);
            sblog.Append("','");
            sblog.Append(SessionHandler.Qstring.Pageform);
            sblog.Append("','");
            sblog.Append(ErHandler.errorMsg.Replace(",", ";").Replace("'", "''").Replace("\n", " "));
            sblog.Append("','");
            sblog.Append(System.DateTime.Now);
            sblog.Append("')");
            cs.Update(sblog.ToString());
            # endregion
        }
    }


    protected void btnErrorOK_Click(object sender, EventArgs e)
    {
        // go back to main page
        ErHandler.clearError();

        if (SessionHandler.referrer.Length > 0)
        {
            Response.Redirect(SessionHandler.referrer, false);
        }
        else
        {
            Response.Redirect("~/index.aspx", true);
        }
    }
}
