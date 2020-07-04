using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClassLibrary1;

namespace WindowsService1
{
    public class HelloWorld : MarshalByRefObject, IHelloWorld
    {
        public string GetString(string message)
        {
            Service1.CreateJobAsync();
            return string.Concat(message, " Hello world");
        }

        public string GetDate()
        {
            return DateTime.Now.ToString();
        }
    }
}
