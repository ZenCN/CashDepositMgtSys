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

        [Description("代付或代扣表ID")]
        public int generation_id { get; set; }

        public string mio_type { get; set; }

        public string bank_account_name { get; set; }

        public string bank_account_no { get; set; }

        public int status { get; set; }

        public string result { get; set; }
    }
}
