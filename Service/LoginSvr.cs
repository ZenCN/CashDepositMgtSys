using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Infrastructure.Operation;
using Service.Model;

namespace Service
{
    public class LoginSvr: BaseSvr
    {
        public Result QueryUserInfo(string user_code)
        {
            var staff = db.Staff.SingleOrDefault(t => t.code == user_code);

            if (staff != null)
            {
                return new Result(ResultType.success,
                    new {role = staff.role, authority = staff.authority, jurisdiction = staff.jurisdiction});
            }
            else
            {
                return new Result(ResultType.query_nothing);
            }
        }
    }
}
