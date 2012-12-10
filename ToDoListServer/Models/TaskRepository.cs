using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using djondb;
using Newtonsoft.Json.Linq;

namespace ToDoListServer.Models
{
    class TaskRepository : ITaskRepository
    {
        string _connString = "localhost";
        string _dbName = "testdb";
        string _nsName = "todo-list";

        public JArray GetAll()
        {
            var items = new JArray();
            DjondbConnection connection = DjondbConnectionManager.getConnection(_connString);

            try
            {
                connection.open();
                BSONObjVectorPtr res = connection.find(_dbName, _nsName, "");

                foreach (BSONObj entry in res)
                {
                    var item = JObject.Parse(entry.toChar());
                    items.Add(item);
                }

                return items;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (connection != null && connection.isOpen())
                    DjondbConnectionManager.releaseConnection(connection);
            }
        }

        public JObject Get(string id)
        {
            var item = new JObject();
            DjondbConnection connection = DjondbConnectionManager.getConnection(_connString);

            try
            {
                connection.open();
                return GetObject(connection, id);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (connection != null && connection.isOpen())
                    DjondbConnectionManager.releaseConnection(connection);
            }
        }

        public JObject Add(JObject item)
        {
            DjondbConnection connection = DjondbConnectionManager.getConnection(_connString);

            try
            {
                connection.open();

                var id = Guid.NewGuid().ToString();
                item["_id"] = id;

                connection.insert(_dbName, _nsName, item.ToString());

                return GetObject(connection, id);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (connection != null && connection.isOpen())
                    DjondbConnectionManager.releaseConnection(connection);
            }
        }

        public void Update(string id, JObject item)
        {
            DjondbConnection connection = DjondbConnectionManager.getConnection(_connString);

            try
            {
                connection.open();
                BSONObjVectorPtr res = connection.find(_dbName, _nsName, string.Format("$'_id' == '{0}'", id));

                if (res.Count > 0)
                {
                    BSONObj dbItem = res[0];
                    IEnumerable<JProperty> properties = item.Properties();

                    foreach (JProperty property in properties)
                        dbItem.add(property.Name, property.Value.ToString());

                    connection.update(_dbName, _nsName, dbItem);
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (connection != null && connection.isOpen())
                    DjondbConnectionManager.releaseConnection(connection);
            }
        }

        public void Remove(string id)
        {
            DjondbConnection connection = DjondbConnectionManager.getConnection(_connString);

            try
            {
                connection.open();
                BSONObjVectorPtr res = connection.find(_dbName, _nsName, string.Format("$'_id' == '{0}'", id));

                if (res.Count > 0)
                {
                    BSONObj dbItem = res[0];
                    connection.remove(_dbName, _nsName, dbItem.getString("_id"), dbItem.getString("_revision"));
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (connection != null && connection.isOpen())
                    DjondbConnectionManager.releaseConnection(connection);
            }
        }

        private JObject GetObject(DjondbConnection connection, string id)
        {
            var item = new JObject();
            BSONObjVectorPtr res = connection.find(_dbName, _nsName, string.Format("$'_id' == '{0}'", id));

            foreach (BSONObj entry in res)
                item = JObject.Parse(entry.toChar());

            return item;
        }
    }
}
