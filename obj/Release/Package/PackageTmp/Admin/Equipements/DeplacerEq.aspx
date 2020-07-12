<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DeplacerEq.aspx.cs" Inherits="PFE.Equipements.DeplacerEq" %>
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
        color:#778899;
        font-size:15px;
        font-weight:bold;
    }
    .notiftitle{
        font-size:17px;
        color:#17a2b8;
        font-weight:bold;
    }
    .header-left .dropdown .dropdown-menu p {
            font-size: 16px;
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
                        <a href="../Dashboard.aspx"><i class="menu-icon fa fa-laptop"></i>Menu </a>
                    </li>

                    <li class="menu-item-has-children dropdown">
                        <a href="#" class="dropdown-toggle" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false"> <i class="menu-icon fa fa-users"></i>Utilisateurs</a>
                        <ul class="sub-menu children dropdown-menu">                           
                            <li><i class="fa fa-list"></i><a href="../Utilisateurs/ConsulterUti.aspx">Consulter</a></li>
                            <li><i class="fa fa-plus-square"></i><a href="../Utilisateurs/AjouterUti.aspx">Ajouter</a></li>
                        </ul>
                    </li>
                    <li class="menu-item-has-children dropdown">
                        <a href="#" class="dropdown-toggle" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false"> <i class="menu-icon fa fa-truck"></i>Equipements</a>
                        <ul class="sub-menu children dropdown-menu">                           
                            <li><i class="fa fa-list"></i><a href="ConsulterEq.aspx">Consulter</a></li>
                            <li><i class="fa fa-plus-square"></i><a href="AjouterEq.aspx">Ajouter</a></li>
                            <li><i class="fa fa-mail-forward"></i><a href="DeplacerEq.aspx">Déplacer</a></li>
                            <li><i class="fa fa-list-alt"></i><a href="DemandeEq.aspx">Consulter les demamdes de location</a></li>
                            <li><i class="fa fa-external-link-square"></i><a href="ConsulterDepEq.aspx">Suivre les locations</a></li>
                        </ul>
                    </li>
                    <li class="menu-item-has-children dropdown">
                        <a href="#" class="dropdown-toggle" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false"> <i class="menu-icon fa fa-clipboard"></i>Simulations</a>
                        <ul class="sub-menu children dropdown-menu">                           
                            <li><i class="fa fa-list"></i><a href="../Simulations/ConsulterSim.aspx">Consulter</a></li>
                            <li><i class="fa fa-plus-square"></i><a href="../Simulations/GenererSim.aspx">Générer</a></li>
                            <li><i class="fa fa-exclamation-triangle"></i><a href="../Simulations/ReclamSim.aspx">Consulter les réclamations</a></li>
                        </ul>
                    </li>
                    
                    <li class="menu-item-has-children dropdown">
                        <a href="#" class="dropdown-toggle" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false"> <i class="menu-icon fa fa-file-text-o"></i>Factures</a>
                        <ul class="sub-menu children dropdown-menu">                           
                            <li><i class="fa fa-list"></i><a href="../Factures/ConsulterFac.aspx">Consulter</a></li>
                            <li><i class="fa fa-plus-square"></i><a href="../Factures/GenererFac.aspx">Générer</a></li>
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
                        <img src="../../images/logo_bmi.png" alt="Logo">
                    </a>
                    <a id="menuToggle" class="menutoggle"><i class="fa fa-bars"></i></a>
                </div>
            </div>
            <div class="top-right">
                <div class="header-menu">
                    <div class="header-left">
                        <button class="search-trigger"><i class="fa fa-search"></i></button>
                        <div class="form-inline">
                                <input class="form-control mr-sm-2" type="text" placeholder="Rechercher ..." aria-label="Rechercher">
                                <button class="search-close" type="submit"><i class="fa fa-close"></i></button>
                        </div>
                        <div class="dropdown for-message">
                            <button class="btn btn-secondary dropdown-toggle" type="button" id="message" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                                <i class="fa fa-bell"></i>
                                <span class="count bg-primary">4</span>
                            </button>
                            <div class="dropdown-menu" aria-labelledby="message">
                                <p class="red text-center notiftitle">Notifications</p>
                                
								<asp:Repeater ID="repNotifs" runat="server">
									<ItemTemplate>
										<a class="dropdown-item media" href="#">
											<div class="message media-body">
												<div class="time float-right"><asp:Label ID="DATE" runat="server" Text='<%#Eval("DT_CREATE") %>'/></div>
												<p><asp:Label ID="OBS" runat="server" CssClass="notif" Text='<%#Eval("ST_OBSTRA") %>'/></p>
											</div>
										</a>
									</ItemTemplate>
									<FooterTemplate>
                                        <div class="text-center">
                                           <a href="../Notifs.aspx" class="seeMore"> Voir plus </a>
                                        </div>
                                    </FooterTemplate>
                                </asp:Repeater>
                            </div>
                        </div>
                    </div>
                        

                    <div class="user-area dropdown float-right">
                        <a href="#" class="dropdown-toggle active" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                            <img class="user-avatar rounded-circle" src="../../images/admin.jpg" alt="User Avatar">
                        </a>

                        <div class="user-menu dropdown-menu">
                            <a class="nav-link" href="../Utilisateurs/ProfilUti.aspx"><i class="fa fa-user"></i>Mon Profil</a>

                            <a class="nav-link" href="../Notifs.aspx"><i class="fa fa-bell-o"></i>Notifications <asp:Label runat="server"  ID="nbrNotif" class="count"/></a>

                            <a class="nav-link" href="../../index.aspx"><i class="fa fa-power-off"></i>se déconnecter</a>
                        </div>
                    </div>
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
                                            <li><a href="ConsulterEq.aspx">Equipements</a></li>
                                            <li class="active">Déplacer</li>
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
                                <div class="col-lg-6">
                                    <div class="card">
                                        <div class="card-header">
                                            <strong>Déplacer un équipement</strong>
                                        </div>
                                        <div class="card-body card-block">
                                            <div ID="mydiv" runat="server" class="sufee-alert alert with-close alert-danger alert-dismissible fade show">
                                                <span class="badge badge-pill badge-danger">Info</span><asp:Label ID="errormsg" Text=" Vous devez remplir tous les champs" runat="server"></asp:Label> 
                                                <button type="button" class="close" data-dismiss="alert" aria-label="Close">
                                                <span aria-hidden="true">&times;</span>
                                                </button>
                                            </div>
                                            <div class="row form-group">
                                                <div class="col col-md-5">
                                                    <asp:Label ID="code" runat="server" CssClass="pb-4 display-5">Désignation de l'équipement: </asp:Label>
                                                </div>
                                                <div class="col-12 col-md-6">
                                                    <asp:TextBox ID="CodcouEQ" runat="server" CssClass="form-control" OnTextChanged="getVal"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="row form-group">
                                                <div class="col col-md-5">
                                                    <asp:Label ID="Label1" runat="server" CssClass="form-control-label">Emplacement Actuel: </asp:Label>
                                                </div>
                                                <div class="col-12 col-md-6">
                                                    <asp:TextBox ID="EmpActuel" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="row form-group">
                                                <div class="col col-md-5">
                                                    <asp:Label ID="Label6" runat="server" CssClass="form-control-label">Type du mouvement: </asp:Label>
                                                </div>
                                                <div class="col-12 col-md-6">
                                                    <asp:DropDownList ID="MVType" CssClass="form-control" runat="server" AppendDataBoundItems="True" AutoPostBack="true" />
                                                </div>
                                            </div>
                                            <div class="row form-group">
                                                <div class="col col-md-5">
                                                    <asp:Label ID="Label2" runat="server" CssClass="form-control-label">Chantier Destination: </asp:Label>
                                                </div>
                                                <div class="col-12 col-md-6">
                                                    <asp:TextBox ID="CHDest" runat="server" CssClass="form-control"></asp:TextBox>
                                                </div>
                                            </div>

                                            <div class="row form-group">
                                                <div class="col col-md-5">
                                                    <asp:Label ID="Label3" runat="server" CssClass="form-control-label">Date: </asp:Label>
                                                </div>
                                                <div class="col-12 col-md-6">
                                                    <asp:TextBox ID="MVTdate" runat="server" CssClass="form-control"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="row form-group">
                                                <div class="col col-md-5">
                                                    <asp:Label ID="Label8" runat="server" CssClass="form-control-label">Taux de location: </asp:Label>
                                                </div>
                                                <div class="col-12 col-md-6">
                                                    <asp:TextBox ID="taux" runat="server" CssClass="form-control"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="row form-group">
                                                <div class="col col-md-5">
                                                    <asp:Label ID="Label7" runat="server" CssClass="form-control-label">CIN charge: </asp:Label>
                                                </div>
                                                <div class="col-12 col-md-6">
                                                    <asp:TextBox ID="CHcin" runat="server" CssClass="form-control" MaxLength="8"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="row form-group">
                                                <div class="col col-md-5">
                                                    <asp:Label ID="Label4" runat="server" CssClass="form-control-label">Compteur charge: </asp:Label>
                                                </div>
                                                <div class="col-12 col-md-6">
                                                    <asp:TextBox ID="Compteur" type="number" runat="server" CssClass="form-control" min="0"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="row form-group">
                                                <div class="col col-md-5">
                                                    <asp:Label ID="Label5" runat="server" CssClass="form-control-label">Observation: </asp:Label>
                                                </div>
                                                <div class="col-12 col-md-6">
                                                    <asp:TextBox id="MVTobs" CssClass="form-control" TextMode="multiline" Columns="20" Rows="5" runat="server" />
                                                </div>
                                            </div>
                                            <div class="card-footer ">
                                                <div class="float-right">
                                                    <asp:Button ID="BTNdeplacer" runat="server" BorderStyle="None" OnClick="BTNdeplacer_Click" Text="Déplacer" class="btn btn-success" />
                                                    <asp:Button ID="BTNannuler" runat="server" BorderStyle="None" Text="Annuler" class="btn btn-danger" OnClick="BTNannuler_Click" />
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            <div class="col-lg-6">
                                <div class="card">
                                    <div class="card-header">
                                        <strong>Liste des Mouvements</strong>
                                    </div>
                                    <div class="card-body">
                                        <hr />
                                        <asp:GridView ID="gvEquipement" CssClass="table table-striped table-bordered" runat="server" AutoGenerateColumns="false" AllowPaging="true" OnPageIndexChanging="BTNdeplacer_Click">
                                            <Columns>

                                                <asp:BoundField DataField="ST_DESCH" HeaderText="Chantier Destination" />
                                                <asp:BoundField DataField="ST_TYPEMVT" HeaderText="Type de mouvement" />
                                                <asp:BoundField DataField="DT_MVT" HeaderText="Date" />
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                    <asp:TextBox runat="server" ID="code_str_lab" CssClass="customer_custom1" />
                                    <asp:TextBox runat="server" ID="query" CssClass="customer_custom1" />
                                    <asp:TextBox runat="server" ID="queryCH" CssClass="customer_custom1" />
                                    <asp:TextBox runat="server" ID="cnx" CssClass="customer_custom1" />

                                </div>
                          </div>
                     </div>
                </div>
        </div>

<div class="clearfix"></div>
</div>
<!-- /#right-panel -->
<!-- Right Panel --> 
    </form>
</body>
</html>
<!-- Scripts -->
<script src="https://ajax.aspnetcdn.com/ajax/jquery/jquery-1.8.0.js" type="text/javascript"></script>
<script src="https://ajax.aspnetcdn.com/ajax/jquery.ui/1.8.22/jquery-ui.js" type="text/javascript"></script>
<link href="https://ajax.aspnetcdn.com/ajax/jquery.ui/1.12.1/themes/blitzer/jquery-ui.css" rel="stylesheet" />
<script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jqueryui/1.8.19/jquery-ui.js"></script>
<script src="https://cdn.jsdelivr.net/npm/jquery@2.2.4/dist/jquery.min.js"></script>
<script src="https://cdn.jsdelivr.net/npm/popper.js@1.14.4/dist/umd/popper.min.js"></script>
<script src="https://cdn.jsdelivr.net/npm/bootstrap@4.1.3/dist/js/bootstrap.min.js"></script>
<script src="https://cdn.jsdelivr.net/npm/jquery-match-height@0.7.2/dist/jquery.matchHeight.min.js"></script>
<script src="../../assets/js/main.js"></script>

        <script type="text/javascript">
            $(function () {
                $("#CHDest").autocomplete({
                    source: function (request, response) {
                        var param = {
                            "eqDes": $('#CHDest').val(),
                            "query": $('#queryCH').val(),
                            "cnx": $('#cnx').val()
                        };
                        console.log(param);
                        $.ajax({
                            url: "DeplacerEq.aspx/Filtre",
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
        </script>

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
                            url: "DeplacerEq.aspx/Filtre",
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