using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Http;
using ClassLibrary1;

namespace WindowsService1
{
    public partial class Service1 : ServiceBase
    {
        System.Runtime.Remoting.Channels.Http.HttpServerChannel hts;

        Timer timer = new Timer(); // name space(using System.Timers;) 
        static IScheduler scheduler;
        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            hts = new HttpServerChannel(8228);
            ChannelServices.RegisterChannel(hts);
            RemotingConfiguration.RegisterWellKnownServiceType(typeof(CustomerLoader),
                "CustomerLoader", WellKnownObjectMode.Singleton);

            WriteFile.WriteToFile("Service is started at " + DateTime.Now);
            //HostObject();
            timer.Elapsed += new ElapsedEventHandler(OnElapsedTimeAsync);
            timer.Interval = 5000; //number in milisecinds  
            timer.Enabled = true;
            timer.AutoReset = false;
        }

        public static async void CreateJobAsync()
        {
            WriteFile.WriteToFile("dang chay create");
            IJobDetail job = JobBuilder.Create<Job>().WithIdentity("IDGName-moi", "IDGgroup-moi").UsingJobData("JDGjob", "noi dung moi tao").Build();
            ITrigger trigger = TriggerBuilder.Create().WithIdentity("IDGName-moi", "IDGgroup-moi")
                .StartAt(DateTime.Now.AddMinutes(1)).WithPriority(1).Build();

            await scheduler.ScheduleJob(job, trigger);
        }

        protected override void OnStop()
        {
            WriteFile.WriteToFile("Service is stopped at " + DateTime.Now);
        }

        private async void OnElapsedTimeAsync(object source, ElapsedEventArgs e)
        {
            scheduler = await StdSchedulerFactory.GetDefaultScheduler();
            await scheduler.Start();
            var listDate = new List<DateTime>();
            listDate.Add(DateTime.Parse("2020/05/08 09:11"));
            listDate.Add(DateTime.Parse("2020/05/08 09:12"));
            listDate.Add(DateTime.Parse("2020/05/08 2:24 PM"));
            for (var i = 0; i < 3; i++)
            {
                IJobDetail job = JobBuilder.Create<Job>().WithIdentity("IDGName-" + i, "IDGgroup-" + i).UsingJobData("JDGjob", "noi dung " + i).Build();
                ITrigger trigger = TriggerBuilder.Create().WithIdentity("IDGName-" + i, "IDGgroup-" + i).StartAt(listDate[i]).WithPriority(1).Build();

                await scheduler.ScheduleJob(job, trigger);
            }
        }
    }
}
