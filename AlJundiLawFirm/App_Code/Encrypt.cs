using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AlJundiLawFirm.App_Code;

namespace AlJundiLawFirm.App_Code
{
    public class Encrypt
    {
        public Encrypt() { }

        public static string encryptQueryString(string strQueryString)
        {
            Encryption64 oES = new Encryption64();
            return oES.Encrypt(strQueryString, "!#$a54?3");
        }

        public static string decryptQueryString(string strQueryString)
        {
            Encryption64 oES = new Encryption64();
            return oES.Decrypt(strQueryString, "!#$a54?3");
        }

    }
}