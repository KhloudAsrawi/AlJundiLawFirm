using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AlJundiLawFirm.App_Code;
using System.Data;
using System.Data.SqlClient;

namespace AlJundiLawFirm.Models
{
    public class ViewConversations
    {
        public int ID_VIEW_CONVERSATIONS { get; set; }
        public int ID_CONVERSATION { get; set; }
        public int ID_USER { get; set; }
        public DateTime DATE_VIEW { get; set; }
        public string IP_ADDRESS { get; set; }

        public ViewConversations() { }
        public ViewConversations(int ID_VIEW_CONVERSATIONS, int ID_CONVERSATION, int ID_USER, DateTime DATE_VIEW, string IP_ADDRESS)
        {
            this.ID_VIEW_CONVERSATIONS = ID_VIEW_CONVERSATIONS;
            this.ID_CONVERSATION = ID_CONVERSATION;
            this.ID_USER = ID_USER;
            this.DATE_VIEW = DATE_VIEW;
            this.IP_ADDRESS = IP_ADDRESS;
        }

        // Add a row indicates that the user saw a consultation
        public static bool InsertViewConsultation(int ID_CONVERSATION, int ID_USER, string IP_ADDRESS)
        {
            try
            {
                string query = "INSERT INTO VIEW_CONVERSATIONS (ID_CONVERSATION, ID_USER, IP_ADDRESS) VALUES (@ID_CONVERSATION, @ID_USER, " +
                               "@IP_ADDRESS)";

                string Scon = ConnectionStringDB.GetConnectionStringDB();
                SqlConnection con = new SqlConnection(Scon);
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = query;

                cmd.Parameters.Add("ID_CONVERSATION", SqlDbType.Int).Value = ID_CONVERSATION;
                cmd.Parameters.Add("ID_USER", SqlDbType.Int).Value = ID_USER != 0 ? ID_USER : (object)DBNull.Value;
                cmd.Parameters.Add("IP_ADDRESS", SqlDbType.NVarChar).Value = IP_ADDRESS; 
                cmd.Connection = con;
                cmd.ExecuteNonQuery();
                con.Close();
                return true;
            }
            catch
            {
                return false;
            }
        }


    }
}