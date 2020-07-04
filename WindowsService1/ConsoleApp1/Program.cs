using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Remoting;
using ClassLibrary1;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Http;
using System.Net.Http;
using System.Net;
using System.IO;
using System.Web.Script.Serialization;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            ChannelServices.RegisterChannel(new HttpClientChannel());//chạy 1 lần

            ICustomerLoader custl = (ICustomerLoader)Activator.GetObject(typeof(ICustomerLoader), "http://localhost:8228/CustomerLoader");
            if (custl != null)
            {
                Console.WriteLine("HTTP SERVER OFFLINE ...PLEASE TRY LATER");
                custl.CreateJob();
            }

            //GetApiAsync();
            Console.ReadKey();
        }

        public static async Task GetApiAsync()
        {
            var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://localhost:44340/ApiAjax/GetDataTest");
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                string json = new JavaScriptSerializer().Serialize(new { username = "bang" });

                streamWriter.Write(json);
            }

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
                Console.WriteLine(result);
            }
        }
    }
}
