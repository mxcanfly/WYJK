using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WYJK.Framework.Core
{
    /// <summary>
    /// 常用正则表达式
    /// </summary>
    public static class RegularConstant
    {
        /// <summary>
        /// 账号，只能由数字，字母组成，并且在6-12位之间
        /// </summary>
        public const string Account1 = "^[a-zA-Z][a-zA-Z0-9]{6,12}$";
        /// <summary>
        /// 中文、英文、数字及下划线
        /// </summary>
        public const string Account2 = @"^[\u4e00-\u9fa5_a-zA-Z0-9]+$";
        /// <summary>
        /// 大陆手机号码格式
        /// </summary>
        public const string Mobile = "^1[0-9]{10}$";
        /// <summary>
        /// 密码格式，只能由数字，字母和特殊字符，并且在6-18位之间
        /// </summary>
        public const string Password = "^[0-9a-zA-Z@_#!$]{6,18}$";
        /// <summary>
        /// 邮箱校验【宽松型，只验证大体格式】
        /// </summary>
        public const string Email1 = @"[\w!#$%&'*+/=?^_`{|}~-]+(?:\.[\w!#$%&'*+/=?^_`{|}~-]+)*@(?:[\w](?:[\w-]*[\w])?\.)+[\w](?:[\w-]*[\w])?";
        /// <summary>
        /// 邮箱校验【严谨性】
        /// </summary>
        public const string Email2 = @"^(\w)+(\.\w+)*@(\w)+((\.\w{2,3}){1,3})$";

        
    }
}
