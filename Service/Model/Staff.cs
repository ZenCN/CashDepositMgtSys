using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Service.Model
{
    public class Staff : DbBase
    {
        [Description("工号")]
        public string code { get; set; }

        [Description("角色：worker 工作人员、leader 领导、accountant 会计、financial 财务资金部")]
        public string role { get; set; }

        [Description("权限，当role为会计accountant时：0 初审、1 复审")]
        public Nullable<int> authority { get; set; }

        [Description("限定的审核范围")]
        public string jurisdiction { get; set; }

        [Description("所属机构代码")]
        public string agency_code { get; set; }
    }
}
