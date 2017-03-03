using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using Service.Model;

namespace Service
{
    public class BaseSvr
    {
        protected Db db;

        protected BaseSvr(bool create_entity = true)
        {
            if (create_entity)
            {
                db = new Db();
            }
        }
    }
}
