using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Quartz;
using Quartz.Impl;

namespace Service
{
    public class QuartzManager<T> where T : class, IJob
    {
        //调度程序的工厂的接口中 实例一个具体的调度方法
        private static ISchedulerFactory schedulerFactory = new StdSchedulerFactory();
        //JOb群组名
        private static string JOB_GROUP_NAME = "JOBGROUP_NAME";
        //触发器群
        private static string TRIGGER_GROUP_NAME = "TRIGGERGROUP_NAME";

        /// <summary>
        /// 添加一个定时任务，使用默认的任务组名，触发器名，触发器组名
        /// </summary>
        /// <param name="pStrJobName">任务名</param>
        /// <param name="pStrCronExpress">触发器表达式</param>
        public static void AddJob(string pStrJobName, string pStrCronExpress)
        {
            try
            {
                //接口中获取调度工厂的  GetScheduler()  方法
                IScheduler sched = schedulerFactory.GetScheduler();
                //创建任务
                IJobDetail job = JobBuilder.Create<T>().WithIdentity(pStrJobName, JOB_GROUP_NAME).Build();
                //创建触发器
                ITrigger trigger = TriggerBuilder.Create()
                    .WithIdentity(pStrJobName, TRIGGER_GROUP_NAME)
                    .WithCronSchedule(pStrCronExpress)
                    .Build();

                sched.ScheduleJob(job, trigger);
                sched.Start();
            }
            catch (Exception e)
            {

                throw new Exception(e.Message);
            }
        }


        /// <summary>
        /// 移除一个任务(使用默认的任务组名，触发器名，触发器组名)
        /// </summary>
        /// <param name="pStrJobName"></param>
        public static void RemoveJob(string pStrJobName)
        {
            try
            {
                IScheduler sched = schedulerFactory.GetScheduler();
                JobKey jobkey = new JobKey(pStrJobName);
                TriggerKey triggerKey = new TriggerKey(pStrJobName, TRIGGER_GROUP_NAME);

                //停止触发器
                sched.PauseTrigger(triggerKey);
                //移除触发器
                sched.UnscheduleJob(triggerKey);
                //删除任务
                sched.DeleteJob(jobkey);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }


        /// <summary>
        /// 修改一个任务的触发时间(使用默认的任务组名，触发器名，触发器组名)
        /// </summary>
        /// <param name="pStrJobName">任务名</param>
        /// <param name="pStrCronExpress">触发器表达式</param>
        public static void ModifyJobTime(string pStrJobName, string pStrCronExpress, IDictionary<string, object> pDictionary)
        {
            try
            {
                IScheduler sched = schedulerFactory.GetScheduler();
                TriggerKey triggerKey = new TriggerKey(pStrJobName, TRIGGER_GROUP_NAME);
                ICronTrigger trigger = (ICronTrigger)sched.GetTrigger(triggerKey);
                if (trigger == null)
                {
                    return;
                }
                RemoveJob(pStrJobName);
                AddJob(pStrJobName, pStrCronExpress);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }


        /// <summary>
        ///  开启所有定时任务
        /// </summary>
        public static void StartAllJobs()
        {
            try
            {
                IScheduler sched = schedulerFactory.GetScheduler();
                sched.Start();
            }
            catch (Exception e)
            {

                throw new Exception(e.Message);
            }
        }

        /// <summary>
        /// 关闭所有的定时任务
        /// </summary>
        public static void ShutdownAllJobs()
        {
            try
            {
                IScheduler sched = schedulerFactory.GetScheduler();
                if (!sched.IsShutdown)
                {
                    sched.Shutdown();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        /// <summary>
        /// 恢复所有的任务
        /// </summary>
        public static void ResumeAllJobs()
        {
            try
            {
                IScheduler sched = schedulerFactory.GetScheduler();
                if (!sched.IsShutdown)
                {
                    sched.ResumeAll();
                }
            }
            catch (Exception e)
            {

                throw new Exception(e.Message);
            }
        }

        /// <summary>
        /// 暂停所有的作业
        /// </summary>
        public static void PauseAllJobs()
        {
            try
            {
                IScheduler sched = schedulerFactory.GetScheduler();
                sched.PauseAll();
            }
            catch (Exception e)
            {

                throw new Exception(e.Message);
            }
        }
    }
}
