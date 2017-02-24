﻿using System;
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
                gives.record_date = DateTime.Now;

                db.Generation_gives.Add(gives);

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

        public Result Search(int page_index, int page_size, string salesman_card_id, string salesman_name)
        {
            db = new Db();

            try
            {
                int page_count = 0;
                int record_count = 0;
                List<Generation_gives> list = null;

                if (!string.IsNullOrEmpty(salesman_card_id))
                {
                    list = db.Generation_gives.Where(t => t.salesman_card_id.Contains(salesman_card_id)).ToList();
                }

                if (!string.IsNullOrEmpty(salesman_name))
                {
                    if (list != null)
                    {
                        list = list.Where(t => t.salesman_name.Contains(salesman_name)).ToList();
                    }
                    else
                    {
                        list = db.Generation_gives.Where(t => t.salesman_name.Contains(salesman_name)).ToList();
                    }
                }

                if (list == null)
                {
                    list = db.Generation_gives.ToList();
                }

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

                return new Result(ResultType.success);
            }
            catch (Exception ex)
            {
                return new Result(ResultType.error, new Message(ex).ErrorDetails);
            }
        }
    }
}