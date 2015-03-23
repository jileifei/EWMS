//==============================================================================
//	CAUTION: This file is generated by IBatisNetGen.DaoIntf.cst at 2011/9/14 21:40:03
//				Any manual editing will be lost in re-generation.
//==============================================================================
using System;
using System.Collections.Generic;
using System.Text;

using CMS.Domain;

namespace CMS.DataAccess.Interface {
	
    /// <summary><c>IGlobalVariableDao</c> is the DAO interface for <see cref="CMS.Domain.GlobalVariable"/>.</summary>
    public partial interface IGlobalVariableDao {        

		/// <summary>Returns the total count of objects.</summary>
		int GetCount();

		/// <summary>Finds a <see cref="GlobalVariable"/> instance by the primary key value.</summary>
		GlobalVariable Find(Int64 globalID);

		/// <summary>Finds all GlobalVariable instances.</summary>
		IList<GlobalVariable> FindAll();
		
		/// <summary>Finds all GlobalVariable instances without Lob columns loaded.</summary>
		IList<GlobalVariable> QuickFindAll();
		
		/// <summary>Finds GlobalVariable instances by GlobalName value.</summary>
		IList<GlobalVariable> FindByGlobalName(String globalName);
		
		/// <summary>Finds GlobalVariable instances by EnName value.</summary>
		IList<GlobalVariable> FindByEnName(String enName);
		
		/// <summary>Finds GlobalVariable instances by Content value.</summary>
		IList<GlobalVariable> FindByContent(String content);
		
		/// <summary>Finds GlobalVariable instances by IsInclude value.</summary>
		IList<GlobalVariable> FindByIsInclude(Boolean isInclude);
		
		/// <summary>Finds GlobalVariable instances by AddDate value.</summary>
		IList<GlobalVariable> FindByAddDate(DateTime addDate);
		
		/// <summary>Finds GlobalVariable instances by IsDeleted value.</summary>
		IList<GlobalVariable> FindByIsDeleted(Int16 isDeleted);
		
		/// <summary>Inserts a new GlobalVariable instance into underlying database table.</summary>
		void Insert(GlobalVariable obj);
		
		/// <summary>Update the underlying database record of a GlobalVariable instance.</summary>
		void Update(GlobalVariable obj);
		
		/// <summary>Delete the underlying database record of a GlobalVariable instance.</summary>
		void Delete(GlobalVariable obj);
		
        /// <summary>Deletes <see cref="GlobalVariable"/> instances by <see cref="GlobalVariable.GlobalName"/>.</summary>
		int DeleteByGlobalName(String globalName);
		
        /// <summary>Deletes <see cref="GlobalVariable"/> instances by <see cref="GlobalVariable.EnName"/>.</summary>
		int DeleteByEnName(String enName);
		
        /// <summary>Deletes <see cref="GlobalVariable"/> instances by <see cref="GlobalVariable.Content"/>.</summary>
		int DeleteByContent(String content);
		
        /// <summary>Deletes <see cref="GlobalVariable"/> instances by <see cref="GlobalVariable.IsInclude"/>.</summary>
		int DeleteByIsInclude(Boolean isInclude);
		
        /// <summary>Deletes <see cref="GlobalVariable"/> instances by <see cref="GlobalVariable.AddDate"/>.</summary>
		int DeleteByAddDate(DateTime addDate);
		
        /// <summary>Deletes <see cref="GlobalVariable"/> instances by <see cref="GlobalVariable.IsDeleted"/>.</summary>
		int DeleteByIsDeleted(Int16 isDeleted);
		
		/// <summary>Reload the underlying database record of a GlobalVariable instance.</summary>
		void Reload(GlobalVariable obj);
		
	}

}