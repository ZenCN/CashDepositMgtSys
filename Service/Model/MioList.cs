using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Service.Model
{
    public class MioList: DbBase
    {
        [Description("批次号")]
        public string batch_id { get; set; }

        [Description("代付表ID")]
        public int generation_gives_id { get; set; }
    }
}
