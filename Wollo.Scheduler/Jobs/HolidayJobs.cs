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
    /// This job is used to update holiday
    /// </summary>
    public class HolidayJobs : IJob
    {
        // To initialize logger instance
        private static Logger logger = LogManager.GetCurrentClassLogger();
        public void Execute(IJobExecutionContext context)
        {
            try
            {
                using (PortalContext dbcontext = new PortalContext())
                {
                    JobInfo job = dbcontext.JobInfo.Where(x => x.JobCode == context.JobDetail.Key.Name).FirstOrDefault(); // Get job by code
                    job.IsRunning = true;
                    job.updated_date = DateTime.UtcNow;
                    dbcontext.Entry(job).State = EntityState.Modified; //Update the job
                    dbcontext.SaveChanges();
                    logger.Info("Holiday Job Exceution Start....");
                }

                using (PortalContext dbcontext = new PortalContext())
                {
                    //Holiday Job logic start
                    DateTime startDate = DateTime.Now.Date;
                    TimeSpan startTime = new TimeSpan(0, 0, 0);
                    DateTime startDateTime = startDate.Add(startTime);
                    DateTime endDate = DateTime.Now.Date;
                    TimeSpan endTime = new TimeSpan(23, 59, 59);
                    DateTime endDateTime = endDate.Add(endTime);
                    //List<Holiday_Master> lstHolidayMaster = AutoMapperHelper.GetInstance().Map<List<Holiday_Master>>(_holidayMasterRepository.FindBy(x => x.holiday_date >= startDateTime && x.holiday_date <= endDateTime).ToList());
                    List<Holiday_Status_Master> lstHolidayStatus = dbcontext.HolidayStatusMaster.ToList();
                    int expireId = lstHolidayStatus.Where(x => x.status == "Expire").FirstOrDefault().id;
                    List<Holiday_Master> lstHoliday = dbcontext.HolidayMaster.Where(x => x.holiday_statusid != expireId).ToList();
                    foreach (var item in lstHoliday)
                    {
                        if ((item.holiday_date - endDateTime).Days <= item.notify_before && (item.holiday_date - endDateTime).Days > 0)
                        {
                            item.holiday_statusid = lstHolidayStatus.Where(x => x.status == "Upcoming").FirstOrDefault().id;
                            item.updated_date = DateTime.UtcNow;
                            item.updated_by = "Scheduler";
                            dbcontext.Entry(item).State = EntityState.Modified;
                        }
                        else if((item.holiday_date - endDateTime).Days == 0)
                        {
                            item.holiday_statusid = lstHolidayStatus.Where(x => x.status == "Active").FirstOrDefault().id;
                            item.updated_date = DateTime.UtcNow;
                            item.updated_by = "Scheduler";
                            dbcontext.Entry(item).State = EntityState.Modified;
                        }
                        else if ((item.holiday_date - endDateTime).Days < 0)
                        {
                            item.holiday_statusid = lstHolidayStatus.Where(x => x.status == "Expire").FirstOrDefault().id;
                            item.updated_date = DateTime.UtcNow;
                            item.updated_by = "Scheduler";
                            dbcontext.Entry(item).State = EntityState.Modified;
                        }
                    }
                    dbcontext.SaveChanges();
                    //Holiday Job logic end
                }

                using (PortalContext dbcontext = new PortalContext())
                {
                    JobInfo job = dbcontext.JobInfo.Where(x => x.JobCode == context.JobDetail.Key.Name).FirstOrDefault(); // Get job by code
                    job.IsRunning = false;
                    job.updated_date = DateTime.UtcNow;
                    dbcontext.Entry(job).State = EntityState.Modified; //Update the job
                    dbcontext.SaveChanges();
                    logger.Info("Holiday Job Exceution End....");
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Holiday Job Error");
            }
        }
    }
}
