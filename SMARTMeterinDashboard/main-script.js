// ************************************ TREE.JS***************************************//
function treeit() {
    $(function () {
        // Initialize Fancytree
        $("#tree").fancytree({
            extensions: ["dnd", "edit", "glyph", "wide", "childcounter"],
            checkbox: true,
            dnd: {
                focusOnClick: true,
                dragStart: function (node, data) { return true; },
                dragEnter: function (node, data) { return false; },
                dragDrop: function (node, data) { data.otherNode.copyTo(node, data.hitMode); }
            },
            glyph: glyph_opts,
            selectMode: 2,
            source: {
                url: "http://localhost:23292/Service1.svc/mainnode",
                cache: false
            },
            childcounter: {
                deep: true,
                hideZeros: true,
                hideExpanded: true
            },
            toggleEffect: { effect: "drop", options: { direction: "left" }, duration: 400 },
            wide: {
                iconWidth: "1em",       // Adjust this if @fancy-icon-width != "16px"
                iconSpacing: "0.5em",   // Adjust this if @fancy-icon-spacing != "3px"
                labelSpacing: "0.1em",  // Adjust this if padding between icon and label != "3px"
                levelOfs: "1.5em"       // Adjust this if ul padding != "16px"
            },

            icon: function (event, data) {
                // if( data.node.isFolder() ) {
                // 	return "glyphicon glyphicon-book";
                // }
            }, activate: function (event, data) {
                // A node was activated: display its title:
                var node = data.node;
                document.getElementById("res").value = node.title;
                //alert(node.title);
            },
            lazyLoad: function (event, data) {
                var node = data.node;
                // Issue an ajax request to load child nodes
                if (node.key.indexOf('a') > -1) {
                    data.result = {
                        url: "http://localhost:23292/Service1.svc/child?parent=" + node.title,
                        data: { key: node.key }
                    }
                }
                if (node.key.indexOf('b') > -1) {
                    data.result = {
                        url: "http://localhost:23292/Service1.svc/grandchild?parent=" + node.title,
                        data: { key: node.key }
                    }
                }
                if (node.key.indexOf('c') > -1) {
                    data.result = {


                        url: "http://localhost:23292/Service1.svc/greatgrandchild?parent=" + node.title,
                        data: { key: node.key }
                    }
                }
            },
            loadChildren: function (event, data) {
                // update node and parent counters after lazy loading
                data.node.updateCounters();
            }
        });
    });
};

var lpc;
function cruncher(kWhJ, kWJ, kVARhJ, kVARJ, timeStuff) {
    $(function () {
      lpc =  $('#container').highcharts('StockChart', {
            rangeSelector: {
                buttons: [{
                    count: 1,
                    type: 'hour',
                    text: '1h'
                }, {
                    count: 1,
                    type: 'day',
                    text: '1D'
                }, {
                    count: 1,
                    type: 'week',
                    text: '1W'
                }, {
                    count: 1,
                    type: 'month',
                    text: '1M'
                }, {
                    count: 3,
                    type: 'month',
                    text: '3M'
                },
                 {
                     count: 1,
                     type: 'year',
                     text: '1Y'
                 }, {
                     type: 'all',
                     text: 'All'
                 }],
                selected: 1
            },

            title: {
                text: 'Load Profile: ' + timeStuff
            },
            legend: {
                enabled: true,
            },
            series: [{
                name: 'kWh',
                data: JSON.parse("[" + kWhJ + "]")
            }, {
                name: 'kW',
                data: JSON.parse("[" + kWJ + "]")
            }, {
                name: 'kVARh',
                data: JSON.parse("[" + kVARhJ + "]")
            }, {
                name: 'kVAR',
                data: JSON.parse("[" + kVARJ + "]")
            }]
        });

        lpc = $('#container').highcharts();
        $('#dialogLPGraph').on('show.bs.modal', function () {
            $('#container').css('visibility', 'hidden');
        });
        $('#dialogLPGraph').on('shown.bs.modal', function () {
            $('#container').css('visibility', 'initial');
            lpc.reflow();
        });
    });
};


function upitStat(s1, s2, s3, t1, t2, t3, ty, serial) {
    $(function () {
        $('#containerStatGraph').highcharts('StockChart', {
            rangeSelector: {
                buttons: [{
                    count: 1,
                    type: 'hour',
                    text: '1h'
                }, {
                    count: 1,
                    type: 'day',
                    text: '1D'
                }, {
                    count: 1,
                    type: 'week',
                    text: '1W'
                }, {
                    count: 1,
                    type: 'month',
                    text: '1M'
                }, {
                    count: 3,
                    type: 'month',
                    text: '3M'
                },
                 {
                     count: 1,
                     type: 'year',
                     text: '1Y'
                 }, {
                     type: 'all',
                     text: 'All'
                 }],
                selected: 1
            },

            title: {
                text: serial + ' :' + ty
            },
            legend: {
                enabled: true,
            },
            series: [{
                name: t1,
                data: JSON.parse("[" + s1 + "]")
            }, {
                name: t2,
                data: JSON.parse("[" + s2 + "]")
            }, {
                name: t3,
                data: JSON.parse("[" + s3 + "]")
            }]
        });
    });
};
function upitStat1(s1, t1, ty, serial) {
    $(function () {
        $('#containerStatGraph').highcharts('StockChart', {
            rangeSelector: {
                buttons: [{
                    count: 1,
                    type: 'hour',
                    text: '1h'
                }, {
                    count: 1,
                    type: 'day',
                    text: '1D'
                }, {
                    count: 1,
                    type: 'week',
                    text: '1W'
                }, {
                    count: 1,
                    type: 'month',
                    text: '1M'
                }, {
                    count: 3,
                    type: 'month',
                    text: '3M'
                },
                 {
                     count: 1,
                     type: 'year',
                     text: '1Y'
                 }, {
                     type: 'all',
                     text: 'All'
                 }],
                selected: 1
            },

            title: {
                text: serial + ' :' + ty
            },
            legend: {
                enabled: true,
            },
            series: [{
                name: t1,
                data: JSON.parse("[" + s1 + "]")
            }]
        });
    });
};
$(function () {

    Highcharts.setOptions({
        global: {
            useUTC: false
        }
    });
    Highcharts.setOptions(Highcharts.theme);
    // Create the chart
    $('#containe').highcharts('StockChart', {
        chart: {
            events: {
                load: function () {

                    // set up the updating of the chart each second
                    var series = this.series[0];
                    setInterval(function () {
                        var x = (new Date()).getTime(), // current time
                            y = Math.round(Math.random() * 100);
                        series.addPoint([x, y], true, true);
                    }, 1000);
                }
            }
        },

        rangeSelector: {
            buttons: [{
                count: 1,
                type: 'minute',
                text: '1M'
            }, {
                count: 5,
                type: 'minute',
                text: '5M'
            }, {
                type: 'all',
                text: 'All'
            }],
            inputEnabled: false,
            selected: 0
        },

        title: {
            text: 'Live data flow'
        },

        exporting: {
            enabled: false
        },

        series: [{
            name: 'kbps data',
            data: (function () {
                // generate an array of random data
                var data = [], time = (new Date()).getTime(), i;

                for (i = -999; i <= 0; i += 1) {
                    data.push([
                        time + i * 1000,
                        Math.round(Math.random() * 100)
                    ]);
                }
                return data;
            }())
        }]
    });

});

function Spin(hilt,spinner) {
    if (hilt == 1) {
        $(document.getElementById(spinner)).show();
    }
    else {
        $(document.getElementById(spinner)).hide();
    }
}

$(function () {
    $('#containerTotCon').highcharts({
        chart: {
            type: 'column',
            margin: 75,
            options3d: {
                enabled: true,
                alpha: 15,
                beta: 15,
                depth: 50
            }
        },
        plotOptions: {
            column: {
                depth: 25
            }
        },
        series: [{
            title: "Hours",
            data: [29.9, 71.5, 106.4, 129.2, 144.0, 176.0, 135.6, 148.5, 216.4, 194.1, 95.6, 54.4]
        }]
    });
});

var init = 0;
var inits = "";

$(function () {
    $(document).on('click', '.tree li.parent_li > span', function (e) {
        var children = $(this).parent('li.parent_li').find(' > ul > li');
        if (children.is(":visible")) {
            children.hide('fast');
            if (($(this).attr('title', 'Expand this branch').find(' > i').hasClass('fa-minus-circle'))) {
                $(this).attr('title', 'Expand this branch').find(' > i').addClass('fa-plus-circle').removeClass('fa-minus-circle');
            }
        } else {
            children.show('fast');
            if (($(this).attr('title', 'Collapse this branch').find(' > i').hasClass('fa-plus-circle'))) {
                $(this).attr('title', 'Collapse this branch').find(' > i').addClass('fa-minus-circle').removeClass('fa-plus-circle');
            }
        }
        e.stopPropagation();
    });
});
/// Elements To handle javascript UIS CONTAING ALL DIV CLASSES
var divs = $("#gpslocation, #deviceGroup, #manreg, #autoreg, #offlineOnline, #relay, #events, #stats, #fileexp, #LP, #usermgmt, #acclog, #actlog, #commLog, #instRead, #dataRead, #networkSettings, #relaySettings, #generalSettings, #remoteUpgrade, #DivFotTotCon, #manrega, #rfdat, #fd, #time, #homie");



var theAas = $("#autoregA, #locatedeviceA, #relonoffA, #eventsA, #loadThresholdA, #netstatA, #ganalA, #dwnrprtA, #lpA, #usrmgmtA, #acclgA, #actlgA, #commlgA, #instRA, #dataRA, #netsetA, #gensetA, #frmwreA, #totpow, #eventRA, #deviceGroupA, #manregA, #rfdatA, #fdA, #timeA");
var currentTab;
var tbs = "#nth1,#nth1t,#nth2,#nth2t,#nth3,#nth3t,#nth4,#nth4t,#nth5,#nth5t,#nth6,#nth6t,#nth7,#nth7t,#nth8,#nth8t,#nth9,#nth9t,#nth10,#nth10t,#nth11,#nth11t,#nth12,#nth12t,#nth13,#nth13t,#nth14,#nth14t,#nth15,#nth15t,#nth16,#nth16t,#nth17,#nth17t,#nth18,#nth18t,#nth19,#nth19t,#nth20,#nth20t,#nth21,#nth21t,#nth22,#nth22t,#nth23,#nth23t,#nth24,#nth24t";
function registerCloseEvent() {
    $(".closeTab").click(function () {
        $(this).parent().parent().remove(); //remove li of tab
        if (currentTab == $(this).parent().parent().attr('title')) {
            $(divs).hide();
            $("#myTab li").removeClass();
            $('#homie').show(); // Select first tab
            $("#homo").addClass('active');
       
        }
    });
}

$(document).ready(function () {
    Load_Power_factor();
    Load_Current();
    Loadmainconsumption();
    Loadyearlyconsumption();
    Loadyearlypieconsumption();
    LoadHomegraph();
    createVoltageGraph();
    // Selector
    $(bars).fadeOut();
    $("#opGif").hide();
    $("#loading").hide();
    $("#EditWell").hide();
    $('#sysstat,#jumbo').hide();
    $('#createth,#setth,#planTable').hide();
   
    $(mens).hide();
    $('#dgcreator').show();
    $('#hero').click(function () {
        $('#sysstat,#jumbo').show();
    
    });
    //initilize tabs
    $(function() {
        $("#myTab").on("click", "a", function(e) {
            e.preventDefault();
            $(divs).hide();
            $("#myTab li").removeClass();
            $(this).parent().addClass('active');
            currentTab = $(this).parent().attr('title');
            $("#" + $(this).attr("class")).show();
        });
        registerCloseEvent();
    });
   
    //getMeterssvc();

    $('#example').DataTable();
    $('#exampleLP').DataTable();
    $('#etable').DataTable();
    $('#instRtable').DataTable();
    $('#exampleEvents').DataTable();
    $('#accessT').DataTable();
    $('#commT').DataTable();
    $('#actionT').DataTable();

    $('#pcreate').click(function () {
        $('#setth').hide();
        $('#createth,#planTable').show();
        $(tbs).attr('disabled', false);
    });

  $(window).resize(function() {
    setboxHeight();
  });
  $('[data-toggle=confirmation]').confirmation({
      rootSelector: '[data-toggle=confirmation]'
  });
  $('input[name="my-checkbox"]').bootstrapSwitch('toggleIndeterminate', true, true);

  $("[data-toggle='toggle']").click(function () {
      var selector = $(this).data("target");
      $(selector).toggleClass('in');
  });

  $('#boof').on('click', function () {
      $('#Networkcontainer').highcharts().setSize(null, null);
      $('#Networkcontainer').highcharts().hasUserSize = false;
     $('#containerStatGraph').highcharts().setSize(null, null);
      $('#containerStatGraph').highcharts().hasUserSize = false;
  });
    /**************** Store Meters ****************************/
  var fileInput = $('#files');
  var uploadButton = $('#upload');

  uploadButton.on('click', function () {
      if (!window.FileReader) {
          alert('Your browser is not supported')
      }
      var input = fileInput.get(0);

      // Create a reader object
      var reader = new FileReader();
      if (input.files.length) {
          var textFile = input.files[0];
          reader.readAsText(textFile);
          $(reader).on('load', processFile);
      } else {
          alert('Please upload a file before continuing')
      }
  });
    /********************** Store Meters **************************/

    $(theAas).click(function () {
        $(divs).hide();
        $("#" + $(this).attr("class")).show();
        $("#menu li").removeClass();
        $(this).parent().addClass('active');
        setboxHeight();

        var gop = true;
        var c2 = $('#' + $(this).attr("class")).attr("class");

        $('#myTab li').each(function (i) {
            var c1 = $(this).attr('title');
            if (c1 == c2) {
                gop = false;
                $("#myTab li").removeClass();
                $(this).addClass('active');
            };
        });

        if (gop) {
            $("#myTab li").removeClass();
            $('#myTab').append('<li title="' + $('#' + $(this).attr("class")).attr("class") + '" class="active"><a class="' + $(this).attr("class") + '"><button class="close closeTab" type="button" > <i class="fa fa-times-circle"></i></button>' + $('#' + $(this).attr("class")).attr("class") + '</a></li>');
            registerCloseEvent();
            currentTab = $('#' + $(this).attr("class")).attr("class");
        };
    });

    var planClasses = $("#createPlan,#editPlan,#allocatePlan");
    var plandivs = $("#planSchedule,#planview,#planassign");

    $(plandivs).hide();
    $("#planSchedule").show();

   
   

    

    $('[data-toggle="tooltip"]').tooltip();
    $('.form_datetime').datetimepicker({
        //language:  'fr',
        weekStart: 1,
        todayBtn: 1,
        autoclose: 1,
        todayHighlight: 1,
        startView: 2,
        forceParse: 0,
        showMeridian: 1
    });
    var chart = $('#containe').highcharts();
    $('#myModala').on('show.bs.modal', function () {
        $('#containe').css('visibility', 'hidden');
    });
    $('#myModala').on('shown.bs.modal', function () {
        $('#containe').css('visibility', 'initial');
        chart.reflow();
    });
});

window.onload = function () {
    $("#wrapper").show();
    setboxHeight();
    document.getElementById("loadingaa").style.display = "none";
    $.notify({
        title: 'WELCOME! ' + $('#usernameAccEdit').val(),
        message: 'We have kept your Dashboard neat and tidy while you were away. '
    }, {
        type: 'pastel-info',
        delay: 10000,
        template: '<div data-notify="container" class="col-xs-11 col-sm-3 alert alert-{0}" role="alert">' +
            '<span data-notify="title">{1}</span>' +
            '<span data-notify="message">{2}</span>' +
        '</div>'
    });
    addingMT();
    setInterval(function () {
        
    }, 10000);

    setInterval(function () {
        Load_Power_factor();
        Loadmainconsumption();
        Loadyearlypieconsumption();
        Loadyearlyconsumption();
        LoadHomegraph();
        Load_Current();
        createVoltageGraph();
     
    }, 25000);

    loadInventory();

}


/***************************** Inventory **********************/
function addInventory() {
    $.getJSON("http://localhost:23292/Service1.svc/setinventory?serial=" + document.getElementById("iMeterNew").value + "&typ=" + document.getElementById("iMeterType").value, function (data) {
        ShowMessage(data, 'Success');
        loadInventory();
    });
}

function loadInventory() {
    $.getJSON("http://localhost:23292/Service1.svc/getinventory", function (data) {
        if (data.length > 1) {
            var t = JSON.parse(data);
            var pee = [];
            for (i = 0; i < t.length; i++) {
                pee.push([t[i].serial,t[i].typ,t[i].time]);
            }

            var tab = $('#tableStore').DataTable();
            tab.destroy();
            var dataSet = pee;

            var table = $('#tableStore').DataTable({
                data: dataSet,
                dom: 'Bfrtip',
                select: true,
                buttons: ['copy', 'excel', 'print']
            });

            $('#tableStore').on('click', 'tr', function () {
                var row = table.row(this).data().toString();
                var result = row.split(',');
                document.getElementById("iMeter").value = result[0];
            });
        }
    });
}
/**************************************************************/

function processFile(e) {
    var file = e.target.result,
        results;
    if (file && file.length) {
        results = file.split("\n");
        var array = results[0].split(',');
        $('#name').val(array[0]);
        $('#age').val(results[1]);
        ShowMessage('File read complete','Info');
    }
}


function addingMT() {
    var t = null;

    $.getJSON('http://localhost:23292/Service1.svc/GetMuteData', function (data) {
        t = data;
        if (t.length > 2) {
            var tab = $('#exampaa').DataTable();
            tab.destroy();
            var dataSet = JSON.parse(t);
            //alert(dataSet[0].Serial);
            var gu = [[dataSet[0].Serial, dataSet[0].LastRead]];
            for (i = 1; i < dataSet.length; i++) {
                gu.push([dataSet[i].Serial, dataSet[i].LastRead]);
            }
            $('#exampaa').DataTable({
                data: gu,
                dom: 'Bfrtip',
                select: true,
                buttons: ['copy', 'excel', 'print']
            });
        }
    });
};



var uutt = 0;
function addingRows(plan, startt, endt, ldth, recth) {
    var t = $('#example').DataTable();
    if (uutt > 9) {
        t.rows().remove();
        uutt = 0;
    };
    t.row.add([
    plan,
    startt,
    endt,
    ldth,
    recth
    ]).draw(false);
    uutt++;
};

function addingRowsE(pee) {
    var tab = $('#etable').DataTable();
    tab.destroy();
    var dataSet = pee;

    $('#etable').DataTable({
        data: dataSet,
        dom: 'Bfrtip',
        select: true,
        buttons: ['copy', 'excel', 'print']
    });
};

function addingRowsOD(pee) {
    var tab = $('#instRtable').DataTable();
    tab.destroy();
    var dataSet = pee;

    $('#instRtable').DataTable({
        data: dataSet,
        dom: 'Bfrtip',
        select: true,
        buttons: ['copy', 'excel', 'print']
    });
};

function addingRowsRDEvents(pee) {
    var tab = $('#exampleEvents').DataTable();
    tab.destroy();
    var dataSet = pee;

    $('#exampleEvents').DataTable({
        data: dataSet,
        dom: 'Bfrtip',
        select: true,
        buttons: ['copy', 'excel', 'print']
    });
};

function addingRowsLP(pee) {
   
    var tab = $('#exampleLP').DataTable();
    tab.destroy();
    var dataSet = pee;

    $('#exampleLP').DataTable({
        data: dataSet,
        dom: 'Bfrtip',
        select: true,
        buttons: ['copy', 'excel', 'print', 'colvis']
    });
};

function addingaccessT(pee) {
    var tab = $('#accessT').DataTable();
    tab.destroy();
    var dataSet = pee;

    $('#accessT').DataTable({
        data: dataSet,
        dom: 'Bfrtip',
        select: true,
        buttons: ['copy', 'excel', 'print']
    });
};

function addingactionT(pee) {
    var tab = $('#actionT').DataTable();
    tab.destroy();
    var dataSet = pee;

    $('#actionT').DataTable({
        data: dataSet,
        dom: 'Bfrtip',
        select: true,
        buttons: ['copy', 'excel', 'print']
    });
};

function addingcommT(pee) {
    var tab = $('#commT').DataTable();
    tab.destroy();
    var dataSet = pee;

    $('#commT').DataTable({
        data: dataSet,
        dom: 'Bfrtip',
        select: true,
        buttons: ['copy', 'excel', 'print']
    });
};

function setboxHeight() {
    // Get the height of box1
    var box1height = $('#sidebar-wrapper').height();
    // Set box2 height equal to box1
    $('#dope').height(box1height - 2);
    $('#menu1').height(box1height - 2);
}



function showEload()
{
    $("#loadingE").show();
    removeRDEvents();
}

function hideEload() {
    $("#loadingE").hide();
}

var stopper;
var frmVAR;
var bars = $("#spinTariff,#syncHide,#authHide,#firmwareHide,#ondemandHide, #relayHide, #opHide, #spinSim, #spinGprs, #spinSync");

function fop(BAR, BAR1, Hider) {
    $(bars).fadeOut();
    $(document.getElementById(Hider)).fadeIn();
    clearInterval(stopper);
    var barr = document.getElementById(BAR);
    var barr1 = document.getElementById(BAR1);
    $(function () {
        var width = 0;
        var max = $(barr1).width();
        stopper = setInterval(function () {
            width += 10;
            $(barr).width(width);
            if (width > max) {
                width = 0;
                $(barr).width(0);
                stop(BAR, Hider);
            }
        }, 1000);
    });
};
function stop(BAR, Hider) {
    $(document.getElementById(BAR)).width(0);
    $(document.getElementById(Hider)).fadeOut();
    clearInterval(stopper);
};

function progressBarDoingStufffrm() {
    $("#firmwareHide").fadeIn();
    clearInterval(frmVAR);
    document.getElementById("demo").innerHTML = Date();
    $(function () {
        var width = 0;
        var max = $("#firmwareContainer").width();
        frmVAR = setInterval(function () {
            width += 1;
            $("#firmwareBar").width(width);
            if (width > max) {
                width = 0;
                $("#firmwareBar").width(0);
                stop();
            }
            document.getElementById("demo").innerHTML = Date();
        }, 1000);
    });
};
function stopfrm() {
    document.getElementById("demo").innerHTML = "";
    $("#firmwareBar").width(0);
    $("#firmwareHide").fadeOut();
    clearInterval(frmVAR);
};

function uploadStarted() {
    $get("imgDisplay").src = "preloader.gif";
};
function uploadComplete(sender, args) {
    var imgDisplay = $get("imgDisplay");
    imgDisplay.src = "ok.png";
    var img = new Image();
    img.onload = function () {
        imgDisplay.src = img.src;
    };
    img.src = "<%=ResolveUrl(UploadFolderPath) %>" + args.get_fileName();
};

function alertTimeout(wait) {
    setTimeout(function () {
        $('#alert_container').children('.alert:first-child').fadeOut();
    }, wait - 1000);
    setTimeout(function () {
        $('#alert_container').children('.alert:first-child').remove();
    }, wait);
}

var p = 0;
function addIt(message) {
    p += 1;
    $('#yop').append("<li class='list-group-item'><span style='color:yellow' class='badge'>" + p + "</span>" + message + "</li>");
};

function addIteva(message) {
    p += 1;
    $('#yop').append("<li class='list-group-item'><span style='color:yellow' class='badge'>" + p + "</span>" + message + "</li>");
};

function emptyaddit()
{
    $('#yop').empty();
    p = 0;
}

function fillGPRS(IP11, IP12, IP13, IP14, IP21, IP22, IP23, IP24, port1, port2, Apn, keepATime, gprsReconnectCount, gprsReconnectTime, gprsReconnectInterval, mode) {
    $("#tbIPprimary1").val(IP11);
    $("#tbIPprimary2").val(IP12);
    $("#tbIPprimary3").val(IP13);
    $("#tbIPprimary4").val(IP14);
    $("#IP2nd1").val(IP21);
    $("#IP2nd2").val(IP22);
    $("#IP2nd3").val(IP23);
    $("#IP2nd4").val(IP24);
    $("#tbportPrimary").val(port1);
    $("#tbportSecondary").val(port2);
    $("#APN").val(Apn);
    $("#keepAlivetime").val(keepATime);
    $("#reconnectCount").val(gprsReconnectCount);
    $("#tbreconnecttime").val(gprsReconnectTime);
    $("#tbreconnectInterval").val(gprsReconnectInterval);
    if (mode == 0) {
        $("#mode").val("Mode2");
    }
    else { $("#mode").val("Mode1"); }

    Spin(0, 'spinGprs');
};

function fillRelay(relayReconnectCount, relayReconnectTime, overloadThreshold1, overloadThreshold2, reconnectInterval, cycleReset, enabled) {
    $("#tbreconncet").val(relayReconnectCount);
    $("#tbreconnecttimeThreshold").val(relayReconnectTime);
    $("#th1").val(overloadThreshold1);
    $("#th2").val(overloadThreshold2);
    $("#reconnectInterval").val(reconnectInterval);
    $("#resetInterval").val(cycleReset);
    if (enabled == 1) {
        $("#RelayAutoModeEnableDisable").val("Enable");
    }
    else { $("#RelayAutoModeEnableDisable").val("Disable"); }

    $("#relayBar").width(0);
    $("#relayHide").fadeOut();
    clearInterval(stopper);
};

function fillSims(sim1, sim2, sim3, simSelf) {
    $("#tbSIM1").val(sim1);
    $("#tbSIM2").val(sim2);
    $("#tbSIM3").val(sim3);
    $("#simNumber").val(simSelf);

    Spin(0, 'spinSim');
};

function loaderQuick(state) {
    $("#loading").hide();
    if (state == 0) {
        $("#loading").hide();
    }
    else {
        $("#loading").show();
    };
}

function quickReadValues(kWh,kVARh,freq,time) {
    $("#loading").hide();
    document.getElementById("pkWh").innerHTML = kWh;
    document.getElementById("pkVARh").innerHTML = kVARh;
    document.getElementById("pfrequency").innerHTML = freq;
    document.getElementById("pTime").innerHTML = time;
};



function loaderOp(state,relstate) {
    $("#opGif").hide();
    if (state == 0) {
        $("#opGif").hide();
    }
    else {
        $("#opGif").show();
    };

    showRelayState(relstate);
}

function showRelayState(state) {
    if (state == 0) {
        $('input[name="my-checkbox"]').bootstrapSwitch('state', true, true);
        $('input[name="my-checkbox"]').bootstrapSwitch('toggleState', true, true);
    }

    if (state == 1) {
        $('input[name="my-checkbox"]').bootstrapSwitch('state', true, true);
    };
};

function teststuff(coming) {
    $('#korn').val(coming);
};

function ShowMessage(message, messagetype) {
    var cssclass,pastel,tuttle;
    switch (messagetype) {
        case 'Success':
            cssclass = 'alert-success'
            pastel = 'pastel-success'
            tuttle = '<i class="fa fa-check-square-o"></i> SUCCESS'
            break;
        case 'Error':
            cssclass = 'alert-danger'
            pastel = 'pastel-danger'
            tuttle = '<i class="fa fa-times-circle"></i> ERROR!'
            break;
        case 'Warning':
            cssclass = 'alert-warning'
            pastel = 'pastel-warning'
            tuttle = '<i class="fa fa-warning"></i> WARNING!'
            break;
        default:
            cssclass = 'alert-info'
            pastel = 'pastel-info'
            tuttle = '<i class="fa fa-info-circle"></i> INFO!'
    }
    //$('#alert_container').append('<div id="alert_div" style="margin: 0 0.5%; -webkit-box-shadow: 3px 4px 6px #999;" class="alert fade in ' + cssclass + '"><a href="#" class="close" data-dismiss="alert" aria-label="close">&times;</a><strong>' + messagetype + '!</strong> <span>' + message + '</span></div>');
    $.notify({
        title: tuttle,
        message: message
    }, {
        type: pastel,
        delay: 5000,
        template: '<div data-notify="container" class="col-xs-11 col-sm-3 alert alert-{0}" role="alert">' +
            '<span data-notify="title">{1}</span>' +
            '<span data-notify="message">{2}</span>' +
        '</div>'
    });

    if (cssclass == 'alert-warning') {
        addIt(message);
    }
    //alertTimeout(10000);
};

var mens = $("#dgremover,#dgcreator,#dgeditGroup");

function dgcreateshow() {
    $(mens).hide();
    $('#dgcreator').show();
};

function dgremoveshow() {
    $(mens).hide();
    $('#dgremover').show();
};

function dgeditshow() {
    $(mens).hide();
    $('#dgeditGroup').show();
};

function EditWell(rowID) {
    $('#EditWell').show();
    $('#editGroupFieldName').val(rowID);
};

function addItRF(message) {
    $('#RFAddList').append("<li class='list-group-item'><span style='color:lightgreen' class='badge'><i class='fa fa-check'></i></span>" + message + "</li>");
};

function addItFD(id,rssi,connection,lastcomm) {
    $('#FDAddList').append("<li class='list-group-item'><span style='color:lightgreen' class='badge'><i class='fa fa-check'></i></span>&nbsp<i class='fa fa-fax'></i>&nbspDevice ID:" + id + "&nbsp&nbsp<i class='fa fa-signal'></i>&nbspSignal Strength:" + rssi + "&nbsp&nbsp<i class='fa fa-mobile'></i>&nbspCarrier Technology:" + connection + "&nbsp&nbsp<i class='fa fa-clock-o'></i>&nbspLast Communication:" + lastcomm + "</li>");
};


function addItFDL(id,rssi,connection,lastcomm) {
    $('#FDLAddList').append("<li class='list-group-item'><span style='color:lightgreen' class='badge'><i class='fa fa-check'></i></span>&nbsp<i class='fa fa-fax'></i>&nbspDevice ID:" + id + "&nbsp&nbsp<i class='fa fa-signal'></i>&nbspSignal Strength:" + rssi + "&nbsp&nbsp<i class='fa fa-mobile'></i>&nbspCarrier Technology:" + connection + "&nbsp&nbsp<i class='fa fa-clock-o'></i>&nbspLast Communication:" + lastcomm + "</li>");
};

function SItFD() {
    $('#FDAddList').append("<li class='list-group-item'><span style='color:lightgreen' class='badge'><i class='fa fa-refresh fa-spin'></i></span>Searching...</li>");
};

function removeRFList()
{
    $('#RFAddList').empty();
}

function removeFDList() {
    $('#FDAddList').empty();
    $('#FDLAddList').empty();
}

function uploadComplete1(sender) {
    $get("<%=lblMesg.ClientID%>").innerHTML = "File Uploaded Successfully";
}

function uploadError1(sender) {
    $get("<%=lblMesg.ClientID%>").innerHTML = "File upload failed.";
}

function fillParents(groiss) {
    
var select = document.getElementById("DropDownListParentNode");
var options = JSON.parse(groiss);
for (var i = 0; i < options.length; i++) {
    var opt = options[i];
    var el = document.createElement("option");
    el.textContent = opt;
    el.value = opt;
    select.appendChild(el);
}
};

function EditWellNo() {
    $('#EditWell').hide();
};



//function Load11(serial) {
//    document.getElementById("hero-graph").innerHTML = "";

//    t = null;
//    $.getJSON("http://localhost:23292/Service1.svc/getpf?serial=" + serial, function (data) {
//        t = JSON.parse(data);
//        var tax_data = t
//        // Morris Area Chart
//        Morris.Line({
//            element: 'hero-graph',
//            data: tax_data,
//            xkey: 'Time',
//            ykeys: ['average_pf'],
//            labels: ['Power Factor'],
//            lineWidth: 2,
//            hideHover: 'auto',
//            lineColors: ["#2ECC71"]
//        });
//    });


//}

//function Load12(serial) {
//    document.getElementById("hero-area2").innerHTML = "";

//    t = null;
//    $.getJSON("http://localhost:23292/Service1.svc/tedata3?serial=" + serial, function (data) {
//        t = JSON.parse(data);
//        var tax_data = t
//        // Morris Bar Chart
//        Morris.Bar({
//            element: 'hero-area2',
//            data: tax_data,
//            xkey: 'Time',
//            ykeys: ['Energy'],
//            labels: ['Power(kWh)'],
//            hideHover: 'auto',
//            barColors: ["#2fa9c4"]
//        });
//    });
//}

//function Load13(serial) {
//    document.getElementById("hero-area").innerHTML = "";

//    t = null;
//    $.getJSON("http://localhost:23292/Service1.svc/tedata4?serial=" + serial, function (data) {
//        t = JSON.parse(data);
//        var tax_data = t
//        // Morris Line Chart
//        Morris.Line({
//            element: 'hero-area',
//            data: tax_data,
//            xkey: 'Time',
//            ykeys: ['V1', 'V2', 'V3'],
//            labels: ['Voltage Phase A', 'Voltage B', 'Voltage C'],
//            lineWidth: 2,
//            hideHover: 'auto',
//            lineColors: ["#FF0000", "#00FF00", "#0000FF"]

//        });
//    });
//}

//function DailyCurrent(serial) {
//    document.getElementById("hero-graph22").innerHTML = "";

//    t = null;
//    $.getJSON("http://localhost:23292/Service1.svc/currentreading?serial=" + serial, function (data) {
//        t = JSON.parse(data);
//        var tax_data = t
//        // Morris Line Chart
//        Morris.Line({
//            element: 'hero-graph22',
//            data: tax_data,
//            xkey: 'Time',
//            ykeys: ['current_a', 'current_b', 'current_c'],
//            labels: ['Current Phase A', 'Current B', 'Current C'],
//            lineWidth: 2,
//            hideHover: 'auto',
//            lineColors: ["#FF0000", "#00FF00", "#0000FF"]

//        });
//    });
//}



function Loadmainconsumption() {
    document.getElementById("consumption").innerHTML = "";
    t = null;
    $.getJSON("http://localhost:23292/Service1.svc/tedata", function (data) {
        t = JSON.parse(data.replace(/\\/g, ''));
        var dat = [];
        for (var i = 0; i < t.length; i++) {
            var row = t[i];
            var dateString = row.date;
            var date = new Date(dateString);
            dat.push({ name: row.department, y: parseFloat(row.kWh), date:date});
        }

        Highcharts.setOptions({
            accessibility: {
                enabled: false
            }
        });
        Highcharts.chart('consumption', {
            chart: {
                type: 'column'
            },
            title: {
                text: 'Department Consumption'
            },
            xAxis: {
                categories: dat.map(function (d) { return d.name; }),
                title: {
                    text: 'Department'
                }
            },
            yAxis: {
                min: 0,
                title: {
                    text: 'kWh'
                }
            },
            series: [{
                name: 'Consumption this month',
                data: dat.map(function (d) { return d.y; }),
                zones: [
                    {
                        value: 100000, // Threshold for green color (Normal)
                        color: 'green'
                    },
                    {
                        value: 200000, // Threshold for orange color (Alarm)
                        color: 'orange'
                    },
                    {
                        color: 'red' // Overflow color
                    }
                ],
                tooltip: {
                    valueDecimals: 2 // Adjust the decimal places for tooltip values as desired
                }
            }],
            rangeSelector: {
                enabled: false // Disable the range selector
            },
            navigator: {
                enabled: true,
                
            }
        });
    });
}

function Loadyearlyconsumption() {
    document.getElementById("consumption_year_chart").innerHTML = "";
    t = null;
    $.getJSON("http://localhost:23292/Service1.svc/year_consumption", function (data) {
        t = JSON.parse(data.replace(/\\/g, ''));
        var dat = [];
        for (var i = 0; i < t.length; i++) {
            var row = t[i];
            dat.push({ name: row.department, y: parseFloat(row.kWh) });
        }

        Highcharts.setOptions({
            accessibility: {
                enabled: false
            }
        });
        Highcharts.chart('consumption_year_chart', {
            chart: {
                type: 'column'
            },
            title: {
                text: 'Department Consumption'
            },
            xAxis: {
                categories: dat.map(function (d) { return d.name; }),
                title: {
                    text: 'Department'
                }
            },
            yAxis: {
                min: 0,
                title: {
                    text: 'kWh'
                }
            },
            series: [{
                name: '',
                data: dat.map(function (d) { return d.y; }),


            }],

            navigator: {
                enabled: true
            }

        });
    });
}
function LoadHomegraph() {
    document.getElementById("consumption_pie_chart").innerHTML = "";
    t = null;
    $.getJSON("http://localhost:23292/Service1.svc/tedata", function (data) {
        t = JSON.parse(data.replace(/\\/g, ''));
        var totalUnits = 0;
        var dat = [];
        for (var i = 0; i < t.length; i++) {
            var row = t[i];
            totalUnits += parseFloat(row.kWh);
            dat.push({ name: row.department, y: parseFloat(row.kWh), consumption: parseFloat(row.kWh).toFixed(2) });
        }

        Highcharts.setOptions({
            accessibility: {
                enabled: false
            }
        });
        Highcharts.chart('consumption_pie_chart', {
            chart: {
                type: 'pie'
            },
            title: {
                text: 'Department consumption'
            },
            tooltip: {
                pointFormat: '{series.name}: <b>{point.percentage:.1f}%</b>'
            },
            plotOptions: {
                pie: {
                    allowPointSelect: true,
                    cursor: 'pointer',
                    dataLabels: {
                        enabled: true,
                        format: '<b>{point.name}</b>: {point.percentage:.1f}%',
                        style: {
            fontSize: '14px' // Adjust the font size as desired
        }
                    }
                }
            },
            series: [{
                name: 'Department',
                data: dat.map(function (d) {
                    return {
                        name: d.name,
                        y: d.y,
                        percentage: (d.y / totalUnits) * 100
                    };
                }),
                zones: [
                    {
                        value: 100000, // Threshold for green color (Normal)
                        color: 'green'
                    },
                    {
                        value: 200000, // Threshold for orange color (Alarm)
                        color: 'orange'
                    },
                    {
                        color: 'red' // Overflow color
                    }
                ],
                tooltip: {
                    valueDecimals: 2 // Adjust the decimal places for tooltip values as desired
                }
            }]
        });
    });
}
function Loadyearlypieconsumption() {
    document.getElementById("consumption_year_pie_chart").innerHTML = "";
    t = null;
    $.getJSON("http://localhost:23292/Service1.svc/year_consumption", function (data) {
        t = JSON.parse(data.replace(/\\/g, ''));
        var totalUnits = 0;
        var dat = [];
        for (var i = 0; i < t.length; i++) {
            var row = t[i];
            totalUnits += parseFloat(row.kWh);
            dat.push({ name: row.department, y: parseFloat(row.kWh), consumption: parseFloat(row.kWh).toFixed(2) });
        }

        Highcharts.setOptions({
            accessibility: {
                enabled: false
            }
        });
        Highcharts.chart('consumption_year_pie_chart', {
            chart: {
                type: 'pie'
            },
            title: {
                text: 'Department consumption'
            },
            tooltip: {
                pointFormat: '{series.name}: <b>{point.percentage:.1f}%</b>'
            },
            plotOptions: {
                pie: {
                    allowPointSelect: true,
                    cursor: 'pointer',
                    dataLabels: {
                        enabled: true,
                        format: '<b>{point.name}</b>: {point.percentage:.1f}%',
                        style: {
                            fontSize: '14px' // Adjust the font size as desired
                        }
                    }
                }
            },
            series: [{
                name: 'Department',
                data: dat.map(function (d) {
                    return {
                        name: d.name,
                        y: d.y,
                        percentage: (d.y / totalUnits) * 100
                    };
                })
            }],
            tooltip: {
                valueDecimals: 2 // Adjust the decimal places for tooltip values as desired
            }
        });
    });
}


function createVoltageGraph() {
    $.getJSON("http://localhost:23292/Service1.svc/GetVoltagesofUnits", function (data) {
        var parsedData = JSON.parse(data);
        var voltageData = {};
        var units = [];

        for (var i = 0; i < parsedData.length; i++) {
            var unit = parsedData[i].Meter_no;
            var voltagePhase1 = parseFloat(parsedData[i].Voltage_phase1);
            var voltagePhase2 = parseFloat(parsedData[i].Voltage_phase2);
            var voltagePhase3 = parseFloat(parsedData[i].Voltage_phase3);
            var timestamp = new Date(parsedData[i].Time_Stamp);

            if (!voltageData[unit]) {
                voltageData[unit] = {
                    phase1Values: [],
                    phase2Values: [],
                    phase3Values: [],
                    timestamps: []
                };

                units.push(unit); // Add the unit to the units array
            }

            voltageData[unit].phase1Values.push(voltagePhase1);
            voltageData[unit].phase2Values.push(voltagePhase2);
            voltageData[unit].phase3Values.push(voltagePhase3);
            voltageData[unit].timestamps.push(timestamp);
        }


        var seriesData = [];

        for (var unit in voltageData) {
            if (voltageData.hasOwnProperty(unit)) {
                seriesData.push({
                    name: 'Meter Number :' + unit + ' Voltage Phase 1',
                    data: voltageData[unit].phase1Values.map(function (value, index) {
                        return [voltageData[unit].timestamps[index].getTime(), value];
                    })
                });
                seriesData.push({
                    name: 'Meter Number :' + unit + ' Voltage Phase 2',
                    data: voltageData[unit].phase2Values.map(function (value, index) {
                        return [voltageData[unit].timestamps[index].getTime(), value];
                    })
                });
                seriesData.push({
                    name: 'Meter Number :' + unit + ' Voltage Phase 3',
                    data: voltageData[unit].phase3Values.map(function (value, index) {
                        return [voltageData[unit].timestamps[index].getTime(), value];
                    })
                });
            }
        }


        var chartType = 'column';

        // Function to toggle chart type
        function toggleChartType() {
            if (chartType === 'column') {
                chartType = 'line';
            } else {
                chartType = 'column';
            }

            // Update the chart with the new chart type
            var chart = Highcharts.chart('VoltageGraph', {
                chart: {
                    type: chartType,
                },
                title: {
                    text: 'Voltage Readings'
                },
                xAxis: {
                    type: 'datetime', // Use datetime type for x-axis
                    title: {
                        text: 'Timestamp'
                    }
                },
                yAxis: {
                    title: {
                        text: 'Voltage'
                    }
                },
                navigator: {
                    enabled: true
                },
                rangeSelector: {
                    enabled: true,
                    buttons: [
                      {
                          type: 'minute',
                          count: 1,
                          text: '1m'
                      },
                      {
                          type: 'hour',
                          count: 1,
                          text: '1h'
                      },
                      {
                          type: 'day',
                          count: 1,
                          text: '1d'
                      },
                      {
                          type: 'week',
                          count: 1,
                          text: '1w'
                      },
                      {
                          type: 'month',
                          count: 1,
                          text: '1m'
                      },
                      {
                          type: 'month',
                          count: 6,
                          text: '6m'
                      },
                      {
                          type: 'year',
                          count: 1,
                          text: '1y'
                      },
                      {
                          type: 'all',
                          text: 'All'
                      }
                    ],
                    selected: 0
                },
                series: seriesData
            });
        }

        // Create the initial chart
        var chart = Highcharts.chart('VoltageGraph', {
            chart: {
                type: chartType,
            },
            title: {
                text: 'Voltage Readings'
            },
            xAxis: {
                type: 'datetime', // Use datetime type for x-axis
                title: {
                    text: 'Timestamp'
                }
            },
            yAxis: {
                title: {
                    text: 'Voltage'
                }
            },
            navigator: {
                enabled: true
            },
            rangeSelector: {
                enabled: true,
                buttons: [
                  {
                      type: 'minute',
                      count: 1,
                      text: '1m'
                  },
                  {
                      type: 'hour',
                      count: 1,
                      text: '1h'
                  },
                  {
                      type: 'day',
                      count: 1,
                      text: '1d'
                  },
                  {
                      type: 'week',
                      count: 1,
                      text: '1w'
                  },
                  {
                      type: 'month',
                      count: 1,
                      text: '1m'
                  },
                  {
                      type: 'month',
                      count: 6,
                      text: '6m'
                  },
                  {
                      type: 'year',
                      count: 1,
                      text: '1y'
                  },
                  {
                      type: 'all',
                      text: 'All'
                  }
                ],
                selected: 0
            },
            series: seriesData
        });

        document.getElementById('chartToggleBtn1').addEventListener('click', function (event) {
            event.preventDefault(); // Prevent default form submission behavior

            toggleChartType(); // Call the toggleChartType function
        });
    });
}



function Load_Current() {
    $.getJSON("http://localhost:23292/Service1.svc/GetCurrent", function (data) {
        var parsedData = JSON.parse(data);
        var CurrentData = {};
        var units = [];

        for (var i = 0; i < parsedData.length; i++) {
            var unit = parsedData[i].Meter_no;
            var CurrentPhase1 = parseFloat(parsedData[i].Current_phase_A);
            var CurrentPhase2 = parseFloat(parsedData[i].Current_phase_B);
            var CurrentPhase3 = parseFloat(parsedData[i].Current_phase_C);
            var timestamp = new Date(parsedData[i].Time_Stamp);

            if (!CurrentData[unit]) {
                CurrentData[unit] = {
                    phase1Values: [],
                    phase2Values: [],
                    phase3Values: [],
                    timestamps: []
                };

                units.push(unit); // Add the unit to the units array
            }

            CurrentData[unit].phase1Values.push(CurrentPhase1);
            CurrentData[unit].phase2Values.push(CurrentPhase2);
            CurrentData[unit].phase3Values.push(CurrentPhase3);
            CurrentData[unit].timestamps.push(timestamp);
        }

        var seriesData = [];

        for (var unit in CurrentData) {
            if (CurrentData.hasOwnProperty(unit)) {
                seriesData.push({
                    name: 'Meter Number :' + unit + ' Current Phase A',
                    data: CurrentData[unit].phase1Values.map(function (value, index) {
                        return [CurrentData[unit].timestamps[index].getTime(), value];
                    })
                });
                seriesData.push({
                    name: 'Meter Number :' + unit + ' Current Phase B',
                    data: CurrentData[unit].phase2Values.map(function (value, index) {
                        return [CurrentData[unit].timestamps[index].getTime(), value];
                    })
                });
                seriesData.push({
                    name: 'Meter Number :' + unit + ' Current Phase C',
                    data: CurrentData[unit].phase3Values.map(function (value, index) {
                        return [CurrentData[unit].timestamps[index].getTime(), value];
                    })
                });
            }
        }

        var chartType = 'column';

        // Function to toggle chart type
        function toggleChartType() {
            if (chartType === 'column') {
                chartType = 'line';
            } else {
                chartType = 'column';
            }

            // Update the chart with the new chart type
            var chart = Highcharts.chart('CurrentGraph', {
                chart: {
                    type: chartType,
                },
                title: {
                    text: 'Current Readings'
                },
                xAxis: {
                    type: 'datetime', // Use datetime type for x-axis
                    title: {
                        text: 'Timestamp'
                    }
                },
                yAxis: {
                    title: {
                        text: 'Current'
                    }
                },
                navigator: {
                    enabled: true
                },
                rangeSelector: {
                    enabled: true,
                    buttons: [
                      {
                          type: 'minute',
                          count: 1,
                          text: '1m'
                      },
                      {
                          type: 'hour',
                          count: 1,
                          text: '1h'
                      },
                      {
                          type: 'day',
                          count: 1,
                          text: '1d'
                      },
                      {
                          type: 'week',
                          count: 1,
                          text: '1w'
                      },
                      {
                          type: 'month',
                          count: 1,
                          text: '1m'
                      },
                      {
                          type: 'month',
                          count: 6,
                          text: '6m'
                      },
                      {
                          type: 'year',
                          count: 1,
                          text: '1y'
                      },
                      {
                          type: 'all',
                          text: 'All'
                      }
                    ],
                    selected: 0
                },
                plotOptions: {
                    series: {
                        animation: {
                            duration: 2000, // Adjust the animation duration as desired
                        },
                        lineWidth: 1, // Adjust the line width as desired
                        marker: {
                            enabled: true, // Enable markers on the line
                            radius: 2, // Adjust the marker radius as desired
                            symbol: 'circle' // Use a circle as the marker symbol
                        },
                        states: {
                            hover: {
                                lineWidthPlus: 0 // Disable line width increase on hover
                            }
                        }
                    }
                },
                series: seriesData
            });
        }

        // Create the initial chart
        var chart = Highcharts.chart('CurrentGraph', {
            chart: {
                type: chartType,
            },
            title: {
                text: 'Current Readings'
            },
            xAxis: {
                type: 'datetime', // Use datetime type for x-axis
                title: {
                    text: 'Timestamp'
                }
            },
            yAxis: {
                title: {
                    text: 'Current'
                }
            },
            navigator: {
                enabled: true
            },
            rangeSelector: {
                enabled: true,
                buttons: [
                  {
                      type: 'minute',
                      count: 1,
                      text: '1m'
                  },
                  {
                      type: 'hour',
                      count: 1,
                      text: '1h'
                  },
                  {
                      type: 'day',
                      count: 1,
                      text: '1d'
                  },
                  {
                      type: 'week',
                      count: 1,
                      text: '1w'
                  },
                  {
                      type: 'month',
                      count: 1,
                      text: '1m'
                  },
                  {
                      type: 'month',
                      count: 6,
                      text: '6m'
                  },
                  {
                      type: 'year',
                      count: 1,
                      text: '1y'
                  },
                  {
                      type: 'all',
                      text: 'All'
                  }
                ],
                selected: 0
            },
            series: seriesData
        });

        document.getElementById('chartToggleBtn').addEventListener('click', function (event) {
            event.preventDefault(); // Prevent default form submission behavior

            toggleChartType(); // Call the toggleChartType function
        });
    });
}


function Load_Power_factor() {
    $.getJSON("http://localhost:23292/Service1.svc/powerfactor", function (data) {
        var parsedData = JSON.parse(data);
        var pfdata = {};
        var units = [];

        for (var i = 0; i < parsedData.length; i++) {
            var unit = parsedData[i].Meter_no;
            var pflist = parseFloat(parsedData[i].pf);
            var timestamp = new Date(parsedData[i].Time_Stamp);

            if (!pfdata[unit]) {
                pfdata[unit] = {
                    phase1Values: [],
                    timestamps: []
                };

                units.push(unit); // Add the unit to the units array
            }

            pfdata[unit].phase1Values.push(pflist);
            pfdata[unit].timestamps.push(timestamp);
        }

        
        var seriesData = [];

        for (var unit in pfdata) {
            if (pfdata.hasOwnProperty(unit)) {
                seriesData.push({
                    name: 'Meter Number :' + unit,
                    data: pfdata[unit].phase1Values.map(function (value, index) {
                        return [pfdata[unit].timestamps[index].getTime(), value];
                    })
                });

            }
        }

        var chartType = 'column';

        // Function to toggle chart type
        function toggleChartType() {
            if (chartType === 'column') {
                chartType = 'line';
            } else {
                chartType = 'column';
            }

            // Update the chart with the new chart type
            var chart = Highcharts.chart('pfchart', {
                chart: {
                    type: chartType,
                },
                title: {
                    text: 'Power Factor Readings'
                },
                xAxis: {
                    type: 'datetime', // Use datetime type for x-axis
                    title: {
                        text: 'Timestamp'
                    }
                },
                yAxis: {
                    title: {
                        text: 'Power Factor Reading'
                    }
                },
                navigator: {
                    enabled: true
                },
                rangeSelector: {
                    enabled: true,
                    buttons: [
                      {
                          type: 'minute',
                          count: 1,
                          text: '1m'
                      },
                      {
                          type: 'hour',
                          count: 1,
                          text: '1h'
                      },
                      {
                          type: 'day',
                          count: 1,
                          text: '1d'
                      },
                      {
                          type: 'week',
                          count: 1,
                          text: '1w'
                      },
                      {
                          type: 'month',
                          count: 1,
                          text: '1m'
                      },
                      {
                          type: 'month',
                          count: 6,
                          text: '6m'
                      },
                      {
                          type: 'year',
                          count: 1,
                          text: '1y'
                      },
                      {
                          type: 'all',
                          text: 'All'
                      }
                    ],
                    selected: 0
                },
                plotOptions: {
                    series: {
                        animation: {
                            duration: 2000, // Adjust the animation duration as desired
                        },
                        lineWidth: 1, // Adjust the line width as desired
                        marker: {
                            enabled: true, // Enable markers on the line
                            radius: 2, // Adjust the marker radius as desired
                            symbol: 'circle' // Use a circle as the marker symbol
                        },
                        states: {
                            hover: {
                                lineWidthPlus: 0 // Disable line width increase on hover
                            }
                        }
                    }
                },
                series: seriesData
            });
        }

        // Create the initial chart
        var chart = Highcharts.chart('pfchart', {
            chart: {
                type: chartType,
            },
            title: {
                text: 'Power Factor Readings'
            },
            xAxis: {
                type: 'datetime', // Use datetime type for x-axis
                title: {
                    text: 'Timestamp'
                }
            },
            yAxis: {
                title: {
                    text: 'Power Factor'
                }
            },
            navigator: {
                enabled: true
            },
            rangeSelector: {
                enabled: true,
                buttons: [
                  {
                      type: 'minute',
                      count: 1,
                      text: '1m'
                  },
                  {
                      type: 'hour',
                      count: 1,
                      text: '1h'
                  },
                  {
                      type: 'day',
                      count: 1,
                      text: '1d'
                  },
                  {
                      type: 'week',
                      count: 1,
                      text: '1w'
                  },
                  {
                      type: 'month',
                      count: 1,
                      text: '1m'
                  },
                  {
                      type: 'month',
                      count: 6,
                      text: '6m'
                  },
                  {
                      type: 'year',
                      count: 1,
                      text: '1y'
                  },
                  {
                      type: 'all',
                      text: 'All'
                  }
                ],
                selected: 0
            },
            series: seriesData
        });

	
	
        document.getElementById('chartToggleBtn2').addEventListener('click', function (event) {
            event.preventDefault(); // Prevent default form submission behavior

            toggleChartType(); // Call the toggleChartType function
        });
    });
}


