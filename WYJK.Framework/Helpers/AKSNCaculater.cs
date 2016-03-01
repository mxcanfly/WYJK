using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace WYJK.Framework.Helpers
{
    /// <summary>
    /// 百度SN校验类
    /// </summary>
    internal class AKSNCaculater
    {
        private static string MD5(string password)
        {
            byte[] textBytes = System.Text.Encoding.UTF8.GetBytes(password);
            System.Security.Cryptography.MD5CryptoServiceProvider cryptHandler = null;
            try
            {
                cryptHandler = new System.Security.Cryptography.MD5CryptoServiceProvider();
                byte[] hash = cryptHandler.ComputeHash(textBytes);
                string ret = "";
                foreach (byte a in hash)
                {
                    ret += a.ToString("x");
                }
                return ret;
            }
            catch
            {
                throw;
            }
            finally
            {
                cryptHandler.Dispose();
            }
        }

        private static string UrlEncode(string str)
        {
            str = System.Web.HttpUtility.UrlEncode(str);
            byte[] buf = Encoding.UTF8.GetBytes(str);//等同于Encoding.ASCII.GetBytes(str)
            for (int i = 0; i < buf.Length; i++)
                if (buf[i] == '%')
                {
                    if (buf[i + 1] >= 'a') buf[i + 1] -= 32;
                    if (buf[i + 2] >= 'a') buf[i + 2] -= 32;
                    i += 2;
                }
            return Encoding.UTF8.GetString(buf);//同上，等同于Encoding.ASCII.GetString(buf)
        }

        private static string HttpBuildQuery(IDictionary<string, string> querystring_arrays)
        {

            StringBuilder sb = new StringBuilder();
            foreach (var item in querystring_arrays)
            {
                sb.Append(item.Key);
                sb.Append("=");
                sb.Append(HttpUtility.UrlEncode(item.Value,Encoding.UTF8));
                sb.Append("&");
            }
            sb.Remove(sb.Length - 1, 1);
            return sb.ToString();
        }

        public static string CaculateAKSN(string ak, string sk, string url, IDictionary<string, string> querystring_arrays)
        {
            var queryString = HttpBuildQuery(querystring_arrays);

            var str = HttpUtility.UrlEncode(url + "?" + queryString + sk,Encoding.UTF8);

            return MD5(str);
        }
    }
}
