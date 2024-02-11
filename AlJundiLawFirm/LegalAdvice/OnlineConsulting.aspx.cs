using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AlJundiLawFirm.App_Code;
using AlJundiLawFirm.Models;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;

namespace AlJundiLawFirm.LegalAdvice
{
    public partial class OnlineConsulting : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Page.Title = "الجندي للاستشارات القانونية - طلب استشارة جديدة";
            HttpCookie repCookies = Request.Cookies["UserInfoForAlJundiLaw"];
            if (repCookies != null)
            {
                Session["Role"] = Encrypt.decryptQueryString(repCookies["Role"].ToString().Replace(" ", "+"));
                Session["User"] = Encrypt.decryptQueryString(Server.UrlDecode(repCookies["User"].ToString().Replace(" ", "+")));
            }
            else
            {
                Session["Role"] = "4";
            }

            if (!IsPostBack)
            {
                ViewConsulting();
            }
        }

        private void ViewConsulting()
        {
            try
            {
                int Id_Role = 0;
                if (Session["Role"] != null && Session["Role"].ToString() != "0")
                {
                    Id_Role = int.Parse(Session["Role"].ToString());
                }

                if (Id_Role != 0)
                {
                    List<RolePermission> ListPermission = RolePermission.GetIdPermissions(Id_Role);
                    if (ListPermission != null && ListPermission.Count > 0)
                    {
                        var item = ListPermission.FirstOrDefault(o => o.ID_PERMISSION == 6);
                        if (item != null)
                        {
                            DivOnlineConsulting.Visible = true; 
                            DivNoOnlineConsulting.Visible = false;
                            DivCannotDisplay.Visible = false;
                        }
                        else
                        {
                            DivOnlineConsulting.Visible = false;
                            DivNoOnlineConsulting.Visible = true;
                            DivCannotDisplay.Visible = false;
                        }
                    }
                    else
                    {
                        DivOnlineConsulting.Visible = false;
                        DivNoOnlineConsulting.Visible = true;
                        DivCannotDisplay.Visible = false;
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

        private void DoNotView()
        {
            Session["Role"] = "0";
            Session["User"] = "0";
            DivOnlineConsulting.Visible = false;
            DivNoOnlineConsulting.Visible = false;
            DivCannotDisplay.Visible = true;
            HLBackPage.NavigateUrl = (Request.UrlReferrer == null) ? "~/index.aspx" : Request.UrlReferrer.ToString();
            HLIndex.NavigateUrl = "~/index.aspx";
        }

        protected void BSendConsulting_Click(object sender, EventArgs e)
        {
            try
            {
                bool SendConsultation = true;
                string ErrorMessage = "";

                int IdUser = 0;
                if (Session["User"] != null && Session["User"].ToString() != "0")
                {
                    try
                    {
                        IdUser = int.Parse(Session["User"].ToString());
                    }
                    catch
                    {
                        SendConsultation = false;
                    }
                }
                else
                {
                    SendConsultation = false;
                }

                // Subject
                string Subject = TBSubjectConsulting.Text.Trim();
                if (Subject == "")
                {
                    ErrorMessage = "* الرجاء ادخال عنوان الاستشارة <br />";
                    SendConsultation = false;
                }

                // Content
                string Content = TBContentConsulting.Text.Trim();
                

                int ShareChat = 0;
                if (RBViewConsulting0.Checked == true)
                {
                    ShareChat = 0; // Free Public Consultation
                }
                else if (RBViewConsulting1.Checked == true)
                {
                    ShareChat = 1; // Free Private Consultation
                }
                else if (RBViewConsulting2.Checked == true)
                {
                    ShareChat = 2; // Paid Private Consultation
                }

                //  int.Parse(RBLViewConsulting.SelectedItem.Value);
                bool UrgentChat = false;
                if (ShareChat == 2)
                {
                    UrgentChat = true;
                }

                //    ATTACHMENT
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

                            AttachmentName = IdUser.ToString() + "-" + date + ext;
                            int filesize = FUAttachment.PostedFile.ContentLength;
                            if (filesize <= 26214400)
                            {
                                FUAttachment.SaveAs(Server.MapPath("~/Attachment/" + AttachmentName));
                            }
                            else
                            {
                                ErrorMessage += "حجم الملف أكبر من الحجم المسموح به 25 ميغا بايت";
                                SendConsultation = false;
                            }
                        }
                        else
                        {
                            ErrorMessage += "* نوع الملف المحدد غير مسموح به! نوع الملفات المسموح تحميلها (.bmp - .doc - .docx - .gif - .jfif - .jpeg - .jpg - .pdf - .png - .rtf - .txt)";
                            SendConsultation = false;
                        }
                    }
                    catch
                    {
                        ErrorMessage += "* حدث خطأ ما أثناء تحميل الملف، الرجاء اعادة محاولة مرة أخرى <br />";
                        SendConsultation = false;
                    }
                }

                //    AUDIO_RECORDING
                var AudioRecording =  string.IsNullOrEmpty(fn.Value) ? "" : fn.Value;
                if (AudioRecording != "")
                {
                    if (Content == "")
                    {
                        Content = "<b><u> هناك تسجيل صوتي </u></b>";
                    }
                    else
                    {
                        Content += "\n<b><u> هناك تسجيل صوتي </u></b>";
                    }
                }
                else
                {
                    if (Content == "")
                    {
                        ErrorMessage += "* الرجاء ادخال محتوى الاستشارة <br />";
                        SendConsultation = false;
                    }
                    else if (Content.Length <= 150)
                    {
                        ErrorMessage += "* الاستشارة ﻻ يجب أن تقل عن 150 حرف <br />";
                        SendConsultation = false;
                    }
                }

                if (SendConsultation == true)
                {
                    bool AddConsultation = Conversations.InsertConsultation(IdUser, Subject, Content, ShareChat, UrgentChat, AttachmentName, AudioRecording);
                    if (AddConsultation == true)
                    {
                        int LastRow = Conversations.LastIdConversation(IdUser);
                        if (LastRow != 0)
                        {
                            Response.Redirect("../LegalAdvice/ShowConversation.aspx?Num=" + LastRow);
                        }
                        else
                        {
                            LNotSendConsulting.Text = "تم الاضافة الاستشارة بنجاح ";
                            LNotSendConsulting.ForeColor = Color.Green;
                        }
                    }
                    else
                    {
                        LNotSendConsulting.Text = "حدث خطأ لأثناء ارسال الاستشارة، حاول مرة اخرى ";
                        LNotSendConsulting.ForeColor = Color.Red;
                    }
                }
                else
                {
                    LNotSendConsulting.Text = ErrorMessage;
                    LNotSendConsulting.ForeColor = Color.Red;
                }
            }
            catch
            {
                DoNotView();
            }           
        }
    }
}