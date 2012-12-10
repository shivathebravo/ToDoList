using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

using System.Web.Http;
using Newtonsoft.Json.Linq;
using ToDoListServer.Models;

namespace ToDoListServer.Controllers
{
    public class TaskController : ApiController
    {
        private static readonly ITaskRepository _todolist = new TaskRepository();

        public JArray Get()
        {
            try
            {
                var items = _todolist.GetAll();
                Console.WriteLine("Get()> {0}", items.ToString());
                return items;
            }
            catch
            {
                throw new HttpResponseException(HttpStatusCode.InternalServerError);
            }
        }

        public JObject Get(string id)
        {
            var item = _todolist.Get(id);

            if (item == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            Console.WriteLine("Get({0})> {1}", id, item.ToString());
            return item;
        }

        public JObject Post(JObject item)
        {
            Console.WriteLine("Post> {0}", item.ToString());
            return _todolist.Add(item);
        }

        public void Put(string id, JObject item)
        {
            Console.WriteLine("Put({0})> {1}", id, item.ToString());
            _todolist.Update(id, item);
        }

        public void Delete(string id)
        {
            Console.WriteLine("Delete({0})", id);
            _todolist.Remove(id);
        }
    }
}
