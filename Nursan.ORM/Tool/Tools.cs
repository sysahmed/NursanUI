using Microsoft.Data.SqlClient;
using Nursan.Logging.Messages;
using Nursan.ORM.Result;
using System.Data;
using System.Reflection;

namespace Nursan.ORM.Tool
{
    public static class Tools
    {
        private static SqlConnection _sqlConnection;// = $"Data Source = {XMLStaticIslemler.XmlokuServer()}; Initial Catalog = {XMLStaticIslemler.XmlokuDb()}; User Id={XMLStaticIslemler.XmlokuDbKullanici()}; Password={XMLStaticIslemler.XmlokuDbSifre()}";
        //public static string ConnectionString(string str)
        public static string _sqlConnectionString = $"data source={XMLTools.XMLSeverIp.XmlServerIP()};initial catalog=UretimOtomasyon;user id=sa;password=wrjkd34mk22;TrustServerCertificate=True";
        public static SqlConnection SqlConnection
        {
            get
            {
                if (_sqlConnection == null)
                    // _sqlConnection = new SqlConnection(_sqlConnection);
                    _sqlConnection = new SqlConnection(_sqlConnectionString);
                return _sqlConnection;
            }
            set { _sqlConnection = value; }
        }

        public static IEnumerable<PropertyInfo> GetNormalProperties(Type entityType)
        {
            var rest = entityType.GetProperties().Where(property => !IsGenericCollection(property.PropertyType));
            return rest;
        }
        public static bool IsGenericCollection(Type type)
        {
            var result = type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>);
            return result;
            // Можете да промените този метод, за да включите или изключите други генерични колекции според нуждите ви.
        }
        public static Result<List<IEntity>> ToList<IEntity>(this SqlDataAdapter adp) where IEntity : class, new()
        {
            try
            {
                DataTable dt = new DataTable();
                adp.Fill(dt);
                Type t = typeof(IEntity);
                List<IEntity> list = new List<IEntity>();
                PropertyInfo[] properties = t.GetProperties();
                foreach (DataRow e in dt.Rows)
                {
                    IEntity eT = new IEntity();
                    foreach (PropertyInfo pi in properties)
                    {
                        if (!pi.PropertyType.ToString().Contains("System.Collections.Generic.ICollection") && !pi.PropertyType.ToString().Contains("Entity"))
                        {
                            object value = e[pi.Name];
                            try
                            {
                                if (value != null)
                                {
                                    pi.SetValue(eT, value, null);
                                }
                            }
                            catch (Exception ex)
                            {
                                Messaglama.MessagException(ex.Message + ex.GetType()); ;
                            }
                        }
                    }

                    list.Add(eT);
                }
                return new Result<List<IEntity>>
                {
                    IsSuccess = true,
                    Message = "",
                    Data = list
                };
            }
            catch (Exception ex)
            {
                Messaglama.MessagException(ex.Message + ex.GetType() + adp.SelectCommand.CommandText);
                return new Result<List<IEntity>>
                {
                    IsSuccess = false,
                    Message = ex.Message,
                };
            }
        }
        public static Result<bool> Exec(this SqlCommand command)
        {
            try
            {
                if (command.Connection.State != ConnectionState.Open)
                    command.Connection.Open();
                int result = command.ExecuteNonQuery();
                return new Result<bool>
                {
                    IsSuccess = true,
                    Message = "",
                    Data = result > 0
                };
            }
            catch (Exception ex)
            {
                Messaglama.MessagException(ex.Message + ex.GetType() + command.CommandText);
                if (command.Connection == null)
                {
                    command.Connection.Open();
                    int result = command.ExecuteNonQuery();


                    return new Result<bool>
                    {
                        IsSuccess = true,
                        Message = ex.Message,
                        Data = result > 0
                    };
                }
                else
                {
                    return new Result<bool>
                    {
                        IsSuccess = false,
                        Message = ex.Message,

                    };
                }
            }
            finally
            {
                if (command.Connection != null)
                    if (command.Connection.State != ConnectionState.Closed)
                    {
                        command.Connection.Close();
                        command.Dispose();
                    }
            }
        }
        public static Result<int> ExecID(this SqlCommand command)
        {
            try
            {
                if (command.Connection.State != ConnectionState.Open)
                    command.Connection.Open();
                var result1 = command.ExecuteScalar().ToString();
                int result = Convert.ToInt32(result1);
                return new Result<int>
                {
                    IsSuccess = true,
                    Message = "",
                    Data = result
                };
            }
            catch (Exception ex)
            {
                Messaglama.MessagException(ex.Message + ex.GetType() + command.CommandText);
                if (command.Connection == null)
                {
                    command.Connection.Open();
                    int result = command.ExecuteNonQuery();


                    return new Result<int>
                    {
                        IsSuccess = true,
                        Message = ex.Message,
                        Data = 0
                    };
                }
                else
                {
                    return new Result<int>
                    {
                        IsSuccess = false,
                        Message = ex.Message,

                    };
                }
            }
            finally
            {
                if (command.Connection != null)
                    if (command.Connection.State != ConnectionState.Closed)
                    {
                        command.Connection.Close();
                        command.Dispose();
                    }
            }
        }
    }
}



