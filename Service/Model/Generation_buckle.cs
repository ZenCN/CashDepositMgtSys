﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Service.Model
{
    public class Generation_buckle : DbBase
    {
        public Nullable<System.DateTime> record_date { get; set; }

        public string recorder_code { get; set; }

        public string reviewer_code { get; set; }

        public Nullable<int> review_state { get; set; }

        public Nullable<System.DateTime> review_date { get; set; }

        public string agency_code { get; set; }

        public string salesman_name { get; set; }

        public string salesman_sex { get; set; }

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

        public string remark { get; set; }
    }
}