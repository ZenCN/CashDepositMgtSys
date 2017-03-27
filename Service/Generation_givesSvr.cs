using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web;
using Infrastructure.Exception;
using Infrastructure.Operation;
using Newtonsoft.Json;
using NPOI.OpenXmlFormats.Wordprocessing;
using Service.Model;
using Service.Model.Interface;
using System.Transactions;
using Infrastructure.Helper;

namespace Service
{
    public class Generation_givesSvr : BaseSvr
    {
        public Result Delete(List<int> ids)
        {
            db = new Db();

            try
            {
                var gives = db.Generation_gives.Where(t => ids.Contains(t.id)).ToList();
                gives.ForEach(t => t.is_deleted = 1);

                Entity.SaveChanges(db);

                return new Result(ResultType.success);
            }
            catch (Exception ex)
            {
                return new Result(ResultType.error, new Message(ex).ErrorDetails);
            }
        }

        public Result QuerySchedule(int page_index, int page_size, string agency_code, string channel, DateTime apply_start, DateTime apply_end)
        {
            db = new Db();

            try
            {
                var query = db.Generation_gives.Where(t =>
                    t.channel == channel && t.review_state == 6 &&
                    t.salesman_hiredate >= apply_start &&
                    t.salesman_hiredate <= apply_end).AsQueryable();

                if (!string.IsNullOrEmpty(agency_code))
                {
                    agency_code = agency_code.Substring(0, 4);
                    query = query.Where(t => t.agency_code.StartsWith(agency_code));
                }
                else
                {
                    agency_code = "4300";
                }

                var list = query.ToList();

                int page_count = 0;
                int record_count = 0;
                record_count = list.Count;
                page_count = ((record_count + page_size) - 1) / page_size;

                list = list.OrderByDescending(t => t.salesman_hiredate).Skip(page_index * page_size).Take(page_size).ToList();

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

        public void ExportSchedule(int page_index, int page_size, string agency_code, string channel, DateTime apply_start, DateTime apply_end, string user_jurisdiction)
        {
            db = new Db();

            try
            {
                var query = db.Generation_gives.Where(t =>
                    t.channel == channel && t.review_state == 6 &&
                    t.salesman_hiredate >= apply_start &&
                    t.salesman_hiredate <= apply_end).AsQueryable();

                if (!string.IsNullOrEmpty(agency_code))
                {
                    agency_code = agency_code.Substring(0, 4);
                    query = query.Where(t => t.agency_code.StartsWith(agency_code));
                }
                else
                {
                    agency_code = "4300";
                }

                var list = query.ToList();

                int page_count = 0;
                int record_count = 0;
                record_count = list.Count;
                page_count = ((record_count + page_size) - 1) / page_size;

                list = list.OrderByDescending(t => t.salesman_hiredate).Skip(page_index * page_size).Take(page_size).ToList();

                List<string> p_codes = new List<string>();
                string[] arr = HttpUtility.UrlDecode(user_jurisdiction).Split(new string[] { "、" }, StringSplitOptions.RemoveEmptyEntries);
                p_codes.AddRange(arr);
                var agency = db.Agency.Where(t => p_codes.Contains(t.p_code) || p_codes.Contains(t.code)).ToList();

                new Excel().ExportSchedule(list, agency);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Result SumDetails(int page_index, int page_size, string agency_code, string channel, DateTime apply_start, DateTime apply_end)
        {
            db = new Db();

            try
            {
                var query = db.Generation_gives.Where(t =>
                    t.channel == channel && t.review_state == 6 &&
                    t.salesman_hiredate >= apply_start &&
                    t.salesman_hiredate <= apply_end).AsQueryable();

                if (!string.IsNullOrEmpty(agency_code))
                {
                    agency_code = agency_code.Substring(0, 4);
                    query = query.Where(t => t.agency_code.StartsWith(agency_code));
                }
                else
                {
                    agency_code = "4300";
                }

                var list = query.ToList();

                int page_count = 0;
                int record_count = 0;
                record_count = list.Count;
                page_count = ((record_count + page_size) - 1) / page_size;

                list = list.OrderByDescending(t => t.salesman_hiredate).Skip(page_index * page_size).Take(page_size).ToList();

                return new Result(ResultType.success, new
                {
                    record_count = record_count,
                    page_count = page_count,
                    sum = new
                    {
                        apply_start = apply_start.ToString("yyyy-MM-dd"),
                        apply_end = apply_end.ToString("yyyy-MM-dd"),
                        agency_code = agency_code + "00",
                        item = "代收",
                        count = list.Count,
                        amount = list.Sum(t => t.salesman_refunds),
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

        public void ExportSumDetails(int page_index, int page_size, string agency_code, string channel,
            DateTime apply_start, DateTime apply_end)
        {
            db = new Db();

            try
            {
                var query = db.Generation_gives.Where(t =>
                    t.channel == channel && t.review_state == 6 &&
                    t.salesman_hiredate >= apply_start &&
                    t.salesman_hiredate <= apply_end).AsQueryable();

                if (!string.IsNullOrEmpty(agency_code))
                {
                    agency_code = agency_code.Substring(0, 4);
                    query = query.Where(t => t.agency_code.StartsWith(agency_code));
                }
                else
                {
                    agency_code = "4300";
                }

                var list = query.ToList();

                int page_count = 0;
                int record_count = 0;
                record_count = list.Count;
                page_count = ((record_count + page_size) - 1) / page_size;

                list = list.OrderByDescending(t => t.salesman_hiredate).Skip(page_index * page_size).Take(page_size).ToList();

                Sum sum = new Sum()
                {
                    apply_start = apply_start.ToString("yyyy-MM-dd"),
                    apply_end = apply_end.ToString("yyyy-MM-dd"),
                    agency_code = agency_code + "00",
                    item = "代收",
                    count = list.Count,
                    amount = list.Sum(t => t.salesman_refunds),
                    channel = channel
                };

                new Excel().ExportSumDetails(sum, list);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Result Save(string generation_gives, string deducted_items, int level)
        {
            db = new Db();

            try
            {
                Generation_gives gives = JsonConvert.DeserializeObject<Generation_gives>(generation_gives);

                var old_gives =
                    db.Generation_gives.SingleOrDefault(
                        t =>
                            t.salesman_card_id == gives.salesman_card_id &&
                            t.salesman_hiredate == gives.salesman_hiredate);

                if (old_gives == null || gives.id > 0)
                {
                    if (gives.id > 0)
                    {
                        Entity.Update(db, gives);

                        var old_deducteds = db.Deducted.Where(t => t.generation_gives_id == gives.id).ToList();
                        if (old_deducteds.Any())
                        {
                            db.Deducted.RemoveRange(old_deducteds);
                        }
                    }
                    else
                    {
                        gives.record_date = DateTime.Now;

                        switch (level)
                        {
                            case 2:
                                gives.review_state = 2;
                                break;
                            case 3:
                                gives.review_state = 1;
                                break;
                            case 4:
                                gives.review_state = 0;
                                break;
                        }

                        db.Generation_gives.Add(gives);
                    }

                    Entity.SaveChanges(db);

                    if (!string.IsNullOrEmpty(deducted_items))
                    {
                        List<Deducted> deducteds = JsonConvert.DeserializeObject<List<Deducted>>(deducted_items);
                        deducteds.ForEach(t =>
                        {
                            t.id = 0;
                            t.generation_gives_id = gives.id;
                        });
                        db.Deducted.AddRange(deducteds);

                        Entity.SaveChanges(db);
                    }

                    return new Result(ResultType.success, new {generation_gives_id = gives.id});
                }
                else
                {
                    return new Result(ResultType.no_changed, "该人员的代付信息已被其它工作人员录入");
                }
            }
            catch (Exception ex)
            {
                return new Result(ResultType.error, new Message(ex).ErrorDetails);
            }
        }

        public void Export(int page_index, int page_size, string salesman_card_id, string salesman_name, string salesman_code, string review_state,
            string user_code, string agency_code, int level)
        {
            db = new Db();

            int page_count = 0;
            int record_count = 0;
            List<Generation_gives> list = null;

            var query = db.Generation_gives.Where(t => t.is_deleted != 1).AsQueryable();
            switch (level)
            {
                case 2:
                    var staff = db.Staff.SingleOrDefault(t => t.code == user_code);

                    if (staff.jurisdiction != null)
                    {
                        string[] area_codes = staff.jurisdiction.Split(new string[] { "、" },
                            StringSplitOptions.RemoveEmptyEntries);

                        //如果你是要创建一个OR组成的Predicate就不能把它初始化为True因为这样这个表达试永远为True了，所以只能设置为 False !
                        var expression = PredicateBuilder.False<Generation_gives>();
                        for (int i = 0; i < area_codes.Length; i++)
                        {
                            area_codes[i] = area_codes[i].Substring(0, 4);
                            //特别注意：expression必须要有“独立”的city_code变量，类似于前端“闭包回调”，这样的话“闭包回调”时用到的city_code变量的值都不一样
                            //如果在expression变量的作用域之外申明变量city_code，那么所有expression所用到的city_code都一样，因为对于expression变量而言city_code是“全局变量”
                            //如果在这个方法里city_code变量申明在expression变量的作用域之外，那么所有的expression的city_code值都是area_code数组最后一个值
                            string city_code = area_codes[i];
                            //以下额外用一个city_code变量，而不是直接用area_codes[i],原因同上，"i"对于expression而言是“全局变量”
                            expression = expression.Or(t => t.agency_code.StartsWith(city_code));
                        }

                        //expression使用使用时必须调用Compile()返回IEnumerable类型的对象，如果接收的变量时IQueryable，则必须调用AsQueryable()进行转换
                        query = query.Where(expression.Compile()).AsQueryable();
                    }

                    switch (staff.role)
                    {
                        case "financial":
                            query = query.Where(t => t.review_state >= 4 || t.review_state == -6);
                            break;
                        case "accountant":
                            if (staff.authority == 1) //会计复审
                            {
                                query =
                                    query.Where(
                                        t => t.review_state >= 3 || t.review_state == -6);
                            }
                            else //会计初审
                            {
                                query =
                                    query.Where(
                                        t => t.review_state >= 2 || t.review_state == -4 || t.review_state == -6);
                            }
                            break;
                    }
                    break;
                case 3:
                    agency_code = agency_code.Substring(0, 4);
                    query =
                        query.Where(
                            t =>
                                t.agency_code.StartsWith(agency_code) &&
                                (t.reviewer_code == null || t.reviewer_code == user_code) &&
                                t.review_state != 0 && t.review_state != -2);
                    break;
                case 4:
                    query =
                        query.Where(
                            t =>
                                t.agency_code == agency_code && t.recorder_code == user_code);
                    break;
            }

            if (!string.IsNullOrEmpty(salesman_card_id))
            {
                query = query.Where(t => t.salesman_card_id.StartsWith(salesman_card_id));
            }

            if (!string.IsNullOrEmpty(salesman_name))
            {
                query = query.Where(t => t.salesman_name.Contains(salesman_name));
            }

            if (!string.IsNullOrEmpty(salesman_code))
            {
                query = query.Where(t => t.salesman_code.StartsWith(salesman_code));
            }

            if (!string.IsNullOrEmpty(review_state))
            {
                int state = int.Parse(review_state);
                query = query.Where(t => t.review_state == state);
            }

            list = query.ToList();

            record_count = list.Count;
            page_count = ((record_count + page_size) - 1)/page_size;

            list = list.OrderByDescending(t => t.record_date).Skip(page_index*page_size).Take(page_size).ToList();

            new Excel().Export(list);
        }

        public Result GetDeducteds(int id)
        {
            db = new Db();
            var deducteds = db.Deducted.Where(t => t.generation_gives_id == id).ToList();

            if (deducteds.Any())
            {
                return new Result(ResultType.success, new {list = deducteds});
            }
            else
            {
                return new Result(ResultType.query_nothing);
            }
        }

        public Result Search(int page_index, int page_size, string salesman_card_id, string salesman_name, string salesman_code, string review_state,
            string user_code, string agency_code, int level)
        {
            db = new Db();

            try
            {
                int page_count = 0;
                int record_count = 0;
                List<Generation_gives> list = null;

                var query = db.Generation_gives.Where(t => t.is_deleted != 1).AsQueryable();
                switch (level)
                {
                    case 2:
                        var staff = db.Staff.SingleOrDefault(t => t.code == user_code);

                        if (staff.jurisdiction != null)
                        {
                            string[] area_codes = staff.jurisdiction.Split(new string[] {"、"},
                                StringSplitOptions.RemoveEmptyEntries);

                            //如果你是要创建一个OR组成的Predicate就不能把它初始化为True因为这样这个表达试永远为True了，所以只能设置为 False !
                            var expression = PredicateBuilder.False<Generation_gives>();
                            for (int i = 0; i < area_codes.Length; i++)
                            {
                                area_codes[i] = area_codes[i].Substring(0, 4);
                                //特别注意：expression必须要有“独立”的city_code变量，类似于前端“闭包回调”，这样的话“闭包回调”时用到的city_code变量的值都不一样
                                //如果在expression变量的作用域之外申明变量city_code，那么所有expression所用到的city_code都一样，因为对于expression变量而言city_code是“全局变量”
                                //如果在这个方法里city_code变量申明在expression变量的作用域之外，那么所有的expression的city_code值都是area_code数组最后一个值
                                string city_code = area_codes[i];
                                //以下额外用一个city_code变量，而不是直接用area_codes[i],原因同上，"i"对于expression而言是“全局变量”
                                expression = expression.Or(t => t.agency_code.StartsWith(city_code));
                            }

                            //expression使用使用时必须调用Compile()返回IEnumerable类型的对象，如果接收的变量时IQueryable，则必须调用AsQueryable()进行转换
                            query = query.Where(expression.Compile()).AsQueryable();
                        }

                        switch (staff.role)
                        {
                            case "financial":
                                query = query.Where(t => t.review_state >= 4 || t.review_state == -6);
                                break;
                            case "accountant":
                                if (staff.authority == 1) //会计复审
                                {
                                    query =
                                        query.Where(
                                            t => t.review_state >= 3 || t.review_state == -6);
                                }
                                else //会计初审
                                {
                                    query =
                                        query.Where(
                                            t => t.review_state >= 2 || t.review_state == -4 || t.review_state == -6);
                                }
                                break;
                        }
                        break;
                    case 3:
                        agency_code = agency_code.Substring(0, 4);
                        query =
                            query.Where(
                                t =>
                                    t.agency_code.StartsWith(agency_code) &&
                                    (t.reviewer_code == null || t.reviewer_code == user_code) &&
                                    t.review_state != 0 && t.review_state != -2);
                        break;
                    case 4:
                        query =
                            query.Where(
                                t =>
                                    t.agency_code == agency_code && t.recorder_code == user_code);
                        break;
                }

                if (!string.IsNullOrEmpty(salesman_card_id))
                {
                    query = query.Where(t => t.salesman_card_id.StartsWith(salesman_card_id));
                }

                if (!string.IsNullOrEmpty(salesman_name))
                {
                    query = query.Where(t => t.salesman_name.Contains(salesman_name));
                }

                if (!string.IsNullOrEmpty(salesman_code))
                {
                    query = query.Where(t => t.salesman_code.StartsWith(salesman_code));
                }

                if (!string.IsNullOrEmpty(review_state))
                {
                    int state = int.Parse(review_state);
                    query = query.Where(t => t.review_state == state);
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

                Generation_buckle buckle_success = null;
                gives.ForEach(t =>
                {
                    t.review_state = state;

                    if (state == 2) //市级审核通过后，判断是否为上线后的数据，如果是则直接提交到省财务
                    {
                        buckle_success = db.Generation_buckle.SingleOrDefault(b => b.salesman_card_id == t.salesman_card_id 
                        && b.salesman_hiredate == t.salesman_hiredate && b.review_state == 5); //代扣成功的

                        if (buckle_success != null)
                        {
                            t.review_state = 4;  //上线后的直接推送到省财务,等待财务推送
                        }
                    }
                });

                Entity.SaveChanges(db);

                if (state == 5) //省财务推送
                {
                    Push(gives);
                }

                return new Result(ResultType.success);
            }
            catch (Exception ex)
            {
                return new Result(ResultType.error, new Message(ex).ErrorDetails);
            }
        }

        private void Push(List<Generation_gives> list)
        {
            if (list.Count > 0)
            {
                using (var scope = new TransactionScope())
                {
                    decimal sum_amount = Convert.ToDecimal(list.Sum(t => t.salesman_cash_deposit));

                    MioBatch mio_batch = new MioBatch(); //写入本数据库中的 收付批次表
                    mio_batch.batch_id = DateTime.Now.Ticks.ToString(); //批次号
                    mio_batch.record_count = list.Count; //批次中包含的收付笔数
                    mio_batch.sum_amount = sum_amount; //批次总金额
                    mio_batch.reviewer_code = HttpContext.Current.Request.Cookies["user_code"].Value; //审核人工号
                    mio_batch.review_date = DateTime.Now; //审核日期
                    mio_batch.push_date = DateTime.Now; //推送日期
                    mio_batch.mio_type = "O"; //收付类型 I收、O付

                    db.MioBatch.Add(mio_batch);

                    List<MioList> mio_list = new List<MioList>(); //写入本数据库中的 收付明细表
                    list.ForEach(
                        t =>
                            mio_list.Add(new MioList()
                            {
                                batch_id = mio_batch.batch_id,
                                generation_id = t.id,
                                mio_type = "代付",
                                bank_account_no = t.salesman_bank_account_number,
                                bank_account_name = t.salesman_bank_account_name,
                                cash_deposit = t.salesman_cash_deposit,
                                status = -1,
                                result = "正在处理中"
                            }));

                    db.MioList.AddRange(mio_list);

                    DbInterface db_context = new DbInterface();

                    INTERFACE_MIO_BATCH_BZJ batch = new INTERFACE_MIO_BATCH_BZJ(); //写入保证金收付接口表——批次表
                    batch.MioType = "O"; //收付类型 I收、O付
                    batch.DataCnt = list.Count; //批次中包含的收付笔数
                    batch.SumAmnt = sum_amount; //批次总金额
                    batch.GenerateTime = DateTime.Now; //批次生成的时间
                    batch.GenerateBy = HttpContext.Current.Request.Cookies["user_code"].Value; //产生数据人员，八位ERP工号
                    batch.FromBatchNo = mio_batch.batch_id; //外部系统批次号
                    batch.BatchStatus = 0; //批次状态（默认为0）

                    batch.FromSys = "UnKnow"; //外部系统编号

                    db_context.INTERFACE_MIO_BATCH_BZJ.Add(batch);
                    db_context.SaveChanges();

                    INTERFACE_MIO_LIST_BZJ mio = null;
                    list.ForEach(t =>
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
                        mio.BankCode = "UnKnow"; //中国人寿编码的银行代码，需转换为银联代码
                        mio.BankCode = "?"; //中国人寿编码的银行代码，需转换为银联代码

                        db_context.INTERFACE_MIO_LIST_BZJ.Add(mio);
                    });

                    db_context.SaveChanges();
                    db.SaveChanges();

                    scope.Complete();

                    QuartzManager<QueryGivesInfo>.AddJob(mio_batch.batch_id, "0 1/1 * * * ?");
                    //1分钟之后执行第一次（对应“1/1”第一个1）,然后每隔1分钟执行一次（对应“1/1”第二个1）
                }
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