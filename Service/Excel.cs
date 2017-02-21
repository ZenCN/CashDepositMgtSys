using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Infrastructure.Exception;
using Infrastructure.Operation;
using Service.Model;

namespace Service
{
    public class Excel
    {
        private Db db = null;

        public Result ImportDataInDataBase(List<Generation_buckle> list)
        {
            db = new Db();
            Generation_buckle buckle = null;
            List<Generation_buckle> ignore  = new List<Generation_buckle>();

            try
            {
                foreach (Generation_buckle _this in list)
                {
                    buckle = db.Generation_buckle.SingleOrDefault(t => t.salesman_card_id == _this.salesman_card_id);

                    if (buckle == null)
                    {
                        db.Generation_buckle.Add(_this);
                    }
                    else
                    {
                        ignore.Add(_this);
                    }
                }

                Entity.SaveChanges(db);

                return new Result(ResultType.Success, "部分数据因数据库中已存在已被忽略", list);
            }
            catch (Exception ex)
            {
                return new Result(ResultType.Error, new Message(ex).ErrorDetails);
            }
        }
    }
}
