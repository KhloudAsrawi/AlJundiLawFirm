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
    public partial class AnsweredConsultations : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Page.Title = "الجندي للاستشارات القانونية - الاستشارات المجاب عليها";
            HttpCookie repCookies = Request.Cookies["UserInfoForAlJundiLaw"];
            if (repCookies != null)
            {
                Session["Role"] = Encrypt.decryptQueryString(repCookies["Role"].ToString().Replace(" ", "+"));
                Session["User"] = Encrypt.decryptQueryString(Server.UrlDecode(repCookies["User"].ToString().Replace(" ", "+")));
                if (!IsPostBack)
                {
                    ViewAnsweredConsultations();
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

        private void ViewAnsweredConsultations()
        {
            int IdRole = 0;
            if(Session["Role"] != null && Session["Role"].ToString() != "0")
            {
                IdRole = int.Parse(Session["Role"].ToString());
            }

            List<RolePermission> ListPermission = RolePermission.GetIdPermissions(IdRole);
            if (ListPermission != null && ListPermission.Count > 0)
            {
                var ViewAnsweredConsulting = ListPermission.Any(c => c.ID_PERMISSION == 18);
                if(ViewAnsweredConsulting == true)
                {
                    // Number User
                    int Id_User = 0;
                    if (Session["User"] != null && Session["User"].ToString() != "0")
                    {
                        Id_User = int.Parse(Session["User"].ToString());
                    }

                    // Get count of consultations answered by the Lawyer without count repetition in the same consultation
                    float NumberAnsweredConsultations = ViewConsultationsAnswers.NumberOfConsultationsAnswered(Id_User);
                    if (NumberAnsweredConsultations > 20)
                    {
                        float NumPages = 1;

                        NumPages = NumberAnsweredConsultations / 20;
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
                                DivNumberPages.InnerHtml += "<li class='page-item'><a class='page-link' style='width:auto' href='../LegalAdvice/AnsweredConsultations.aspx?Page=" + (Next + 1) + "'>التالي</a></li>";
                            }
                        }
                        else
                        {
                            DivNumberPages.InnerHtml += "<li class='page-item'><a class='page-link' style='width:auto' href='../LegalAdvice/AnsweredConsultations.aspx?Page=" + (Next + 1) + "'>التالي</a></li>";
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
                                    DivNumberPages.InnerHtml += "<li class='page-item'><a class='page-link page-link-actived' href='../LegalAdvice/AnsweredConsultations.aspx?Page=" + Num + "'>" + Num.ToString() + "</a></li>";
                                }
                                else
                                {
                                    DivNumberPages.InnerHtml += "<li class='page-item'><a class='page-link' href='../LegalAdvice/AnsweredConsultations.aspx?Page=" + Num + "'>" + Num.ToString() + "</a></li>";
                                }
                            }
                            else
                            {
                                DivNumberPages.InnerHtml += "<li class='page-item'><a class='page-link' href='../LegalAdvice/AnsweredConsultations.aspx?Page=" + Num + "'>" + Num.ToString() + "</a></li>";
                            }
                            Num--;
                        }

                        // Previous
                        if (!string.IsNullOrEmpty(Request.QueryString["Page"]))
                        {
                            Previous = int.Parse(Request.QueryString["Page"]) - 1;
                            if (Previous > 0)
                            {
                                DivNumberPages.InnerHtml += "<li class='page-item'><a class='page-link' style='width:auto' href='../LegalAdvice/AnsweredConsultations.aspx?Page=" + Previous + "'>سابق</a></li>";
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

                    // VCA mean Fetch Conversations people and this Lawyer whos Answers it.
                    List<ViewConsultationsAnswers> VCA = ViewConsultationsAnswers.ViewListConsultationsAnswers(Id_User, rang);
                    if (VCA != null && VCA.Count > 0)
                    {
                        DivNumberPages.Visible = true;

                        DivAnswered.InnerHtml = "";
                        foreach (ViewConsultationsAnswers ConA in VCA)
                        {
                            DivAnswered.InnerHtml += "<div class='container-md StyleBox'>";
                            DivAnswered.InnerHtml += "<div class='mb-3 mt-3'>";
                            DivAnswered.InnerHtml += "<div class='row'  style='direction:rtl;'>";
                            DivAnswered.InnerHtml += "<div class='col-lg-2 col-md-3 col-sm-12 text-center StyleFourthColumn'>";
                            DivAnswered.InnerHtml += "<img src='../ImagesOfUsers/" + ConA.PERSONAL_PICTURE_USER_ASK + "' class='rounded-circle SizeImage'/>";

                            // VIEW CONVERSATION
                            string SVIEW_CONVERSATION = Statistics.SumViewConversation(ConA.VIEW_CONVERSATION);
                            DivAnswered.InnerHtml += "<p style='margin-top:10px'><i class='fas fa-eye'></i>&nbsp;" + SVIEW_CONVERSATION + "</p>";
                            DivAnswered.InnerHtml += "</div>";
                            string Subject = ConA.SUBJECT.Replace(@"\s+", " "); 
                            int SizeTextSubject = Subject.Length;
                            if (SizeTextSubject > 80)
                            {
                                Subject = Subject.ToString().Substring(0, 80) + " ...";
                            }

                            string Consul = ConA.CONSULTATION.Replace(@"\s+", " ").Replace("<u>", "").Replace("</u>", "").Replace("<b>", "").Replace("</b>", ""); 
                            int SizeTextConsultation = Consul.Length;
                            if (SizeTextConsultation > 200)
                            {
                                Consul = Consul.ToString().Substring(0, 200) + " ...";
                            }
              
                            DivAnswered.InnerHtml += "<div class='col-lg-10 col-md-9 col-sm-12 StyleThirdColumn StyleP2'>";

                            // Determine the type of consultation, and If there is a new discussion with a lawyer, and If chat is stopped
                            int NumConsultationType = ConA.SHARE_CHAT;
                            string ConsultationType = "";
                            if (NumConsultationType == 0) { ConsultationType = "استشارة مجانية"; }
                            else if (NumConsultationType == 1) { ConsultationType = "استشارة خاصة مجانية"; }
                            else if (NumConsultationType == 2) { ConsultationType = "استشارة خاصة مأجورة"; }
                            else { ConsultationType = "غير معروف نوع الاستشارة"; }

                            DivAnswered.InnerHtml += "<p class='StyleH6'>";
                            string LAST_ANSWER_DATE = ConA.LAST_ANSWER_DATE.HasValue ? ConA.LAST_ANSWER_DATE.Value.ToString() : "";
                            DateTime DLAST_ANSWER_DATE = Convert.ToDateTime(LAST_ANSWER_DATE == "" ? "01/01/1999 12:00:00 PM" : LAST_ANSWER_DATE);
                            string LAST_ANSWER_SUBSCRIBER_DATE = ConA.LAST_ANSWER_SUBSCRIBER_DATE.HasValue ? ConA.LAST_ANSWER_SUBSCRIBER_DATE.Value.ToString() : "";
                            DateTime DLAST_ANSWER_SUBSCRIBER_DATE = Convert.ToDateTime(LAST_ANSWER_SUBSCRIBER_DATE == "" ? "01/01/1999 12:00:00 PM" : LAST_ANSWER_SUBSCRIBER_DATE);
                            if (DLAST_ANSWER_SUBSCRIBER_DATE > DLAST_ANSWER_DATE && ConA.LOCK_SHARE_CHAT == false)
                            {
                                DivAnswered.InnerHtml += "<span style='color:#00FF00'><i class='fas fa-circle'></i></span>&nbsp;";
                            }
                            DivAnswered.InnerHtml += ConsultationType;
                            if (ConA.LOCK_SHARE_CHAT == true)
                            {
                                DivAnswered.InnerHtml += "<span style='color:#FF0000'>&nbsp;(تم ايقاف المحادثة)</span>";
                            }

                            DivAnswered.InnerHtml += "</p>";

                            DivAnswered.InnerHtml += "<p dir='auto'><a href='../LegalAdvice/ShowConversation.aspx?Num=" + ConA.ID_CONVERSATION + "' class='Hyper-Link-Title'>" + Subject + "</a></p>";
                            DivAnswered.InnerHtml += "<p dir='auto' style='text-align:justify'>" + Consul.Replace("هناك تسجيل صوتي", "<b><u> هناك تسجيل صوتي </u></b>") + "</p><br/>";
                            DivAnswered.InnerHtml += "<b>";
                            DivAnswered.InnerHtml += "<p> سؤال بواسطة: " + ConA.FULL_NAME_USER_ASK + " - بتاريخ: " + ConA.CREATEDATE_ASK + "</p>";
                            
                            if (LAST_ANSWER_DATE != "")
                            {
                                DivAnswered.InnerHtml += "<p> تم الرد بتاريخ: " + LAST_ANSWER_DATE + "</p>";
                            }
                            DivAnswered.InnerHtml += "</b>";
                            DivAnswered.InnerHtml += "</div>";
                            DivAnswered.InnerHtml += "</div>";
                            DivAnswered.InnerHtml += "</div>";
                            DivAnswered.InnerHtml += "</div>";
                            DivAnswered.InnerHtml += "<br/>";
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
                {  DoNotView(); }
            }
            else
            {
                DoNotView();
            }
        }
    }
}