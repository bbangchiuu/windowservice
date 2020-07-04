using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClassLibrary1;
using Quartz;

namespace WindowsService1
{
    public class CustomerLoader : System.MarshalByRefObject, ICustomerLoader
    {
        public void CreateJob()
        {
            Service1.CreateJobAsync();
        }

        public string ExecuteSelectCommand(string selCommand)
        {
            return selCommand + " hello";
        }
    }

    public class Job : IJob
    {
        public Task Execute(IJobExecutionContext context)
        {
            var dataMap = context.JobDetail.JobDataMap;
            string value = dataMap.GetString("JDGjob");
            WriteFile.WriteToFile(value);

            return Task.Factory.StartNew(() => 0);
        }
    }
}
