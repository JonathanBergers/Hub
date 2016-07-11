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
using System.Collections;
using System.Collections.Generic;

namespace EggwiseLib
{
    public static class SessionHandler
    {
        #region variables
        private static string _referrer     = "referrer";
        private static string _propFile     = "propFile";
        private static string _menuFile     = "menuFile";
        private static string _qstring      = "qstring";
        private static string _datatable    = "datatable";
        private static string _connection   = "connection";
        private static string _accesslevel  = "accesslevel";
        private static string _user         = "user";
        private static string _navigation   = "navigation";

        #endregion

        // connection
        public static string connection
        {
            get
            {
                if (HttpContext.Current.Session[SessionHandler._connection] == null)
                {
                    return string.Empty;
                }
                else
                {
                    return HttpContext.Current.Session[SessionHandler._connection].ToString();
                }
            }
            set
            {
                HttpContext.Current.Session[SessionHandler._connection] = value;
            }
        }

        // referrer
        public static string referrer
        {
            get
            {
                if (HttpContext.Current.Session[SessionHandler._referrer] == null)
                {
                    return string.Empty;
                }
                else
                {
                    return HttpContext.Current.Session[SessionHandler._referrer].ToString();
                }
            }
            set
            {
                HttpContext.Current.Session[SessionHandler._referrer] = value;
            }
        }

        // config file
        public static XmlDocument propFile
        {
            get
            {
                if (HttpContext.Current.Session[SessionHandler._propFile] == null)
                {
                    return null;
                }
                else
                {
                    return (XmlDocument)HttpContext.Current.Session[SessionHandler._propFile];
                }
            }
            set
            {
                HttpContext.Current.Session[SessionHandler._propFile] = value;
            }
        }

        // menu file
        public static XmlDocument menuFile
        {
            get
            {
                if (HttpContext.Current.Session[SessionHandler._menuFile] == null)
                {
                    return null;
                }
                else
                {
                    return (XmlDocument)HttpContext.Current.Session[SessionHandler._menuFile];
                }
            }
            set
            {
                HttpContext.Current.Session[SessionHandler._menuFile] = value;
            }
        }

        // accesslevel
        public static string accesslevel
        {
            get
            {
                if (HttpContext.Current.Session[SessionHandler._accesslevel] == null)
                {
                    return string.Empty;
                }
                else
                {
                    return HttpContext.Current.Session[SessionHandler._accesslevel].ToString();
                }
            }
            set
            {
                HttpContext.Current.Session[SessionHandler._accesslevel] = value;
            }
        }

        //datatable
        public static DataTable datatable
        {
            get
            {
                if (HttpContext.Current.Session[SessionHandler._datatable] == null)
                {
                    // empty datatable
                    DataTable dt = new DataTable();
                    return dt;
                }
                else
                {
                    return (DataTable)(HttpContext.Current.Session[SessionHandler._datatable]);
                }
            }
            set
            {
                HttpContext.Current.Session[SessionHandler._datatable] = value;
            }
        }

        //user
        public static Usr Usr
        {
            get
            {
                if (HttpContext.Current.Session[SessionHandler._user] == null)
                {
                    // empty user
                    Usr u = new Usr();
                    return u;
                }
                else
                {
                    return (Usr)(HttpContext.Current.Session[SessionHandler._user]);
                }
            }
            set
            {
                HttpContext.Current.Session[SessionHandler._user] = value;
            }
        }

        //querystring
        public static QString Qstring
        {
            get
            {
                if (HttpContext.Current.Session[SessionHandler._qstring] == null)
                {
                    // empty user
                    QString q = new QString();
                    return q;
                }
                else
                {
                    return (QString)(HttpContext.Current.Session[SessionHandler._qstring]);
                }
            }
            set
            {
                HttpContext.Current.Session[SessionHandler._qstring] = value;
            }
        }

        // navigation
        public static Navigation Navigation
        {
            get
            {
                    return (Navigation)(HttpContext.Current.Session[SessionHandler._navigation]);
            }
            set
            {
                HttpContext.Current.Session[SessionHandler._navigation] = value;
            }
        }

    }

}