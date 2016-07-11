<%@ Page Language="C#" EnableEventValidation="false" AutoEventWireup="true" CodeFile="error.aspx.cs" Inherits="_Default" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
        <title>Error Page</title>
      <!-- Compiled and minified CSS -->
  <link rel="stylesheet" href="style/materialize/css/materialize.min.css">


</head>
<body>
    <!--Import jQuery before materialize.js-->
<script   src="https://code.jquery.com/jquery-2.2.3.min.js"   integrity="sha256-a23g1Nt4dtEYOj7bR+vTu7+T8VP13humZFBJNIYoEJo="   crossorigin="anonymous"></script>
 <script src="style/materialize/js/materialize.min.js"></script>

    <!-- Modal Structure -->
<div id="modal1" class="modal">
    <div class="modal-content">
        <h4>Session expired</h4>
        <p>Your session has expired. Please go back to the login page.</p>
        <p runat="server" id ="error_message"></p>
    </div>
    <div class="modal-footer">
        <a href="/index.aspx?page=login&filter=nofilter" class=" modal-action modal-close waves-effect waves-green btn-flat">Go back</a>
    </div>
</div>

    <script>
        

        $('#modal1').openModal();

    </script>


</body>
</html>
