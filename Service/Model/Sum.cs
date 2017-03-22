using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Service.Model
{
    public class Sum
    {
        public string apply_start { get; set; }

        public string apply_end { get; set; }

        public string agency_code { get; set; }

        public string item { get; set; }

        public int count { get; set; }

        public decimal? amount { get; set; }

        public string channel { get; set; }
    }
}
