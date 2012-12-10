using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ServiceModel;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.SelfHost;
using ToDoListServer.Handlers;

namespace ToDoListServer
{
    class Program
    {
        static readonly Uri _baseAddress = new Uri("http://localhost:50123/");

        static void Main(string[] args)
        {
            HttpSelfHostServer server = null;
            try
            {
                HttpSelfHostConfiguration config = new HttpSelfHostConfiguration(_baseAddress);
                config.HostNameComparisonMode = HostNameComparisonMode.Exact;
                config.Routes.MapHttpRoute(
                    name: "DefaultApi",
                    routeTemplate: "api/{controller}/{id}",
                    defaults: new { id = RouteParameter.Optional }
                );
                config.MessageHandlers.Add(new CorsHandler());

                server = new HttpSelfHostServer(config);
                server.OpenAsync().Wait();

                Console.WriteLine("Running on {0}api/Task", _baseAddress.ToString());
                Console.WriteLine("Hit ENTER to exit...");
                Console.ReadLine();
            }
            catch (Exception e)
            {
                Console.WriteLine("Could not start server: {0}", e.GetBaseException().Message);
                Console.WriteLine("Hit ENTER to exit...");
                Console.ReadLine();
            }
            finally
            {
                if (server != null)
                {
                    server.CloseAsync().Wait();
                }
            }
        }
    }
}
