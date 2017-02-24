using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Service.Model
{
    public class Deducted : DbBase
    {
        public Nullable<int> generation_gives_id { get; set; }

        public string item { get; set; }

        public Nullable<decimal> amount { get; set; }

        public string remark { get; set; }
    }
}