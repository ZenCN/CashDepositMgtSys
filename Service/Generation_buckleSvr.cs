﻿using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Transactions;
using System.Web;
using Infrastructure.Exception;
using Infrastructure.Helper;
using Infrastructure.Operation;
using Quartz;
using Service.Model;
using Service.Model.Interface;

namespace Service
{
    public class Generation_buckleSvr : BaseSvr
    {
        public Result Delete(List<int> ids)
        {
            db = new Db();

            try
            {
                var buckle = db.Generation_buckle.Where(t => ids.Contains(t.id)).ToList();
                buckle.ForEach(t => t.is_deleted = 1);

                db.SaveChanges();

                return new Result(ResultType.success);
            }
            catch (Exception ex)
            {
                return new Result(ResultType.error, new Message(ex).ErrorDetails);
            }
        }

        public Result QuerySchedule(int page_index, int page_size, string agency_code, string channel,
            DateTime apply_start, DateTime apply_end, string user_jurisdiction)
        {
            db = new Db();

            try
            {
                var query = db.Generation_buckle.Where(t =>
                    t.channel == channel && t.review_state == 5 &&
                    t.record_date >= apply_start &&
                    t.record_date <= apply_end).AsQueryable();

                if (!string.IsNullOrEmpty(agency_code))
                {
                    if (agency_code.Substring(4) == "00") //市级
                    {
                        string tmp = agency_code.Substring(0, 4);
                        query = query.Where(t => t.agency_code.StartsWith(tmp));
                    }
                    else
                    {
                        query = query.Where(t => t.agency_code == agency_code);
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(user_jurisdiction))
                    {
                        var expression = PredicateBuilder.False<Generation_buckle>();
                        var arr = user_jurisdiction.Split(new string[] { "、" },
                            StringSplitOptions.RemoveEmptyEntries);
                        for (int i = 0; i < arr.Length; i++)
                        {

                            string city_code = arr[i].Substring(0, 4);
                            expression = expression.Or(t => t.agency_code.StartsWith(city_code));
                        }

                        query = query.Where(expression.Compile()).AsQueryable();
                    }
                }

                var list = query.ToList();

                int page_count = 0;
                int record_count = 0;
                record_count = list.Count;
                page_count = ((record_count + page_size) - 1) / page_size;

                list =
                    list.OrderByDescending(t => t.salesman_hiredate).Skip(page_index * page_size).Take(page_size).ToList();

                return new Result(ResultType.success, new
                {
                    record_count = record_count,
                    page_count = page_count,
                    list = list
                });
            }
            catch (Exception ex)
            {
                return new Result(ResultType.error, new Message(ex).ErrorDetails);
            }
        }

        public void ExportSchedule(int page_index, int page_size, string agency_code, string channel,
            DateTime apply_start, DateTime apply_end, string user_jurisdiction)
        {
            db = new Db();

            try
            {
                var query = db.Generation_buckle.Where(t =>
                    t.channel == channel && t.review_state == 5 &&
                    t.record_date >= apply_start &&
                    t.record_date <= apply_end).AsQueryable();

                if (!string.IsNullOrEmpty(agency_code))
                {
                    if (agency_code.Substring(4) == "00") //市级
                    {
                        string tmp = agency_code.Substring(0, 4);
                        query = query.Where(t => t.agency_code.StartsWith(tmp));
                    }
                    else
                    {
                        query = query.Where(t => t.agency_code == agency_code);
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(user_jurisdiction))
                    {
                        var expression = PredicateBuilder.False<Generation_buckle>();
                        var str_arr = user_jurisdiction.Split(new string[] { "、" },
                            StringSplitOptions.RemoveEmptyEntries);
                        for (int i = 0; i < str_arr.Length; i++)
                        {

                            string city_code = str_arr[i].Substring(0, 4);
                            expression = expression.Or(t => t.agency_code.StartsWith(city_code));
                        }

                        query = query.Where(expression.Compile()).AsQueryable();
                    }
                }

                var list = query.ToList();

                list =
                    list.OrderByDescending(t => t.salesman_hiredate).Skip(page_index * page_size).Take(page_size).ToList();

                List<string> p_codes = new List<string>();
                string[] arr = user_jurisdiction.Split(new string[] {"、"}, StringSplitOptions.RemoveEmptyEntries);
                p_codes.AddRange(arr);
                var agency = db.Agency.Where(t => p_codes.Contains(t.p_code) || p_codes.Contains(t.code)).ToList();

                new Excel().ExportSchedule(list, agency);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Result SumDetails(int page_index, int page_size, string agency_code, string channel, DateTime apply_start,
            DateTime apply_end, string user_jurisdiction)
        {
            db = new Db();

            try
            {
                var query = db.Generation_buckle.Where(t =>
                    t.channel == channel && t.review_state == 5 &&
                    t.record_date >= apply_start &&
                    t.record_date <= apply_end).AsQueryable();

                if (!string.IsNullOrEmpty(agency_code))
                {
                    if (agency_code.Substring(4) == "00") //市级
                    {
                        string tmp = agency_code.Substring(0, 4);
                        query = query.Where(t => t.agency_code.StartsWith(tmp));
                    }
                    else
                    {
                        query = query.Where(t => t.agency_code == agency_code);
                    }
                }
                else
                {
                    agency_code = "430000";

                    if (!string.IsNullOrEmpty(user_jurisdiction))
                    {
                        var expression = PredicateBuilder.False<Generation_buckle>();
                        var arr = user_jurisdiction.Split(new string[] {"、"},
                            StringSplitOptions.RemoveEmptyEntries);
                        for (int i = 0; i < arr.Length; i++)
                        {
                            
                            string city_code = arr[i].Substring(0, 4);
                            expression = expression.Or(t => t.agency_code.StartsWith(city_code));
                        }

                        query = query.Where(expression.Compile()).AsQueryable();
                    }
                }

                var list = query.ToList();

                int page_count = 0;
                int record_count = 0;
                record_count = list.Count;
                page_count = ((record_count + page_size) - 1)/page_size;

                list =
                    list.OrderByDescending(t => t.salesman_hiredate).Skip(page_index*page_size).Take(page_size).ToList();

                return new Result(ResultType.success, new
                {
                    record_count = record_count,
                    page_count = page_count,
                    sum = new
                    {
                        apply_start = apply_start.ToString("yyyy-MM-dd"),
                        apply_end = apply_end.ToString("yyyy-MM-dd"),
                        agency_code = agency_code,
                        item = "代收",
                        count = list.Count,
                        amount = list.Sum(t => t.salesman_cash_deposit),
                        channel = channel
                    },
                    details = list
                });
            }
            catch (Exception ex)
            {
                return new Result(ResultType.error, new Message(ex).ErrorDetails);
            }
        }

        public Result Save(Generation_buckle buckle)
        {
            db = new Db();

            try
            {
                var old_buckle =
                    db.Generation_buckle.SingleOrDefault(
                        t =>
                            t.salesman_card_id == buckle.salesman_card_id &&
                            t.salesman_hiredate == buckle.salesman_hiredate);

                if (old_buckle == null || buckle.id > 0)
                {
                    if (buckle.id > 0)
                    {
                        Entity.Update(db, buckle);
                    }
                    else
                    {
                        db.Generation_buckle.Add(buckle);
                    }

                    Entity.SaveChanges(db);

                    return new Result(ResultType.success, new {generation_buckle_id = buckle.id});
                }
                else
                {
                    return new Result(ResultType.no_changed, "该人员的代扣信息已被其它工作人员录入");
                }
            }
            catch (Exception ex)
            {
                return new Result(ResultType.error, new Message(ex).ErrorDetails);
            }
        }

        public Result Search(int page_index, int page_size, string salesman_card_id, string salesman_name,
            string review_state, DateTime apply_start, DateTime apply_end,
            string user_code, string agency_code, int level, string user_role)
        {
            db = new Db();
            List<Generation_buckle> list = null;

            int page_count = 0;
            int record_count = 0;

            IQueryable<Generation_buckle> query =
                db.Generation_buckle.Where(
                    t => t.is_deleted != 1 && t.record_date >= apply_start &&
                        t.record_date <= apply_end);
            switch (level)
            {
                case 2:
                    query = query.Where(t => t.review_state >= 2 || t.review_state == -5);
                    break;
                case 3:
                    string code = agency_code.Substring(0, 4);
                    if (user_role == "leader")
                    {
                        query =
                            query.Where(
                                t =>
                                    t.agency_code.StartsWith(code) &&
                                    t.review_state != 0 && t.review_state != -2);
                    }
                    else  //worker
                    {
                        query =
                        query.Where(
                            t =>
                                t.agency_code.StartsWith(code));
                    }
                    break;
                case 4:
                    query =
                        query.Where(
                            t =>
                                t.agency_code == agency_code);
                    break;
            }

            if (!string.IsNullOrEmpty(salesman_card_id))
            {
                query = query.Where(t => t.salesman_card_id.Contains(salesman_card_id));
            }

            if (!string.IsNullOrEmpty(salesman_name))
            {
                query = query.Where(t => t.salesman_name.Contains(salesman_name));
            }

            if (!string.IsNullOrEmpty(review_state))
            {
                if (review_state == "normal")
                {
                    query = query.Where(t => t.review_state != 5 && t.review_state != -5);
                }
                else
                {
                    int state = int.Parse(review_state);
                    query = query.Where(t => t.review_state == state);
                }
            }

            list = query.ToList();

            record_count = list.Count;
            page_count = ((record_count + page_size) - 1)/page_size;

            list = list.OrderBy(t => t.salesman_hiredate).Skip(page_index*page_size).Take(page_size).ToList();

            return new Result(ResultType.success,
                new {record_count = record_count, page_count = page_count, list = list});
        }

        public Result Import(List<Generation_buckle> list)
        {
            db = new Db();
            Generation_buckle buckle = null;
            List<Generation_buckle> ignore = new List<Generation_buckle>();

            try
            {
                foreach (Generation_buckle _this in list)
                {
                    buckle =
                        db.Generation_buckle.SingleOrDefault(
                            t =>
                                t.salesman_card_id == _this.salesman_card_id &&
                                t.salesman_hiredate == _this.salesman_hiredate && t.is_deleted != 1);

                    if (buckle == null)
                    {
                        _this.record_date = DateTime.Now;
                        db.Generation_buckle.Add(_this);
                    }
                    else
                    {
                        ignore.Add(_this);
                    }
                }

                if (list.Count > ignore.Count)
                    Entity.SaveChanges(db);

                if (ignore.Count > 0)
                    return new Result(ResultType.success,
                        "成功导入" + (list.Count - ignore.Count) + "条，部分人员信息已存在已被忽略导入，详见表格", ignore);
                else
                    return new Result(ResultType.success, "成功导入" + list.Count + "条");
            }
            catch (Exception ex)
            {
                return new Result(ResultType.error, new Message(ex).ErrorDetails);
            }
        }

        public void ExportSumDetails(int page_index, int page_size, string agency_code, string channel,
            DateTime apply_start, DateTime apply_end, string user_jurisdiction)
        {
            db = new Db();

            try
            {
                var query = db.Generation_buckle.Where(t =>
                    t.channel == channel && t.review_state == 5 &&
                    t.record_date >= apply_start &&
                    t.record_date <= apply_end).AsQueryable();

                if (!string.IsNullOrEmpty(agency_code))
                {
                    if (agency_code.Substring(4) == "00") //市级
                    {
                        string tmp = agency_code.Substring(0, 4);
                        query = query.Where(t => t.agency_code.StartsWith(tmp));
                    }
                    else
                    {
                        query = query.Where(t => t.agency_code == agency_code);
                    }
                }
                else
                {
                    agency_code = "430000";

                    if (!string.IsNullOrEmpty(user_jurisdiction))
                    {
                        var expression = PredicateBuilder.False<Generation_buckle>();
                        var arr = user_jurisdiction.Split(new string[] { "、" },
                            StringSplitOptions.RemoveEmptyEntries);
                        for (int i = 0; i < arr.Length; i++)
                        {

                            string city_code = arr[i].Substring(0, 4);
                            expression = expression.Or(t => t.agency_code.StartsWith(city_code));
                        }

                        query = query.Where(expression.Compile()).AsQueryable();
                    }
                }

                var list = query.ToList();

                int page_count = 0;
                int record_count = 0;
                record_count = list.Count;
                page_count = ((record_count + page_size) - 1)/page_size;

                list =
                    list.OrderByDescending(t => t.salesman_hiredate).Skip(page_index*page_size).Take(page_size).ToList();

                Sum sum = new Sum()
                {
                    apply_start = apply_start.ToString("yyyy-MM-dd"),
                    apply_end = apply_end.ToString("yyyy-MM-dd"),
                    agency_code = agency_code,
                    item = "代收",
                    count = list.Count,
                    amount = list.Sum(t => t.salesman_cash_deposit),
                    channel = channel
                };

                new Excel().ExportSumDetails(sum, list);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Export(int page_index, int page_size, string salesman_card_id, string salesman_name,
            string review_state, DateTime apply_start, DateTime apply_end,
            string user_code, string agency_code, int level, string user_role)
        {
            db = new Db();
            List<Generation_buckle> list = null;

            int page_count = 0;
            int record_count = 0;

            IQueryable<Generation_buckle> query =
                db.Generation_buckle.Where(
                    t => t.is_deleted != 1 &&
                        t.record_date >= apply_start && t.record_date <= apply_end);
            switch (level)
            {
                case 2:
                    query = query.Where(t => t.review_state >= 2 || t.review_state == -5);
                    break;
                case 3:
                    string code = agency_code.Substring(0, 4);
                    if (user_role == "leader")
                    {
                        query =
                            query.Where(
                                t =>
                                    t.agency_code.StartsWith(code) &&
                                    t.review_state != 0 && t.review_state != -2);
                    }
                    else  //worker
                    {
                        query =
                        query.Where(
                            t =>
                                t.agency_code == agency_code);
                    }
                    break;
                case 4:
                    query =
                        query.Where(
                            t =>
                                t.agency_code == agency_code);
                    break;
            }

            if (!string.IsNullOrEmpty(salesman_card_id))
            {
                query = query.Where(t => t.salesman_card_id.Contains(salesman_card_id));
            }

            if (!string.IsNullOrEmpty(salesman_name))
            {
                query = query.Where(t => t.salesman_name.Contains(salesman_name));
            }

            if (!string.IsNullOrEmpty(review_state))
            {
                if (review_state == "normal")
                {
                    query = query.Where(t => t.review_state != 5 && t.review_state != -5);
                }
                else
                {
                    int state = int.Parse(review_state);
                    query = query.Where(t => t.review_state == state);
                }
            }

            list = query.ToList();

            record_count = list.Count;
            page_count = ((record_count + page_size) - 1)/page_size;

            list = list.OrderBy(t => t.salesman_hiredate).Skip(page_index*page_size).Take(page_size).ToList();

            new Excel().Export(list);
        }

        public Result ChangeReviewState(List<int> ids, int state)
        {
            db = new Db();

            try
            {
                List<Generation_buckle> buckle_list = db.Generation_buckle.Where(t => ids.Contains(t.id)).ToList();
                buckle_list.ForEach(t => t.review_state = state);

                db.SaveChanges();

                if (state == 4) //省级推送
                {
                    if (buckle_list.Count > 0)
                    {
                        //using (var buckle_scope = new TransactionScope())
                        {
                            decimal sum_amount = Convert.ToDecimal(buckle_list.Sum(t => t.salesman_cash_deposit));

                            MioBatch mio_batch = new MioBatch(); //写入本数据库中的 收付批次表
                            mio_batch.batch_id = DateTime.Now.Ticks.ToString(); //批次号
                            mio_batch.record_count = buckle_list.Count; //批次中包含的收付笔数
                            mio_batch.sum_amount = sum_amount; //批次总金额
                            mio_batch.reviewer_code = HttpContext.Current.Request.Cookies["user_code"].Value; //审核人工号
                            mio_batch.review_date = DateTime.Now; //审核日期
                            mio_batch.push_date = DateTime.Now; //推送日期
                            mio_batch.mio_type = "I"; //收付类型 I收、O付

                            db.MioBatch.Add(mio_batch);

                            List<MioList> mio_list = new List<MioList>(); //写入本数据库中的 收付明细表
                            buckle_list.ForEach(
                                t =>
                                    mio_list.Add(new MioList()
                                    {
                                        batch_id = mio_batch.batch_id,
                                        generation_id = t.id,
                                        mio_type = "代扣",
                                        bank_account_no = t.salesman_bank_account_number,
                                        bank_account_name = t.salesman_bank_account_name,
                                        cash_deposit = t.salesman_cash_deposit,
                                        status = -1,
                                        result = "代扣处理中..."
                                    }));

                            db.MioList.AddRange(mio_list);

                            //using (var gives_scope = new TransactionScope())
                            {
                                DbInterface db_context = new DbInterface();

                                INTERFACE_MIO_BATCH_BZJ batch = new INTERFACE_MIO_BATCH_BZJ(); //写入保证金收付接口表——批次表
                                batch.MioType = "I"; //收付类型 I收、O付
                                batch.DataCnt = buckle_list.Count; //批次中包含的收付笔数
                                batch.SumAmnt = sum_amount; //批次总金额
                                batch.GenerateTime = DateTime.Now; //批次生成的时间
                                batch.GenerateBy = HttpContext.Current.Request.Cookies["user_code"].Value;
                                //产生数据人员，八位ERP工号
                                batch.FromBatchNo = mio_batch.batch_id; //外部系统批次号
                                batch.BatchStatus = 0; //批次状态（默认为0）

                                batch.FromSys = "UnKnow"; //外部系统编号

                                db_context.INTERFACE_MIO_BATCH_BZJ.Add(batch);
                                db_context.SaveChanges();

                                INTERFACE_MIO_LIST_BZJ mio = null;
                                buckle_list.ForEach(t =>
                                {
                                    mio = new INTERFACE_MIO_LIST_BZJ();
                                    mio.ClicBranch = t.agency_code; //待收付数据的机构(与商户号相关)
                                    mio.BatchId = batch.BatchId; //接口批次表生成的id
                                    mio.ApplTime = DateTime.Now; //审核时间
                                    mio.ProcStatus = "0"; //数据检查结果，0-新数据待检查，1检验通过,2审核通过
                                    mio.AccBookOrCard = "C"; //帐号类型(C银行卡，B存折)
                                    mio.AccPersonOrCompany = "P"; //P私人，C公司。不填时，默认为私人
                                    mio.BankAccName = t.salesman_bank_account_name; //银行户名
                                    mio.BankAcc = t.salesman_bank_account_number; //银行账号
                                    mio.MioAmount = t.salesman_cash_deposit.Value; //交易金额
                                    mio.FromSys = "UnKnow"; //外部系统编号
                                    mio.FromBatchNo = batch.FromBatchNo; //外部系统批次号
                                    mio.MioStatus = -1; //收付结果。成功、余额不足、户名错、账户冻结等，需字典表
                                    mio.AccCurrencyType = "CNY"; //人民币：CNY, 港元：HKD，美元：USD。不填时，默认为人民币。

                                    mio.FromUniqLine = "UnKnow"; //外部系统对于本条数据的唯一编码
                                    mio.BankCode = t.salesman_bank_name; //中国人寿编码的银行代码，需转换为银联代码

                                    db_context.INTERFACE_MIO_LIST_BZJ.Add(mio);
                                });

                                db_context.SaveChanges();
                                //gives_scope.Complete();
                            }

                            db.SaveChanges();
                            //buckle_scope.Complete();

                            QuartzManager<QueryBuckleInfo>.AddJob(mio_batch.batch_id, "0 1/1 * * * ?");
                            //1分钟之后执行第一次（对应“1/1”第一个1）,然后每隔1分钟执行一次（对应“1/1”第二个1）
                        }
                    }
                }

                return new Result(ResultType.success);
            }
            catch (DbEntityValidationException ex)
            {
                return new Result(ResultType.error, new Message(ex).ErrorDetails);
            }
        }
    }
}