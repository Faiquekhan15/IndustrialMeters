<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AmiLogin.aspx.cs" Inherits="WebApplication1.WebForm1" %>

<!DOCTYPE html>
<html >
  <head>
    <meta charset="UTF-8">
    <title>T-RECS Login</title>
        <link rel="stylesheet" href="StyleSheet1.css">
      <link rel="stylesheet" href="StyleSheet2.css" />
      <link rel="stylesheet" href="animate.css" />
      <script src="https://ajax.googleapis.com/ajax/libs/jquery/1.12.0/jquery.js" charset="UTF-8"></script>
<script src="http://maxcdn.bootstrapcdn.com/bootstrap/3.3.6/js/bootstrap.js" charset="UTF-8"></script>
<script type="text/javascript" src="bootstrap-notify.js" charset="utf-8"></script>
        <link rel="stylesheet" href="http://maxcdn.bootstrapcdn.com/bootstrap/3.3.6/css/bootstrap.css"/>
    <script type="text/javascript">
        function ShowMessage(message, messagetype) {
            var cssclass, pastel, tuttle;
            switch (messagetype) {
                case 'Success':
                    cssclass = 'alert-success'
                    pastel = 'pastel-success'
                    tuttle = 'SUCCESS'
                    break;
                case 'Error':
                    cssclass = 'alert-danger'
                    pastel = 'pastel-danger'
                    tuttle = 'ERROR!'
                    break;
                case 'Warning':
                    cssclass = 'alert-warning'
                    pastel = 'pastel-warning'
                    tuttle = 'WARNING!'
                    break;
                default:
                    cssclass = 'alert-info'
                    pastel = 'pastel-info'
                    tuttle = 'INFO!'
            }
            //$('#alert_container').append('<div id="alert_div" style="margin: 0 0.5%; -webkit-box-shadow: 3px 4px 6px #999;" class="alert fade in ' + cssclass + '"><a href="#" class="close" data-dismiss="alert" aria-label="close">&times;</a><strong>' + messagetype + '!</strong> <span>' + message + '</span></div>');
            $.notify({
                title: tuttle,
                message: message
            }, {
                type: pastel,
                delay: 2000,
                template: '<div data-notify="container" class="col-xs-11 col-sm-3 alert alert-{0}" role="alert">' +
                    '<span data-notify="title">{1}</span>' +
                    '<span data-notify="message">{2}</span>' +
                '</div>'
            });
        };
    </script>
  </head>

  <body>
  <div class="container">
    <div class="row">
        <div class="col-md-4 col-md-offset-7">
            <div class="panel panel-default">
                <div class="panel-heading">
                    <span class="glyphicon glyphicon-user"></span>Transfopower Smart Energy System</div>
                <div class="panel-body">
                    <form class="form-horizontal" runat="server" role="form">
                            <asp:ScriptManager ID="up" runat="server"></asp:ScriptManager>
                    <div class="form-group">
                        <label for="inputEmail3" class="col-sm-3 control-label">
                            Username</label>
                        <div class="col-sm-9">
                            <input type="text" class="form-control" runat="server" id="inputEmail3" placeholder="Username" required>
                        </div>
                    </div>
                    <div class="form-group">
                        <label for="inputPassword3" class="col-sm-3 control-label">
                            Password</label>
                        <div class="col-sm-9">
                            <input type="password" class="form-control" runat="server" id="inputPassword3" placeholder="Password" required>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-sm-offset-3 col-sm-9">
                            <div class="checkbox">
                                <label>
                                    <input type="checkbox"/>
                                    Remember me
                                </label>
                            </div>
                        </div>
                    </div>
                                            <div class="form-group last">
                        <div class="col-sm-offset-3 col-sm-9">
                            <asp:Button ID="submit" Text="Login" runat="server" CssClass="btn btn-success btn-sm" OnClick="submit_Click" />
                                 <button type="reset" class="btn btn-default btn-sm">
                                Reset</button>
                        </div>
                    </div>
                    </form>
                </div>
                <div class="panel-footer">
                    Copyright &copy;2023 Transfopower Industries (Pvt) Limited </div>
            </div>
        </div>
                <div class="messagealert" id="alert_container">
            </div>
    </div>
</div>
  </body>
</html>


