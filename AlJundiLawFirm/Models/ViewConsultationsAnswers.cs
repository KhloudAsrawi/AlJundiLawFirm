using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AlJundiLawFirm.App_Code;
using System.Data;
using System.Data.SqlClient;

namespace AlJundiLawFirm.Models
{
    public class ViewConsultationsAnswers
    {
        public int ID_CONVERSATION { get; set; }
        public int ID_USER_ASK { get; set; }
        public string FULL_NAME_USER_ASK { get; set; }
        public int GENDER_USER_ASK { get; set; }
        public string PERSONAL_PICTURE_USER_ASK { get; set; }
        public string SUBJECT { get; set; }
        public string CONSULTATION { get; set; }
        public DateTime CREATEDATE_ASK { get; set; }
        public int SHARE_CHAT { get; set; }
        public bool URGENT_CHAT { get; set; }
        public int VIEW_CONVERSATION { get; set; }
        public DateTime? LAST_ANSWER_DATE { get; set; }
        public int COUNT_ANSWERS { get; set; }
        public int ID_USER_ANSWER { get; set; }
        public string FULL_NAME_USER_ANSWER { get; set; }
        public bool LOCK_SHARE_CHAT { get; set; }
        public DateTime? LAST_ANSWER_SUBSCRIBER_DATE { get; set; }

        public ViewConsultationsAnswers() { }
        public ViewConsultationsAnswers(int ID_CONVERSATION, int ID_USER_ASK, string FULL_NAME_USER_ASK, int GENDER_USER_ASK,
                                        string PERSONAL_PICTURE_USER_ASK, string SUBJECT, string CONSULTATION, 
                                        DateTime CREATEDATE_ASK, int SHARE_CHAT, bool URGENT_CHAT, int VIEW_CONVERSATION, 
                                        DateTime LAST_ANSWER_DATE, int COUNT_ANSWERS, int ID_USER_ANSWER, 
                                        string FULL_NAME_USER_ANSWER, bool LOCK_SHARE_CHAT, DateTime LAST_ANSWER_SUBSCRIBER_DATE)
        {
            this.ID_CONVERSATION = ID_CONVERSATION;
            this.ID_USER_ASK = ID_USER_ASK;
            this.FULL_NAME_USER_ASK = FULL_NAME_USER_ASK;
            this.GENDER_USER_ASK = GENDER_USER_ASK;
            this.PERSONAL_PICTURE_USER_ASK = PERSONAL_PICTURE_USER_ASK;
            this.SUBJECT = SUBJECT;
            this.CONSULTATION = CONSULTATION;
            this.CREATEDATE_ASK = CREATEDATE_ASK;
            this.SHARE_CHAT = SHARE_CHAT;
            this.URGENT_CHAT = URGENT_CHAT;
            this.VIEW_CONVERSATION = VIEW_CONVERSATION;
            this.LAST_ANSWER_DATE = LAST_ANSWER_DATE;
            this.COUNT_ANSWERS = COUNT_ANSWERS;
            this.ID_USER_ANSWER = ID_USER_ANSWER;
            this.FULL_NAME_USER_ANSWER = FULL_NAME_USER_ANSWER;
            this.LOCK_SHARE_CHAT = LOCK_SHARE_CHAT;
            this.LAST_ANSWER_SUBSCRIBER_DATE = LAST_ANSWER_SUBSCRIBER_DATE;
        }

        // Get count of consultations answered by the Lawyer without count repetition in the same consultation
        public static int NumberOfConsultationsAnswered(int ID_USER_ANSWER)
        {
            int MaxIdReply = 0;
            string Scon = ConnectionStringDB.GetConnectionStringDB();
            SqlConnection con = new SqlConnection(Scon);
            try
            {
                string query = "SELECT COUNT(DISTINCT ID_CONVERSATION) FROM VIEW_CONSULTATIONS_ANSWERS WHERE ID_USER_ANSWER = " +
                               "@ID_USER_ANSWER";
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = query;
                cmd.Parameters.AddWithValue("ID_USER_ANSWER", ID_USER_ANSWER);
                cmd.Connection = con;
                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    MaxIdReply = dr.GetInt32(0);
                    return MaxIdReply;
                }
                else
                {
                    return 0;
                }
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

        public static int NumberOfConsultationsAnsweredSubscriber(int ID_USER_ANSWER)
        {
            int MaxIdReply = 0;
            string Scon = ConnectionStringDB.GetConnectionStringDB();
            SqlConnection con = new SqlConnection(Scon);
            try
            {
                string query = "SELECT COUNT(DISTINCT ID_CONVERSATION) FROM VIEW_CONSULTATIONS_ANSWERS WHERE " +
                               "ID_USER_ANSWER = @ID_USER_ANSWER AND LAST_ANSWER_SUBSCRIBER_DATE > LAST_ANSWER_DATE " +
                               "AND LOCK_SHARE_CHAT = 0";
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = query;
                cmd.Parameters.AddWithValue("ID_USER_ANSWER", ID_USER_ANSWER);
                cmd.Connection = con;
                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    MaxIdReply = dr.GetInt32(0);
                    return MaxIdReply;
                }
                else
                {
                    return 0;
                }
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

        // If Lawyer Answers to the Conversations, will be display Conversations people.
        // Use 'ID_USER_ANSWER' to find Answers 's Lawyer 
        public static List<ViewConsultationsAnswers> ViewListConsultationsAnswers(int ID_USER_ANSWER, int RANGE)
        {
            List<ViewConsultationsAnswers> ListConsultationsAnswers = new List<ViewConsultationsAnswers>();

            string Scon = ConnectionStringDB.GetConnectionStringDB();
            SqlConnection con = new SqlConnection(Scon);
            try
            {
                string query = "SELECT ID_CONVERSATION, ID_USER_ASK, FULL_NAME_USER_ASK, GENDER_USER_ASK, " +
                               "PERSONAL_PICTURE_USER_ASK, SUBJECT, CAST(CONSULTATION AS NVARCHAR(1000)), CREATEDATE_ASK, " +
                               "SHARE_CHAT, URGENT_CHAT, VIEW_CONVERSATION, COUNT_ANSWERS, LAST_ANSWER_SUBSCRIBER_DATE, " +
                               "FULL_NAME_USER_ANSWER, LOCK_SHARE_CHAT, MAX(LAST_ANSWER_DATE) AS LAST_ANSWER_DATE  " +
                               "FROM VIEW_CONSULTATIONS_ANSWERS WHERE ID_USER_ANSWER = @ID_USER_ANSWER " +
                               "GROUP BY ID_CONVERSATION, ID_USER_ASK, FULL_NAME_USER_ASK, " +
                               "GENDER_USER_ASK, PERSONAL_PICTURE_USER_ASK,SUBJECT, CAST(CONSULTATION AS NVARCHAR(1000)), " +
                               "CREATEDATE_ASK, SHARE_CHAT, URGENT_CHAT, VIEW_CONVERSATION, COUNT_ANSWERS, " +
                               "LAST_ANSWER_SUBSCRIBER_DATE, FULL_NAME_USER_ANSWER, LOCK_SHARE_CHAT ORDER BY " +
                               "MAX(LAST_ANSWER_DATE) DESC OFFSET @RANGE ROWS FETCH NEXT 20 ROWS ONLY";
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = query;
                cmd.Parameters.AddWithValue("ID_USER_ANSWER", ID_USER_ANSWER);
                cmd.Parameters.AddWithValue("RANGE", RANGE);
                cmd.Connection = con;
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {                             
                    ViewConsultationsAnswers ConsultationsAnswers = new ViewConsultationsAnswers();
                    ConsultationsAnswers.ID_CONVERSATION = dr.GetInt32(0);
                    ConsultationsAnswers.ID_USER_ASK = dr.GetInt32(1);
                    ConsultationsAnswers.FULL_NAME_USER_ASK = dr.GetString(2);
                    int IdGender = dr.GetInt32(3);
                    ConsultationsAnswers.GENDER_USER_ASK = IdGender;

                    // To help choose a user's photo by gender
                    string LogoImageParson = "";
                    if (IdGender == 0)
                    {
                        LogoImageParson = "LogoImageUserMan.png";
                    }
                    else if (IdGender == 1)
                    {
                        LogoImageParson = "LogoImageUserWoman.png";
                    }
                    else if (IdGender == 2)
                    {
                        LogoImageParson = "LogoImageUser.png";
                    }
                    ConsultationsAnswers.PERSONAL_PICTURE_USER_ASK = dr.IsDBNull(4) ? LogoImageParson : dr.GetString(4);

                    ConsultationsAnswers.SUBJECT = dr.GetString(5);
                    ConsultationsAnswers.CONSULTATION = dr.GetString(6);
                    ConsultationsAnswers.CREATEDATE_ASK = dr.GetDateTime(7);
                    ConsultationsAnswers.SHARE_CHAT = dr.GetInt32(8);
                    ConsultationsAnswers.URGENT_CHAT = dr.GetBoolean(9);
                    ConsultationsAnswers.VIEW_CONVERSATION = dr.IsDBNull(10) ? 0 : dr.GetInt32(10);
                    ConsultationsAnswers.COUNT_ANSWERS = dr.IsDBNull(11) ? 0 : dr.GetInt32(11);
                    ConsultationsAnswers.LAST_ANSWER_SUBSCRIBER_DATE = dr.IsDBNull(12) ? (DateTime?)null : dr.GetDateTime(12);
                    ConsultationsAnswers.FULL_NAME_USER_ANSWER = dr.GetString(13);
                    ConsultationsAnswers.LOCK_SHARE_CHAT = dr.GetBoolean(14);
                    ConsultationsAnswers.LAST_ANSWER_DATE = dr.IsDBNull(15) ? (DateTime?)null : dr.GetDateTime(15);
 
                    ListConsultationsAnswers.Add(ConsultationsAnswers);
                }
                return ListConsultationsAnswers;
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

        // Display Conversations people not have Answers
        public static List<ViewConsultationsAnswers> ViewListConsultationsNoAnswers(int RANGE)
        {
            List<ViewConsultationsAnswers> ListConsultationsNoAnswers = new List<ViewConsultationsAnswers>();

            string Scon = ConnectionStringDB.GetConnectionStringDB();
            SqlConnection con = new SqlConnection(Scon);
            try
            {
                string query = "SELECT ID_CONVERSATION, ID_USER_ASK, FULL_NAME_USER_ASK, GENDER_USER_ASK, " +
                               "PERSONAL_PICTURE_USER_ASK, SUBJECT, CONSULTATION, CREATEDATE_ASK, SHARE_CHAT, URGENT_CHAT, " +
                               "VIEW_CONVERSATION FROM VIEW_CONSULTATIONS_NO_ANSWERS ORDER BY CREATEDATE_ASK ASC " +
                               "OFFSET @RANGE ROWS FETCH NEXT 20 ROWS ONLY";
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = query;
                cmd.Parameters.AddWithValue("RANGE", RANGE);
                cmd.Connection = con;
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    ViewConsultationsAnswers ConsultationsNoAnswers = new ViewConsultationsAnswers();
                    ConsultationsNoAnswers.ID_CONVERSATION = dr.GetInt32(0);
                    ConsultationsNoAnswers.ID_USER_ASK = dr.GetInt32(1);
                    ConsultationsNoAnswers.FULL_NAME_USER_ASK = dr.GetString(2);
                    int IdGender = dr.GetInt32(3);
                    ConsultationsNoAnswers.GENDER_USER_ASK = IdGender;

                    // To help choose a user's photo by gender
                    string LogoImageParson = "";
                    if (IdGender == 0)
                    {
                        LogoImageParson = "LogoImageUserMan.png";
                    }
                    else if (IdGender == 1)
                    {
                        LogoImageParson = "LogoImageUserWoman.png";
                    }
                    else if (IdGender == 2)
                    {
                        LogoImageParson = "LogoImageUser.png";
                    }
                    ConsultationsNoAnswers.PERSONAL_PICTURE_USER_ASK = dr.IsDBNull(4) ? LogoImageParson : dr.GetString(4);

                    ConsultationsNoAnswers.SUBJECT = dr.GetString(5);
                    ConsultationsNoAnswers.CONSULTATION = dr.GetString(6);
                    ConsultationsNoAnswers.CREATEDATE_ASK = dr.GetDateTime(7);
                    ConsultationsNoAnswers.SHARE_CHAT = dr.GetInt32(8);
                    ConsultationsNoAnswers.URGENT_CHAT = dr.GetBoolean(9);
                    ConsultationsNoAnswers.VIEW_CONVERSATION = dr.IsDBNull(10) ? 0 : dr.GetInt32(10);

                    ListConsultationsNoAnswers.Add(ConsultationsNoAnswers);
                }
                return ListConsultationsNoAnswers;
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

        // Display Conversations people not have Answers
        public static List<ViewConsultationsAnswers> ViewListPaidConsultationsNoAnswers(int RANGE)
        {
            List<ViewConsultationsAnswers> ListConsultationsNoAnswers = new List<ViewConsultationsAnswers>();

            string Scon = ConnectionStringDB.GetConnectionStringDB();
            SqlConnection con = new SqlConnection(Scon);
            try
            {
                string query = "SELECT ID_CONVERSATION, ID_USER_ASK, FULL_NAME_USER_ASK, GENDER_USER_ASK, " +
                               "PERSONAL_PICTURE_USER_ASK, SUBJECT, CONSULTATION, CREATEDATE_ASK, SHARE_CHAT, URGENT_CHAT, " +
                               "VIEW_CONVERSATION, LAST_ANSWER_DATE, COUNT_ANSWERS FROM VIEW_CONSULTATIONS_NO_ANSWERS " +
                               "WHERE URGENT_CHAT = 1 ORDER BY CREATEDATE_ASK ASC " +
                               "OFFSET @RANGE ROWS FETCH NEXT 20 ROWS ONLY";
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = query;
                cmd.Parameters.AddWithValue("RANGE", RANGE);
                cmd.Connection = con;
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    ViewConsultationsAnswers ConsultationsNoAnswers = new ViewConsultationsAnswers();
                    ConsultationsNoAnswers.ID_CONVERSATION = dr.GetInt32(0);
                    ConsultationsNoAnswers.ID_USER_ASK = dr.GetInt32(1);
                    ConsultationsNoAnswers.FULL_NAME_USER_ASK = dr.GetString(2);
                    int IdGender = dr.GetInt32(3);
                    ConsultationsNoAnswers.GENDER_USER_ASK = IdGender;

                    // To help choose a user's photo by gender
                    string LogoImageParson = "";
                    if (IdGender == 0)
                    {
                        LogoImageParson = "LogoImageUserMan.png";
                    }
                    else if (IdGender == 1)
                    {
                        LogoImageParson = "LogoImageUserWoman.png";
                    }
                    else if (IdGender == 2)
                    {
                        LogoImageParson = "LogoImageUser.png";
                    }
                    ConsultationsNoAnswers.PERSONAL_PICTURE_USER_ASK = dr.IsDBNull(4) ? LogoImageParson : dr.GetString(4);

                    ConsultationsNoAnswers.SUBJECT = dr.GetString(5);
                    ConsultationsNoAnswers.CONSULTATION = dr.GetString(6);
                    ConsultationsNoAnswers.CREATEDATE_ASK = dr.GetDateTime(7);
                    ConsultationsNoAnswers.SHARE_CHAT = dr.GetInt32(8);
                    ConsultationsNoAnswers.URGENT_CHAT = dr.GetBoolean(9);
                    ConsultationsNoAnswers.VIEW_CONVERSATION = dr.IsDBNull(10) ? 0 : dr.GetInt32(10);

                    ListConsultationsNoAnswers.Add(ConsultationsNoAnswers);
                }
                return ListConsultationsNoAnswers;
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

        // Count Public Consultations
        public static int NumberPublicConsultations()
        {
            int NumberUrgent = 0;
            string Scon = ConnectionStringDB.GetConnectionStringDB();
            SqlConnection con = new SqlConnection(Scon);
            try
            {
                string query = "SELECT COUNT(*) FROM VIEW_CONVERSATION_WITH_SUBSCRIBERS WHERE SHARE_CHAT = 0";
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = query;
                cmd.Connection = con;
                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    NumberUrgent = dr.GetInt32(0);
                    return NumberUrgent;
                }
                else
                {
                    return 0;
                }
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

        // Count Public Consultations according to the text
        public static int NumberPublicConsultations(string text)
        {
            int NumberUrgent = 0;
            string Scon = ConnectionStringDB.GetConnectionStringDB();
            SqlConnection con = new SqlConnection(Scon);
            try
            {
                string query = "SELECT COUNT(*) FROM VIEW_CONVERSATION_WITH_SUBSCRIBERS WHERE SHARE_CHAT = 0 AND ("+ text +")";
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = query;
                cmd.Connection = con;
                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    NumberUrgent = dr.GetInt32(0);
                    return NumberUrgent;
                }
                else
                {
                    return 0;
                }
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


        // Sorting the consultations according to the type of sorting example "Recently Asked", 
        // "Most Active", "Recently Answer" ... etc 
        public static List<ViewConsultationsAnswers> SortConsulting(int Range, string order)
        {
            List<ViewConsultationsAnswers> ListConsultations = new List<ViewConsultationsAnswers>();

            string Scon = ConnectionStringDB.GetConnectionStringDB();
            SqlConnection con = new SqlConnection(Scon);
            try
            {
                string query = "SELECT ID_CONVERSATION, ID_USER_ASK, FULL_NAME_USER_ASK, GENDER_USER_ASK, " +
                               "PERSONAL_PICTURE_USER_ASK, SUBJECT, CONSULTATION, CREATEDATE_ASK, SHARE_CHAT, URGENT_CHAT, " +
                               "VIEW_CONVERSATION, LAST_ANSWER_DATE, COUNT_ANSWERS, LOCK_SHARE_CHAT " +
                               "FROM VIEW_CONVERSATION_WITH_SUBSCRIBERS " +
                               "WHERE SHARE_CHAT = 0 ORDER BY " + order + " OFFSET @Range ROWS FETCH NEXT 20 ROWS ONLY";

                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = query;
                cmd.Parameters.AddWithValue("Range", Range);
                cmd.Connection = con;
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    ViewConsultationsAnswers PublicConsultation = new ViewConsultationsAnswers();
                    PublicConsultation.ID_CONVERSATION = dr.GetInt32(0);
                    PublicConsultation.ID_USER_ASK = dr.GetInt32(1);
                    PublicConsultation.FULL_NAME_USER_ASK = dr.GetString(2);
                    int IdGender = dr.GetInt32(3);
                    PublicConsultation.GENDER_USER_ASK = IdGender;

                    // To help choose a user's photo by gender
                    string LogoImageParson = "";
                    if (IdGender == 0)
                    {
                        LogoImageParson = "LogoImageUserMan.png";
                    }
                    else if (IdGender == 1)
                    {
                        LogoImageParson = "LogoImageUserWoman.png";
                    }
                    else if (IdGender == 2)
                    {
                        LogoImageParson = "LogoImageUser.png";
                    }
                    PublicConsultation.PERSONAL_PICTURE_USER_ASK = dr.IsDBNull(4) ? LogoImageParson : dr.GetString(4);

                    PublicConsultation.SUBJECT = dr.GetString(5);
                    PublicConsultation.CONSULTATION = dr.GetString(6);
                    PublicConsultation.CREATEDATE_ASK = dr.GetDateTime(7);
                    PublicConsultation.SHARE_CHAT = dr.GetInt32(8);
                    PublicConsultation.URGENT_CHAT = dr.GetBoolean(9);
                    PublicConsultation.VIEW_CONVERSATION = dr.IsDBNull(10) ? 0 : dr.GetInt32(10);
                    PublicConsultation.LAST_ANSWER_DATE = dr.IsDBNull(11) ? (DateTime?)null : dr.GetDateTime(11);
                    PublicConsultation.COUNT_ANSWERS = dr.GetInt32(12);
                    PublicConsultation.LOCK_SHARE_CHAT = dr.GetBoolean(13);

                    ListConsultations.Add(PublicConsultation);
                }
                return ListConsultations;
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

        public static List<ViewConsultationsAnswers> SelectSearchByText(int Range, string Text)
        {
            List<ViewConsultationsAnswers> ListConsultations = new List<ViewConsultationsAnswers>();

            string Scon = ConnectionStringDB.GetConnectionStringDB();
            SqlConnection con = new SqlConnection(Scon);
            try
            {
                string query = "SELECT ID_CONVERSATION, ID_USER_ASK, FULL_NAME_USER_ASK, GENDER_USER_ASK, " +
                               "PERSONAL_PICTURE_USER_ASK, SUBJECT, CONSULTATION, CREATEDATE_ASK, SHARE_CHAT, URGENT_CHAT, " +
                               "VIEW_CONVERSATION, LAST_ANSWER_DATE, COUNT_ANSWERS, LOCK_SHARE_CHAT " +
                               "FROM VIEW_CONVERSATION_WITH_SUBSCRIBERS " +
                               "WHERE SHARE_CHAT = 0 AND (" + Text + ") ORDER BY CREATEDATE_ASK DESC " +
                               "OFFSET 0 ROWS FETCH NEXT 20 ROWS ONLY";

                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = query;
                cmd.Parameters.AddWithValue("Range", Range);
                cmd.Connection = con;
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    ViewConsultationsAnswers PublicConsultation = new ViewConsultationsAnswers();
                    PublicConsultation.ID_CONVERSATION = dr.GetInt32(0);
                    PublicConsultation.ID_USER_ASK = dr.GetInt32(1);
                    PublicConsultation.FULL_NAME_USER_ASK = dr.GetString(2);
                    int IdGender = dr.GetInt32(3);
                    PublicConsultation.GENDER_USER_ASK = IdGender;

                    // To help choose a user's photo by gender
                    string LogoImageParson = "";
                    if (IdGender == 0)
                    {
                        LogoImageParson = "LogoImageUserMan.png";
                    }
                    else if (IdGender == 1)
                    {
                        LogoImageParson = "LogoImageUserWoman.png";
                    }
                    else if (IdGender == 2)
                    {
                        LogoImageParson = "LogoImageUser.png";
                    }
                    PublicConsultation.PERSONAL_PICTURE_USER_ASK = dr.IsDBNull(4) ? LogoImageParson : dr.GetString(4);

                    PublicConsultation.SUBJECT = dr.GetString(5);
                    PublicConsultation.CONSULTATION = dr.GetString(6);
                    PublicConsultation.CREATEDATE_ASK = dr.GetDateTime(7);
                    PublicConsultation.SHARE_CHAT = dr.GetInt32(8);
                    PublicConsultation.URGENT_CHAT = dr.GetBoolean(9);
                    PublicConsultation.VIEW_CONVERSATION = dr.IsDBNull(10) ? 0 : dr.GetInt32(10);
                    PublicConsultation.LAST_ANSWER_DATE = dr.IsDBNull(11) ? (DateTime?)null : dr.GetDateTime(11);
                    PublicConsultation.COUNT_ANSWERS = dr.GetInt32(12);
                    PublicConsultation.LOCK_SHARE_CHAT = dr.GetBoolean(13);

                    ListConsultations.Add(PublicConsultation);
                }
                return ListConsultations;
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

        // Number of consultations offered by the subscriber (ID_USER_ASK)
        public static int NumberOfConsultationsBySubscriber(int ID_USER_ASK)
        {
            int MaxIdReply = 0;
            string Scon = ConnectionStringDB.GetConnectionStringDB();
            SqlConnection con = new SqlConnection(Scon);
            try
            {
                string query = "SELECT COUNT(*) FROM VIEW_CONVERSATION_WITH_SUBSCRIBERS WHERE ID_USER_ASK = @ID_USER_ASK";
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = query;
                cmd.Parameters.AddWithValue("ID_USER_ASK", ID_USER_ASK);
                cmd.Connection = con;
                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    MaxIdReply = dr.GetInt32(0);
                    return MaxIdReply;
                }
                else
                {
                    return 0;
                }
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

        // Number of consultations offered by the subscriber(ID_USER_ASK)
        public static List<ViewConsultationsAnswers> SelectConsultationsToSubscriber(int Range, int ID_USER_ASK)
        {
            List<ViewConsultationsAnswers> ListConsultations = new List<ViewConsultationsAnswers>();

            string Scon = ConnectionStringDB.GetConnectionStringDB();
            SqlConnection con = new SqlConnection(Scon);
            try
            {
                string query = "SELECT ID_CONVERSATION, ID_USER_ASK, FULL_NAME_USER_ASK, GENDER_USER_ASK, " +
                               "PERSONAL_PICTURE_USER_ASK, SUBJECT, CONSULTATION, CREATEDATE_ASK, SHARE_CHAT, URGENT_CHAT, " +
                               "VIEW_CONVERSATION, LAST_ANSWER_DATE, COUNT_ANSWERS, LOCK_SHARE_CHAT " +
                               "FROM VIEW_CONVERSATION_WITH_SUBSCRIBERS " +
                               "WHERE ID_USER_ASK = @ID_USER_ASK ORDER BY CREATEDATE_ASK DESC " +
                               "OFFSET @Range ROWS FETCH NEXT 20 ROWS ONLY";

                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = query;
                cmd.Parameters.AddWithValue("Range", Range);
                cmd.Parameters.AddWithValue("ID_USER_ASK", ID_USER_ASK);
                cmd.Connection = con;
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    ViewConsultationsAnswers PublicConsultation = new ViewConsultationsAnswers();
                    PublicConsultation.ID_CONVERSATION = dr.GetInt32(0);
                    PublicConsultation.ID_USER_ASK = dr.GetInt32(1);
                    PublicConsultation.FULL_NAME_USER_ASK = dr.GetString(2);
                    int IdGender = dr.GetInt32(3);
                    PublicConsultation.GENDER_USER_ASK = IdGender;

                    // To help choose a user's photo by gender
                    string LogoImageParson = "";
                    if (IdGender == 0)
                    {
                        LogoImageParson = "LogoImageUserMan.png";
                    }
                    else if (IdGender == 1)
                    {
                        LogoImageParson = "LogoImageUserWoman.png";
                    }
                    else if (IdGender == 2)
                    {
                        LogoImageParson = "LogoImageUser.png";
                    }
                    PublicConsultation.PERSONAL_PICTURE_USER_ASK = dr.IsDBNull(4) ? LogoImageParson : dr.GetString(4);

                    PublicConsultation.SUBJECT = dr.GetString(5);
                    PublicConsultation.CONSULTATION = dr.GetString(6);
                    PublicConsultation.CREATEDATE_ASK = dr.GetDateTime(7);
                    PublicConsultation.SHARE_CHAT = dr.GetInt32(8);
                    PublicConsultation.URGENT_CHAT = dr.GetBoolean(9);
                    PublicConsultation.VIEW_CONVERSATION = dr.IsDBNull(10) ? 0 : dr.GetInt32(10);
                    PublicConsultation.LAST_ANSWER_DATE = dr.IsDBNull(11) ? (DateTime?)null : dr.GetDateTime(11);
                    PublicConsultation.COUNT_ANSWERS = dr.GetInt32(12);
                    PublicConsultation.LOCK_SHARE_CHAT = dr.GetBoolean(13);

                    ListConsultations.Add(PublicConsultation);
                }
                return ListConsultations;
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