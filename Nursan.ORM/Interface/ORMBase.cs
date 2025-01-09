using Microsoft.Data.SqlClient;
using Nursan.Domain.Table;
using Nursan.ORM.Result;
using Nursan.ORM.Tool;
using System.Data;
using System.Reflection;

namespace Nursan.ORM.Interface
{
    public class ORMBase<IEntity, OT> : IORM<IEntity> where IEntity : class, new()
         where OT : class, new()
    {
        #region OtherVariable
        string values = "";
        string query = string.Empty;
        private SqlCommand _commmand;
        private static OT _current;
        public static OT Current
        {
            get
            {
                if (_current == null)
                {
                    _current = new OT();
                }
                return _current;
            }
            // set { _current = value; }
        }
        private readonly Type type;
        //private Table TableAtrib;
        private Type ETType
        {
            get
            {
                return typeof(IEntity);
            }
        }
        private Table TableAtrib
        {
            get
            {

                var atribute = ETType.GetCustomAttributes(typeof(Table), false);
                if (atribute.Length > 0 && atribute.Any())
                {
                    Table tbl = (Table)atribute[0];
                    return tbl;
                }
                return null;
            }
            // set { TableAtrib = value; }
        }
        #endregion

        #region Constractor
        public ORMBase()
        {
            _commmand = new SqlCommand();
            _commmand.Connection = Tools.SqlConnection;
            type = typeof(IEntity);
        }

        #endregion

        #region Select
        public Result<List<IEntity>> Select(string whereQuery)
        {
            using (SqlListening listener = new SqlListening())
            {
                query = $"select * from ";

                var attributes = type.GetCustomAttributes(typeof(Table), false);
                if (attributes != null && attributes.Any())//
                {
                    Table tbl = (Table)attributes[0];
                    query += tbl.TableName;
                }
                SqlDataAdapter adp = new SqlDataAdapter($" {query} {whereQuery}", Tools.SqlConnection);
                return adp.ToList<IEntity>();
            }
        }
        public Result<List<IEntity>> SelectQuery(string whereQuery)
        {
            using (SqlListening listener = new SqlListening())
            {

                SqlDataAdapter adp = new SqlDataAdapter($" {whereQuery}", Tools.SqlConnection);
                return adp.ToList<IEntity>();
            }
        }
        public int SelectQuerSayi(string whereQuery)
        {
            using (SqlListening listener = new SqlListening())
            {
                using (SqlCommand command = new SqlCommand(whereQuery, Tools.SqlConnection))
                {
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataTable dataTable = new DataTable();
                    string veri = string.Empty;
                    adapter.Fill(dataTable);
                    // Now, you can work with the DataTable which contains the data from the database
                    foreach (DataRow row in dataTable.Rows)
                    {
                        // Access columns using row["ColumnName"]
                        veri = row.ItemArray[0].ToString();
                    }
                    return int.Parse(veri);
                }
            }
        }
        public Result<List<IEntity>> SelectMany(string whereQuery)
        {
            using (SqlListening listener = new SqlListening())
            {
                query = $"select * from ";

                var attributes = type.GetCustomAttributes(typeof(Table), false);
                if (attributes != null && attributes.Any())//
                {
                    Table tbl = (Table)attributes[0];
                    query += tbl.TableName;
                }
                SqlDataAdapter adp = new SqlDataAdapter($" {query} {whereQuery}", Tools.SqlConnection);
                return adp.ToList<IEntity>();
            }
        }
        #endregion

        #region Insert
        public Result<bool> Insert(IEntity entity)
        {
            query = $" insert into";
            query += string.Format(" {0}(", TableAtrib.TableName);
            string values = $" values(";
            PropertyInfo[] propertyInfos = ETType.GetProperties();
            foreach (PropertyInfo pi in propertyInfos)
            {

                if (pi.Name == TableAtrib.IdentityColumn)
                {
                    continue;
                }

                object value = pi.GetValue(entity);
                if (value == null) continue;
                query += string.Format("{0},", pi.Name);


                if (pi.PropertyType.FullName.Contains("System.DateTime"))
                {
                    values += string.Format("{0},", $"convert(datetime,'{Convert.ToDateTime(value).ToString("dd-MM-yy HH:mm:ss")}',5)");
                }
                else
                {
                    values += string.Format("'{0}',", value);
                }
                _commmand.Parameters.AddWithValue(string.Format("@{0}", pi.Name), value);
            }
            query = query.Remove(query.Length - 1, 1);
            values = values.Remove(values.Length - 1, 1);
            query += string.Format(") {0})", values);
            //query += $"{query}";//; SET IDENTITY_INSERT {TableAtrib.TableName} OFF;
            _commmand.CommandText = query;
            _commmand.Parameters.Clear();
            return _commmand.Exec();
        }
        public Result<int> InsertReturnID(IEntity entity)
        {
            query = $"insert into";
            query += string.Format(" {0}(", TableAtrib.TableName);
            string values = $" values(";
            PropertyInfo[] propertyInfos = ETType.GetProperties();
            foreach (PropertyInfo pi in propertyInfos)
            {

                if (pi.Name == TableAtrib.IdentityColumn)
                {
                    continue;
                }

                object value = pi.GetValue(entity);
                if (value == null) continue;
                query += string.Format("{0},", pi.Name);


                if (pi.PropertyType.FullName.Contains("System.DateTime"))
                {
                    values += string.Format("{0},", $"convert(datetime,'{Convert.ToDateTime(value).ToString("dd-MM-yy HH:mm:ss")}',5)");
                }
                else
                {
                    values += string.Format("'{0}',", value);
                }
                _commmand.Parameters.AddWithValue(string.Format("@{0}", pi.Name), value);
            }
            query = query.Remove(query.Length - 1, 1);
            values = values.Remove(values.Length - 1, 1);
            query += string.Format(") {0})", values);
            _commmand.CommandText = query + ";SELECT SCOPE_IDENTITY()";
            _commmand.Parameters.Clear();
            return _commmand.ExecID();
        }
        #endregion

        #region Update
        public Result<bool> Update(IEntity entity, string key)
        {

            string query = "UPDATE ";
            query += String.Format(TableAtrib.TableName + " ");
            query += "SET ";
            PropertyInfo[] properties = ETType.GetProperties();
            foreach (PropertyInfo pi in properties)
            {
                if (pi.Name == TableAtrib.IdentityColumn)
                {
                    continue;
                }
                object value = pi.GetValue(entity);
                if (value == null) continue;
                else
                {
                    if (pi.PropertyType.FullName.Contains("System.DateTime"))
                    {
                        query += $"{pi.Name} = convert(datetime,'{Convert.ToDateTime(value).ToString("dd-MM-yy HH:mm:ss")}',5),";
                    }
                    else
                    {
                        _commmand.Parameters.AddWithValue(string.Format("{0}", pi.Name), value);
                        query += string.Format("{0}='{1}',", pi.Name, value);
                    }
                    values += string.Format("@{0},", pi.Name);
                }
            }
            values = values.Remove(values.Length - 1, 1);
            query = query.Remove(query.Length - 1, 1);


            query += " WHERE " + TableAtrib.IdentityColumn + "=" + key;

            _commmand.CommandText = query;
            _commmand.Parameters.Clear();
            return _commmand.Exec();
        }

        #endregion

        #region Delete
        public Result<bool> Delete(IEntity entity, string key)
        {
            string query = "DELETE FROM ";
            query += String.Format(TableAtrib.TableName + " ");
            query += " WHERE " + TableAtrib.IdentityColumn + "=" + key;
            _commmand.CommandText = query;
            _commmand.Parameters.Clear();
            return _commmand.Exec();
        }
        public Result<bool> DeleteFromDayYear(IEntity entity, string key)
        {
            string query = "DELETE FROM ";
            query += String.Format(TableAtrib.TableName + " ");
            query += $" WHERE {TableAtrib.PrimaryColumn} = '{key}'";
            _commmand.CommandText = query;
            _commmand.Parameters.Clear();
            return _commmand.Exec();
        }
        #endregion
    }
}

