using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Service.Model
{
    public class Staff : DbBase
    {
        public string code { get; set; }

        public string agency_code { get; set; }

        public string name { get; set; }

        public string pwd { get; set; }

        public Nullable<int> enable { get; set; }
    }
}
