using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WYJK.Web.Models
{
    public class SubjectEditModel
    {
        /// <summary>
        /// 题目
        /// </summary>	
        public string Subject { get; set; }
        /// <summary>
        /// 排序
        /// </summary>	
        public int Sort { get; set; }
        /// <summary>
        /// 答案列表
        /// </summary>	
        public string Answer { get; set; }
        /// <summary>
        /// 贷款金额列表
        /// </summary>	
        public decimal LoanAmount { get; set; }
    }
    public class SubjectViewModel
    {
        /// <summary>
        /// 题目ID
        /// </summary>
        public int SubjectID { get; set; }
        /// <summary>
        /// 题目
        /// </summary>
        [Required(ErrorMessage = "题目必填")]
        public string Subject { get; set; }
        /// <summary>
        /// 排序
        /// </summary>	
        [Required(ErrorMessage = "排序必填")]
        [Range(minimum: 0, maximum: 9999, ErrorMessage = "不是正确的数字")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "必须为数字")]
        public int Sort { get; set; }
        /// <summary>
        /// 答案列表
        /// </summary>	
        [Required(ErrorMessage = "答案必填")]
        public List<string> Answer { get; set; }
        /// <summary>
        /// 贷款金额列表
        /// </summary>	
        [Required(ErrorMessage = "贷款金额必填")]
        public List<decimal> LoanAmount { get; set; }
    }

}