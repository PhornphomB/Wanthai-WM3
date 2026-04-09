using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Data.Objects;
using System.Reflection;
using System.Data.Entity;

namespace Prototype.Providers
{
    public class CustomExecuteStore : IDisposable
    {
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }


        public CustomExecuteStore(string _connectionString)
        {
            this.ConnectionString = _connectionString;
        }
        public CustomExecuteStore(DbContext context)
        {
            this.ConnectionString = context.Database.Connection.ConnectionString;
        }

        public enum ExecuteType
        {
            ExecuteReader,
            ExecuteNonQuery,
            ExecuteScalar,
            ExecuteFillTable
        };

        private string ConnectionString { get; set; }
        private SqlConnection Connection { get; set; }
        private SqlCommand Command { get; set; }
        public List<SqlParameter> OutParameters { get; private set; }

        private void Open()
        {
            try
            {
                Connection = new SqlConnection(ConnectionString);
                Connection.Open();
            }
            catch (Exception ex)
            {
                Close();
            }
        }
        private void Close()
        {
            if (Connection != null)
            {
                Connection.Close();
            }
        }


        // executes stored procedure with DB parameteres if they are passed
        private object ExecuteProcedure(string procedureName, ExecuteType executeType, List<SqlParameter> parameters)
        {
            object returnObject = null;

            if (Connection != null)
            {
                if (Connection.State == ConnectionState.Open)
                {
                    Command = new SqlCommand(procedureName, Connection);
                    Command.CommandType = CommandType.StoredProcedure;
                    Command.CommandTimeout = 1200;

                    // pass stored procedure parameters to command
                    if (parameters != null)
                    {
                        Command.Parameters.Clear();
                        Command.Parameters.AddRange(parameters.ToArray());
                    }

                    switch (executeType)
                    {
                        case ExecuteType.ExecuteReader:
                            returnObject = Command.ExecuteReader();
                            break;
                        case ExecuteType.ExecuteNonQuery:
                            returnObject = Command.ExecuteNonQuery();
                            break;
                        case ExecuteType.ExecuteScalar:
                            returnObject = Command.ExecuteScalar();
                            break;
                        case ExecuteType.ExecuteFillTable:
                            var dt = new DataTable();
                            var adapt = new SqlDataAdapter(Command);

                            adapt.Fill(dt);
                            adapt.Dispose();
                            adapt = null;

                            returnObject = dt;
                            break;
                        default:
                            break;
                    }
                }
            }

            return returnObject;
        }

        // updates output parameters from stored procedure
        private void UpdateOutParameters()
        {
            if (Command.Parameters.Count > 0)
            {
                OutParameters = new List<SqlParameter>();
                OutParameters.Clear();

                SqlParameter prm;
                for (int i = 0; i < Command.Parameters.Count; i++)
                {
                    if (Command.Parameters[i].Direction == ParameterDirection.Output)
                    {
                        prm = new SqlParameter(Command.Parameters[i].ParameterName, Command.Parameters[i].Value);
                        prm.Direction = ParameterDirection.Output;
                        OutParameters.Add(prm);
                    }
                }
            }
        }


        public int ExecuteNonQuery(string procedureName)
        {
            return ExecuteNonQuery(procedureName, null);
        }
        public int ExecuteNonQuery(string procedureName, List<SqlParameter> parameters)
        {
            Open();

            int result = (int)ExecuteProcedure(procedureName, ExecuteType.ExecuteNonQuery, parameters);

            UpdateOutParameters();

            Close();

            return result;
        }


        // executes scalar query stored procedure and maps result to single object
        public T ExecuteSingle<T>(string procedureName) where T : new()
        {
            return ExecuteSingle<T>(procedureName, null);
        }
        public T ExecuteSingle<T>(string procedureName, List<SqlParameter> parameters) where T : new()
        {
            Open();
            IDataReader reader = (IDataReader)ExecuteProcedure(procedureName, ExecuteType.ExecuteReader, parameters);
            T tempObject = new T();

            if (reader.Read())
            {
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    PropertyInfo propertyInfo = typeof(T).GetProperty(reader.GetName(i));
                    propertyInfo.SetValue(tempObject, reader.GetValue(i), null);
                }
            }

            reader.Close();

            UpdateOutParameters();

            Close();

            return tempObject;
        }


        // executes list query stored procedure and maps result generic list of objects
        public List<T> ExecuteToList<T>(string procedureName) where T : new()
        {
            return ExecuteToList<T>(procedureName, null);
        }
        public List<T> ExecuteToList<T>(string procedureName, List<SqlParameter> parameters) where T : new()
        {
            List<T> objects = new List<T>();

            Open();
            IDataReader reader = (IDataReader)ExecuteProcedure(procedureName, ExecuteType.ExecuteReader, parameters);

            while (reader.Read())
            {
                T tempObject = new T();

                for (int i = 0; i < reader.FieldCount; i++)
                {
                    if (reader.GetValue(i) != DBNull.Value)
                    {
                        PropertyInfo propertyInfo = typeof(T).GetProperty(reader.GetName(i));

                        if (propertyInfo != null)
                            propertyInfo.SetValue(tempObject, reader.GetValue(i), null);
                    }
                }

                objects.Add(tempObject);
            }

            reader.Close();

            UpdateOutParameters();

            Close();

            return objects;
        }

        public DataTable ExecuteToTable(string procedureName)
        {
            return this.ExecuteToTable(procedureName, null);
        }
        public DataTable ExecuteToTable(string procedureName, List<SqlParameter> parameters)
        {
            return this.ExecuteToTable(procedureName, parameters, null);
        }
        public DataTable ExecuteToTable(string procedureName, List<SqlParameter> parameters, List<string> headerTexts)
        {
            Open();

            DataTable dt = (DataTable)ExecuteProcedure(procedureName, ExecuteType.ExecuteFillTable, parameters);

            UpdateOutParameters();

            Close();

            if ((dt != null && dt.Rows.Count > 0) && (headerTexts != null && headerTexts.Count > 0))
            {
                for (int i = 0; i < headerTexts.Count; i++)
                {
                    dt.Columns[i].ColumnName = headerTexts[i];
                }
            }

            return dt;
        }
    }
}
