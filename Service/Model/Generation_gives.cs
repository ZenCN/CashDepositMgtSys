using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Service.Model
{
    public class Generation_gives: DbBase
    {
        public Nullable<System.DateTime> record_date { get; set; }

        public string recorder_code { get; set; }

        public string reviewer_code { get; set; }

        public Nullable<int> review_state { get; set; }

        public Nullable<System.DateTime> review_date { get; set; }

        public string agency_code { get; set; }

        public string salesman_name { get; set; }

        public string salesman_sex { get; set; }

        public string salesman_code { get; set; }

        [Description("流水号")]
        public string gather_serial_num { get; set; }

        public string salesman_card_type { get; set; }

        public string salesman_card_id { get; set; }

        public string salesman_phone { get; set; }

        public Nullable<System.DateTime> salesman_hiredate { get; set; }

        public string salesman_bank_account_name { get; set; }

        public string salesman_bank_account_number { get; set; }

        public string salesman_bank_name { get; set; }

        public string salesman_bank_province { get; set; }

        public string salesman_bank_city { get; set; }

        public Nullable<decimal> salesman_cash_deposit { get; set; }

        public Nullable<decimal> salesman_refunds { get; set; }

        public Nullable<int> salesman_refunds_state { get; set; }

        public string remark { get; set; }

        public Nullable<int> is_deleted { get; set; }

        public string channel { get; set; }

        public string process_result { get; set; }

        public Nullable<DateTime> finish_time { get; set; }
    }
}
