using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WYJK.Entity;

namespace WYJK.Data.IService
{
    /// <summary>
    /// 借款答题
    /// </summary>
    public interface ILoanSubjectService
    {
        /// <summary>
        /// 获取题目列表
        /// </summary>
        /// <returns></returns>
        Dictionary<int, LoanSubject>.ValueCollection GetSubjectList(string Subject);

        /// <summary>
        /// 添加题目
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        bool AddSubject(LoanSubject model);
        /// <summary>
        /// 更新题目
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        bool UpdateSubject(LoanSubject model);
        /// <summary>
        /// 批量删除题目
        /// </summary>
        /// <param name="SubjectIDstr"></param>
        /// <returns></returns>
        bool BatchDelete(string SubjectIDstr);

        /// <summary>
        /// 获取题目详情
        /// </summary>
        /// <param name="SubjectID"></param>
        /// <returns></returns>
        LoanSubject GetSubjectDetail(int SubjectID);

        /// <summary>
        /// 获取题目
        /// </summary>
        /// <returns></returns>
        LoanSubject GetChoiceSubject(int? SubjectID);

        /// <summary>
        /// 身价计算
        /// </summary>
        /// <param name="AnswerIDStr"></param>
        /// <returns></returns>
        bool ValueCalculation(int MemberID, string AnswerIDStr);

        /// <summary>
        /// 获取用户身价
        /// </summary>
        /// <param name="MemberID"></param>
        /// <returns></returns>
        decimal GetMemberValue(int MemberID);

    }
}
