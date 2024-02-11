<%@ Page Title="" Language="C#" MasterPageFile="~/HeaderWebsite.Master" AutoEventWireup="true" CodeBehind="ShowConversation.aspx.cs" Inherits="AlJundiLawFirm.LegalAdvice.ShowConversation" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<link href="../Content/StyleBoxInAllPages.css" rel="stylesheet" />
<%-- For audio recording --%>
<script src="https://code.jquery.com/jquery-3.4.1.js"></script>
<script type="text/javascript" src="/Scripts/record/audiodisplay.js"></script> 
<script type="text/javascript" src="/Scripts/record/recorder.js"></script> 
<script type="text/javascript" src="/Scripts/record/main.js"></script>
<style> 
button{
    font-size: 30px;
    border: 0px;
    color: #ff0000;
}

/* File Upload Design  */
.file-upload
{
    display : inline-block;
    overflow: hidden; 
    position: relative; 
    text-align: center;
    vertical-align: middle;
    /* Cosmetics */
    color: #ff0000; 
} 
   
/* The button size */ 
.file-upload { height: 3.5em; }
.file-upload,.file-upload span { width: 3.5em; } 
   
.file-upload input
{
    position: absolute;
    top: 0;
    left: 0;
    margin: 0;
    font-size: 11px;
    /* Loses tab index in webkit if width is set to 0 */
    opacity: 0;
    filter: alpha(opacity=0);
}

.file-upload strong { font: normal 12px Tahoma,sans-serif;text-align:center;vertical-align:middle; } 
 
.file-upload span
{
    position: absolute;
    top: 0; left: 0;
    display: inline-block;
    /* Adjust button text vertical alignment */
    padding-top: .15em;
}
</style>
<%-- Div Show Conversation --%>
<br />
<div class="container-md StyleBox" id="DivShowConversation" runat="server">
    <div class='mb-3 mt-3'>
        <div class="row">
            <div class="col-lg-2 col-md-12 StyleFirstColumn">
                <asp:Image ID="ImageUserWroteConsultation" CssClass="rounded-circle SizeImage" runat="server" Width="90%" />
            </div>
            <div class="col-lg-8 col-md-12 StyleFirstColumn">
                <h1 dir="auto" class="StyleH1-2"><asp:Label ID="Subject" runat="server" Text="Subject" /></h1>
            </div>
            <div class="col-lg-2 col-md-12 StyleFirstColumn" style="text-align:right;">
                <p><i class="fa far fa-clock"></i>&nbsp;<asp:Label ID="CreateDate" runat="server" Text="CreateDate" /></p>
                <p><i class="fas fa-user-alt"></i>&nbsp;<asp:Label ID="Fullname" runat="server" Text="Fullname" /></p>
                <p><i class="fas fa-eye"></i>&nbsp;<asp:Label ID="View" runat="server" Text="View" /></p>
            </div>
        </div>
    </div>
    <div class='mb-3 mt-3'>
        <div dir="auto">
            <div class="row">
                <div class="col-lg-12 col-md-12 StyleThirdColumn">
                    <br />
                    <p style="text-align:justify;font-weight:normal;overflow-wrap:break-word;"><asp:Label ID="Content" runat="server" Text="Content" /></p>
                </div>
            </div>
        </div>
    </div>
    <div class='mb-3 mt-3'>
        <div class="row text-center">
            <div class="col-lg-6 col-md-12 col-sm-12 Location-Audio-Attachment">
                <asp:LinkButton ID="DownloadAttachment" runat="server" OnClick="DownloadAttachment_Click" Visible="false"><i class='fas fa-download' style="font-size:24px;color:#ff0000"></i></asp:LinkButton>
            </div> 
            <div class="col-lg-6 col-md-12 col-sm-12 Location-Audio-Attachment">
                <asp:Literal ID="AudioCounseling" runat="server" Visible="false"></asp:Literal>
            </div>
        </div>
    </div>
</div>

<%-- الردود --%>
<div id="DivReplyConsultation" runat="server" style="direction:rtl;" visible="true">
</div>

<%-- اضافة رد --%>
<br />
<div class="container-md StyleBox" id="DivReply" runat="server" visible="false" style="direction:rtl;">
    <div class="mb-3 mt-3 text-center" id="ViewDataChangeCondition" runat="server" visible="false">
        <div class="row">
            <div class="col-lg-12 col-md-12 col-sm-12 StyleColumn StyleH6">
                <asp:Label ID="TypeCondition" runat="server" />
            </div>
        </div>
        <div class="row StyleColumn">
            <div class="col-lg-6 col-md-12 col-sm-12 Location-Button">
                <asp:Button ID="BChangeCondition" CssClass="form-control btn btn-warning" runat="server" Text="تحويل إلى استشارة خاصة مجانية"  OnClick="BChangeCondition_Click"/>
            </div>
            <div class="col-lg-6 col-md-12 col-sm-12 Location-Button">
                <asp:Button ID="BStopConversation" CssClass="form-control btn btn-warning" runat="server" Text="إيقاف الدردشة" OnClick="BStopConversation_Click" />
            </div>
        </div>
    </div>
    <div class="mb-3 mt-3 text-center">
        <div class="row">
            <div class="col-lg-12 col-md-12 col-sm-12 StyleFourthColumn ">
                <asp:Label ID="Label1" runat="server" Text="إضافة رد" CssClass="StyleH1-2" />
            </div>
        </div>
    </div>
    <div class="mb-3 mt-3 text-center StyleColumn" >
        <div class="row">
            <div class="col-lg-12 col-md-12 col-sm-12 StyleFourthColumn StyleH1-2">
                <asp:TextBox ID="TBReply" runat="server" TextMode="MultiLine" Width="95%"></asp:TextBox>
            </div>
        </div>       
    </div>
    <div class="mb-3 mt-3">
        <div class="row text-center">
            <div class="col-lg-6 col-md-12 col-sm-12 Location-Button" >
                <label class="file-upload">
                    <span><strong><i class='fas fa-paperclip' style="font-size:24px;"></i></strong></span>
                    <asp:FileUpload ID="FUAttachment" runat="server" Width="60px">
                    </asp:FileUpload>
                </label>
            </div>
            <div class="col-lg-6 col-md-12 col-sm-12 Location-Button" >
                <button id="RecordSound" style='font-size:24px'><i class='fas fa-microphone'></i></button></div>
                <asp:HiddenField ID="fn" runat="server" />
                <script type="text/javascript">
                        function uploadFile(blob) {
                            $.ajax({
                                url: "FileUpload.ashx",
                                type: "POST",
                                data: blob,
                                contentType: false,
                                processData: false,
                                success: function (result) {
                                    console.log(result);
                                    $('#' + '<% = fn.ClientID %>').val(result);
                                },
                                error: function (err) {
                                }
                            });
                        }
                        $('#RecordSound').click(function () {
                            var stopRecord = "<i class='fas fa-microphone-slash'></i>";
                            var startRecord = "<i class='fas fa-microphone'></i>";
                            if (!$('#RecordSound').html().includes('slash')) {
                                toggleRecording(false);
                                $('#RecordSound').html(stopRecord);
                            } else {
                                $('#RecordSound').html(startRecord);
                                toggleRecording(true);
                            }
                            return false;
                        });
                    </script>
        </div>
    </div>
    <div class="mb-3 mt-3 text-center StyleColumn" >
        <div class="row">
            <div class="col-lg-12 col-md-12" >
                <asp:Button ID="BReply" runat="server" Text="الرد" OnClick="BReply_Click" CssClass="form-control btn btn-warning" Width="40%" />
            </div>
        </div>
    </div>
    <div class="mb-3 mt-3 text-center" >
        <div class="row">
            <div class="col-lg-12 col-md-12" >
                 <br /><asp:Label ID="LNotReply" runat="server" CssClass="Verify"></asp:Label>
            </div>
        </div>
    </div>
</div>

<%-- Div No Show Consulting--%>
<div class="container-md StyleBox" id="DivNoShowConsulting" runat="server" visible="false" >
    <div class="row">
        <div class="col-lg-3 col-md-12 StyleFirstColumn">
            <br />
            <asp:Image ID="Image2" runat="server" ImageUrl="~/Images/OnlineConsulting.png" Width="90%" />
            <br />
        </div>
        <div class="col-lg-9 col-md-12 text-center" style="direction:rtl">
            <br />
            <asp:Label ID="LNoShowConsulting" runat="server" Text="لا يمكن عرض الاستشارة التي طلبتها الآن. فقد تكون غير متوفرة مؤقتاً، أو إن الرابط الذي ضغطت عليه غير كامل أو منتهي الصلاحية، أو أنه ليس لديك إذن بعرض الاستشارة باعتبارها استشارة خاصة. "></asp:Label>
            <br />
            <div class="row">
                <div class="col-lg-6 col-md-12" style="direction:rtl;">
                    <asp:HyperLink ID="HLBackPage2" runat="server"  >عودة الى الصفحة السابقة</asp:HyperLink>
                </div>
                <div class="col-lg-6 col-md-12" style="direction:rtl;">
                    <asp:HyperLink ID="HLIndex2" runat="server" NavigateUrl="~/Index.aspx">الصفحة الرئيسية</asp:HyperLink>
                </div>
            </div>
        </div>
    </div>
</div>

<%-- Div The page can't be displayed --%>
<div class="container-md StyleBox" id="DivCannotDisplay" runat="server" visible="false" >
    <div class="mb-3 mt-3 text-center">
        <br />
        <asp:Label ID="LCannotDisplay" runat="server" Text="لا يمكن عرض الصفحة التي طلبتها الآن. فقد تكون غير متوفرة مؤقتاً، أو إن الرابط الذي ضغطت عليه غير كامل أو منتهي الصلاحية، أو أنه ليس لديك إذن بعرض الصفحة "></asp:Label>
        <br />
    </div>
    <div class="mb-3 mt-3 text-center">
        <div class="row">
            <div class="col-lg-6 col-md-12" style="direction:rtl;">
                <asp:HyperLink ID="HLBackPage" runat="server"  >عودة الى الصفحة السابقة</asp:HyperLink>
            </div>
            <div class="col-lg-6 col-md-12" style="direction:rtl;">
                <asp:HyperLink ID="HLIndex" runat="server" NavigateUrl="~/Index.aspx">الصفحة الرئيسية</asp:HyperLink>
            </div>
        </div>
        <br />
    </div>
</div>
</asp:Content>
