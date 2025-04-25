using System;
using System.Data;
using System.Data.SqlClient;

namespace StudentManagementSystem
{
    public static class DatabaseHelper
    {
        public static string connStr = "Data Source=MAYUR5365\\SQLEXPRESS; Initial Catalog=StudentManagementDB; Integrated Security=True;";

        public static int ExecuteInsert(string query)
        {
            var conn = new SqlConnection(connStr);
            var cmd = new SqlCommand(query + "; SELECT SCOPE_IDENTITY();", conn);
            conn.Open();
            var result = cmd.ExecuteScalar();
            return Convert.ToInt32(result);
        }

        public static void ExecuteQuery(string query)
        {
            var conn = new SqlConnection(connStr);
             var cmd = new SqlCommand(query, conn);
            conn.Open();
            cmd.ExecuteNonQuery();
        }

        public static DataTable GetData(string query)
        {
            var dt = new DataTable();
            var conn = new SqlConnection(connStr);
            var adapter = new SqlDataAdapter(query, conn);
            adapter.Fill(dt);
            return dt;
        }
    }
}