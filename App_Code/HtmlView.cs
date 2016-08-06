using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EggwiseLib;
/// <summary>
/// Summary description for HtmlView
/// </summary>
public abstract class HtmlView
{

    private string RootPath = "~/views/";
    private string ViewPath;

    protected HtmlView(string viewPath)
    {
        ViewPath = viewPath;
    }

    protected HtmlView(string rootPath, string viewPath)
    {
        RootPath = rootPath;
        ViewPath = viewPath;
    }


    protected string getViewPath()
    {
        return RootPath + ViewPath;
    }

    protected string getServerPath()
    {
        return HttpContext.Current.Server.MapPath(getViewPath());
    }

    public string getTemplate()
    {
        return System.IO.File.ReadAllText(getServerPath());
    }
    


}


public class FileUploadView : HtmlView
{
    
    public FileUploadView() : base("Fileupload.cshtml")
    {
    }



}
