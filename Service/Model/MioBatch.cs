using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Service.Model
{
    public class MioBatch: DbBase
    {
        [Description("批次号")]
        public string batch_id { get; set; }

        [Description("代付笔数")]
        public int record_count { get; set; }

        [Description("代付总金额")]
        public decimal sum_amount { get; set; }

        [Description("I收/O付")]
        public string mio_type { get; set; }

        [Description("审核人工号")]
        public string reviewer_code { get; set; }

        [Description("审核日期")]
        public Nullable<DateTime> review_date { get; set; }

        [Description("推送结果")]
        public string push_result { get; set; }

        [Description("推送日期")]
        public Nullable<DateTime> push_date { get; set; }

        [Description("批次处理完成时间")]
        public Nullable<DateTime> done_date { get; set; }
    }
}
