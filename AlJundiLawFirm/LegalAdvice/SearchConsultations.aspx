<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SearchConsultations.aspx.cs" Inherits="AlJundiLawFirm.LegalAdvice.SearchConsultations" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<title></title>
<meta charset="utf-8" />
<meta name="viewport" content="width=device-width, initial-scale=1" />
<link href="https://cdn.jsdelivr.net/npm/bootstrap@5.1.3/dist/css/bootstrap.min.css" rel="stylesheet"/>
<script src="https://cdn.jsdelivr.net/npm/bootstrap@5.1.3/dist/js/bootstrap.bundle.min.js"></script>
<%-- صور --%>
<%--<script src='https://kit.fontawesome.com/a076d05399.js'></script>--%>
<script src="../Scripts/a076d05399.js"></script>
<link href="../Content/StyleBoxInAllPages.css" rel="stylesheet" />
</head>
<body style="background-color:rgba(255, 255, 255, 0.00)">
<form id="form1" runat="server">
    <div id="DivSearch" runat="server">
    <div class="container-md text-center">
        <div class="row" style="display:flex;justify-content:center;align-items:center;">
            <div class="col-lg-6 col-md-12">
                <div class="input-group">
                    <asp:Button ID="BSearch" runat="server" Text="بحث" CssClass="btn btn-warning Font-Size-input-group" OnClick="BSearch_Click" />
                    <asp:TextBox ID="TBSearch" runat="server" TextMode="Search" CssClass="form-control Font-Size-input-group" placeholder="أبحث عن استشارة"></asp:TextBox>
                    <span class="input-group-text Font-Size-input-group"><i class='fas fa-search'></i></span>
                </div>
            </div>
        </div>
        <br />
        <div class="row" style="direction:rtl;display:flex;justify-content:center;align-items:center;">
            <div class="col-lg-2 col-md-12 col-sm-12 Height-Buttons ">
                <asp:LinkButton ID="LBRecentlyAsked" CssClass="form-control btn btn-warning" runat="server">سئل حديثا</asp:LinkButton>  
            </div>
            <div class="col-lg-2 col-md-12 col-sm-12 Height-Buttons">
                <asp:LinkButton ID="LBMostActive" CssClass="form-control btn btn-warning" runat="server">الأكثر نشاطاً</asp:LinkButton> 
            </div>
            <div class="col-lg-2 col-md-12 col-sm-12 Height-Buttons">
                <asp:LinkButton ID="LBRecentlyAnswer" CssClass="form-control btn btn-warning"  runat="server">مجاب حديثا</asp:LinkButton> 
            </div>
            <div class="col-lg-2 col-md-12 col-sm-12 Height-Buttons">
                <asp:LinkButton ID="LBOldestAsked" CssClass="form-control btn btn-warning" runat="server">سئل قديماً</asp:LinkButton> 
            </div>
            <div class="col-lg-2 col-md-12 col-sm-12 Height-Buttons">
                <asp:LinkButton ID="LBMostAnswers" CssClass="form-control btn btn-warning" runat="server">الأكثر إجابة</asp:LinkButton> 
            </div>
        </div>
    </div>
</div>
</form>
</body>
</html>
