<%@ Page Language="C#" EnableEventValidation="false" AutoEventWireup="true"  ValidateRequest="false" CodeFile="index.aspx.cs" Inherits="index" UICulture="auto" Culture="auto" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >


<head runat="server">
    
    <title>Place holder</title>
</head>
   
<main onload=""  style="height: 100%">
    <body>
    
    
        <script>
            

//            $("input").on("change.autofill", function () {
//                console.log('inputs = ', ' asd')
//                $("[for=password]").addClass("active");
//            }).click(function () {
//                $(this).unbind("change.autofill");
//            });
        </script>

         <form id="frm" runat="server" data-parsley-excluded="input[type=button], input[type=submit], input[type=reset], input[type=hidden], input[parsley-enabled=no], [disabled], :hidden" data-parsley-validate data-parsley-errors-wrapper=" " data-parsley-error-template=" " style="height: 100%">
        </form>
</body>
 

        <%--navigation bar dropdown menu.--%>
        
    <ul id="nav_dropdown" class="dropdown-content">
      <li><a onClick="Materialize.toast('Logged Off', 600)"href="/logoff.aspx">Log off</a></li>
    
    </ul>

    

    </main>

<%--    
     <footer class="page-footer">
          <div class="container">
            <div class="row">
              <div class="col l6 s12">
                <h5 class="white-text">Footer Content</h5>
                <p class="grey-text text-lighten-4">You can use rows and columns here to organize your footer content.</p>
              </div>
              <div class="col l4 offset-l2 s12">
                <h5 class="white-text">Links</h5>
                <ul>
                  <li><a class="grey-text text-lighten-3" href="#!">Link 1</a></li>
                  <li><a class="grey-text text-lighten-3" href="#!">Link 2</a></li>
                  <li><a class="grey-text text-lighten-3" href="#!">Link 3</a></li>
                  <li><a class="grey-text text-lighten-3" href="#!">Link 4</a></li>
                </ul>
              </div>
            </div>
          </div>
          <div class="footer-copyright">
            <div class="container">
            © 2014 Copyright Text
            <a class="grey-text text-lighten-4 right" href="#!">More Links</a>
            </div>
          </div>
        </footer>--%>
</html>
