using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data.EntityClient;
using System.Xml.Linq;

namespace Prototype.Providers
{
    public class ConfigurationEF
    {
        public string ServerName { get; set; }
        public string Database { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        public string ConnectStringName { get; set; }
        public string ConfigPath { get; set; }

        public ConfigurationEF(string _connectStringName, string _configPath)
        {
            ConnectStringName = _connectStringName;
            ConfigPath = _configPath;
        }

        public bool TestConnection()
        {
            try
            {
                using (var sqlConnect = new System.Data.SqlClient.SqlConnection(String.Format(
                    "Data Source={0};Database={1};User Id={2};Password={3}", ServerName, Database, Username, Password)))
                {
                    sqlConnect.Open();

                    if (sqlConnect.State == System.Data.ConnectionState.Open)
                        sqlConnect.Close();
                }

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Test Connection Fail.\n" + ex.Message);
            }

        }

        public void GetEFConnectionString()
        {
            try
            {
                //CreateXDocument and load configuration file
                var doc = XDocument.Load(ConfigPath);

                //Find all connection strings
                var result = from p in doc.Descendants("connectionStrings").Descendants()
                             select p;

                //Go through each connection string elements find atribute specified by argument and replace its value with newVAlue
                foreach (var child in result)
                {
                    foreach (var atr in child.Attributes())
                    {
                        if (atr.Name.LocalName == "name" && atr.Value == ConnectStringName)
                            if (atr.NextAttribute != null && atr.NextAttribute.Name == "connectionString")
                            {
                                // Create the EF connection string from existing
                                var entityBuilder = new EntityConnectionStringBuilder(atr.NextAttribute.Value);
                                var cfgDB = entityBuilder.ProviderConnectionString.Split(';');

                                var servername = cfgDB.First(qry => qry.ToLower().Contains("data source="));
                                var database = cfgDB.First(qry => qry.ToLower().Contains("initial catalog="));
                                var username = cfgDB.First(qry => qry.ToLower().Contains("user id="));
                                var password = cfgDB.First(qry => qry.ToLower().Contains("password="));

                                ServerName = servername.Substring(servername.IndexOf("=") + 1);
                                Database = database.Substring(database.IndexOf("=") + 1);
                                Username = username.Substring(username.IndexOf("=") + 1);
                                Password = password.Substring(password.IndexOf("=") + 1);
                            }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Get Connection String Fail.\n" + ex.Message);
            }
        }

        public void SaveEFConnectionString()
        {

            var connectionString = new System.Data.SqlClient.SqlConnectionStringBuilder()
            {
                DataSource = ServerName,
                InitialCatalog = Database,
                UserID = Username,
                Password = Password,
                MultipleActiveResultSets = true,
                ConnectTimeout = 60,
            };

            ChangeEFConnectionString(connectionString.ConnectionString + ";persist security info=True");
        }
        private void ChangeEFConnectionString(string _newValue)
        {
            try
            {
                //CreateXDocument and load configuration file
                var doc = XDocument.Load(ConfigPath);

                //Find all connection strings
                var result = from p in doc.Descendants("connectionStrings").Descendants()
                             select p;

                //Go through each connection string elements find atribute specified by argument and replace its value with newVAlue
                foreach (var child in result)
                {
                    foreach (var atr in child.Attributes())
                    {
                        if (atr.Name.LocalName == "name" && atr.Value == ConnectStringName)
                            if (atr.NextAttribute != null && atr.NextAttribute.Name == "connectionString")
                            {
                                // Create the EF connection string from existing
                                var entityBuilder = new EntityConnectionStringBuilder(atr.NextAttribute.Value);
                                //
                                entityBuilder.ProviderConnectionString = _newValue;
                                //back the modified connection string to the configuration file
                                atr.NextAttribute.Value = entityBuilder.ToString();
                            }
                    }
                }

                doc.Save(ConfigPath);
            }
            catch (Exception ex)
            {
                throw new Exception("Change Connection String Fail.\n" + ex.Message);
            }
        }
    }

    public enum AccessState
    {
        Read,
        Save
    }
    public class ConfigurationWCF
    {
        public string ServiceUrl { get; set; }
        public string BindingConfigName { get; set; }
        public string ConfigPath { get; set; }

        public ConfigurationWCF(string _bindingConfigName, string _configPath)
        {
            ServiceUrl = string.Empty;
            BindingConfigName = _bindingConfigName;
            ConfigPath = _configPath;
        }

        public void AccessConfigService(AccessState _access)
        {
            try
            {
                //CreateXDocument and load configuration file
                var doc = XDocument.Load(ConfigPath);

                //Find all connection strings
                var child = (from p in doc.Descendants("system.serviceModel").Descendants("client").Descendants()
                             select p).FirstOrDefault();

                //Go through each connection string elements find atribute specified by argument and replace its value with newVAlue
                if (child != null)
                {
                    foreach (var atr in child.Attributes())
                    {
                        if (atr.Name.LocalName == "name" && atr.Value == BindingConfigName)
                        {
                            if (atr.NextAttribute != null && atr.NextAttribute.Name == "address")
                            {
                                switch (_access)
                                {
                                    case AccessState.Read:
                                        ServiceUrl = atr.NextAttribute.Value;
                                        break;
                                    case AccessState.Save:
                                        try
                                        {
                                            atr.NextAttribute.Value = ServiceUrl;
                                            doc.Save(ConfigPath);
                                        }
                                        catch (Exception ex)
                                        {
                                            throw new Exception("Change Connection String Fail.\n" + ex.Message);
                                        }
                                        break;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Get Connection String Fail.\n" + ex.Message);
            }
        }
    }
}
