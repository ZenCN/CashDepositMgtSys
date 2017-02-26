﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Infrastructure.Exception;
using Infrastructure.Operation;
using Newtonsoft.Json;
using NPOI.OpenXmlFormats.Wordprocessing;
using Service.Model;
using Service.Model.Interface;

namespace Service
{
    public class Generation_givesSvr
    {
        private Db db = null;

        public Result Save(string generation_gives, string deducted_items)
        {
            db = new Db();

            try
            {
                Generation_gives gives = JsonConvert.DeserializeObject<Generation_gives>(generation_gives);

                if (gives.id > 0)
                {
                    Entity.Update(db, gives);
                }
                else
                {
                    gives.record_date = DateTime.Now;
                    db.Generation_gives.Add(gives);
                }

                Entity.SaveChanges(db);

                if (!string.IsNullOrEmpty(deducted_items))
                {
                    List<Deducted> deducteds = JsonConvert.DeserializeObject<List<Deducted>>(deducted_items);
                    deducteds.ForEach(t => { t.generation_gives_id = gives.id; });
                    db.Deducted.AddRange(deducteds);

                    Entity.SaveChanges(db);
                }

                return new Result(ResultType.success, new {generation_gives_id = gives.id});
            }
            catch (Exception ex)
            {
                return new Result(ResultType.error, new Message(ex).ErrorDetails);
            }
        }

        public Result Search(int page_index, int page_size, string salesman_card_id, string salesman_name,
            string user_code, string agency_code, int level)
        {
            db = new Db();

            try
            {
                int page_count = 0;
                int record_count = 0;
                List<Generation_gives> list = null;

                IQueryable<Generation_gives> query = null;
                switch (level)
                {
                    case 2:
                        query = db.Generation_gives;
                        break;
                    case 3:
                        agency_code = agency_code.Substring(0, 4);
                        query = db.Generation_gives.Where(t => t.agency_code.StartsWith(agency_code) && (t.reviewer_code == null || t.reviewer_code == user_code));
                        break;
                    default:
                        query = db.Generation_gives.Where(t => t.agency_code == agency_code && t.recorder_code == user_code);
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

                list = query.ToList();

                record_count = list.Count;
                page_count = ((record_count + page_size) - 1)/page_size;

                list = list.OrderByDescending(t => t.record_date).Skip(page_index*page_size).Take(page_size).ToList();

                return new Result(ResultType.success,
                    new {record_count = record_count, page_count = page_count, list = list});
            }
            catch (Exception ex)
            {
                return new Result(ResultType.error, new Message(ex).ErrorDetails);
            }
        }

        public Result ChangeReviewState(List<int> ids, int state)
        {
            db = new Db();

            try
            {
                var gives = db.Generation_gives.Where(t => ids.Contains(t.id)).ToList();
                gives.ForEach(t => t.review_state = state);

                Entity.SaveChanges(db);

                if (state == 3)  //省级审核通过
                {
                    Generation_buckle buckle = null;
                    List<Generation_gives> list = new List<Generation_gives>();
                    
                    gives.ForEach(g =>
                    {
                        buckle =
                            db.Generation_buckle.SingleOrDefault(
                                b =>
                                    b.salesman_card_id == g.salesman_card_id &&
                                    b.salesman_bank_account_name == g.salesman_bank_account_name);

                        if (buckle != null)
                        {
                            list.Add(g);  //添加
                        }
                    });

                    decimal sum_amount = Convert.ToDecimal(list.Sum(t => t.salesman_refunds));
                    using (DbInterface db_context = new DbInterface())
                    {
                        INTERFACE_MIO_BATCH_BZJ batch = new INTERFACE_MIO_BATCH_BZJ(); ;
                        batch.MioType = "O";
                        batch.DataCnt = list.Count;
                        batch.SumAmnt = sum_amount;
                        batch.GenerateTime = DateTime.Now;
                        batch.GenerateBy = HttpContext.Current.Request.Cookies["user_code"].Value;

                        batch.FromSys = "UnKnow";
                        batch.FromBatchNo = "UnKnow";
                        batch.BatchStatus = 0;

                        db_context.INTERFACE_MIO_BATCH_BZJ.Add(batch);
                        Entity.SaveChanges(db_context);

                        INTERFACE_MIO_LIST_BZJ mio = null;
                        list.ForEach(t =>
                        {
                            mio = new INTERFACE_MIO_LIST_BZJ();
                            mio.ClicBranch = t.agency_code;
                            mio.BatchId = batch.BatchId;
                            mio.ApplTime = DateTime.Now;
                            mio.ProcStatus = "0";
                            mio.AccBookOrCard = "C";
                            mio.AccPersonOrCompany = "P";
                            mio.BankAccName = t.salesman_bank_account_name;
                            mio.BankAcc = t.salesman_bank_account_number;
                            mio.MioAmount = t.salesman_refunds.Value;

                            mio.FromSys = "UnKnow";
                            mio.FromBatchNo = "UnKnow";
                            mio.FromUniqLine = "UnKnow";
                            mio.BankCode = "UnKnow";
                            mio.MioStatus = -1;
                            mio.AccCurrencyType = "CNY";

                            db_context.INTERFACE_MIO_LIST_BZJ.Add(mio);
                        });

                        Entity.SaveChanges(db_context);
                    }
                }

                return new Result(ResultType.success);
            }
            catch (Exception ex)
            {
                return new Result(ResultType.error, new Message(ex).ErrorDetails);
            }
        }

        public Result Import(List<Generation_gives> list)
        {
            db = new Db();
            Generation_gives gives = null;
            List<Generation_gives> ignore = new List<Generation_gives>();

            try
            {
                foreach (Generation_gives _this in list)
                {
                    gives =
                        db.Generation_gives.SingleOrDefault(
                            t =>
                                t.salesman_card_id == _this.salesman_card_id &&
                                t.salesman_hiredate == _this.salesman_hiredate);

                    if (gives == null)
                    {
                        _this.record_date = DateTime.Now;
                        db.Generation_gives.Add(_this);
                    }
                    else
                    {
                        ignore.Add(_this);
                    }
                }

                if (list.Count > ignore.Count)
                    Entity.SaveChanges(db);

                if (ignore.Count > 0)
                    return new Result(ResultType.success, "但部分人员信息因已存在已被忽略导入，详见表格", ignore);
                else
                    return new Result(ResultType.success, "人员信息全部成功导入");
            }
            catch (Exception ex)
            {
                return new Result(ResultType.error, new Message(ex).ErrorDetails);
            }
        }
    }
}