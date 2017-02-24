using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Infrastructure.Exception;
using Infrastructure.Operation;
using Service.Model;

namespace Service
{
    public class Generation_buckleSvr
    {
        private Db db = null;

        public Result Search(int page_index, int page_size, string salesman_card_id, string salesman_name)
        {
            db = new Db();
            List<Generation_buckle> list = null;

            int page_count = 0;
            int record_count = 0;

            if (!string.IsNullOrEmpty(salesman_card_id))
            {
                list = db.Generation_buckle.Where(t => t.salesman_card_id.Contains(salesman_card_id)).ToList();
            }

            if (!string.IsNullOrEmpty(salesman_name))
            {
                if (list != null)
                {
                    list = list.Where(t => t.salesman_name.Contains(salesman_name)).ToList();
                }
                else
                {
                    list = db.Generation_buckle.Where(t => t.salesman_name.Contains(salesman_name)).ToList();
                }
            }

            if (list == null)
            {
                list = db.Generation_buckle.ToList();
            }

            record_count = list.Count;
            page_count = ((record_count + page_size) - 1) / page_size;

            list = list.OrderByDescending(t => t.record_date).Skip(page_index*page_size).Take(page_size).ToList();

            return new Result(ResultType.success,
                new {record_count = record_count, page_count = page_count, list = list});
        }

        public Result Import(List<Generation_buckle> list)
        {
            db = new Db();
            Generation_buckle buckle = null;
            List<Generation_buckle> ignore  = new List<Generation_buckle>();

            try
            {
                foreach (Generation_buckle _this in list)
                {
                    buckle = db.Generation_buckle.SingleOrDefault(t => t.salesman_card_id == _this.salesman_card_id && t.salesman_hiredate == _this.salesman_hiredate);

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

                if(ignore.Count > 0)
                    return new Result(ResultType.success, "但部分人员信息因已存在已被忽略导入，详见表格", ignore);
                else
                    return new Result(ResultType.success, "人员信息全部成功导入");
            }
            catch (Exception ex)
            {
                return new Result(ResultType.error, new Message(ex).ErrorDetails);
            }
        }

        public void Export(int page_index, int page_size, string salesman_card_id, string salesman_name)
        {
            db = new Db();
            List<Generation_buckle> list = null;

            int page_count = 0;
            int record_count = 0;

            if (!string.IsNullOrEmpty(salesman_card_id))
            {
                list = db.Generation_buckle.Where(t => t.salesman_card_id.Contains(salesman_card_id)).ToList();
            }

            if (!string.IsNullOrEmpty(salesman_name))
            {
                if (list != null)
                {
                    list = list.Where(t => t.salesman_name.Contains(salesman_name)).ToList();
                }
                else
                {
                    list = db.Generation_buckle.Where(t => t.salesman_name.Contains(salesman_name)).ToList();
                }
            }

            if (list == null)
            {
                list = db.Generation_buckle.ToList();
            }

            record_count = list.Count;
            page_count = ((record_count + page_size) - 1) / page_size;

            list = list.OrderByDescending(t => t.record_date).Skip(page_index * page_size).Take(page_size).ToList();

            new Excel().Export(list);
        }

        public Result ChangeRecordState(List<int> ids, int state)
        {
            db = new Db();

            try
            {
                var buckle = db.Generation_buckle.Where(t => ids.Contains(t.id)).ToList();
                buckle.ForEach(t => t.review_state = state);

                Entity.SaveChanges(db);

                return new Result(ResultType.success);
            }
            catch (Exception ex)
            {
                return new Result(ResultType.error, new Message(ex).ErrorDetails);
            }
        }
    }
}
