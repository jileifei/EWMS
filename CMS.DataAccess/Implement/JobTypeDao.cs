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

namespace CMS.DataAccess.Implement {
	
    /// <summary><c>JobTypeDao</c> is the implementation of <see cref="IJobTypeDao"/>.</summary>
    public partial class JobTypeDao : BaseDAO, IJobTypeDao {

		/// <summary>Implements <see cref="IJobTypeDao.GetCount"/></summary>
		public int GetCount() {
			String stmtId = "JobType.GetCount";
			int result = SqlMapperManager.Instance.QueryForObject<int>(stmtId, null);
			return result;
		}

		/// <summary>Implements <see cref="IJobTypeDao.Find"/></summary>
		public JobType Find(Int64 iD) {
			String stmtId = "JobType.Find";
			JobType result = SqlMapperManager.Instance.QueryForObject<JobType>(stmtId, iD);
			return result;
		}

		/// <summary>Implements <see cref="IJobTypeDao.FindAll"/></summary>
		public IList<JobType> FindAll() {
			String stmtId = "JobType.FindAll";
			IList<JobType> result = SqlMapperManager.Instance.QueryForList<JobType>(stmtId, null);
			return result;
		}
		
		/// <summary>Implements <see cref="IJobTypeDao.QuickFindAll"/></summary>
		public IList<JobType> QuickFindAll() {
			String stmtId = "JobType.QuickFindAll";
			IList<JobType> result = SqlMapperManager.Instance.QueryForList<JobType>(stmtId, null);
			return result;
		}
		
		/// <summary>Implements <see cref="IJobTypeDao.FindByName"/></summary>
		public IList<JobType> FindByName(String name) {
			String stmtId = "JobType.FindByName";
			IList<JobType> result = SqlMapperManager.Instance.QueryForList<JobType>(stmtId, name);
			return result;
		}
		
		/// <summary>Implements <see cref="IJobTypeDao.FindByDescription"/></summary>
		public IList<JobType> FindByDescription(String description) {
			String stmtId = "JobType.FindByDescription";
			IList<JobType> result = SqlMapperManager.Instance.QueryForList<JobType>(stmtId, description);
			return result;
		}
		
		/// <summary>Implements <see cref="IJobTypeDao.Insert"/></summary>
		public void Insert(JobType obj) {
			if (obj == null) throw new ArgumentNullException("obj");
			String stmtId = "JobType.Insert";
			SqlMapperManager.Instance.Insert(stmtId, obj);
		}
		
		/// <summary>Implements <see cref="IJobTypeDao.Update"/></summary>
		public int Update(JobType obj) {
			if (obj == null) throw new ArgumentNullException("obj");
			String stmtId = "JobType.Update";
			int count=SqlMapperManager.Instance.Update(stmtId, obj);
            return count;
		}
		
		/// <summary>Implements <see cref="IJobTypeDao.Delete"/></summary>
		public void Delete(JobType obj) {
			if (obj == null) throw new ArgumentNullException("obj");
			String stmtId = "JobType.Delete";
			SqlMapperManager.Instance.Delete(stmtId, obj);
		}
		
		/// <summary>Implements <see cref="IJobTypeDao.DeleteByName"/></summary>
		public int DeleteByName(String name) {
			String stmtId = "JobType.DeleteByName";
			int result = SqlMapperManager.Instance.Delete(stmtId, name);
			return result;
		}
		
		/// <summary>Implements <see cref="IJobTypeDao.DeleteByDescription"/></summary>
		public int DeleteByDescription(String description) {
			String stmtId = "JobType.DeleteByDescription";
			int result = SqlMapperManager.Instance.Delete(stmtId, description);
			return result;
		}
		
		/// <summary>Implements <see cref="IJobTypeDao.Reload"/></summary>
		public void Reload(JobType obj) {
			if (obj == null) throw new ArgumentNullException("obj");
			String stmtId = "JobType.Find";
			SqlMapperManager.Instance.QueryForObject<JobType>(stmtId, obj, obj);
		}
		
	}

}