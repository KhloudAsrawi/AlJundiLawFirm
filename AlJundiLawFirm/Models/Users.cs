using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AlJundiLawFirm.App_Code;
using System.Data;
using System.Data.SqlClient;

namespace AlJundiLawFirm.Models
{
    public class Users
    {
        public int ID_USER { get; set; }
        public int ID_ROLE { get; set; }
        public string USERNAME { get; set; }
        public string PASSWORD_USER { get; set; }
        public DateTime REGISTERED_AT { get; set; }
        public DateTime LAST_LOGIN { get; set; }
        public DateTime LAST_CHANGE_PASSWORD { get; set; }
        public string VERIFICATION_CODE { get; set; }
        public bool USER_IS_ACTIVE { get; set; }
        public string FULL_NAME { get; set; }
        public string EMAIL { get; set; }
        public string MOBILE_NUMBER { get; set; }
        public int GENDER { get; set; }
        public string PERSONAL_PICTURE { get; set; }

        // from table 'ROle'
        public string TITLE_ROLE { get; set; }

        public Users() { }
        public Users(int ID_USER, int ID_ROLE, string USERNAME, string PASSWORD_USER, DateTime REGISTERED_AT, DateTime LAST_LOGIN, 
                     DateTime LAST_CHANGE_PASSWORD, string VERIFICATION_CODE, bool USER_IS_ACTIVE, string FULL_NAME, string EMAIL,
                     string MOBILE_NUMBER, int GENDER, string PERSONAL_PICTURE, string TITLE_ROLE)
        {
            this.ID_USER = ID_USER;
            this.ID_ROLE = ID_ROLE;
            this.USERNAME = USERNAME;
            this.PASSWORD_USER = PASSWORD_USER;
            this.REGISTERED_AT = REGISTERED_AT;
            this.LAST_LOGIN = LAST_LOGIN;
            this.LAST_CHANGE_PASSWORD = LAST_CHANGE_PASSWORD;
            this.VERIFICATION_CODE = VERIFICATION_CODE;
            this.USER_IS_ACTIVE = USER_IS_ACTIVE;
            this.FULL_NAME = FULL_NAME;
            this.EMAIL = EMAIL;
            this.MOBILE_NUMBER = MOBILE_NUMBER;
            this.GENDER = GENDER;
            this.PERSONAL_PICTURE = PERSONAL_PICTURE;
            this.TITLE_ROLE = TITLE_ROLE;
        }


        // Make sure the "Username" is not repeated
        public static bool CheckUniqueUsername(string Username)
        {
            string Scon = ConnectionStringDB.GetConnectionStringDB();
            SqlConnection con = new SqlConnection(Scon);
            try
            {
                string query = "SELECT USERNAME FROM USERS WHERE USERNAME = @Username";
                con.Open();
                SqlCommand cmd = new SqlCommand(query , con);
                cmd.Parameters.AddWithValue("Username", Username.Trim());
                cmd.Connection = con;
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
            finally
            {
                con.Close();
            }
        }

        // Make sure the "Email" is not repeated 
        public static bool CheckUniqueEmail(string Email)
        {
            string Scon = ConnectionStringDB.GetConnectionStringDB();
            SqlConnection con = new SqlConnection(Scon);
            try
            {
                string query = "SELECT EMAIL FROM USERS WHERE EMAIL = LOWER(@Email)";
                con.Open();
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("Email", Email.Trim());
                cmd.Connection = con;
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
            finally
            {
                con.Close();
            }
        }

        // Make sure the "Mobile" is not repeated 
        public static bool CheckUniqueMobile(string Mobile)
        {
            string Scon = ConnectionStringDB.GetConnectionStringDB();
            SqlConnection con = new SqlConnection(Scon);
            try
            {
                string query = "SELECT MOBILE_NUMBER FROM USERS WHERE MOBILE_NUMBER = @Mobile";
                con.Open();
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("Mobile", Mobile.Trim());
                cmd.Connection = con;
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
            finally
            {
                con.Close();
            }
        }

        // Adding a New User in to DataBase Oracel 
        public static bool InsertUser(int ID_ROLE, string USERNAME, string PASSWORD_USER, string FULL_NAME, string EMAIL, string MOBILE_NUMBER,
                                      int GENDER, string PERSONAL_PICTURE)
        {
            try
            {
                string query = "INSERT INTO USERS (ID_ROLE, USERNAME, PASSWORD_USER, FULL_NAME, EMAIL, MOBILE_NUMBER, GENDER, " +
                               "PERSONAL_PICTURE) VALUES (@ID_ROLE, @USERNAME, @PASSWORD_USER, @FULL_NAME, LOWER(@EMAIL), @MOBILE_NUMBER, @GENDER, " +
                               "@PERSONAL_PICTURE)";
                string Scon = ConnectionStringDB.GetConnectionStringDB();
                SqlConnection con = new SqlConnection(Scon);
                con.Open();
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("ID_ROLE", ID_ROLE);
                cmd.Parameters.AddWithValue("USERNAME", USERNAME);
                cmd.Parameters.AddWithValue("PASSWORD_USER", PASSWORD_USER);
                cmd.Parameters.AddWithValue("FULL_NAME", FULL_NAME);
                cmd.Parameters.AddWithValue("EMAIL", EMAIL);
                cmd.Parameters.AddWithValue("MOBILE_NUMBER", MOBILE_NUMBER != "" ? MOBILE_NUMBER : (object)DBNull.Value);
                cmd.Parameters.AddWithValue("GENDER", GENDER);
                cmd.Parameters.AddWithValue("PERSONAL_PICTURE", PERSONAL_PICTURE != "" ? PERSONAL_PICTURE : (object)DBNull.Value);
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


        // Find and Send "Verification Code" To User
        public static string SelectVerificationCode(string UserName, string Email)
        {
            string Scon = ConnectionStringDB.GetConnectionStringDB();
            SqlConnection con = new SqlConnection(Scon);
            try
            {
                string query = "SELECT VERIFICATION_CODE FROM USERS WHERE (USERNAME =@USERNAME) OR (EMAIL = LOWER(@EMAIL))";
                con.Open();
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.CommandText = query;
                cmd.Parameters.AddWithValue("USERNAME", UserName);
                cmd.Parameters.AddWithValue("EMAIL", Email);
                cmd.Connection = con;
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    return dr.GetString(0);
                }
                else
                {
                    return "";
                }
            }
            catch
            {
                return "";
            }
            finally
            {
                con.Close();
            }
        }


        // User Activation (Is_Active = 1) or User Deactivation (Is_Active = 0)
        public static bool UpdateIsActiveUser(int IsActive, string Username, string Password, string VerificationCode)
        {
            string Scon = ConnectionStringDB.GetConnectionStringDB();
            SqlConnection con = new SqlConnection(Scon);
            try
            {
                string query = "UPDATE USERS SET USER_IS_ACTIVE = @IsActive WHERE (USERNAME = @Username) AND (PASSWORD_USER = @Password) AND (VERIFICATION_CODE = @VerificationCode)";
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = query;
                cmd.Parameters.AddWithValue("IsActive", IsActive);
                cmd.Parameters.AddWithValue("Username", Username);
                cmd.Parameters.AddWithValue("Password", Password);
                cmd.Parameters.AddWithValue("VerificationCode", VerificationCode);
                cmd.Connection = con;
                cmd.ExecuteNonQuery();
                con.Close();
                return true;
            }
            catch
            {
                return false;
            }
            finally
            {

                con.Close();
            }
        }

        // Login Platform and get roles
        public static Users LoginUser(string Username, string Password)
        {
            Users InfoUser = new Users();
            string Scon = ConnectionStringDB.GetConnectionStringDB();
            SqlConnection con = new SqlConnection(Scon);
            try
            {
                string query = "SELECT ID_USER, ID_ROLE , USERNAME, USER_IS_ACTIVE , FULL_NAME , GENDER, PERSONAL_PICTURE " +
                               "FROM USERS WHERE (USERNAME = @Username) AND (PASSWORD_USER= @Password)";
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = query;
                cmd.Parameters.AddWithValue("Username", Username);
                cmd.Parameters.AddWithValue("Password", Password);
                cmd.Connection = con;
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    InfoUser.ID_USER = dr.GetInt32(0);
                    InfoUser.ID_ROLE = dr.GetInt32(1);
                    InfoUser.USERNAME = dr.GetString(2);
                    InfoUser.USER_IS_ACTIVE = dr.GetBoolean(3);
                    InfoUser.FULL_NAME = dr.GetString(4);

                    string Picture = "";
                    int IdGender = dr.GetInt32(5);
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

                    InfoUser.PERSONAL_PICTURE = dr.IsDBNull(6) ? Picture : dr.GetString(6);
                    return InfoUser;
                }
                else
                {
                    return null;
                }

            }
            catch(Exception ex)
            {
                InfoUser.USER_IS_ACTIVE = false;
                InfoUser.USERNAME = "DataBase Error";
                InfoUser.PASSWORD_USER = ex.Message;
                return InfoUser;
            }
            finally
            {

                con.Close();
            }
        }

        // Check Information User to Change PassWord if Forget
        public static Users CheckInfoChangePassWord(string Email, string VerificationCode)
        {
            Users InfoUser = new Users();
            string Scon = ConnectionStringDB.GetConnectionStringDB();
            SqlConnection con = new SqlConnection(Scon);
            try
            {
                string query = "SELECT PASSWORD_USER FROM USERS WHERE VERIFICATION_CODE = @VerificationCode AND EMAIL = LOWER(@Email)";
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = query;
                cmd.Parameters.AddWithValue("VerificationCode", VerificationCode);
                cmd.Parameters.AddWithValue("Email", Email);
                cmd.Connection = con;
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    InfoUser.PASSWORD_USER = dr.GetString(0);
                    return InfoUser;
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

        // Update password ( change password ) and Add modification date
        public static bool UpdatePassword(string Email, string VerificationCode, string Password)
        {
            DateTime todayDate = DateTime.Now;
            string LastChange = todayDate.ToString("MM/dd/yyyy hh:mm:ss");

            try
            {
                string query = "UPDATE USERS SET PASSWORD_USER =@Password, LAST_CHANGE_PASSWORD =@LastChange " +
                               "WHERE EMAIL= LOWER(@Email) AND VERIFICATION_CODE = @VerificationCode";

                string Scon = ConnectionStringDB.GetConnectionStringDB();
                SqlConnection con = new SqlConnection(Scon);
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = query;
                cmd.Parameters.AddWithValue("Password", Password);
                cmd.Parameters.AddWithValue("LastChange", LastChange);
                cmd.Parameters.AddWithValue("Email", Email);
                cmd.Parameters.AddWithValue("VerificationCode", VerificationCode);
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

        // Update password ( change password ) and Add modification date
        public static bool UpdatePassword(int ID_USER, string Password)
        {
            DateTime todayDate = DateTime.Now;
            string LastChange = todayDate.ToString("MM/dd/yyyy hh:mm:ss");

            try
            {
                string query = "UPDATE USERS SET PASSWORD_USER =@Password, LAST_CHANGE_PASSWORD =@LastChange " +
                               "WHERE ID_USER = @ID_USER";

                string Scon = ConnectionStringDB.GetConnectionStringDB();
                SqlConnection con = new SqlConnection(Scon);
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = query;
                cmd.Parameters.AddWithValue("Password", Password);
                cmd.Parameters.AddWithValue("LastChange", LastChange);
                cmd.Parameters.AddWithValue("ID_USER", ID_USER);
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

        // Display user information based on Id User
        public static Users SelectInfoUser(int ID_USER)
        {
            Users InfoUser = new Users();
            string Scon = ConnectionStringDB.GetConnectionStringDB();
            SqlConnection con = new SqlConnection(Scon);
            try
            {
                string query = "SELECT U.ID_USER, U.ID_ROLE, U.USERNAME, U.PASSWORD_USER, U.REGISTERED_AT, U.LAST_LOGIN, " +
                               "U.LAST_CHANGE_PASSWORD, U.VERIFICATION_CODE, U.USER_IS_ACTIVE, U.FULL_NAME, LOWER(U.EMAIL), " +
                               "U.MOBILE_NUMBER, U.GENDER, U.PERSONAL_PICTURE, R.TITLE_ROLE FROM USERS U, ROLE R " +
                               "WHERE U.ID_ROLE = R.ID_ROLE AND ID_USER = @ID_USER AND USER_IS_ACTIVE = 1";    
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = query;
                cmd.Parameters.AddWithValue("ID_USER", ID_USER);
                cmd.Connection = con;
                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    //PERSONAL_PICTURE NULL
                    InfoUser.ID_USER = dr.GetInt32(0);
                    InfoUser.ID_ROLE = dr.GetInt32(1);
                    InfoUser.USERNAME = dr.GetString(2);
                    InfoUser.PASSWORD_USER = dr.GetString(3);
                    InfoUser.REGISTERED_AT = dr.GetDateTime(4);
                    InfoUser.LAST_LOGIN = dr.GetDateTime(5);
                    InfoUser.LAST_CHANGE_PASSWORD = dr.GetDateTime(6);
                    InfoUser.VERIFICATION_CODE = dr.GetString(7);
                    InfoUser.USER_IS_ACTIVE = dr.GetBoolean(8);
                    InfoUser.FULL_NAME = dr.GetString(9);
                    InfoUser.EMAIL = dr.GetString(10);
                    InfoUser.MOBILE_NUMBER = dr.IsDBNull(11) ? "" : dr.GetString(11);
                    int IdGender = dr.GetInt32(12);
                    InfoUser.GENDER = IdGender;

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

                    InfoUser.PERSONAL_PICTURE = dr.IsDBNull(13) ? LogoImageParson : dr.GetString(13);
                    InfoUser.TITLE_ROLE = dr.GetString(14);

                    return InfoUser;
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

        // Update Personal Picture for user
        public static bool UpdatePersonalPicture(int ID_USER, string PERSONALPICTURE)
        {
            try
            {
                string query = "UPDATE USERS SET PERSONAL_PICTURE =@PERSONALPICTURE WHERE ID_USER =@ID_USER";
                string Scon = ConnectionStringDB.GetConnectionStringDB();
                SqlConnection con = new SqlConnection(Scon);
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = query;
                cmd.Parameters.Add("ID_USER", SqlDbType.Int).Value = ID_USER;
                cmd.Parameters.Add("PERSONALPICTURE", SqlDbType.VarChar).Value = PERSONALPICTURE != "" ? PERSONALPICTURE : (object)DBNull.Value;
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

        public static bool UpdatePersonalPicture(int ID_USER)
        {
            try
            {
                string PERSONALPICTURE = "";
                string query = "UPDATE USERS SET PERSONAL_PICTURE =@PERSONALPICTURE WHERE ID_USER =@ID_USER";
                string Scon = ConnectionStringDB.GetConnectionStringDB();
                SqlConnection con = new SqlConnection(Scon);
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = query;
                cmd.Parameters.Add("ID_USER", SqlDbType.Int).Value = ID_USER;
                cmd.Parameters.Add("PERSONALPICTURE", SqlDbType.VarChar).Value = PERSONALPICTURE != "" ? (object)DBNull.Value : (object)DBNull.Value;
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

        // Update Full Name
        public static bool UpdateFullName(int ID_USER, string FULL_NAME)
        {
            try
            {
                string query = "UPDATE USERS SET FULL_NAME = @FULL_NAME WHERE ID_USER =@ID_USER";
                string Scon = ConnectionStringDB.GetConnectionStringDB();
                SqlConnection con = new SqlConnection(Scon);
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = query;
                cmd.Parameters.Add("ID_USER", SqlDbType.Int).Value = ID_USER;
                cmd.Parameters.Add("FULL_NAME", SqlDbType.NVarChar).Value = FULL_NAME != "" ? FULL_NAME : "مشترك مجهول الهوية";
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

        // Update Gender
        public static bool UpdateGender(int ID_USER, int GENDER)
        {
            try
            {
                string query = "UPDATE USERS SET GENDER = @GENDER WHERE ID_USER =@ID_USER";
                string Scon = ConnectionStringDB.GetConnectionStringDB();
                SqlConnection con = new SqlConnection(Scon);
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = query;
                cmd.Parameters.Add("ID_USER", SqlDbType.Int).Value = ID_USER;
                cmd.Parameters.Add("GENDER", SqlDbType.Int).Value = GENDER;
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

        // Update Gender Email
        public static bool UpdateEmail(int ID_USER, string EMAIL)
        {
            try
            {
                string query = "UPDATE USERS SET EMAIL = LOWER(@EMAIL) WHERE ID_USER =@ID_USER";
                string Scon = ConnectionStringDB.GetConnectionStringDB();
                SqlConnection con = new SqlConnection(Scon);
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = query;
                cmd.Parameters.Add("ID_USER", SqlDbType.Int).Value = ID_USER;
                cmd.Parameters.Add("EMAIL", SqlDbType.VarChar).Value = EMAIL;
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

        // Update Gender Mobile
        public static bool UpdateMobile(int ID_USER, string MOBILE_NUMBER)
        {
            try
            {
                string query = "UPDATE USERS SET MOBILE_NUMBER = @MOBILE_NUMBER WHERE ID_USER =@ID_USER";
                string Scon = ConnectionStringDB.GetConnectionStringDB();
                SqlConnection con = new SqlConnection(Scon);
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = query;
                cmd.Parameters.AddWithValue("ID_USER", ID_USER);
                cmd.Parameters.AddWithValue("MOBILE_NUMBER", MOBILE_NUMBER != "" ? MOBILE_NUMBER : (object)DBNull.Value);
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