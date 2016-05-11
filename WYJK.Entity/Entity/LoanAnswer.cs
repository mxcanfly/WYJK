using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WYJK.Entity
{
    /// <summary>
    /// 答案
    /// </summary>
    public class LoanAnswer
    {
        /// <summary>
        /// 答案ID
        /// </summary>		
        public int AnswerID { get; set; }
        /// <summary>
        /// 答案
        /// </summary>		
        public string Answer { get; set; }
        /// <summary>
        /// 贷款金额
        /// </summary>		
        [JsonIgnore]
        public decimal LoanAmount { get; set; }
        /// <summary>
        /// 题目ID
        /// </summary>		
        [JsonIgnore]
        public int SubjectID { get; set; }

    }
}
