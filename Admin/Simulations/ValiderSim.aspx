<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ValiderSim.aspx.cs" Inherits="PFE.Simulations.ValiderSim" %>

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
    .police{
        font-size:22px;
        color:#008000;
        font-weight:bold;
        text-align:center;
    }
    .total{
        font-size:20px;
        background-color:#9ACD32;
        color:#F5F5F5;
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
                            <li><i class="fa fa-list"></i><a href="../Equipements/ConsulterEq.aspx">Consulter</a></li>
                            <li><i class="fa fa-plus-square"></i><a href="../Equipements/AjouterEq.aspx">Ajouter</a></li>
                            <li><i class="fa fa-mail-forward"></i><a href="../Equipements/DeplacerEq.aspx">Déplacer</a></li>
                            <li><i class="fa fa-list-alt"></i><a href="../Equipements/DemandeEq.aspx">Consulter les demamdes de location</a></li>
                            <li><i class="fa fa-external-link-square"></i><a href="../Equipements/ConsulterDepEq.aspx">Suivre les locations</a></li>
                        </ul>
                    </li>
                    <li class="menu-item-has-children dropdown">
                        <a href="#" class="dropdown-toggle" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false"> <i class="menu-icon fa fa-clipboard"></i>Simulations</a>
                        <ul class="sub-menu children dropdown-menu">                           
                            <li><i class="fa fa-list"></i><a href="ConsulterSim.aspx">Consulter</a></li>
                            <li><i class="fa fa-plus-square"></i><a href="GenererSim.aspx">Générer</a></li>
                            <li><i class="fa fa-exclamation-triangle"></i><a href="ReclamSim.aspx">Consulter les réclamations</a></li>
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
                                    <li><a href="ConsulterSim.aspx">Simulations</a></li>
                                    <li><a href="active">Détails de la Simulation</a></li>
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
                    <div class="col-md-12">
                        <div class="card">
                            <div class="card-header">
                                <strong class="card-title">Détails de la simulation</strong>
                            </div>
                            <div class="card-body">
                                <div class="float-right btn btn-info m-l-10 m-b-10">
                                <asp:Label ID="SimTotal" runat="server"></asp:Label>
                                </div>
                                <br />
                                <br />
                                  <asp:GridView ID="gvSimDetails" runat="server" AutoGenerateColumns="false" ShowFooter="false" DataKeyNames="ID_DEP"
                                  ShowHeaderWhenEmpty="true" OnRowCommand="gvSimDetails_RowCommand" OnRowEditing="gvSimDetails_RowEditing" OnRowCancelingEdit="gvSimDetails_RowCancelingEdit"
                                  OnRowUpdating="gvSimDetails_RowUpdating" OnRowDeleting="gvSimDetails_RowDeleting" CssClass="table table-striped table-bordered table-sm" style=" table-layout: fixed;">
                
                                    <Columns >
                                            <asp:TemplateField HeaderText="Code" >
                                                <ItemTemplate><asp:Label Text='<%# Eval("ST_CODEQ") %>' runat="server" /></ItemTemplate>
                                                <EditItemTemplate><asp:TextBox ID="txtST_CODEQ" Text='<%# Eval("ST_CODEQ") %>' runat="server" /></EditItemTemplate>
                                                <FooterTemplate><asp:TextBox ID="txtST_CODEQFooter" runat="server" /></FooterTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Equipement">
                                                <ItemTemplate><asp:Label Text='<%# Eval("ST_DESEQ") %>' runat="server" /></ItemTemplate>
                                                <EditItemTemplate><asp:TextBox ID="txtST_DESEQ" Text='<%# Eval("ST_DESEQ") %>' runat="server" /></EditItemTemplate>
                                                <FooterTemplate><asp:TextBox ID="txtST_DESEQFooter" runat="server" /></FooterTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Chantier">
                                                <ItemTemplate><asp:Label Text='<%# Eval("ST_DESCH") %>' runat="server" /></ItemTemplate>
                                                <EditItemTemplate><asp:TextBox ID="txtST_DESCH" Text='<%# Eval("ST_DESCH") %>' runat="server" /></EditItemTemplate>
                                                <FooterTemplate><asp:TextBox ID="txtST_DESCHFooter" runat="server" /></FooterTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="D.entrée">
                                                <ItemTemplate><asp:Label Text='<%# Eval("DT_ENTREE") %>' runat="server" /></ItemTemplate>
                                                <EditItemTemplate><asp:TextBox ID="txtDT_ENTREE" Text='<%# Eval("DT_ENTREE") %>' runat="server" /></EditItemTemplate>
                                                <FooterTemplate><asp:TextBox ID="txtDT_ENTREEFooter" runat="server" /></FooterTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="D.sortie">
                                                <ItemTemplate><asp:Label Text='<%# Eval("DT_SORTIE") %>' runat="server" /></ItemTemplate>
                                                <EditItemTemplate><asp:TextBox ID="txtDT_SORTIE" Text='<%# Eval("DT_SORTIE") %>' runat="server" /></EditItemTemplate>
                                                <FooterTemplate><asp:TextBox ID="txtDT_SORTIEFooter" runat="server" /></FooterTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="NB JRS">
                                                <ItemTemplate><asp:Label Text='<%# Eval("NU_NBJRS") %>' runat="server" /></ItemTemplate>
                                                <EditItemTemplate><asp:TextBox ID="txtNU_NBJRS" Text='<%# Eval("NU_NBJRS") %>' runat="server" /></EditItemTemplate>
                                                <FooterTemplate><asp:TextBox ID="txtNU_NBJRSFooter" runat="server" /></FooterTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Taux">
                                                <ItemTemplate><asp:Label Text='<%# Eval("NU_TAUXLOC") %>' runat="server" /></ItemTemplate>
                                                <EditItemTemplate><asp:TextBox ID="txtNU_TAUXLOC" Text='<%# Eval("NU_TAUXLOC") %>' runat="server" /></EditItemTemplate>
                                                <FooterTemplate><asp:TextBox ID="txtNU_TAUXLOCFooter" runat="server" /></FooterTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Valeur">
                                                <ItemTemplate><asp:Label Text='<%# Eval("NU_VALEUR") %>' runat="server" /></ItemTemplate>
                                                <EditItemTemplate><asp:TextBox ID="txtNU_VALEUR" Text='<%# Eval("NU_VALEUR") %>' runat="server" /></EditItemTemplate>
                                                <FooterTemplate><asp:TextBox ReadOnly="true" ID="txtNU_VALEURFooter" runat="server" /></FooterTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:ImageButton ImageUrl="~/images/edit.png" runat="server" CommandName="Edit" ToolTip="Modifier" Width="20px" Height="20px"/>
                                                    <asp:ImageButton ImageUrl="~/images/delete.png" runat="server" CommandName="Delete" ToolTip="Supprimer" Width="20px" Height="20px"/>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:ImageButton ImageUrl="~/images/save.png" runat="server" CommandName="Update" ToolTip="Mettre a jour" Width="20px" Height="20px"/>
                                                    <asp:ImageButton ImageUrl="~/images/cancel.png" runat="server" CommandName="Cancel" ToolTip="Annuler" Width="20px" Height="20px"/>
                                                </EditItemTemplate>
                                                <FooterTemplate>
                                                    <asp:ImageButton ImageUrl="~/images/addnew.png" runat="server" CommandName="AddNew" ToolTip="Ajouter" Width="20px" Height="20px"/>
                                                </FooterTemplate>
                                           </asp:TemplateField>
                                    </Columns>
                        </asp:GridView>
                                    <asp:GridView ID="gvSimulationValide" CssClass="table table-striped table-bordered" runat="server" AutoGenerateColumns="false" AllowPaging="true" OnPageIndexChanging = "OnPaging">
                                        <Columns>
                                               <asp:BoundField DataField="ST_CODEQ" HeaderText ="Code" />
                                               <asp:BoundField DataField="ST_DESEQ" HeaderText ="Désignation" />
                                               <asp:BoundField DataField="DT_ENTREE" HeaderText ="D.entrée" />
                                               <asp:BoundField DataField="DT_SORTIE" HeaderText ="D.sortie" />
                                               <asp:BoundField DataField="NU_NBJRS" HeaderText ="Nb jrs" />
                                               <asp:BoundField DataField="NU_TAUXLOC" HeaderText ="Taux" />
                                               <asp:BoundField DataField="NU_VALEUR" HeaderText ="Valeur" />
                                        </Columns>
                                    </asp:GridView>
                                <div class="float-right">
                                    <div class="text-center">
                                        <asp:Button ID="Valider" runat="server" Text="Valider" CssClass="btn btn-info" OnClick="Valider_Click" />
                                    </div>
                                    <br />
                                    <asp:Button ID="ExportExcel" runat="server" Text="exporter vers excel" CssClass="btn btn-outline-success" OnClick="ExportExcel_Click" />
                                    <asp:Button ID="ExportPDF" runat="server" Text="exporter vers PDF" CssClass="btn btn-outline-danger" OnClick="ExportPDF_Click"  />
                                </div>
                        <asp:Label ID="lblSuccessMessage" Text="" runat="server" ForeColor="Green" />
                        <br />
                        <asp:Label ID="lblErrorMessage" Text="" runat="server" ForeColor="Red" />
                                
                            </div>
                        </div>
                    </div>
                </div>
            </div><!-- .animated -->
        </div><!-- .content -->


        <div class="clearfix"></div>
    </div><!-- /#right-panel -->

    <!-- Right Panel -->

    <!-- Scripts -->
    <script src="https://cdn.jsdelivr.net/npm/jquery@2.2.4/dist/jquery.min.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/popper.js@1.14.4/dist/umd/popper.min.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@4.1.3/dist/js/bootstrap.min.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/jquery-match-height@0.7.2/dist/jquery.matchHeight.min.js"></script>
    <script src="../../assets/js/main.js"></script>
    <script src="../../assets/js/lib/data-table/datatables.min.js"></script>
    <script src="../../assets/js/lib/data-table/dataTables.bootstrap.min.js"></script>
    <script src="../../assets/js/lib/data-table/dataTables.buttons.min.js"></script>
    <script src="../../assets/js/lib/data-table/buttons.bootstrap.min.js"></script>
    <script src="../../assets/js/lib/data-table/jszip.min.js"></script>
    <script src="../../assets/js/lib/data-table/vfs_fonts.js"></script>
    <script src="../../assets/js/lib/data-table/buttons.html5.min.js"></script>
    <script src="../../assets/js/lib/data-table/buttons.print.min.js"></script>
    <script src="../../assets/js/lib/data-table/buttons.colVis.min.js"></script>
    <script src="../../assets/js/init/datatables-init.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $('#bootstrap-data-table-export').DataTable();
        });
  </script>
</form>
</body>
</html>









