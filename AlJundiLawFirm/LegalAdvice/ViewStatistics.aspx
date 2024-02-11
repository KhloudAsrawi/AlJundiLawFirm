<%@ Page Title="" Language="C#" MasterPageFile="~/HeaderWebsite.Master" AutoEventWireup="true" CodeBehind="ViewStatistics.aspx.cs" Inherits="AlJundiLawFirm.LegalAdvice.ViewStatistics" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<link href="../Content/StyleBoxInAllPages.css" rel="stylesheet" />
<%-- Div View Statistics --%>
<br />
<div class="container-md StyleBox" id="DivViewStatistics" runat="server">
    <div class="row">
        <div class="col-lg-3 col-md-12 StyleFirstColumn">
            <br />
            <asp:Image ID="Image1" runat="server" ImageUrl="~/Images/Statistics.png" Width="90%" />
            <br /><br />
        </div>
        <div class="col-lg-9 col-md-12 StyleSecondColumn" style="direction:rtl;text-align:right;">
            <div class="mb-3 mt-3">
                <br />
                <p class="text-center"><asp:Label ID="LTSignup" runat="server" CssClass="StyleH1-2" Text="أحصائيات الموقع"></asp:Label></p>
            </div>
            <div class="mb-3 mt-3">
                <asp:Label ID="LNumNumberConsultations" runat="server">استشارات:</asp:Label>
                <asp:Label ID="NumNumberConsultations"  runat="server" Text="0"></asp:Label>
                <asp:Label ID="SNumberConsultations" runat="server" Text=" "></asp:Label>
                <br /><br />
            </div>
            <div class="mb-3 mt-3">
                <asp:Label ID="LNumNumberAnswers" runat="server">الاجوبة:</asp:Label>
                <asp:Label ID="NumNumberAnswers"  runat="server" Text="0"></asp:Label>
                <asp:Label ID="SNumberAnswers" runat="server" Text=" "></asp:Label>
                <br /><br />
            </div>
            <div class="mb-3 mt-3">
                <asp:Label ID="LNumNumberUser" runat="server">المشتركين:</asp:Label>
                <asp:Label ID="NumNumberUser" runat="server" Text="0"></asp:Label>
                <asp:Label ID="SNumberUser" runat="server" Text=" "></asp:Label>
                <br /><br />
            </div>
            <div class="mb-3 mt-3">
                <asp:Label ID="LNumSumViewConversation" runat="server">مشاهدات:</asp:Label>
                <asp:Label ID="NumSumViewConversation" runat="server" Text="0"></asp:Label>
                <asp:Label ID="SSumViewConversation" runat="server" Text=" "></asp:Label>
                <br /><br />
            </div>
        </div>
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
