using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Service.Model
{
    public class Account: DbBase
    {
        public string type { get; set; }  //I收、O付

        public string number { get; set; }
    }
}
