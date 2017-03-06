using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Quartz;
using Service.Model;
using Service.Model.Interface;

namespace Service
{
    public class QueryBuckleInfo : IJob
    {
        private Db db = null;
        private DbInterface db_interface = null;
        private string batch_id = null;

        public void Execute(IJobExecutionContext context)
        {
            db_interface = db_interface ?? new DbInterface();
            batch_id = batch_id ?? context.JobDetail.Key.Name;

            INTERFACE_MIO_BATCH_BZJ batch_interface = db_interface.INTERFACE_MIO_BATCH_BZJ.SingleOrDefault(t => t.FromBatchNo == batch_id);
            if (batch_interface != null && batch_interface.Remark == "处理完成")
            {
                db = new Db();

                var batch = db.MioBatch.SingleOrDefault(t => t.batch_id == batch_id);
                batch.done_date = DateTime.Now;
                batch.push_result = "处理完成";
                db.Update(batch);

                var batch_interface_list =
                    db_interface.INTERFACE_MIO_LIST_BZJ.Where(t => t.FromBatchNo == batch_id).ToList();  //获取清单表

                var batch_list = db.MioList.Where(t => t.batch_id == batch_id).ToList();
                batch_list.ForEach(t =>
                {
                    var batch_detail =
                        batch_interface_list.SingleOrDefault(
                            s =>
                                s.FromBatchNo == batch_id && s.BankAcc == t.bank_account_no &&
                                s.BankAccName == t.bank_account_name);
                    t.result = batch_detail.ErrMsg;  //将清单表信息复制过来

                    db.Update(t);
                });

                db.SaveChanges();

                QuartzManager<QueryBuckleInfo>.RemoveJob(batch_id);
            }
        }
    }

    public class QueryGivesInfo : IJob
    {
        public void Execute(IJobExecutionContext context)
        {

        }
    }
}
