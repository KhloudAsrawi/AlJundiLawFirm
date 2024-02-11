using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AlJundiLawFirm.App_Code;
using AlJundiLawFirm.Models;

namespace AlJundiLawFirm.LegalAdvice
{
    public partial class UrgentConsultations : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Page.Title = "الجندي للاستشارات القانونية - الاستشارات المأجورة";
            HttpCookie repCookies = Request.Cookies["UserInfoForAlJundiLaw"];
            if (repCookies != null)
            {
                Session["Role"] = Encrypt.decryptQueryString(repCookies["Role"].ToString().Replace(" ", "+"));
                Session["User"] = Encrypt.decryptQueryString(Server.UrlDecode(repCookies["User"].ToString().Replace(" ", "+")));
                if (!IsPostBack)
                {
                    ViewConsultationsNoAnswered();
                }
            }
            else
            {
                DoNotView();
            }
        }

        private void DoNotView()
        {
            Session["Role"] = "0";
            Session["User"] = "0";
            DivSearch.Visible = false;
            DivAnsweredConsultations.Visible = false;
            DivNoAnsweredConsultations.Visible = false;
            DivCannotDisplay.Visible = true;
            HLBackPage.NavigateUrl = (Request.UrlReferrer == null) ? "~/index.aspx" : Request.UrlReferrer.ToString();
            HLIndex.NavigateUrl = "~/index.aspx";
        }

        private void ViewConsultationsNoAnswered()
        {
            int IdRole = 0;
            if (Session["Role"] != null && Session["Role"].ToString() != "0")
            {
                IdRole = int.Parse(Session["Role"].ToString());
            }

            List<RolePermission> ListPermission = RolePermission.GetIdPermissions(IdRole);
            if (ListPermission != null && ListPermission.Count > 0)
            {
                var ViewAnsweredConsulting = ListPermission.Any(c => c.ID_PERMISSION == 18);
                if (ViewAnsweredConsulting == true)
                {
                    // Get count of Paid  consultations answered by the Lawyer without count repetition in the same consultation
                    float NumberAnsweredPaidConsultations = Statistics.NumberUrgentConsultationsNotAnswered();
                    if (NumberAnsweredPaidConsultations > 20)
                    {
                        float NumPages = 1;

                        NumPages = NumberAnsweredPaidConsultations / 20;
                        float Mod = NumPages % 1;
                        DivNumberPages.InnerHtml = "";
                        int Previous = 0;
                        int Next = 1;
                        int ThisPage = 0;

                        DivNumberPages.InnerHtml += "<ul class='pagination justify-content-center'>";

                        // Next
                        if (!string.IsNullOrEmpty(Request.QueryString["Page"]))
                        {
                            Next = int.Parse(Request.QueryString["Page"]);
                            if (Next <= NumPages)
                            {
                                DivNumberPages.InnerHtml += "<li class='page-item'><a class='page-link' style='width:auto' href='../LegalAdvice/NewConsultations.aspx?Page=" + (Next + 1) + "'>التالي</a></li>";
                            }
                        }
                        else
                        {
                            DivNumberPages.InnerHtml += "<li class='page-item'><a class='page-link' style='width:auto' href='../LegalAdvice/NewConsultations.aspx?Page=" + (Next + 1) + "'>التالي</a></li>";
                        }

                        // Pages
                        int Num = 0;
                        if (Mod > 0) { Num = Convert.ToInt32(NumPages - float.Parse(Mod.ToString())) + 1; }
                        else { Num = Convert.ToInt32(NumPages); }

                        for (float i = 0; i < NumPages; i++)
                        {
                            if (!string.IsNullOrEmpty(Request.QueryString["Page"]))
                            {
                                ThisPage = int.Parse(Request.QueryString["Page"]);
                                if (ThisPage == Num)
                                {
                                    DivNumberPages.InnerHtml += "<li class='page-item'><a class='page-link page-link-actived' href='../LegalAdvice/NewConsultations.aspx?Page=" + Num + "'>" + Num.ToString() + "</a></li>";
                                }
                                else
                                {
                                    DivNumberPages.InnerHtml += "<li class='page-item'><a class='page-link' href='../LegalAdvice/NewConsultations.aspx?Page=" + Num + "'>" + Num.ToString() + "</a></li>";
                                }
                            }
                            else
                            {
                                DivNumberPages.InnerHtml += "<li class='page-item'><a class='page-link' href='../LegalAdvice/NewConsultations.aspx?Page=" + Num + "'>" + Num.ToString() + "</a></li>";
                            }
                            Num--;
                        }

                        // Previous
                        if (!string.IsNullOrEmpty(Request.QueryString["Page"]))
                        {
                            Previous = int.Parse(Request.QueryString["Page"]) - 1;
                            if (Previous > 0)
                            {
                                DivNumberPages.InnerHtml += "<li class='page-item'><a class='page-link' style='width:auto' href='../LegalAdvice/NewConsultations.aspx?Page=" + Previous + "'>سابق</a></li>";
                            }
                        }
                        DivNumberPages.InnerHtml += "</ul>";
                    }

                    // Find the page we are standing on now
                    int rang = 0;
                    if (!string.IsNullOrEmpty(Request.QueryString["Page"]))
                    {
                        rang = (int.Parse(Request.QueryString["Page"]) - 1) * 20;
                    }

                    // VPCNA mean Fetch Paid Conversations people not have Answers
                    List<ViewConsultationsAnswers> VPCNA = ViewConsultationsAnswers.ViewListPaidConsultationsNoAnswers(rang);  // ViewConsultationsAnswers.ViewListConsultationsAnswers(Id_User, rang);
                    if (VPCNA != null && VPCNA.Count > 0)
                    {
                        DivNumberPages.Visible = true;

                        DivNoAnswered.InnerHtml = "";
                        foreach (ViewConsultationsAnswers Con in VPCNA)
                        {
                            DivNoAnswered.InnerHtml += "<div class='container-md StyleBox'>";
                            DivNoAnswered.InnerHtml += "<div class='mb-3 mt-3'>";
                            DivNoAnswered.InnerHtml += "<div class='row'  style='direction:rtl;'>";
                            DivNoAnswered.InnerHtml += "<div class='col-lg-2 col-md-3 col-sm-12 text-center StyleFourthColumn'>";
                            DivNoAnswered.InnerHtml += "<img src='../ImagesOfUsers/" + Con.PERSONAL_PICTURE_USER_ASK + "' class='rounded-circle SizeImage'/>";

                            // VIEW CONVERSATION
                            string SVIEW_CONVERSATION = Statistics.SumViewConversation(Con.VIEW_CONVERSATION);
                            DivNoAnswered.InnerHtml += "<p style='margin-top:10px'><i class='fas fa-eye'></i>&nbsp;" + SVIEW_CONVERSATION + "</p>";
                            DivNoAnswered.InnerHtml += "</div>";
                            string Subject = Con.SUBJECT.Replace(@"\s+", " ");
                            int SizeTextSubject = Subject.Length;
                            if (SizeTextSubject > 80)
                            {
                                Subject = Subject.ToString().Substring(0, 80) + " ...";
                            }

                            string Consul = Con.CONSULTATION.Replace(@"\s+", " ").Replace("<u>", "").Replace("</u>", "").Replace("<b>", "").Replace("</b>", "");
                            int SizeTextConsultation = Consul.Length;
                            if (SizeTextConsultation > 200)
                            {
                                Consul = Consul.ToString().Substring(0, 200) + " ...";
                            }

                            DivNoAnswered.InnerHtml += "<div class='col-lg-10 col-md-9 col-sm-12 StyleThirdColumn StyleP2'>";

                            // Consultation Type
                            int NumConsultationType = Con.SHARE_CHAT;
                            string ConsultationType = "";
                            if (NumConsultationType == 0) { ConsultationType = "استشارة مجانية"; }
                            else if (NumConsultationType == 1) { ConsultationType = "استشارة خاصة مجانية"; }
                            else if (NumConsultationType == 2) { ConsultationType = "استشارة خاصة مأجورة"; }
                            else { ConsultationType = "غير معروف نوع الاستشارة"; }
                            DivNoAnswered.InnerHtml += "<p class='StyleH6'>" + ConsultationType + "</p>";

                            DivNoAnswered.InnerHtml += "<p dir='auto'><a href='../LegalAdvice/ShowConversation.aspx?Num=" + Con.ID_CONVERSATION + "' class='Hyper-Link-Title'>" + Subject + "</a></p>";
                            DivNoAnswered.InnerHtml += "<p dir='auto' style='text-align:justify'>" + Consul.Replace("هناك تسجيل صوتي", "<b><u> هناك تسجيل صوتي </u></b>") + "</p><br/>";
                            DivNoAnswered.InnerHtml += "<b>";
                            DivNoAnswered.InnerHtml += "<p> سؤال بواسطة: " + Con.FULL_NAME_USER_ASK + " - بتاريخ: " + Con.CREATEDATE_ASK + "</p>";
                            DivNoAnswered.InnerHtml += "</b>";
                            DivNoAnswered.InnerHtml += "</div>";
                            DivNoAnswered.InnerHtml += "</div>";
                            DivNoAnswered.InnerHtml += "</div>";
                            DivNoAnswered.InnerHtml += "</div>";
                            DivNoAnswered.InnerHtml += "<br/>";
                        }
                    }
                    else
                    {
                        DivSearch.Visible = true;
                        DivAnsweredConsultations.Visible = false;
                        DivNoAnsweredConsultations.Visible = true;
                        DivCannotDisplay.Visible = false;
                        HLBackPage1.NavigateUrl = (Request.UrlReferrer == null) ? "~/index.aspx" : Request.UrlReferrer.ToString();
                        HLIndex1.NavigateUrl = "~/index.aspx";
                    }
                }
                else
                { DoNotView(); }
            }
            else
            {
                DoNotView();
            }
        }
    }
}