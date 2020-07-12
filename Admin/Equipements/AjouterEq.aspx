<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AjouterEq.aspx.cs" Inherits="PFE.Equipements.AjouterEq" %>
<!doctype html>
<!--[if lt IE 7]>      <html class="no-js lt-ie9 lt-ie8 lt-ie7" lang=""> <![endif]-->
<!--[if IE 7]>         <html class="no-js lt-ie9 lt-ie8" lang=""> <![endif]-->
<!--[if IE 8]>         <html class="no-js lt-ie9" lang=""> <![endif]-->
<!--[if gt IE 8]><!--> <html class="no-js" lang=""> <!--<![endif]-->
<head>
    <meta charset="utf-8">
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <title>BBS</title>
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
                                            <li class="active">Ajouter</li>
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
                                <strong>Ajouter un équipement</strong>
                            </div>
                            <div class="card-body card-block">
                                <div id="mydiv" runat="server" class="sufee-alert alert with-close alert-danger alert-dismissible fade show">
                                <span class="badge badge-pill badge-danger">Info</span><asp:Label ID="errormsg" Text=" Vous devez remplir tous les champs" runat="server"></asp:Label> 
                                <button type="button" class="close" data-dismiss="alert" aria-label="Close">
                                <span aria-hidden="true">&times;</span>
                                </button>
                                </div>
                                <div class="row">
                                     <div class="col-lg-6">
                                         <div class="row">
                                             <div class="col-lg-5"><asp:Label ID="Label12" runat="server" CssClass="form-control-label">ID équipement:</asp:Label></div>
                                             <div class="col-lg-7"><asp:TextBox ID="idEQ" TextMode="Number" Text="1234" runat="server" CssClass="form-control" MaxLength="9"></asp:TextBox></div>
                                         </div>
                                     </div>
                                    <div class="col-lg-6">
                                         <div class="row">
                                             <div class="col-lg-5"><asp:Label ID="Label0" runat="server" CssClass="form-control-label">Code d'équipement:</asp:Label></div>
                                             <div class="col-lg-7"><asp:TextBox ID="CodCouEQ" Text="code" runat="server" CssClass="form-control" MaxLength="14"></asp:TextBox></div>
                                        </div>
                                    </div>
                                </div>
                                <hr />
                                <div class="row">
                                     <div class="col-lg-6">
                                         <div class="row">
                                            <div class="col-lg-5"><asp:Label ID="Label1" runat="server" CssClass="form-control-label">Catégorie:</asp:Label></div>
                                            <div class="col-lg-7"><asp:TextBox ID="Categorie" Text="catégorie" runat="server" CssClass="form-control"></asp:TextBox></div>
                                         </div>
                                     </div>
                                    <div class="col-lg-6">
                                         <div class="row">
                                            <div class="col-lg-5"><asp:Label ID="Label3" runat="server" CssClass="form-control-label">Désignation:</asp:Label></div>
                                            <div class="col-lg-7"> <asp:TextBox ID="DesEQ" Text="désignation" runat="server" CssClass="form-control"></asp:TextBox></div>
                                        </div>
                                    </div>
                                </div>
                                <hr />
                                <div class="row">
                                    <div class="col-lg-6">
                                         <div class="row">
                                            <div class="col-lg-5"><asp:Label ID="Label4" runat="server" CssClass="form-control-label">Emplacement:</asp:Label></div>
                                            <div class="col-lg-7"><asp:TextBox ID="EmpEQ" runat="server" Text="emplacement" CssClass="form-control"></asp:TextBox></div>
                                         </div>
                                    </div>
                                     <div class="col-lg-6">
                                         <div class="row">
                                            <div class="col-lg-5"><asp:Label ID="Label7" runat="server" CssClass="form-control-label">Code magasin:</asp:Label></div>
                                            <div class="col-lg-7"> <asp:TextBox ID="codeMag" runat="server" Text="magasin" CssClass="form-control"></asp:TextBox></div>
                                        </div>
                                     </div>
                                </div>
                                <hr />
                                <div class="row">
                                     <div class="col-lg-6">
                                         <div class="row">
                                            <div class="col-lg-5"><asp:Label ID="Label8" runat="server" CssClass="form-control-label">Fournisseur:</asp:Label></div>
                                            <div class="col-lg-7"> <asp:TextBox ID="fournisseur" runat="server" Text="fournisseur" CssClass="form-control"></asp:TextBox></div>
                                        </div>
                                     </div>
                                     <div class="col-lg-6">
                                         <div class="row">
                                            <div class="col-lg-5"><asp:Label ID="Label9" runat="server" CssClass="form-control-label">Date d'achat:</asp:Label></div>
                                            <div class="col-lg-7"> <asp:TextBox ID="dtAch" runat="server" CssClass="form-control"></asp:TextBox></div>
                                         </div>
                                     </div>
                                </div>
                                <hr />
                                <div class="row">
                                     <div class="col-lg-6">
                                         <div class="row">
                                            <div class="col-lg-5"><asp:Label ID="Label5" runat="server" CssClass="form-control-label">Prix Achat:</asp:Label></div>
                                            <div class="col-lg-7"><asp:TextBox ID="PrixEQ" runat="server" TextMode="Number" CssClass="form-control"></asp:TextBox></div>
                                        </div>
                                     </div>
                                     <div class="col-lg-6">
                                         <div class="row">
                                            <div class="col-lg-5"><asp:Label ID="Label2" runat="server" CssClass="form-control-label">Taux de loacation:</asp:Label></div>
                                            <div class="col-lg-7"><asp:TextBox ID="TauxLoc" runat="server" TextMode="number" CssClass="form-control"></asp:TextBox></div>
                                         </div>
                                     </div>
                                </div>
                                <hr />
                                <div class="row">
                                     <div class="col-lg-6">
                                         <div class="row">
                                            <div class="col-lg-5"><asp:Label ID="Label13" runat="server" CssClass="form-control-label">Date fin vie:</asp:Label></div>
                                            <div class="col-lg-7"> <asp:TextBox ID="dtFinVie" runat="server" CssClass="form-control"></asp:TextBox></div>
                                        </div>
                                     </div>
                                     <div class="col-lg-6">
                                         <div class="row">
                                            <div class="col-lg-5"> <asp:Label ID="Label6" runat="server" CssClass="form-control-label">Observation:</asp:Label></div>
                                            <div class="col-lg-7"><asp:TextBox ID="EQobs" runat="server" Text="observation..." CssClass="form-control" Rows="4"></asp:TextBox></div>
                                         </div>
                                     </div>
                                </div>
                                <hr />
                                <div class="row">
                                     <div class="col-lg-6">
                                         <div class="row">
                                            <div class="col-lg-5"> <asp:Label ID="Label14" runat="server" CssClass="form-control-label">Etat:</asp:Label></div>
                                            <div class="col-lg-7"><asp:DropDownList ID="etat" CssClass="form-control" runat="server" AppendDataBoundItems="True" AutoPostBack="true" /></div>
                                        </div>
                                     </div>
                                     <div class="col-lg-6">
                                         <div class="row">
                                             <div class="col-sm-6"></div>
                                             <div class="col-sm-6 text-right">
                                                <asp:Button ID="BTNajouter" runat="server" BorderStyle="None" Text="Ajouter" CssClass="btn btn-success" OnClick="BTNajouter_CLICK"/>
                                                <asp:Button ID="BTNannuler" runat="server" BorderStyle="None"  Text="Annuler" CssClass="btn btn-danger" OnClick="BTNannuler_Click"/>
                                             </div>
                                         </div>
                                     </div>
                                </div>
                                <asp:TextBox runat="server" ID="cnx" CssClass="customer_custom1"/>
                                <asp:TextBox runat="server" ID="query" CssClass="customer_custom1"/>
                                <asp:TextBox runat="server" ID="queryFour" CssClass="customer_custom1"/>
                                <asp:TextBox runat="server" ID="queryCateg" CssClass="customer_custom1"/>
                                <asp:TextBox runat="server" ID="queryMag" CssClass="customer_custom1"/>
                            
                              </div>
                           </div>
                        </div>
                    </div>
            </div><!-- .animated -->
        </div><!-- .content -->
    <div class="clearfix"></div>
         
</div><!-- /#right-panel -->
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
        $("#codeMag").autocomplete({
            source: function (request, response) {
                var param = {
                    "eqDes": $('#codeMag').val(),
                    "query": $('#queryMag').val(),
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
                $("#EmpEQ").autocomplete({
                    source: function (request, response) {
                        var param = {
                            "eqDes": $('#EmpEQ').val(),
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

        </script>
<script type="text/javascript">
    $(function () {
        $("#fournisseur").autocomplete({
            source: function (request, response) {
                var param = {
                    "eqDes": $('#fournisseur').val(),
                    "query": $('#queryFour').val(),
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
            minLength: 1 //This is the Char length of inputTextBox  
        });
    });

        </script>
<script type="text/javascript">
    $(function () {
        $("#Categorie").autocomplete({
            source: function (request, response) {
                var param = {
                    "eqDes": $('#Categorie').val(),
                    "query": $('#queryCateg').val(),
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
                            console.log(data.d);   
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
            minLength: 1 //This is the Char length of inputTextBox  
        });
    });

        </script>