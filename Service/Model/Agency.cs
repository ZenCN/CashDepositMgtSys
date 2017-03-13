using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Service.Model
{
    public class Agency : DbBase
    {
        public string code { get; set; }

        public string p_code { get; set; }

        public string name { get; set; }

        public string type { get; set; }
    }
}
