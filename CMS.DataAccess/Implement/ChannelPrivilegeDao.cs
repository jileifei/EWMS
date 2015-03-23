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
	
    /// <summary><c>ChannelPrivilegeDao</c> is the implementation of <see cref="IChannelPrivilegeDao"/>.</summary>
    public partial class ChannelPrivilegeDao : BaseDAO, IChannelPrivilegeDao {

		/// <summary>Implements <see cref="IChannelPrivilegeDao.GetCount"/></summary>
		public int GetCount() {
			String stmtId = "ChannelPrivilege.GetCount";
			int result = SqlMapperManager.Instance.QueryForObject<int>(stmtId, null);
			return result;
		}

		/// <summary>Implements <see cref="IChannelPrivilegeDao.Find"/></summary>
		public ChannelPrivilege Find(Int32 iD) {
			String stmtId = "ChannelPrivilege.Find";
			ChannelPrivilege result = SqlMapperManager.Instance.QueryForObject<ChannelPrivilege>(stmtId, iD);
			return result;
		}

		/// <summary>Implements <see cref="IChannelPrivilegeDao.FindAll"/></summary>
		public IList<ChannelPrivilege> FindAll() {
			String stmtId = "ChannelPrivilege.FindAll";
			IList<ChannelPrivilege> result = SqlMapperManager.Instance.QueryForList<ChannelPrivilege>(stmtId, null);
			return result;
		}
		
		/// <summary>Implements <see cref="IChannelPrivilegeDao.QuickFindAll"/></summary>
		public IList<ChannelPrivilege> QuickFindAll() {
			String stmtId = "ChannelPrivilege.QuickFindAll";
			IList<ChannelPrivilege> result = SqlMapperManager.Instance.QueryForList<ChannelPrivilege>(stmtId, null);
			return result;
		}
		
		/// <summary>Implements <see cref="IChannelPrivilegeDao.FindByRoleID"/></summary>
		public IList<ChannelPrivilege> FindByRoleID(Int64 roleID) {
			String stmtId = "ChannelPrivilege.FindByRoleID";
			IList<ChannelPrivilege> result = SqlMapperManager.Instance.QueryForList<ChannelPrivilege>(stmtId, roleID);
			return result;
		}
		
		/// <summary>Implements <see cref="IChannelPrivilegeDao.FindByChannelID"/></summary>
		public IList<ChannelPrivilege> FindByChannelID(Int64 channelID) {
			String stmtId = "ChannelPrivilege.FindByChannelID";
			IList<ChannelPrivilege> result = SqlMapperManager.Instance.QueryForList<ChannelPrivilege>(stmtId, channelID);
			return result;
		}
		
		/// <summary>Implements <see cref="IChannelPrivilegeDao.FindByIsBrowse"/></summary>
		public IList<ChannelPrivilege> FindByIsBrowse(Boolean isBrowse) {
			String stmtId = "ChannelPrivilege.FindByIsBrowse";
			IList<ChannelPrivilege> result = SqlMapperManager.Instance.QueryForList<ChannelPrivilege>(stmtId, isBrowse);
			return result;
		}
		
		/// <summary>Implements <see cref="IChannelPrivilegeDao.FindByAddAuth"/></summary>
		public IList<ChannelPrivilege> FindByAddAuth(Boolean addAuth) {
			String stmtId = "ChannelPrivilege.FindByAddAuth";
			IList<ChannelPrivilege> result = SqlMapperManager.Instance.QueryForList<ChannelPrivilege>(stmtId, addAuth);
			return result;
		}
		
		/// <summary>Implements <see cref="IChannelPrivilegeDao.FindByEditAuth"/></summary>
		public IList<ChannelPrivilege> FindByEditAuth(Boolean editAuth) {
			String stmtId = "ChannelPrivilege.FindByEditAuth";
			IList<ChannelPrivilege> result = SqlMapperManager.Instance.QueryForList<ChannelPrivilege>(stmtId, editAuth);
			return result;
		}
		
		/// <summary>Implements <see cref="IChannelPrivilegeDao.FindByDelAuth"/></summary>
		public IList<ChannelPrivilege> FindByDelAuth(Boolean delAuth) {
			String stmtId = "ChannelPrivilege.FindByDelAuth";
			IList<ChannelPrivilege> result = SqlMapperManager.Instance.QueryForList<ChannelPrivilege>(stmtId, delAuth);
			return result;
		}
		
		/// <summary>Implements <see cref="IChannelPrivilegeDao.FindByAuditingAuth"/></summary>
		public IList<ChannelPrivilege> FindByAuditingAuth(Boolean auditingAuth) {
			String stmtId = "ChannelPrivilege.FindByAuditingAuth";
			IList<ChannelPrivilege> result = SqlMapperManager.Instance.QueryForList<ChannelPrivilege>(stmtId, auditingAuth);
			return result;
		}
		
		/// <summary>Implements <see cref="IChannelPrivilegeDao.FindByCreateTime"/></summary>
		public IList<ChannelPrivilege> FindByCreateTime(DateTime createTime) {
			String stmtId = "ChannelPrivilege.FindByCreateTime";
			IList<ChannelPrivilege> result = SqlMapperManager.Instance.QueryForList<ChannelPrivilege>(stmtId, createTime);
			return result;
		}
		
		/// <summary>Implements <see cref="IChannelPrivilegeDao.FindByCreateUserID"/></summary>
		public IList<ChannelPrivilege> FindByCreateUserID(Int64 createUserID) {
			String stmtId = "ChannelPrivilege.FindByCreateUserID";
			IList<ChannelPrivilege> result = SqlMapperManager.Instance.QueryForList<ChannelPrivilege>(stmtId, createUserID);
			return result;
		}
		
		/// <summary>Implements <see cref="IChannelPrivilegeDao.Insert"/></summary>
		public void Insert(ChannelPrivilege obj) {
			if (obj == null) throw new ArgumentNullException("obj");
			String stmtId = "ChannelPrivilege.Insert";
			SqlMapperManager.Instance.Insert(stmtId, obj);
		}
		
		/// <summary>Implements <see cref="IChannelPrivilegeDao.Update"/></summary>
		public void Update(ChannelPrivilege obj) {
			if (obj == null) throw new ArgumentNullException("obj");
			String stmtId = "ChannelPrivilege.Update";
			SqlMapperManager.Instance.Update(stmtId, obj);
		}
		
		/// <summary>Implements <see cref="IChannelPrivilegeDao.Delete"/></summary>
		public void Delete(ChannelPrivilege obj) {
			if (obj == null) throw new ArgumentNullException("obj");
			String stmtId = "ChannelPrivilege.Delete";
			SqlMapperManager.Instance.Delete(stmtId, obj);
		}
		
		/// <summary>Implements <see cref="IChannelPrivilegeDao.DeleteByRoleID"/></summary>
		public int DeleteByRoleID(Int64 roleID) {
			String stmtId = "ChannelPrivilege.DeleteByRoleID";
			int result = SqlMapperManager.Instance.Delete(stmtId, roleID);
			return result;
		}
		
		/// <summary>Implements <see cref="IChannelPrivilegeDao.DeleteByChannelID"/></summary>
		public int DeleteByChannelID(Int64 channelID) {
			String stmtId = "ChannelPrivilege.DeleteByChannelID";
			int result = SqlMapperManager.Instance.Delete(stmtId, channelID);
			return result;
		}
		
		/// <summary>Implements <see cref="IChannelPrivilegeDao.DeleteByIsBrowse"/></summary>
		public int DeleteByIsBrowse(Boolean isBrowse) {
			String stmtId = "ChannelPrivilege.DeleteByIsBrowse";
			int result = SqlMapperManager.Instance.Delete(stmtId, isBrowse);
			return result;
		}
		
		/// <summary>Implements <see cref="IChannelPrivilegeDao.DeleteByAddAuth"/></summary>
		public int DeleteByAddAuth(Boolean addAuth) {
			String stmtId = "ChannelPrivilege.DeleteByAddAuth";
			int result = SqlMapperManager.Instance.Delete(stmtId, addAuth);
			return result;
		}
		
		/// <summary>Implements <see cref="IChannelPrivilegeDao.DeleteByEditAuth"/></summary>
		public int DeleteByEditAuth(Boolean editAuth) {
			String stmtId = "ChannelPrivilege.DeleteByEditAuth";
			int result = SqlMapperManager.Instance.Delete(stmtId, editAuth);
			return result;
		}
		
		/// <summary>Implements <see cref="IChannelPrivilegeDao.DeleteByDelAuth"/></summary>
		public int DeleteByDelAuth(Boolean delAuth) {
			String stmtId = "ChannelPrivilege.DeleteByDelAuth";
			int result = SqlMapperManager.Instance.Delete(stmtId, delAuth);
			return result;
		}
		
		/// <summary>Implements <see cref="IChannelPrivilegeDao.DeleteByAuditingAuth"/></summary>
		public int DeleteByAuditingAuth(Boolean auditingAuth) {
			String stmtId = "ChannelPrivilege.DeleteByAuditingAuth";
			int result = SqlMapperManager.Instance.Delete(stmtId, auditingAuth);
			return result;
		}
		
		/// <summary>Implements <see cref="IChannelPrivilegeDao.DeleteByCreateTime"/></summary>
		public int DeleteByCreateTime(DateTime createTime) {
			String stmtId = "ChannelPrivilege.DeleteByCreateTime";
			int result = SqlMapperManager.Instance.Delete(stmtId, createTime);
			return result;
		}
		
		/// <summary>Implements <see cref="IChannelPrivilegeDao.DeleteByCreateUserID"/></summary>
		public int DeleteByCreateUserID(Int64 createUserID) {
			String stmtId = "ChannelPrivilege.DeleteByCreateUserID";
			int result = SqlMapperManager.Instance.Delete(stmtId, createUserID);
			return result;
		}
		
		/// <summary>Implements <see cref="IChannelPrivilegeDao.Reload"/></summary>
		public void Reload(ChannelPrivilege obj) {
			if (obj == null) throw new ArgumentNullException("obj");
			String stmtId = "ChannelPrivilege.Find";
			SqlMapperManager.Instance.QueryForObject<ChannelPrivilege>(stmtId, obj, obj);
		}
		
	}

}