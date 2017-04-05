using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Infrastructure.Operation;
using Service.Model;

namespace Service
{
    public class LoginSvr : BaseSvr
    {
        public Result QueryUserInfo(string user_code)
        {
            var staff = db.Staff.SingleOrDefault(t => t.code == user_code);

            if (staff != null)
            {
                if (staff.role == "accountant")
                {
                    Agencys agency_info = null;
                    List<Agencys> agencys = null;

                    List<string> p_codes = new List<string>();
                    string[] arr = staff.jurisdiction.Split(new string[] {"、"}, StringSplitOptions.RemoveEmptyEntries);
                    p_codes.AddRange(arr);

                    agencys = (from t in db.Agency
                        where p_codes.Contains(t.code)
                        select new Agencys()
                        {
                            code = t.code,
                            name = t.name
                        }).ToList();

                    agencys.ForEach(_this =>
                    {
                        _this.lower = (from t in db.Agency
                            where t.p_code == _this.code
                            select new Agencys()
                            {
                                code = t.code,
                                name = t.name
                            }).ToList();
                    });

                    return new Result(ResultType.success,
                        new
                        {
                            role = staff.role,
                            authority = staff.authority,
                            jurisdiction = staff.jurisdiction,
                            agency = agencys
                        });
                }
                else
                {
                    return new Result(ResultType.success,
                        new
                        {
                            role = staff.role
                        });
                }
            }
            else
            {
                return new Result(ResultType.query_nothing);
            }
        }
    }

    public class Agencys
    {
        public string code { get; set; }

        public string name { get; set; }

        public List<Agencys> lower { get; set; }
    }
}
