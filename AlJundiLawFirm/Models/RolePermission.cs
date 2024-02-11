using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AlJundiLawFirm.App_Code;
using System.Data;
using System.Data.SqlClient;

namespace AlJundiLawFirm.Models
{
    public class RolePermission
    {
        // Table 'TYPE_USER'
        public int ID_TYPEUSER { get; set; } // PK in table TYPE_USER, And FK in table ROLE
        public string TYPEUSER { get; set; }

        // Table 'ROLE'
        public int ID_ROLE { get; set; } // PK in table ROLE, and Fk in table ROLE_PERMISSION
        public string TITLE_ROLE { get; set; }

        // Table 'PERMISSIONS'
        public int ID_PERMISSION { get; set; } // PK in table PERMISSIONS, and Fk in table ROLE_PERMISSION
        public string TITLE_PERMISSION { get; set; }
        public string TYPE_PERMISSION { get; set; }

        // Table 'ROLE_PERMISSION'
        public int ID_ROLE_PERMISSION { get; set; }

        public RolePermission() { }
        public RolePermission(int ID_TYPEUSER, string TYPEUSER, int ID_ROLE, string TITLE_ROLE, int ID_PERMISSION, string TITLE_PERMISSION,
                              string TYPE_PERMISSION, int ID_ROLE_PERMISSION)
        {
            this.ID_TYPEUSER = ID_TYPEUSER;
            this.TYPEUSER = TYPEUSER;
            this.ID_ROLE = ID_ROLE;
            this.TITLE_ROLE = TITLE_ROLE;
            this.ID_PERMISSION = ID_PERMISSION;
            this.TITLE_PERMISSION = TITLE_PERMISSION;
            this.TYPE_PERMISSION = TYPE_PERMISSION;
            this.ID_ROLE_PERMISSION = ID_ROLE_PERMISSION;
        }

        // To know the User's Permissions (خالص)
        public static List<RolePermission> GetIdPermissions(int IDRole)
        {
            List<RolePermission> ListPermissions = new List<RolePermission>();
            string Scon = ConnectionStringDB.GetConnectionStringDB();
            SqlConnection con = new SqlConnection(Scon);
            try
            {
                string query = "SELECT ID_ROLE, ID_PERMISSION FROM ROLE_PERMISSION WHERE ID_ROLE =@IDRole " +
                               "ORDER BY ID_PERMISSION ASC";
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = query;
                cmd.Parameters.AddWithValue("IDRole", IDRole);
                cmd.Connection = con;
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    RolePermission Permissions = new RolePermission();
                    Permissions.ID_PERMISSION = dr.GetInt32(1);
                    ListPermissions.Add(Permissions);
                }
                return ListPermissions;
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