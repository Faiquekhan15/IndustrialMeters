<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="main.aspx.cs" Inherits="WebApplication1.main" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<%@ Register Assembly="TimePicker" Namespace="MKB.TimePicker" TagPrefix="cc1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=edge" /> 
    <title>Dashboard</title>
  <link href="bootstrap-datetimepicker.css" rel="stylesheet" />
  <link rel="stylesheet" href="http://maxcdn.bootstrapcdn.com/bootstrap/3.3.6/css/bootstrap.css"/>
  <link href="simple-side.css" rel="stylesheet"/>
  <link href="font-awesome.css" rel="stylesheet" />

    <link href="https://cdn.datatables.net/1.10.12/css/dataTables.bootstrap.min.css" rel="stylesheet" />
    <link href="https://cdn.datatables.net/responsive/2.1.0/css/responsive.bootstrap.min.css" rel="stylesheet" />
    <link href="https://cdn.datatables.net/buttons/1.2.2/css/buttons.dataTables.min.css" rel="stylesheet" />
    <link href ="https://cdn.datatables.net/buttons/1.2.2/css/buttons.bootstrap.min.css" rel="stylesheet" />
    <link href="https://cdn.datatables.net/select/1.2.0/css/select.bootstrap.min.css" rel="stylesheet" />
    <link href="animate.css" rel="stylesheet"/>
    <link href="bootstrap-switch.css" rel="stylesheet" />
    <link href="StyleSheet2.css" rel="stylesheet" />
        <!-- Morris Charts CSS -->
    <link href="css/morris.css" rel="stylesheet"/>
    <link href="css/stats.css" rel="stylesheet"/>
    <link rel="stylesheet" href="vendors/morris/morris.css"/>
    <style type="text/css">
.panel-default > .panel-heading-custom{
  background-image: none;
  background-color: #1A5276;
  color: #ffffff;
}


.tab {
  overflow: hidden;
  border: 1px solid #ccc;
  background-color: #f1f1f1;
}

/* Style the buttons that are used to open the tab content */
.tab button {
  background-color: inherit;
  float: left;
  border: none;
  outline: none;
  cursor: pointer;
  padding: 14px 16px;
  transition: 0.3s;
}

/* Change background color of buttons on hover */
.tab button:hover {
  background-color: #ddd;
}

/* Create an active/current tablink class */
.tab button.active {
  background-color: #ccc;
}

/* Style the tab content */
.tabcontent {
  display: none;
  padding: 6px 12px;
  border: 1px solid #ccc;
  border-top: none;
}
.box {
    background-color: #f5f5f5;
    border-radius: 5px;
    padding: 10px;
    text-align: center;
    box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
    margin-bottom: 20px;
  }

  .box-body {
    padding: 7px 20px 7px 10px;
  }

  .radius-icon2 {
    font-size: 16px;
    color: #ffffff;
    background-color: #f0ad4e;
    border-radius: 50%;
    padding: 10px;
  }

  .radius-info {
    float: left;
    margin-top: 5px;
  }

  .radius-info-subheading {
    font-size: 12px;
  }


.panel-default > .panel-heading-custom2{
  background-image: none;
  background-color: #5B2C6F;
  color: #ffffff;
}
.panel-default > .panel-heading-custom3{
  background-image: none;
  background-color: #f07c4a;
  color: #ffffff;
}

.panel-default > .panel-heading-custom-Red{
  background-image: none;
  background-color: #CB4335;
  color: #ffffff;
}

.panel-default > .panel-heading-custom-Orange{
  background-image: none;
  background-color: #F39C12;
  color: #ffffff;
}

.panel-default > .panel-heading-custom-Blue{
  background-image: none;
  background-color: #2471A3;
  color: #ffffff;
}
.color-box {
  display: inline-block;
  width: 10px;
  height: 10px;
  margin-right: 5px;
}

.legend {
  margin-top: 10px;
  float:right;
}

.legend-item {
  display: inline-flex;
  align-items: center;
  margin-right: 15px;
}

.normal .color-box {
  background-color: green;
}

.alarm .color-box {
  background-color: yellow;
}

.overflow .color-box {
  background-color: red;
}



.panel-default > .panel-heading-custom-Yellow{
  background-image: none;
  background-color: #F1C40F;
  color: #ffffff;
}
        .redwhale {
            color: #D92121;
        }
        .bluewhale {
            color: #ffcc00;
        }
        .greenwhale {
            color: #009933;
        }
        .yellowwhale {
            color: #E19C26;
        }
        /* Scrollbar */

::-webkit-scrollbar {
    width: 8px;
}
/* Track */

::-webkit-scrollbar-track {
    -webkit-box-shadow: inset 0 0 6px rgb(153, 153, 153);
    -webkit-border-radius: 10px;
    border-radius: 10px;
}
/* Handle */

::-webkit-scrollbar-thumb {
    -webkit-border-radius: 10px;
    border-radius: 10px;
    background: rgba(5, 34, 147, 0.59);
    -webkit-box-shadow: inset 0 0 6px rgb(153, 153, 153);
}

::-webkit-scrollbar-thumb:window-inactive {
   background: rgb(153, 153, 153);
}

		/* Reduce bootstrap's default 'panel' padding: */
		div#tree {
			padding: 3px 5px;
		}
	/* Define custom width and alignment of table columns */
	#treetable {
		table-layout: fixed;
	}
	#treetable tr td:nth-of-type(1) {
		text-align: right;
	}
	#treetable tr td:nth-of-type(2) {
		text-align: center;
	}
	#treetable tr td:nth-of-type(3) {
		min-width: 100px;
		white-space: nowrap;
		overflow: hidden;
		text-overflow: ellipsis;
	}
    </style>
</head>
<body>
    <form id="form1" class="newback" runat="server" defaultbutton="btnDisableEnter" >
        <asp:ScriptManager ID="MainScript" runat="server" AjaxFrameworkMode="Enabled"></asp:ScriptManager>
                    <div id="loadingaa">
                        <h3 style="font-family:Calibri;color:black; margin-top:10%;">
                            <img src="e8a49208-e33f-4fa8-97be-b85fa435e980.png" style="height:24px;width:24px;" /> Loading your Dashboard...</h3>
                        <div class="spinner">
                            <div class="rect1"></div>
                            <div class="rect2"></div>
                            <div class="rect3"></div>
                            <div class="rect4"></div>
                            <div class="rect5"></div>

                        </div>
                        <h3 style="font-family:Calibri;color:black;">Please wait.</h3>

                        T-RDCS - Transfopower Industries SMART METERING Suite
</div>    
        <div class="messagealert" id="alert_container">
            </div>
        <asp:TextBox ID="typ" runat="server" Visible="false" />
        <div id="asss" style="min-width:1220px">         
                <nav class="navbar navbar-default no-margin">
    <!-- Brand and toggle get grouped for better mobile display -->
                <div class="navbar-header fixed-brand">
                    <button type="button" class="navbar-toggle collapsed" data-toggle="collapse"  id="menu-toggle">
                      <span class="glyphicon glyphicon-th-large" aria-hidden="true"></span>
                    </button>
                </div><!-- navbar-header-->
 
                <div class="collapse navbar-collapse" id="bs-example-navbar-collapse-1" style="background-color: black;color: white !important;">
                            <ul class="nav navbar-nav">
                                <li class="active" ><button class="navbar-toggle collapse in" data-toggle="collapse" id="menu-toggle-2"> <span class="glyphicon glyphicon-th-large" aria-hidden="true"></span></button></li>
                                <li><button id="boof" type="button" class="navbar-toggle collapse in" data-toggle="toggle" data-target="#dem"><i class="glyphicon glyphicon-plus"></i></button></li>
                                <li class="navbar-brand" title="Transfopower Industries Reliable Energy Data Collection Suite"><span style="color:rgba(13, 128, 165, 0.91);" class="glyphicon glyphicon-flash"></span><b style="color: white;">T-DRCS</b></li>
                                <li class="navbar-brand" data-toggle="modal" data-target="#dialog4"><span style="color:rgba(11, 225, 212, 0.93);" class="glyphicon glyphicon-user" title="Account"></span> <span style="color: white;">User</span></li>
                                <li class="navbar-brand" data-toggle="modal" data-target="#dialog3"><span style="color:rgba(11, 225, 212, 0.93);" class="glyphicon glyphicon-info-sign" title="Info"></span></li>  
                                <li class="navbar-brand" data-toggle="modal" data-target="#dialogEvents"><span style="color:rgba(230, 7, 7, 0.93);" class="glyphicon glyphicon-flag"></span><span style="color: white;"> E Dispatch</span></li>  
                                <li class="navbar-brand" data-toggle="modal" data-target="#dialogAlarmsConfiguration"><span style="color:rgba(252, 233, 17, 0.93);" class="glyphicon glyphicon-bell"></span><span style="color: white;"> Alarms</span></li>
                                <li class="navbar-brand" data-toggle="modal" data-target="#CriticalAlarmModal"><span style="color:rgba(241, 98, 15, 0.93);" class="glyphicon glyphicon-fire"></span><span style="color: white;"> Critical</span></li>    
                                <li class="navbar-brand" data-toggle="modal" data-target="#dialogEventReport"><span style="color:rgba(11, 225, 212, 0.93);" class="glyphicon glyphicon-floppy-save"></span><span style="color: white;"> Reports</span></li>  
                                <li class="navbar-brand" data-toggle="modal" data-target="#myModala"><span style="color:rgba(11, 225, 212, 0.93);" class="glyphicon glyphicon-stats"></span><span style="color: white;"> Flow</span></li>  
                                <li class="navbar-brand" data-toggle="modal" data-target="#errorModal"><span style="color:rgba(11, 225, 212, 0.93);" class="glyphicon glyphicon-warning-sign"></span><span style="color: white;"> e-Log</span></li>  
                                <li class="navbar-brand"><span style="color:rgba(230, 7, 7, 0.93);" class="glyphicon glyphicon-off"></span><asp:Button ID="Button1" runat="server" style="background-color:rgba(0, 0, 0, 0.00); color:white; border:none;" Text="Logout" OnClick="end_Click" /></li>   
                            </ul>
                </div><!-- bs-example-navbar-collapse-1 -->
    </nav>
    <div id="wrapper" style="display:none; background-color:rgb(238, 238, 238)">
        <!-- Sidebar -->
        <div id="sidebar-wrapper">
            <ul class="sidebar-nav nav-pills nav-stacked" id="menu">
 
                <li>
                    <a href="#"><span class="fa-stack fa-lg pull-left"><i class="fa fa-dashboard fa-stack-1x "></i></span> Asset Management</a>
                       <ul class="nav-pills nav-stacked" id="Ul1" runat="server" style="list-style-type:none;">
                                      <li title="Manual Registration">
            <a id="manregA" class="manrega" title="Manual Network Entry"><span class="fa-stack fa-lg pull-left"><i class="fa fa-list-alt fa-stack-1x "></i></span> Manual Registration</a> 
               </li>
                <li ><a id="autoregA" title="Auto Register Devices" class="autoreg"><span class="fa-stack fa-lg pull-left"><i class="fa fa-rocket fa-stack-1x "></i></span> Auto Registration</a>
                
                </li>
              
          <li><a id="deviceGroupA" title="Device Groups" class="deviceGroup"><span class="fa-stack fa-lg pull-left"><i class="fa fa-group fa-stack-1x "></i></span> Device Groups</a>

              </li>
            
            <li><a title="Meter Inventory" data-toggle="modal" data-target="#modalInventory"><span class="fa-stack fa-lg pull-left"><i class="fa fa-shopping-cart fa-stack-1x "></i></span> Meter Inventory</a>

             </li>
                    </ul>
                </li>
                
                <li>
                    <a href="#"><span class="fa-stack fa-lg pull-left"><i class="fa fa-calculator fa-stack-1x"></i></span>Data Analysis</a>
                              <ul id= "Ul3" class="nav-pills nav-stacked" style="list-style-type:none;" runat="server">
                                  <li style="display:none;"><a id="netstatA" title="Network State" class="offlineOnline">
                                      <span class="fa-stack fa-lg pull-left"><i class="fa fa-pie-chart fa-stack-1x "></i></span>Network Status</a>

              </li>
               <li >
                   <a id="ganalA" title="Graphical Data Analysis" class="stats">
                       <span class="fa-stack fa-lg pull-left"><i class="fa fa-line-chart fa-stack-1x "></i></span>Graphical Analysis</a>
                 
               </li>
            <li>  
                <a id="dwnrprtA" title="Download Reports" class="fileexp"><span class="fa-stack fa-lg pull-left"><i class="fa fa-floppy-o fa-stack-1x "></i></span>File Export</a>
                    
             </li>
            <li >
                <a id="lpA" title="Veiw,download and analyse Load Profile Data" class="LP"><span class="fa-stack fa-lg pull-left"><i class="fa fa-area-chart fa-stack-1x "></i></span>Load Profile</a>
                    
             </li>

            </ul>

                </li>
                
                <li>
                    <a href="#"> <span class="fa-stack fa-lg pull-left"><i class="fa fa-lock fa-stack-1x "></i></span>Security Management</a>
                    <ul id="Ul4" class="nav-pills nav-stacked" style="list-style-type:none;" runat="server">
               <li >
                   <a id="usrmgmtA" title="Manage Users" class="usermgmt"><span class="fa-stack fa-lg pull-left"><i class="fa fa-users fa-stack-1x "></i></span>User Management</a>
             
               </li>
                        <li ><a id="acclgA" title="T-RECS User Access Log" class="acclog"><span class="fa-stack fa-lg pull-left"><i class="fa fa-sign-in fa-stack-1x "></i></span>Access Log</a>
                
                </li>
               <li >
                   <a id="actlgA" title="User acitvity trail" class="actlog"><span class="fa-stack fa-lg pull-left"><i class="fa fa-check fa-stack-1x "></i></span>Activity Log</a>
                
                </li>
              <li >
                  <a id="commlgA" title="Attemped communications trail" class="commLog"><span class="fa-stack fa-lg pull-left"><i class="fa fa-random fa-stack-1x "></i></span>Communication Log</a>
                
                </li>
              
                  </ul>
                </li>
                <li>
                    <a href="#"><span class="fa-stack fa-lg pull-left"><i class="fa fa-youtube-play fa-stack-1x "></i></span>Real-time Reading</a>
                                 <ul id="Ul5" class="nav-pills nav-stacked" style="list-style-type:none;" runat="server">
               <li >
                   <a id="instRA" title="Quick energy data reading" class="instRead"><span class="fa-stack fa-lg pull-left"><i class="fa fa-download fa-stack-1x "></i></span>Instant Read</a>
             
               </li>
                <li >
                    <a id="dataRA" title="Advanced data fetching" class="dataRead"><span class="fa-stack fa-lg pull-left"><i class="fa fa-database fa-stack-1x "></i></span>Data Read</a>
                </li>

                  
                
                  </ul>
                </li>
                
        </div><!-- /#sidebar-wrapper -->
        <!-- Page Content -->
        <div id="page-content-wrapper">
                    <table>
                      <tr >
                          <td>
                              <div id="dem" class="">  
                                 <div id="menu1" class="panel panel-default" style="overflow-y:auto; border:none; overflow-x:auto;">  
                                       <div class="panel panel-default" style="border:none;">
                                        <div class="panel-heading">
                                         <div class="panel-title">
                                             <table>
                                                 <tr>
                                                     <td>
                                                         <div class="input-group">
                                                             <div class="input-group-btn">
                                                                 <button type="button" title="Sort by" class="btn btn-default dropdown-toggle btn-sm" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">Sort by <span class="caret"></span></button>
                                                                 <ul class="dropdown-menu">
                                                                     <li><a><i class="fa fa-refresh fa-spin"></i> Loading...</a></li>

                                                                 </ul>
                                                                 <asp:LinkButton ID="LinkButtonTreeSearch" runat="server" CssClass="btn btn-default btn-sm" ToolTip="Search" Enabled="false" OnClick="LinkButtonTreeSearch_Click">
                                                                     <span aria-hidden="true" class="glyphicon glyphicon-search"></span>

                                                                 </asp:LinkButton>
                                                                 <asp:LinkButton ID="LinkButtonRefreshTree" runat="server" CssClass="btn btn-default btn-sm" ToolTip="Load Tree" OnClientClick="treeit()" Enabled="false" OnClick="LinkButtonRefreshTree_Click">
                                                                     <span aria-hidden="true" class="glyphicon glyphicon-refresh"></span>

                                                                 </asp:LinkButton>
                                                                 <asp:LinkButton ID="LinkButtonHealthReport" runat="server" data-toggle="modal" data-target="#dialogHealthReport" CssClass="btn btn-default btn-sm" ToolTip="Generate Health Report">
                                                                     <span aria-hidden="true" class="glyphicon glyphicon-heart"></span>

                                                                 </asp:LinkButton>
      </div><!-- /btn-group -->
      <input type="text" id="searchTextTree" placeholder="Network Tree" runat="server" class="form-control input-sm"  maxlength="10" onkeydown = "return (!(event.keyCode>=65) && event.keyCode!=32);" aria-label="...">
    </div><!-- /input-group -->
                                                     </td>
                                                     <td>
                                        <asp:UpdateProgress runat="server" ID="UpdateProgress9">
                    <ProgressTemplate>
                        <i class="fa fa-circle-o-notch fa-lg fa-spin"></i>
                    </ProgressTemplate>
                </asp:UpdateProgress>
                                                     </td>
                                                     
                                                 </tr>
                                             </table>
                                         </div>
                                      </div>
    <div class="tree small">
            <table>
                <tr>
                    <td><input type="text" id="res" style="display:none;" runat="server" class="form-control" /></td>
                    <td><input type="text" id="sclass" style="display:none;" runat="server" class="form-control" /></td>
                </tr>
                
            </table> 
        <div id="tree" class="panel panel-body fancytree-colorize-hover fancytree-fade-expander">
		</div>  
</div>

                                               </div>
                                               </div>
                                    </div>
                          </td>
                          <td style="width:100%;">
            <div id="dope" class="panel panel-default" style="overflow:auto; border:none;">
                <div class="container-fluid ">
    <ul class="nav nav-tabs marginBottom" id="myTab">
        <li title="TRANSFOPOWER AMI"><a class="homie"><img src="logo.png" width="80" /></a>

        </li>
        <li title="Home" id="homo" class="active "><a class="homie">Home </a>

        </li>
    

    </ul>
</div>
                <div id ="homie" class="Home" runat="server" style="width:100%;overflow-x:auto;overflow-y:auto;">
    <br />
                <div class="container-fluid">
                    <div class="panel-body">
  <div class="row">
    <div class="col-md-4">
      <div class="panel panel-default">
        <div class="panel-body text-center">
          <i class="fa fa-building fa-2x"></i>
          <h5>Management Object</h5>
          <p id="pM0"></p>
        </div>
      </div>
    </div>
    <div class="col-md-4">
      <div class="panel panel-default">
        <div class="panel-body text-center">
          <i class="fa fa-users fa-2x"></i>
          <h5>Number of Departments</h5>
          <p id="pNoD"></p>
        </div>
      </div>
    </div>
    <div class="col-md-4">
      <div class="panel panel-default">
        <div class="panel-body text-center">
          <i class="fa fa-bolt fa-2x"></i>
          <h5>Number of Saving Meters</h5>
          <p id="NoSM"></p>
        </div>
      </div>
    </div>
  </div>
</div>

                  <div class="tab">
  <button class="tablinks" onclick="openCity(event, 'Departmental_Consumption')">Department electricity consumption</button>
  <button class="tablinks" onclick="openCity(event, 'Powerfactor')">Power Factor</button>
  <button class="tablinks" onclick="openCity(event, 'Tvoltage')">Voltage</button>
   <button class="tablinks" onclick="openCity(event, 'Tcurrent')">Current</button>
</div>

<!-- Tab content -->
<div id="Departmental_Consumption" class="tabcontent">
     <div class="panel-heading">
    <i class="fa fa-bar-chart"></i>
    Electricity Consumption this month
      
    <span class="badge">
      <b>
        <i class="fa fa-refresh" onclick="Loadmainconsumption()"></i>
      </b>
    </span>
         <div class="legend">
  <div class="legend-item normal">
    <div class="color-box"></div>
    <span>Normal</span>
  </div>
  <div class="legend-item alarm">
    <div class="color-box"></div>
    <span>Alarm</span>
  </div>
  <div class="legend-item overflow">
    <div class="color-box"></div>
    <span>Overflow</span>
  </div>
</div>


  </div>
    
  <div class="panel-body">
    <div id="consumption" style="height:400px;"></div>
      </div>
</div>
<div id="Powerfactor" class="tabcontent">
  

     <div class="panel-heading">
    <i class="fa fa-bar-chart"></i>
    Power Factor
 <button id="chartToggleBtn2" style="
    float: right;
">  <i class="fa fa-line-chart"></i>
</button>
  </div>

    <div class="panel-body">
  
  <div id="pfchart" style="height: 425px;"></div>
</div>

</div>
<div id="Tvoltage" class="tabcontent">
  
         <div class="panel-heading">
    <i class="fa fa-bar-chart"></i>
    Total Voltages
    <button id="chartToggleBtn1" style="
    float: right;
">  <i class="fa fa-line-chart"></i>
</button>
  </div>

      <div class="panel-body">
  
  <div id="VoltageGraph" style="height: 425px;"></div>
          </div>
</div>
<div id="Tcurrent" class="tabcontent">
      <div class="panel-heading">
    <i class="fa fa-bar-chart"></i>
    Total Current
<button id="chartToggleBtn" style="
    float: right;
">  <i class="fa fa-line-chart"></i>
</button>

  </div>
          <div class="panel-body">
  <div id="CurrentGraph" style="height: 425px;"></div>
</div>
</div>


             <div class="panel panel-default">
      <div class="panel-heading" style="padding: 20px;">
        <i class="fa fa-pie-chart"></i>
        Electricity Consumption this month
        <button id="toggleButton" class="btn btn-primary" style="float: right; background-color: white;color: black;">+</button>
      </div>
    
      <div class="panel-body">
        <div id="consumption_pie_chart" style="height:400px;"></div>
      </div>
    
 

             </div>

 <div class="panel panel-default">
      <div class="panel-heading" style="padding: 20px;">
        <i class="fa fa-bar-chart"></i>
        Energy used Year
        <button id="toggleButton1" class="btn btn-primary" style="float: right; background-color: white;color: black;">+</button>
      </div>
    
      <div class="panel-body">
        <div id="consumption_year_chart" style="height:400px;"></div>
      </div>
    
 

             </div>

                     <div class="panel panel-default">
      <div class="panel-heading" style="padding: 20px;">
        <i class="fa fa-pie-chart"></i>
        Energy used Year
        <button id="toggleButton2" class="btn btn-primary" style="float: right; background-color: white;color: black;">+</button>
      </div>
    
      <div class="panel-body">
        <div id="consumption_year_pie_chart" style="height:400px;"></div>
      </div>
    
 

             </div>

  <div class="row">
    <div class="col-sm-5">
        <div class="panel panel-default">
            <div class="panel-heading">
                <i class="fa fa-user-times" style="color:red;"></i> Mute Meters  <span class="badge"><i class="fa fa-refresh" onclick="addingMT()"></i></span>
                 <asp:UpdateProgress runat="server" ID="UpdateProgress10">
                    <ProgressTemplate>
                        <i class="fa fa-circle-o-notch fa-lg fa-spin"></i>
                    </ProgressTemplate>
                </asp:UpdateProgress>
  </div>
  <div class="panel-body">
                            <table id="exampaa" class="display compact table table-striped table-bordered" style="width:100%; font-size:small">
        <thead>
            <tr>
                <th>Serial</th>
                <th>Last Online</th>
            </tr>
        </thead>
        <tfoot>
            <tr>
                <th>Serial</th>
                <th>Last Online</th>
            </tr>
        </tfoot>
    </table>
      </div>
                                       </div>
    </div>
        <div class="col-sm-7">
                 <div class="panel-heading">
    <i class="fa fa-file" style="color:red;"></i> Total Meter Data
</div>
<div class="panel-body">
    <table id="MainData" class="display compact table table-striped table-bordered" style="width:100%; font-size:small">
        <thead>
            <tr>
                <th>Attribute</th>
                <th>Value</th>
            </tr>
        </thead>
        <tbody>
            <tr>
                <td>Cumulative Power Consumption</td>
                <td><span id="CPC" runat="server"></span></td>
            </tr>
            <tr>
                <td>Monthly total quantity of electricity</td>
                <td><span id="Mtqoe" runat="server"></span></td>
            </tr>
            <tr>
                <td>Daily total quantity of electricity</td>
                <td><span id="Dtqoe" runat="server"></span></td>
            </tr>
            <tr>
                <td>Maximum Power Factor</td>
                <td><span id="MaxPF" runat="server"></span></td>
            </tr>
            <tr>
                <td>Minimum Power Factor</td>
                <td><span id="MinPaf" runat="server"></span></td>
            </tr>


        </tbody>
    </table>
</div>
               
                </div>
  </div>
<div class="row">

</div>
</div>

                </div>
              
               



                   <div id ="dataRead" class="On Demand Reading" runat="server" style="display:none; width:100%;overflow-x:auto;overflow-y:auto;">
                        
                         <asp:Timer runat="server" id="UpdateTimer" interval="5000" ontick="UpdateTimer_Tick" Enabled="false" />
                                <asp:UpdatePanel ID="OnDemandReadingUpdatePanel" UpdateMode="Conditional" runat="server">
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger controlid="UpdateTimer" eventname="Tick" />
                                        <asp:PostBackTrigger ControlID="LinkButtonOnDemandDataDownload" />

                                    </Triggers>
                                    <ContentTemplate>
            <table class ="table table-condensed">
                <tr>
                    <td>
                         Quantity Type<asp:DropDownList ID="dataType" CssClass="form-control input-sm" runat="server">
                <asp:ListItem Selected="True">Meter Data</asp:ListItem>
                
            </asp:DropDownList>
                    </td>
                    <td>
                    <br />
                    <div style="min-width:130px;">
<asp:Button ID="read" Text="Read" CssClass="btn btn-primary btn-sm" runat="server" OnClick="read_Click" />
                        <asp:LinkButton ID="LinkButtonOnDemandDataDownload" 
                runat="server"  Enabled="false"
                CssClass="btn btn-danger btn-sm" data-toggle="tooltip" ToolTip="Download PDF"  
                OnClick="LinkButtonOnDemandDataDownload_Click" >
    <span aria-hidden="true" class="glyphicon glyphicon-download-alt"></span> PDF
</asp:LinkButton>
                        </div>
             <asp:TextBox ID="TextBoxData" runat="server" Visible="false"></asp:TextBox>
                        </td>
                    <td>
                        <br />
                        <asp:UpdateProgress runat="server" ID="UpdateProgress5" AssociatedUpdatePanelID="OnDemandReadingUpdatePanel">
                    <ProgressTemplate>
                        <i class="fa fa-circle-o-notch fa-lg fa-spin"></i>
                    </ProgressTemplate>
                </asp:UpdateProgress>
                    </td>
                </tr>
            </table>
                    </ContentTemplate>
        </asp:UpdatePanel>
                                <div id="ondemandHide">
             <div class="progress" id="ondemandContainer">
                 <div class="progress-bar progress-bar-success" id="ondemandBar" role="progressbar" aria-valuenow="0" aria-valuemin="0" aria-valuemax="100">

                 </div>

             </div>

         </div>
                       
                       
                       <div class="panel panel-default" style="border:none;">
                            <div class="panel-body">
       
                   
                  
                              
        <table id="instRtable" class="display compact table table-striped table-bordered  dt-responsive nowrap" style="width:100%; font-size:small">
        <thead>
            <tr>
                <th>Meter Serial</th>
                <th>Description</th>
                <th>Value</th>
                <th>Time-Stamp</th>
            </tr>
        </thead>
        <tfoot>
            <tr>
                <th>Meter Serial</th>
                <th>Description</th>
                <th>Value</th>
                <th>Time-Stamp</th>
            </tr>
        </tfoot>
    </table>
        </div>
  </div>
             

                   </div>
                <div id ="manrega"  class="Manual Registration" runat="server" style="display:none; width:100%;overflow:hidden;">
                                                                <div class="panel panel-default" style="border:none;">
                                                                    <div class="panel-body">  
                                                                        <div class="row">
        <div class="col-md-6">
          <div class="thumbnail">
            <img src="TRDRNPic.jpg" alt="...">
            <div class="caption">
              <h3>Check meter</h3>
              <p>Din Rail Meter</p>
              <p><a class="btn btn-primary" data-toggle="modal" data-target="#meterAddition">Add Device</a></p>
            </div>
          </div>
        </div>
      </div>
      
    <div class="panel panel-danger" style="text-align:center">
        <div class="panel-heading"><h4><i class="fa fa-trash-o"></i> Remove Device</h4></div>
        <div class="panel-body">
            <table style="width:100%;">
                <tr>
                    <td>
                        <h4 id="mus" style="color:darkred">No Selection</h4>
                    </td>
                    <td>
            <asp:LinkButton ID="meterRemove" ToolTip="Warning! Device removal may result in loss of records. Do you want to proceed?" CssClass="btn btn-danger" data-toggle="confirmation" runat="server" OnClick="meterRemove_Click">
                <i class="fa fa-trash-o"></i> Remove Device
            </asp:LinkButton>
                    </td>
                </tr>
            </table>
        </div>
    </div>
      </div>
                                                                    </div>
                    </div>


                    <div id ="autoreg"  class="Automatic Registration" runat="server" style="display:none; width:100%;overflow:auto;">
                                                <div class="panel panel-default" style="border:none;">
      <div class="panel-body">
                                <table class="table table-condensed">
                                        <tr>
          <td>
            <div class="input-group">
              <input type="text" id="SearchBoxAutoReg" title="Search By Serial" runat="server" onkeydown="return (!(event.keyCode>=65) && event.keyCode!=32);" maxlength="10" class="form-control input-sm" placeholder="Search Serial">
              <span class="input-group-addon"><span class="glyphicon glyphicon-search"></span></span>
            </div>
          </td>
          <td>
            <div class="input-group">
              <asp:DropDownList ID="Parentnode" ToolTip="Parent" CssClass="form-control input-sm" runat="server"></asp:DropDownList>
              <span class="input-group-addon"><i class="fa fa-arrows-h"></i></span>
            </div>
          </td>
          <td>
            <div class="input-group">
              <asp:DropDownList ID="childnode" ToolTip="child" CssClass="form-control input-sm" runat="server"></asp:DropDownList>
              <span class="input-group-addon"><i class="fa fa-arrows-h"></i></span>
            </div>
          </td>
        </tr>
        <tr>
          <td>
            <div class="input-group">
              <asp:DropDownList ID="Grandchild" ToolTip="Grandchild" CssClass="form-control input-sm" runat="server"></asp:DropDownList>
              <span class="input-group-addon">
                <i class="fa fa-arrows-h"></i>
              </span>
            </div>
          </td>
          <td>
            <div class="input-group">
              <asp:DropDownList ID="DropDownListAutoMeterType" ToolTip="Meter Type" CssClass="form-control input-sm" runat="server">
                <asp:ListItem Text="Single Phase" Selected="True" />
                <asp:ListItem Text="Three Phase" />
              </asp:DropDownList>
              <span class="input-group-addon">
                <i class="fa fa-arrows-h"></i>
              </span>
            </div>
          </td>
        </tr>
                                                                       <td style="min-width:200px">
  <asp:LinkButton ID="LinkButtonAutoRegSearch" runat="server" CssClass="btn btn-danger black-background white btn-sm" ToolTip="Search" OnClick="LinkButtonAutoRegSearch_Click">
    <span aria-hidden="true" class="glyphicon glyphicon-search"></span> Search
  </asp:LinkButton>
  &nbsp;
  <asp:LinkButton ID="LinkButtonAddAutoMeter" runat="server" data-toggle="confirmation" data-popout="true" data-singleton="true" data-placement="bottom" CssClass="btn btn-success btn-sm" ToolTip="Add to Network" OnClick="LinkButtonAddAutoMeter_Click">
    <span aria-hidden="true" class="glyphicon glyphicon-log-in"></span> Add
  </asp:LinkButton>
</td>
<td>
  <asp:UpdateProgress runat="server" ID="UpdateProgress17">
    <ProgressTemplate>
      <i class="fa fa-circle-o-notch fa-lg fa-spin"></i>
    </ProgressTemplate>
  </asp:UpdateProgress>
</td>
</table>
<asp:UpdatePanel runat="server" ID="gridAutoRegUPanel" UpdateMode="Conditional">
  <Triggers>
    <asp:AsyncPostBackTrigger ControlID="GridViewAutoReg" EventName="PageIndexChanging" />
  </Triggers>
  <ContentTemplate>
    <asp:GridView ID="GridViewAutoReg" AutoGenerateColumns="true" GridLines="None" CssClass="table table-condensed" PagerStyle-CssClass="pgr" AutoGenerateSelectButton="true" AlternatingRowStyle-CssClass="alt" AllowPaging="true" PageSize="10" OnPageIndexChanging="GridViewAutoReg_PageIndexChanging" runat="server">
      <SelectedRowStyle BorderColor="Black" BorderWidth="2px" />
    </asp:GridView>
  </ContentTemplate>
</asp:UpdatePanel>
</div>



                                                    </div>
                   </div>
                <div id ="deviceGroup"  class="Device Groups" runat="server" style="display:none; width:100%;overflow:auto;">
                                           <div class="panel panel-default" style="border:none;">
                                               <div class="panel-body">
            <ol class="breadcrumb">
                <li id="deviceGroupsBCrumbCreate"><a href="javascript:dgcreateshow()">Create Group</a></li>
                <li id="deviceGroupsBCrumbEdit"><a href="javascript:dgeditshow()">Edit Group</a></li>
                <li id="deviceGroupsBCrumbRemove" class="active"><a href="javascript:dgremoveshow()">Group Smart Devices</a></li>
            </ol>
            <div id="dgcreator" class="panel panel-default" style="border:none;">
                <div class="panel-heading">Create Group</div>
                <div class="panel-body">
                    <div class="panel panel-default">
                        <div class="panel-heading">Customer Groups
                            <asp:UpdateProgress runat="server" ID="UpdateProgress37">
                                <ProgressTemplate>
                                    <i class="fa fa-circle-o-notch fa-lg fa-spin"></i>
                                </ProgressTemplate>
                            </asp:UpdateProgress>
                        </div>
                        <div class="panel-body">
                            <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <table class="table">
                                        <tr>
                                            <td>
                                                <asp:DropDownList ID="groupType" CssClass="form-control" runat="server">
                                                    <asp:ListItem Text="Factory"></asp:ListItem>
                                                    <asp:ListItem Text="Departments"></asp:ListItem>
                                                    <asp:ListItem Text="Machine"></asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="deviceGroupNameTextBox" CssClass="form-control" runat="server" MaxLength="50" placeholder="Group Name"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:Button ID="deviceGroupCreateButton" data-toggle="confirmation" data-popout="true" data-singleton="true" runat="server" Text="Create" OnClick="deviceGroupCreateButton_Click" CssClass="btn btn-success" />
                                            </td>
                                        </tr>
                                    </table>
                                                <asp:Label ID="Label2"  runat="server"></asp:Label>
                                                <asp:GridView ID="GridView2" CssClass="table table-striped table-bordered table-responsive"  runat="server" AutoGenerateColumns = "false" Font-Names = "Arial" ShowHeaderWhenEmpty="true" Font-Size = "11pt" AlternatingRowStyle-BackColor = "#C2D69B"  AllowPaging ="true"  ShowFooter = "true" OnPageIndexChanging="GridView2_PageIndexChanging" onrowediting="GridView2_RowEditing" onrowupdating="GridView2_RowUpdating"  onrowcancelingedit="GridView2_RowCancelingEdit" PageSize = "10" >
                                                    <Columns>
                                                        <asp:TemplateField   HeaderText = "ID">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblid"  runat="server" Text='<%# Eval("id")%>'></asp:Label>

                                                            </ItemTemplate>
                                                            <FooterTemplate>
                                                                <asp:TextBox ID="txtid" MaxLength = "50" CssClass="form-control"  runat="server"></asp:TextBox>

                                                            </FooterTemplate>

                                                        </asp:TemplateField>
                                                        <asp:TemplateField   HeaderText = "areaID">
                                                             <ItemTemplate>
                                                                 <asp:Label ID="lblareaID"  runat="server" Text='<%# Eval("areaID")%>'></asp:Label>

                                                             </ItemTemplate>
                                                            <EditItemTemplate>
                                                                <asp:TextBox ID="txtareaID"  runat="server" CssClass="form-control" Text='<%# Eval("areaID")%>'></asp:TextBox>

                                                            </EditItemTemplate>
                                                            <FooterTemplate>
                                                                <asp:TextBox ID="txtareaID" CssClass="form-control"  runat="server"></asp:TextBox>

                                                            </FooterTemplate>

                                                        </asp:TemplateField>
                                                        <asp:TemplateField   HeaderText = "category">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblcategory"  runat="server" Text='<%# Eval("category")%>'></asp:Label>

                                                            </ItemTemplate>
                                                            <EditItemTemplate>
                                                                <asp:TextBox ID="txtcategory" Enabled="false"  runat="server" CssClass="form-control" Text='<%# Eval("category")%>'></asp:TextBox>

                                                            </EditItemTemplate>
                                                            <FooterTemplate>
                                                                <asp:TextBox ID="txtcategory" Enabled="false" CssClass="form-control"  runat="server"></asp:TextBox>

                                                            </FooterTemplate>

                                                        </asp:TemplateField>
                                                        <asp:TemplateField   HeaderText = "parentNode">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblparentNode"  runat="server" Text='<%# Eval("parentNode")%>'></asp:Label>

                                                            </ItemTemplate>
                                                            <EditItemTemplate>
                                                                <asp:TextBox ID="txtparentNode" Enabled="false"  runat="server" CssClass="form-control" Text='<%# Eval("parentNode")%>'></asp:TextBox>

                                                            </EditItemTemplate>
                                                            <FooterTemplate>
                                                                <asp:TextBox ID="txtparentNode" Enabled="false" CssClass="form-control"  runat="server"></asp:TextBox>

                                                            </FooterTemplate>

                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="lnkRemovec"  runat="server" CommandArgument = '<%# Eval("id")%>' CssClass="btn btn-danger" OnClientClick = "return confirm('Do you want to delete?')" Text = "Delete" OnClick ="lnkRemovec_Click">
                                                                    <span aria-hidden="true" class="glyphicon glyphicon-remove-sign"></span>

                                                                </asp:LinkButton>

                                                            </ItemTemplate>
                                                            <FooterTemplate>
                                                                <asp:Button ID="btnAddc"  runat="server" Text="Add" CssClass="btn btn-primary btn-sm" OnClick ="btnAddc_Click" />

                                                            </FooterTemplate>

                                                        </asp:TemplateField>
                                                        <asp:CommandField  ShowEditButton="True" />

                                                    </Columns>
                                                    <AlternatingRowStyle BackColor="#C2D69B"  />

                                                </asp:GridView>

                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID = "GridView2" />

                                            </Triggers>

                                        </asp:UpdatePanel>

                                    </div>

                                </div>

                            </div>

                        </div>
                        <div id="dgeditGroup">
          <div class="panel panel-default" style="border:none;">
  <div class="panel-heading">Edit Group</div>
  <div class="panel-body">
      <table class="table table-condensed">
          <tr>
              <td>
                                                                                        <div class="input-group">
  <input type="text" id="TextBoxSearchGroups" title="Password"  runat="server" maxlength="30" class="form-control" placeholder="Search">
  <span class="input-group-addon"><i class="fa fa-search"></i></span>
</div>
              </td>
              <td>
                  <div class="input-group">
                      <asp:DropDownList ID="DropDownGroupSearch" CssClass="form-control"  runat="server">
                          <asp:ListItem Text="Factory"></asp:ListItem>
                          <asp:ListItem Text="Department"></asp:ListItem>
                          <asp:ListItem Text="Machine"></asp:ListItem>
                          
</asp:DropDownList>
  <span class="input-group-addon"><i class="fa fa-search"></i></span>
</div>
              </td>
              <td>
        <asp:LinkButton ID="LinkButtonGroupSearch"  runat="server" CssClass="btn btn-primary" ToolTip="Search" OnClick="LinkButtonGroupSearch_Click" >
            <span aria-hidden="true" class="fa fa-search"></span> Search

        </asp:LinkButton>
              </td>
              <td>
                          <asp:LinkButton ID="LinkButtonEditGroup"  runat="server" CssClass="btn btn-success" ToolTip="Edit" OnClick="LinkButtonEditGroup_Click" >
    <span aria-hidden="true" class="fa fa-edit"></span> Edit
</asp:LinkButton>
              </td>
            <td>
                 <asp:UpdateProgress  runat="server" ID="UpdateProgress19">
                    <ProgressTemplate>
                        <i class="fa fa-circle-o-notch fa-lg fa-spin"></i>
                    </ProgressTemplate>
                </asp:UpdateProgress>
            </td>
          </tr>
      </table>
      <div id="EditWell" class="well well-sm">
          <table>
              <tr>
                  <td>
                      <div class="input-group">
  <input type="text" id="editGroupFieldName" title="Group Name" disabled  runat="server" maxlength="30" class="form-control input-sm" placeholder="Rename Group">
  <span class="input-group-addon"><i class="fa fa-edit"></i></span>
</div>
                  </td>
                  <td>&nbsp</td>
                 
                  
                  <td>
           
            <asp:UpdatePanel ID="upPanel" UpdateMode="Conditional"  runat="server">
                <ContentTemplate>
                                           <div class="input-group">
<asp:DropDownList ID="DropDownListParentNode" CssClass="form-control input-sm"  runat="server">
</asp:DropDownList>
  <span class="input-group-addon"><i class="fa fa-edit"></i></span>
</div>
                </ContentTemplate>
            </asp:UpdatePanel>
                  </td>
                  <td>&nbsp</td>
                  <td>
                                                <asp:LinkButton ID="LinkButtonEditGroupSave"  runat="server" data-toggle="confirmation" data-popout="true" data-singleton="true" CssClass="btn btn-success" ToolTip="Cancel" OnClick="LinkButtonEditGroupSave_Click" >
                                                    <span aria-hidden="true" class="fa fa-save"></span> Save

                                                </asp:LinkButton>
                  </td>
                      <td>&nbsp</td>
                  <td>
                                                <asp:LinkButton ID="LinkButtonEditGroupCancel"  runat="server" 
                CssClass="btn btn-danger" ToolTip="Cancel"  
                OnClick="LinkButtonEditGroupCancel_Click" >
    <span aria-hidden="true" class="fa fa-crosshairs"></span> Cancel
</asp:LinkButton>
                  </td>
                  <td>&nbsp</td>
                  <td>
              <asp:LinkButton ID="LinkButtonRefreshParents"  runat="server" 
                CssClass="btn btn-primary" ToolTip="Refresh"  
                OnClick="LinkButtonRefreshParents_Click" >
    <span aria-hidden="true" class="fa fa-refresh"></span>
</asp:LinkButton>
                  </td>
              </tr>
          </table>
      </div>
      <asp:UpdatePanel ID="groupgridupanel" UpdateMode="Conditional"  runat="server">
          <ContentTemplate>
                                      <asp:gridview id="GridViewOfTheGroups" 
                        autogeneratecolumns="true"
                         GridLines="None"  AutoGenerateSelectButton="true" AutoGenerateDeleteButton="true"
                         CssClass="table table-condensed"    
                            PagerStyle-CssClass="pgr" Font-Size="Small"
                            AlternatingRowStyle-CssClass="alt" OnRowDeleting="GridViewOfTheGroups_RowDeleting"
                        AllowPaging="true" PageSize="10" OnPageIndexChanging="GridViewOfTheGroups_PageIndexChanging"
                         runat="server">
                           <SelectedRowStyle  BorderColor="SkyBlue" BorderWidth="2px"/>
                </asp:gridview>
          </ContentTemplate>
      </asp:UpdatePanel>
      </div>
                                                    </div>
      </div>
                        <div id="dgremover" class="panel panel-default" style="border:none;">
  <div class="panel-heading">Group Allocation</div>
  <div class="panel-body">
      <asp:UpdatePanel ID="alloupdate"  runat="server" UpdateMode="Conditional">
          <ContentTemplate>

      <table class="table">
          <tr>
              <td>
                    <div class="input-group">
                        <asp:DropDownList ID="DropDownListsubutility" CssClass="form-control"  runat="server">
                        </asp:DropDownList>
                            <span class="input-group-addon"><i class="fa fa-group"></i> Department</span>
                        </div>
              </td>
             <td>&nbsp</td>
              <td>
                  <div class="input-group">
                      <asp:DropDownList ID="DropDownListMachine" CssClass="form-control"  runat="server">

                      </asp:DropDownList>
                      <span class="input-group-addon"><i class="fa fa-group"></i> Machine</span>

                  </div>
              </td>
          </tr>
              
 
              </table>
              
          </ContentTemplate>
      </asp:UpdatePanel>
      <table>
          <tr>
              <td>
                  <asp:LinkButton ID="SearchForGroupsAlloLinkButton"  runat="server"  CssClass="btn btn-default" ToolTip="Refresh"  
                OnClick="SearchForGroupsAlloLinkButton_Click" >
    <span aria-hidden="true" class="fa fa-refresh"></span>
</asp:LinkButton>
              </td>
              <td>&nbsp</td>
              <td>
                  <asp:LinkButton ID="LinkButtonGroupsAlloToMeter"  runat="server" data-toggle="confirmation" data-popout="true" data-singleton="true"  CssClass="btn btn-default" ToolTip="Refresh"  
                OnClick="LinkButtonGroupsAlloToMeter_Click" >
    <span aria-hidden="true" class="fa fa-database"></span> Allocate
</asp:LinkButton>
              </td>
<td>
                 <asp:UpdateProgress  runat="server" ID="UpdateProgress20">
                    <ProgressTemplate>
                        <i class="fa fa-circle-o-notch fa-lg fa-spin"></i>
                    </ProgressTemplate>
                </asp:UpdateProgress>
</td>
          </tr>
      </table>

      </div>

                                                </div>

                    </div>

  
                                               </div>
                    </div>

                    
                    <div id ="stats"  class="Graphs" style="display:none; width:100%;overflow:auto;" runat="server">
                       <div class="panel panel-default" style="border:none;">
  <div class="panel-body">
      <table class="table table-condensed">
          <tr>
              <td>
                 <div style="width:100%;" class="input-group date form_datetime col-md-5" data-date="2016-04-16T05:25:07Z" data-date-format="yyyy/m/d HH:ii:ss P" data-link-field="dtp_input1">
                    <input id="rootG1" runat="server" placeholder="Start Range" class="form-control input-sm" size="16" type="text" value="" readonly>
                    <span class="input-group-addon"><span class="glyphicon glyphicon-remove"></span></span>
					<span class="input-group-addon"><span class="glyphicon glyphicon-th"></span></span>
                </div>
              </td>
              <td>
                  <div style="width:100%;" class="input-group date form_datetime col-md-5" data-date="2016-04-16T05:25:07Z" data-date-format="yyyy/m/d HH:ii:ss P" data-link-field="dtp_input1">
                    <input id="rootG2" runat="server" placeholder="End Range" class="form-control input-sm" size="16" type="text" value="" readonly>
                    <span class="input-group-addon"><span class="glyphicon glyphicon-remove"></span></span>
					<span class="input-group-addon"><span class="glyphicon glyphicon-th"></span></span>
                </div>
              </td>
              </tr>
              <tr>
                  <td>
      <asp:DropDownList ID="GraphQuanityType" CssClass="form-control input-sm" runat="server">
          <asp:ListItem Text="Cumulative Active Energy Absolute"/>
          <asp:ListItem Text="Cumulative Reactive Energy Absolute"/>
          <asp:ListItem Text="Maximum Demand Active Absolute"/>
          <asp:ListItem Text="Cumulative Maximum Demand Active Absolute"/>
          <asp:ListItem Text="Voltage"/>
          <asp:ListItem Text="Current"/>
          <asp:ListItem Text="Average Power Factor"/>
      </asp:DropDownList>
                      </td>
                  <td>

                                                  <asp:LinkButton ID="LinkButtonStatGraphGen" 
                runat="server" 
                CssClass="btn btn-primary btn-sm" ToolTip="Graph"  
                OnClick="LinkButtonStatGraphGen_Click" >
    <span aria-hidden="true" class="glyphicon glyphicon-stats"></span> Graph
</asp:LinkButton>

                  </td>
                  <td>
                 <asp:UpdateProgress runat="server" ID="UpdateProgress21">
                    <ProgressTemplate>
                        <i class="fa fa-circle-o-notch fa-lg fa-spin"></i>
                    </ProgressTemplate>
                </asp:UpdateProgress>
                  </td>
          </tr>
      </table>
                              <div class="panel panel-default">
                                  <div class="panel-heading">Graph</div>
                                  <div class="panel-body">
                                 <div id="containerStatGraph" style="height: 400px; max-height: 400px;"></div>

                                  </div>
                          </div>
  </div>
</div>
                   </div>
                    <div id ="fileexp"  class="Export .csv" runat="server" style="display:none; width:100%;overflow-x:auto;overflow-y:auto;" >
                       <div class="panel panel-default" style="border:none;">
  <div class="panel-body">
      <div class="panel panel-default">
  <div class="panel-heading">Report Parameters</div>
  <div class="panel-body">
      <table class="table table-condensed">
          <tr>
              <th>
Report Type 
              </th>
              <th>
Latest Reading Upto
              </th>
              <th>
                  Download
              </th>
          </tr>
          <tr>
              <td>
                  <asp:DropDownList ID="csvDDL" CssClass="form-control input-sm" runat="server">
                      <asp:ListItem Text="Instantaneous" />
                      <asp:ListItem Text="Billing" />
                              </asp:DropDownList>
              </td>
              <td>
         <div style="width:240px;" class="input-group date form_datetime col-md-5" data-date="2016-04-16T05:25:07Z" data-date-format="yyyy/m/d HH:ii:ss P" data-link-field="dtp_input1">
                    <input id="csvSingleDT" runat="server" class="form-control input-sm" size="16" type="text" value="" readonly>
                    <span class="input-group-addon"><span class="glyphicon glyphicon-remove"></span></span>
					<span class="input-group-addon"><span class="glyphicon glyphicon-th"></span></span>
                </div>
              </td>
              <td>
                    <asp:LinkButton ID="LinkButtonCSVDownload" 
                runat="server" 
                CssClass="btn btn-success btn-sm" data-toggle="tooltip" ToolTip="Download Report"  
                OnClick="LinkButtonCSVDownload_Click" >
    <span aria-hidden="true" class="glyphicon glyphicon-download-alt"></span> .csv
</asp:LinkButton>
              </td>
          </tr>
      </table>
  </div>
</div>
<div class="container-fluid">
            <div class="row">
                <div class="col-md-4">
                        <div class="thumbnail">
        <img src="reports.png" style="width:100%; height:100%;" />
      <div class="caption">
          <div class="popover-markup"><a href="#" class="brigger">Custom Reports</a><div class="head hide"><i class="fa fa-info-circle"></i> Info</div><div class="content hide"><p>Launch Custom Reports Dialogue</p></div></div>
        <p>Reports with user customizable parameters. </p>
        <a id="b1" class="btn btn-primary btn-sm"  data-toggle="modal" data-target="#crmodal"> Open</a>
      </div>
    </div>
                </div>
                <div class="col-md-4">
                        <div class="thumbnail">
        <img src="reports.png" style="width:100%; height:100%;" />
      <div class="caption">
          <div class="popover-markup"><a href="#" class="brigger">System Reports</a><div class="head hide"><i class="fa fa-info-circle"></i> Info</div><div class="content hide"><p>Lauch system reports Dialogue</p></div></div>
        <p>Critical System reports.</p>
        <a id="b2" class="btn btn-primary btn-sm"  data-toggle="modal" data-target="#cremodal"> Open</a>
      </div>
    </div>
                </div>
                <div class="col-md-4">
                        <div class="thumbnail">
        <img src="reports.png" style="width:100%; height:100%;" />
      <div class="caption">
          <div class="popover-markup"><a href="#" class="brigger">Advanced Reports</a><div class="head hide"><i class="fa fa-info-circle"></i> Info</div><div class="content hide"><p>Launch advanced reports Dialogue</p></div></div>
        <p>Advanced meter data reports.</p>
        <a class="btn btn-primary btn-sm"  data-toggle="modal" data-target="#dialogEventReport"> Open</a>
      </div>
    </div>
                </div>
                </div>
    </div>
            <div class="panel panel-default">
  <div class="panel-heading">Batch Billing Report Upload Configuration
                       <asp:UpdateProgress runat="server" ID="UpdateProgress22">
                    <ProgressTemplate>
                        <i class="fa fa-circle-o-notch fa-lg fa-spin"></i>
                    </ProgressTemplate>
                </asp:UpdateProgress>
  </div>
  <div class="panel-body">
<div id = "dvGrid">
<asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
<ContentTemplate>
    <asp:Label ID="seee" runat="server"></asp:Label>
<asp:GridView ID="GridView1" CssClass="table table-striped table-bordered table-responsive" runat="server"
AutoGenerateColumns = "false" Font-Names = "Arial" ShowHeaderWhenEmpty="true"
Font-Size = "11pt" AlternatingRowStyle-BackColor = "#C2D69B"  AllowPaging ="true"  ShowFooter = "true" 
OnPageIndexChanging ="GridView1_PageIndexChanging" onrowediting="GridView1_RowEditing"
onrowupdating="GridView1_RowUpdating"  onrowcancelingedit="GridView1_RowCancelingEdit"
PageSize = "10" >
<Columns>
<asp:TemplateField   HeaderText = "BatchID">
    <ItemTemplate>
        <asp:Label ID="lblBatchID" runat="server"
        Text='<%# Eval("BatchID")%>'></asp:Label>
    </ItemTemplate>
    <FooterTemplate>
        <asp:TextBox ID="txtBatchID"
            MaxLength = "20" CssClass="form-control" runat="server"></asp:TextBox>
    </FooterTemplate>
</asp:TemplateField>
<asp:TemplateField   HeaderText = "FTPPath">
    <ItemTemplate>
        <asp:Label ID="lblFTPPath" runat="server"
                Text='<%# Eval("FTPPath")%>'></asp:Label>
    </ItemTemplate>
    <EditItemTemplate>
        <asp:TextBox ID="txtFTPPath" runat="server" CssClass="form-control"
            Text='<%# Eval("FTPPath")%>'></asp:TextBox>
    </EditItemTemplate> 
    <FooterTemplate>
        <asp:TextBox ID="txtFTPPath" CssClass="form-control" runat="server"></asp:TextBox>
    </FooterTemplate>
</asp:TemplateField>
<asp:TemplateField   HeaderText = "Username">
    <ItemTemplate>
        <asp:Label ID="lblUsername" runat="server"
            Text='<%# Eval("Username")%>'></asp:Label>
    </ItemTemplate>
    <EditItemTemplate>
        <asp:TextBox ID="txtUsername" runat="server" CssClass="form-control"
            Text='<%# Eval("Username")%>'></asp:TextBox>
    </EditItemTemplate> 
    <FooterTemplate>
        <asp:TextBox ID="txtUsername" CssClass="form-control" runat="server"></asp:TextBox>
    </FooterTemplate>
</asp:TemplateField>
<asp:TemplateField   HeaderText = "Password">
    <ItemTemplate>
        <asp:Label ID="lblPassword" runat="server"
                Text='<%# Eval("Password")%>'></asp:Label>
    </ItemTemplate>
    <EditItemTemplate>
        <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control"
            Text='<%# Eval("Password")%>'></asp:TextBox>
    </EditItemTemplate> 
    <FooterTemplate>
        <asp:TextBox ID="txtPassword" CssClass="form-control" runat="server"></asp:TextBox>
    </FooterTemplate>
</asp:TemplateField>
<asp:TemplateField   HeaderText = "UploadTime">
    <ItemTemplate>
        <asp:Label ID="lblUploadTime" runat="server"
                Text='<%# Eval("UploadTime")%>'></asp:Label>
    </ItemTemplate>
    <EditItemTemplate>
        <asp:TextBox ID="txtUploadTime" runat="server" CssClass="form-control"
            Text='<%# Eval("UploadTime")%>'></asp:TextBox>
    </EditItemTemplate> 
    <FooterTemplate>
        <asp:TextBox ID="txtUploadTime" CssClass="form-control" runat="server"></asp:TextBox>
    </FooterTemplate>
</asp:TemplateField>
<asp:TemplateField   HeaderText = "UploadFrequency">
    <ItemTemplate>
        <asp:Label ID="lblUploadFrequency" runat="server"
                Text='<%# Eval("UploadFrequency")%>'></asp:Label>
    </ItemTemplate>
    <EditItemTemplate>
        <asp:TextBox ID="txtUploadFrequency" runat="server" CssClass="form-control"
            Text='<%# Eval("UploadFrequency")%>'></asp:TextBox>
    </EditItemTemplate> 
    <FooterTemplate>
        <asp:TextBox ID="txtUploadFrequency" CssClass="form-control" runat="server"></asp:TextBox>
    </FooterTemplate>
</asp:TemplateField>
<asp:TemplateField>
    <ItemTemplate>
        <asp:LinkButton ID="lnkRemove" runat="server"
            CommandArgument = '<%# Eval("BatchID")%>' CssClass="btn btn-danger"
         OnClientClick = "return confirm('Do you want to delete?')"
        Text = "Delete" OnClick ="lnkRemove_Click">
    <span aria-hidden="true" class="glyphicon glyphicon-remove-sign"></span>
        </asp:LinkButton>
    </ItemTemplate>
    <FooterTemplate>
        <asp:Button ID="btnAdd" runat="server" Text="Add" CssClass="btn btn-primary btn-sm"
            OnClick ="btnAdd_Click" />
    </FooterTemplate>
</asp:TemplateField>
<asp:CommandField  ShowEditButton="True" />
</Columns>
<AlternatingRowStyle BackColor="#C2D69B"  />
</asp:GridView>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID = "GridView1" />
</Triggers>
</asp:UpdatePanel>
</div>
      <table class="table table-condensed">
          <tr>
              <th>Set Meter Batch</th>
              <th>
                 <asp:UpdateProgress runat="server" ID="UpdateProgress23">
                    <ProgressTemplate>
                        <i class="fa fa-circle-o-notch fa-lg fa-spin"></i>
                    </ProgressTemplate>
                </asp:UpdateProgress>
              </th>
          </tr>
          <tr>
              <td>
                  <asp:UpdatePanel UpdateMode="Conditional" runat="server">
                      <Triggers>
<asp:AsyncPostBackTrigger ControlID = "GridView1" />
                      </Triggers>
                      <ContentTemplate>
                                          <div class="input-group">
                  <asp:DropDownList ID="batches" CssClass="form-control input-sm" runat="server"></asp:DropDownList>
  <span class="input-group-addon"><span class="glyphicon glyphicon-search"></span></span>
</div>
                      </ContentTemplate>
                  </asp:UpdatePanel>
              </td>
              <td>                    
                  <asp:LinkButton ID="ftpLinkButtonGenerateBatchFiles" 
                runat="server" OnClick="ftpLinkButtonGenerateBatchFiles_Click"
                CssClass="btn btn-success btn-sm" data-toggle="confirmation" ToolTip="Add Device to Batch"   >
    <span aria-hidden="true" class="glyphicon glyphicon-plus-sign"></span> Add
</asp:LinkButton>

              </td>
              <td>
                  <a href="#" class="btn btn-primary btn-sm" data-toggle="modal" data-target="#ftpFiles"><i class="fa fa-windows"></i> View Uploads</a>
              </td>
              <td>
                 <asp:UpdateProgress runat="server" ID="UpdateProgress24">
                    <ProgressTemplate>
                        <i class="fa fa-circle-o-notch fa-lg fa-spin"></i>
                    </ProgressTemplate>
                </asp:UpdateProgress>
              </td>
          </tr>
      </table>
      </div>
                </div>
  </div>
</div>
                   </div>
                    <div id ="instRead"  class="Quick Read" runat="server" style="display:none; width:100%;overflow-x:auto;overflow-y:auto;">
               <div class="panel panel-default" style="border:none;">
  <div class="panel-body">

<table class="table table-condensed">
  <tr>
    <th>Report</th>
      <th>
          Read
      </th>
      <th>Progress</th>
  </tr>
  <tr>
      <td>
  <asp:LinkButton ID="reportLink" 
                runat="server" 
                CssClass="btn btn-danger btn-sm" data-toggle="tooltip" ToolTip="Download PDF"  
                OnClick="reportLink_Click" >
    <span aria-hidden="true" class="glyphicon glyphicon-download-alt"></span> PDF
</asp:LinkButton>
      </td>
      <td>
            <asp:UpdatePanel runat="server" id="TimedPanel" updatemode="Conditional">
            <Triggers>
                <asp:AsyncPostBackTrigger controlid="Timer1" eventname="Tick" />
            </Triggers>
            <ContentTemplate>
                 <asp:Timer runat="server" id="Timer1" interval="5000" ontick="Timer1_Tick" Enabled="false" />
              <asp:LinkButton ID="LinkButtonRead" 
                runat="server" 
                CssClass="btn btn-default btn-sm" data-toggle="tooltip" ToolTip="Read"  
                OnClick="LinkButtonRead_Click" >
    <span aria-hidden="true" class="glyphicon glyphicon-log-in"></span> Read
</asp:LinkButton>
               </ContentTemplate>
            </asp:UpdatePanel>
      </td>
          <td>
              <div style="width:140px"><img src="new-special-progbar.gif" id="loading" /></div>
      </td>
    <td>
                 <asp:UpdateProgress runat="server" ID="UpdateProgress25">
                    <ProgressTemplate>
                        <i class="fa fa-circle-o-notch fa-lg fa-spin"></i>
                    </ProgressTemplate>
                </asp:UpdateProgress>
    </td>
  </tr>
</table>


<section class="content">
 
  <div class="row">
      <div class="col-md-4 col-sm-12 col-xs-12">
      <div class="box" style="border-radius:2px;">
        <div class="box-body" style="padding:7px 20px 7px 10px;">
          <i class="radius-icon2 fa bg-yellow  fa-tasks"></i>
          <div class="radius-info" style="float:left;margin-top:5px;"></div>
            <h4 class="radius-info-subheading font12">
              Frequency:<p id="pfrequency"></p>
              <span style="color: #333333;"></span>
            </h4>
          
        </div>
      </div>
    </div>
    
    <div class="col-md-4 col-sm-12 col-xs-12">
      <div class="box" style="border-radius:2px;">
        <div class="box-body" style="padding:7px 20px 7px 10px;">
          <i class="radius-icon2 fa  bg-blue  fa-battery-three-quarters" style="background: #00796B  !important;"></i>
          <div class="radius-info" style="float:left;margin-top:5px;"></div>
            <h4 class="radius-info-subheading font12">
              Total Active Electric Quantity:<p id="pkWh"></p>
              <span style="color: #333333;"></span>
            </h4>
          
        </div>
      </div>
    </div>
    <div class="col-md-4 col-sm-12 col-xs-12">
      <div class="box" style="border-radius:2px;">
        <div class="box-body" style="padding:7px 20px 7px 10px;">
          <i class="radius-icon2 fa bg-blue fa-battery-0" style="background: #FF5722 !important;"></i>
          <div class="radius-info" style="margin-top:5px;"></div>
            <h4 class="radius-info-subheading font12">
              Total Reactive Power:<p id="pkVARh"></p>
              <span style="color: #333333;"></span>
            </h4>
          
        </div>
      </div>
    </div>
    <div class="col-md-4 col-sm-12 col-xs-12">
      <div class="box" style="border-radius:2px;">
        <div class="box-body" style="padding:7px 20px 7px 10px;">
          <i class="radius-icon2 fa  bg-green  fa-clock-o"></i>
          <div class="radius-info" style="float:left;margin-top:5px;"></div>
            <h4 class="radius-info-subheading font12">
              Time Of Reading:<p id="pTime"></p>
              <span style="color: #333333;"></span>
            </h4>
          
        </div>
      </div>
    </div>
    <!---->
    <div class="clearfix"></div>
  </div>
  <!----><div class="row ng-star-inserted">
    <div class="col-md-12 col-sm-12 col-xs-12">
      Please select the corresponding sensor device from the device tree on the right!
    </div>
  </div>
  
  
  
  
  <!---->
  
  
  <!---->
  
  <!---->
  <!---->
  
  
</section>
      
                        </div>
                    </div>
                   </div>
                    <div id ="usermgmt"  class="User Management" runat="server" style="display:none; min-height:300px; width:100%;overflow-x:auto;overflow-y:auto;">
                                           <div class="panel panel-default" style="border:none;">
  <div class="panel-body">
      <table class="table table-condensed">
          <tr>
              <td>
                  <table style="width:100%;">
                      <tr>
                          <td>
                                          <div class="input-group">
  <input type="text" id="searchBoxUsers" title="Search" runat="server" maxlength="20" class="form-control input-sm" placeholder="Search">
  <span class="input-group-addon"><span class="glyphicon glyphicon-search"></span></span>
</div>
                          </td>
                          <td>&nbsp</td>
                          <td>
                                 <asp:UpdatePanel runat="server" ID="UserSearchUPanel" UpdateMode="Conditional">
                  <ContentTemplate>
                  <!-- Split button -->
                      <div style="width:100px;">
<div class="btn-group">
  <asp:Button ID="btnsearchUser" Text="Search" CssClass="btn btn-primary btn-sm" runat="server" OnClick="btnsearchUser_Click" />
  <button type="button" class="btn btn-primary dropdown-toggle btn-sm" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
    <span class="caret"></span>
    <span class="sr-only">Toggle Dropdown</span>
  </button>
  <ul class="dropdown-menu">
    <li><p><asp:button ID="searchByUsername" runat="server" style="background-color:transparent; border:none;" OnClick="searchByUsername_Click" Text="By Username" /></p></li>
    <li><p><asp:button ID="searchByName" runat="server" style="background-color:transparent; border:none;" OnClick="searchByName_Click" Text="By Name" /></p></li>
    <li><p><asp:button ID="searchByContact" runat="server" style="background-color:transparent; border:none;" OnClick="searchByContact_Click" Text="By Contact" /></p></li>
    <li><p><asp:button ID="searchByCNIC" runat="server" style="background-color:transparent; border:none;" OnClick="searchByCNIC_Click" Text="C.N.I.C" /></p></li>
  </ul>
</div>
                      </div>
                                    </ContentTemplate>
              </asp:UpdatePanel>
                          </td>
                      </tr>
                  </table>
              </td>
               <td>
                  <a class="btn btn-success btn-sm" data-toggle="modal" data-target="#modalUserManagement"><i class="fa fa-user-plus" aria-hidden="true"></i> Add</a>
              </td>
              <td>
                  <a class="btn btn-danger btn-sm" title="Remove User" data-toggle="modal" data-target="#myModalUserRemove"><i class="fa fa-user-times" aria-hidden="true"></i></a>
              </td>
              <td>
                  <a class="btn btn-primary btn-sm" title="Manage Permissions" data-toggle="modal" data-target="#modalManageUserPermissions"><i class="fa fa-edit" aria-hidden="true"></i></a>
              </td>
              <td>
                 <asp:UpdateProgress runat="server" ID="UpdateProgress26">
                    <ProgressTemplate>
                        <i class="fa fa-circle-o-notch fa-lg fa-spin"></i>
                    </ProgressTemplate>
                </asp:UpdateProgress>
              </td>
          </tr>
      </table>     
      <asp:UpdatePanel runat="server" ID="UPanelUsers" UpdateMode="Conditional">
          <Triggers>
              <asp:AsyncPostBackTrigger ControlID="GridviewUserlist" EventName="PageIndexChanging" />
          </Triggers>
          <ContentTemplate>
                            <asp:GridView ID="GridviewUserlist"
            autogeneratecolumns="true"
                         GridLines="None"  
                         CssClass="table table-condensed"
                         AutoGenerateSelectButton="true" 
                            PagerStyle-CssClass="pgr"  
                            AlternatingRowStyle-CssClass="alt" OnSelectedIndexChanged="GridviewUserlist_SelectedIndexChanged"
                       AllowPaging="true" PageSize="10" OnPageIndexChanging="GridviewUserlist_PageIndexChanging"
                        runat="server">
                           <SelectedRowStyle  BorderColor="SkyBlue" BorderWidth="2px"/>
            </asp:GridView>
          </ContentTemplate>
      </asp:UpdatePanel>
          <asp:TextBox ID="selecteduser" runat="server" Visible="false" />
    <asp:TextBox ID="selectedusertype" runat="server" Visible="false" />
  </div>
                                               </div>                 
  </div>
                    
                   <div id ="acclog"  class="Access Log" runat="server" style="display:none;width:100%;overflow-x:auto;overflow-y:auto;">
                       <div class="panel panel-default" style="border:none;">
  <div class="panel-body">    

         <table>
          <tr>
              <td>
      (Use Ctrl + F to search for specfic entries)&nbsp<asp:Button ID="btnaccessT" CssClass="btn btn-default btn-sm" Text="Load..." runat="server" OnClick="btnaccessT_Click" />
              </td>
              <td>
                  &nbsp
              </td>
              <td>
                  
            <asp:UpdateProgress runat="server" ID="UpdateProgress1">
                    <ProgressTemplate>
                        <i class="fa fa-circle-o-notch fa-lg fa-spin"></i>
                    </ProgressTemplate>
                </asp:UpdateProgress>
              </td>
          </tr>
      </table>
                                          <table id="accessT" class="display compact table table-striped table-bordered  dt-responsive nowrap" style="width:100%; font-size:small">
        <thead>
            <tr>
                <th>Username</th>
                <th>Last Login</th>
                <th>Client IP</th>
            </tr>
        </thead>
        <tfoot>
            <tr>
                <th>Username</th>
                <th>Last Login</th>
                <th>Client IP</th>
            </tr>
        </tfoot>
    </table>
      </div>
</div> 
                   </div>
                   <div id ="actlog"  class="Actions Log" runat="server" style="display:none;width:100%;overflow-x:auto;overflow-y:auto;">
                       <div class="panel panel-default" style="border:none;">
  <div class="panel-body">  

             <table>
          <tr>
              <td>
      (Use Ctrl + F to search for specfic entries)&nbsp<asp:Button ID="btnactionT" CssClass="btn btn-default btn-sm" Text="Load..." runat="server" OnClick="btnactionT_Click" />   
              </td>
              <td>
                  &nbsp
              </td>
              <td>
                  
            <asp:UpdateProgress runat="server" ID="UpdateProgress2">
                    <ProgressTemplate>
                        <i class="fa fa-circle-o-notch fa-lg fa-spin"></i>
                    </ProgressTemplate>
                </asp:UpdateProgress>
              </td>
          </tr>
      </table>
                                          <table id="actionT" class="display compact table table-striped table-bordered  dt-responsive nowrap" style="width:100%; font-size:small">
        <thead>
            <tr>
                   <th>User Name</th>
                <th>Activity</th>
                <th>Meter Number</th>
                <th>Time stamp</th>
            </tr>
        </thead>
        <tfoot>
            <tr>
                     <th>User Name</th>
                <th>Activity</th>
                <th>Meter Number</th>
                <th>Time stamp</th>
            </tr>
        </tfoot>
    </table>
      </div>
</div>   
                   </div>
                    <div id ="commLog"  class="Comm Log" runat="server" style="display:none;width:100%;overflow-x:auto;overflow-y:auto;">
<div class="panel panel-default" style="border:none;">
  <div class="panel-body">

                   <table>
          <tr>
              <td>
        (Use Ctrl + F to search for specfic entries)&nbsp<asp:Button ID="btncommT" CssClass="btn btn-default btn-sm" Text="Load..." runat="server" OnClick="btncommT_Click" /> 
              </td>
              <td>
                  &nbsp
              </td>
              <td>
                  
            <asp:UpdateProgress runat="server" ID="UpdateProgress3">
                    <ProgressTemplate>
                        <i class="fa fa-circle-o-notch fa-lg fa-spin"></i>
                    </ProgressTemplate>
                </asp:UpdateProgress>
              </td>
          </tr>
      </table>
                                          <table id="commT" class="display compact table table-striped table-bordered  dt-responsive nowrap" style="width:100%; font-size:small">
        <thead>
            <tr>
               <th>Meter Serial</th>
                <th>Activity</th>
                <th>Packet</th>
                <th>Time Stamp</th>
            </tr>
        </thead>
        <tfoot>
            <tr>
                <th>Meter Serial</th>
                <th>Activity</th>
                <th>Packet</th>
                <th>Time Stamp</th>
            </tr>
        </tfoot>
    </table>
      </div>
</div> 
                   </div>
                    <div id ="offlineOnline"  class="Network" runat="server" style="display:none; width:100%;overflow:auto;">
                              <div class="panel panel-default" style="border:none;">
  <div class="panel-body">

      </div>
                                  </div>
                   </div>


                                    <div id="DivFotTotCon"  class="Consumption" runat="server" style="border:none;min-height:400px; display:none; overflow:hidden;">
                                                                               <div class="panel panel-default">
  <div class="panel-heading">
      Per Hour Network Power Consumption
      <span class="badge"><asp:UpdatePanel runat="server" ID="UpdatePanelPowerCon" UpdateMode="Conditional"><Triggers><asp:AsyncPostBackTrigger ControlID="btnSystemPower" EventName="Click" /></Triggers><ContentTemplate><asp:Button ID="btnSystemPower" OnClick="btnSystemPower_Click" style="background:none; border:none;" Text="reload" runat="server" /></ContentTemplate></asp:UpdatePanel></span>
  </div>
  <div class="panel-body">

<div id="containerTotCon"></div>
      </div>
</div>
                        </div>
                    

                      <div id ="LP"  class="Load Profile" runat="server" style="display:none;width:100%;  overflow:auto;">
                       <div class="panel panel-default" style="border:none;">
  <div class="panel-body">                        
                                  <table class="table table-condensed">
                                      <tr>
                                          <th>
                                              <b>Start Range</b>
                                          </th>
                                          <th>
                                              <b>End Range</b>
                                          </th>
                                          <th>Actions</th>
                                      </tr>
                                      <tr>
                                          <td>
                                             <br />
                        <div style="min-width:220px;" class="input-group date form_datetime col-md-5" data-date="2016-04-16T05:25:07Z" data-date-format="yyyy/m/d HH:ii:ss P" data-link-field="dtp_input1">
                    <input id="LPr1" runat="server" class="form-control input-sm" size="16" type="text" value="" readonly>
                    <span class="input-group-addon"><span class="glyphicon glyphicon-remove"></span></span>
					<span class="input-group-addon"><span class="glyphicon glyphicon-th"></span></span>
                </div>
                                          </td>
                                          <td>
                                              <br />
                              <div style="min-width:220px;" class="input-group date form_datetime col-md-5" data-date="2016-04-16T05:25:07Z" data-date-format="yyyy/m/d HH:ii:ss P" data-link-field="dtp_input1">
                    <input id="LPr2" runat="server" class="form-control input-sm" size="16" type="text" value="" readonly>
                    <span class="input-group-addon"><span class="glyphicon glyphicon-remove"></span></span>
					<span class="input-group-addon"><span class="glyphicon glyphicon-th"></span></span>
                </div>
                                          </td>
                                          <td style="width:200px;">
                   <asp:UpdatePanel ID="LPUpdatePanel" runat="server" UpdateMode="Conditional">
                              <Triggers>
                                  <asp:PostBackTrigger ControlID="SubmitBtn" />
                                  <asp:AsyncPostBackTrigger ControlID="btnLPRead" EventName="Click" />
                              </Triggers>
                              <ContentTemplate>
                                  <br />
                                  <div>
        <asp:Button ID="btnLPRead" runat="server" CssClass="btn btn-primary btn-sm" Text="Read" data-toggle="tooltip" title="Read Data" OnClick="btnLPRead_Click" />
    <asp:LinkButton ID="SubmitBtn" 
                runat="server" 
                CssClass="btn btn-danger btn-sm" data-toggle="tooltip" Enabled="false" ToolTip="Download PDF"  
                OnClick="SubmitBtn_Click" >
    <span aria-hidden="true" class="glyphicon glyphicon-download-alt"></span>PDF
</asp:LinkButton>
    <button type="button" data-toggle="modal" data-target="#dialogLPGraph" title="Load Profile Graph" class="btn btn-default btn-sm">
      <span class="glyphicon glyphicon-stats"></span>
    </button>
                                      </div>
                                           </ContentTemplate>
                          </asp:UpdatePanel>
                                          </td>
                                          <td>
                                                    <br />      <asp:UpdateProgress runat="server" ID="UpdateProgress4" AssociatedUpdatePanelID="LPUpdatePanel">
                    <ProgressTemplate>
                        <i class="fa fa-circle-o-notch fa-lg fa-spin"></i>
                    </ProgressTemplate>
                </asp:UpdateProgress>
                                          </td>
                                      </tr>
                                  </table>

                               <table id="exampleLP" class="display compact table table-striped table-bordered  dt-responsive nowrap" style="width:100%; font-size:small">
        <thead>
            <tr>
                <th>Meter Serial</th>
                <th>kWh</th>
                <th>kW</th>
                <th>kVARh</th>
                <th>kVAR</th>
                <th>Recording Time</th>
            </tr>
        </thead>
        <tfoot>
            <tr>
                <th>Meter Serial</th>
                <th>kWh</th>
                <th>kW</th>
                <th>kVARh</th>
                <th>kVAR</th>
                <th>Recording Time</th>
            </tr>
        </tfoot>
    </table>

                          <asp:Label ID="erlabel" runat="server" />
      </div>
</div>
                      </div>
                      
</div>
                          </td>
                      </tr>
                  </table>
        </div>
        <!-- /#page-content-wrapper -->
    </div>
    <!-- /#wrapper -->

                  <asp:Button ID="btnDisableEnter" runat="server" Text="" OnClientClick="return false;" style="display:none;"/> 
    </div>

        
  <!-- Modal -->
  <div class="modal fade" id="myModala" role="dialog">
    <div class="modal-dialog modal-lg">
    
      <!-- Modal content-->
      <div class="modal-content">
        <div class="modal-header">
          <button type="button" class="close" data-dismiss="modal">&times;</button>
          <h4 class="modal-title">Data Flow</h4>
        </div>
        <div class="modal-body">
          <div id="containe" style="height: 400px; width:100%;"></div>
        </div>
        <div class="modal-footer">
          <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
        </div>
      </div>
      
    </div>
  </div>


     

        <div class="modal fade" id="meterAddition" role="dialog">
    <div class="modal-dialog">
    
      <!-- Modal content-->
      <div class="modal-content">
        <div class="modal-header">
          <button type="button" class="close" data-dismiss="modal">&times;</button>
          <h4 class="modal-title">Add Device</h4>
        </div>
        <div class="modal-body">
          <table class="table table-condensed">
              <tr>
                  <td>
                        <asp:UpdatePanel ID="upUnitList"  runat="server" UpdateMode="Conditional">
                          <ContentTemplate>
                              <div class="input-group">
                                  <asp:DropDownList ID="UnitList"  runat="server" CssClass="form-control"></asp:DropDownList>
                                  <span class="input-group-addon"><i class="fa fa-fax"></i></span>

                              </div>
                          </ContentTemplate>
                          <Triggers>
                              <asp:AsyncPostBackTrigger ControlID="goose" />
                          </Triggers>
                      </asp:UpdatePanel>


                  </td>
                  
                  <td>
                      <div class="input-group">
                   <asp:DropDownList ID="DropDownListMeterType" data-toggle="tooltip" ToolTip="Meter Type" CssClass="form-control input-sm"  runat="server">
                    <asp:ListItem Text="Single Phase"  Selected="True"/>
                    <asp:ListItem Text="Three Phase" />
                </asp:DropDownList>
                          <span class="input-group-addon"><i class="fa fa-fax"></i></span>

                      </div>
                  </td>
               
              </tr>
              <tr>
                  <td>
                      <asp:UpdatePanel ID="upmeterSrrl"  runat="server" UpdateMode="Conditional">
                          <ContentTemplate>
                              <div class="input-group">
                                  <asp:DropDownList ID="meterSrrl"  runat="server" CssClass="form-control"></asp:DropDownList>
                                  <span class="input-group-addon"><i class="fa fa-fax"></i></span>

                              </div>
                          </ContentTemplate>
                          <Triggers>
                              <asp:AsyncPostBackTrigger ControlID="goose" />
                          </Triggers>
                      </asp:UpdatePanel>
                  </td>
                     <td>

                           <asp:UpdatePanel ID="UpMachineList"  runat="server" UpdateMode="Conditional">
                          <ContentTemplate>
                              <div class="input-group">
                                  <asp:DropDownList ID="MachineList"  runat="server" CssClass="form-control"></asp:DropDownList>
                                  <span class="input-group-addon"><i class="fa fa-fax"></i></span>

                              </div>
                          </ContentTemplate>
                          <Triggers>
                              <asp:AsyncPostBackTrigger ControlID="goose" />
                          </Triggers>
                      </asp:UpdatePanel>




                  </td>
              
               
 
              </tr>
              <tr>
                  <td>
                      <div class="input-group">
                      <asp:TextBox ID="Utility" data-toggle="tooltip" ToolTip="Utility" CssClass="form-control input-sm" MaxLength="50"  runat="server" placeholder="Utility Co" />
                          <span class="input-group-addon"><i class="fa fa-legal"></i></span>

                      </div>
                  </td>
               
              </tr>
          </table>
        </div>
        <div class="modal-footer">
                  <asp:Button CssClass="btn btn-primary" ID="goose"  runat="server" Text="Reload" OnClick="goose_Click" />
                  <asp:Button ID="addThisMeter" data-toggle="confirmation" data-popout="true" data-singleton="true"  CssClass="btn btn-success"  runat="server" Text="Add" OnClick="addThisMeter_Click" />
                <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
        </div>
      </div>
      
    </div>
  </div>
  <div class="modal fade" id="modalUserManagement" role="dialog">
    <div class="modal-dialog">
    
      <!-- Modal content-->
      <div class="modal-content">
        <div class="modal-header">
          <button type="button" class="close" data-dismiss="modal">&times;</button>
          <h4 class="modal-title">Create New Account</h4>
        </div>
        <div class="modal-body">
          <table class="table table-condensed">
                              <tr>
                    <td>
                        User Details
                    </td>
                    <td></td>
                    <td></td>
                </tr>
              <tr>
                  <td>
                                              <div class="input-group">
  <input type="text" id="addAccountName" title="Full Name" runat="server" maxlength="30" class="form-control input-sm" placeholder="Full Name">
  <span class="input-group-addon"><span class="glyphicon glyphicon-user"></span></span>
</div>
                  </td>
                  <td>
                                              <div class="input-group">
  <input type="text" id="addAccountphone" title="Phone" runat="server" onkeydown = "return (!(event.keyCode>=65) && event.keyCode!=32);" maxlength="11" class="form-control input-sm" placeholder="Phone">
  <span class="input-group-addon"><span class="glyphicon glyphicon-phone"></span></span>
</div>
                  </td>
                  <td>
                                              <div class="input-group">
  <input type="text" id="addaccdesignation" title="Designation" runat="server" maxlength="20" class="form-control input-sm" placeholder="Designation">
  <span class="input-group-addon"><i class="fa fa-shield"></i></span>
</div>
                  </td>
              </tr>
                            <tr>
                  <td>
                                              <div class="input-group">
  <input type="text" id="accaddDepartment" title="Department" runat="server" maxlength="20" class="form-control input-sm" placeholder="Department">
  <span class="input-group-addon"><i class="fa fa-group"></i></span>
</div>
                  </td>
                  <td>
                                              <div class="input-group">
  <input type="text" id="accaddcnic" title="C.N.I.C" runat="server" onkeydown = "return (!(event.keyCode>=65) && event.keyCode!=32);" maxlength="13" class="form-control input-sm" placeholder="C.N.I.C">
  <span class="input-group-addon"><i class="fa fa-male"></i></span>
</div>
                  </td>
                  <td>
                                              <div class="input-group">
  <input type="text" id="addaccemail" title="email" runat="server" maxlength="30" class="form-control input-sm" placeholder="email">
  <span class="input-group-addon">@</span>
</div>
                  </td>
              </tr>
          </table>

            <table class="table table-condensed">
                <tr>
                    <td>
                        Account Details
                    </td>
                    <td></td>
                    <td></td>
                </tr>
                <tr>
                    <td>
                        <div class="input-group">
                         <asp:DropDownList ID="usrtyp" CssClass="form-control input-sm" data-toggle="tooltip" ToolTip="Account Type" runat="server">
            <asp:ListItem Text="User" Value="User"/>
            <asp:ListItem Text="Admin" Value="Admin"/>
            <asp:ListItem Text="Power User"  Value="Power User"/>
        </asp:DropDownList>
        <span class="input-group-addon"><i class="fa fa-user"></i></span>
                            </div>
                    </td>
                    <td>
                                                                      <div class="input-group">
  <input type="text" id="addAccUsername" title="Username" runat="server" maxlength="30" class="form-control input-sm" placeholder="Username">
  <span class="input-group-addon"><i class="fa fa-user"></i></span>
</div>
                    </td>
                    <td>
                                                                      <div class="input-group">
  <input type="password" id="addAccPassword" title="Password" runat="server" maxlength="30" class="form-control input-sm" placeholder="Password">
  <span class="input-group-addon"><i class="fa fa-lock"></i></span>
</div>
                    </td>
                    <td>
                                                                      <div class="input-group">
  <input type="password" id="addAccConfirmPassword" title="Confirm Password" runat="server" maxlength="30" class="form-control input-sm" placeholder="Confirm Password">
  <span class="input-group-addon"><i class="fa fa-lock"></i></span>
</div>
                    </td>
                </tr>
            </table>
        </div>
        <div class="modal-footer">
            <asp:LinkButton ID="LinkButtonAddUser" 
                runat="server"  data-toggle="confirmation" data-popout="true" data-singleton="true"
                CssClass="btn btn-success btn-sm" ToolTip="Add Account"  
                OnClick="LinkButtonAddUser_Click" >
    <i class="fa fa-user-plus" aria-hidden="true"></i> Add
</asp:LinkButton>
          <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
        </div>
      </div>
      
    </div>
  </div>

<!-- Modal -->
        <div id="myModalUserRemove" class="modal fade" role="dialog">
  <div class="modal-dialog modal-sm">

    <!-- Modal content-->
    <div class="modal-content">
      <div class="modal-header">
        <button type="button" class="close" data-dismiss="modal">&times;</button>
        <h4 class="modal-title">Confirmation</h4>
      </div>
      <div class="modal-body">
        <p>Do you really want to remove this User Account?</p>
      </div>
      <div class="modal-footer">
                      <asp:LinkButton ID="LinkButtonRemoveUser" 
                runat="server"  data-toggle="confirmation" data-popout="true" data-singleton="true"
                CssClass="btn btn-danger" ToolTip="Remove Account"  
                OnClick="LinkButtonRemoveUser_Click" >
    <i class="fa fa-user-times" aria-hidden="true"></i> Remove
</asp:LinkButton>
        <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
      </div>
    </div>

  </div>
</div>
        <div id="modalManageUserPermissions" class="modal fade" role="dialog">
  <div class="modal-dialog modal-sm">

    <!-- Modal content-->
    <div class="modal-content">
      <div class="modal-header">
        <button type="button" class="close" data-dismiss="modal">&times;</button>
        <h4 class="modal-title">Manage Permissions</h4>
      </div>
      <div class="modal-body">
         <i class="fa fa-cog fa-spin fa-fw" aria-hidden="true"></i>
<span class="sr-only">Saving. Hang tight!</span> <b>Permissions</b>
                <asp:CheckBoxList ID="checklistPermissions" CssClass="table" runat="server">
            <asp:ListItem Text="Asset Managemnet" Value="Asset Managemnet" />
            <asp:ListItem Text="Load Managemnet" Value="Load Management" />
            <asp:ListItem Text="Archives" Value="Archives" />
            <asp:ListItem Text="Real-Time Readings" Value="Real-Time Readings" />
            <asp:ListItem Text="Device Configuration" Value="Device Configuration" />
        </asp:CheckBoxList>
      </div>
      <div class="modal-footer">
                                <asp:LinkButton ID="LinkButtonAddPermissions" 
                runat="server"  data-toggle="confirmation" data-popout="true" data-singleton="true"
                CssClass="btn btn-danger" ToolTip="Update Permissions"  
                OnClick="LinkButtonAddPermissions_Click" >
    <i class="fa fa-user-secret" aria-hidden="true"></i> Ok
</asp:LinkButton>
        <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
      </div>
    </div>

  </div>
</div>
        <div class="modal fade" id="CriticalAlarmModal" role="dialog">
    <div class="modal-dialog">
    
      <!-- Modal content-->
      <div class="modal-content">
        <div class="modal-header">
          <button type="button" class="close" data-dismiss="modal">&times;</button>
          <h4 class="modal-title">Recent Critical Alarms</h4>
        </div>
        <div class="modal-body">
            <div class="panel-group">
    <div class="panel panel-warning">
      <div class="panel-heading">
        <h4 class="panel-title">
          <a data-toggle="collapse" href="#collapsee"><span class="glyphicon glyphicon-chevron-down"></span> Alarms</a>
        </h4>
      </div>
      <div id="collapsee" class="panel-collapse collapse">
          <asp:LinkButton ID="LinkButtonLoadRecentAlarms" 
                runat="server" 
                CssClass="btn btn-default btn-sm" ToolTip="Refresh"  
                OnClick="LinkButtonLoadRecentAlarms_Click" >
    <span aria-hidden="true" class="glyphicon glyphicon-refresh"></span>
</asp:LinkButton>
        <ul id="yop" class="list-group">
        </ul>
      </div>
    </div>
  </div>
        </div>
        <div class="modal-footer">
          <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
        </div>
      </div>
      
    </div>
  </div>
        <div class="modal fade" id="dialogLPGraph" role="dialog">
    <div class="modal-dialog modal-lg">
    
      <!-- Modal content-->
      <div class="modal-content">
        <div class="modal-header">
          <button type="button" class="close" data-dismiss="modal">&times;</button>
          <h4 class="modal-title">Load Curve</h4>
        </div>
        <div class="modal-body">
                    <div id="container" style="height: 400px; max-height: 400px;"></div>
        </div>
        <div class="modal-footer">
          <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
        </div>
      </div>
      
    </div>
  </div>
        <div class="modal fade" id="dialogHealthReport" role="dialog">
    <div class="modal-dialog modal-sm">
    
      <!-- Modal content-->
      <div class="modal-content">
        <div class="modal-header">
          <button type="button" class="close" data-dismiss="modal">&times;</button>
          <h4 class="modal-title">Meter Health</h4>
        </div>
        <div class="modal-body">
<b>Click Image to Download</b>  <asp:ImageButton ID="downloadHReport" ImageUrl="~/deviceHealth.png" runat="server" OnClick="downloadHReport_Click" />
        </div>
        <div class="modal-footer">
          <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
        </div>
      </div>
      
    </div>
  </div>
        <div id="dialogEmail" style="display: none">

        </div>


        <div class="modal fade" id="dialogEventReport" role="dialog">
    <div class="modal-dialog">
    
      <!-- Modal content-->
      <div class="modal-content">
        <div class="modal-header">
          <button type="button" class="close" data-dismiss="modal">&times;</button>
          <h4 class="modal-title">Advanced Reports</h4>
        </div>
        <div class="modal-body">
                          <h4>Event Report</h4>
            <table style="width:100%;">
                <tr>
                    <td>
                      <b>Event Type</b><asp:DropDownList ID="eventPopupType" CssClass="form-control input-sm" runat="server">
                              <asp:ListItem Text="Relay Operation" Value="Relay Operation" />
                              <asp:ListItem Text="Power Fail" Value="Power Fail" />
                              <asp:ListItem Text="MDI reset" Value="MDI reset" />
                              <asp:ListItem Text="Parametrization" Value="Parametrization" />
                              <asp:ListItem Text="Phase failure" Value="Phase failure" />
                              <asp:ListItem Text="Over/under Volt" Value="Over/under Volt" />
                              <asp:ListItem Text="Demand Over Load" Value="Demand Over Load" />
                              <asp:ListItem Text="Reverse Energy" Value="Reverse Energy" />
                              <asp:ListItem Text="Reverse Polarity" Value="Reverse Polarity" />
                              <asp:ListItem Text="CT Bypass" Value="CT Bypass" />
                              <asp:ListItem Text="All" Value ="All" />
                                           </asp:DropDownList>
                    </td>
                    </tr>
                <tr>
                    <td>
                        <b>Serial</b><asp:DropDownList ID="eventPopupDropDownlist" CssClass="form-control input-sm" runat="server" />
                    </td>
                </tr>
            </table>
           
            <table>
                <tr>
                    <td>
                     Start Date <div style="width:250px;" class="input-group date form_datetime col-md-5" data-date="2016-04-16T05:25:07Z" data-date-format="yyyy/m/d HH:ii:ss" data-link-field="dtp_input1">
                    <input id="evntRprtdt1" runat="server" class="form-control input-sm" size="16" type="text" value="2016/4/4 12:12:12" readonly>
                    <span class="input-group-addon"><span class="glyphicon glyphicon-remove"></span></span>
					<span class="input-group-addon"><span class="glyphicon glyphicon-th"></span></span>
                </div>
                    </td>
                    <td>
                  End Date <div style="width:250px;" class="input-group date form_datetime col-md-5" data-date="2016-04-16T05:25:07Z" data-date-format="yyyy/m/d HH:ii:ss" data-link-field="dtp_input1">
                    <input id="evntRprtdt2" runat="server" class="form-control input-sm" size="16" type="text" value="2016/4/4 12:12:12" readonly>
                    <span class="input-group-addon"><span class="glyphicon glyphicon-remove"></span></span>
					<span class="input-group-addon"><span class="glyphicon glyphicon-th"></span></span>
                </div>
                    </td>
                    <td>
                        <br />
                          &nbsp<asp:LinkButton ID="ButtonGenerateEventReport" 
                runat="server" data-toggle="confirmation" data-popout="true" data-singleton="true"  data-placement="left"
                CssClass="btn btn-danger btn-sm" ToolTip="Download PDF"  
                OnClick="ButtonGenerateEventReport_Click" >
    <span aria-hidden="true" class="glyphicon glyphicon-download-alt"></span> PDF
</asp:LinkButton>
                    </td>
                </tr>
            </table>

                          <h4>MDI Exceed Report</h4>

                                   <table>
                <tr>
                    <td>
                                         Start Date <div style="width:250px;" class="input-group date form_datetime col-md-5" data-date="2016-04-16T05:25:07Z" data-date-format="yyyy/m/d HH:ii:ss" data-link-field="dtp_input1">
                    <input id="MDIr1" runat="server" class="form-control input-sm" size="16" type="text" value="2016/4/4 12:12:12" readonly>
                    <span class="input-group-addon"><span class="glyphicon glyphicon-remove"></span></span>
					<span class="input-group-addon"><span class="glyphicon glyphicon-th"></span></span>
                </div>
                    </td>
                    <td>
                 End Date <div style="width:250px;" class="input-group date form_datetime col-md-5" data-date="2016-04-16T05:25:07Z" data-date-format="yyyy/m/d HH:ii:ss" data-link-field="dtp_input1">
                    <input id="MDIr2" runat="server" class="form-control input-sm" size="16" type="text" value="2016/4/4 12:12:12" readonly>
                    <span class="input-group-addon"><span class="glyphicon glyphicon-remove"></span></span>
					<span class="input-group-addon"><span class="glyphicon glyphicon-th"></span></span>
                </div>
                    </td>
                    <td>
                        <br />
                        <asp:LinkButton ID="btnMDIExceedReportGen" 
                runat="server" data-toggle="confirmation" data-popout="true" data-singleton="true"  data-placement="left"
                CssClass="btn btn-danger btn-sm" ToolTip="Download PDF"  
                OnClick="btnMDIExceedReportGen_Click" >
    <span aria-hidden="true" class="glyphicon glyphicon-download-alt"></span> PDF
</asp:LinkButton>
                    </td>
                </tr>
            </table>
            <table style="width:100%;">
                <tr>
                    <td>
 <b>Start Serial</b><asp:DropDownList ID="DropDownListMDIExceedR1" CssClass="form-control input-sm" runat="server" />
                    </td>
                    </tr>
                <tr>
                    <td>
 <b>End Serial</b><asp:DropDownList ID="DropDownListMDIExceedR2" CssClass="form-control input-sm" runat="server" />
                    </td>
                    </tr>
                <tr>
                    <td>
 <b>MDI Exceed Value</b><asp:TextBox ID="MDIEXceedValue" CssClass="form-control input-sm" style="border-radius:10px" runat="server" placeholder="kW" MaxLength="4" />
                    </td>
                </tr>
            </table>

        </div>
        <div class="modal-footer">
          <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
        </div>
      </div>
      
    </div>
  </div>
        <div class="modal fade" id="dialogEvents" role="dialog">
    <div class="modal-dialog">
    
      <!-- Modal content-->
      <div class="modal-content">
        <div class="modal-header">
          <button type="button" class="close" data-dismiss="modal">&times;</button>
          <h4 class="modal-title">Alarm Dispatch Configuration
                 <asp:UpdateProgress runat="server" ID="UpdateProgress32">
                    <ProgressTemplate>
                        <i class="fa fa-circle-o-notch fa-lg fa-spin" style="color:whitesmoke"></i>
                    </ProgressTemplate>
                </asp:UpdateProgress>
          </h4>
        </div>
        <div class="modal-body">
                          <asp:UpdatePanel runat="server" UpdateMode="Conditional">
                             <Triggers>
                                 <asp:AsyncPostBackTrigger ControlID="GridviewEventConfiguration" EventName="RowEditing" />
                                 <asp:AsyncPostBackTrigger ControlID="GridviewEventConfiguration" EventName="RowUpdating" />
                                 <asp:AsyncPostBackTrigger ControlID="GridviewEventConfiguration" EventName="RowCancelingEdit" />
                                 <asp:AsyncPostBackTrigger ControlID="GridviewEventConfiguration" EventName="PageIndexChanging" />
                             </Triggers>
                             <ContentTemplate>
         <asp:GridView ID="GridviewEventConfiguration"
            autogeneratecolumns="false"
                         GridLines="None"  
                         CssClass="table table-condensed" 
                            PagerStyle-CssClass="pgr"  
                            AlternatingRowStyle-CssClass="alt" OnRowEditing="GridviewEventConfiguration_RowEditing"
              OnRowUpdating="GridviewEventConfiguration_RowUpdating" OnRowCancelingEdit="GridviewEventConfiguration_RowCancelingEdit"
                       AllowPaging="true" PageSize="10" OnPageIndexChanging="GridviewEventConfiguration_PageIndexChanging"
                        runat="server" >
                           <SelectedRowStyle  BorderColor="SkyBlue" BorderWidth="2px"/>
             <Columns>
        <asp:TemplateField HeaderText="Serial" ItemStyle-Width="150">
        <ItemTemplate>
            <asp:Label ID="lblSerial" runat="server" Text='<%# Eval("Serial") %>'></asp:Label>
        </ItemTemplate>
        <EditItemTemplate>
            <asp:TextBox ID="txtSerial" CssClass="form-control input-sm" Width="120" runat="server" Text='<%# Eval("Serial") %>'></asp:TextBox>
        </EditItemTemplate>
    </asp:TemplateField>
    <asp:TemplateField HeaderText="Alert E-Mail Dispatch Address" ItemStyle-Width="150">
        <ItemTemplate>
            <asp:Label ID="lblName" runat="server" Text='<%# Eval("Alert Dispatch Email ID") %>'></asp:Label>
        </ItemTemplate>
        <EditItemTemplate>
            <asp:TextBox ID="txtName" CssClass="form-control input-sm" Width="200" MaxLength="30" runat="server" Text='<%# Eval("Alert Dispatch Email ID") %>'></asp:TextBox>
        </EditItemTemplate>
    </asp:TemplateField>
    <asp:TemplateField HeaderText="Alert SMS Dispatch Number" ItemStyle-Width="150">
        <ItemTemplate>
            <asp:Label ID="lblCountry" runat="server" Text='<%# Eval("Alert Dispatch SMS Number") %>'></asp:Label>
        </ItemTemplate>
        <EditItemTemplate>
            <asp:TextBox ID="txtCountry" CssClass="form-control input-sm" MaxLength="10" onkeydown = "return (!(event.keyCode>=65) && event.keyCode!=32);" Width="120" runat="server" Text='<%# Eval("Alert Dispatch SMS Number") %>'></asp:TextBox>
        </EditItemTemplate>
    </asp:TemplateField>
    <asp:CommandField ButtonType="Link" ShowEditButton="true" ItemStyle-Width="150"/>
</Columns>
            </asp:GridView>
                             </ContentTemplate>
                         </asp:UpdatePanel>
        </div>
        <div class="modal-footer">
          <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
        </div>
      </div>
      
    </div>
  </div>
        <div class="modal fade" id="dialogAlarmsConfiguration" role="dialog">
    <div class="modal-dialog">
    
      <!-- Modal content-->
      <div class="modal-content">
        <div class="modal-header">
          <button type="button" class="close" data-dismiss="modal">&times;</button>
          <h4 class="modal-title">Critical Alarms
                 <asp:UpdateProgress runat="server" ID="UpdateProgress33">
                    <ProgressTemplate>
                        <i class="fa fa-circle-o-notch fa-lg fa-spin" style="color:whitesmoke"></i>
                    </ProgressTemplate>
                </asp:UpdateProgress>
          </h4>
        </div>
        <div class="modal-body" style="width:100%;height:400px;overflow:auto;">
     <asp:UpdatePanel runat="server" UpdateMode="Conditional">
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="GridViewAlarmsConfig" EventName="RowEditing" />
            <asp:AsyncPostBackTrigger ControlID="GridViewAlarmsConfig" EventName="RowCancelingEdit" />
            <asp:AsyncPostBackTrigger ControlID="GridViewAlarmsConfig" EventName="RowUpdating" />
            <asp:AsyncPostBackTrigger ControlID="GridViewAlarmsConfig" EventName="PageIndexChanging" />
        </Triggers>
        <ContentTemplate>
                  <asp:gridview id="GridViewAlarmsConfig" 
                        autogeneratecolumns="true"
                         GridLines="None"  
                         CssClass="table table-condensed"  
                            PagerStyle-CssClass="pgr"  
                            AlternatingRowStyle-CssClass="alt" OnRowEditing="GridViewAlarmsConfig_RowEditing"
                       OnRowCancelingEdit="GridViewAlarmsConfig_RowCancelingEdit" OnRowUpdating="GridViewAlarmsConfig_RowUpdating"
                       AllowPaging="true" PageSize="10" OnPageIndexChanging="GridViewAlarmsConfig_PageIndexChanging"
                        runat="server">
                           <SelectedRowStyle  BorderColor="SkyBlue" BorderWidth="2px"/>
                      <Columns>
<asp:TemplateField HeaderText="Configure" ItemStyle-Width="120">
           <ItemTemplate>
                <asp:DropDownList ID="alarmStatusList" runat="server">
                <asp:ListItem Text="Non Critical" Value="0" />
                 <asp:ListItem Text="Critical" Value="1" />
            </asp:DropDownList>
        </ItemTemplate>
            <EditItemTemplate>
            <asp:DropDownList ID="alarmStatusList" runat="server">
                <asp:ListItem Text="Non Critical" Value="0" />
                 <asp:ListItem Text="Critical" Value="1" />
            </asp:DropDownList>
        </EditItemTemplate>
    </asp:TemplateField>
            <asp:TemplateField HeaderText="Code" ItemStyle-Width="80">
        <ItemTemplate>
            <asp:Label ID="lblCode" runat="server" Text='<%# Eval("Event Code") %>'></asp:Label>
        </ItemTemplate>
        <EditItemTemplate>
            <asp:TextBox ID="txtCode" Width="80" runat="server" Text='<%# Eval("Event Code") %>'></asp:TextBox>
        </EditItemTemplate>
    </asp:TemplateField>
                          <asp:CommandField ButtonType="Link" ShowEditButton="true" ItemStyle-Width="100"/>
                      </Columns>
                </asp:gridview>
            <asp:Label ID="err" runat="server" />
           </ContentTemplate>
    </asp:UpdatePanel>
        </div>
        <div class="modal-footer">
          <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
        </div>
      </div>
      
    </div>
  </div>
        <div class="modal fade" id="dialog4" role="dialog">
    <div class="modal-dialog modal-sm">
    
      <!-- Modal content-->
      <div class="modal-content">
        <div class="modal-header">
          <button type="button" class="close" data-dismiss="modal">&times;</button>
          <h4 class="modal-title">Account Settings</h4>
        </div>
        <div class="modal-body">
          <div class="inner-addon left-addon">
    <i class="glyphicon glyphicon-user"></i>
    <input type="text" id="usernameAccEdit" runat="server" maxlength="30" disabled="disabled" placeholder="Username" class="form-control" />
</div>
            <br />
              <div class="inner-addon left-addon">
    <i class="glyphicon glyphicon-lock"></i>
    <input type="text" id="oldpassword" runat="server" maxlength="30" placeholder="Old Password" class="form-control" />
</div>
            <br />
                  <div class="inner-addon left-addon">
    <i class="glyphicon glyphicon-lock"></i>
    <input type="text" id="newpassword" maxlength="30" runat="server" placeholder="New Password" class="form-control" />
</div>
        </div>
        <div class="modal-footer">
                                    <asp:Button ID="configure" data-toggle="confirmation" data-popout="true" data-singleton="true"   CssClass="btn btn-success" OnClick="configure_Click" Text="Set" runat="server" />
          <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
        </div>
      </div>
      
    </div>
  </div>
        <div class="modal fade" id="dialog3" role="dialog">
    <div class="modal-dialog">
    
      <!-- Modal content-->
      <div class="modal-content">
        <div class="modal-header">
          <button type="button" class="close" data-dismiss="modal">&times;</button>
          <h4 class="modal-title">License Agreement</h4>
        </div>
        <div class="modal-body">
                            <h4>Developed by Transfopower R&D Department </h4>
    <p>
        Software License Agreement<br />

1. This is an agreement between Licensor and Licensee, who is being licensed to use the named Software.<br />

2. Licensee acknowledges that this is only a limited nonexclusive license. Licensor is and remains the owner of all titles, rights, and interests in the Software.<br />

3. This License permits Licensee to install the Software on more than one computer system, as long as the Software will not be used on more than one computer system simultaneously. Licensee will not make copies of the Software or allow copies of the Software to be made by others, unless authorized by this License Agreement. Licensee may make copies of the Software for backup purposes only.<br />

4. This Software is subject to a limited warranty. Licensor warrants to Licensee that the physical medium on which this Software is distributed is free from defects in materials and workmanship under normal use, the Software will perform according to its printed documentation, and to the best of Licensor's knowledge Licensee's use of this Software according to the printed documentation is not an infringement of any third party's intellectual property rights. This limited warranty lasts for a period of ____ days after delivery. To the extent permitted by law, THE ABOVE-STATED LIMITED WARRANTY REPLACES ALL OTHER WARRANTIES, EXPRESS OR IMPLIED, AND LICENSOR DISCLAIMS ALL IMPLIED WARRANTIES INCLUDING ANY IMPLIED WARRANTY OF TITLE, MERCHANTABILITY, NONINFRINGEMENT, OR OF FITNESS FOR A PARTICULAR PURPOSE. No agent of Licensor is authorized to make any other warranties or to modify this limited warranty. Any action for breach of this limited warranty must be commenced within one year of the expiration of the warranty. Because some jurisdictions do not allow any limit on the length of an implied warranty, the above limitation may not apply to this Licensee. If the law does not allow disclaimer of implied warranties, then any implied warranty is limited to ____ days after delivery of the Software to Licensee. Licensee has specific legal rights pursuant to this warranty and, depending on Licensee's jurisdiction, may have additional rights.<br />

5. In case of a breach of the Limited Warranty, Licensee's exclusive remedy is as follows: Licensee will return all copies of the Software to Licensor, at Licensee's cost, along with proof of purchase. (Licensee can obtain a step-by-step explanation of this procedure, including a return authorization code, by contacting Licensor at [address and toll free telephone number].) At Licensor's option, Licensor will either send Licensee a replacement copy of the Software, at Licensor's expense, or issue a full refund.<br />

6. Notwithstanding the foregoing, LICENSOR IS NOT LIABLE TO LICENSEE FOR ANY DAMAGES, INCLUDING COMPENSATORY, SPECIAL, INCIDENTAL, EXEMPLARY, PUNITIVE, OR CONSEQUENTIAL DAMAGES, CONNECTED WITH OR RESULTING FROM THIS LICENSE AGREEMENT OR LICENSEE'S USE OF THIS SOFTWARE. Licensee's jurisdiction may not allow such a limitation of damages, so this limitation may not apply.<br />

7. Licensee agrees to defend and indemnify Licensor and hold Licensor harmless from all claims, losses, damages, complaints, or expenses connected with or resulting from Licensee's business operations.<br />

8. Licensor has the right to terminate this License Agreement and Licensee's right to use this Software upon any material breach by Licensee.<br />

9. Licensee agrees to return to Licensor or to destroy all copies of the Software upon termination of the License.<br />

10. This License Agreement is the entire and exclusive agreement between Licensor and Licensee regarding this Software. This License Agreement replaces and supersedes all prior negotiations, dealings, and agreements between Licensor and Licensee regarding this Software.<br />

11. This License Agreement is governed by the law of [State] applicable to [State] contracts.<br />

12. This License Agreement is valid without Licensor's signature. It becomes effective upon the earlier of Licensee's signature or Licensee's use of the Software.<br />
    </p>
        <h3>Copyright © Transfopower Industries Private Limited.</h3>
    <img id="logo" src="logo.png" runat="server" />
        </div>
        <div class="modal-footer">
          <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
        </div>
      </div>
      
    </div>
  </div>
        <div class="modal fade" id="crmodal" role="dialog">
    <div class="modal-dialog">
    
      <!-- Modal content-->
      <div class="modal-content">
        <div class="modal-header">
          <button type="button" class="close" data-dismiss="modal">&times;</button>
          <h4 class="modal-title"><i class="fa fa-newspaper-o"></i> Custom Report Generation</h4>
        </div>
        <div class="modal-body">
            <asp:Label ID="see" runat="server"></asp:Label>
        <h4>Report Parameters <i class="fa fa-newspaper-o"></i></h4>
          <table class="table table-striped table-responsive">
              <tr>
                  <td><asp:CheckBox ID="kWhcheck" Text="kWh" ToolTip="1.0.15.8.0.255" runat="server" /></td>
                  <td><asp:CheckBox ID="kVARhcheck" Text="kVARh" ToolTip="1.0.94.92.0.255" runat="server" /></td>
                  <td><asp:CheckBox ID="pfcheck" Text="Avg Power Factor" ToolTip="1.0.13.7.0.255" runat="server" /></td>
                  <td><asp:CheckBox ID="mdiabscheck" Text="MDI Abs" ToolTip="1.0.15.6.0.255" runat="server" /></td>
              </tr>
              <tr>
                  <td><asp:CheckBox ID="mdicumcheck" Text="MDI Cum" ToolTip="1.0.15.2.0.255" runat="server" /></td>
                  <td><asp:CheckBox ID="mdirdatecheck" Text="MDI Reser Date" ToolTip="1.0.0.1.2.255" runat="server" /></td>
                  <td><asp:CheckBox ID="mdircountcheck" Text="MDI Reset Count" ToolTip="1.0.0.1.0.255" runat="server" /></td>
                  <td><asp:CheckBox ID="includeOBIScheck" Text="Include OBIS" runat="server" /></td>
              </tr>
            </table>
            <table class="table table-responsive">
                <tr>
                    <th>Interval From</th>
                    <th>To</th>
                </tr>
              <tr>
                  <td>
                <div style="width:100%;" class="input-group date form_datetime col-md-5" data-date="" data-date-format="yyyy/m/d HH:ii:ss" data-link-field="dtp_input1">
                    <input id="Text1" runat="server" class="form-control input-sm" size="16" type="text" value="2016/4/4 12:12:12" readonly>
                    <span class="input-group-addon"><span class="glyphicon glyphicon-remove"></span></span>
					<span class="input-group-addon"><span class="glyphicon glyphicon-th"></span></span>
                </div>
                  </td>
                  <td>
                <div style="width:100%;" class="input-group date form_datetime col-md-5" data-date="" data-date-format="yyyy/m/d HH:ii:ss" data-link-field="dtp_input1">
                    <input id="Text2" runat="server" class="form-control input-sm" size="16" type="text" value="2016/4/4 12:12:12" readonly>
                    <span class="input-group-addon"><span class="glyphicon glyphicon-remove"></span></span>
					<span class="input-group-addon"><span class="glyphicon glyphicon-th"></span></span>
                </div>
                  </td>
              </tr>
                <tr id="range">
                    <td>
     <div class="input-group">
<asp:DropDownList ID="fu" ToolTip="Serial From" CssClass="form-control" runat="server"></asp:DropDownList>
  <span class="input-group-addon"><span class="glyphicon glyphicon-signal"></span></span>
</div>
                    </td>
                    <td>
     <div class="input-group">
<asp:DropDownList ID="fu1" ToolTip="Serial To" CssClass="form-control" runat="server"></asp:DropDownList>
  <span class="input-group-addon"><span class="glyphicon glyphicon-signal"></span></span>
</div>
                    </td>
                </tr>
          </table>
        </div>
        <div class="modal-footer">
        <asp:Button ID="generateCustomReport" data-toggle="confirmation" CssClass="btn btn-success" Text="Generate" runat="server" OnClick="generateCustomReport_Click" />
          <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
        </div>
      </div>
      
    </div>
  </div>
        <div class="modal fade" id="cremodal" role="dialog">
    <div class="modal-dialog modal-lg">
    
      <!-- Modal content-->
      <div class="modal-content">
        <div class="modal-header">
          <button type="button" class="close" data-dismiss="modal">&times;</button>
          <h4 class="modal-title"><i class="fa fa-line-chart"></i> System Stats</h4>
        </div>
        <div class="modal-body">
        <h4>System Reports <i class="fa fa-newspaper-o"></i></h4>
        <div id="jumbo1" class="panel" style="text-align:center">
            <p>Click 'Load' to pull up system stats.</p>
            </div>
        <div id="jumbo" class="panel" style="text-align:center">
            <i class="fa fa-circle-o-notch fa-5x fa-spin"></i>
        </div>
            <asp:Label ID="Label1" runat="server"></asp:Label>
  					<div id="sysstat" class="row">
  						<div class="col-md-6">
  							<div id="hero-bar" style="height: 200px;"></div>
  						</div>
  						<div class="col-md-3">
  							<div id="hero-donut" style="height: 200px;"></div>
  						</div>
  						<div class="col-md-3">
  							<div id="hero-donut2" style="height: 200px;"></div>
  						</div>
  					</div>
        </div>
        <div class="modal-footer">
        <a id="hero" href="#" class="btn btn-success">Load</a>
          <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
        </div>
      </div>
      
    </div>
  </div>
        <div class="modal fade" id="ftpFiles" role="dialog">
    <div class="modal-dialog">
    
      <!-- Modal content-->
      <div class="modal-content">
        <div class="modal-header">
          <button type="button" class="close" data-dismiss="modal">&times;</button>
          <h4 class="modal-title"><i class="fa fa-download"></i> Billing Data Batch Files</h4>
        </div>
        <div class="modal-body" style="text-align:center;">
          <asp:GridView ID="gvFiles" CssClass="table table-bordered table-striped table-condensed table-responsive" runat="server" AutoGenerateColumns="false">
<Columns>
    <asp:BoundField DataField="Name" HeaderText="Name"  />
    <asp:BoundField DataField="Size" HeaderText="Size (KB)" DataFormatString="{0:N2}"
         />
    <asp:BoundField DataField="Date" HeaderText="Created Date"  />
    <asp:TemplateField>
        <ItemTemplate>
                    <asp:LinkButton ID="lnkDownload" runat="server" Text="Download" OnClick="lnkDownload_Click"
            CommandArgument='<%# Eval("Name") %>'></asp:LinkButton>
        </ItemTemplate>
    </asp:TemplateField>
</Columns>
</asp:GridView>
                        <asp:UpdateProgress runat="server" ID="UpdateProgress8">
                    <ProgressTemplate>
                        <i class="fa fa-circle-o-notch fa-5x fa-spin"></i>
                    </ProgressTemplate>
                </asp:UpdateProgress>
        </div>
        <div class="modal-footer">
          <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
        </div>
      </div>
      
    </div>
  </div>
        <div class="modal fade" id="errorModal" role="dialog">
    <div class="modal-dialog">
    
      <!-- Modal content-->
      <div class="modal-content">
        <div class="modal-header">
          <button type="button" class="close" data-dismiss="modal">&times;</button>
          <h4 class="modal-title"><i class="fa fa-warning" style="color:darkred"></i> Error Log</h4>
        </div>
        <div class="modal-body">
            <asp:UpdatePanel runat="server">
                <ContentTemplate>
          <p><i class="fa fa-warning" style="color:darkred"></i> <asp:Label ID="errLog" runat="server">No errors reported.</asp:Label></p>
                            </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <div class="modal-footer">
          <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
        </div>
      </div>
      
    </div>
  </div>
        <div class="modal fade" id="modalInventory" role="dialog">
    <div class="modal-dialog">
    
      <!-- Modal content-->
      <div class="modal-content">
        <div class="modal-header">
          <button type="button" class="close" data-dismiss="modal">&times;</button>
          <h4 class="modal-title">Add Store Meter</h4>
        </div>
        <div class="modal-body">
          <div class="row">
              <div class="col-md-12">
                  <input type="hidden" id="iMeter" runat="server" />
                  <table class="table">
                      <tr>
                          <td>
                              <input type="text" MaxLength="10" placeholder="Device Serial" onkeydown = "return (!(event.keyCode>=65) && event.keyCode!=32);" runat="server" id="iMeterNew" class="form-control" />
                          </td>
                          <td>
                              <asp:DropDownList ID="iMeterType" CssClass="form-control" runat="server">
                                  <asp:ListItem>Three Phase</asp:ListItem>
                                  <asp:ListItem>Single Phase</asp:ListItem>
                              </asp:DropDownList>
                          </td>
                          <td>
                              <button type="button" data-toggle="confirmation" class="btn btn-primary" onclick="addInventory();">Add Meter</button>
                          </td>
                          <td>
                              <asp:UpdateProgress runat="server" ID="UpdateProgress38">
                    <ProgressTemplate>
                        <i class="fa fa-circle-o-notch fa-lg fa-spin"></i>
                    </ProgressTemplate>
                </asp:UpdateProgress>
                          </td>
                      </tr>
                  </table>
                  <table id="tableStore" class="display compact table table-striped table-bordered  dt-responsive nowrap" style="width:100%; font-size:small">
        <thead>
            <tr>
                <th>Meter Serial</th>
                <th>Device Type</th>
                <th>Date Added</th>
            </tr>
        </thead>
        <tfoot>
            <tr>
                <th>Meter Serial</th>
                <th>Device Type</th>
                <th>Date Added</th>
            </tr>
        </tfoot>
    </table>
                  
                 <br />
                  <div class="row">
                      <div class="col-md-3">
                          <asp:Button ID="deleteImeter" data-toggle="confirmation" CssClass="btn btn-danger" Text="Remove Selection" runat="server" OnClick="deleteImeter_Click" />
                          </div>
                      <div class="col-md-6">
                          <input type=file id=files class="form-control" />
                      </div>
                      <div class="col-md-3">
                          <button type="button" class="btn btn-primary" id=upload><i class="fa fa-upload"></i> Upload</button>
                      </div>
                  </div>
                  

<br/><br/>

              </div>
          </div>
        </div>
        <div class="modal-footer">
          <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
        </div>
      </div>
      
    </div>
  </div>
  
        <!-- Modal Ended -->
    
    </form>

<script type="text/javascript" src="js/jquery-2.2.3.min.js"></script>
	<script src="http://code.jquery.com/ui/1.12.1/jquery-ui.min.js"></script>
<script src="http://maxcdn.bootstrapcdn.com/bootstrap/3.3.6/js/bootstrap.js" charset="UTF-8"></script>
<script src="https://cdn.datatables.net/1.10.12/js/jquery.dataTables.min.js" charset="utf-8"></script>
<script src="https://cdn.datatables.net/1.10.12/js/dataTables.bootstrap.min.js" charset="utf-8"></script>
<script src="https://cdn.datatables.net/responsive/2.1.0/js/dataTables.responsive.min.js" charset="utf-8"></script>
<script src="https://cdn.datatables.net/responsive/2.1.0/js/responsive.bootstrap.min.js" charset="utf-8"></script>
<script src="https://cdn.datatables.net/buttons/1.2.2/js/dataTables.buttons.min.js" charset="utf-8"></script>
<script src="https://cdnjs.cloudflare.com/ajax/libs/jszip/2.5.0/jszip.min.js" charset="utf-8"></script>
<script src="https://cdn.rawgit.com/bpampuch/pdfmake/0.1.18/build/pdfmake.min.js" charset="utf-8"></script>
<script src="https://cdn.rawgit.com/bpampuch/pdfmake/0.1.18/build/vfs_fonts.js" charset="utf-8"></script>
<script src="https://cdn.datatables.net/buttons/1.2.2/js/buttons.html5.min.js" charset="utf-8"></script>
<script src="https://cdn.datatables.net/buttons/1.2.2/js/buttons.bootstrap.min.js" charset="utf-8"></script>
<script src="https://cdn.datatables.net/select/1.2.0/js/dataTables.select.min.js" charset="utf-8"></script>
<script src="https://cdn.datatables.net/buttons/1.2.2/js/buttons.colVis.min.js" charset="utf-8"></script>
<script src="https://cdn.datatables.net/buttons/1.2.2/js/buttons.print.min.js" charset="utf-8"></script>
<script type="text/javascript" src="Scripts/bootstrap-datetimepicker.js" charset="UTF-8"></script>
<script type="text/javascript" src="Scripts/bootstrap-switch.js" charset="utf-8"></script>
<script type="text/javascript" src="bootstrap-notify.js" charset="utf-8"></script>
<script type="text/javascript" src="bootstrap-confirmation.js" charset="utf-8"></script>
<script type="text/javascript" src="https://code.highcharts.com/stock/highstock.js" charset="UTF-8"></script>
<script type="text/javascript" src="https://code.highcharts.com/stock/modules/exporting.js" charset="UTF-8"></script>
<script type="text/javascript" src="http://code.highcharts.com/highcharts.js" charset="UTF-8"></script>
<script type="text/javascript" src="http://code.highcharts.com/modules/exporting.js" charset="UTF-8"></script>
<script src="http://code.highcharts.com/highcharts-3d.js" charset="utf-8" type="text/javascript"></script>
<script src="Scripts/json2.min.js" type="text/javascript" charset="UTF-8"></script>

<link href="src/skin-bootstrap/ui.fancytree.css" rel="stylesheet"
		class="skinswitcher">

	<script src="src/jquery.fancytree.js"></script>
	<script src="src/jquery.fancytree.dnd.js"></script>
	<script src="src/jquery.fancytree.edit.js"></script>
	<script src="src/jquery.fancytree.glyph.js"></script>
	<script src="src/jquery.fancytree.table.js"></script>
	<script src="src/jquery.fancytree.wide.js"></script>
    <script src="src/jquery.fancytree.childcounter.js"></script>

<!-- Start_Exclude: This block is not part of the sample code -->
	<link href="lib/prettify.css" rel="stylesheet">
	<script src="lib/prettify.js"></script>
	<link href="demo/sample.css" rel="stylesheet">
	<script src="demo/sample.js"></script>

<script src="main-script.js" type="text/javascript"></script>

<script src="dark-unica.js" type="text/javascript"></script>
<script src="side-menu.js"></script>
    

<script type="text/javascript">
    glyph_opts = {
        preset: "bootstrap3",
        map: {
            expanderClosed: "glyphicon glyphicon-menu-right",  // glyphicon-plus-sign
            expanderLazy: "glyphicon glyphicon-menu-right",  // glyphicon-plus-sign
            expanderOpen: "glyphicon glyphicon-menu-down"  // glyphicon-minus-sign
        }
    };
    treeit();
</script>


    <script src="vendors/jquery.knob.js"></script>
    <script src="vendors/raphael-min.js"></script>
    <script src="vendors/morris/morris.min.js"></script>

    <script src="vendors/flot/jquery.flot.js"></script>
    <script src="vendors/flot/jquery.flot.categories.js"></script>
    <script src="vendors/flot/jquery.flot.pie.js"></script>
    <script src="vendors/flot/jquery.flot.time.js"></script>
    <script src="vendors/flot/jquery.flot.stack.js"></script>
    <script src="vendors/flot/jquery.flot.resize.js"></script>
    <script>
        function openCity(evt, cityName) {
            evt.preventDefault();
            var i, tabcontent, tablinks;

            // Get all elements with class="tabcontent" and hide them
            tabcontent = document.getElementsByClassName("tabcontent");
            for (i = 0; i < tabcontent.length; i++) {
                tabcontent[i].style.display = "none";
            }

            // Get all elements with class="tablinks" and remove the class "active"
            tablinks = document.getElementsByClassName("tablinks");
            for (i = 0; i < tablinks.length; i++) {
                tablinks[i].className = tablinks[i].className.replace(" active", "");
            }

            // Show the current tab, and add an "active" class to the button that opened the tab
            document.getElementById(cityName).style.display = "block";
            evt.currentTarget.className += " active";
        }
 
</script>

<script>
    document.getElementById("toggleButton").addEventListener("click", function (event) {
        event.preventDefault();
        var chartContainer = document.getElementById("consumption_pie_chart");
        var toggleButton = document.getElementById("toggleButton");
        if (chartContainer.style.display === "none") {
            chartContainer.style.display = "block";
            toggleButton.textContent = "-";
        } else {
            chartContainer.style.display = "none";
            toggleButton.textContent = "+";
        }
    });

    document.getElementById("toggleButton1").addEventListener("click", function (event) {
        event.preventDefault();
        var chartContainer1 = document.getElementById("consumption_year_chart");
        var toggleButton1 = document.getElementById("toggleButton1");
        if (chartContainer1.style.display === "none") {
            chartContainer1.style.display = "block";
            toggleButton1.textContent = "-";
        } else {
            chartContainer1.style.display = "none";
            toggleButton1.textContent = "+";
        }
    });


    document.getElementById("toggleButton2").addEventListener("click", function (event) {
        event.preventDefault();
        var chartContainer2 = document.getElementById("consumption_year_pie_chart");
        var toggleButton2 = document.getElementById("toggleButton2");
        if (chartContainer2.style.display === "none") {
            chartContainer2.style.display = "block";
            toggleButton2.textContent = "-";
        } else {
            chartContainer2.style.display = "none";
            toggleButton2.textContent = "+";
        }
    });



</script>


</body>
</html>

