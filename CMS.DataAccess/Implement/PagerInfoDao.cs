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
	
    /// <summary><c>PagerInfoDao</c> is the implementation of <see cref="IPagerInfoDao"/>.</summary>
    public partial class PagerInfoDao : IPagerInfoDao {

		/// <summary>Implements <see cref="IPagerInfoDao.GetCount"/></summary>
		public int GetCount() {
			String stmtId = "PagerInfo.GetCount";
			int result = SqlMapperManager.Instance.QueryForObject<int>(stmtId, null);
			return result;
		}

		/// <summary>Implements <see cref="IPagerInfoDao.Find"/></summary>
		public PagerInfo Find(Int32 pageID) {
			String stmtId = "PagerInfo.Find";
			PagerInfo result = SqlMapperManager.Instance.QueryForObject<PagerInfo>(stmtId, pageID);
			return result;
		}

		/// <summary>Implements <see cref="IPagerInfoDao.FindAll"/></summary>
		public IList<PagerInfo> FindAll() {
			String stmtId = "PagerInfo.FindAll";
			IList<PagerInfo> result = SqlMapperManager.Instance.QueryForList<PagerInfo>(stmtId, null);
			return result;
		}
		
		/// <summary>Implements <see cref="IPagerInfoDao.QuickFindAll"/></summary>
		public IList<PagerInfo> QuickFindAll() {
			String stmtId = "PagerInfo.QuickFindAll";
			IList<PagerInfo> result = SqlMapperManager.Instance.QueryForList<PagerInfo>(stmtId, null);
			return result;
		}
		
		/// <summary>Implements <see cref="IPagerInfoDao.FindByDataType"/></summary>
		public IList<PagerInfo> FindByDataType(Int32? dataType) {
			String stmtId = "PagerInfo.FindByDataType";
			IList<PagerInfo> result = SqlMapperManager.Instance.QueryForList<PagerInfo>(stmtId, dataType);
			return result;
		}
		
		/// <summary>Implements <see cref="IPagerInfoDao.FindByWhere"/></summary>
		public IList<PagerInfo> FindByWhere(String where) {
			String stmtId = "PagerInfo.FindByWhere";
			IList<PagerInfo> result = SqlMapperManager.Instance.QueryForList<PagerInfo>(stmtId, where);
			return result;
		}
		
		/// <summary>Implements <see cref="IPagerInfoDao.FindByOrderBy"/></summary>
		public IList<PagerInfo> FindByOrderBy(String orderBy) {
			String stmtId = "PagerInfo.FindByOrderBy";
			IList<PagerInfo> result = SqlMapperManager.Instance.QueryForList<PagerInfo>(stmtId, orderBy);
			return result;
		}
		
		/// <summary>Implements <see cref="IPagerInfoDao.FindByPageSize"/></summary>
		public IList<PagerInfo> FindByPageSize(Int32? pageSize) {
			String stmtId = "PagerInfo.FindByPageSize";
			IList<PagerInfo> result = SqlMapperManager.Instance.QueryForList<PagerInfo>(stmtId, pageSize);
			return result;
		}
		
		/// <summary>Implements <see cref="IPagerInfoDao.FindByAddDate"/></summary>
		public IList<PagerInfo> FindByAddDate(DateTime? addDate) {
			String stmtId = "PagerInfo.FindByAddDate";
			IList<PagerInfo> result = SqlMapperManager.Instance.QueryForList<PagerInfo>(stmtId, addDate);
			return result;
		}
		
		/// <summary>Implements <see cref="IPagerInfoDao.Insert"/></summary>
		public void Insert(PagerInfo obj) {
			if (obj == null) throw new ArgumentNullException("obj");
			String stmtId = "PagerInfo.Insert";
			SqlMapperManager.Instance.Insert(stmtId, obj);
		}
		
		/// <summary>Implements <see cref="IPagerInfoDao.Update"/></summary>
		public void Update(PagerInfo obj) {
			if (obj == null) throw new ArgumentNullException("obj");
			String stmtId = "PagerInfo.Update";
			SqlMapperManager.Instance.Update(stmtId, obj);
		}
		
		/// <summary>Implements <see cref="IPagerInfoDao.Delete"/></summary>
		public void Delete(PagerInfo obj) {
			if (obj == null) throw new ArgumentNullException("obj");
			String stmtId = "PagerInfo.Delete";
			SqlMapperManager.Instance.Delete(stmtId, obj);
		}
		
		/// <summary>Implements <see cref="IPagerInfoDao.DeleteByDataType"/></summary>
		public int DeleteByDataType(Int32? dataType) {
			String stmtId = "PagerInfo.DeleteByDataType";
			int result = SqlMapperManager.Instance.Delete(stmtId, dataType);
			return result;
		}
		
		/// <summary>Implements <see cref="IPagerInfoDao.DeleteByWhere"/></summary>
		public int DeleteByWhere(String where) {
			String stmtId = "PagerInfo.DeleteByWhere";
			int result = SqlMapperManager.Instance.Delete(stmtId, where);
			return result;
		}
		
		/// <summary>Implements <see cref="IPagerInfoDao.DeleteByOrderBy"/></summary>
		public int DeleteByOrderBy(String orderBy) {
			String stmtId = "PagerInfo.DeleteByOrderBy";
			int result = SqlMapperManager.Instance.Delete(stmtId, orderBy);
			return result;
		}
		
		/// <summary>Implements <see cref="IPagerInfoDao.DeleteByPageSize"/></summary>
		public int DeleteByPageSize(Int32? pageSize) {
			String stmtId = "PagerInfo.DeleteByPageSize";
			int result = SqlMapperManager.Instance.Delete(stmtId, pageSize);
			return result;
		}
		
		/// <summary>Implements <see cref="IPagerInfoDao.DeleteByAddDate"/></summary>
		public int DeleteByAddDate(DateTime? addDate) {
			String stmtId = "PagerInfo.DeleteByAddDate";
			int result = SqlMapperManager.Instance.Delete(stmtId, addDate);
			return result;
		}
		
		/// <summary>Implements <see cref="IPagerInfoDao.Reload"/></summary>
		public void Reload(PagerInfo obj) {
			if (obj == null) throw new ArgumentNullException("obj");
			String stmtId = "PagerInfo.Find";
			SqlMapperManager.Instance.QueryForObject<PagerInfo>(stmtId, obj, obj);
		}
		
	}

}
