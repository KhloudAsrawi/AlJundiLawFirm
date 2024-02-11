using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AlJundiLawFirm.App_Code;
using System.Data;
using System.Data.SqlClient;

namespace AlJundiLawFirm.Models
{
    public class Conversations
    {
        public int ID_CONVERSATION { get; set; }
        public int ID_USER { get; set; } // Fk table Users
        public string SUBJECT { get; set; }
        public string CONSULTATION { get; set; }
        public DateTime CREATE_DATE { get; set; }
        public int SHARE_CHAT { get; set; }
        public bool URGENT_CHAT { get; set; }
        public string ATTACHMENT { get; set; }
        public string AUDIO_RECORDING { get; set; }
        public int VIEW_CONVERSATION { get; set; }
        public DateTime? LAST_ANSWER_DATE { get; set; }
        public bool LOCK_SHARE_CHAT { get; set; }
        public int COUNT_ANSWERS { get; set; }

        // Data from table 'USERS'
        public string FULL_NAME { get; set; }
        public int GENDER { get; set; }
        public string PERSONAL_PICTURE { get; set; }

        // default
        public int ID_DEFAULT { get; set; }



        public Conversations() { }
        public Conversations(int ID_CONVERSATION, int ID_USER, string SUBJECT, string CONSULTATION, DateTime CREATE_DATE, 
                             int SHARE_CHAT, bool URGENT_CHAT, string ATTACHMENT, string AUDIO_RECORDING, int VIEW_CONVERSATION, 
                             DateTime LAST_ANSWER_DATE, bool LOCK_SHARE_CHAT, int COUNT_ANSWERS, string FULL_NAME, 
                             int GENDER, string PERSONAL_PICTURE, int ID_DEFAULT)
        {
            this.ID_CONVERSATION = ID_CONVERSATION;
            this.ID_USER = ID_USER;
            this.SUBJECT = SUBJECT;
            this.CONSULTATION = CONSULTATION;
            this.CREATE_DATE = CREATE_DATE;
            this.SHARE_CHAT = SHARE_CHAT;
            this.URGENT_CHAT = URGENT_CHAT;
            this.ATTACHMENT = ATTACHMENT;
            this.AUDIO_RECORDING = AUDIO_RECORDING;
            this.VIEW_CONVERSATION = VIEW_CONVERSATION;
            this.LAST_ANSWER_DATE = LAST_ANSWER_DATE;
            this.LOCK_SHARE_CHAT = LOCK_SHARE_CHAT;
            this.COUNT_ANSWERS = COUNT_ANSWERS;
            this.FULL_NAME = FULL_NAME;
            this.GENDER = GENDER;
            this.PERSONAL_PICTURE = PERSONAL_PICTURE;
            this.ID_DEFAULT = ID_DEFAULT;
        }

        // Insert / Add a new consultation
        public static bool InsertConsultation(int ID_USER, string SUBJECT, string CONSULTATION, int SHARE_CHAT, bool URGENT_CHAT, 
                                              string ATTACHMENT, string AUDIO_RECORDING)
        {
            try
            {
                string query = "INSERT INTO CONVERSATIONS (ID_USER, SUBJECT, CONSULTATION, SHARE_CHAT, URGENT_CHAT, ATTACHMENT, " +
                               "AUDIO_RECORDING) VALUES (@ID_USER, @SUBJECT, @CONSULTATION, @SHARE_CHAT, @URGENT_CHAT, @ATTACHMENT, " +
                               "@AUDIO_RECORDING)";

                string Scon = ConnectionStringDB.GetConnectionStringDB();
                SqlConnection con = new SqlConnection(Scon);
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = query;
                cmd.Parameters.Add("ID_USER", SqlDbType.Int).Value = ID_USER;
                cmd.Parameters.Add("SUBJECT", SqlDbType.NVarChar).Value = SUBJECT;
                cmd.Parameters.Add("CONSULTATION", SqlDbType.NText).Value = CONSULTATION;
                cmd.Parameters.Add("SHARE_CHAT", SqlDbType.Int).Value = SHARE_CHAT;
                cmd.Parameters.Add("URGENT_CHAT", SqlDbType.Bit).Value = URGENT_CHAT;
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

        // View number (ID) the last consultation
        public static int LastIdConversation(int IdUser)
        {
            int LastRow = 0;
            string Scon = ConnectionStringDB.GetConnectionStringDB();
            SqlConnection con = new SqlConnection(Scon);
            try
            {
                string query = "SELECT MAX(ID_CONVERSATION) FROM CONVERSATIONS WHERE ID_USER = @IdUser";
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = query;
                cmd.Parameters.AddWithValue("IdUser", IdUser);
                cmd.Connection = con;
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    LastRow = dr.IsDBNull(0) ? 0 : dr.GetInt32(0);
                }
                return LastRow;
            }
            catch
            {
                return 0;
            }
            finally
            {
                con.Close();
            }
        }

        // View information Consultation by the Consultation Number (IdConversation)
        public static Conversations ViewConsultation(int IdConversation)
        {
            Conversations viewConversation = new Conversations();
            string Scon = ConnectionStringDB.GetConnectionStringDB();
            SqlConnection con = new SqlConnection(Scon);
            try
            {
                string query = "SELECT ID_CONVERSATION, ID_USER, SUBJECT, CONSULTATION, CREATE_DATE, SHARE_CHAT, URGENT_CHAT, ATTACHMENT, " +
                               "AUDIO_RECORDING, VIEW_CONVERSATION, LAST_ANSWER_DATE, LOCK_SHARE_CHAT, COUNT_ANSWERS " +
                               "FROM CONVERSATIONS WHERE ID_CONVERSATION = @IdConversation";
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = query;
                cmd.Parameters.AddWithValue("IdConversation", IdConversation);
                cmd.Connection = con;
                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    viewConversation.ID_CONVERSATION = dr.GetInt32(0);
                    viewConversation.ID_USER = dr.GetInt32(1);
                    viewConversation.SUBJECT = dr.GetString(2);
                    viewConversation.CONSULTATION = dr.GetString(3);
                    viewConversation.CREATE_DATE = dr.GetDateTime(4);
                    viewConversation.SHARE_CHAT = dr.GetInt32(5);
                    viewConversation.URGENT_CHAT = dr.GetBoolean(6);
                    viewConversation.ATTACHMENT = dr.IsDBNull(7)? "" : dr.GetString(7);
                    viewConversation.AUDIO_RECORDING = dr.IsDBNull(8) ? "" : dr.GetString(8);
                    viewConversation.VIEW_CONVERSATION = dr.IsDBNull(9) ? 0 : dr.GetInt32(9);
                    viewConversation.LAST_ANSWER_DATE = dr.IsDBNull(10) ? (DateTime?)null : dr.GetDateTime(10);
                    viewConversation.LOCK_SHARE_CHAT = dr.IsDBNull(11) ? false : dr.GetBoolean(11);
                    viewConversation.COUNT_ANSWERS = dr.IsDBNull(12) ? 0 : dr.GetInt32(12);
                    return viewConversation;
                }
                else
                {
                    return null;
                }
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

        // Update 'Count Answers' and 'Last Answer Date' only When the lawyer writes the reply
        public static bool UpdateCountAnswers(int ID_CONVERSATION )
        {
            try
            {
                string query = "UPDATE CONVERSATIONS SET " +
                                        "COUNT_ANSWERS = ((SELECT COUNT_ANSWERS FROM CONVERSATIONS WHERE ID_CONVERSATION = @ID_CONVERSATION) + 1), " +
                                        "LAST_ANSWER_DATE = getdate() " +
                               "WHERE ID_CONVERSATION = @ID_CONVERSATION";

                string Scon = ConnectionStringDB.GetConnectionStringDB();
                SqlConnection con = new SqlConnection(Scon);
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = query;
                //cmd.Parameters.AddWithValue("LAST_ANSWER_DATE", LAST_ANSWER_DATE);
                cmd.Parameters.AddWithValue("ID_CONVERSATION", ID_CONVERSATION);
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

        // Update Last Answer Subscriber Date' only When the Subscriber writes the reply
        public static bool UpdateDateReplySubscriber(int ID_CONVERSATION)
        {
            try
            {
                string query = "UPDATE CONVERSATIONS SET " +
                                        "LAST_ANSWER_SUBSCRIBER_DATE = getdate() " +
                               "WHERE ID_CONVERSATION = @ID_CONVERSATION AND LAST_ANSWER_DATE IS NOT NULL";

                string Scon = ConnectionStringDB.GetConnectionStringDB();
                SqlConnection con = new SqlConnection(Scon);
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = query;
                cmd.Parameters.AddWithValue("ID_CONVERSATION", ID_CONVERSATION);
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
        // Update number users whos saw this Conversation
        public static bool UpdateViewConversation(int IdConversation)
        {
            try
            {
                string query = "UPDATE CONVERSATIONS SET VIEW_CONVERSATION = ((SELECT VIEW_CONVERSATION FROM CONVERSATIONS " +
                               "WHERE ID_CONVERSATION = @IdConversation ) + 1 ) WHERE ID_CONVERSATION = @IdConversation";

                string Scon = ConnectionStringDB.GetConnectionStringDB();
                SqlConnection con = new SqlConnection(Scon);
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = query;
                cmd.Parameters.AddWithValue("IdConversation", IdConversation);
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

        // View the last 10 public consultations
        public static List<Conversations> ViewTop10PublicConsultation()
        {
            List<Conversations> ListConversations = new List<Conversations>();
            string Scon = ConnectionStringDB.GetConnectionStringDB();
            SqlConnection con = new SqlConnection(Scon);
            try
            {
                string query = "SELECT TOP 10 C.ID_CONVERSATION, C.ID_USER, U.FULL_NAME, U.GENDER, U.PERSONAL_PICTURE, " +
                               "C.SUBJECT, C.CONSULTATION, C.CREATE_DATE, C.SHARE_CHAT, C.URGENT_CHAT, C.VIEW_CONVERSATION, " +
                               "C.LAST_ANSWER_DATE, C.COUNT_ANSWERS FROM CONVERSATIONS C, USERS U WHERE SHARE_CHAT = 0 " +
                               "AND C.ID_USER = U.ID_USER ORDER BY CREATE_DATE DESC";
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = query;
                cmd.Connection = con;
                SqlDataReader dr = cmd.ExecuteReader();
                int id = 1;
                while (dr.Read())
                {
                    Conversations ViewConversations = new Conversations();
                    ViewConversations.ID_CONVERSATION = dr.GetInt32(0);
                    ViewConversations.ID_USER = dr.GetInt32(1);
                    ViewConversations.FULL_NAME = dr.GetString(2);

                    string Picture = "";
                    int IdGender = dr.GetInt32(3);
                    if (IdGender == 0)
                    {
                        Picture = "LogoImageUserMan.png";

                    }
                    else if (IdGender == 1)
                    {
                        Picture = "LogoImageUserWoman.png";
                    }
                    else
                    {
                        Picture = "LogoImageUser.png";
                    }

                    ViewConversations.PERSONAL_PICTURE = dr.IsDBNull(4) ? Picture : dr.GetString(4);

                    ViewConversations.SUBJECT = dr.GetString(5);
                    ViewConversations.CONSULTATION = dr.GetString(6);
                    ViewConversations.CREATE_DATE = dr.GetDateTime(7);
                    ViewConversations.SHARE_CHAT = dr.GetInt32(8);
                    ViewConversations.URGENT_CHAT = dr.GetBoolean(9);
                    ViewConversations.VIEW_CONVERSATION = dr.IsDBNull(10) ? 0 : dr.GetInt32(10);
                    ViewConversations.LAST_ANSWER_DATE = dr.IsDBNull(11) ? (DateTime?)null : dr.GetDateTime(11);
                    ViewConversations.COUNT_ANSWERS = dr.IsDBNull(12) ? 0 : dr.GetInt32(12);
                    ViewConversations.ID_DEFAULT = id;
                    id++;

                    ListConversations.Add(ViewConversations);
                }
                return ListConversations;
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

        // Update number users whos saw this Conversation
        public static bool ChangeToFreePrivateConsultation(int IdConversation)
        {
            try
            {
                string query = "UPDATE CONVERSATIONS SET SHARE_CHAT = 1, URGENT_CHAT = 0 WHERE ID_CONVERSATION = @IdConversation";

                string Scon = ConnectionStringDB.GetConnectionStringDB();
                SqlConnection con = new SqlConnection(Scon);
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = query;
                cmd.Parameters.AddWithValue("IdConversation", IdConversation);
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

        public static bool LockShareChat(int IdConversation)
        {
            try
            {
                string query = "UPDATE CONVERSATIONS SET LOCK_SHARE_CHAT = 1 WHERE ID_CONVERSATION = @IdConversation";

                string Scon = ConnectionStringDB.GetConnectionStringDB();
                SqlConnection con = new SqlConnection(Scon);
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = query;
                cmd.Parameters.AddWithValue("IdConversation", IdConversation);
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