using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace AlJundiLawFirm.LegalAdvice
{
    public partial class SearchConsultations : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            LBRecentlyAsked.Attributes.Add("target", "_parent");
            LBRecentlyAsked.Attributes.Add("href", "../LegalAdvice/CounselingPublic.aspx?Sort=RecentlyAsked");

            LBMostActive.Attributes.Add("target", "_parent");
            LBMostActive.Attributes.Add("href", "../LegalAdvice/CounselingPublic.aspx?Sort=MostActive");


            LBRecentlyAnswer.Attributes.Add("target", "_parent");
            LBRecentlyAnswer.Attributes.Add("href", "../LegalAdvice/CounselingPublic.aspx?Sort=RecentlyAnswer");

            LBOldestAsked.Attributes.Add("target", "_parent");
            LBOldestAsked.Attributes.Add("href", "../LegalAdvice/CounselingPublic.aspx?Sort=OldestAsked");

            LBMostAnswers.Attributes.Add("target", "_parent");
            LBMostAnswers.Attributes.Add("href", "../LegalAdvice/CounselingPublic.aspx?Sort=MostAnswers");
        }

        protected void BSearch_Click(object sender, EventArgs e)
        {
            string text = TBSearch.Text.Trim();
            string url = "../LegalAdvice/CounselingPublic.aspx?Sort=Search&For=" + text;
            Page.ClientScript.RegisterStartupScript(this.GetType(), "OpenWindow", "window.open('"+url+ "','_parent');", true);
        }
    }
}