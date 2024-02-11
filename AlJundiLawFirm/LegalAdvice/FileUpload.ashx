<%@ WebHandler Language="C#" Class="AlJundiLawFirm.LegalAdvice.FileUpload" %>

using System;
using System.Web;

namespace AlJundiLawFirm.LegalAdvice
{
    public class FileUpload : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            DateTime todayDate = DateTime.Now;
            string date = todayDate.ToString("MMddyyyy-hhmmss");
            // var userName = context.Request.Cookies["UserInfoForAlJundiLaw"]["User"].ToString();
            var userName = new Random().Next(1, 10000000);
            var fileName = userName + "-" + date + ".wav";
            context.Request.SaveAs(context.Server.MapPath("~/Recording/" + fileName), false);
            context.Response.ContentType = "text/plain";
            context.Response.Write(fileName);
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}
