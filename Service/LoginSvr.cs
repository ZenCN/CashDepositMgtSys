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
                string agency = "";

                if (staff.role == "accountant")
                {
                    List<string> p_codes = new List<string>();
                    string[] arr = staff.jurisdiction.Split(new string[] {"、"}, StringSplitOptions.RemoveEmptyEntries);
                    p_codes.AddRange(arr);
                    var list = (from t in db.Agency
                        where p_codes.Contains(t.p_code) || p_codes.Contains(t.code)
                        select new
                        {
                            code = t.code,
                            name = t.name
                        }).ToList();
                    agency = "{";
                    list.ForEach(t =>
                    {
                        agency += "\"" + t.code.Trim() + "\":\"" + t.name.Trim() + "\",";
                    });
                    agency = agency.Remove(agency.Length - 1) + "}";
                }
                else
                {
                    var list = (from t in db.Agency
                        select new
                        {
                            code = t.code,
                            name = t.name
                        }).ToList();
                    agency = "{";
                    list.ForEach(t =>
                    {
                        agency += "\"" + t.code.Trim() + "\":\"" + t.name.Trim() + "\",";
                    });
                    agency = agency.Remove(agency.Length - 1) + "}";
                }

                return new Result(ResultType.success,
                    new {role = staff.role, authority = staff.authority, jurisdiction = staff.jurisdiction, agency = agency });
            }
            else
            {
                return new Result(ResultType.query_nothing);
            }
        }
    }
}
