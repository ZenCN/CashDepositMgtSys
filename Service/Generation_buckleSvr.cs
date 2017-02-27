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

        public Result Delete(List<int> ids)
        {
            db = new Db();

            try
            {
                var buckle = db.Generation_buckle.Where(t => ids.Contains(t.id)).ToList();
                buckle.ForEach(t => t.is_deleted = 1);

                Entity.SaveChanges(db);

                return new Result(ResultType.success);
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
            catch (Exception ex)
            {
                return new Result(ResultType.error, new Message(ex).ErrorDetails);
            }
        }

        public Result Search(int page_index, int page_size, string salesman_card_id, string salesman_name,
            string user_code, string agency_code, int level)
        {
            db = new Db();
            List<Generation_buckle> list = null;

            int page_count = 0;
            int record_count = 0;

            IQueryable<Generation_buckle> query = db.Generation_buckle.Where(t => t.is_deleted != 1);
            switch (level)
            {
                case 3:
                    agency_code = agency_code.Substring(0, 4);
                    query =
                        query.Where(
                            t =>
                                t.agency_code.StartsWith(agency_code) &&
                                (t.reviewer_code == null || t.reviewer_code == user_code));
                    break;
                case 4:
                    query = query.Where(t => t.agency_code == agency_code && t.recorder_code == user_code);
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
                                t.salesman_hiredate == _this.salesman_hiredate);

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
                    return new Result(ResultType.success, "但部分人员信息因已存在已被忽略导入，详见表格", ignore);
                else
                    return new Result(ResultType.success, "人员信息全部成功导入");
            }
            catch (Exception ex)
            {
                return new Result(ResultType.error, new Message(ex).ErrorDetails);
            }
        }

        public void Export(int page_index, int page_size, string salesman_card_id, string salesman_name,
            string user_code, string agency_code, int level)
        {
            db = new Db();
            List<Generation_buckle> list = null;

            int page_count = 0;
            int record_count = 0;

            IQueryable<Generation_buckle> query = db.Generation_buckle.Where(t => t.is_deleted != 1);
            switch (level)
            {
                case 3:
                    agency_code = agency_code.Substring(0, 4);
                    query =
                        query.Where(
                            t =>
                                t.agency_code.StartsWith(agency_code) &&
                                (t.reviewer_code == null || t.reviewer_code == user_code));
                    break;
                case 4:
                    query = query.Where(t => t.agency_code == agency_code && t.recorder_code == user_code);
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
            page_count = ((record_count + page_size) - 1) / page_size;

            list = list.OrderByDescending(t => t.record_date).Skip(page_index * page_size).Take(page_size).ToList();

            new Excel().Export(list);
        }

        public Result ChangeReviewState(List<int> ids, int state)
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