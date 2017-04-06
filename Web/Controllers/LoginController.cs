using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using Service;

namespace Web.Controllers
{
    public class LoginController : Controller
    {
        public ActionResult Index()
        {
            return File("~/client/index.html", "text/html");
        }

        public string QueryUserInfo(string user_code)
        {
            LoginSvr svr = new LoginSvr();

            return JsonConvert.SerializeObject(svr.QueryUserInfo(user_code));
        }

        public string DecodeUserInfo(string userno, string pwd)
        {
            userno = Decode(userno.Replace(" ", "+"));
            pwd = Decode(pwd.Replace(" ", "+"));

            var client = new WebClient();
            client.Encoding = Encoding.UTF8;
            string url = string.Format("http://10.20.147.103:8080/api/json/reply/login?username={0}&password={1}", userno, pwd);

            return client.DownloadString(url);
        }

        private string Decode(string decryptString)
        {
            try
            {
                byte[] Keys = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };
                string decryptKey = "minglong";
                decryptKey = GetSubString(decryptKey, 8, "");
                decryptKey = decryptKey.PadRight(8, ' ');
                byte[] rgbKey = Encoding.UTF8.GetBytes(decryptKey);
                byte[] rgbIV = Keys;
                byte[] inputByteArray = Convert.FromBase64String(decryptString);
                DESCryptoServiceProvider DCSP = new DESCryptoServiceProvider();

                MemoryStream mStream = new MemoryStream();
                CryptoStream cStream = new CryptoStream(mStream, DCSP.CreateDecryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
                cStream.Write(inputByteArray, 0, inputByteArray.Length);
                cStream.FlushFinalBlock();
                return Encoding.UTF8.GetString(mStream.ToArray());
            }
            catch(Exception ex)
            {
                return ex.Message;
            }
        }

        /// <summary>
        /// 字符串如果操过指定长度则将超出的部分用指定字符串代替
        /// </summary>
        /// <param name="p_SrcString">要检查的字符串</param>
        /// <param name="p_Length">指定长度</param>
        /// <param name="p_TailString">用于替换的字符串</param>
        /// <returns>截取后的字符串</returns>
        public string GetSubString(string p_SrcString, int p_Length, string p_TailString)
        {
            string myResult = p_SrcString;

            if (p_Length >= 0)
            {
                byte[] bsSrcString = Encoding.Default.GetBytes(p_SrcString);

                if (bsSrcString.Length > p_Length)
                {
                    int nRealLength = p_Length;
                    int[] anResultFlag = new int[p_Length];
                    byte[] bsResult = null;

                    int nFlag = 0;
                    for (int i = 0; i < p_Length; i++)
                    {

                        if (bsSrcString[i] > 127)
                        {
                            nFlag++;
                            if (nFlag == 3)
                            {
                                nFlag = 1;
                            }
                        }
                        else
                        {
                            nFlag = 0;
                        }

                        anResultFlag[i] = nFlag;
                    }

                    if ((bsSrcString[p_Length - 1] > 127) && (anResultFlag[p_Length - 1] == 1))
                    {
                        nRealLength = p_Length + 1;
                    }

                    bsResult = new byte[nRealLength];

                    Array.Copy(bsSrcString, bsResult, nRealLength);

                    myResult = Encoding.Default.GetString(bsResult);

                    myResult = myResult + p_TailString;
                }

            }

            return myResult;
        }
    }
}
