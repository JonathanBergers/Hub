using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

//added
using System.Data.SqlClient;
using System.Text;

// to be deleted
using System.Diagnostics;

namespace EggwiseLib
{
    public class ConnectionString
    {
        private string sConnectionString;

        public ConnectionString(string source)
        {
            sConnectionString = ConfigurationManager.ConnectionStrings[source].ConnectionString;

            // debug
            Debug.Print(sConnectionString);
        }

        // get record
        public DataTable Select(string sql)
        {
            // debug
            Debug.Print(sql);

            DataTable dt = new DataTable();
            string str = string.Empty;
            try
            {
                SqlDataAdapter da = new SqlDataAdapter(sql, sConnectionString);
                da.SelectCommand.CommandTimeout = 600; // 10 minutes
            da.Fill(dt);
            }

            catch (System.InvalidOperationException ex)
            {
                str = "Source:" + ex.Source;
                str += "\n" + "Message:" + ex.Message;
                str += "\n" + "\n";
                str += "\n" + "Stack Trace :" + ex.StackTrace;
                ErHandler.errorMsg = str;
                ErHandler.throwError();
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                str = "Source:" + ex.Source;
                str += "\n" + "Message:" + ex.Message;
                ErHandler.errorMsg = str;
                ErHandler.throwError();
            }
            catch (System.Exception ex)
            {
                str = "Source:" + ex.Source;
                str += "\n" + "Message:" + ex.Message;
                ErHandler.errorMsg = str;
                ErHandler.throwError();
            }

            return dt;
        }

        // parse sql
        public bool Parse(string sql)
        {
            // debug
            Debug.Print(sql);

            try
            {
                SqlConnection conn = new SqlConnection(sConnectionString);

                using (conn)
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }

            catch
            {
                return false;
            }

            return true;
        }

        // create audit
        public void CreateAuditTable(string table)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("if  not exists (select * from sys.objects where ");
            sql.Append("object_id = object_id('");
            sql.Append(table);
            sql.Append("') and type in ('u')) ");
            sql.Append("create table ");
            sql.Append(table);
            sql.Append("(ID bigint identity(1,1) not null,");
            sql.Append("action nvarchar(50) null,");
            sql.Append("usr nvarchar(50) null,");
            sql.Append("page nvarchar(50) null,");
            sql.Append("descr nvarchar(250) null,");
            sql.Append("ts nvarchar(50) null,");
            sql.Append("constraint pk_");
            sql.Append(table);
            sql.Append(" primary key clustered");
            sql.Append("(ID asc ))");

            // create table
            this.Update(sql.ToString());
        }

        // create logtable
        public void CreateLogTable(string table)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("if  not exists (select * from sys.objects where ");
            sql.Append("object_id = object_id('");
            sql.Append(table);
            sql.Append("') and type in ('u')) ");
            sql.Append("create table ");
            sql.Append(table);
            sql.Append("(ID bigint identity(1,1) not null,");
            sql.Append("ver nvarchar(50) null,");
            sql.Append("usr nvarchar(50) null,");
            sql.Append("page nvarchar(50) null,");
            sql.Append("descr nvarchar(1000) null,");
            sql.Append("ts nvarchar(50) null,");
            sql.Append("constraint pk_");
            sql.Append(table);
            sql.Append(" primary key clustered");
            sql.Append("(ID asc ))");

            // create table
            this.Update(sql.ToString());
        }

        // create authtable
        public void CreateAuthTable(string table, Headers columns)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("if  not exists (select * from sys.objects where ");
            sql.Append("object_id = object_id('");
            sql.Append(table + "_aut");
            sql.Append("') and type in ('u')) ");
            sql.Append("create table ");
            sql.Append(table + "_aut");
            sql.Append("(ID int identity(1,1) not null,");

            foreach (Header h in columns.Items)
            {
                if (h.Name.ToUpper() != "ID")
                {
                    sql.Append(h.Name);
                    sql.Append(" int null, ");
                }
            }
            sql.Append("usr nvarchar(50) null,");
            sql.Append("moddate datetime null,");

            sql.Append("constraint pk_");
            sql.Append(table + "_aut");
            sql.Append(" primary key clustered");
            sql.Append("(ID asc ))");

            // create table
            this.Update(sql.ToString());
        }

        // update record
        public bool Update(string sql)
        {
            // debug
            Debug.Print(sql);

            string str = string.Empty;
            try
            {
                SqlConnection conn = new SqlConnection(sConnectionString);
               
                using(conn)
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Prepare();
                    cmd.CommandTimeout = 600; // 10 minutes
                    cmd.ExecuteNonQuery();
                    conn.Close();
                    return true;
                }
            }
            catch (System.InvalidOperationException ex)
            {
                str = "Source:" + ex.Source;
                str += "\n" + "Message:" + ex.Message;
                str += "\n" + "\n";
                str += "\n" + "Stack Trace :" + ex.StackTrace;
                ErHandler.errorMsg = str;
                ErHandler.throwError();
                return false;
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                str = "Source:" + ex.Source;
                str += "\n" + "Message:" + ex.Message;
                ErHandler.errorMsg = str;
                ErHandler.throwError();
                return false;
            }
            catch (System.Exception ex)
            {
                str = "Source:" + ex.Source;
                str += "\n" + "Message:" + ex.Message;
                ErHandler.errorMsg = str;
                ErHandler.throwError();
                return false;
            }

        }
    }

}
