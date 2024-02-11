using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AlJundiLawFirm.App_Code;
using AlJundiLawFirm.Models;
using System.IO;

namespace AlJundiLawFirm.LegalAdvice
{
    public partial class ShowConversation : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            HttpCookie repCookies = Request.Cookies["UserInfoForAlJundiLaw"];
            if (repCookies != null)
            {
                Session["Role"] = Encrypt.decryptQueryString(repCookies["Role"].ToString().Replace(" ", "+"));
                Session["User"] = Encrypt.decryptQueryString(Server.UrlDecode(repCookies["User"].ToString().Replace(" ", "+")));
            }
            else
            {
                Session["Role"] = "4";
                Session["User"] = "0";
            }

            if(!IsPostBack)
            {
                ViewConsulting();
            }
        }

        // Get Ip Address
        public string GetIpAddress()
        {
            string VisitorIpAddress = string.Empty;
            if (HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != null)
            {
                VisitorIpAddress = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString();
            }
            else if (HttpContext.Current.Request.UserHostAddress != null)
            {
                VisitorIpAddress = HttpContext.Current.Request.UserHostAddress;
            }

            return VisitorIpAddress;
        }

        private void DoNotView()
        {
            Session["Role"] = "0";
            Session["User"] = "0";
            DivShowConversation.Visible = false;
            DivNoShowConsulting.Visible = false;
            DivCannotDisplay.Visible = true;
            HLBackPage.NavigateUrl = (Request.UrlReferrer == null) ? "~/index.aspx" : Request.UrlReferrer.ToString();
            HLIndex.NavigateUrl = "~/index.aspx";
        }

        private void DoNotViewConsulting()
        {
            Session["Role"] = "4";
            Session["User"] = "0";
            DivShowConversation.Visible = false;
            DivNoShowConsulting.Visible = true;
            DivCannotDisplay.Visible = false;
            HLBackPage2.NavigateUrl = (Request.UrlReferrer == null) ? "~/index.aspx" : Request.UrlReferrer.ToString();
            HLIndex2.NavigateUrl = "~/index.aspx";
        }

        private void ViewConsulting()
        {
            try
            {
                // Find Number Consultation To View 
                int NumConsultation = 0;
                if (!string.IsNullOrEmpty(Request.QueryString["Num"]))
                {
                    NumConsultation = int.Parse(Request.QueryString["Num"].ToString().Replace(" ", "+"));
                }
                else
                {
                    NumConsultation = 0;
                }

                if (NumConsultation != 0)
                {
                    bool ViewConsultation = true; // View Consultation
                    bool ViewControlFile = false; // View Controls File : Audio Recording, Download Attachment, Text area to ConversationReplies

                    // View information of the user who 'Visit' the Consultation
                    int IdUserVisit = 0;
                    if (Session["User"] != null && Session["User"].ToString() != "0")
                    {
                        try
                        {
                            IdUserVisit = int.Parse(Session["User"].ToString());
                        }
                        catch
                        {
                            ViewConsultation = false;
                        }
                    }
                    else
                    {
                        IdUserVisit = 0;
                    }

                    Conversations viewConsultation = Conversations.ViewConsultation(NumConsultation);
                    if (viewConsultation != null)
                    {
                        int IdUserWrote = viewConsultation.ID_USER; // ID user who 'Wrote' the Consultation
                        int IdShareChat = viewConsultation.SHARE_CHAT; // Private if IdShareChat = 0
                        if (IdShareChat == 0) // public
                        {
                            ViewConsultation = true;
                        }
                        else if (IdShareChat == 1 || IdShareChat == 2) // Private
                        {
                            ViewConsultation = false;
                        }

                        if (IdUserVisit == IdUserWrote) // View the Consultation if the advisor wrote the Consultation
                        {
                            ViewConsultation = true;
                            ViewControlFile = true;
                        }

                        int Id_Role = 0;
                        if (Session["Role"] != null && Session["Role"].ToString() != "0") // View the Consultation of who is lawyer
                        {
                            Id_Role = int.Parse(Session["Role"].ToString());
                            List<RolePermission> ListPermission = RolePermission.GetIdPermissions(Id_Role);
                            if (ListPermission != null && ListPermission.Count > 0)
                            {
                                var item = ListPermission.FirstOrDefault(o => o.ID_PERMISSION == 15);
                                if (item != null)
                                {
                                    ViewConsultation = true;
                                    ViewControlFile = true;
                                    //// View Data Change Condition
                                    ViewDataChangeCondition.Visible = true;
                                    int NumConsultationType = viewConsultation.SHARE_CHAT;
                                    string ConsultationType = "";
                                    if (NumConsultationType == 0) { ConsultationType = "استشارة مجانية"; }
                                    else if (NumConsultationType == 1) { ConsultationType = "استشارة خاصة مجانية"; }
                                    else if (NumConsultationType == 2) { ConsultationType = "استشارة خاصة مأجورة"; }
                                    else { ConsultationType = "غير معروف نوع الاستشارة"; }
                                    TypeCondition.Text += ConsultationType;
                                }
                            }
                        }


                        if (ViewConsultation == true)
                        {
                            // View information of the user who 'Wrote' the Consultation
                            Users InfoUserWroteConsultation = Users.SelectInfoUser(IdUserWrote);
                            if (InfoUserWroteConsultation != null)
                            {
                                ImageUserWroteConsultation.ImageUrl = "../ImagesOfUsers/" + InfoUserWroteConsultation.PERSONAL_PICTURE;
                                CreateDate.Text = viewConsultation.CREATE_DATE.ToString();
                                Fullname.Text = InfoUserWroteConsultation.FULL_NAME;
                                View.Text = Statistics.SumViewConversation(viewConsultation.VIEW_CONVERSATION);
                                Subject.Text = viewConsultation.SUBJECT;
                                Content.Text = viewConsultation.CONSULTATION.Replace("\n", "<br/>").Replace("\b", "<b>");

                                Page.Title = "الجندي للاستشارات القانونية - " + viewConsultation.SUBJECT;
                                // View Control File
                                if (ViewControlFile == true)
                                {
                                    // View the user's audio recording of this consultation
                                    if (viewConsultation.AUDIO_RECORDING != "")
                                    {
                                        AudioCounseling.Visible = true;
                                        AudioCounseling.Text = "<audio controls>" +
                                                               "<Source src='../Recording/" + viewConsultation.AUDIO_RECORDING + "' type='audio/mp3'/>" +
                                                               "<Source src='../Recording/" + viewConsultation.AUDIO_RECORDING + "' type='audio/wav'/>" +
                                                               "<Source src='../Recording/" + viewConsultation.AUDIO_RECORDING + "' type='audio/ogg'/>" +
                                                               "<Source src='../Recording/" + viewConsultation.AUDIO_RECORDING + "' type='audio/mpeg'/>" +
                                                               "</audio>";
                                    }
                                    // View the user's attachment of this consultation
                                    if (viewConsultation.ATTACHMENT != "")
                                    {
                                        DownloadAttachment.Visible = true;
                                        Session["DownloadAttachment"] = viewConsultation.ATTACHMENT;
                                    }

                                    // Show reply text area  
                                    if (viewConsultation.LOCK_SHARE_CHAT == false)
                                    {
                                        DivReply.Visible = true;
                                    }
                                }

                                // View all replies for this Id Consultation
                                List<ConversationReplies> replies = ConversationReplies.SelectConversationReplies(NumConsultation);
                                if (replies != null && replies.Count > 0)
                                {
                                    string ImageUserReplay = "";
                                    string FullNameUserReplay = "";
                                    string DateUserReplay = "";
                                    int IdUserReplay = 0;
                                    string ReplayUser = "";
                                    string AudioRecordingUserReplay = "";
                                    string AttachmentUserReplay = "";

                                    DivReplyConsultation.InnerHtml = "";
                                    foreach (ConversationReplies r in replies)
                                    {
                                        // Featch Information User Replay 
                                        IdUserReplay = r.ID_USER;
                                        Users InfoUserReplay = Users.SelectInfoUser(IdUserReplay);
                                        if (InfoUserReplay != null)
                                        {
                                            ImageUserReplay = InfoUserReplay.PERSONAL_PICTURE;
                                            FullNameUserReplay = InfoUserReplay.FULL_NAME;
                                            DateUserReplay = r.CREATE_DATE.ToString();
                                            ReplayUser = r.REPLY.ToString().Replace("\n", "<br />");

                                            DivReplyConsultation.InnerHtml += "<br />";
                                            DivReplyConsultation.InnerHtml += "<div class='container-md StyleBox'>";
                                            DivReplyConsultation.InnerHtml += "<div class='mb-3 mt-3 text-center'>";
                                            DivReplyConsultation.InnerHtml += "<div class='row'>";
                                            DivReplyConsultation.InnerHtml += "<div class='col-lg-1 col-md-3 col-sm-12 text-center StyleFourthColumn'>";
                                            DivReplyConsultation.InnerHtml += "<img src='../ImagesOfUsers/" + ImageUserReplay + "' class='rounded-circle SizeImage' width='90%' />";
                                            DivReplyConsultation.InnerHtml += "</div>";
                                            DivReplyConsultation.InnerHtml += "<div class='col-lg-11 col-md-9 col-sm-12 StyleFourthColumn' style='text-align:justify'>";
                                            DivReplyConsultation.InnerHtml += "<p><i class='fas fa-user-alt'></i>&nbsp;&nbsp;" + FullNameUserReplay + "</p>";
                                            DivReplyConsultation.InnerHtml += "<p><i class='fa far fa-clock'></i>&nbsp;&nbsp;" + DateUserReplay + "</p>";
                                            DivReplyConsultation.InnerHtml += "</div>";
                                            DivReplyConsultation.InnerHtml += "</div>";
                                            DivReplyConsultation.InnerHtml += "</div>";
                                            DivReplyConsultation.InnerHtml += "<div class='mb-3 mt-3 StyleColumn'>";
                                            DivReplyConsultation.InnerHtml += "<p dir='auto' style='text-align:justify;font-weight:normal;overflow-wrap:break-word;'>" + ReplayUser + "</p>";
                                            DivReplyConsultation.InnerHtml += "</div>";

                                            if (ViewControlFile == true)
                                            {
                                                AudioRecordingUserReplay = r.AUDIO_RECORDING;
                                                AttachmentUserReplay = r.ATTACHMENT;
                                                if (AudioRecordingUserReplay != "" || AttachmentUserReplay != "")
                                                {
                                                    DivReplyConsultation.InnerHtml += "<div class='mb-3 mt-3'>";
                                                    DivReplyConsultation.InnerHtml += "<div class='row text-center'>";
                                                    DivReplyConsultation.InnerHtml += "<div class='col-lg-6 col-md-12 col-sm-12 Location-Audio-Attachment'>";
                                                    if (AudioRecordingUserReplay != "")
                                                    {
                                                        DivReplyConsultation.InnerHtml += "<audio controls>";
                                                        DivReplyConsultation.InnerHtml += "<Source src='../Recording/" + AudioRecordingUserReplay + "' type='audio/mp3'/>";
                                                        DivReplyConsultation.InnerHtml += "<Source src='../Recording/" + AudioRecordingUserReplay + "' type='audio/wav'/>";
                                                        DivReplyConsultation.InnerHtml += "<Source src='../Recording/" + AudioRecordingUserReplay + "' type='audio/ogg'/>";
                                                        DivReplyConsultation.InnerHtml += "<Source src='../Recording/" + AudioRecordingUserReplay + "' type='audio/mpeg'/>";
                                                        DivReplyConsultation.InnerHtml += "</audio>";
                                                    }
                                                    DivReplyConsultation.InnerHtml += "</div>";

                                                    DivReplyConsultation.InnerHtml += "<div class='col-lg-6 col-md-12 col-sm-12 Location-Audio-Attachment'>";
                                                    if (AttachmentUserReplay != "")
                                                    {
                                                        string filename = Request.Url.Scheme + "://" + Request.Url.Host + ":" + Request.Url.Port + "/Attachment/" + AttachmentUserReplay;
                                                        DivReplyConsultation.InnerHtml += "<a href='" + filename + "' download><i class='fas fa-download' style='font-size:24px;color:#ff0000'></i></a>";
                                                    }
                                                    DivReplyConsultation.InnerHtml += "</div>";

                                                    DivReplyConsultation.InnerHtml += "</div>";
                                                    DivReplyConsultation.InnerHtml += "</div>";
                                                }
                                            }
                                            DivReplyConsultation.InnerHtml += "</div>";
                                        }
                                    }
                                    DivReplyConsultation.InnerHtml += "<br />";
                                }

                                // Add a row indicates that the user saw a consultation
                                string IpAddress = GetIpAddress();
                                bool InsertViewConsultation = ViewConversations.InsertViewConsultation(NumConsultation, IdUserVisit, IpAddress);
                                if (InsertViewConsultation == true)
                                {
                                    Conversations.UpdateViewConversation(NumConsultation);
                                }
                            }
                            else
                            {
                                DoNotViewConsulting();
                            }
                        }
                        else
                        {
                            DoNotViewConsulting();
                        }
                    }
                    else
                    {
                        DoNotView();
                    }
                }
                else
                {
                    DoNotView();
                }
            }
            catch
            {
                DoNotView();
            }
        }

        protected void BReply_Click(object sender, EventArgs e)
        {
            try
            {
                bool checkfileupload = true;

                // Find Number Consultation To View 
                int NumConsultation = 0;
                if (!string.IsNullOrEmpty(Request.QueryString["Num"]))
                {
                    NumConsultation = int.Parse(Request.QueryString["Num"].ToString().Replace(" ", "+"));
                }
                else
                {
                    NumConsultation = 0;
                }

                // View information of the user who 'Reply' to Consultation
                int IdUserReply = 0;
                if (Session["User"] != null && Session["User"].ToString() != "0")
                {
                    try
                    {
                        IdUserReply = int.Parse(Session["User"].ToString());
                    }
                    catch
                    {
                        checkfileupload = false;
                    }
                }
                else
                {
                    checkfileupload = false;
                }

                string Reply = TBReply.Text.Trim();


                // for Attachment 
                string AttachmentName = "";
                if (FUAttachment.HasFile)
                {
                    try
                    {
                        string ext = Path.GetExtension(FUAttachment.FileName).ToLower();
                        if (ext == ".txt" || ext == ".doc" || ext == ".docx" || ext == ".pdf" || ext == ".rtf" || ext == ".jpg" || ext == ".jpeg"
                            || ext == ".gif" || ext == ".png" || ext == ".jfif" || ext == ".bmp")
                        {
                            DateTime todayDate = DateTime.Now;
                            string date = todayDate.ToString("MMddyyyy-hhmmss");

                            AttachmentName = IdUserReply.ToString() + "-" + date + ext;
                            int filesize = FUAttachment.PostedFile.ContentLength;
                            if (filesize <= 26214400)
                            {
                                FUAttachment.SaveAs(Server.MapPath("~/Attachment/" + AttachmentName));
                            }
                            else
                            {
                                LNotReply.Text += "حجم الملف أكبر من الحجم المسموح به 25 ميغا بايت";
                                checkfileupload = false;
                            }
                        }
                        else
                        {
                            LNotReply.Text += "* نوع الملف المحدد غير مسموح به! نوع الملفات المسموح تحميلها (.bmp - .doc - .docx - .gif - .jfif - .jpeg - .jpg - .pdf - .png - .rtf - .txt)";
                            checkfileupload = false;
                        }
                    }
                    catch
                    {
                        LNotReply.Text += "* حدث خطأ ما أثناء تحميل الملف، الرجاء اعادة محاولة مرة أخرى <br />";
                        checkfileupload = false;
                    }
                }
                else
                {
                    checkfileupload = true;
                }

                // For recording          
                var AudioRecording = string.IsNullOrEmpty(fn.Value) ? "" : fn.Value;
                if (AudioRecording != "")
                {
                    if (Reply == "")
                    {
                        Reply = "<b><u> هناك تسجيل صوتي </u></b>";
                    }
                    else
                    {
                        Reply += "\n<b><u> هناك تسجيل صوتي </u></b>";
                    }
                }
                else
                {
                    if (Reply == "")
                    {
                        LNotReply.Text = "* الرجاء كتابة الرد ";
                        checkfileupload = false;
                    }
                }


                if (checkfileupload == true)
                {
                    bool InsertReply = ConversationReplies.InsertConversationReply(NumConsultation, IdUserReply, Reply, AttachmentName, AudioRecording);
                    if (InsertReply == true)
                    {
                        int Id_Role = 0;
                        if (Session["Role"] != null && Session["Role"].ToString() != "0") // View the Consultation of who is lawyer
                        {
                            Id_Role = int.Parse(Session["Role"].ToString());
                            List<RolePermission> ListPermission = RolePermission.GetIdPermissions(Id_Role);
                            if (ListPermission != null && ListPermission.Count > 0)
                            {
                                var item = ListPermission.FirstOrDefault(o => o.ID_PERMISSION == 15);
                                if (item != null)
                                {
                                    Conversations.UpdateCountAnswers(NumConsultation);
                                }
                                else
                                {
                                    Conversations.UpdateDateReplySubscriber(NumConsultation);
                                }
                            }
                            else
                            {
                                Conversations.UpdateDateReplySubscriber(NumConsultation);
                            }
                        }

                        Response.Redirect(Request.RawUrl);
                    }
                    else
                    {
                        LNotReply.Text = "* لم يتم إرسال الرد، الرجاء المحاولة مرة أخرى.";
                    }
                }
            }
            catch
            {
                DoNotView();
            }
        }

        protected void BChangeCondition_Click(object sender, EventArgs e)
        {
            // Find Number Consultation To View 
            int NumConsultation = 0;
            if (!string.IsNullOrEmpty(Request.QueryString["Num"]))
            {
                NumConsultation = int.Parse(Request.QueryString["Num"].ToString().Replace(" ", "+"));
                Conversations.ChangeToFreePrivateConsultation(NumConsultation);
                Response.Redirect(Request.RawUrl);
            }
            else
            {
                NumConsultation = 0;
            }
        }

        protected void BStopConversation_Click(object sender, EventArgs e)
        {
            // Find Number Consultation To View 
            int NumConsultation = 0;
            if (!string.IsNullOrEmpty(Request.QueryString["Num"]))
            {
                NumConsultation = int.Parse(Request.QueryString["Num"].ToString().Replace(" ", "+"));
                Conversations.LockShareChat(NumConsultation);
                Response.Redirect(Request.RawUrl);
            }
            else
            {
                NumConsultation = 0;
            }
        }

        protected void DownloadAttachment_Click(object sender, EventArgs e)
        {
            string filename = Server.MapPath("~/Attachment/" + Session["DownloadAttachment"].ToString());
            FileInfo fileInfo = new FileInfo(filename);

            if (fileInfo.Exists)
            {
                Response.Clear();
                Response.AppendHeader("Content-Disposition", "attachment; filename=" + fileInfo.Name);
                string ext = fileInfo.Extension;
                if (ext == ".doc" || ext == ".docx")
                {
                    Response.ContentType = "Application/msword";
                }
                else if (ext == ".txt")
                {
                    Response.ContentType = "text/plain";
                }
                else if (ext == ".pdf")
                {
                    Response.ContentType = "Application/pdf";
                }
                else
                {
                    Response.ContentType = "application/octet-stream";
                }
                Response.TransmitFile(fileInfo.FullName);
                Response.End();
            }
        }
    }
}