using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AlJundiLawFirm.App_Code
{
    public class ConnectionStringDB
    {
        public ConnectionStringDB()
        {

        }

        public static string GetConnectionStringDB()
        {
            return "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\\AlJundiDB.mdf;Integrated Security=True";
            //return "Data Source=.\\MSSQLSERVER2014;Integrated Security=False;User ID=mb-jundi;Connect Timeout=15;Encrypt=False;Packet Size=4096";
            // الاساسي
            //return "Data Source=.\\MSSQLSERVER2014;Initial Catalog=AlJundiDB;Integrated Security=False;User ID=mb-jundi;Password=Hgpkp@2023;Connect Timeout=15;Encrypt=False;Packet Size=4096";

        }
    }
}