using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AlJundiLawFirm.App_Code;
using System.Data;
using System.Data.SqlClient;

namespace AlJundiLawFirm.Models
{
    public class ConversationReplies
    {
        public int ID_CONVERSATION_REPLIES { get; set; }
        public int ID_CONVERSATION { get; set; } // FK from table CONVERSATIONS
        public int ID_USER { get; set; } // FK from table USERS
        public int ID_REPLY { get; set; }
        public string REPLY { get; set; }
        public DateTime CREATE_DATE { get; set; }
        public string ATTACHMENT { get; set; }
        public string AUDIO_RECORDING { get; set; }

        public ConversationReplies() { }
        public ConversationReplies(int ID_CONVERSATION_REPLIES, int ID_CONVERSATION, int ID_USER, int ID_REPLY, string REPLY, 
                                   DateTime CREATE_DATE, string ATTACHMENT, string AUDIO_RECORDING)
        {
            this.ID_CONVERSATION_REPLIES = ID_CONVERSATION_REPLIES;
            this.ID_CONVERSATION = ID_CONVERSATION;
            this.ID_USER = ID_USER;
            this.ID_REPLY = ID_REPLY;
            this.REPLY = REPLY;
            this.CREATE_DATE = CREATE_DATE;
            this.ATTACHMENT = ATTACHMENT;
            this.AUDIO_RECORDING = AUDIO_RECORDING;
        }

        // Select the last Reply to the consultation, And Count Replies
        public static List<int> GetLastReply(int IdConversation)
        {
            var Values = new List<int>();
            string Scon = ConnectionStringDB.GetConnectionStringDB();
            SqlConnection con = new SqlConnection(Scon);
            try
            {
                string query = "SELECT MAX(ID_REPLY), COUNT(*) FROM CONVERSATION_REPLIES WHERE ID_CONVERSATION = @IdConversation";
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = query;
                cmd.Parameters.AddWithValue("IdConversation", IdConversation);
                cmd.Connection = con;
                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    Values.Add(dr.IsDBNull(0) ? 0 : dr.GetInt32(0));
                    Values.Add(dr.IsDBNull(1) ? 0 : dr.GetInt32(1));
                    return Values;
                }
                else
                {
                    Values.Add(0);
                    Values.Add(0);
                    return Values;
                }
            }
            catch
            {
                Values.Add(0);
                Values.Add(0);
                return Values;
            }
            finally
            {
                con.Close();
            }
        }

 
        // Insert / Add conversation reply into "CONVERSATION_REPLIES" table on database
        public static bool InsertConversationReply(int ID_CONVERSATION, int ID_USER, string REPLY, string ATTACHMENT, string AUDIO_RECORDING)
        {
            List<int> LastReply = ConversationReplies.GetLastReply(ID_CONVERSATION);
            int ID_REPLY = LastReply[0] + 1;

            try
            {
                string query = "INSERT INTO CONVERSATION_REPLIES (ID_CONVERSATION, ID_USER, ID_REPLY, REPLY, ATTACHMENT, AUDIO_RECORDING) " +
                               "VALUES (@ID_CONVERSATION, @ID_USER, @ID_REPLY, @REPLY, @ATTACHMENT, @AUDIO_RECORDING)";

                string Scon = ConnectionStringDB.GetConnectionStringDB();
                SqlConnection con = new SqlConnection(Scon);
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = query;

                cmd.Parameters.Add("ID_CONVERSATION", SqlDbType.Int).Value = ID_CONVERSATION;
                cmd.Parameters.Add("ID_USER", SqlDbType.Int).Value = ID_USER;
                cmd.Parameters.Add("ID_REPLY", SqlDbType.Int).Value = ID_REPLY;
                cmd.Parameters.Add("REPLY", SqlDbType.NText).Value = REPLY;
                cmd.Parameters.Add("ATTACHMENT", SqlDbType.VarChar).Value = ATTACHMENT != "" ? ATTACHMENT : (object)DBNull.Value;
                cmd.Parameters.Add("AUDIO_RECORDING", SqlDbType.VarChar).Value = AUDIO_RECORDING != "" ? AUDIO_RECORDING : (object)DBNull.Value;
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

        // View all replies for this Id Consultation
        public static List<ConversationReplies> SelectConversationReplies(int IdConversation)
        {
            List<ConversationReplies> ListConversationReplies = new List<ConversationReplies>();
            string Scon = ConnectionStringDB.GetConnectionStringDB();
            SqlConnection con = new SqlConnection(Scon);
            try
            {
                string query = "SELECT ID_CONVERSATION_REPLIES, ID_CONVERSATION, ID_USER, ID_REPLY, REPLY, CREATE_DATE, ATTACHMENT, " +
                               "AUDIO_RECORDING FROM CONVERSATION_REPLIES WHERE ID_CONVERSATION = @IdConversation ORDER BY ID_REPLY ASC ";
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = query;
                cmd.Parameters.Add("IdConversation", SqlDbType.Int).Value = IdConversation;
                cmd.Connection = con;
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    ConversationReplies ConversationReplies = new ConversationReplies();
                    ConversationReplies.ID_CONVERSATION_REPLIES = dr.GetInt32(0);
                    ConversationReplies.ID_CONVERSATION = dr.GetInt32(1);
                    ConversationReplies.ID_USER = dr.GetInt32(2);
                    ConversationReplies.ID_REPLY = dr.GetInt32(3);
                    ConversationReplies.REPLY = dr.GetString(4);
                    ConversationReplies.CREATE_DATE = dr.GetDateTime(5);
                    ConversationReplies.ATTACHMENT = dr.IsDBNull(6) ? "" : dr.GetString(6);
                    ConversationReplies.AUDIO_RECORDING = dr.IsDBNull(7) ? "" : dr.GetString(7);

                    ListConversationReplies.Add(ConversationReplies);
                }
                return ListConversationReplies;
            }
            catch
            {
                return null;
            }
            finally
            {
                con.Close();
            }
        }

    }
}