using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace WUYK.Common.Helpers
{
    public static class SecurityHelper
    {
        #region 获取由SHA1加密的字符串
        public static string HashPasswordUseSha1(string str)
        {
            using (SHA1CryptoServiceProvider sha1 = new SHA1CryptoServiceProvider())
            {
                byte[] str1 = Encoding.UTF8.GetBytes(str);
                byte[] str2 = sha1.ComputeHash(str1);
                StringBuilder enText = new StringBuilder();
                foreach (byte iByte in str2)
                {
                    enText.AppendFormat("{0:x2}", iByte);
                }
                return enText.ToString();
            }
        }
        #endregion

        /// <summary>
        /// 加密用户信息[先MD5，再SHA256]
        /// </summary>
        /// <param name="account">账户</param>
        /// <param name="password">密码</param>
        /// <returns></returns>
        public static string HashPassword(string account, string password)
        {
            string md5 = HashPasswordUseMd5(account + ":" + password);
            return HashPasswordUseSha256(md5);
        }

        public static string HashPasswordUseSha256(string text)
        {
            using (SHA256 sha256 = new SHA256Managed())
            {
                byte[] str1 = Encoding.UTF8.GetBytes(text);
                byte[] str2 = sha256.ComputeHash(str1);
                StringBuilder enText = new StringBuilder();
                foreach (byte iByte in str2)
                {
                    enText.AppendFormat("{0:x2}", iByte);
                }
                return enText.ToString();
            }
            
        }

        public static string HashPasswordUseSha512(string text)
        {
            using (SHA512 sha256 = new SHA512Managed())
            {
                byte[] str1 = Encoding.UTF8.GetBytes(text);
                byte[] str2 = sha256.ComputeHash(str1);
                StringBuilder enText = new StringBuilder();
                foreach (byte iByte in str2)
                {
                    enText.AppendFormat("{0:x2}", iByte);
                }
                return enText.ToString();
            }

        }

        #region 获取由MD5加密的字符串
        public static string HashPasswordUseMd5(string str)
        {
            using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
            {
                byte[] str1 = Encoding.UTF8.GetBytes(str);
                byte[] str2 = md5.ComputeHash(str1, 0, str1.Length);
                StringBuilder enText = new StringBuilder();
                foreach (byte iByte in str2)
                {
                    enText.AppendFormat("{0:x2}", iByte);
                }
                return enText.ToString();
            }
        }
        #endregion
    }
}
