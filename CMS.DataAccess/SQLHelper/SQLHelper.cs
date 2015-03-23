using System;
using System.Xml;
using System.Data;
using System.Data.SqlClient;
using System.Data.Common;
using System.Collections;
using System.Configuration;

namespace CMS.DataAccess.SQLHelper
{
    /// <summary>
    /// DAO封装了系统中所有的数据库操作,将数据库连接,事务等进行了统一的管理,为数据库访问提供了方便
    /// 在系统的所有操作中都不应该直接使用SqlConnection这样的连接对象来访问数据库,必须通过该对象提
    /// 供的方法来完成数据库的访问
    /// <remarks>
    /// 
    /// e.g.:
    /// 	///  使用DAO对象简洁的语法为:
    ///     using (com.piao.dao.DAO dao = new com.piao.dao.DAO()){
    ///			//这里使用数据库操作的代码
    ///			dao.ExecuteNoneQuery("delte from abcd");
    ///		}
    ///	 上面的这种语法也可以保证数据库连接的关闭和资源的释放
    ///	 
    /// e.g.:  
    ///  String sql = "select * from abc"
    ///  DAO dao = null;
    ///  DataSet ds = new DataSet();
    ///  DataReader reader = null;
    ///  try{
    ///     dao = new DAO(ConnectionManager.JYDB);
    ///     dao.FillDataset(CommandType.Text, sql,ds);
    ///     dao.BeginTrans();  //开始事务
    ///     
    ///     int count = dao.ExecuteNonQuery("delete from abc where id=123");
    ///     reader = dao.ExecuteReader("select a,b,c,d from abc where class='123'");
    ///     while( reader.Read() ){  //遍历DataReader
    ///			 System.Console.WriteLine(reader["a"]);
    ///			//this.ExecuteNonQuery(CommandType.Text,"update aa set name=@name",reader["a"]); // 永远不要这样写!!!除非外层循环是在对一个DataSet中的数据遍历
    ///     }
    ///     
    ///     dao.CommitTrans(); //提交事务
    ///     
    ///  }catch(Exception ex){
    ///		dao.RollbackTrans();   //回滚事务
    ///     throw ex;
    ///  }finally{
    ///		if ( reader != null ){
    ///			reader.Close();
    ///		}
    ///		if ( dao != null ){
    ///		   dao.Dispose();
    ///		}
    ///  }
    ///  
    /// </remarks>
    /// </summary>
    public class SQLHelper : IDisposable
    {
        private SqlConnection conn;	    //数据操作中使用的数据库连接
        private SqlTransaction trans;   //当开始一个事务时,该对象将被初始化
        private SqlDataAdapter adapter; //用来进行数据库操作的adapter,DAO对象的所有对dataset的操作都通过它来完成
        private bool isDisposed = false;
        private bool isDataAdapterCanClone = false;
        private SqlCommandBuilder builder = null;
        private string defaultConStr = ConfigurationManager.ConnectionStrings["CMSDB"].ConnectionString;
        
        /// <summary>
        /// 使用已存在的DataAdapter生成一个Dao的实例，这样在进行DataSet的操作时
        /// 可以使用该DataAdapter已有的Select,Insert,Update，DeleteCommand
        /// </summary>
        /// <param name="adapter"></param>
        public SQLHelper(SqlDataAdapter adapter, string sqlConStr)
        {
            this.conn = GetSqlConnection(sqlConStr);
            if (adapter == null)
            {
                this.initDataAdapter();
            }
            else
            {
                this.adapter = adapter;
                initDataAdapter();
                isDataAdapterCanClone = true;
            }

        }

        /// <summary>
        /// 计算页面的开始和结束编号
        /// </summary>
        /// <param name="pageNo">页码</param>
        /// <param name="pageSize">页面大小</param>
        /// <param name="startID">返回的开始编号</param>
        /// <param name="endID">返回的结束编号</param>
        public static void GetPageID(int pageNo, int pageSize, ref int startID, ref int endID) {
            startID = (pageNo - 1) * pageSize + 1;
            endID = startID + pageSize - 1; 
        }

        #region 返回某一特定页的数据记录

        
        /// <summary>
        /// 支持SQLServer2005以上版本的分页
        /// </summary>
        /// <param name="pageNo">页码</param>
        /// <param name="pageSize">页面大小</param>
        /// <param name="recourdCount"></param>
        /// <param name="sql">要执行的sql: 如  select * from aa where name = @name and type=@type 不要包含排序字段</param>
        /// <param name="orderByFields">排序字段：example: id asc,type des  ,必须提供，如果没有可以设置为查询表的主键</param>
        /// <param name="sqlParames">可以为空</param>
        /// <returns></returns>
        public DataSet PageingQuery(int pageNo, int pageSize, string sql, string orderByFields,ref int recourdCount, SqlParameter[] sqlParames)
        {

            if (string.IsNullOrEmpty(sql)) {
                throw new Exception("invalid sql statement!");
            }

            sql = sql.Trim();
            string sqlCount = "select count(1) as Total from ( " + sql + ") t";

           
            if (sql.ToLower().StartsWith("select")) {
                sql = sql.Substring("select".Length, sql.Length - "select".Length);
            }

            int startID = 0;
            int endID = 0;
            GetPageID(pageNo, pageSize, ref startID, ref endID);

            SqlParameter spStartID = new SqlParameter("@startID", startID);
            SqlParameter spEndID = new SqlParameter("@endID", endID);

            System.Collections.Generic.List<SqlParameter> spList = new System.Collections.Generic.List<SqlParameter>();
            spList.Add(spStartID);
            spList.Add(spEndID);
            if ( sqlParames != null ){
                foreach (SqlParameter sp in sqlParames) {
                    spList.Add(sp);
                }
            }

            string fullSql = sqlCount + "; select * from ( SELECT row_number() OVER (ORDER BY " + orderByFields + ") as rownum__identy,"
                       + sql   + " ) t   where rownum__identy between @startID and @endID Order By  " + orderByFields;

            DataSet dataSet = null;

            dataSet = ExecuteDataset(CommandType.Text, fullSql,spList.ToArray() );
            recourdCount = Convert.ToInt32(dataSet.Tables[0].Rows[0][0]);
            dataSet.Tables.RemoveAt(0);

            dataSet.Tables[0].Columns.RemoveAt(0);

            return dataSet;
        }



        #endregion

        /// <summary>
        /// SQLServer2000 的分页
        /// </summary>
        /// <param name="TableName">数据库表名</param>
        /// <param name="FieldList">字段列表</param>
        /// <param name="PrimaryKey">单一主键或唯一值键</param>
        /// <param name="Where">查询条件 不含'where'字符，如id>10 and len(userid)>9</param>
        /// <param name="Order">排序 不含'order by'字符，多表连接时也不能包含表名，如id asc,userid desc，必须指定asc或desc,注意当@SortType=3时生效，记住一定要在最后加上主键</param>
        /// <param name="SortType">排序规则 1:正序asc 2:倒序desc 3:多列排序方法</param>
        /// <param name="PageSize">每页记录数</param>
        /// <param name="PageIndex">当前页数</param>
        /// <param name="TotalCount">总记录</param>
        /// <param name="TotalPageCount">总页数</param>
        /// <returns></returns>
        public DataSet PageingQuery(string TableName,string FieldList,string PrimaryKey,string Where,string Order,int SortType,
            int PageSize,int PageIndex,out int TotalCount,out int TotalPageCount)
        {

            SqlParameter[] parameters = new SqlParameter[11];
            parameters[0] = new SqlParameter("@TableName", TableName);
            parameters[1] = new SqlParameter("@FieldList", FieldList);
            parameters[2] = new SqlParameter("@PrimaryKey", PrimaryKey);
            parameters[3] = new SqlParameter("@Where", Where);
            parameters[4] = new SqlParameter("@Order", Order);
            parameters[5] = new SqlParameter("@SortType", SortType);
            parameters[6] = new SqlParameter("@RecorderCount", "0");
            parameters[7] = new SqlParameter("@PageSize", PageSize);
            parameters[8] = new SqlParameter("@PageIndex", PageIndex);
            parameters[9] = new SqlParameter("@TotalCount", SqlDbType.Int);
            parameters[9].Direction = ParameterDirection.Output;
            parameters[10] = new SqlParameter("@TotalPageCount",SqlDbType.Int);
            parameters[10].Direction = ParameterDirection.Output;

            DataSet ds = new DataSet();
            ds = ExecuteDataset(CommandType.StoredProcedure, "UP_PAGERR", parameters);

            Int32.TryParse(parameters[9].Value.ToString(), out TotalCount);
            Int32.TryParse(parameters[10].Value.ToString(), out TotalPageCount);
            return ds;
        }


        /// <summary>
        /// 适用于sql server 2000的分页函数
        /// author: xwarrior @2011/10/
        /// </summary>
        /// <param name="Fields">查询字段列表：如newsdoc.id,newsdoc.channelID, newsdoc.title,newsdoc.Summary,newsdoc.author, newsdoc.smallImageUrl,newsdoc.clickCount,newsdoc.status</param>
        /// <param name="tabnenames">表名列表，如：newsdoc inner join  ChannelInfo on newsdoc.channelID = channelInfo.id</param>
        /// <param name="where">查询条件，不带where,如：contains(newsdoc.title,''广告'') or contains(newsdoc.Summary,''广告'') and newsdoc.IsAuditing = 0 and newsdoc.IsDelete = 0</param>
        /// <param name="querySortFields">排序条件，可以有多个字段，不带order by,如：newsdoc.publictime desc,ChannelInfo.createtime asc</param>
        /// <param name="SortField">查询结果中的主要排序键字段，必须指一个对查找结果中可以代表现顺序的字段，如：publictime</param>
        /// <param name="SortType">查找结果中排序字段的排序方式：desc或asc</param>
        /// <param name="PageSize">页面大小，如：10</param>
        /// <param name="PageIndex">页码：如2</param>
        /// <param name="TotalRecords">int输出的总记录数</param>
        /// <param name="TotalPages">int输出的总页数</param>
        /// <returns></returns>
        public DataSet SQLPagging(string Fields, string tabnenames, string where, string querySortFields, string SortField, string SortType,
            int PageSize, int PageIndex, out int TotalRecords, out int TotalPages)
        {

            SqlParameter[] parameters = new SqlParameter[11];
            parameters[0] = new SqlParameter("@Fields", Fields);
            parameters[1] = new SqlParameter("@tabnenames", tabnenames);
            parameters[2] = new SqlParameter("@where", where);
            parameters[3] = new SqlParameter("@querySortFields", querySortFields);
            parameters[4] = new SqlParameter("@SortField", SortField);
            parameters[5] = new SqlParameter("@SortType", SortType);
            parameters[6] = new SqlParameter("@PageSize", PageSize);
            parameters[7] = new SqlParameter("@PageIndex", PageIndex);

            parameters[8] = new SqlParameter("@TotalRecords", SqlDbType.Int);
            parameters[8].Direction = ParameterDirection.Output;
 
            parameters[9] = new SqlParameter("@TotalPages", SqlDbType.Int);
            parameters[9].Direction = ParameterDirection.Output;

            DataSet ds = new DataSet();
            ds = ExecuteDataset(CommandType.StoredProcedure, "UP_SQLPagging", parameters);

            Int32.TryParse(parameters[8].Value.ToString(), out TotalRecords);
            Int32.TryParse(parameters[9].Value.ToString(), out TotalPages);
            return ds;
        }
        

        /// <summary>
        /// current opend connection instance
        /// </summary>
        public SqlConnection Connection {
            get {
                return this.conn;
            }
        }

        /// <summary>
        /// 使用指定的连接字符串创建一个DAO对象
        /// </summary>
        /// <param name="sqlConStr"></param>
        public SQLHelper(String sqlConStr)
        {
            this.conn = GetSqlConnection(sqlConStr);
            this.adapter = new SqlDataAdapter();
        }

        /// <summary>
        /// 使用指定的连接字符串创建一个DAO对象
        /// </summary>
        /// <param name="sqlConStr"></param>
        public SQLHelper()
        {
            this.conn = GetSqlConnection(defaultConStr);
            this.adapter = new SqlDataAdapter();
        }

        /// <summary>
        /// 使用指定的数据库连接创建一个DAO对象
        /// </summary>
        /// <param name="conn"></param>
        public SQLHelper(SqlConnection conn)
        {
            this.conn = conn;
            this.adapter = new SqlDataAdapter();
        }


        private SqlDataAdapter getDataAdapter()
        {
            if (this.adapter == null)
            {
                this.initDataAdapter();
            }
            return this.adapter;
        }

        /// <summary>
        /// 以默认的事务级别开始一个事务
        /// </summary>
        public void BeginTrans()
        {
            if (this.trans != null)
            {
                throw new InvalidOperationException(" 事务已经启动,不能重新开始!");
            }
            this.trans = this.conn.BeginTransaction(IsolationLevel.ReadCommitted);
        }

        /// <summary>
        /// 以指定的事务级别和事务名称开始一个事务
        /// </summary>
        /// <param name="isoLevel"></param>
        /// <param name="transationName"></param>
        public void BeginTrans(IsolationLevel isoLevel, String transationName)
        {
            if (this.trans != null)
            {
                throw new InvalidOperationException(" 事务已经启动,不能重新开始!");
            }
            this.trans = this.conn.BeginTransaction(isoLevel, transationName);
        }

        /// <summary>
        /// 提交已开始的事务
        /// </summary>
        public void CommitTrans()
        {
            if (this.trans == null)
            {
                throw new InvalidOperationException("没有启动事务,因此不需要提交!");
            }
            this.trans.Commit();
            this.trans = null;
        }

        /// <summary>
        /// 回滚事务
        /// </summary>
        public void RollbackTrans()
        {
            if (this.trans == null)
            {
                throw new InvalidOperationException("没有启动事务,不能进行回滚!");
            }
            this.trans.Rollback();
        }

        /// <summary>
        /// 回滚开始的事务
        /// </summary>
        /// <param name="transactionName"></param>
        public void RollbackTrans(String transactionName)
        {
            if (this.trans == null)
            {
                throw new InvalidOperationException("没有启动事务,不能进行回滚!");
            }
            this.trans.Rollback(transactionName);
        }

        #region IDisposable 成员
        /// <summary>
        /// 释放数据库访问对象占用的资源,在使用完DAO对象以后,必须在finally块中调用该方法来释放资源
        /// </summary>
        public void Dispose()
        {
            if (this.isDisposed) return;
            try
            {
                if (this.trans != null && this.trans.Connection != null)
                {
                    this.trans.Commit();
                    this.trans.Dispose();
                }
            }
            catch (Exception)
            {
                //提交事务失败也要关闭连接
                //throw;
            }
           
            if (this.conn != null && this.conn.State != ConnectionState.Closed)
            {
                this.conn.Close();
            }
            if (this.adapter != null)
            {
                if (this.adapter.SelectCommand != null)
                {
                    this.adapter.SelectCommand.Dispose();
                }
                if (this.adapter.UpdateCommand != null)
                {
                    this.adapter.UpdateCommand.Dispose();
                }
                if (this.adapter.DeleteCommand != null)
                {
                    this.adapter.DeleteCommand.Dispose();
                }
                if (this.adapter.InsertCommand != null)
                {
                    this.adapter.InsertCommand.Dispose();
                }
                this.adapter.Dispose();
            }
            GC.SuppressFinalize(this);
            this.isDisposed = true;
        }

        #endregion

        #region ExecuteNonQuery


        /// <summary>
        /// 执行一条无返回结果的sql,返回受影响的数据行数
        /// </summary>
        /// <remarks>
        /// e.g.:  
        ///  int result = ExecuteNonQuery("delete form a where id=1");
        /// </remarks>
        /// <param name="commandType"></param>
        /// <param name="commandText"></param>
        /// <returns></returns>
        public int ExecuteNonQuery(string commandText)
        {
            // Pass through the call providing null for the set of SqlParameters
            return ExecuteNonQuery(CommandType.Text, commandText);
        }

        /// <summary>
        /// 使用指定的命令类型执行一条无返回结果的sql,返回受影响的数据行数
        /// </summary>
        /// <remarks>
        /// e.g.:  
        ///  int result = ExecuteNonQuery(CommandType.Text, "delete form a where id=1");
        /// </remarks>
        /// <param name="commandType"></param>
        /// <param name="commandText"></param>
        /// <returns></returns>
        public int ExecuteNonQuery(CommandType commandType, string commandText)
        {
            // Pass through the call providing null for the set of SqlParameters
            return ExecuteNonQuery(commandType, commandText, (SqlParameter[])null);
        }

        /// <summary>
        /// Execute a SqlCommand (that returns no resultset) against the specified SqlConnection 
        /// using the provided parameters.
        /// </summary>
        /// <remarks>
        /// e.g.:  
        ///  int result = ExecuteNonQuery(CommandType.Text, "PublishOrders", new SqlParameter("@prodid", 24));
        /// </remarks>
        /// <param name="connection">A valid SqlConnection</param>
        /// <param name="commandType">The CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">The stored procedure name or T-SQL command</param>
        /// <param name="commandParameters">An array of SqlParamters used to execute the command</param>
        /// <returns>An int representing the number of rows affected by the command</returns>
        public int ExecuteNonQuery(CommandType commandType, string commandText, params SqlParameter[] commandParameters)
        {
            // Create a command and prepare it for execution
            SqlCommand cmd = new SqlCommand();
            PrepareCommand(cmd, commandType, commandText, commandParameters);

            // Finally, execute the command
            int retval = cmd.ExecuteNonQuery();

            // Detach the SqlParameters from the command object, so they can be used again
            cmd.Parameters.Clear();
            isDataAdapterCanClone = false;
            return retval;
        }

        /// <summary>
        /// Execute a stored procedure via a SqlCommand (that returns no resultset) against the specified SqlConnection 
        /// using the provided parameter values.  This method will query the database to discover the parameters for the 
        /// stored procedure (the first time each stored procedure is called), and assign the values based on parameter order.
        /// </summary>
        /// <remarks>
        /// This method provides no access to output parameters or the stored procedure's return value parameter.
        /// 
        /// e.g.:  
        ///  int result = ExecuteNonQuery(conn, "PublishOrders", 24, 36);
        /// </remarks>
        /// <param name="connection">A valid SqlConnection</param>
        /// <param name="spName">The name of the stored procedure</param>
        /// <param name="parameterValues">An array of objects to be assigned as the input values of the stored procedure</param>
        /// <returns>An int representing the number of rows affected by the command</returns>
        public int ExecuteNonQuery(string spName, params object[] parameterValues)
        {
            if (spName == null || spName.Length == 0) throw new ArgumentNullException("spName");
            isDataAdapterCanClone = false;
            // If we receive parameter values, we need to figure out where they go
            if ((parameterValues != null) && (parameterValues.Length > 0))
            {
                // Pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
                SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(this.conn, spName);

                // Assign the provided values to these parameters based on parameter order
                AssignParameterValues(commandParameters, parameterValues);

                // Call the overload that takes an array of SqlParameters
                return ExecuteNonQuery(CommandType.Text, spName, commandParameters);
            }
            else
            {
                // Otherwise we can just call the SP without params
                return ExecuteNonQuery(CommandType.Text, spName);
            }
        }

        #endregion ExecuteNonQuery

        #region ExecuteDataset


        /// <summary>
        /// Execute a SqlCommand (that returns a resultset and takes no parameters) against the provided SqlConnection. 
        /// </summary>
        /// <remarks>
        /// e.g.:  
        ///  DataSet ds = ExecuteDataset(conn, CommandType.Text, "GetOrders");
        /// </remarks>
        /// <param name="connection">A valid SqlConnection</param>
        /// <param name="commandType">The CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">The stored procedure name or T-SQL command</param>
        /// <returns>A dataset containing the resultset generated by the command</returns>
        public DataSet ExecuteDataset(CommandType commandType, string commandText)
        {
            // Pass through the call providing null for the set of SqlParameters
            return ExecuteDataset(commandType, false, commandText);
        }

        /// <summary>
        /// Execute a SqlCommand (that returns a resultset and takes no parameters) against the provided SqlConnection. 
        /// </summary>
        /// <remarks>
        /// e.g.:  
        ///  DataSet ds = ExecuteDataset(conn, CommandType.Text, "GetOrders");
        /// </remarks>
        /// <param name="connection">A valid SqlConnection</param>
        /// <param name="commandType">The CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">The stored procedure name or T-SQL command</param>
        /// <returns>A dataset containing the resultset generated by the command</returns>
        public DataSet ExecuteDataset(CommandType commandType, bool isBuildCommands, string commandText)
        {
            // Pass through the call providing null for the set of SqlParameters
            return ExecuteDataset(commandType, isBuildCommands, commandText, (SqlParameter[])null);
        }

        /// <summary>
        /// Execute a SqlCommand (that returns a resultset) against the specified SqlConnection 
        /// using the provided parameters.
        /// </summary>
        /// <remarks>
        /// e.g.:  
        ///  DataSet ds = ExecuteDataset(conn, CommandType.Text, "GetOrders", new SqlParameter("@prodid", 24));
        /// </remarks>
        /// <param name="connection">A valid SqlConnection</param>
        /// <param name="commandType">The CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">The stored procedure name or T-SQL command</param>
        /// <param name="commandParameters">An array of SqlParamters used to execute the command</param>
        /// <returns>A dataset containing the resultset generated by the command</returns>
        public DataSet ExecuteDataset(CommandType commandType, string commandText, params SqlParameter[] commandParameters)
        {
            return ExecuteDataset(commandType, false, commandText, commandParameters);
        }


        /// <summary>
        /// Execute a SqlCommand (that returns a resultset) against the specified SqlConnection 
        /// using the provided parameters.
        /// </summary>
        /// <remarks>
        /// e.g.:  
        ///  DataSet ds = ExecuteDataset(conn, CommandType.Text, "GetOrders", new SqlParameter("@prodid", 24));
        /// </remarks>
        /// <param name="connection">A valid SqlConnection</param>
        /// <param name="commandType">The CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">The stored procedure name or T-SQL command</param>
        /// <param name="commandParameters">An array of SqlParamters used to execute the command</param>
        /// <returns>A dataset containing the resultset generated by the command</returns>
        public DataSet ExecuteDataset(CommandType commandType, bool isBuildCommands, string commandText, params SqlParameter[] commandParameters)
        {
            // Create a command and prepare it for execution
            SqlCommand cmd = new SqlCommand();

            PrepareCommand(cmd, commandType, commandText, commandParameters);

            // Create the DataAdapter & DataSet
            this.getDataAdapter().SelectCommand = cmd;

            DataSet ds = new DataSet();

            // Fill the DataSet using default values for DataTable names, etc
            this.getDataAdapter().Fill(ds);

            // Detach the SqlParameters from the command object, so they can be used again
            isDataAdapterCanClone = isBuildCommands;
            if (isBuildCommands)
            {
                this.BuildCommand();
            }
            cmd.Parameters.Clear();
            // Return the dataset
            return ds;

        }


        /// <summary>
        /// Execute a stored procedure via a SqlCommand (that returns a resultset) against the specified 
        /// SqlTransaction using the provided parameter values.  This method will query the database to discover the parameters for the 
        /// stored procedure (the first time each stored procedure is called), and assign the values based on parameter order.
        /// </summary>
        /// <remarks>
        /// This method provides no access to output parameters or the stored procedure's return value parameter.
        /// 
        /// e.g.:  
        ///  DataSet ds = ExecuteDataset(trans, "GetOrders", 24, 36);
        /// </remarks>
        /// <param name="transaction">A valid SqlTransaction</param>
        /// <param name="spName">The name of the stored procedure</param>
        /// <param name="parameterValues">An array of objects to be assigned as the input values of the stored procedure</param>
        /// <returns>A dataset containing the resultset generated by the command</returns>
        public DataSet ExecuteDataset(string spName, params object[] parameterValues)
        {
            return ExecuteDataset(spName, false, parameterValues);
        }


        /// <summary>
        /// Execute a stored procedure via a SqlCommand (that returns a resultset) against the specified 
        /// SqlTransaction using the provided parameter values.  This method will query the database to discover the parameters for the 
        /// stored procedure (the first time each stored procedure is called), and assign the values based on parameter order.
        /// </summary>
        /// <remarks>
        /// This method provides no access to output parameters or the stored procedure's return value parameter.
        /// 
        /// e.g.:  
        ///  DataSet ds = ExecuteDataset(trans, "GetOrders", 24, 36);
        /// </remarks>
        /// <param name="transaction">A valid SqlTransaction</param>
        /// <param name="spName">The name of the stored procedure</param>
        /// <param name="parameterValues">An array of objects to be assigned as the input values of the stored procedure</param>
        /// <returns>A dataset containing the resultset generated by the command</returns>
        public DataSet ExecuteDataset(string spName, bool isBuildCommands, params object[] parameterValues)
        {

            if (spName == null || spName.Length == 0) throw new ArgumentNullException("spName");

            DataSet ds = null;
            // If we receive parameter values, we need to figure out where they go
            if ((parameterValues != null) && (parameterValues.Length > 0))
            {
                // Pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
                SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(this.conn, spName);

                // Assign the provided values to these parameters based on parameter order
                AssignParameterValues(commandParameters, parameterValues);

                // Call the overload that takes an array of SqlParameters
                ds = ExecuteDataset(CommandType.Text, isBuildCommands, spName, commandParameters);
            }
            else
            {
                // Otherwise we can just call the SP without params
                ds = ExecuteDataset(CommandType.Text, isBuildCommands, spName);
            }
            isDataAdapterCanClone = isBuildCommands;
            if (isBuildCommands)
            {
                this.BuildCommand();
            }
            return ds;
        }

        #endregion ExecuteDataset

        #region ExecuteReader

        /// <summary>
        /// 执行一条sql语句，并返回对应的SqlDataReader
        /// </summary>
        /// <remarks>
        /// You must close the related reader by Dispose dao
        /// </remarks>
        /// <param name="commandText">The stored procedure name or T-SQL command</param>
        /// <returns>SqlDataReader containing the results of the command</returns>
        public SqlDataReader ExecuteReader(string commandText)
        {
            return ExecuteReader(CommandType.Text, false, commandText, null);
        }

        /// <summary>
        /// 执行一条sql语句，并返回对应的SqlDataReader
        /// </summary>
        /// <remarks>
        /// You must close the related reader by Dispose dao
        /// </remarks>
        /// <param name="commandText">The stored procedure name or T-SQL command</param>
        /// <param name="isBuildCommands">在执行该命名后是否创建该次执行的Sql对应的Insert,Update,delete Command</param>
        /// <returns>SqlDataReader containing the results of the command</returns>
        public SqlDataReader ExecuteReader(string commandText, bool isBuildCommands)
        {
            return ExecuteReader(CommandType.Text, isBuildCommands, commandText, null);
        }


        /// <summary>
        ///执行一条sql语句，并返回SqlDataReader
        /// </summary>
        /// <remarks>
        /// You must close the related reader by Dispose dao
        /// </remarks>
        /// <param name="commandText">The stored procedure name or T-SQL command</param>
        /// <param name="commandParameters">An array of SqlParameters to be associated with the command or 'null' if no parameters are required</param>
        /// <param name="connectionOwnership">Indicates whether the connection parameter was provided by the caller, or created by SqlHelper</param>
        /// <returns>SqlDataReader containing the results of the command</returns>
        public SqlDataReader ExecuteReader(string commandText, SqlParameter[] commandParameters)
        {
            return ExecuteReader(commandText, false, commandParameters);
        }

        /// <summary>
        ///执行一条sql语句，并返回SqlDataReader
        /// </summary>
        /// <remarks>
        /// You must close the related reader by Dispose dao
        /// </remarks>
        /// <param name="commandText">The stored procedure name or T-SQL command</param>
        ///<param name="isBuildCommands">在执行该命名后是否创建该次执行的Sql对应的Insert,Update,delete Command</param>
        /// <param name="commandParameters">An array of SqlParameters to be associated with the command or 'null' if no parameters are required</param>
        /// <param name="connectionOwnership">Indicates whether the connection parameter was provided by the caller, or created by SqlHelper</param>
        /// <returns>SqlDataReader containing the results of the command</returns>
        public SqlDataReader ExecuteReader(string commandText, bool isBuildCommands, SqlParameter[] commandParameters)
        {
            return ExecuteReader(CommandType.Text, isBuildCommands, commandText, commandParameters);
        }


        /// <summary>
        /// Create and prepare a SqlCommand, and call ExecuteReader with the appropriate CommandBehavior.
        /// </summary>
        /// <remarks>
        /// You must close the related reader by Dispose dao
        /// </remarks>
        /// <param name="commandType">The CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">The stored procedure name or T-SQL command</param>
        /// <param name="commandParameters">An array of SqlParameters to be associated with the command or 'null' if no parameters are required</param>
        /// <param name="connectionOwnership">Indicates whether the connection parameter was provided by the caller, or created by SqlHelper</param>
        /// <returns>SqlDataReader containing the results of the command</returns>
        public SqlDataReader ExecuteReader(CommandType commandType, string commandText, SqlParameter[] commandParameters)
        {
            return ExecuteReader(commandType, false, commandText, commandParameters);
        }

        /// <summary>
        /// Create and prepare a SqlCommand, and call ExecuteReader with the appropriate CommandBehavior.
        /// </summary>
        /// <remarks>
        /// You must close the related reader by Dispose dao
        /// </remarks>
        /// <param name="commandType">The CommandType (stored procedure, text, etc.)</param>
        /// <param name="isBuildCommands">在执行该命名后是否创建该次执行的Sql对应的Insert,Update,delete Command</param>
        /// <param name="commandText">The stored procedure name or T-SQL command</param>
        /// <param name="commandParameters">An array of SqlParameters to be associated with the command or 'null' if no parameters are required</param>
        /// <param name="connectionOwnership">Indicates whether the connection parameter was provided by the caller, or created by SqlHelper</param>
        /// <returns>SqlDataReader containing the results of the command</returns>
        public SqlDataReader ExecuteReader(CommandType commandType, bool isBuildCommands, string commandText, SqlParameter[] commandParameters)
        {
            // Create a command and prepare it for execution
            SqlCommand cmd = new SqlCommand();
            SqlDataReader dataReader = null;
            try
            {
                PrepareCommand(cmd, commandType, commandText, commandParameters);
                // Create a reader
                dataReader = cmd.ExecuteReader();
                bool canClear = true;
                foreach (SqlParameter commandParameter in cmd.Parameters)
                {
                    if (commandParameter.Direction != ParameterDirection.Input)
                        canClear = false;
                }

                if (canClear)
                {
                    cmd.Parameters.Clear();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            isDataAdapterCanClone = isBuildCommands;
            if (isBuildCommands)
            {
                this.BuildCommand();
            }
            return dataReader;
        }


        /// <summary>
        /// Execute a SqlCommand (that returns a resultset and takes no parameters) against the provided SqlConnection. 
        /// </summary>
        /// <remarks>
        /// e.g.:  
        ///  SqlDataReader dr = ExecuteReader(conn, CommandType.Text, "GetOrders");
        /// </remarks>
        /// <param name="connection">A valid SqlConnection</param>
        /// <param name="commandType">The CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">The stored procedure name or T-SQL command</param>
        /// <returns>A SqlDataReader containing the resultset generated by the command</returns>
        public SqlDataReader ExecuteReader(CommandType commandType, string commandText)
        {
            return ExecuteReader(commandType, false, commandText);
        }

        /// <summary>
        /// Execute a SqlCommand (that returns a resultset and takes no parameters) against the provided SqlConnection. 
        /// </summary>
        /// <remarks>
        /// e.g.:  
        ///  SqlDataReader dr = ExecuteReader(conn, CommandType.Text, "GetOrders");
        /// </remarks>
        /// <param name="connection">A valid SqlConnection</param>
        /// <param name="commandType">The CommandType (stored procedure, text, etc.)</param>
        /// <param name="isBuildCommands">在执行该命名后是否创建该次执行的Sql对应的Insert,Update,delete Command</param>
        /// <param name="commandText">The stored procedure name or T-SQL command</param>
        /// <returns>A SqlDataReader containing the resultset generated by the command</returns>
        public SqlDataReader ExecuteReader(CommandType commandType, bool isBuildCommands, string commandText)
        {
            // Pass through the call providing null for the set of SqlParameters
            return ExecuteReader(commandType, isBuildCommands, commandText, (SqlParameter[])null);
        }


        /// <summary>
        /// Execute a stored procedure via a SqlCommand (that returns a resultset) against the specified
        /// SqlTransaction using the provided parameter values.  This method will query the database to discover the parameters for the 
        /// stored procedure (the first time each stored procedure is called), and assign the values based on parameter order.
        /// </summary>
        /// <remarks>
        /// This method provides no access to output parameters or the stored procedure's return value parameter.
        /// 
        /// e.g.:  
        ///  SqlDataReader dr = ExecuteReader(trans, "GetOrders", 24, 36);
        /// </remarks>
        /// <param name="transaction">A valid SqlTransaction</param>
        /// <param name="spName">The name of the stored procedure</param>
        /// <param name="parameterValues">An array of objects to be assigned as the input values of the stored procedure</param>
        /// <returns>A SqlDataReader containing the resultset generated by the command</returns>
        public SqlDataReader ExecuteReader(string spName, params object[] parameterValues)
        {
            return ExecuteReader(spName, false, parameterValues);
        }

        /// <summary>
        /// Execute a stored procedure via a SqlCommand (that returns a resultset) against the specified
        /// SqlTransaction using the provided parameter values.  This method will query the database to discover the parameters for the 
        /// stored procedure (the first time each stored procedure is called), and assign the values based on parameter order.
        /// </summary>
        /// <remarks>
        /// This method provides no access to output parameters or the stored procedure's return value parameter.
        /// 
        /// e.g.:  
        ///  SqlDataReader dr = ExecuteReader(trans, "GetOrders", 24, 36);
        /// </remarks>
        /// <param name="transaction">A valid SqlTransaction</param>
        /// <param name="spName">The name of the stored procedure</param>
        /// <param name="isBuildCommands">在执行该命名后是否创建该次执行的Sql对应的Insert,Update,delete Command</param>
        /// <param name="parameterValues">An array of objects to be assigned as the input values of the stored procedure</param>
        /// <returns>A SqlDataReader containing the resultset generated by the command</returns>
        public SqlDataReader ExecuteReader(string spName, bool isBuildCommands, params object[] parameterValues)
        {
            if (spName == null || spName.Length == 0) throw new ArgumentNullException("spName");

            SqlDataReader reader = null;
            // If we receive parameter values, we need to figure out where they go
            if ((parameterValues != null) && (parameterValues.Length > 0))
            {
                SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(this.conn, spName);

                AssignParameterValues(commandParameters, parameterValues);

                reader = ExecuteReader(CommandType.Text, spName, commandParameters);
            }
            else
            {
                // Otherwise we can just call the SP without params
                reader = ExecuteReader(CommandType.Text, spName);
            }
            isDataAdapterCanClone = isBuildCommands;
            if (isBuildCommands)
            {
                this.BuildCommand();
            }
            return reader;
        }

        #endregion ExecuteReader

        #region ExecuteScalar

        /// <summary>
        /// Execute a SqlCommand (that returns a 1x1 resultset and takes no parameters) against the provided SqlTransaction. 
        /// </summary>
        /// <remarks>
        /// e.g.:  
        ///  int orderCount = (int)ExecuteScalar(trans, CommandType.Text, "GetOrderCount");
        /// </remarks>
        /// <param name="transaction">A valid SqlTransaction</param>
        /// <param name="commandType">The CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">The stored procedure name or T-SQL command</param>
        /// <returns>An object containing the value in the 1x1 resultset generated by the command</returns>
        public object ExecuteScalar(CommandType commandType, string commandText)
        {
            // Pass through the call providing null for the set of SqlParameters
            return ExecuteScalar(commandType, commandText, (SqlParameter[])null);
        }

        /// <summary>
        /// Execute a SqlCommand (that returns a 1x1 resultset) against the specified SqlTransaction
        /// using the provided parameters.
        /// </summary>
        /// <remarks>
        /// e.g.:  
        ///  int orderCount = (int)ExecuteScalar(trans, CommandType.Text, "GetOrderCount", new SqlParameter("@prodid", 24));
        /// </remarks>
        /// <param name="transaction">A valid SqlTransaction</param>
        /// <param name="commandType">The CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">The stored procedure name or T-SQL command</param>
        /// <param name="commandParameters">An array of SqlParamters used to execute the command</param>
        /// <returns>An object containing the value in the 1x1 resultset generated by the command</returns>
        public object ExecuteScalar(CommandType commandType, string commandText, params SqlParameter[] commandParameters)
        {
            // Create a command and prepare it for execution
            SqlCommand cmd = new SqlCommand();
            PrepareCommand(cmd, commandType, commandText, commandParameters);

            // Execute the command & return the results
            object retval = cmd.ExecuteScalar();

            // Detach the SqlParameters from the command object, so they can be used again
            cmd.Parameters.Clear();
            isDataAdapterCanClone = false;
            return retval;
        }


        /// <summary>
        /// Execute a stored procedure via a SqlCommand (that returns a 1x1 resultset) against the specified
        /// SqlTransaction using the provided parameter values.  This method will query the database to discover the parameters for the 
        /// stored procedure (the first time each stored procedure is called), and assign the values based on parameter order.
        /// </summary>
        /// <remarks>
        /// This method provides no access to output parameters or the stored procedure's return value parameter.
        /// 
        /// e.g.:  
        ///  int orderCount = (int)ExecuteScalar(trans, "GetOrderCount", 24, 36);
        /// </remarks>
        /// <param name="transaction">A valid SqlTransaction</param>
        /// <param name="spName">The name of the stored procedure</param>
        /// <param name="parameterValues">An array of objects to be assigned as the input values of the stored procedure</param>
        /// <returns>An object containing the value in the 1x1 resultset generated by the command</returns>
        public object ExecuteScalar(string spName, params object[] parameterValues)
        {
            if (spName == null || spName.Length == 0) throw new ArgumentNullException("spName");

            // If we receive parameter values, we need to figure out where they go
            if ((parameterValues != null) && (parameterValues.Length > 0))
            {
                // PPull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
                SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(this.conn, spName);

                // Assign the provided values to these parameters based on parameter order
                AssignParameterValues(commandParameters, parameterValues);

                // Call the overload that takes an array of SqlParameters
                return ExecuteScalar(CommandType.Text, spName, commandParameters);
            }
            else
            {
                // Otherwise we can just call the SP without params
                return ExecuteScalar(CommandType.Text, spName);
            }
        }

        #endregion ExecuteScalar

        #region ExecuteXmlReader
        /// <summary>
        /// Execute a SqlCommand (that returns a resultset and takes no parameters) against the provided SqlConnection. 
        /// </summary>
        /// <remarks>
        /// e.g.:  
        ///  XmlReader r = ExecuteXmlReader(conn, CommandType.Text, "GetOrders");
        /// </remarks>
        /// <param name="connection">A valid SqlConnection</param>
        /// <param name="commandType">The CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">The stored procedure name or T-SQL command using "FOR XML AUTO"</param>
        /// <returns>An XmlReader containing the resultset generated by the command</returns>
        public XmlReader ExecuteXmlReader(CommandType commandType, string commandText)
        {
            // Pass through the call providing null for the set of SqlParameters
            return ExecuteXmlReader(commandType, commandText, (SqlParameter[])null);
        }

        /// <summary>
        /// Execute a SqlCommand (that returns a resultset) against the specified SqlConnection 
        /// using the provided parameters.
        /// </summary>
        /// <remarks>
        /// e.g.:  
        ///  XmlReader r = ExecuteXmlReader(conn, CommandType.Text, "GetOrders", new SqlParameter("@prodid", 24));
        /// </remarks>
        /// <param name="connection">A valid SqlConnection</param>
        /// <param name="commandType">The CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">The stored procedure name or T-SQL command using "FOR XML AUTO"</param>
        /// <param name="commandParameters">An array of SqlParamters used to execute the command</param>
        /// <returns>An XmlReader containing the resultset generated by the command</returns>
        public XmlReader ExecuteXmlReader(CommandType commandType, string commandText, params SqlParameter[] commandParameters)
        {

            // Create a command and prepare it for execution
            SqlCommand cmd = new SqlCommand();
            PrepareCommand(cmd, commandType, commandText, commandParameters);
            // Create the DataAdapter & DataSet
            XmlReader retval = cmd.ExecuteXmlReader();

            // Detach the SqlParameters from the command object, so they can be used again
            cmd.Parameters.Clear();
            isDataAdapterCanClone = false;
            return retval;
        }

        /// <summary>
        /// Execute a stored procedure via a SqlCommand (that returns a resultset) against the specified SqlConnection 
        /// using the provided parameter values.  This method will query the database to discover the parameters for the 
        /// stored procedure (the first time each stored procedure is called), and assign the values based on parameter order.
        /// </summary>
        /// <remarks>
        /// This method provides no access to output parameters or the stored procedure's return value parameter.
        /// 
        /// e.g.:  
        ///  XmlReader r = ExecuteXmlReader(conn, "GetOrders", 24, 36);
        /// </remarks>
        /// <param name="connection">A valid SqlConnection</param>
        /// <param name="spName">The name of the stored procedure using "FOR XML AUTO"</param>
        /// <param name="parameterValues">An array of objects to be assigned as the input values of the stored procedure</param>
        /// <returns>An XmlReader containing the resultset generated by the command</returns>
        public XmlReader ExecuteXmlReader(string spName, params object[] parameterValues)
        {
            if (spName == null || spName.Length == 0) throw new ArgumentNullException("spName");

            // If we receive parameter values, we need to figure out where they go
            if ((parameterValues != null) && (parameterValues.Length > 0))
            {
                // Pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
                SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(this.conn, spName);

                // Assign the provided values to these parameters based on parameter order
                AssignParameterValues(commandParameters, parameterValues);

                // Call the overload that takes an array of SqlParameters
                return ExecuteXmlReader(CommandType.Text, spName, commandParameters);
            }
            else
            {
                // Otherwise we can just call the SP without params
                return ExecuteXmlReader(CommandType.Text, spName);
            }
        }

        /// <summary>
        /// Execute a SqlCommand (that returns a resultset and takes no parameters) against the provided SqlTransaction. 
        /// </summary>
        /// <remarks>
        /// e.g.:  
        ///  XmlReader r = ExecuteXmlReader(trans, CommandType.Text, "GetOrders");
        /// </remarks>
        /// <param name="transaction">A valid SqlTransaction</param>
        /// <param name="commandType">The CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">The stored procedure name or T-SQL command using "FOR XML AUTO"</param>
        /// <returns>An XmlReader containing the resultset generated by the command</returns>
        #endregion ExecuteXmlReader

        #region FillDataset


        /// <summary>
        /// Execute a SqlCommand (that returns a resultset and takes no parameters) against the provided SqlConnection. 
        /// </summary>
        /// <remarks>
        /// e.g.:  
        ///  FillDataset("GetOrders", ds);
        /// </remarks>
        /// <param name="commandText">The stored procedure name or T-SQL command</param>
        /// <param name="dataSet">A dataset wich will contain the resultset generated by the command</param>
        /// <param name="tableNames">This array will be used to create table mappings allowing the DataTables to be referenced
        /// by a user defined name (probably the actual table name)
        /// </param>    
        public void FillDataset(string commandText, DataSet dataSet)
        {
            FillDataset(commandText, false, dataSet);
        }

        /// <summary>
        /// Execute a SqlCommand (that returns a resultset and takes no parameters) against the provided SqlConnection. 
        /// </summary>
        /// <remarks>
        /// e.g.:  
        ///  FillDataset("GetOrders", ds);
        /// </remarks>
        /// <param name="commandText">The stored procedure name or T-SQL command</param>
        /// <param name="dataSet">A dataset wich will contain the resultset generated by the command</param>
        /// <param name="tableNames">This array will be used to create table mappings allowing the DataTables to be referenced
        /// by a user defined name (probably the actual table name)
        /// </param>    
        public void FillDataset(string commandText, bool isBuildCommands, DataSet dataSet)
        {
            FillDataset(CommandType.Text, isBuildCommands, commandText, dataSet, null);
        }


        /// <summary>
        /// Execute a SqlCommand (that returns a resultset and takes no parameters) against the provided SqlConnection. 
        /// </summary>
        /// <remarks>
        /// e.g.:  
        ///  FillDataset "GetOrders", ds, new string[] {"orders"});
        /// </remarks>
        /// <param name="commandText">The stored procedure name or T-SQL command</param>
        /// <param name="dataSet">A dataset wich will contain the resultset generated by the command</param>
        /// <param name="tableNames">This array will be used to create table mappings allowing the DataTables to be referenced
        /// by a user defined name (probably the actual table name)
        /// </param>    
        public void FillDataset(string commandText, DataSet dataSet, string[] tableNames)
        {
            FillDataset(commandText, false, dataSet, tableNames);
        }

        /// <summary>
        /// Execute a SqlCommand (that returns a resultset and takes no parameters) against the provided SqlConnection. 
        /// </summary>
        /// <remarks>
        /// e.g.:  
        ///  FillDataset "GetOrders", ds, new string[] {"orders"});
        /// </remarks>
        /// <param name="commandText">The stored procedure name or T-SQL command</param>
        /// <param name="dataSet">A dataset wich will contain the resultset generated by the command</param>
        /// <param name="tableNames">This array will be used to create table mappings allowing the DataTables to be referenced
        /// by a user defined name (probably the actual table name)
        /// </param>    
        public void FillDataset(string commandText, bool isBuildCommands, DataSet dataSet, string[] tableNames)
        {
            FillDataset(CommandType.Text, isBuildCommands, commandText, dataSet, tableNames);
        }

        /// <summary>
        /// Execute a SqlCommand (that returns a resultset and takes no parameters) against the provided SqlConnection. 
        /// </summary>
        /// <remarks>
        /// e.g.:  
        ///  FillDataset(CommandType.Text, "GetOrders", ds, new string[] {"orders"});
        /// </remarks>
        /// <param name="commandType">The CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">The stored procedure name or T-SQL command</param>
        /// <param name="dataSet">A dataset wich will contain the resultset generated by the command</param>
        /// <param name="tableNames">This array will be used to create table mappings allowing the DataTables to be referenced
        /// by a user defined name (probably the actual table name)
        /// </param>    
        public void FillDataset(CommandType commandType, string commandText, DataSet dataSet, string[] tableNames)
        {
            FillDataset(commandType, false, commandText, dataSet, tableNames);
        }

        /// <summary>
        /// Execute a SqlCommand (that returns a resultset and takes no parameters) against the provided SqlConnection. 
        /// </summary>
        /// <remarks>
        /// e.g.:  
        ///  FillDataset(CommandType.Text, "GetOrders", ds, new string[] {"orders"});
        /// </remarks>
        /// <param name="commandType">The CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">The stored procedure name or T-SQL command</param>
        /// <param name="dataSet">A dataset wich will contain the resultset generated by the command</param>
        /// <param name="tableNames">This array will be used to create table mappings allowing the DataTables to be referenced
        /// by a user defined name (probably the actual table name)
        /// </param>    
        public void FillDataset(CommandType commandType, bool isBuildCommands,
            string commandText, DataSet dataSet, string[] tableNames)
        {
            FillDataset(commandType, isBuildCommands, commandText, dataSet, tableNames, null);
        }


        /// <summary>
        /// Execute a stored procedure via a SqlCommand (that returns a resultset) against the specified SqlConnection 
        /// using the provided parameter values.  This method will query the database to discover the parameters for the 
        /// stored procedure (the first time each stored procedure is called), and assign the values based on parameter order.
        /// </summary>
        /// <remarks>
        /// This method provides no access to output parameters or the stored procedure's return value parameter.
        /// 
        /// e.g.:  
        ///  FillDataset(conn, "GetOrders", ds, new string[] {"orders"}, 24, 36);
        /// </remarks>
        /// <param name="connection">A valid SqlConnection</param>
        /// <param name="spName">The name of the stored procedure</param>
        /// <param name="dataSet">A dataset wich will contain the resultset generated by the command</param>
        /// <param name="tableNames">This array will be used to create table mappings allowing the DataTables to be referenced
        /// by a user defined name (probably the actual table name)
        /// </param>
        /// <param name="parameterValues">An array of objects to be assigned as the input values of the stored procedure</param>
        public void FillDataset(string spName, DataSet dataSet, string[] tableNames, params object[] parameterValues)
        {
            FillDataset(spName, false, dataSet, tableNames, parameterValues);
        }

        /// <summary>
        /// Execute a stored procedure via a SqlCommand (that returns a resultset) against the specified SqlConnection 
        /// using the provided parameter values.  This method will query the database to discover the parameters for the 
        /// stored procedure (the first time each stored procedure is called), and assign the values based on parameter order.
        /// </summary>
        /// <remarks>
        /// This method provides no access to output parameters or the stored procedure's return value parameter.
        /// 
        /// e.g.:  
        ///  FillDataset(conn, "GetOrders", ds, new string[] {"orders"}, 24, 36);
        /// </remarks>
        /// <param name="connection">A valid SqlConnection</param>
        /// <param name="spName">The name of the stored procedure</param>
        /// <param name="dataSet">A dataset wich will contain the resultset generated by the command</param>
        /// <param name="tableNames">This array will be used to create table mappings allowing the DataTables to be referenced
        /// by a user defined name (probably the actual table name)
        /// </param>
        /// <param name="parameterValues">An array of objects to be assigned as the input values of the stored procedure</param>
        public void FillDataset(string spName, bool isBuildCommands,
            DataSet dataSet, string[] tableNames,
            params object[] parameterValues)
        {
            if (dataSet == null) throw new ArgumentNullException("dataSet");
            if (spName == null || spName.Length == 0) throw new ArgumentNullException("spName");

            // If we receive parameter values, we need to figure out where they go
            if ((parameterValues != null) && (parameterValues.Length > 0))
            {
                // Pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
                SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(this.conn, spName);

                // Assign the provided values to these parameters based on parameter order
                AssignParameterValues(commandParameters, parameterValues);

                // Call the overload that takes an array of SqlParameters
                FillDataset(CommandType.Text, isBuildCommands, spName, dataSet, tableNames, commandParameters);
            }
            else
            {
                // Otherwise we can just call the SP without params
                FillDataset(CommandType.Text, isBuildCommands, spName, dataSet, tableNames);
            }
        }


        /// <summary>
        /// Private helper method that execute a SqlCommand (that returns a resultset) against the specified SqlTransaction and SqlConnection
        /// using the provided parameters.
        /// </summary>
        /// <remarks>
        /// e.g.:  
        ///  FillDataset(conn, trans, CommandType.Text, "GetOrders", ds, new string[] {"orders"}, new SqlParameter("@prodid", 24));
        /// </remarks>
        /// <param name="commandType">The CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">The stored procedure name or T-SQL command</param>
        /// <param name="dataSet">A dataset wich will contain the resultset generated by the command</param>
        /// <param name="tableNames">This array will be used to create table mappings allowing the DataTables to be referenced
        /// by a user defined name (probably the actual table name)
        /// </param>
        /// <param name="commandParameters">An array of SqlParamters used to execute the command</param>
        public void FillDataset(CommandType commandType,
            string commandText, DataSet dataSet, string[] tableNames,
            params SqlParameter[] commandParameters)
        {
            FillDataset(commandType, false, commandText, dataSet, tableNames, commandParameters);
        }


        /// <summary>
        /// Private helper method that execute a SqlCommand (that returns a resultset) against the specified SqlTransaction and SqlConnection
        /// using the provided parameters.
        /// </summary>
        /// <remarks>
        /// e.g.:  
        ///  FillDataset(conn, trans, CommandType.Text, "GetOrders", ds, new string[] {"orders"}, new SqlParameter("@prodid", 24));
        /// </remarks>
        /// <param name="commandType">The CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">The stored procedure name or T-SQL command</param>
        /// <param name="dataSet">A dataset wich will contain the resultset generated by the command</param>
        /// <param name="tableNames">This array will be used to create table mappings allowing the DataTables to be referenced
        /// by a user defined name (probably the actual table name)
        /// </param>
        /// <param name="commandParameters">An array of SqlParamters used to execute the command</param>
        private void FillDataset(CommandType commandType, bool isBuildCommands,
            string commandText, DataSet dataSet, string[] tableNames,
            params SqlParameter[] commandParameters)
        {

            if (dataSet == null) throw new ArgumentNullException("dataSet");

            // Create a command and prepare it for execution
            SqlCommand command = new SqlCommand();
            PrepareCommand(command, commandType, commandText, commandParameters);

            // Create the DataAdapter & DataSet
            this.adapter.SelectCommand = command;
            // Add the table mappings specified by the user
            if (tableNames != null && tableNames.Length > 0)
            {
                string tableName = "Table";
                for (int index = 0; index < tableNames.Length; index++)
                {
                    if (tableNames[index] == null || tableNames[index].Length == 0) throw new ArgumentException("The tableNames parameter must contain a list of tables, a value was provided as null or empty string.", "tableNames");
                    this.adapter.TableMappings.Add(tableName, tableNames[index]);
                    tableName = "Table" + (index + 1).ToString();
                }
            }

            // Fill the DataSet using default values for DataTable names, etc
            this.adapter.Fill(dataSet);
            isDataAdapterCanClone = isBuildCommands;
            if (isBuildCommands)
            {
                this.BuildCommand();
            }
            // Detach the SqlParameters from the command object, so they can be used again
            command.Parameters.Clear();
        }


        /// <summary>
        /// sujianhui 增加（用于返回分页的查询记录）
        /// </summary>
        /// <param name="commandType"></param>
        /// <param name="commandText"></param>
        /// <param name="dataSet">要用记录和架构（如果必要）填充的 System.Data.DataSet</param>
        /// <param name="startRecord">从其开始的从零开始的记录号</param>
        /// <param name="maxRecords">要检索的最大记录数</param>
        /// <param name="srcTable">用于表映射的源表的名称</param>
        /// <param name="commandParameters"></param>
        public void FillDataset(CommandType commandType, string commandText,
                                    DataSet dataSet, int startRecord, int maxRecords,
                                    string srcTable, params SqlParameter[] commandParameters)
        {
            SqlCommand command = new SqlCommand();

            this.PrepareCommand(command, commandType, commandText, commandParameters);

            // Create the DataAdapter
            this.adapter.SelectCommand = command;

            this.adapter.Fill(dataSet, startRecord, maxRecords, srcTable);
        }
        #endregion

        #region UpdateDataset
        /// <summary>
        /// Executes the respective command for each inserted, updated, or deleted row in the DataSet.
        /// </summary>
        /// <remarks>
        /// e.g.:  
        ///  UpdateDataset(conn, insertCommand, deleteCommand, updateCommand, dataSet, "Order");
        /// </remarks>
        /// <param name="insertCommand">A valid transact-SQL statement or stored procedure to insert new records into the data source</param>
        /// <param name="deleteCommand">A valid transact-SQL statement or stored procedure to delete records from the data source</param>
        /// <param name="updateCommand">A valid transact-SQL statement or stored procedure used to update records in the data source</param>
        /// <param name="dataSet">The DataSet used to update the data source</param>
        /// <param name="tableName">The DataTable used to update the data source.</param>
        public void UpdateDataset(SqlCommand insertCommand, SqlCommand deleteCommand, SqlCommand updateCommand, DataSet dataSet, string tableName)
        {
            if (insertCommand == null) throw new ArgumentNullException("insertCommand");
            if (deleteCommand == null) throw new ArgumentNullException("deleteCommand");
            if (updateCommand == null) throw new ArgumentNullException("updateCommand");
            if (tableName == null || tableName.Length == 0) throw new ArgumentNullException("tableName");

            // Set the data adapter commands
            this.adapter.UpdateCommand = updateCommand;
            this.adapter.InsertCommand = insertCommand;
            this.adapter.DeleteCommand = deleteCommand;
            DataSet tempds = dataSet.GetChanges();
            if (tempds != null && tempds.Tables[tableName].Rows.Count > 0)
            {
                // Update the dataset changes in the data source
                this.adapter.Update(tempds, tableName);

                // Commit all the changes made to the DataSet
                dataSet.AcceptChanges();
            }
            this.isDataAdapterCanClone = true;

        }

        /// <summary>
        /// 将DataSet的值使用当前Dao对象的dataAdapter的命令对象更新到数据库中
        /// 调用该方法时，请确保使用了正确的DataAdapter,否则会出现意想不到的结果
        /// </summary>
        /// <param name="ds">要保存的DataSet</param>
        public void UpdateDataset(DataSet ds, String tableName)
        {
            DataSet tempds = ds.GetChanges();
            if (tempds != null && tempds.Tables[tableName].Rows.Count > 0)
            {
                this.adapter.Update(tempds, tableName);
                ds.AcceptChanges();
                this.isDataAdapterCanClone = true;
            }
        }
        #endregion

        #region CreateCommand
        /// <summary>
        /// Simplify the creation of a Sql command object by allowing
        /// a stored procedure and optional parameters to be provided
        /// </summary>
        /// <remarks>
        /// e.g.:  
        ///  SqlCommand command = CreateCommand("AddCustomer", "CustomerID", "CustomerName");
        /// </remarks>
        /// <param name="spName">The name of the stored procedure</param>
        /// <param name="sourceColumns">An array of string to be assigned as the source columns of the stored procedure parameters</param>
        /// <returns>A valid SqlCommand object</returns>
        public SqlCommand CreateCommand(string spName, params string[] sourceColumns)
        {

            if (spName == null || spName.Length == 0) throw new ArgumentNullException("spName");

            // Create a SqlCommand
            SqlCommand cmd = new SqlCommand(spName, this.conn);
            cmd.CommandType = CommandType.Text;

            // If we receive parameter values, we need to figure out where they go
            if ((sourceColumns != null) && (sourceColumns.Length > 0))
            {
                // Pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
                SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(this.conn, spName);

                // Assign the provided source columns to these parameters based on parameter order
                for (int index = 0; index < sourceColumns.Length; index++)
                    commandParameters[index].SourceColumn = sourceColumns[index];

                // Attach the discovered parameters to the SqlCommand object
                AttachParameters(cmd, commandParameters);
            }

            return cmd;
        }


        /// <summary>
        /// 使用指定的sql创建一个SqlCommand
        /// </summary>
        /// <param name="Sql"></param>
        /// <returns></returns>
        public SqlCommand CreateCommand(string Sql)
        {
            SqlCommand cmd = null;
            if (this.trans == null)
            {
                cmd = new SqlCommand(Sql, this.conn);
            }
            else
            {
                cmd = new SqlCommand(Sql, this.conn, this.trans);
            }
            return cmd;
        }

        /// <summary>
        /// 根据一条SQL自动生成当前的Dao对象对应的DataAdapter对象的InsertCommand,UpdateCommand，和DeleteCommand
        /// </summary>
        /// <param name="SelectSql"></param>
        private void BuildCommand()
        {
            isDataAdapterCanClone = true;
            builder = new SqlCommandBuilder(this.getDataAdapter());
            this.adapter.InsertCommand = builder.GetInsertCommand();
            this.adapter.UpdateCommand = builder.GetUpdateCommand();
            this.adapter.DeleteCommand = builder.GetDeleteCommand();
        }

        #endregion

        #region ExecuteNonQueryTypedParams
        /// <summary>
        /// Execute a stored procedure via a SqlCommand (that returns no resultset) against the specified SqlConnection 
        /// using the dataRow column values as the stored procedure's parameters values.  
        /// This method will query the database to discover the parameters for the 
        /// stored procedure (the first time each stored procedure is called), and assign the values based on row values.
        /// </summary>
        /// <param name="spName">The name of the stored procedure</param>
        /// <param name="dataRow">The dataRow used to hold the stored procedure's parameter values.</param>
        /// <returns>An int representing the number of rows affected by the command</returns>
        public int ExecuteNonQueryTypedParams(String spName, DataRow dataRow)
        {
            if (spName == null || spName.Length == 0) throw new ArgumentNullException("spName");

            // If the row has values, the store procedure parameters must be initialized
            if (dataRow != null && dataRow.ItemArray.Length > 0)
            {
                // Pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
                SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(this.conn, spName);

                // Set the parameters values
                AssignParameterValues(commandParameters, dataRow);

                return ExecuteNonQuery(CommandType.Text, spName, commandParameters);
            }
            else
            {
                return ExecuteNonQuery(CommandType.Text, spName);
            }
        }
        #endregion

        #region ExecuteDatasetTypedParams

        /// <summary>
        /// Execute a stored procedure via a SqlCommand (that returns a resultset) against the specified SqlConnection 
        /// using the dataRow column values as the store procedure's parameters values.
        /// This method will query the database to discover the parameters for the 
        /// stored procedure (the first time each stored procedure is called), and assign the values based on row values.
        /// </summary>
        /// <param name="connection">A valid SqlConnection object</param>
        /// <param name="spName">The name of the stored procedure</param>
        /// <param name="dataRow">The dataRow used to hold the stored procedure's parameter values.</param>
        /// <returns>A dataset containing the resultset generated by the command</returns>
        public DataSet ExecuteDatasetTypedParams(SqlConnection connection, String spName, DataRow dataRow)
        {
            if (spName == null || spName.Length == 0) throw new ArgumentNullException("spName");

            // If the row has values, the store procedure parameters must be initialized
            if (dataRow != null && dataRow.ItemArray.Length > 0)
            {
                // Pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
                SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(this.conn, spName);

                // Set the parameters values
                AssignParameterValues(commandParameters, dataRow);

                return ExecuteDataset(CommandType.Text, spName, commandParameters);
            }
            else
            {
                return ExecuteDataset(CommandType.Text, spName);
            }
        }


        #endregion

        #region ExecuteReaderTypedParams

        /// <summary>
        /// Execute a stored procedure via a SqlCommand (that returns a resultset) against the specified SqlConnection 
        /// using the dataRow column values as the stored procedure's parameters values.
        /// This method will query the database to discover the parameters for the 
        /// stored procedure (the first time each stored procedure is called), and assign the values based on parameter order.
        /// </summary>
        /// <param name="spName">The name of the stored procedure</param>
        /// <param name="dataRow">The dataRow used to hold the stored procedure's parameter values.</param>
        /// <returns>A SqlDataReader containing the resultset generated by the command</returns>
        public SqlDataReader ExecuteReaderTypedParams(String spName, DataRow dataRow)
        {
            if (spName == null || spName.Length == 0) throw new ArgumentNullException("spName");

            // If the row has values, the store procedure parameters must be initialized
            if (dataRow != null && dataRow.ItemArray.Length > 0)
            {
                // Pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
                SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(this.conn, spName);

                // Set the parameters values
                AssignParameterValues(commandParameters, dataRow);

                return ExecuteReader(CommandType.Text, spName, commandParameters);
            }
            else
            {
                return ExecuteReader(CommandType.Text, spName);
            }
        }

        #endregion

        #region ExecuteScalarTypedParams

        /// <summary>
        /// Execute a stored procedure via a SqlCommand (that returns a 1x1 resultset) against the specified SqlConnection 
        /// using the dataRow column values as the stored procedure's parameters values.
        /// This method will query the database to discover the parameters for the 
        /// stored procedure (the first time each stored procedure is called), and assign the values based on parameter order.
        /// </summary>
        /// <param name="spName">The name of the stored procedure</param>
        /// <param name="dataRow">The dataRow used to hold the stored procedure's parameter values.</param>
        /// <returns>An object containing the value in the 1x1 resultset generated by the command</returns>
        public object ExecuteScalarTypedParams(String spName, DataRow dataRow)
        {
            if (spName == null || spName.Length == 0) throw new ArgumentNullException("spName");

            // If the row has values, the store procedure parameters must be initialized
            if (dataRow != null && dataRow.ItemArray.Length > 0)
            {
                // Pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
                SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(this.conn, spName);

                // Set the parameters values
                AssignParameterValues(commandParameters, dataRow);

                return ExecuteScalar(CommandType.Text, spName, commandParameters);
            }
            else
            {
                return ExecuteScalar(CommandType.Text, spName);
            }
        }

        #endregion

        #region ExecuteXmlReaderTypedParams
        /// <summary>
        /// Execute a stored procedure via a SqlCommand (that returns a resultset) against the specified SqlConnection 
        /// using the dataRow column values as the stored procedure's parameters values.
        /// This method will query the database to discover the parameters for the 
        /// stored procedure (the first time each stored procedure is called), and assign the values based on parameter order.
        /// </summary>
        /// <param name="spName">The name of the stored procedure</param>
        /// <param name="dataRow">The dataRow used to hold the stored procedure's parameter values.</param>
        /// <returns>An XmlReader containing the resultset generated by the command</returns>
        public XmlReader ExecuteXmlReaderTypedParams(String spName, DataRow dataRow)
        {
            if (spName == null || spName.Length == 0) throw new ArgumentNullException("spName");

            // If the row has values, the store procedure parameters must be initialized
            if (dataRow != null && dataRow.ItemArray.Length > 0)
            {
                // Pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
                SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(this.conn, spName);

                // Set the parameters values
                AssignParameterValues(commandParameters, dataRow);

                return ExecuteXmlReader(CommandType.Text, spName, commandParameters);
            }
            else
            {
                return ExecuteXmlReader(CommandType.Text, spName);
            }
        }

        #endregion

        /// <summary>
        /// 使用指定的字符串获得数据库连接
        /// </summary>
        /// <param name="sqlConstr"></param>
        /// <returns></returns>
        private SqlConnection GetSqlConnection(String sqlConstr)
        {
            SqlConnection conn = new SqlConnection(sqlConstr);
            conn.Open();
            return conn;
        }

        /// <summary>
        /// 获得Dao对象当前使用的DataAdapter
        /// 要Clone 当前的Adapter，必须在执行一个包启了表的主键的Select操作之后,并且必须是立即调用，否则无法Clone
        /// 或者刚刚使用DAO对象执行完了一个UpdateDataSet方法之后才可以调用该Clone的方法。
        /// </summary>
        public SqlDataAdapter ClonedDataAdapter
        {
            get
            {
                SqlDataAdapter ad = null;
                if (isDataAdapterCanClone)
                {
                    ad = (SqlDataAdapter)((ICloneable)this.getDataAdapter()).Clone();
                    if (ad != null)
                    {
                        if (this.getDataAdapter().SelectCommand != null)
                        {
                            ad.SelectCommand = (SqlCommand)((ICloneable)this.getDataAdapter().SelectCommand).Clone();
                        }

                        if (this.getDataAdapter().InsertCommand != null)
                        {
                            ad.InsertCommand = (SqlCommand)((ICloneable)this.getDataAdapter().InsertCommand).Clone();
                        }

                        if (this.getDataAdapter().UpdateCommand != null)
                        {
                            ad.UpdateCommand = (SqlCommand)((ICloneable)this.getDataAdapter().UpdateCommand).Clone();
                        }

                        if (this.getDataAdapter().DeleteCommand != null)
                        {
                            ad.DeleteCommand = (SqlCommand)((ICloneable)this.getDataAdapter().DeleteCommand).Clone();
                        }
                    }
                }
                else
                {
                    throw new InvalidOperationException("要Clone 当前的Adapter，必须在执行一个包启了表的主键的Select操作之后,并且必须是立即调用，否则无法Clone!");
                }
                return ad;
            }
            set
            {
                this.isDataAdapterCanClone = true;
                this.adapter = value;
                this.initDataAdapter();
            }
        }

        /// <summary>
        /// 初始化由外面传入的DataAdapter,并更新该dataAdapter的数据库连接为当前dao的连接
        /// </summary>
        private void initDataAdapter()
        {
            if (this.adapter != null)
            {
                if (this.adapter.SelectCommand != null)
                {
                    this.adapter.SelectCommand.Connection = this.conn;
                }
                if (this.adapter.UpdateCommand != null)
                {
                    this.adapter.UpdateCommand.Connection = this.conn;
                }

                if (this.adapter.InsertCommand != null)
                {
                    this.adapter.InsertCommand.Connection = this.conn;
                }

                if (this.adapter.DeleteCommand != null)
                {
                    this.adapter.DeleteCommand.Connection = this.conn;
                }
            }
            else
            {
                this.adapter = new SqlDataAdapter();
            }
        }

        /// <summary>
        /// 批量保存
        /// </summary>
        /// <param name="dtData"></param>
        public void GoBulkCopy(DataTable dtData,string destTableName)
        {
            // 建立一个SqlBulkCopy 对象以便执行大量复制作业
            SqlBulkCopy bcp = null;
            try
            {
                if (this.trans != null)
                {
                    bcp = new SqlBulkCopy(this.conn, SqlBulkCopyOptions.KeepIdentity, this.trans);
                }
                else
                {
                    bcp = new SqlBulkCopy(this.conn);
                }

                // 指定目标数据表的名称。
                bcp.DestinationTableName = destTableName;
                bcp.WriteToServer(dtData);

            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
                if (bcp != null)
                {
                    bcp.Close();
                }
            }
        }

        #region private utility methods

        /// <summary>
        /// This method is used to attach array of SqlParameters to a SqlCommand.
        /// 
        /// This method will assign a value of DbNull to any parameter with a direction of
        /// InputOutput and a value of null.  
        /// 
        /// This behavior will prevent default values from being used, but
        /// this will be the less common case than an intended pure output parameter (derived as InputOutput)
        /// where the user provided no input value.
        /// </summary>
        /// <param name="command">The command to which the parameters will be added</param>
        /// <param name="commandParameters">An array of SqlParameters to be added to command</param>
        private void AttachParameters(SqlCommand command, SqlParameter[] commandParameters)
        {
            if (command == null) throw new ArgumentNullException("command");
            if (commandParameters != null)
            {
                foreach (SqlParameter p in commandParameters)
                {
                    if (p != null)
                    {
                        // Check for derived output value with no value assigned
                        if ((p.Direction == ParameterDirection.InputOutput ||
                            p.Direction == ParameterDirection.Input) &&
                            (p.Value == null))
                        {
                            p.Value = DBNull.Value;
                        }
                        command.Parameters.Add(p);
                    }
                }
            }
        }

        /// <summary>
        /// This method assigns dataRow column values to an array of SqlParameters
        /// </summary>
        /// <param name="commandParameters">Array of SqlParameters to be assigned values</param>
        /// <param name="dataRow">The dataRow used to hold the stored procedure's parameter values</param>
        private void AssignParameterValues(SqlParameter[] commandParameters, DataRow dataRow)
        {
            if ((commandParameters == null) || (dataRow == null))
            {
                // Do nothing if we get no data
                return;
            }

            int i = 0;
            // Set the parameters values
            foreach (SqlParameter commandParameter in commandParameters)
            {
                // Check the parameter name
                if (commandParameter.ParameterName == null ||
                    commandParameter.ParameterName.Length <= 1)
                    throw new Exception(
                        string.Format(
                        "Please provide a valid parameter name on the parameter #{0}, the ParameterName property has the following value: '{1}'.",
                        i, commandParameter.ParameterName));
                if (dataRow.Table.Columns.IndexOf(commandParameter.ParameterName.Substring(1)) != -1)
                    commandParameter.Value = dataRow[commandParameter.ParameterName.Substring(1)];
                i++;
            }
        }

        /// <summary>
        /// This method assigns an array of values to an array of SqlParameters
        /// </summary>
        /// <param name="commandParameters">Array of SqlParameters to be assigned values</param>
        /// <param name="parameterValues">Array of objects holding the values to be assigned</param>
        private void AssignParameterValues(SqlParameter[] commandParameters, object[] parameterValues)
        {
            if ((commandParameters == null) || (parameterValues == null))
            {
                // Do nothing if we get no data
                return;
            }

            // We must have the same number of values as we pave parameters to put them in
            if (commandParameters.Length != parameterValues.Length)
            {
                throw new ArgumentException("Parameter count does not match Parameter Value count.");
            }

            // Iterate through the SqlParameters, assigning the values from the corresponding position in the 
            // value array
            for (int i = 0, j = commandParameters.Length; i < j; i++)
            {
                // If the current array value derives from IDbDataParameter, then assign its Value property
                if (parameterValues[i] is IDbDataParameter)
                {
                    IDbDataParameter paramInstance = (IDbDataParameter)parameterValues[i];
                    if (paramInstance.Value == null)
                    {
                        commandParameters[i].Value = DBNull.Value;
                    }
                    else
                    {
                        commandParameters[i].Value = paramInstance.Value;
                    }
                }
                else if (parameterValues[i] == null)
                {
                    commandParameters[i].Value = DBNull.Value;
                }
                else
                {
                    commandParameters[i].Value = parameterValues[i];
                }
            }
        }

        /// <summary>
        /// This method opens (if necessary) and assigns a connection, transaction, command type and parameters 
        /// to the provided command
        /// </summary>
        /// <param name="command">The SqlCommand to be prepared</param>
        /// <param name="connection">A valid SqlConnection, on which to execute this command</param>
        /// <param name="transaction">A valid SqlTransaction, or 'null'</param>
        /// <param name="commandType">The CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">The stored procedure name or T-SQL command</param>
        /// <param name="commandParameters">An array of SqlParameters to be associated with the command or 'null' if no parameters are required</param>
        /// <param name="mustCloseConnection"><c>true</c> if the connection was opened by the method, otherwose is false.</param>
        private void PrepareCommand(SqlCommand command, CommandType commandType, string commandText, SqlParameter[] commandParameters)
        {

            if (command == null) throw new ArgumentNullException("command");
            if (commandText == null || commandText.Length == 0) throw new ArgumentNullException("commandText");

            // If the provided connection is not open, we will open it
            if (this.conn.State != ConnectionState.Open)
            {
                this.conn.Open();
            }

            // Associate the connection with the command
            command.Connection = this.conn;
            command.CommandTimeout = 0;
            // Set the command text (stored procedure name or SQL statement)
            command.CommandText = commandText;

            // If we were provided a transaction, assign it
            if (this.trans != null)
            {
                if (this.trans.Connection == null) throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");
                command.Transaction = this.trans;
            }

            // Set the command type
            command.CommandType = commandType;

            // Attach the command parameters if they are provided
            if (commandParameters != null)
            {
                AttachParameters(command, commandParameters);
            }
            return;
        }

        #endregion private utility methods & constructors
    }
}
