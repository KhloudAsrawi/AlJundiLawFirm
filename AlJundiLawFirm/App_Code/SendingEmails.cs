using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Net;
using System.Net.Mail;
using System.IO;
using System.Text;
using AlJundiLawFirm.App_Code;
using AlJundiLawFirm.Models;

namespace AlJundiLawFirm.App_Code
{
    public class SendingEmails
    {
        public SendingEmails()
        {
        }

        public static bool SendingEmail(string Email, string Username, string VerificationCode)
        {
            string EMailAddress = "khlood86@hotmail.com";
            string PasswordEmail = "2-og,]hpl]hgusvh,d22";
            string Subject = "التحقق من عنوان بريدك الإلكتروني وتأكيد اشتراك في موقع الجندي للاستشارات القانونية";
            string Message = "<div dir='rtl' style='font-size:16px;'>" +
                             "<strong> مرحباً <span style='font-size:20px;font-weight:bold;color:red;'>" + Username + "</span >،</strong>" +
                             "<br /> تتطلب محاول تسجيل الدخول لموقع الجندي للاستشارات القانونية مزيداً من التحقق،" +
                             "<br /> لذلك لإكمال تسجيل الدخول يجب إدخال رمز التحقق التالي،" +
                             "<br /><center><span style='font-size:22px;font-weight:bold;color:blue;'>" + VerificationCode + "</span></center>" +
                             "<br /><span style='font-size:22px;font-weight:bold;color:red;'>" + 
                             "ملاحظة هامة: يجب عليك حفظ رمز التحقق وذلك بحال نسيان كلمة السر تستطيع من خلال رمز التحقق إعادة تعين كلمة السر</span>" +
                             "<br />إذا كنت تريد إكمال تسجيل الدخول لموقع الجندي للاستشارات القانونية، قم بزيارة الموقع:" +
                             "<br /><a href='http://mb-jundi.com/UserAccount/ActivateAccount.aspx'>http://mb-jundi.com/UserAccount/ActivateAccount.aspx</a>" +
                             "<br />قم بزيارة الموقع من الصفحة الرئيسة:" +
                             "<br /><a href='http://mb-jundi.com/Index.aspx'>http://mb-jundi.com/Index.aspx</a>" +
                             "<br /> شكراً،" +
                             "<br /><strong> موقع الجندي للاستشارات القانونية </strong> " +
                             "</div>";
            try
            {
                MailMessage msg = new MailMessage();
                msg.From = new MailAddress(EMailAddress); // ايميل الموقع
                msg.To.Add(Email); // المستخدم
                msg.Subject = Subject;
                msg.Body = Message;
                msg.IsBodyHtml = true;
                msg.BodyEncoding = Encoding.UTF8;
                SmtpClient smt = new SmtpClient();
                smt.Host = "imap-mail.outlook.com";
                System.Net.NetworkCredential ntwd = new NetworkCredential();
                ntwd.UserName = EMailAddress; // // ايميل الموقع
                ntwd.Password = PasswordEmail; // كلمة السر الموقع
                smt.UseDefaultCredentials = false;
                smt.Credentials = ntwd;
                smt.Port = 587;
                smt.EnableSsl = true;
                smt.Send(msg);
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }
    }
}