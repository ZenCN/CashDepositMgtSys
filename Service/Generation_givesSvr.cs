using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Infrastructure.Exception;
using Infrastructure.Operation;
using Newtonsoft.Json;
using Service.Model;

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
                gives.record_date = DateTime.Now;;

                List<Deducted> deducteds = JsonConvert.DeserializeObject<List<Deducted>>(deducted_items);

                db.Generation_gives.Add(gives);

                Entity.SaveChanges(db);

                deducteds.ForEach(t =>
                {
                    t.generation_gives_id = gives.id;
                });
                db.Deducted.AddRange(deducteds);

                Entity.SaveChanges(db);

                return new Result(ResultType.success, new {generation_gives_id = gives.id});
            }
            catch (Exception ex)
            {
                return new Result(ResultType.error, new Message(ex).ErrorDetails);
            }
        }
    }
}
