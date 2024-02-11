using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AlJundiLawFirm.App_Code;
using System.Data;
using System.Data.SqlClient;

namespace AlJundiLawFirm.Models
{
    public class Statistics
    {
        public float NUMBER { get; set; }
        public string GREATER_NAME { get; set; }

        public Statistics() { }

        public Statistics(float NUMBER, string GREATER_NAME)
        {
            this.NUMBER = NUMBER;
            this.GREATER_NAME = GREATER_NAME;
        }

        // Number of urgent consultations not answered
        public static int NumberUrgentConsultationsNotAnswered()
        {
            int NumberUrgent = 0;
            string Scon = ConnectionStringDB.GetConnectionStringDB();
            SqlConnection con = new SqlConnection(Scon);
            try
            {
                string query = "SELECT COUNT(*) FROM CONVERSATIONS WHERE URGENT_CHAT = 1 AND COUNT_ANSWERS = 0";
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

        // Number of consultations not answered
        public static int NumberConsultationsNotAnswered()
        {
            int NumberUrgent = 0;
            string Scon = ConnectionStringDB.GetConnectionStringDB();
            SqlConnection con = new SqlConnection(Scon);
            try
            {
                string query = "SELECT COUNT(*) FROM CONVERSATIONS WHERE COUNT_ANSWERS = 0 AND LOCK_SHARE_CHAT = 0";
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

        // Number consultations
        public static Statistics NumberConsultations()
        {
            Statistics ListNumber = new Statistics();
            float Number = 0f;
            string Scon = ConnectionStringDB.GetConnectionStringDB();
            SqlConnection con = new SqlConnection(Scon);
            try
            {
                string query = "SELECT COUNT(*) FROM CONVERSATIONS";
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = query;
                cmd.Connection = con;
                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    Number = dr.GetInt32(0);
                    if (Number > 1000 && Number < 1000000)
                    {
                        ListNumber.NUMBER = float.Parse(Convert.ToString(Math.Round(Number / 1000, 2)));
                        ListNumber.GREATER_NAME = "K";
                    }
                    else if (Number >= 1000000)
                    {
                        ListNumber.NUMBER = float.Parse(Convert.ToString(Math.Round(Number / 1000000, 2)));
                        ListNumber.GREATER_NAME = "M";
                    }
                    else
                    {
                        ListNumber.NUMBER = float.Parse(Convert.ToString(Number));
                        ListNumber.GREATER_NAME = "";
                    }
                    return ListNumber;
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


        // Number Answers
        public static Statistics NumberAnswers()
        {
            Statistics ListNumber = new Statistics();
            float Number = 0f;
            string Scon = ConnectionStringDB.GetConnectionStringDB();
            SqlConnection con = new SqlConnection(Scon);
            try
            {
                string query = "SELECT COUNT(*) FROM CONVERSATION_REPLIES WHERE ID_USER = 1 OR ID_USER = 2";
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = query;
                cmd.Connection = con;
                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    Number = dr.GetInt32(0);
                    if (Number > 1000 && Number < 1000000)
                    {
                        ListNumber.NUMBER = float.Parse(Convert.ToString(Math.Round(Number / 1000, 2)));
                        ListNumber.GREATER_NAME = "K";
                    }
                    else if (Number >= 1000000)
                    {
                        ListNumber.NUMBER = float.Parse(Convert.ToString(Math.Round(Number / 1000000, 2)));
                        ListNumber.GREATER_NAME = "M";
                    }
                    else
                    {
                        ListNumber.NUMBER = float.Parse(Convert.ToString(Number));
                        ListNumber.GREATER_NAME = "";
                    }
                    return ListNumber;
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

        // Number Users
        public static Statistics NumberUsers()
        {
            Statistics ListNumber = new Statistics();
            float Number = 0f;
            string Scon = ConnectionStringDB.GetConnectionStringDB();
            SqlConnection con = new SqlConnection(Scon);
            try
            {
                string query = "SELECT COUNT(*) FROM USERS WHERE USER_IS_ACTIVE = 1";
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = query;
                cmd.Connection = con;
                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    Number = dr.GetInt32(0);
                    if (Number > 1000 && Number < 1000000)
                    {
                        ListNumber.NUMBER = float.Parse(Convert.ToString(Math.Round(Number / 1000, 2)));
                        ListNumber.GREATER_NAME = "K";
                    }
                    else if (Number >= 1000000)
                    {
                        ListNumber.NUMBER = float.Parse(Convert.ToString(Math.Round(Number / 1000000, 2)));
                        ListNumber.GREATER_NAME = "M";
                    }
                    else
                    {
                        ListNumber.NUMBER = float.Parse(Convert.ToString(Number));
                        ListNumber.GREATER_NAME = "";
                    }
                    return ListNumber;
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


        // SUM VIEW CONVERSATION 
        public static Statistics SumViewConversation()
        {
            Statistics ListNumber = new Statistics();
            float Number = 0f;
            string Scon = ConnectionStringDB.GetConnectionStringDB();
            SqlConnection con = new SqlConnection(Scon);
            try
            {
                string query = "SELECT SUM(VIEW_CONVERSATION) FROM CONVERSATIONS";
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = query;
                cmd.Connection = con;
                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    Number = dr.GetInt32(0);
                    if (Number > 1000 && Number < 1000000)
                    {
                        ListNumber.NUMBER = float.Parse(Convert.ToString(Math.Round(Number / 1000, 2)));
                        ListNumber.GREATER_NAME = "K";
                    }
                    else if (Number >= 1000000)
                    {
                        ListNumber.NUMBER = float.Parse(Convert.ToString(Math.Round(Number / 1000000, 2)));
                        ListNumber.GREATER_NAME = "M";
                    }
                    else
                    {
                        ListNumber.NUMBER = float.Parse(Convert.ToString(Number));
                        ListNumber.GREATER_NAME = "";
                    }
                    return ListNumber;
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

        // Show text number of views
        public static string SumViewAnswer(int NUMBER)
        {
            try
            {
                string SCOUNT_ANSWERS = "";
                int COUNT_ANSWERS = NUMBER;
                if (COUNT_ANSWERS == 0) { SCOUNT_ANSWERS = "لم تتم الإجابة بعد"; }
                else if (COUNT_ANSWERS == 1) { SCOUNT_ANSWERS = "إجابة واحدة"; }
                else if (COUNT_ANSWERS == 2) { SCOUNT_ANSWERS = "إجابتين"; }
                else if (COUNT_ANSWERS >= 3 && COUNT_ANSWERS <= 10) { SCOUNT_ANSWERS = COUNT_ANSWERS.ToString() + " إجابات"; }
                else if (COUNT_ANSWERS >= 11 && COUNT_ANSWERS < 1000) { SCOUNT_ANSWERS = COUNT_ANSWERS.ToString() + " إجابة"; }
                else if (COUNT_ANSWERS >= 1000 && COUNT_ANSWERS < 1000000) { SCOUNT_ANSWERS = (COUNT_ANSWERS / 1000).ToString() + " K إجابة"; }
                else if (COUNT_ANSWERS >= 1000000) { SCOUNT_ANSWERS = (COUNT_ANSWERS / 1000000).ToString() + " M إجابة"; }

                return SCOUNT_ANSWERS;
            }
            catch
            {
                return "لم تتم الإجابة بعد";
            }
        }

        // Show text number of views
        public static string SumViewConversation(int NUMBER)
        {
            try
            {
                string SVIEW_CONVERSATION = "";
                int VIEW_CONVERSATION = NUMBER;
                if (VIEW_CONVERSATION == 0) { SVIEW_CONVERSATION = "ولا مشاهدة"; }
                else if (VIEW_CONVERSATION == 1) { SVIEW_CONVERSATION = "مشاهدة واحدة"; }
                else if (VIEW_CONVERSATION == 2) { SVIEW_CONVERSATION = "مشاهدتين"; }
                else if (VIEW_CONVERSATION >= 3 && VIEW_CONVERSATION <= 10) { SVIEW_CONVERSATION = VIEW_CONVERSATION.ToString() + " مشاهدات"; }
                else if (VIEW_CONVERSATION >= 11 && VIEW_CONVERSATION < 1000) { SVIEW_CONVERSATION = VIEW_CONVERSATION.ToString() + " مشاهدة"; }
                else if (VIEW_CONVERSATION >= 1000 && VIEW_CONVERSATION < 1000000) { SVIEW_CONVERSATION = (VIEW_CONVERSATION / 1000).ToString() + " K مشاهدة"; }
                else if (VIEW_CONVERSATION >= 1000000) { SVIEW_CONVERSATION = (VIEW_CONVERSATION / 1000000).ToString() + " M مشاهدة"; }

                return SVIEW_CONVERSATION;
            }
            catch
            {
                return "ولا مشاهدة";
            }
        }
    }
}