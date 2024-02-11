<%@ Page Title="" Language="C#" MasterPageFile="~/HeaderWebsite.Master" AutoEventWireup="true" CodeBehind="OnlineConsulting.aspx.cs" Inherits="AlJundiLawFirm.LegalAdvice.OnlineConsulting" %>
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

<%-- Div Online Consulting--%>
<br />
<div class="container-md StyleBox" id="DivOnlineConsulting" runat="server">
    <div class="row">
        <div class="col-lg-3 col-md-12 StyleFirstColumn">
            <asp:Image ID="Image1" runat="server" ImageUrl="~/Images/OnlineConsulting.png" Width="90%" />
            <br />
            <br />
            <asp:Label ID="LOnlineConsultingDetails" runat="server" Text="يبذل محامي في موقع الجندي للاستشارات القانونية كل الجهد لنيل ثقتكم ووضع خبراته القانونية بين أيديكم من خلال الرد على استشاراتكم القانونية، والتي ستحظى بكل أهمية وخصوصية مطلقة والسرية الكاملة لجميع البيانات المرسلة من قبلكم.<br>يرجى إدخال البيانات كاملة وصحيحة وعدم إغفالها ليتمكن المحامي من الإجابة على استشارتكم في الموقع."></asp:Label>
            <br />
            <br />
            <asp:HyperLink ID="HLAskOnlineLegalAdvice" runat="server" Target="_blank" NavigateUrl="~/LegalAdvice/AskOnlineLegalAdvice.aspx" CssClass="Hyper-Link-Go">ماهي الاستشارات القانونية اونلاين؟</asp:HyperLink>
        </div>
        <div class="col-lg-9 col-md-12 StyleSecondColumn" style="direction:rtl;text-align:right;">
            <div class="mb-3 mt-3">
                <br />
                <p class="text-center"><asp:Label ID="LTSignup" runat="server" CssClass="StyleH1-2" Text="استشارات اونلاين عبر موقع الجندي لاستشارات القانونية"></asp:Label></p>
            </div>
            <div class="mb-3 mt-3">
                <asp:Label ID="LSubjectConsulting" runat="server" For="TBSubjectConsulting">عنوان الاستشارة القانونية:<span style="color:red">*</span></asp:Label>
                <br />
                <asp:TextBox ID="TBSubjectConsulting" runat="server" placeholder="ادخل عنوان الاستشارة" CssClass="form-control"></asp:TextBox>
            </div>
            <div class="mb-3 mt-3">
                <asp:Label ID="LContentConsulting" runat="server" For="TBContentConsulting">محتوى الاستشارة القانونية:<span style="color:red">*</span> </asp:Label>
                <span class="Not">الحد الأدنى لعدد الأحرف 150 حرف، إلا بحال كان هناك تسجل صوتي.</span>
                <br />
                <asp:TextBox ID="TBContentConsulting" runat="server" TextMode="MultiLine"  placeholder="ادرج استشارتك هنا" CssClass="form-control"></asp:TextBox>
            </div>
            <div class="mb-3 mt-3">
                <div class="row text-center">
                    <div class="col-lg-6 col-md-12 col-sm-12 Location-Button"> 
                        <label class="file-upload">
                            <span><strong><i class='fas fa-paperclip' style="font-size:24px;"></i></strong></span>
                            <asp:FileUpload ID="FUAttachment" runat="server" Width="60px">
                            </asp:FileUpload>
                        </label>
                    </div>
                    <div class="col-lg-6 col-md-12 col-sm-12 Location-Button"> 
                        <button id="RecordSound" style='font-size:24px'><i class='fas fa-microphone'></i></button>
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
            </div>
            <div class="mb-3 mt-3">
                <asp:Label ID="LViewConsulting" runat="server" For="TBContentConsulting">نوع الاستشارة:</asp:Label>
                <div class="row" style="background-color:rgba(255, 193, 7, 0.35);margin-left:5px;">
                    <div class="col-lg-9 col-md-12" >
                        <asp:RadioButton ID="RBViewConsulting0" runat="server" GroupName="RBLViewConsulting" Checked="true"/>&nbsp;&nbsp;استشارة مجانية
                        <br />
                        <asp:Label ID="NotTypeConsulting1" runat="server">
                            <span class="Not2">هذه الاستشارة عامة، سوف أنتظر دوري في الاجابة (قد تصل مدة الانتظار اسبوعين)</span>
                        </asp:Label>
                    </div>
                    <div class="col-lg-3 col-md-12 text-center" style="margin:auto">مجاناً</div>
                </div>
                <div class="row" style="margin-left:5px;">
                    <div class="col-lg-9 col-md-12">
                        <asp:RadioButton ID="RBViewConsulting1" runat="server" GroupName="RBLViewConsulting"/>&nbsp;&nbsp;استشارة خاصة مجانية
                        <br />
                        <asp:Label ID="NotTypeConsulting2" runat="server" >
                            <span class="Not2">هذه الاستشارة خاصة، سوف أنتظر دوري في الاجابة (قد تصل مدة الانتظار اسبوعين) </span>
                        </asp:Label>
                    </div>
                    <div class="col-lg-3 col-md-12 text-center" style="margin:auto">مجاناً</div>
                </div>
                <div class="row" style="background-color:rgba(255, 193, 7, 0.35);margin-left:5px;">
                    <div class="col-lg-9 col-md-12">
                        <asp:RadioButton ID="RBViewConsulting2" runat="server" GroupName="RBLViewConsulting"/>&nbsp;&nbsp;استشارة خاصة مأجورة
                        <br />
                        <asp:Label ID="Label1" runat="server" >
                            <span class="Not2">
                                هذه الأستشارة خاصة، سوف يعمل المحامي على الإجابة مباشرة بعد الاستلام المبلغ و ذلك خلال 72 ساعة
                            </span>
                        </asp:Label>
                    </div>
                    <div class="col-lg-3 col-md-12 text-center" style="margin:auto">مأجورة</div>
                </div>
            </div>
            <div class="mb-3 mt-3">
                <asp:Button ID="BSendConsulting" runat="server" Text="ارسال الاستشارة" OnClick="BSendConsulting_Click"/>
                <asp:Label ID="LNotSendConsulting" runat="server" ></asp:Label>
                <br />
                <br />
                <br />
            </div>
        </div>
    </div>
</div>

<%-- Div No Online Consulting--%>
<div class="container-md StyleBox" id="DivNoOnlineConsulting" runat="server" visible="false">
    <div class="row">
        <div class="col-lg-3 col-md-12 StyleFirstColumn ">
            <br />
            <asp:Image ID="Image2" runat="server" ImageUrl="~/Images/OnlineConsulting.png" Width="90%" />
            <br />
        </div>
        <div class="col-lg-9 col-md-12 StyleSecondColumn text-center" style="direction:rtl">
            <br />
            <p class="text-center"><asp:Label ID="Label3" runat="server" CssClass="StyleH1-2" Text="استشارات اونلاين عبر موقع الجندي لاستشارات القانونية"></asp:Label></p>
            <asp:Label ID="LOnlineConsultingDetails2" runat="server" Text="يبذل محامي في موقع الجندي للاستشارات القانونية كل الجهد لنيل ثقتكم ووضع خبراته القانونية بين أيديكم من خلال الرد على استشاراتكم القانونية، والتي ستحظى بكل أهمية وخصوصية مطلقة والسرية الكاملة لجميع البيانات المرسلة من قبلكم."></asp:Label>
            <br />
            <br />
            <asp:Label ID="LLogIn" runat="server" Text="يرجى تسجيل الدخول لتتمكن من ارسال الاستشارة اونلاين، "></asp:Label>
            <asp:HyperLink ID="HLLogIn" runat="server" NavigateUrl="~/UserAccount/Login.aspx" CssClass="Hyper-Link-Go">تسجيل الدخول</asp:HyperLink>
            <br />
            <br />
            <asp:HyperLink ID="HLAskOnlineLegalAdvice2" runat="server" NavigateUrl="~/LegalAdvice/AskOnlineLegalAdvice.aspx" CssClass="Hyper-Link-Go">ماهي الاستشارات القانونية اونلاين؟</asp:HyperLink>
            <br />
            <br />
        </div>
    </div>
</div>

<%-- Div The page can't be displayed --%>
<div class="container-md StyleBox StyleColumn" id="DivCannotDisplay" runat="server" visible="false">
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
