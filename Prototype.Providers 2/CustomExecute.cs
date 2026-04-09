using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Data.Objects;

namespace Prototype.Providers
{
    public static class CustomExecute
    {
        public static int ExecuteSql(this System.Data.Objects.ObjectContext context, string _commandText)
        {
            int complete = 0;

            var entityConnection = (System.Data.EntityClient.EntityConnection)context.Connection;

            using (SqlConnection conn = new SqlConnection(entityConnection.StoreConnection.ConnectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(_commandText))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = conn;
                    complete = cmd.ExecuteNonQuery();
                }
            }

            return complete;
        }

        public static DataTable QuerySql(this System.Data.Objects.ObjectContext context, string _commandText, string _tableName)
        {
            var dt = new DataTable();

            var entityConnection = (System.Data.EntityClient.EntityConnection)context.Connection;

            using (SqlConnection conn = new SqlConnection(entityConnection.StoreConnection.ConnectionString))
            {
                conn.Open();
                using (SqlDataAdapter cmd = new SqlDataAdapter(_commandText, conn))
                {
                    cmd.Fill(dt);
                }
            }

            dt.TableName = _tableName;

            return dt;
        }

        public static int ExecuteStoreFunction(this System.Data.Objects.ObjectContext context, string _storeName, SqlParameter[] _parameter)
        {
            return ExecuteStoreFunction<int>(context, _storeName, _parameter, null);
        }
        public static TType ExecuteStoreFunction<TType>(this System.Data.Objects.ObjectContext context, string _storeName, SqlParameter[] _parameters, SqlParameter _prmOutput)
        {
            TType _obj = default(TType);

            var entityConnection = (System.Data.EntityClient.EntityConnection)context.Connection;

            using (SqlConnection conn = new SqlConnection(entityConnection.StoreConnection.ConnectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(_storeName))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    foreach (var prm in _parameters.Where(qry => qry.Direction != ParameterDirection.Output))
                    {
                        cmd.Parameters.AddWithValue(prm.ParameterName, prm.Value);
                    }

                    if (_prmOutput != null)
                    {
                        _prmOutput.Direction = ParameterDirection.Output;
                        cmd.Parameters.Add(_prmOutput);

                        cmd.Connection = conn;
                        cmd.ExecuteNonQuery();
                        _obj = (TType)_prmOutput.Value;
                    }
                    else
                    {
                        SqlParameter _parmReturnValue = default(SqlParameter);

                        _parmReturnValue = cmd.Parameters.Add("RETURN_VALUE", SqlDbType.Int);
                        _parmReturnValue.Direction = ParameterDirection.ReturnValue;

                        cmd.Connection = conn;
                        cmd.ExecuteNonQuery();
                        _obj = (TType)_parmReturnValue.Value;
                    }

                }
            }

            return _obj;
        }

        public static DataTable ExecuteStoreFunction2Table(this System.Data.Objects.ObjectContext context, string _storeName, SqlParameter[] _parameters)
        {
            var entityConnection = (System.Data.EntityClient.EntityConnection)context.Connection;
            DataTable dt = new DataTable();

            using (SqlConnection conn = new SqlConnection(entityConnection.StoreConnection.ConnectionString))
            {
                conn.Open();

                using (SqlCommand cmd = new SqlCommand(_storeName))
                {
                    
                    cmd.CommandType = CommandType.StoredProcedure;

                    foreach (var prm in _parameters)
                    {
                        cmd.Parameters.AddWithValue(prm.ParameterName, prm.Value);
                    }

                    cmd.Connection = conn;
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                   
                }
            }

            return dt;
        }
        public static DataTable ExecuteStoreFunction2Table(this System.Data.Objects.ObjectContext context, string _storeName, SqlParameter[] _parameters,List<string> lHeaderName)
        {
            var entityConnection = (System.Data.EntityClient.EntityConnection)context.Connection;
            DataTable dt = new DataTable();

            using (SqlConnection conn = new SqlConnection(entityConnection.StoreConnection.ConnectionString))
            {
                conn.Open();

                using (SqlCommand cmd = new SqlCommand(_storeName))
                {

                    cmd.CommandType = CommandType.StoredProcedure;

                    if (_parameters != null)
                    {
                        foreach (var prm in _parameters)
                        {
                            cmd.Parameters.AddWithValue(prm.ParameterName, prm.Value);
                        }
                    }
                    

                    cmd.Connection = conn;
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);

                }
            }

            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < lHeaderName.Count; i++)
                {
                    dt.Columns[i].ColumnName = lHeaderName[i];
                }
            }
            return dt;
        }
    }
}
