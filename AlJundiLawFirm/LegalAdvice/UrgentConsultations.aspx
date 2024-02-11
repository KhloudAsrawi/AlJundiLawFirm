<%@ Page Title="" Language="C#" MasterPageFile="~/HeaderWebsite.Master" AutoEventWireup="true" CodeBehind="UrgentConsultations.aspx.cs" Inherits="AlJundiLawFirm.LegalAdvice.UrgentConsultations" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<link href="../Content/StyleBoxInAllPages.css" rel="stylesheet" />
<br />
<iframe id="DivSearch" runat="server" src="SearchConsultations.aspx" style="width:100%" frameborder="0" scrolling="no"></iframe>
<br />
<%-- الاستشارات التي اجبت عليها --%>
<div id="DivAnsweredConsultations" runat="server">
    <div id="DivNoAnswered" runat="server">
    </div>
    <div class="container-md mt-3" id="DivNumberPages" runat="server" visible="false">                 
    </div>
</div>

<%-- لا توجد استشارات جديدة مأجورة للاجابة عليها --%>
<div id="DivNoAnsweredConsultations" runat="server" class="container-md StyleBox StyleColumn" visible="false" style="direction:rtl">
    <div class="mb-3 mt-3 text-center">
        <br />
        <asp:Label ID="Label1" runat="server" Text="لا توجد استشارات جديدة مأجورة للاجابة عليها"></asp:Label>
        <br />
    </div>
    <div class="mb-3 mt-3 text-center">
        <div class="row">
            <div class="col-lg-6 col-md-12" style="direction:rtl;">
                <asp:HyperLink ID="HLBackPage1" runat="server" CssClass="Hyper-Link-Go">عودة الى الصفحة السابقة</asp:HyperLink>
            </div>
            <div class="col-lg-6 col-md-12" style="direction:rtl;">
                <asp:HyperLink ID="HLIndex1" runat="server" NavigateUrl="~/Index.aspx" CssClass="Hyper-Link-Go">الصفحة الرئيسية</asp:HyperLink>
            </div>
        </div>
        <br />
    </div>        
</div>

<%-- لا يمكن الوصول الى هذه الصفحة --%>
<div id="DivCannotDisplay" runat="server" class="container-md StyleBox StyleColumn" visible="false" style="direction:rtl">
    <div class="mb-3 mt-3 text-center">
        <br />
        <asp:Label ID="LCannotDisplay" runat="server" Text="لا يمكن عرض الصفحة التي طلبتها الآن. فقد تكون غير متوفرة مؤقتاً، أو إن الرابط الذي ضغطت عليه غير كامل أو منتهي الصلاحية، أو أنه ليس لديك إذن بعرض الصفحة "></asp:Label>
        <br />
    </div>
    <div class="mb-3 mt-3 text-center">
        <div class="row">
            <div class="col-lg-6 col-md-12" style="direction:rtl;">
                <asp:HyperLink ID="HLBackPage" runat="server" CssClass="Hyper-Link-Go">عودة الى الصفحة السابقة</asp:HyperLink>
            </div>
            <div class="col-lg-6 col-md-12" style="direction:rtl;">
                <asp:HyperLink ID="HLIndex" runat="server" NavigateUrl="~/Index.aspx" CssClass="Hyper-Link-Go">الصفحة الرئيسية</asp:HyperLink>
            </div>
        </div>
        <br />
    </div>
</div>
</asp:Content>
