<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DemanderEq.aspx.cs" Inherits="PFE.Admin.Visiteur.DemanderEq" %>
<!doctype html>
<!--[if lt IE 7]>      <html class="no-js lt-ie9 lt-ie8 lt-ie7" lang=""> <![endif]-->
<!--[if IE 7]>         <html class="no-js lt-ie9 lt-ie8" lang=""> <![endif]-->
<!--[if IE 8]>         <html class="no-js lt-ie9" lang=""> <![endif]-->
<!--[if gt IE 8]><!--> <html class="no-js" lang=""> <!--<![endif]-->
<head>
    <meta charset="utf-8">
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <title>BMI facturation</title>
    <meta name="description" content="Ela Admin - HTML5 Admin Template">
    <meta name="viewport" content="width=device-width, initial-scale=1">

    <link rel="apple-touch-icon" href="https://i.imgur.com/QRAUqs9.png">
    <link rel="shortcut icon" href="../../images/logo_bmi.png">

    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/normalize.css@8.0.0/normalize.min.css">
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap@4.1.3/dist/css/bootstrap.min.css">
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/font-awesome@4.7.0/css/font-awesome.min.css">
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/gh/lykmapipo/themify-icons@0.1.2/css/themify-icons.css">
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/pixeden-stroke-7-icon@1.2.3/pe-icon-7-stroke/dist/pe-icon-7-stroke.min.css">
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/flag-icon-css/3.2.0/css/flag-icon.min.css">
    <link rel="stylesheet" href="../../assets/css/cs-skin-elastic.css">
    <link rel="stylesheet" href="../../assets/css/style.css">

    <link href='https://fonts.googleapis.com/css?family=Open+Sans:400,600,700,800' rel='stylesheet' type='text/css'>
    <style>
    .customer_custom1 {
         display: none;
     }
    .notif{
        color: #696969;
    }
    .seeMore{
        color:#808000;
    }
    .notiftitle{
        font-size:17px;
        color:#9ACD32;
        font-weight:bold;
    }
    .right-panel .navbar-brand img {
    max-width: 63px;
    }
    .right-panel header.header {
    padding: 0 10px;
    }
     </style>
    <!-- <script type="text/javascript" src="https://cdn.jsdelivr.net/html5shiv/3.7.3/html5shiv.min.js"></script> -->

</head>
<body>
    <!-- Left Panel -->
     <form runat="server" class="form-horizontal">

    <aside id="left-panel" class="left-panel">
        <nav class="navbar navbar-expand-sm navbar-default">
            <div id="main-menu" class="main-menu collapse navbar-collapse">
                <ul class="nav navbar-nav">
                    <li class="active">
                        <a href="../index.aspx"><i class="menu-icon fa fa-laptop"></i>Page de connexion </a>
                    </li>
                    <li class="menu-item-has-children dropdown">
                        <a href="#" class="dropdown-toggle" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false"> <i class="menu-icon fa fa-truck"></i>Equipements</a>
                        <ul class="sub-menu children dropdown-menu">                           
                            <li><i class="fa fa-list"></i><a href="ConsulterEq.aspx">Consulter</a></li>
                            <li><i class="fa fa-list-alt"></i><a href="DemandeEq.aspx">Consulter les demamdes de location</a></li>
                        </ul>
                    </li>
                </ul>
            </div><!-- /.navbar-collapse -->
        </nav>
    </aside>

    <!-- Left Panel -->

    <!-- Right Panel -->

    <div id="right-panel" class="right-panel">

        <!-- Header-->
        <header id="header" class="header">
            <div class="top-left">
                <div class="navbar-header">
                    <a class="navbar-brand" href="../Dashboard.aspx">
                        <img src="../images/logo_bmi.png" alt="Logo">
                    </a>
                    <a id="menuToggle" class="menutoggle"><i class="fa fa-bars"></i></a>
                </div>
            </div>
            <div class="top-right">
                <div class="header-menu">
                </div>
            </div>
        </header><!-- /header -->
        <!-- Header-->

        <div class="breadcrumbs">
            <div class="breadcrumbs-inner">
                <div class="row m-0">
                    <div class="col-sm-4">
                        <div class="page-header float-left">
                            <div class="page-title">
                                <h1>Menu</h1>
                            </div>
                        </div>
                    </div>
                    <div class="col-sm-8">
                        <div class="page-header float-right">
                            <div class="page-title">
                                <ol class="breadcrumb text-right">
                                    <li><a href="../Dashboard.aspx">Menu</a></li>
                                    <li><a href="ConsulterUser.aspx">Utilisateurs</a></li>
                                    <li class="active">Modifier</li>
                                </ol>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

      <div class="content">
          <div class="animated fadeIn">
                <div class="row">
                    <div class="col-lg-12">
                        <div class="card">
                            <div class="card-header">
                                <strong>Demander Location</strong>
                            </div>
                            <div class="card-body card-block">
                                <div class="row form-group">
                                     <div class="col-lg-6">
                                         <div class="row">
                                             <div class="col-lg-4"><asp:Label ID="Label4" runat="server" CssClass="form-control-label">Nom:</asp:Label></div>
                                             <div class="col-lg-6"><asp:TextBox ID="nom" runat="server" value="user" CssClass="form-control"></asp:TextBox></div>
                                         </div>
                                     </div>
                                    <div class="col-lg-6">
                                         <div class="row">
                                             <div class="col-lg-4"><asp:Label ID="Label7" runat="server" CssClass="form-control-label">Prénom:</asp:Label></div>
                                             <div class="col-lg-6"><asp:TextBox ID="prenom" runat="server" value="user" CssClass="form-control"></asp:TextBox></div>
                                        </div>
                                    </div>
                                </div>
                                <hr />
                                <div class="row form-group">
                                     <div class="col-lg-6">
                                         <div class="row">
                                             <div class="col-lg-4"><asp:Label ID="Label2" runat="server" CssClass="form-control-label">CIN:</asp:Label></div>
                                             <div class="col-lg-6"><asp:TextBox ID="cin" runat="server" CssClass="form-control" MaxLength="8"></asp:TextBox></div><br />
                                         </div>
                                     </div>
                                    <div class="col-lg-6">
                                         <div class="row">
                                             <div class="col-lg-4"><asp:Label ID="Label5" runat="server" CssClass="form-control-label">Adresse</asp:Label></div>
                                             <div class="col-lg-6"><asp:TextBox ID="adresse" runat="server" CssClass="form-control"></asp:TextBox></div>
                                        </div>
                                    </div>
                                </div>
                                <hr />
                                <div class="row form-group">
                                     <div class="col-lg-6">
                                         <div class="row">
                                             <div class="col-lg-4"><asp:Label ID="Label1" runat="server" CssClass="form-control-label">Adresse email:</asp:Label></div>
                                             <div class="col-lg-6"><asp:TextBox ID="mail" runat="server" CssClass="form-control"></asp:TextBox></div><br />
                                         </div>
                                     </div>
                                    <div class="col-lg-6">
                                         <div class="row">
                                             <div class="col-lg-4"><asp:Label ID="Label6" runat="server" CssClass="form-control-label">Téléphone:</asp:Label></div>
                                             <div class="col-lg-6"><asp:TextBox ID="Tel" runat="server" TextMode="Number" CssClass="form-control" MaxLength="8"></asp:TextBox></div>
                                        </div>
                                    </div>
                                </div>
                                <hr />
                                <div class="row form-group">
                                     <div class="col-lg-6">
                                         <div class="row">
                                             <div class="col-lg-4"><asp:Label ID="Label3" runat="server" CssClass="form-control-label">Code équipement:</asp:Label></div>
                                             <div class="col-lg-6"><asp:TextBox ID="CodcouEQ" runat="server" value="user" CssClass="form-control"></asp:TextBox></div><br />
                                         </div>
                                     </div>
                                    <div class="col-lg-6">
                                         <div class="row form-group">
                                             <div class="col-lg-4"><asp:Label ID="select" CssClass=" form-control-label" runat="server">localisation du chantier:</asp:Label></div>
                                             <div class="col-lg-6"><asp:TextBox ID="chantier"  CssClass="form-control" runat="server"/></div>
                                        </div>
                                    </div>
                                </div>
                                <div class="row form-group">
                                     <div class="col-lg-6">
                                         <div class="row">
                                             <div class="col-lg-4"><asp:Label ID="Label8" runat="server" CssClass="form-control-label">Date début de location</asp:Label></div>
                                             <div class="col-lg-6"><asp:TextBox ID="dtDebut" runat="server" value="user" CssClass="form-control"></asp:TextBox></div><br />
                                         </div>
                                     </div>
                                    <div class="col-lg-6">
                                         <div class="row form-group">
                                             <div class="col-lg-4"><asp:Label ID="Label9" CssClass=" form-control-label" runat="server">Date fin de location:</asp:Label></div>
                                             <div class="col-lg-6"><asp:TextBox ID="dtFin"  CssClass="form-control" runat="server"/></div>
                                        </div>
                                    </div>
                                </div>
                                <div id="mydiv" runat="server" class="sufee-alert alert with-close alert-danger alert-dismissible fade show">
                                    <span class="badge badge-pill badge-danger">Info</span><asp:Label ID="errormsg" Text="Vous devez remplir tous les champs" runat="server"></asp:Label> 
                                    <button type="button" class="close" data-dismiss="alert" aria-label="Close">
                                    <span aria-hidden="true">&times;</span>
                                    </button>
                                </div>
                                    <div class="float-right">
                                                    <asp:Button ID="BTNenvoyer" runat="server" BorderStyle="None" Text="Envoyer" class="btn btn-success" OnClick="BTNenvoyer_Click" />
                                                    <asp:Button ID="BTNannuler" runat="server" BorderStyle="None" Text="Annuler" class="btn btn-danger" OnClick="BTNannuler_Click" />
                                    </div>
                                </div>
                            </div>
                                    <asp:TextBox runat="server" ID="query" CssClass="customer_custom1" />
                                    <asp:TextBox runat="server" ID="cnx" CssClass="customer_custom1" />
                        </div>
                   </div>
            </div><!-- .animated -->
        </div><!-- .content -->

    <div class="clearfix"></div>
</div> <!-- Right Panel -->
<!-- Scripts -->
<script src="https://cdn.jsdelivr.net/npm/jquery@2.2.4/dist/jquery.min.js"></script>
<script src="https://cdn.jsdelivr.net/npm/popper.js@1.14.4/dist/umd/popper.min.js"></script>
<script src="https://cdn.jsdelivr.net/npm/bootstrap@4.1.3/dist/js/bootstrap.min.js"></script>
<script src="https://cdn.jsdelivr.net/npm/jquery-match-height@0.7.2/dist/jquery.matchHeight.min.js"></script>
<script src="../../assets/js/main.js"></script>
         
<script type="text/javascript">
    $(function () {
        $("#CodcouEQ").autocomplete({
            source: function (request, response) {
                var param = {
                    "eqDes": $('#CodcouEQ').val(),
                    "query": $('#query').val(),
                    "cnx": $('#cnx').val()
                };
                console.log(param);
                $.ajax({
                    url: "../Admin/Equipements/DeplacerEq.aspx/Filtre",
                    data: JSON.stringify(param),
                    dataType: "json",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    dataFilter: function (data) {
                        return data;
                    },
                    success: function (data) {
                        response($.map(data.d, function (item) {
                            return {
                                value: item
                            }
                        }))
                    },
                    error: function (XMLHttpRequest, textStatus, errorThrown) {

                        var err = eval("(" + XMLHttpRequest.responseText + ")");
                        alert(err.Message)
                        console.log(err);
                    }
                });
            },
            minLength: 2 //This is the Char length of inputTextBox  
        });
    });
    $('#CodcouEQ').on('keyup keypress', function (e) {
        var keyCode = e.keyCode || e.which;
        if (keyCode === 13) {
            e.preventDefault();
            return false;
        }
    });
    $('#CHDest').on('keyup keypress', function (e) {
        var keyCode = e.keyCode || e.which;
        if (keyCode === 13) {
            e.preventDefault();
            return false;
        }
    });
        </script>
    </form>
</body>
</html>

