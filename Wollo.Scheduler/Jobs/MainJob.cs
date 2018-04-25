using NLog;
using Quartz;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wollo.Data;
using Wollo.Entities.Models;

namespace Wollo.Scheduler.Jobs
{
    /// <summary>
    /// MainJob
    /// </summary>
    public class MainJob : IJob
    {
        // initialization of logger instance
        private static Logger logger = LogManager.GetCurrentClassLogger();
        public void Execute(IJobExecutionContext context)
        {
            try
            {
                logger.Info("Main Job Exceution Start....");
                IScheduler sched = context.Scheduler;
                if (sched == null)
                {
                    logger.Info("Main Job Scheduler is Null");
                }
                else
                {
                    SetUpJobs(sched);
                }
                logger.Info("Main Job Exceution End....");
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Main Job Error");
            }

        }

        /// <summary>
        /// To setup Jobs from the database
        /// </summary>
        /// <param name="sched"></param>
        public void SetUpJobs(IScheduler sched)
        {
            List<JobInfo> jobs = new List<JobInfo>();
            try
            {
                using (PortalContext dbcontext = new PortalContext())
                {
                    jobs = dbcontext.JobInfo.ToList();

                    foreach (JobInfo job in jobs)
                    {
                        JobInfo jobToUpdate = dbcontext.JobInfo.Where(x => x.JobCode == job.JobCode).FirstOrDefault(); //Get job by job code

                        // Check Whether the Job is registered or not
                        bool isRegistered = sched.CheckExists(JobKey.Create(job.JobCode, JobScheduler.jobGroupName));

                        logger.Info(String.Format("Job {0} Status {1}", job.JobCode, isRegistered ? "Registered" : "Not Registered"));

                        //Case: Job Already exists but Cron expression is modified or deactivated by admin
                        if (((bool)job.IsModified || !job.IsActive) && isRegistered)
                        {
                            //Delete the job
                            sched.DeleteJob(JobKey.Create(job.JobCode, JobScheduler.jobGroupName));

                            try
                            {
                                //update job info in database
                                jobToUpdate.IsRunning = false;
                                logger.Info("Modified Job before update :" + jobToUpdate.IsModified);
                                jobToUpdate.IsModified = false;
                                jobToUpdate.updated_date = DateTime.UtcNow;
                                dbcontext.Entry(jobToUpdate).State = EntityState.Modified; //Update the job
                                dbcontext.SaveChanges();
                                logger.Info("Modified Job after update :" + dbcontext.JobInfo.Where(x => x.JobCode == job.JobCode).FirstOrDefault().IsModified);
                            }
                            catch (Exception ex)
                            {
                                logger.Error(ex, "Error");
                            }
                        }

                        //Case: Job is active but not registered yet
                        if (job.IsActive && !isRegistered)
                        {
                            switch (job.JobCode)
                            {
                                case "HolidayJobs":
                                    //**************************************************************
                                    //               Schedule HolidayJobs
                                    //************************************************************** 
                                    IJobDetail rjobDetail = JobBuilder.Create<HolidayJobs>().WithIdentity("HolidayJobs", JobScheduler.jobGroupName).Build();
                                    rjobDetail.JobDataMap["srcJob"] = "HolidayJobs";

                                    ITrigger rtrigger = TriggerBuilder.Create().WithIdentity("HolidayJobsTrigger", JobScheduler.jobGroupName).WithCronSchedule(job.ScheduleType.CronExpression).Build();
                                    sched.ScheduleJob(rjobDetail, rtrigger);
                                    break;
                            }
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, "SetUpJobs Error");
            }
        }
    }
}
