using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WYJK.Data.IService;
using WYJK.Entity;

namespace WYJK.Data.ServiceImpl
{
    /// <summary>
    /// 借款题目
    /// </summary>
    public class LoanSubjectService : ILoanSubjectService
    {
        /// <summary>
        /// 获取题目列表
        /// </summary>
        /// <returns></returns>
        public Dictionary<int, LoanSubject>.ValueCollection GetSubjectList(string Subject)
        {
            string sql = $@" select LoanSubject.SubjectID, LoanSubject.Subject,LoanSubject.Sort,LoanSubject.SubjectID,LoanAnswer.Answer from LoanSubject 
                              left join LoanAnswer on
                              LoanSubject.SubjectID = LoanAnswer.SubjectID
                              where Subject like '%{Subject}%'";
            var lookUp = new Dictionary<int, LoanSubject>();
            List<LoanSubject> list = DbHelper.CustomQuery<LoanSubject, LoanAnswer, LoanSubject>(sql,
                (subject, answer) =>
            {
                LoanSubject s;
                if (!lookUp.TryGetValue(subject.SubjectID, out s))
                {
                    lookUp.Add(subject.SubjectID, s = subject);
                }

                s.LoanAnswerList.Add(answer);
                return subject;
            },
            "SubjectID").ToList();
            var result = lookUp.Values;
            Debug.WriteLine(lookUp.Values);

            return result;
        }

        /// <summary>
        /// 添加题目
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool AddSubject(LoanSubject model)
        {
            string sqlstr = $"insert into LoanSubject(Subject,Sort) values('{model.Subject}',{model.Sort})";
            int result = DbHelper.ExecuteSqlCommandScalar(sqlstr, new SqlParameter[] { });
            int result1 = 0;
            if (result > 0)
            {
                string sql = string.Empty;
                foreach (var item in model.LoanAnswerList)
                {
                    sql += $"insert into LoanAnswer(Answer,LoanAmount,SubjectID) values('{item.Answer}',{item.LoanAmount},{result});";
                }
                result1 = DbHelper.ExecuteSqlCommand(sql, null);
                return result1 > 0;
            }

            return false;
        }

        /// <summary>
        /// 批量删除题目
        /// </summary>
        /// <param name="SubjectIDstr"></param>
        /// <returns></returns>
        public bool BatchDelete(string SubjectIDstr)
        {
            string sqlstr = $"delete from LoanSubject where SubjectID in('{SubjectIDstr}')";
            int result = DbHelper.ExecuteSqlCommand(sqlstr, null);
            return result > 0;
        }

        /// <summary>
        /// 获取题目详情
        /// </summary>
        /// <param name="SubjectID"></param>
        /// <returns></returns>
        public LoanSubject GetSubjectDetail(int SubjectID)
        {
            string sqlSubject = $"select * from LoanSubject where SubjectID ={SubjectID}";
            LoanSubject subjectModel = DbHelper.QuerySingle<LoanSubject>(sqlSubject);

            string sqlAnswer = $"select * from LoanAnswer where SubjectID={SubjectID}";
            List<LoanAnswer> answerList = DbHelper.Query<LoanAnswer>(sqlAnswer);

            subjectModel.LoanAnswerList.AddRange(answerList);

            return subjectModel;
        }

        /// <summary>
        /// 更新题目
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool UpdateSubject(LoanSubject model)
        {
            if (BatchDelete(model.SubjectID.ToString()) &&
             AddSubject(model))
                return true;
            else
                return false;
        }

        /// <summary>
        /// 获取题目
        /// </summary>
        /// <returns></returns>
        public LoanSubject GetChoiceSubject(int? SubjectID)
        {
            string sqlSubject = string.Empty;
            LoanSubject subjectModel = null;
            if (SubjectID == null)
            {
                sqlSubject = $"select top 1 * from LoanSubject order by Sort";
                subjectModel = DbHelper.QuerySingle<LoanSubject>(sqlSubject);
            }
            else {
                sqlSubject = $"select * from LoanSubject where SubjectID ={SubjectID}";
                subjectModel = DbHelper.QuerySingle<LoanSubject>(sqlSubject);
            }

            string sqlAnswer = $"select * from LoanAnswer where SubjectID={subjectModel.SubjectID}";
            List<LoanAnswer> answerList = DbHelper.Query<LoanAnswer>(sqlAnswer);

            subjectModel.LoanAnswerList.AddRange(answerList);

            string sqlNextSubject = $"select top 1 SubjectID from LoanSubject where Sort > (select Sort from LoanSubject where SubjectID={subjectModel.SubjectID}) order by Sort";
            int NextSubjectID = DbHelper.QuerySingle<int>(sqlNextSubject);
            subjectModel.NextSubjectID = NextSubjectID;

            string sqlSubjectCount = "select count(*) from LoanSubject";
            int SubjectCount = DbHelper.QuerySingle<int>(sqlSubjectCount);

            subjectModel.SubjectCount = SubjectCount;

            return subjectModel;
        }

        /// <summary>
        /// 身价计算
        /// </summary>
        /// <param name="AnswerIDStr"></param>
        /// <returns></returns>
        public bool ValueCalculation(int MemberID, string AnswerIDStr)
        {
            string sqlstr = $"select SUM(LoanAmount) from LoanAnswer where AnswerID in({AnswerIDStr})";
            decimal value = DbHelper.QuerySingle<decimal>(sqlstr);
            string sqlstr1 = $@"INSERT INTO MemberLoan
                                           (MemberID
                                           , TotalAmount
                                           ,AvailableAmount)
                                     VALUES
                                           ({MemberID}
                                           ,{value}
                                           ,{value})";
            int result = DbHelper.ExecuteSqlCommandScalar(sqlstr1, new DbParameter[] { });
            return result > 0;
        }

        /// <summary>
        /// 获取用户身价
        /// </summary>
        /// <param name="MemberID"></param>
        /// <returns></returns>
        public decimal GetMemberValue(int MemberID)
        {
            string strsql = $"select TotalAmount from MemberLoan where MemberID={MemberID}";
            decimal value = DbHelper.QuerySingle<decimal>(strsql);
            return value;
        }
    }
}
