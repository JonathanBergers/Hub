using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using RazorEngine;
using RazorEngine.Templating;

/// <summary>
/// Summary description for Renderer
/// </summary>

public static class Renderer
{


    public static string renderFileupload()
    {

//
//        string result =
//
//            Engine.Razor.RunCompile(template, "test2", null, new { Name = "World" });
        HtmlView fileUploadView = new FileUploadView();
        return fileUploadView.getTemplate();




    }
//    public static string renderFileupload()
//    {
//
//        string path = HttpContext.Current.Server.MapPath("~/views/Fileupload.cshtml");
//
//        Debug.Print("RENDERER DEBUG, path: " + path);
//
//        //        string template = "Hello @Model.Name, welcome to RazorEngine!";
//
//        string template = System.IO.File.ReadAllText(path);
//
//        Debug.Print("RENDERER DEBUG, template: " + template);
//        string result =
//        
//            Engine.Razor.RunCompile(template, "test2", null, new { Name = "World" });
//        
//        Debug.Print("RENDERER DEBUG, rendered: " + result);
//        
//
//        return result;
//
//    }
}