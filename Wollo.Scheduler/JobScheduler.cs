using NLog;
using Quartz;
using Quartz.Impl;
using System;
using System.ComponentModel;
using System.Configuration;
using System.ServiceProcess;
using Wollo.Scheduler.Jobs;

namespace Wollo.Scheduler
{
    //*************************************************************************************
    //              This class is responsible to schedule all jobs in the application
    //*************************************************************************************    
    public partial class JobScheduler : ServiceBase
    {
        static Quartz.IScheduler sched = null;
        public static string jobGroupName = "Wollo.RPE";
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public JobScheduler()
        {
            InitializeComponent();

            this.ServiceName = "Wollo.RPE";

            // These Flags set whether or not to handle that specific
            //  type of event. Set to true if you need it, false otherwise.
            this.CanHandlePowerEvent = true;
            this.CanHandleSessionChangeEvent = true;
            this.CanPauseAndContinue = true;
            this.CanShutdown = true;
            this.CanStop = true;
        }


        protected override void OnStart(string[] args)
        {

            string scheduleFromDatabase = ConfigurationManager.AppSettings["MainJobCronExp"];

            // Code that runs on application startup                
            // construct a factory
            ISchedulerFactory schedFact = new StdSchedulerFactory();
            sched = schedFact.GetScheduler();
            sched.Start();
            MainJobExceution();
        }

        /// <summary>
        /// This is the main job to check or schedule other jobs.
        /// </summary>
        protected void MainJobExceution()
        {
            try
            {
                if (sched.CheckExists(JobKey.Create("MainJob", jobGroupName)))
                {
                    sched.DeleteJob(JobKey.Create("MainJob", jobGroupName));
                }

                string scheduleFromConfig = ConfigurationManager.AppSettings["MainJobCronExp"];

                IJobDetail jobDetailM = JobBuilder.Create<MainJob>().WithIdentity("MainJob", jobGroupName).Build();
                jobDetailM.JobDataMap["srcJob"] = "MainJob";

                logger.Info(String.Format(jobGroupName));
                ITrigger triggerM = TriggerBuilder.Create().WithIdentity("MainTrigger", jobGroupName).WithCronSchedule(scheduleFromConfig).Build();
                sched.ScheduleJob(jobDetailM, triggerM);

                logger.Info(String.Format(scheduleFromConfig));

            }
            catch (Exception ex)
            {
                logger.Error(ex, "Job Scheduler Error");
            }
        }

        protected override void OnStop()
        {
            //  Code that runs on application shutdown
            if (sched != null)
            {
                sched.Shutdown();
            }
        }
    }
}
