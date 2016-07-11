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
    public static class MenuBuilder
    {



//        /**
//         * 
//         * FALCO
//         * Maakt een menu item voor html van een menu itm
//         */
//
//        public static HtmlGenericControl buildMenuItem(MenuItm m, HtmlGenericControl parent)
//        {
//
//            
//                           #region security
//                bool allowed = false;
//
//                if (SessionHandler.Usr.Roles == null)
//                {
//                    ErHandler.errorMsg = "Session has expired!";
//                    ErHandler.throwError();
//                }
//
//                foreach (string role in  SessionHandler.Usr.Roles)
//                {
//                    if (m.Security.ToLower() == role.ToLower())
//                    {
//                        allowed = true;
//                    }
//                }
//                string  test =  SessionHandler.Usr.Roles.Find(delegate(string a) { return a == m.Security.ToLower(); });
//                #endregion
//
//
//
//
//            if (m.Security == "application" || allowed)
//            {
//                HtmlGenericControl listItem = new HtmlGenericControl("li");
//
//                //            d.Attributes["class"] = "menuitem";
//                //            d.ID = "ctlmenu" + i;
//                HtmlGenericControl aItem = new HtmlGenericControl("a");
//                listItem.Controls.Add(aItem);
//                aItem.InnerHtml = m.MenuText;
//
//
//                if (m.Module != "" && m.Module != null)
//                {
//                    aItem.Attributes.Add("href", "/index.aspx?page=" + m.Module + "&filter=nofilter");
//                }
//                else
//                {
//                    //TODO DING IS GEEN MODULE< DOE IETS MET ON CLICK ?
//                    // ER WAS IETS IN DE JS
//                }
//
//               
////                parent.Controls.Add(listItem);
//
//                foreach (MenuItm childMenuItm in m.Children)
//                {
//                    listItem.Controls.Add(buildMenuItem(childMenuItm, listItem));
//                    //RECURSIVE
//                }
//                if (parent != null)
//                {
//                    parent.Controls.Add(listItem);
//                    return parent;
//                }
//                else
//                {
//                    return listItem;
//                }
//            }
//            else
//            {
//                return parent;
//
//
//            }





            

            //TODO
            // TOON KINDEREN


//            HtmlAnchor l = new HtmlAnchor();
//            if (m.Module != "" && m.Module != null)
//            {
//                l.HRef = "~/index.aspx?page=" + m.Module + "&filter=nofilter";
//            }
//            else
//            {
////                pmen = d.ID;
//                l.Style.Add("text-decoration", "underline");
//                l.HRef = "javascript:showsubmenu('" + d.ID + "')";
//
//            }



        

        public static HtmlGenericControl BuildMenu(XmlDocument xmlMenu)
        {
            #region variables

            //lijst met menuItems zonder kinderen
            HtmlGenericControl p = new HtmlGenericControl("ul");
            p.Attributes["class"] = "side-nav fixed";
            p.Style.Add("left", "0px");

            //lijst met menuItems met kinderen
            HtmlGenericControl pk = new HtmlGenericControl("div");
            pk.Attributes["class"] = "collapsible collapsible-accordion";
            pk.Attributes["data-collapsible"] = "accordion";
            p.Controls.Add(pk);

           
            p.ID = "pagemenu";
            MenuItms r = new MenuItms();
            r.Load(xmlMenu);
            int i = 0;
            string pmen = "";
            p.Controls.Add(CreateHTMLObjects.CreateNewLine(1, 0));
            #endregion







            //menu items toevoegen in loop:

            HtmlGenericControl lastSubMenuList = null;
            int index = 0;

            foreach (MenuItm m in r.Items)
            {

                #region security

                bool allowed = false;

                if (SessionHandler.Usr.Roles == null)
                {
                    ErHandler.errorMsg = "Session has expired!";
                    ErHandler.throwError();
                }

                foreach (string role in  SessionHandler.Usr.Roles)
                {
                    if (m.Security.ToLower() == role.ToLower())
                    {
                        allowed = true;
                    }
                }

                #endregion





                if (m.Security == "application" || allowed)
                {
                    HtmlGenericControl menuItem = buildMenuItem(m, index);
//                    if (m.Children == null || m.Children.Count == 0)
//                    {
//                        // geen kinderen
//                        pk.Controls.Add(menuItem);
//                    }
//                    else
//                    {
//                        //wel kinderen
//                        p.Controls.Add(menuItem);
//                    }
                    pk.Controls.Add(menuItem);
                   //pk.Controls.Add(buildMenuItem(m, index));
                }
                

                


                index ++;


//
//            #region old
//
//          #region security
//                bool allowed = false;
//
//                if (SessionHandler.Usr.Roles == null)
//                {
//                    ErHandler.errorMsg = "Session has expired!";
//                    ErHandler.throwError();
//                }
//
//                foreach (string role in  SessionHandler.Usr.Roles)
//                {
//                    if (m.Security.ToLower() == role.ToLower())
//                    {
//                        allowed = true;
//                    }
//                }
//                string  test =  SessionHandler.Usr.Roles.Find(delegate(string a) { return a == m.Security.ToLower(); });
//                #endregion
//
//if (m.Security == "application" || allowed)
//                {
//                   
//                    if (m.Level == 1)
//
//                    #region level 1
//                    {
//                        HtmlGenericControl menuItem = new HtmlGenericControl("li");
//                        menuItem.Style.Add("width", "100%");
//                       
//                        menuItem.ID = "ctlmenu" + i;
//                        HtmlAnchor l = new HtmlAnchor();
//                        if (m.Module != "" && m.Module != null)
//                        {
//
//                            //heeft geen kinderen
//                            l.HRef = "~/index.aspx?page=" + m.Module + "&filter=nofilter";
//                            l.Name = "menuItem";
//                            menuItem.Attributes["class"] = "menuitem bold waves-effect waves-light";
//                            l.InnerHtml = "<b>" + m.MenuText + "</b>";
//                            menuItem.Controls.Add(l);
//                            p.Controls.Add(menuItem);
//                        }
//                        else
//                        {   
//                            //heeft wel kinderen
//                            pmen = menuItem.ID;
//                            menuItem.Attributes["class"] = "";
//                            l.Name = "menuItem";
//                            l.Attributes["class"] = "collapsible-header waves-effect waves-light";
//                            l.InnerHtml = "<b>" + m.MenuText + "</b>";
//                            menuItem.Controls.Add(l);
//
//                            HtmlGenericControl subMenuList = new HtmlGenericControl("ul");
//                            subMenuList.Attributes["class"] = "collapsible-body";
//                            menuItem.Controls.Add(subMenuList);
//
//
//
//                            pk.Controls.Add(menuItem);
//                            lastSubMenuList = subMenuList;
//                        }
//
//                    
//                    }
//                    #endregion
//
//                    else
//
//                    #region level 2
//                    {
//                        HtmlGenericControl subMenuItem = new HtmlGenericControl("li");
//                        subMenuItem.Attributes["class"] = "submenuitem";
//                        subMenuItem.ID = pmen + "_ctlmenu_" + i;
//
//                        
//                       // d.Style.Add("display", "none");
//                        HtmlAnchor l = new HtmlAnchor();
//                        l.Name = "menuItem";
//                        l.HRef = "~/index.aspx?page=" + m.Module + "&filter=nofilter";
//                        l.InnerHtml = m.MenuText;
//                        subMenuItem.Controls.Add(CreateHTMLObjects.CreateNewLine(1, 8));
//                        subMenuItem.Controls.Add(l);
//                        lastSubMenuList.Controls.Add(CreateHTMLObjects.CreateNewLine(1, 8));
//                        lastSubMenuList.Controls.Add(subMenuItem);
//                    }
//                    #endregion
//
//                    i++;
//                }
//            }
//
//
//
//            p.Controls.Add(CreateHTMLObjects.CreateNewLine(1, 8));

            }
            return p;
            }




    public static HtmlGenericControl buildMenuItem(MenuItm m, int menuID)
{
    HtmlGenericControl menuItem = new HtmlGenericControl("li");
    menuItem.Style.Add("width", "100%");

    menuItem.ID = "ctlmenu" + menuID;
    HtmlAnchor l = new HtmlAnchor();




        if (m.Children == null || m.Children.Count == 0)
        {

            //heeft geen kinderen
            if (!string.IsNullOrEmpty(m.Module))
            {
               l.HRef = "~/index.aspx?page=" + m.Module + "&filter=nofilter"; 
            }
            
            l.Name = "menuItem";
            menuItem.Attributes["class"] = "collapsible-header menuitem bold waves-effect waves-light";
            l.InnerHtml = "<b>" + m.MenuText + "</b>";
            menuItem.Controls.Add(l);




        }
        else
        {

            //heeft wel kinderen
            String menuId = menuItem.ID;
            menuItem.Attributes["class"] = "";
            l.Name = "menuItem";
            l.Attributes["class"] = "collapsible-header waves-effect waves-light";
            l.InnerHtml = "<b>" + m.MenuText + "</b>";
            menuItem.Controls.Add(l);

            HtmlGenericControl subMenuList = new HtmlGenericControl("ul");
            subMenuList.Attributes["class"] = "collapsible-body";
            menuItem.Controls.Add(subMenuList);

            int index = 0;
            foreach (MenuItm menuItm in m.Children)
            {

                bool allowed = false;

                if (SessionHandler.Usr.Roles == null)
                {
                    ErHandler.errorMsg = "Session has expired!";
                    ErHandler.throwError();
                }

                foreach (string role in  SessionHandler.Usr.Roles)
                {

                    if (string.IsNullOrEmpty(m.Security))
                    {
                        allowed = true;
                        continue;
                    }
                    if (m.Security.ToLower() == role.ToLower())
                    {
                        allowed = true;
                    }
                }


                if (m.Security == "application" || allowed)
                {
                    subMenuList.Controls.Add(buildSubMenuItem(menuItm, menuItem.ID, index));
                   
                }


        
                index ++;


            }






            //voeg toe aan lijst met menuItems die collapseble zijn
        }

        return menuItem;

}

        public static HtmlGenericControl buildSubMenuItem(MenuItm m, String mID, int nr)
        {


            HtmlGenericControl subMenuItem = new HtmlGenericControl("li");
            subMenuItem.Attributes["class"] = "submenuitem";
            subMenuItem.ID = mID + "_ctlmenu_" + nr;


            // d.Style.Add("display", "none");
            HtmlAnchor l = new HtmlAnchor();
            l.Name = "menuItem";

            if (!string.IsNullOrEmpty(m.Module))
            {
                l.HRef = "~/index.aspx?page=" + m.Module + "&filter=nofilter";
            }
           
            l.InnerHtml = m.MenuText;
            subMenuItem.Controls.Add(CreateHTMLObjects.CreateNewLine(1, 8));
            subMenuItem.Controls.Add(l);


            return subMenuItem;
       


        }

            }

       




        




    


}