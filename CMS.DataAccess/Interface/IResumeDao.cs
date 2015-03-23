//==============================================================================
//	CAUTION: This file is generated by IBatisNetGen.DaoIntf.cst at 2011/9/14 21:40:03
//				Any manual editing will be lost in re-generation.
//==============================================================================
using System;
using System.Collections.Generic;
using System.Text;

using CMS.Domain;

namespace CMS.DataAccess.Interface {
	
    /// <summary><c>IResumeDao</c> is the DAO interface for <see cref="CMS.Domain.Resume"/>.</summary>
    public partial interface IResumeDao {        

		/// <summary>Returns the total count of objects.</summary>
		int GetCount();

		/// <summary>Finds a <see cref="Resume"/> instance by the primary key value.</summary>
		Resume Find(Int64 iD);

		/// <summary>Finds all Resume instances.</summary>
		IList<Resume> FindAll();
		
		/// <summary>Finds all Resume instances without Lob columns loaded.</summary>
		IList<Resume> QuickFindAll();
		
		/// <summary>Finds Resume instances by PositionID value.</summary>
		IList<Resume> FindByPositionID(Int64 positionID);
		
		/// <summary>Finds Resume instances by Name value.</summary>
		IList<Resume> FindByName(String name);
		
		/// <summary>Finds Resume instances by Mobile value.</summary>
		IList<Resume> FindByMobile(String mobile);
		
		/// <summary>Finds Resume instances by Telphone value.</summary>
		IList<Resume> FindByTelphone(String telphone);
		
		/// <summary>Finds Resume instances by Text value.</summary>
		IList<Resume> FindByText(String text);
		
		/// <summary>Finds Resume instances by FileID value.</summary>
		IList<Resume> FindByFileID(Int64 fileID);
		
		/// <summary>Finds Resume instances by CreateTime value.</summary>
		IList<Resume> FindByCreateTime(DateTime createTime);
		
		/// <summary>Finds Resume instances by Status value.</summary>
		IList<Resume> FindByStatus(Int32 status);
		
		/// <summary>Finds Resume instances by ReviewUserID value.</summary>
		IList<Resume> FindByReviewUserID(Int64? reviewUserID);
		
		/// <summary>Finds Resume instances by ReviewTime value.</summary>
		IList<Resume> FindByReviewTime(DateTime? reviewTime);

        IList<Resume> FindAllUnion();

        IList<Resume> FindAllUnionByjobID(Int64 positionID);

        Resume FindAllDetail(Int64 ID);

		/// <summary>Inserts a new Resume instance into underlying database table.</summary>
		void Insert(Resume obj);
		
		/// <summary>Update the underlying database record of a Resume instance.</summary>
		int Update(Resume obj);
		
		/// <summary>Delete the underlying database record of a Resume instance.</summary>
		int Delete(Resume obj);
		
        /// <summary>Deletes <see cref="Resume"/> instances by <see cref="Resume.PositionID"/>.</summary>
		int DeleteByPositionID(Int64 positionID);
		
        /// <summary>Deletes <see cref="Resume"/> instances by <see cref="Resume.Name"/>.</summary>
		int DeleteByName(String name);
		
        /// <summary>Deletes <see cref="Resume"/> instances by <see cref="Resume.Mobile"/>.</summary>
		int DeleteByMobile(String mobile);
		
        /// <summary>Deletes <see cref="Resume"/> instances by <see cref="Resume.Telphone"/>.</summary>
		int DeleteByTelphone(String telphone);
		
        /// <summary>Deletes <see cref="Resume"/> instances by <see cref="Resume.Text"/>.</summary>
		int DeleteByText(String text);
		
        /// <summary>Deletes <see cref="Resume"/> instances by <see cref="Resume.FileID"/>.</summary>
		int DeleteByFileID(Int64 fileID);
		
        /// <summary>Deletes <see cref="Resume"/> instances by <see cref="Resume.CreateTime"/>.</summary>
		int DeleteByCreateTime(DateTime createTime);
		
        /// <summary>Deletes <see cref="Resume"/> instances by <see cref="Resume.Status"/>.</summary>
		int DeleteByStatus(Int32 status);
		
        /// <summary>Deletes <see cref="Resume"/> instances by <see cref="Resume.ReviewUserID"/>.</summary>
		int DeleteByReviewUserID(Int64? reviewUserID);
		
        /// <summary>Deletes <see cref="Resume"/> instances by <see cref="Resume.ReviewTime"/>.</summary>
		int DeleteByReviewTime(DateTime? reviewTime);
		
		/// <summary>Reload the underlying database record of a Resume instance.</summary>
		void Reload(Resume obj);
		
	}

}
