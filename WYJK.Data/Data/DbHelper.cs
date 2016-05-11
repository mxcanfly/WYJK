using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using WYJK.Framework.Setting;
using static Dapper.SqlMapper;

namespace WYJK.Data
{
    /// <summary>
    /// 数据库访问辅助类
    /// </summary>
    public static class DbHelper
    {
        #region 单条查询语句
        /// <summary>
        /// 执行查询语句返回指定实体信息
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static List<TEntity> Query<TEntity>(string sql, object parameters = null)
        {
            using (DbConnection connection = new SqlConnection(WebConfigurationManager.DefaultConnectionString))
            {
                connection.Open();
                return connection.Query<TEntity>(sql, parameters) as List<TEntity>;
            }
        }

        #region 连表查询语句
        /// <summary>
        /// 连表查询
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static List<TReturn> CustomQuery<TFirst, TSecond, TReturn>(string sql, Func<TFirst, TSecond, TReturn> map, string splitOn, object parameters = null)
        {
            using (DbConnection connection = new SqlConnection(WebConfigurationManager.DefaultConnectionString))
            {
                connection.Open();
                return connection.Query<TFirst, TSecond, TReturn>(sql, map, parameters, splitOn: splitOn) as List<TReturn>;
            }
        }
        #endregion

        public static async Task<List<TEntity>> QueryAsync<TEntity>(string sql, object parameters = null)
        {
            using (DbConnection connection = new SqlConnection(WebConfigurationManager.DefaultConnectionString))
            {
                await connection.OpenAsync();
                return await connection.QueryAsync<TEntity>(sql, parameters) as List<TEntity>;
            }
        }

        public static async Task<List<TEntity>> QueryAsync<TEntity>(string sql, object parameters, CommandType commandType)
        {
            using (DbConnection connection = new SqlConnection(WebConfigurationManager.DefaultConnectionString))
            {
                await connection.OpenAsync();
                return await connection.QueryAsync<TEntity>(sql, parameters, null, null, commandType) as List<TEntity>;
            }
        }

        public static TEntity QuerySingle<TEntity>(string sql, object parameters = null)
        {
            using (DbConnection connection = new SqlConnection(WebConfigurationManager.DefaultConnectionString))
            {
                connection.Open();
                return (connection.Query<TEntity>(sql, parameters)).FirstOrDefault();
            }
        }

        public static async Task<TEntity> QuerySingleAsync<TEntity>(string sql, object parameters = null)
        {
            using (DbConnection connection = new SqlConnection(WebConfigurationManager.DefaultConnectionString))
            {
                await connection.OpenAsync();
                return (await connection.QueryAsync<TEntity>(sql, parameters)).FirstOrDefault();
            }
        }

        /// <summary>
        /// 执行一条查询语句
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static IEnumerable<TEntity> QueryMany<TEntity>(string sql, object parameters)
        {
            using (DbConnection connection = DbConnectionFactory.Creator())
            {
                connection.Open();
                return connection.Query<TEntity>(sql, parameters);
            }
        }
        #endregion

        #region 获取多个结果集中的数据

        /// <summary>
        /// 获取多个结果集中的数据
        /// </summary>
        /// <typeparam name="TFirst">第一个查询结果类型</typeparam>
        /// <typeparam name="TSecond">第二个查询结果类型</typeparam>
        /// <param name="sql">查询语句</param>
        /// <param name="parameters">参数</param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public static async Task<Tuple<List<TFirst>, List<TSecond>>> QueryMultipleAsync<TFirst, TSecond>(string sql, IEnumerable<DbParameter> parameters = null, CommandType commandType = CommandType.Text)
        {
            using (DbConnection connection = DbConnectionFactory.Creator())
            using (DbCommand command = connection.CreateCommand())
            {
                await connection.OpenAsync();
                if (parameters != null)
                {
                    command.Parameters.AddRange(parameters.ToArray());
                }
                command.CommandType = commandType;
                command.CommandText = sql;

                using (DbDataReader reader = await command.ExecuteReaderAsync())
                {
                    List<TFirst> firstList = new List<TFirst>();
                    List<TSecond> secondList = new List<TSecond>();
                    Func<IDataReader, object> firstFun = null;
                    while (await reader.ReadAsync())
                    {
                        if (firstFun == null)
                        {
                            firstFun = GetDeserializer(typeof(TFirst), reader);
                        }
                        TFirst result = (TFirst)firstFun(reader);
                        firstList.Add(result);
                    }
                    bool isNext = await reader.NextResultAsync();
                    Func<IDataReader, object> secondFun = null;

                    while (await reader.ReadAsync() && isNext)
                    {
                        if (secondFun == null)
                        {
                            secondFun = GetDeserializer(typeof(TSecond), reader);

                        }
                        TSecond result = (TSecond)secondFun(reader);
                        secondList.Add(result);
                    }

                    return Tuple.Create(firstList, secondList);
                }

            }
        }
        #endregion

        #region 执行 DML 语句
        /// <summary>
        /// 执行 DML 语句
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static int ExecuteSqlCommand(string sql, object parameters)
        {
            using (DbConnection connection = new SqlConnection(WebConfigurationManager.DefaultConnectionString))
            {
                connection.Open();
                return connection.Execute(sql, parameters);
            }
        }
        /// <summary>
        /// 执行一条数据库操作语句并返回受影响的行数
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public static int ExecuteSqlCommand(string sql, object parameters, CommandType commandType)
        {
            using (DbConnection connection = new SqlConnection(WebConfigurationManager.DefaultConnectionString))
            {
                connection.Open();
                return connection.Execute(sql, parameters, null, null, commandType);
            }
        }
        /// <summary>
        /// 异步执行一条数据库操作语句并返回受影响的行数
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public static async Task<int> ExecuteSqlCommandAsync(string sql, IEnumerable<DbParameter> parameters = null, CommandType commandType = CommandType.Text)
        {
            using (DbConnection connection = new SqlConnection(WebConfigurationManager.DefaultConnectionString))
            using (DbCommand command = connection.CreateCommand())
            {
                await connection.OpenAsync();
                command.CommandText = sql;
                command.CommandType = commandType;
                if (parameters != null)
                {
                    command.Parameters.AddRange(parameters.ToArray());
                }

                return await command.ExecuteNonQueryAsync();
            }
        }
        #endregion

        #region 执行插入语句并返回最后插入的ID标识
        /// <summary>
        /// 执行插入语句并返回最后插入的ID标识
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public static int ExecuteSqlCommandScalar(string sql, DbParameter[] parameters, CommandType commandType = CommandType.Text)
        {
            using (DbConnection connection = new SqlConnection(WebConfigurationManager.DefaultConnectionString))
            using (DbCommand command = connection.CreateCommand())
            {

                connection.Open();
                command.CommandText = sql;
                if (commandType == CommandType.Text)
                {
                    command.CommandText = command.CommandText + ";SELECT SCOPE_IDENTITY();";
                }
                command.CommandType = commandType;
                command.Parameters.AddRange(parameters);
                return int.Parse(command.ExecuteScalar() + "");
            }
        }
        #endregion

        #region 执行插入语句并返回最后插入的ID标识
        /// <summary>
        /// 执行插入语句并返回最后插入的ID标识
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public static async Task<int> ExecuteSqlCommandScalarAsync(string sql, DbParameter[] parameters, CommandType commandType = CommandType.Text)
        {
            using (DbConnection connection = new SqlConnection(WebConfigurationManager.DefaultConnectionString))
            using (DbCommand command = connection.CreateCommand())
            {

                await connection.OpenAsync();
                command.CommandText = sql;
                if (commandType == CommandType.Text)
                {
                    command.CommandText = command.CommandText + ";SELECT SCOPE_IDENTITY();";
                }
                command.CommandType = commandType;
                command.Parameters.AddRange(parameters);
                return int.Parse(await command.ExecuteScalarAsync() + "");
            }
        }
        #endregion

        #region 值类型转换方法

        public static Func<IDataReader, object> GetDeserializer(Type type, IDataReader reader, int startBound = 0,
            int length = -1, bool returnNullIfMissing = false)
        {
            if (type == typeof(object))
            {
                return GetTypeDeserializer(type, reader);
            }
            if (type.IsEnum == false && type.FullName != "System.Data.Linq.Binary" && type.IsValueType)
            {

                Type underlyingType = Nullable.GetUnderlyingType(type);
                if (underlyingType != null && underlyingType.IsEnum)
                {
                    return GetStructDeserializer(type, underlyingType, startBound);
                }
                else
                {
                    return GetStructDeserializer(type, type, startBound);
                }


            }
            return GetTypeDeserializer(type, reader, startBound, length, returnNullIfMissing);
        }


        /// <summary>
        /// 值类型转换方法
        /// </summary>
        /// <param name="type"></param>
        /// <param name="effectiveType"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static Func<IDataReader, object> GetStructDeserializer(Type type, Type effectiveType, int index)
        {
            if (type == typeof(char))
            {
                return r => (char)r.GetValue(index);
            }
            if (type == typeof(char?))
            {
                return r => (char?)(r.GetValue(index));
            }
            if (type.FullName == "System.Data.Linq.Binary")
            {
                return r => Activator.CreateInstance(type, new object[] { r.GetValue(index) });
            }
            if (!effectiveType.IsEnum)
            {
                return r =>
                {
                    object val = r.GetValue(index);
                    if (!(val is DBNull))
                    {
                        return val;
                    }
                    return null;
                };
            }
            return r =>
            {
                object val = r.GetValue(index);
                if (val is DBNull)
                {
                    return null;
                }
                return Enum.ToObject(effectiveType, val);
            };
        }
        #endregion
    }
}
