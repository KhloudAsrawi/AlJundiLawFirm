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
    public partial class ViewStatistics : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Page.Title = "الجندي للاستشارات القانونية - الاحصائيات";
            HttpCookie repCookies = Request.Cookies["UserInfoForAlJundiLaw"];
            if (repCookies != null)
            {
                Session["Role"] = Encrypt.decryptQueryString(repCookies["Role"].ToString().Replace(" ", "+"));
                Session["User"] = Encrypt.decryptQueryString(Server.UrlDecode(repCookies["User"].ToString().Replace(" ", "+")));
                if (!IsPostBack)
                {
                    ViewStatistic();
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
            DivViewStatistics.Visible = false;
            DivCannotDisplay.Visible = true;
            HLBackPage.NavigateUrl = (Request.UrlReferrer == null) ? "~/index.aspx" : Request.UrlReferrer.ToString();
            HLIndex.NavigateUrl = "~/index.aspx";
        }

        private void ViewStatistic()
        {
            try
            {
                int IdRole = 0;
                if (Session["Role"] != null && Session["Role"].ToString() != "0")
                {
                    IdRole = int.Parse(Session["Role"].ToString());
                }

                List<RolePermission> ListPermission = RolePermission.GetIdPermissions(IdRole);
                if (ListPermission != null && ListPermission.Count > 0)
                {
                    var ViewAnsweredConsulting = ListPermission.Any(c => c.ID_PERMISSION == 20);
                    if (ViewAnsweredConsulting == true)
                    {
                        // Number Consultations
                        Statistics Consultation = Statistics.NumberConsultations();
                        if (Consultation != null)
                        {
                            NumNumberConsultations.Text = Consultation.NUMBER.ToString();
                            SNumberConsultations.Text = Consultation.GREATER_NAME;
                        }

                        // Number Answers
                        Statistics Answers = Statistics.NumberAnswers();
                        if (Answers != null)
                        {
                            NumNumberAnswers.Text = Answers.NUMBER.ToString();
                            SNumberAnswers.Text = Answers.GREATER_NAME;
                        }

                        // Number Users
                        Statistics NUsers = Statistics.NumberUsers();
                        if (NUsers != null)
                        {
                            NumNumberUser.Text = NUsers.NUMBER.ToString();
                            SNumberUser.Text = NUsers.GREATER_NAME;
                        }

                        // Sum View Conversation
                        Statistics SumView = Statistics.SumViewConversation();
                        if (SumView != null)
                        {
                            NumSumViewConversation.Text = SumView.NUMBER.ToString();
                            SSumViewConversation.Text = SumView.GREATER_NAME;
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
    }
}