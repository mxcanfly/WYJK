using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WYJK.Model
{
    public class Employee
    {
        /// <summary>
        /// 
        /// </summary>
        public int ID
        {
            get; set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string EmployeeName
        {
            get; set;
        }
        /// <summary>
        /// 注册类型：0：兼职，1：正式
        /// </summary>
        public string RegType
        {
            get; set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string Password
        {
            get; set;
        }
        /// <summary>
        /// 身份证号
        /// </summary>
        public string IdentityCard
        {
            get; set;
        }
        /// <summary>
        /// 
        /// </summary>
        public int? CompanyID
        {
            get; set;
        }
        /// <summary>
        /// 
        /// </summary>
        public int? DepartmentID
        {
            get; set;
        }
        /// <summary>
        /// 
        /// </summary>
        public int? RoleID
        {
            get; set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string TrueName
        {
            get; set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string ContactPhone
        {
            get; set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string QQ
        {
            get; set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string OfficeTelephone
        {
            get; set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string Email
        {
            get; set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string Address
        {
            get; set;
        }
        /// <summary>
        /// 开户行
        /// </summary>
        public string BankAccount
        {
            get; set;
        }
        /// <summary>
        /// 开户人
        /// </summary>
        public string UserAccount
        {
            get; set;
        }
        /// <summary>
        /// 银行卡号
        /// </summary>
        public string CardNo
        {
            get; set;
        }
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool Enabled
        {
            get; set;
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? CreateTime
        {
            get; set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string CreateUser
        {
            get; set;
        }
    }
}
