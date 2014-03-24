﻿[module:
    System.Diagnostics.CodeAnalysis.SuppressMessage(
        "StyleCop.CSharp.DocumentationRules", "*",
        Justification = "Reviewed. Suppression of all documentation rules is OK here.")]

namespace Vendord.SmartDevice.Linked
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlServerCe;
    using System.IO;
    using System.Linq;
     
    public class DbQueryExecutor
    {
        private string _connectionString;

        public DbQueryExecutor(string connectionString)
        {
            _connectionString = connectionString;
        }

        public object ExecuteScalar(string cmdText, SqlCeParameter[] parameters)
        {
            object result = null;
            using (var conn = new SqlCeConnection(this._connectionString))
            {
                conn.Open();
                SqlCeCommand cmd = conn.CreateCommand();
                cmd.CommandText = cmdText;
                if (parameters != null)
                {
                    cmd.Parameters.AddRange(parameters);
                }
                result = cmd.ExecuteScalar();
                cmd.Parameters.Clear();
            }

            return result;
        }

        public int ExecuteNonQuery(string cmdText, SqlCeParameter[] parameters)
        {
            int rowsAffected;
            rowsAffected = 0;

            using (SqlCeConnection conn = new SqlCeConnection(this._connectionString))
            {
                conn.Open();
                SqlCeCommand cmd = conn.CreateCommand();
                cmd.CommandText = cmdText;
                if (parameters != null)
                {
                    cmd.Parameters.AddRange(parameters);
                }
                rowsAffected = cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
            }

            return rowsAffected;
        }
    }
}