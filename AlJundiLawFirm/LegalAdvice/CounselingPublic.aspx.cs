using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AlJundiLawFirm.Models;

namespace AlJundiLawFirm.LegalAdvice
{
    public partial class CounselingPublic : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Page.Title = "الجندي للاستشارات القانونية - استشارات";
            if (!string.IsNullOrEmpty(Request.QueryString["Sort"]))
            {
                if (!IsPostBack)
                {
                    if (Request.QueryString["Sort"].ToString() == "RecentlyAsked")
                    {
                        SortConsultingBy("CREATEDATE_ASK DESC"); // سئل حديثا
                    }
                    else if (Request.QueryString["Sort"].ToString() == "MostActive")
                    {
                        SortConsultingBy("VIEW_CONVERSATION DESC , CREATEDATE_ASK DESC"); // أكثر نشاطا
                    }
                    else if (Request.QueryString["Sort"].ToString() == "RecentlyAnswer")
                    {
                        SortConsultingBy("LAST_ANSWER_DATE DESC , CREATEDATE_ASK DESC"); // مجاب حديثا
                    }
                    else if (Request.QueryString["Sort"].ToString() == "OldestAsked")
                    {
                        SortConsultingBy("CREATEDATE_ASK ASC"); // سئل قديما
                    }
                    else if (Request.QueryString["Sort"].ToString() == "MostAnswers")
                    {
                        SortConsultingBy("COUNT_ANSWERS DESC , CREATEDATE_ASK DESC"); // الأكثر إجابة 
                    }
                    else if (Request.QueryString["Sort"].ToString() == "Search")
                    {
                        string Text = Request.QueryString["For"].ToString();
                        SearchByText(Text); // البحث حسب النص
                    }
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
            DivConsultations.Visible = false;
            DivNoConsultations.Visible = false;
            DivCannotDisplay.Visible = true;
            HLBackPage.NavigateUrl = (Request.UrlReferrer == null) ? "~/index.aspx" : Request.UrlReferrer.ToString();
            HLIndex.NavigateUrl = "~/index.aspx";
        }

        private void SortConsultingBy(string order)
        {
            try
            {
                // Get count of consultations public
                float NumberConsultations = ViewConsultationsAnswers.NumberPublicConsultations();
                if (NumberConsultations > 20)
                {
                    float NumPages = 1;

                    NumPages = NumberConsultations / 20;
                    float Mod = NumPages % 1;
                    DivNumberPages.InnerHtml = "";
                    int Previous = 0;
                    int Next = 1;
                    int ThisPage = 0;
                    string View = Request.QueryString["Sort"].ToString();

                    DivNumberPages.InnerHtml += "<ul class='pagination justify-content-center'>";

                    // Next
                    if (!string.IsNullOrEmpty(Request.QueryString["Page"]))
                    {
                        Next = int.Parse(Request.QueryString["Page"]);
                        if (Next <= NumPages)
                        {
                            DivNumberPages.InnerHtml += "<li class='page-item'><a class='page-link' style='width:auto' href='../LegalAdvice/CounselingPublic.aspx?Sort=" + View + "&Page=" + (Next + 1) + "'>التالي</a></li>";
                        }
                    }
                    else
                    {
                        DivNumberPages.InnerHtml += "<li class='page-item'><a class='page-link' style='width:auto' href='../LegalAdvice/CounselingPublic.aspx?Sort=" + View + "&Page=" + (Next + 1) + "'>التالي</a></li>";
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
                                DivNumberPages.InnerHtml += "<li class='page-item'><a class='page-link page-link-actived' href='../LegalAdvice/CounselingPublic.aspx?Sort=" + View + "&Page=" + Num + "'>" + Num.ToString() + "</a></li>";
                            }
                            else
                            {
                                DivNumberPages.InnerHtml += "<li class='page-item'><a class='page-link' href='../LegalAdvice/CounselingPublic.aspx?Sort=" + View + "&Page=" + Num + "'>" + Num.ToString() + "</a></li>";
                            }
                        }
                        else
                        {
                            DivNumberPages.InnerHtml += "<li class='page-item'><a class='page-link' href='../LegalAdvice/CounselingPublic.aspx?Sort=" + View + "&Page=" + Num + "'>" + Num.ToString() + "</a></li>";
                        }
                        Num--;
                    }

                    // Previous
                    if (!string.IsNullOrEmpty(Request.QueryString["Page"]))
                    {
                        Previous = int.Parse(Request.QueryString["Page"]) - 1;
                        if (Previous > 0)
                        {
                            DivNumberPages.InnerHtml += "<li class='page-item'><a class='page-link' style='width:auto' href='../LegalAdvice/CounselingPublic.aspx?Sort=" + View + "&Page=" + Previous + "'>سابق</a></li>";
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

                // VCNA mean Fetch Conversations people
                List<ViewConsultationsAnswers> VCNA = ViewConsultationsAnswers.SortConsulting(rang, order); 
                if (VCNA != null && VCNA.Count > 0)
                {
                    DivNumberPages.Visible = true;

                    DivConsultation.InnerHtml = "";
                    foreach (ViewConsultationsAnswers Con in VCNA)
                    {
                        DivConsultation.InnerHtml += "<div class='container-md StyleBox'>";
                        DivConsultation.InnerHtml += "<div class='mb-3 mt-3'>";
                        DivConsultation.InnerHtml += "<div class='row'  style='direction:rtl;'>";
                        DivConsultation.InnerHtml += "<div class='col-lg-2 col-md-3 col-sm-12 text-center StyleFourthColumn'>";
                        DivConsultation.InnerHtml += "<img src='../ImagesOfUsers/" + Con.PERSONAL_PICTURE_USER_ASK + "' class='rounded-circle SizeImage'/>";

                        // VIEW CONVERSATION
                        string SVIEW_CONVERSATION = Statistics.SumViewConversation(Con.VIEW_CONVERSATION);
                        DivConsultation.InnerHtml += "<p style='margin-top:10px'><i class='fas fa-eye'></i>&nbsp;" + SVIEW_CONVERSATION + "</p>";

                        string SVIEW_ANSWER = Statistics.SumViewAnswer(Con.COUNT_ANSWERS);
                        DivConsultation.InnerHtml += "<p style='margin-top:10px'><i class='fas fa-gavel'></i>&nbsp;" + SVIEW_ANSWER + "</p>";

                        DivConsultation.InnerHtml += "</div>";
                        string Subject = Con.SUBJECT.Replace(@"\s+", " ");
                        int SizeTextSubject = Subject.Length;
                        if (SizeTextSubject > 80)
                        {
                            Subject = Subject.ToString().Substring(0, 80) + "...";
                        }

                        string Consul = Con.CONSULTATION.Replace(@"\s+", " ").Replace("<u>", "").Replace("</u>", "").Replace("<b>", "").Replace("</b>", "");
                        int SizeTextConsultation = Consul.Length;
                        if (SizeTextConsultation > 200)
                        {
                            Consul = Consul.ToString().Substring(0, 200) + "...";
                        }

                        DivConsultation.InnerHtml += "<div class='col-lg-10 col-md-9 col-sm-12 StyleThirdColumn StyleP2'>";
                        DivConsultation.InnerHtml += "<p class='StyleH6'>";
                        if (Con.LOCK_SHARE_CHAT == true)
                        {
                            DivConsultation.InnerHtml += "<span style='color:#FF0000'>&nbsp;(تم ايقاف المحادثة)</span>";
                        }
                        DivConsultation.InnerHtml += "</p>";

                        DivConsultation.InnerHtml += "<p dir='auto'><a href='../LegalAdvice/ShowConversation.aspx?Num=" + Con.ID_CONVERSATION + "' class='Hyper-Link-Title'>" + Subject + "</a></p>";
                        DivConsultation.InnerHtml += "<p dir='auto' style='text-align:justify'>" + Consul.Replace("هناك تسجيل صوتي", "<b><u> هناك تسجيل صوتي </u></b>") + "</p><br/>";
                        DivConsultation.InnerHtml += "<b>";
                        DivConsultation.InnerHtml += "<p> سؤال بواسطة: " + Con.FULL_NAME_USER_ASK + " - بتاريخ: " + Con.CREATEDATE_ASK + "</p>";
                        string LAST_ANSWER_DATE = Con.LAST_ANSWER_DATE.HasValue ? Con.LAST_ANSWER_DATE.Value.ToString() : "";
                        if (LAST_ANSWER_DATE != "")
                        {
                            DivConsultation.InnerHtml += "<p> تم الرد بتاريخ: " + LAST_ANSWER_DATE + "</p>";
                        }
                        DivConsultation.InnerHtml += "</b>";
                        DivConsultation.InnerHtml += "</div>";
                        DivConsultation.InnerHtml += "</div>";
                        DivConsultation.InnerHtml += "</div>";
                        DivConsultation.InnerHtml += "</div>";
                        DivConsultation.InnerHtml += "<br/>";
                    }
                }
                else
                {
                    DivSearch.Visible = true;
                    DivConsultations.Visible = false;
                    DivNoConsultations.Visible = true;
                    DivCannotDisplay.Visible = false;
                    HLBackPage1.NavigateUrl = (Request.UrlReferrer == null) ? "~/index.aspx" : Request.UrlReferrer.ToString();
                    HLIndex1.NavigateUrl = "~/index.aspx";
                }
            }
            catch
            {
                DoNotView();
            }
        }


        // بحث حسب النص
        private void SearchByText(string text)
        {
            string searchinname = "";
            searchinname += "SUBJECT LIKE N'%" + text + "%' OR CONSULTATION LIKE N'%" + text + "%'";

            // Delete Word Engith from text
            List<string> IgnoredWordsEN = new List<string>() { "a", "is", "about", "above", "after", "again", "against", "all", "just",
                                          "text", "of", "and", "it", "the", "she", "he", "they" };
            text = string.Join(" ", text.Split().Where(WordsEN => !IgnoredWordsEN.Contains(WordsEN, StringComparer.InvariantCultureIgnoreCase)));

            // Delete Word Aribic from text
            List<string> IgnoredWordsAR = new List<string>() { "في" , "سوف" , "إلى", "الى", "ان", "أن", "إن", "يكون", "حيث", "حول",
                                          "فوق", "الكل", "فقط", "نص", "على", "و", "أو", "او", "من" };
            text = string.Join(" ", text.Split().Where(WordsAR => !IgnoredWordsAR.Contains(WordsAR, StringComparer.InvariantCultureIgnoreCase)));

            string[] words = text.Split(' '); // Divide the text into a group of words
            int LengthWords = words.Length;
            for (int i = 0; i < LengthWords; i++)
            {
                if (LengthWords > 1)
                {
                    for (int y = 0; y < LengthWords; y++)
                    {
                        if (i != y)
                        {
                            searchinname += " OR SUBJECT LIKE N'%" + words[i] + " " + words[y] + "%' OR CONSULTATION LIKE N'%" + words[i] + " " + words[y] + "%'";
                        }
                    }
                }
                searchinname += " OR SUBJECT LIKE N'%" + words[i] + "%' OR CONSULTATION LIKE N'%" + words[i] + "%'";
            }

            try
            {
                // Get count of consultations public by text
                float NumberConsultations = ViewConsultationsAnswers.NumberPublicConsultations(searchinname);
                if (NumberConsultations > 20)
                {
                    float NumPages = 1;

                    NumPages = NumberConsultations / 20;
                    float Mod = NumPages % 1;
                    DivNumberPages.InnerHtml = "";
                    int Previous = 0;
                    int Next = 1;
                    int ThisPage = 0;
                    string View = Request.QueryString["Sort"].ToString();
                    string For = Request.QueryString["For"].ToString();

                    DivNumberPages.InnerHtml += "<ul class='pagination justify-content-center'>";

                    // Next
                    if (!string.IsNullOrEmpty(Request.QueryString["Page"]))
                    {
                        Next = int.Parse(Request.QueryString["Page"]);
                        if (Next <= NumPages)
                        {
                            DivNumberPages.InnerHtml += "<li class='page-item'><a class='page-link' style='width:auto' href='../LegalAdvice/CounselingPublic.aspx?Sort=" + View + "&For=" + For + "&Page=" + (Next + 1) + "'>التالي</a></li>";
                        }
                    }
                    else
                    {
                        DivNumberPages.InnerHtml += "<li class='page-item'><a class='page-link' style='width:auto' href='../LegalAdvice/CounselingPublic.aspx?Sort=" + View + "&For=" + For + "&Page=" + (Next + 1) + "'>التالي</a></li>";
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
                                DivNumberPages.InnerHtml += "<li class='page-item'><a class='page-link page-link-actived' href='../LegalAdvice/CounselingPublic.aspx?Sort=" + View + "&For=" + For + "&Page=" + Num + "'>" + Num.ToString() + "</a></li>";
                            }
                            else
                            {
                                DivNumberPages.InnerHtml += "<li class='page-item'><a class='page-link' href='../LegalAdvice/CounselingPublic.aspx?Sort=" + View + "&For=" + For + "&Page=" + Num + "'>" + Num.ToString() + "</a></li>";
                            }
                        }
                        else
                        {
                            DivNumberPages.InnerHtml += "<li class='page-item'><a class='page-link' href='../LegalAdvice/CounselingPublic.aspx?Sort=" + View + "&For=" + For + "&Page=" + Num + "'>" + Num.ToString() + "</a></li>";
                        }
                        Num--;
                    }

                    // Previous
                    if (!string.IsNullOrEmpty(Request.QueryString["Page"]))
                    {
                        Previous = int.Parse(Request.QueryString["Page"]) - 1;
                        if (Previous > 0)
                        {
                            DivNumberPages.InnerHtml += "<li class='page-item'><a class='page-link' style='width:auto' href='../LegalAdvice/CounselingPublic.aspx?Sort=" + View + "&For=" + For + "&Page=" + Previous + "'>سابق</a></li>";
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

                // VCNA mean Fetch Conversations public by text
                List<ViewConsultationsAnswers> VCNA = ViewConsultationsAnswers.SelectSearchByText(rang, searchinname);
                if (VCNA != null && VCNA.Count > 0)
                {
                    DivNumberPages.Visible = true;

                    DivConsultation.InnerHtml = "";
                    foreach (ViewConsultationsAnswers Con in VCNA)
                    {
                        DivConsultation.InnerHtml += "<div class='container-md StyleBox'>";
                        DivConsultation.InnerHtml += "<div class='mb-3 mt-3'>";
                        DivConsultation.InnerHtml += "<div class='row'  style='direction:rtl;'>";
                        DivConsultation.InnerHtml += "<div class='col-lg-2 col-md-3 col-sm-12 text-center StyleFourthColumn'>";
                        DivConsultation.InnerHtml += "<img src='../ImagesOfUsers/" + Con.PERSONAL_PICTURE_USER_ASK + "' class='rounded-circle SizeImage'/>";

                        // VIEW CONVERSATION
                        string SVIEW_CONVERSATION = Statistics.SumViewConversation(Con.VIEW_CONVERSATION);
                        DivConsultation.InnerHtml += "<p style='margin-top:10px'><i class='fas fa-eye'></i>&nbsp;" + SVIEW_CONVERSATION + "</p>";

                        string SVIEW_ANSWER = Statistics.SumViewAnswer(Con.COUNT_ANSWERS);
                        DivConsultation.InnerHtml += "<p style='margin-top:10px'><i class='fas fa-gavel'></i>&nbsp;" + SVIEW_ANSWER + "</p>";

                        DivConsultation.InnerHtml += "</div>";
                        string Subject = Con.SUBJECT.Replace(@"\s+", " ");
                        int SizeTextSubject = Subject.Length;
                        if (SizeTextSubject > 80)
                        {
                            Subject = Subject.ToString().Substring(0, 80) + "...";
                        }

                        string Consul = Con.CONSULTATION.Replace(@"\s+", " ").Replace("<u>", "").Replace("</u>", "").Replace("<b>", "").Replace("</b>", "");
                        int SizeTextConsultation = Consul.Length;
                        if (SizeTextConsultation > 200)
                        {
                            Consul = Consul.ToString().Substring(0, 200) + "...";
                        }

                        DivConsultation.InnerHtml += "<div class='col-lg-10 col-md-9 col-sm-12 StyleThirdColumn StyleP2'>";
                        DivConsultation.InnerHtml += "<p class='StyleH6'>";
                        if (Con.LOCK_SHARE_CHAT == true)
                        {
                            DivConsultation.InnerHtml += "<span style='color:#FF0000'>&nbsp;(تم ايقاف المحادثة)</span>";
                        }
                        DivConsultation.InnerHtml += "</p>";

                        DivConsultation.InnerHtml += "<p class='StyleH2'><a href='../LegalAdvice/ShowConversation.aspx?Num=" + Con.ID_CONVERSATION + "' class='Hyper-Link-Title'>" + Subject + "</a></p>";
                        DivConsultation.InnerHtml += "<p style='text-align:justify'>" + Consul.Replace("هناك تسجيل صوتي", "<b><u> هناك تسجيل صوتي </u></b>") + "</p><br/>";
                        DivConsultation.InnerHtml += "<b>";
                        DivConsultation.InnerHtml += "<p> سؤال بواسطة: " + Con.FULL_NAME_USER_ASK + " - بتاريخ: " + Con.CREATEDATE_ASK + "</p>";
                        string LAST_ANSWER_DATE = Con.LAST_ANSWER_DATE.HasValue ? Con.LAST_ANSWER_DATE.Value.ToString() : "";
                        if (LAST_ANSWER_DATE != "")
                        {
                            DivConsultation.InnerHtml += "<p> تم الرد بتاريخ: " + LAST_ANSWER_DATE + "</p>";
                        }
                        DivConsultation.InnerHtml += "</b>";
                        DivConsultation.InnerHtml += "</div>";
                        DivConsultation.InnerHtml += "</div>";
                        DivConsultation.InnerHtml += "</div>";
                        DivConsultation.InnerHtml += "</div>";
                        DivConsultation.InnerHtml += "<br/>";
                    }
                }
                else
                {
                    DivSearch.Visible = true;
                    DivConsultations.Visible = false;
                    DivNoConsultations.Visible = true;
                    DivCannotDisplay.Visible = false;
                    HLBackPage1.NavigateUrl = (Request.UrlReferrer == null) ? "~/index.aspx" : Request.UrlReferrer.ToString();
                    HLIndex1.NavigateUrl = "~/index.aspx";
                }
            }
            catch
            {
                DoNotView();
            }
        }
    }
}