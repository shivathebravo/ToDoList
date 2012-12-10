using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json.Linq;

namespace ToDoListServer.Models
{
    public interface ITaskRepository
    {
        JArray GetAll();
        JObject Get(string id);
        JObject Add(JObject item);
        void Remove(string id);
        void Update(string id, JObject item);
    }
}
