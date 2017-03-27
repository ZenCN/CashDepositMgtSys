using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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

            INTERFACE_MIO_BATCH_BZJ batch_interface =
                db_interface.INTERFACE_MIO_BATCH_BZJ.SingleOrDefault(t => t.FromBatchNo == batch_id);
            if (batch_interface != null && batch_interface.BatchStatus == 5) //处理完成
            {
                try
                {
                    db = new Db();

                    var batch = db.MioBatch.SingleOrDefault(t => t.batch_id == batch_id);
                    batch.done_date = DateTime.Now;
                    batch.push_result = batch_interface.Remark;
                    db.Update(batch);

                    var batch_interface_list =
                        db_interface.INTERFACE_MIO_LIST_BZJ.Where(t => t.FromBatchNo == batch_id).ToList(); //获取清单表

                    Generation_buckle buckle = null;

                    var batch_list = db.MioList.Where(t => t.batch_id == batch_id).ToList();
                    List<string> phones = new List<string>();
                    batch_list.ForEach(t =>
                    {
                        var batch_detail =
                            batch_interface_list.SingleOrDefault(
                                s =>
                                    s.FromBatchNo == batch_id && s.BankAcc == t.bank_account_no &&
                                    s.BankAccName == t.bank_account_name);

                        t.status = batch_detail.MioStatus;
                        t.result = batch_detail.ErrMsg; //将清单表信息复制过来
                        db.Update(t);

                        buckle = db.Generation_buckle.SingleOrDefault(b => b.id == t.generation_id);
                        if (buckle != null)
                        {
                            if (t.status == 0) //0表示处理成功
                            {
                                buckle.review_state = 5;

                                int count = 0;
                                int cur_num = 1;
                                string max_num = db.Generation_buckle.Max(g => g.gather_serial_num);
                                /*--------------------生成流水号--------------------*/
                                if (max_num != null)
                                {
                                    cur_num = int.Parse(max_num.Substring(12));
                                    cur_num++;
                                }
                                max_num = cur_num.ToString();
                                count = 8 - cur_num.ToString().Length;
                                for (int i = 0; i < count; i++)
                                {
                                    max_num = "0" + max_num;
                                }
                                buckle.gather_serial_num = DateTime.Now.Year + buckle.agency_code + "88" + max_num;
                                /*--------------------生成虚拟销售人员代码--------------------*/
                                cur_num = 1;
                                max_num = db.Generation_buckle.Max(g => g.salesman_code);
                                if (max_num != null)
                                {
                                    cur_num = int.Parse(max_num.Substring(6));
                                    cur_num++;
                                }
                                max_num = cur_num.ToString();
                                count = 8 - cur_num.ToString().Length;
                                for (int i = 0; i < count; i++)
                                {
                                    max_num = "0" + max_num;
                                }
                                buckle.salesman_code = buckle.agency_code + max_num;

                                phones.Add(buckle.salesman_phone);
                            }
                            else
                            {
                                buckle.review_state = -5; //失败退回到市级未提交状态
                            }
                            buckle.process_result = batch_detail.ErrMsg;
                            buckle.finish_time = batch_detail.FinishTime;
                            db.Update(buckle);
                        }
                    });

                    db.SaveChanges();

                    HttpWebRequest request = null;
                    string url = null;
                    /*phones.ForEach(phone =>
                    {
                        url =
                            "http://10.20.147.103:8080/api/json/reply/sendmsg?username=43010271000014&password=170046&systemid=0&mobile=" +
                            phone + "&content=银行代扣成功&dept=信息技术部&clerk=李轩";

                        request = (HttpWebRequest) HttpWebRequest.Create(url);
                        request.Method = "post";
                        request.GetResponse();
                    });*/
                }
                finally
                {
                    QuartzManager<QueryBuckleInfo>.RemoveJob(batch_id);
                }
            }
        }
    }

    public class QueryGivesInfo : IJob
    {
        private Db db = null;
        private DbInterface db_interface = null;
        private string batch_id = null;

        public void Execute(IJobExecutionContext context)
        {
            db_interface = db_interface ?? new DbInterface();
            batch_id = batch_id ?? context.JobDetail.Key.Name;

            INTERFACE_MIO_BATCH_BZJ batch_interface =
                db_interface.INTERFACE_MIO_BATCH_BZJ.SingleOrDefault(t => t.FromBatchNo == batch_id);
            if (batch_interface != null && batch_interface.BatchStatus == 5)
            {
                try
                {
                    db = new Db();

                    var batch = db.MioBatch.SingleOrDefault(t => t.batch_id == batch_id);
                    batch.done_date = DateTime.Now;
                    batch.push_result = batch_interface.Remark;
                    db.Update(batch);

                    var batch_interface_list =
                        db_interface.INTERFACE_MIO_LIST_BZJ.Where(t => t.FromBatchNo == batch_id).ToList(); //获取清单表

                    Generation_gives gives = null;

                    var batch_list = db.MioList.Where(t => t.batch_id == batch_id).ToList();
                    batch_list.ForEach(t =>
                    {
                        var batch_detail =
                            batch_interface_list.SingleOrDefault(
                                s =>
                                    s.FromBatchNo == batch_id && s.BankAcc == t.bank_account_no &&
                                    s.BankAccName == t.bank_account_name);
                        t.status = batch_detail.MioStatus;
                        t.result = batch_detail.ErrMsg; //将清单表信息复制过来
                        db.Update(t);

                        gives = db.Generation_gives.SingleOrDefault(b => b.id == t.generation_id);
                        if (gives != null)
                        {
                            if (t.status == 0) //0表示处理成功
                            {
                                gives.review_state = 6;
                            }
                            else
                            {
                                gives.review_state = -6; //失败退回到提交状态
                            }
                            gives.process_result = batch_detail.ErrMsg;
                            gives.finish_time = batch_detail.FinishTime;
                            db.Update(gives);
                        }
                    });

                    db.SaveChanges();
                }
                finally
                {
                    QuartzManager<QueryGivesInfo>.RemoveJob(batch_id);
                }
            }
        }
    }
}