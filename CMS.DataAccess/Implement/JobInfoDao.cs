//==============================================================================
//	CAUTION: This file is generated by IBatisNetGen.DaoImpl.cst at 2011/9/14 21:40:03
//				Any manual editing will be lost in re-generation.
//==============================================================================
using System;
using System.Collections.Generic;
using System.Text;
using IBatisNet.DataMapper;
using CMS.Domain;
using CMS.DataAccess.Interface;
using System.Collections;

namespace CMS.DataAccess.Implement {
	
    /// <summary><c>JobInfoDao</c> is the implementation of <see cref="IJobInfoDao"/>.</summary>
    public partial class JobInfoDao : BaseDAO, IJobInfoDao {

		/// <summary>Implements <see cref="IJobInfoDao.GetCount"/></summary>
		public int GetCount() {
			String stmtId = "JobInfo.GetCount";
			int result = SqlMapperManager.Instance.QueryForObject<int>(stmtId, null);
			return result;
		}

		/// <summary>Implements <see cref="IJobInfoDao.Find"/></summary>
		public JobInfo Find(Int32 iD) {
			String stmtId = "JobInfo.Find";
			JobInfo result = SqlMapperManager.Instance.QueryForObject<JobInfo>(stmtId, iD);
			return result;
		}

		/// <summary>Implements <see cref="IJobInfoDao.FindAll"/></summary>
		public IList<JobInfo> FindAll() {
			String stmtId = "JobInfo.FindAll";
			IList<JobInfo> result = SqlMapperManager.Instance.QueryForList<JobInfo>(stmtId, null);
			return result;
		}
		
		/// <summary>Implements <see cref="IJobInfoDao.QuickFindAll"/></summary>
		public IList<JobInfo> QuickFindAll() {
			String stmtId = "JobInfo.QuickFindAll";
			IList<JobInfo> result = SqlMapperManager.Instance.QueryForList<JobInfo>(stmtId, null);
			return result;
		}
		
		/// <summary>Implements <see cref="IJobInfoDao.FindByJobTypeID"/></summary>
		public IList<JobInfo> FindByJobTypeID(Int64 jobTypeID) {
			String stmtId = "JobInfo.FindByJobTypeID";
			IList<JobInfo> result = SqlMapperManager.Instance.QueryForList<JobInfo>(stmtId, jobTypeID);
			return result;
		}
		
		/// <summary>Implements <see cref="IJobInfoDao.FindByName"/></summary>
		public IList<JobInfo> FindByName(String name) {
			String stmtId = "JobInfo.FindByName";
			IList<JobInfo> result = SqlMapperManager.Instance.QueryForList<JobInfo>(stmtId, name);
			return result;
		}
		
		/// <summary>Implements <see cref="IJobInfoDao.FindByEmployeeNumber"/></summary>
		public IList<JobInfo> FindByEmployeeNumber(Int32 employeeNumber) {
			String stmtId = "JobInfo.FindByEmployeeNumber";
			IList<JobInfo> result = SqlMapperManager.Instance.QueryForList<JobInfo>(stmtId, employeeNumber);
			return result;
		}
		
		/// <summary>Implements <see cref="IJobInfoDao.FindByJobDescription"/></summary>
		public IList<JobInfo> FindByJobDescription(String jobDescription) {
			String stmtId = "JobInfo.FindByJobDescription";
			IList<JobInfo> result = SqlMapperManager.Instance.QueryForList<JobInfo>(stmtId, jobDescription);
			return result;
		}
		
		/// <summary>Implements <see cref="IJobInfoDao.FindByResponsbility"/></summary>
		public IList<JobInfo> FindByResponsbility(String responsbility) {
			String stmtId = "JobInfo.FindByResponsbility";
			IList<JobInfo> result = SqlMapperManager.Instance.QueryForList<JobInfo>(stmtId, responsbility);
			return result;
		}
		
		/// <summary>Implements <see cref="IJobInfoDao.FindBySort"/></summary>
		public IList<JobInfo> FindBySort(Int32 sort) {
			String stmtId = "JobInfo.FindBySort";
			IList<JobInfo> result = SqlMapperManager.Instance.QueryForList<JobInfo>(stmtId, sort);
			return result;
		}
		
		/// <summary>Implements <see cref="IJobInfoDao.FindByStatus"/></summary>
		public IList<JobInfo> FindByStatus(Int32 status) {
			String stmtId = "JobInfo.FindByStatus";
			IList<JobInfo> result = SqlMapperManager.Instance.QueryForList<JobInfo>(stmtId, status);
			return result;
		}
		
		/// <summary>Implements <see cref="IJobInfoDao.FindByPublicdate"/></summary>
		public IList<JobInfo> FindByPublicdate(DateTime publicdate) {
			String stmtId = "JobInfo.FindByPublicdate";
			IList<JobInfo> result = SqlMapperManager.Instance.QueryForList<JobInfo>(stmtId, publicdate);
			return result;
		}
		
		/// <summary>Implements <see cref="IJobInfoDao.Insert"/></summary>
		public void Insert(JobInfo obj) {
			if (obj == null) throw new ArgumentNullException("obj");
			String stmtId = "JobInfo.Insert";
			SqlMapperManager.Instance.Insert(stmtId, obj);
		}
		
		/// <summary>Implements <see cref="IJobInfoDao.Update"/></summary>
		public int Update(JobInfo obj) {
			if (obj == null) throw new ArgumentNullException("obj");
			String stmtId = "JobInfo.Update";
			int count=SqlMapperManager.Instance.Update(stmtId, obj);
            return count;
		}
		
		/// <summary>Implements <see cref="IJobInfoDao.Delete"/></summary>
		public int Delete(Int64 id) {
			String stmtId = "JobInfo.Delete";
            Hashtable ht = new Hashtable();
            ht.Add("ID", id);
			int count=SqlMapperManager.Instance.Delete(stmtId, ht);
            return count;
		}
		
		/// <summary>Implements <see cref="IJobInfoDao.DeleteByJobTypeID"/></summary>
		public int DeleteByJobTypeID(Int64 jobTypeID) {
			String stmtId = "JobInfo.DeleteByJobTypeID";
			int result = SqlMapperManager.Instance.Delete(stmtId, jobTypeID);
			return result;
		}
		
		/// <summary>Implements <see cref="IJobInfoDao.DeleteByName"/></summary>
		public int DeleteByName(String name) {
			String stmtId = "JobInfo.DeleteByName";
			int result = SqlMapperManager.Instance.Delete(stmtId, name);
			return result;
		}
		
		/// <summary>Implements <see cref="IJobInfoDao.DeleteByEmployeeNumber"/></summary>
		public int DeleteByEmployeeNumber(Int32 employeeNumber) {
			String stmtId = "JobInfo.DeleteByEmployeeNumber";
			int result = SqlMapperManager.Instance.Delete(stmtId, employeeNumber);
			return result;
		}
		
		/// <summary>Implements <see cref="IJobInfoDao.DeleteByJobDescription"/></summary>
		public int DeleteByJobDescription(String jobDescription) {
			String stmtId = "JobInfo.DeleteByJobDescription";
			int result = SqlMapperManager.Instance.Delete(stmtId, jobDescription);
			return result;
		}
		
		/// <summary>Implements <see cref="IJobInfoDao.DeleteByResponsbility"/></summary>
		public int DeleteByResponsbility(String responsbility) {
			String stmtId = "JobInfo.DeleteByResponsbility";
			int result = SqlMapperManager.Instance.Delete(stmtId, responsbility);
			return result;
		}
		
		/// <summary>Implements <see cref="IJobInfoDao.DeleteBySort"/></summary>
		public int DeleteBySort(Int32 sort) {
			String stmtId = "JobInfo.DeleteBySort";
			int result = SqlMapperManager.Instance.Delete(stmtId, sort);
			return result;
		}
		
		/// <summary>Implements <see cref="IJobInfoDao.DeleteByStatus"/></summary>
		public int DeleteByStatus(Int32 status) {
			String stmtId = "JobInfo.DeleteByStatus";
			int result = SqlMapperManager.Instance.Delete(stmtId, status);
			return result;
		}
		
		/// <summary>Implements <see cref="IJobInfoDao.DeleteByPublicdate"/></summary>
		public int DeleteByPublicdate(DateTime publicdate) {
			String stmtId = "JobInfo.DeleteByPublicdate";
			int result = SqlMapperManager.Instance.Delete(stmtId, publicdate);
			return result;
		}
		
		/// <summary>Implements <see cref="IJobInfoDao.Reload"/></summary>
		public void Reload(JobInfo obj) {
			if (obj == null) throw new ArgumentNullException("obj");
			String stmtId = "JobInfo.Find";
			SqlMapperManager.Instance.QueryForObject<JobInfo>(stmtId, obj, obj);
		}
		
	}

}