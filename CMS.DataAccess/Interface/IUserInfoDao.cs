//==============================================================================
//	CAUTION: This file is generated by IBatisNetGen.DaoIntf.cst at 2011/9/14 21:40:03
//				Any manual editing will be lost in re-generation.
//==============================================================================
using System;
using System.Collections.Generic;
using System.Text;

using CMS.Domain;

namespace CMS.DataAccess.Interface {
	
    /// <summary><c>IUserInfoDao</c> is the DAO interface for <see cref="CMS.Domain.UserInfo"/>.</summary>
    public partial interface IUserInfoDao {        

		/// <summary>Returns the total count of objects.</summary>
		int GetCount();

		/// <summary>Finds a <see cref="UserInfo"/> instance by the primary key value.</summary>
		UserInfo Find(Int64 iD);

		/// <summary>Finds all UserInfo instances.</summary>
		IList<UserInfo> FindAll();
		
		/// <summary>Finds all UserInfo instances without Lob columns loaded.</summary>
		IList<UserInfo> QuickFindAll();
		
		/// <summary>Finds UserInfo instances by UserName value.</summary>
		IList<UserInfo> FindByUserName(String userName);
		
		/// <summary>Finds UserInfo instances by Password value.</summary>
		IList<UserInfo> FindByPassword(String password);
		
		/// <summary>Finds UserInfo instances by RealName value.</summary>
		IList<UserInfo> FindByRealName(String realName);
		
		/// <summary>Finds UserInfo instances by RoleID value.</summary>
		IList<UserInfo> FindByRoleID(Int64 roleID);
		
		/// <summary>Finds UserInfo instances by Department value.</summary>
		IList<UserInfo> FindByDepartment(String department);
		
		/// <summary>Finds UserInfo instances by CreateTime value.</summary>
		IList<UserInfo> FindByCreateTime(DateTime createTime);
		
		/// <summary>Finds UserInfo instances by CreateUserID value.</summary>
		IList<UserInfo> FindByCreateUserID(Int64 createUserID);
		
		/// <summary>Finds UserInfo instances by Email value.</summary>
		IList<UserInfo> FindByEmail(String email);
		
		/// <summary>Finds UserInfo instances by Status value.</summary>
		IList<UserInfo> FindByStatus(Int32 status);
		
		/// <summary>Inserts a new UserInfo instance into underlying database table.</summary>
		void Insert(UserInfo obj);
		
		/// <summary>Update the underlying database record of a UserInfo instance.</summary>
		void Update(UserInfo obj);
		
		/// <summary>Delete the underlying database record of a UserInfo instance.</summary>
		void Delete(UserInfo obj);
		
        /// <summary>Deletes <see cref="UserInfo"/> instances by <see cref="UserInfo.UserName"/>.</summary>
		int DeleteByUserName(String userName);
		
        /// <summary>Deletes <see cref="UserInfo"/> instances by <see cref="UserInfo.Password"/>.</summary>
		int DeleteByPassword(String password);
		
        /// <summary>Deletes <see cref="UserInfo"/> instances by <see cref="UserInfo.RealName"/>.</summary>
		int DeleteByRealName(String realName);
		
        /// <summary>Deletes <see cref="UserInfo"/> instances by <see cref="UserInfo.RoleID"/>.</summary>
		int DeleteByRoleID(Int64 roleID);
		
        /// <summary>Deletes <see cref="UserInfo"/> instances by <see cref="UserInfo.Department"/>.</summary>
		int DeleteByDepartment(String department);
		
        /// <summary>Deletes <see cref="UserInfo"/> instances by <see cref="UserInfo.CreateTime"/>.</summary>
		int DeleteByCreateTime(DateTime createTime);
		
        /// <summary>Deletes <see cref="UserInfo"/> instances by <see cref="UserInfo.CreateUserID"/>.</summary>
		int DeleteByCreateUserID(Int64 createUserID);
		
        /// <summary>Deletes <see cref="UserInfo"/> instances by <see cref="UserInfo.Email"/>.</summary>
		int DeleteByEmail(String email);
		
        /// <summary>Deletes <see cref="UserInfo"/> instances by <see cref="UserInfo.Status"/>.</summary>
		int DeleteByStatus(Int32 status);
		
		/// <summary>Reload the underlying database record of a UserInfo instance.</summary>
		void Reload(UserInfo obj);
		
	}

}