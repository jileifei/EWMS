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
	
    /// <summary><c>NewsDocDao</c> is the implementation of <see cref="INewsDocDao"/>.</summary>
    public partial class NewsDocDao : BaseDAO, INewsDocDao {

		/// <summary>Implements <see cref="INewsDocDao.GetCount"/></summary>
		public int GetCount() {
			String stmtId = "NewsDoc.GetCount";
			int result = SqlMapperManager.Instance.QueryForObject<int>(stmtId, null);
			return result;
		}

		/// <summary>Implements <see cref="INewsDocDao.Find"/></summary>
		public NewsDoc Find(Int64 iD) {
			String stmtId = "NewsDoc.Find";
			NewsDoc result = SqlMapperManager.Instance.QueryForObject<NewsDoc>(stmtId, iD);
			return result;
		}

		/// <summary>Implements <see cref="INewsDocDao.FindAll"/></summary>
		public IList<NewsDoc> FindAll() {
			String stmtId = "NewsDoc.FindAll";
			IList<NewsDoc> result = SqlMapperManager.Instance.QueryForList<NewsDoc>(stmtId, null);
			return result;
		}
		
		/// <summary>Implements <see cref="INewsDocDao.QuickFindAll"/></summary>
		public IList<NewsDoc> QuickFindAll() {
			String stmtId = "NewsDoc.QuickFindAll";
			IList<NewsDoc> result = SqlMapperManager.Instance.QueryForList<NewsDoc>(stmtId, null);
			return result;
		}
		
		/// <summary>Implements <see cref="INewsDocDao.FindByChannelID"/></summary>
		public IList<NewsDoc> FindByChannelID(Int64 channelID) {
			String stmtId = "NewsDoc.FindByChannelID";
			IList<NewsDoc> result = SqlMapperManager.Instance.QueryForList<NewsDoc>(stmtId, channelID);
			return result;
		}
		
		/// <summary>Implements <see cref="INewsDocDao.FindBySpecialID"/></summary>
		public IList<NewsDoc> FindBySpecialID(Int64? specialID) {
			String stmtId = "NewsDoc.FindBySpecialID";
			IList<NewsDoc> result = SqlMapperManager.Instance.QueryForList<NewsDoc>(stmtId, specialID);
			return result;
		}
		
		/// <summary>Implements <see cref="INewsDocDao.FindByTitle"/></summary>
		public IList<NewsDoc> FindByTitle(String title) {
			String stmtId = "NewsDoc.FindByTitle";
			IList<NewsDoc> result = SqlMapperManager.Instance.QueryForList<NewsDoc>(stmtId, title);
			return result;
		}
		
		/// <summary>Implements <see cref="INewsDocDao.FindBySubTitle"/></summary>
		public IList<NewsDoc> FindBySubTitle(String subTitle) {
			String stmtId = "NewsDoc.FindBySubTitle";
			IList<NewsDoc> result = SqlMapperManager.Instance.QueryForList<NewsDoc>(stmtId, subTitle);
			return result;
		}
		
		/// <summary>Implements <see cref="INewsDocDao.FindByTitleColor"/></summary>
		public IList<NewsDoc> FindByTitleColor(String titleColor) {
			String stmtId = "NewsDoc.FindByTitleColor";
			IList<NewsDoc> result = SqlMapperManager.Instance.QueryForList<NewsDoc>(stmtId, titleColor);
			return result;
		}
		
		/// <summary>Implements <see cref="INewsDocDao.FindByAuthor"/></summary>
		public IList<NewsDoc> FindByAuthor(String author) {
			String stmtId = "NewsDoc.FindByAuthor";
			IList<NewsDoc> result = SqlMapperManager.Instance.QueryForList<NewsDoc>(stmtId, author);
			return result;
		}
		
		/// <summary>Implements <see cref="INewsDocDao.FindByPublicTime"/></summary>
		public IList<NewsDoc> FindByPublicTime(DateTime publicTime) {
			String stmtId = "NewsDoc.FindByPublicTime";
			IList<NewsDoc> result = SqlMapperManager.Instance.QueryForList<NewsDoc>(stmtId, publicTime);
			return result;
		}
		
		/// <summary>Implements <see cref="INewsDocDao.FindByContent"/></summary>
		public IList<NewsDoc> FindByContent(String content) {
			String stmtId = "NewsDoc.FindByContent";
			IList<NewsDoc> result = SqlMapperManager.Instance.QueryForList<NewsDoc>(stmtId, content);
			return result;
		}
		
		/// <summary>Implements <see cref="INewsDocDao.FindBySummary"/></summary>
		public IList<NewsDoc> FindBySummary(String summary) {
			String stmtId = "NewsDoc.FindBySummary";
			IList<NewsDoc> result = SqlMapperManager.Instance.QueryForList<NewsDoc>(stmtId, summary);
			return result;
		}
		
		/// <summary>Implements <see cref="INewsDocDao.FindByIsTop"/></summary>
		public IList<NewsDoc> FindByIsTop(Boolean isTop) {
			String stmtId = "NewsDoc.FindByIsTop";
			IList<NewsDoc> result = SqlMapperManager.Instance.QueryForList<NewsDoc>(stmtId, isTop);
			return result;
		}
		
		/// <summary>Implements <see cref="INewsDocDao.FindByIsRecommend"/></summary>
		public IList<NewsDoc> FindByIsRecommend(Boolean isRecommend) {
			String stmtId = "NewsDoc.FindByIsRecommend";
			IList<NewsDoc> result = SqlMapperManager.Instance.QueryForList<NewsDoc>(stmtId, isRecommend);
			return result;
		}
		
		/// <summary>Implements <see cref="INewsDocDao.FindByIsBold"/></summary>
		public IList<NewsDoc> FindByIsBold(Boolean isBold) {
			String stmtId = "NewsDoc.FindByIsBold";
			IList<NewsDoc> result = SqlMapperManager.Instance.QueryForList<NewsDoc>(stmtId, isBold);
			return result;
		}
		
		/// <summary>Implements <see cref="INewsDocDao.FindByTags"/></summary>
		public IList<NewsDoc> FindByTags(String tags) {
			String stmtId = "NewsDoc.FindByTags";
			IList<NewsDoc> result = SqlMapperManager.Instance.QueryForList<NewsDoc>(stmtId, tags);
			return result;
		}
		
		/// <summary>Implements <see cref="INewsDocDao.FindByLinkurl"/></summary>
		public IList<NewsDoc> FindByLinkurl(String linkurl) {
			String stmtId = "NewsDoc.FindByLinkurl";
			IList<NewsDoc> result = SqlMapperManager.Instance.QueryForList<NewsDoc>(stmtId, linkurl);
			return result;
		}
		
		/// <summary>Implements <see cref="INewsDocDao.FindBySmallImageUrl"/></summary>
		public IList<NewsDoc> FindBySmallImageUrl(String smallImageUrl) {
			String stmtId = "NewsDoc.FindBySmallImageUrl";
			IList<NewsDoc> result = SqlMapperManager.Instance.QueryForList<NewsDoc>(stmtId, smallImageUrl);
			return result;
		}
		
		/// <summary>Implements <see cref="INewsDocDao.FindBySource"/></summary>
		public IList<NewsDoc> FindBySource(String source) {
			String stmtId = "NewsDoc.FindBySource";
			IList<NewsDoc> result = SqlMapperManager.Instance.QueryForList<NewsDoc>(stmtId, source);
			return result;
		}
		
		/// <summary>Implements <see cref="INewsDocDao.FindByClickCount"/></summary>
		public IList<NewsDoc> FindByClickCount(Int32 clickCount) {
			String stmtId = "NewsDoc.FindByClickCount";
			IList<NewsDoc> result = SqlMapperManager.Instance.QueryForList<NewsDoc>(stmtId, clickCount);
			return result;
		}
		
		/// <summary>Implements <see cref="INewsDocDao.FindByStatus"/></summary>
		public IList<NewsDoc> FindByStatus(Int32 status) {
			String stmtId = "NewsDoc.FindByStatus";
			IList<NewsDoc> result = SqlMapperManager.Instance.QueryForList<NewsDoc>(stmtId, status);
			return result;
		}
		
		/// <summary>Implements <see cref="INewsDocDao.FindByCreateUserID"/></summary>
		public IList<NewsDoc> FindByCreateUserID(Int64? createUserID) {
			String stmtId = "NewsDoc.FindByCreateUserID";
			IList<NewsDoc> result = SqlMapperManager.Instance.QueryForList<NewsDoc>(stmtId, createUserID);
			return result;
		}
		
		/// <summary>Implements <see cref="INewsDocDao.FindByCreateUserIP"/></summary>
		public IList<NewsDoc> FindByCreateUserIP(String createUserIP) {
			String stmtId = "NewsDoc.FindByCreateUserIP";
			IList<NewsDoc> result = SqlMapperManager.Instance.QueryForList<NewsDoc>(stmtId, createUserIP);
			return result;
		}
		
		/// <summary>Implements <see cref="INewsDocDao.FindByCreateTime"/></summary>
		public IList<NewsDoc> FindByCreateTime(DateTime? createTime) {
			String stmtId = "NewsDoc.FindByCreateTime";
			IList<NewsDoc> result = SqlMapperManager.Instance.QueryForList<NewsDoc>(stmtId, createTime);
			return result;
		}
		
		/// <summary>Implements <see cref="INewsDocDao.FindByAuditUserID"/></summary>
		public IList<NewsDoc> FindByAuditUserID(Int64? auditUserID) {
			String stmtId = "NewsDoc.FindByAuditUserID";
			IList<NewsDoc> result = SqlMapperManager.Instance.QueryForList<NewsDoc>(stmtId, auditUserID);
			return result;
		}
		
		/// <summary>Implements <see cref="INewsDocDao.FindByAuditTime"/></summary>
		public IList<NewsDoc> FindByAuditTime(DateTime? auditTime) {
			String stmtId = "NewsDoc.FindByAuditTime";
			IList<NewsDoc> result = SqlMapperManager.Instance.QueryForList<NewsDoc>(stmtId, auditTime);
			return result;
		}
		
		/// <summary>Implements <see cref="INewsDocDao.FindByModifyUserID"/></summary>
		public IList<NewsDoc> FindByModifyUserID(Int64? modifyUserID) {
			String stmtId = "NewsDoc.FindByModifyUserID";
			IList<NewsDoc> result = SqlMapperManager.Instance.QueryForList<NewsDoc>(stmtId, modifyUserID);
			return result;
		}
		
		/// <summary>Implements <see cref="INewsDocDao.FindByModifyTime"/></summary>
		public IList<NewsDoc> FindByModifyTime(DateTime? modifyTime) {
			String stmtId = "NewsDoc.FindByModifyTime";
			IList<NewsDoc> result = SqlMapperManager.Instance.QueryForList<NewsDoc>(stmtId, modifyTime);
			return result;
		}
		
		/// <summary>Implements <see cref="INewsDocDao.FindByModifyUserIP"/></summary>
		public IList<NewsDoc> FindByModifyUserIP(String modifyUserIP) {
			String stmtId = "NewsDoc.FindByModifyUserIP";
			IList<NewsDoc> result = SqlMapperManager.Instance.QueryForList<NewsDoc>(stmtId, modifyUserIP);
			return result;
		}
		
		/// <summary>Implements <see cref="INewsDocDao.FindByIsAuditing"/></summary>
		public IList<NewsDoc> FindByIsAuditing(Int16 isAuditing) {
			String stmtId = "NewsDoc.FindByIsAuditing";
			IList<NewsDoc> result = SqlMapperManager.Instance.QueryForList<NewsDoc>(stmtId, isAuditing);
			return result;
		}
		
		/// <summary>Implements <see cref="INewsDocDao.FindByIsDelete"/></summary>
		public IList<NewsDoc> FindByIsDelete(Int16 isDelete) {
			String stmtId = "NewsDoc.FindByIsDelete";
			IList<NewsDoc> result = SqlMapperManager.Instance.QueryForList<NewsDoc>(stmtId, isDelete);
			return result;
		}
		
		/// <summary>Implements <see cref="INewsDocDao.Insert"/></summary>
		public void Insert(NewsDoc obj) {
			if (obj == null) throw new ArgumentNullException("obj");
			String stmtId = "NewsDoc.Insert";
			SqlMapperManager.Instance.Insert(stmtId, obj);
		}
		
		/// <summary>Implements <see cref="INewsDocDao.Update"/></summary>
		public void Update(NewsDoc obj) {
			if (obj == null) throw new ArgumentNullException("obj");
			String stmtId = "NewsDoc.Update";
			SqlMapperManager.Instance.Update(stmtId, obj);
		}
		
		/// <summary>Implements <see cref="INewsDocDao.Delete"/></summary>
		public void Delete(NewsDoc obj) {
			if (obj == null) throw new ArgumentNullException("obj");
			String stmtId = "NewsDoc.Delete";
			SqlMapperManager.Instance.Delete(stmtId, obj);
		}
		
		/// <summary>Implements <see cref="INewsDocDao.DeleteByChannelID"/></summary>
		public int DeleteByChannelID(Int64 channelID) {
			String stmtId = "NewsDoc.DeleteByChannelID";
			int result = SqlMapperManager.Instance.Delete(stmtId, channelID);
			return result;
		}
		
		/// <summary>Implements <see cref="INewsDocDao.DeleteBySpecialID"/></summary>
		public int DeleteBySpecialID(Int64? specialID) {
			String stmtId = "NewsDoc.DeleteBySpecialID";
			int result = SqlMapperManager.Instance.Delete(stmtId, specialID);
			return result;
		}
		
		/// <summary>Implements <see cref="INewsDocDao.DeleteByTitle"/></summary>
		public int DeleteByTitle(String title) {
			String stmtId = "NewsDoc.DeleteByTitle";
			int result = SqlMapperManager.Instance.Delete(stmtId, title);
			return result;
		}
		
		/// <summary>Implements <see cref="INewsDocDao.DeleteBySubTitle"/></summary>
		public int DeleteBySubTitle(String subTitle) {
			String stmtId = "NewsDoc.DeleteBySubTitle";
			int result = SqlMapperManager.Instance.Delete(stmtId, subTitle);
			return result;
		}
		
		/// <summary>Implements <see cref="INewsDocDao.DeleteByTitleColor"/></summary>
		public int DeleteByTitleColor(String titleColor) {
			String stmtId = "NewsDoc.DeleteByTitleColor";
			int result = SqlMapperManager.Instance.Delete(stmtId, titleColor);
			return result;
		}
		
		/// <summary>Implements <see cref="INewsDocDao.DeleteByAuthor"/></summary>
		public int DeleteByAuthor(String author) {
			String stmtId = "NewsDoc.DeleteByAuthor";
			int result = SqlMapperManager.Instance.Delete(stmtId, author);
			return result;
		}
		
		/// <summary>Implements <see cref="INewsDocDao.DeleteByPublicTime"/></summary>
		public int DeleteByPublicTime(DateTime publicTime) {
			String stmtId = "NewsDoc.DeleteByPublicTime";
			int result = SqlMapperManager.Instance.Delete(stmtId, publicTime);
			return result;
		}
		
		/// <summary>Implements <see cref="INewsDocDao.DeleteByContent"/></summary>
		public int DeleteByContent(String content) {
			String stmtId = "NewsDoc.DeleteByContent";
			int result = SqlMapperManager.Instance.Delete(stmtId, content);
			return result;
		}
		
		/// <summary>Implements <see cref="INewsDocDao.DeleteBySummary"/></summary>
		public int DeleteBySummary(String summary) {
			String stmtId = "NewsDoc.DeleteBySummary";
			int result = SqlMapperManager.Instance.Delete(stmtId, summary);
			return result;
		}
		
		/// <summary>Implements <see cref="INewsDocDao.DeleteByIsTop"/></summary>
		public int DeleteByIsTop(Boolean isTop) {
			String stmtId = "NewsDoc.DeleteByIsTop";
			int result = SqlMapperManager.Instance.Delete(stmtId, isTop);
			return result;
		}
		
		/// <summary>Implements <see cref="INewsDocDao.DeleteByIsRecommend"/></summary>
		public int DeleteByIsRecommend(Boolean isRecommend) {
			String stmtId = "NewsDoc.DeleteByIsRecommend";
			int result = SqlMapperManager.Instance.Delete(stmtId, isRecommend);
			return result;
		}
		
		/// <summary>Implements <see cref="INewsDocDao.DeleteByIsBold"/></summary>
		public int DeleteByIsBold(Boolean isBold) {
			String stmtId = "NewsDoc.DeleteByIsBold";
			int result = SqlMapperManager.Instance.Delete(stmtId, isBold);
			return result;
		}
		
		/// <summary>Implements <see cref="INewsDocDao.DeleteByTags"/></summary>
		public int DeleteByTags(String tags) {
			String stmtId = "NewsDoc.DeleteByTags";
			int result = SqlMapperManager.Instance.Delete(stmtId, tags);
			return result;
		}
		
		/// <summary>Implements <see cref="INewsDocDao.DeleteByLinkurl"/></summary>
		public int DeleteByLinkurl(String linkurl) {
			String stmtId = "NewsDoc.DeleteByLinkurl";
			int result = SqlMapperManager.Instance.Delete(stmtId, linkurl);
			return result;
		}
		
		/// <summary>Implements <see cref="INewsDocDao.DeleteBySmallImageUrl"/></summary>
		public int DeleteBySmallImageUrl(String smallImageUrl) {
			String stmtId = "NewsDoc.DeleteBySmallImageUrl";
			int result = SqlMapperManager.Instance.Delete(stmtId, smallImageUrl);
			return result;
		}
		
		/// <summary>Implements <see cref="INewsDocDao.DeleteBySource"/></summary>
		public int DeleteBySource(String source) {
			String stmtId = "NewsDoc.DeleteBySource";
			int result = SqlMapperManager.Instance.Delete(stmtId, source);
			return result;
		}
		
		/// <summary>Implements <see cref="INewsDocDao.DeleteByClickCount"/></summary>
		public int DeleteByClickCount(Int32 clickCount) {
			String stmtId = "NewsDoc.DeleteByClickCount";
			int result = SqlMapperManager.Instance.Delete(stmtId, clickCount);
			return result;
		}
		
		/// <summary>Implements <see cref="INewsDocDao.DeleteByStatus"/></summary>
		public int DeleteByStatus(Int32 status) {
			String stmtId = "NewsDoc.DeleteByStatus";
			int result = SqlMapperManager.Instance.Delete(stmtId, status);
			return result;
		}
		
		/// <summary>Implements <see cref="INewsDocDao.DeleteByCreateUserID"/></summary>
		public int DeleteByCreateUserID(Int64? createUserID) {
			String stmtId = "NewsDoc.DeleteByCreateUserID";
			int result = SqlMapperManager.Instance.Delete(stmtId, createUserID);
			return result;
		}
		
		/// <summary>Implements <see cref="INewsDocDao.DeleteByCreateUserIP"/></summary>
		public int DeleteByCreateUserIP(String createUserIP) {
			String stmtId = "NewsDoc.DeleteByCreateUserIP";
			int result = SqlMapperManager.Instance.Delete(stmtId, createUserIP);
			return result;
		}
		
		/// <summary>Implements <see cref="INewsDocDao.DeleteByCreateTime"/></summary>
		public int DeleteByCreateTime(DateTime? createTime) {
			String stmtId = "NewsDoc.DeleteByCreateTime";
			int result = SqlMapperManager.Instance.Delete(stmtId, createTime);
			return result;
		}
		
		/// <summary>Implements <see cref="INewsDocDao.DeleteByAuditUserID"/></summary>
		public int DeleteByAuditUserID(Int64? auditUserID) {
			String stmtId = "NewsDoc.DeleteByAuditUserID";
			int result = SqlMapperManager.Instance.Delete(stmtId, auditUserID);
			return result;
		}
		
		/// <summary>Implements <see cref="INewsDocDao.DeleteByAuditTime"/></summary>
		public int DeleteByAuditTime(DateTime? auditTime) {
			String stmtId = "NewsDoc.DeleteByAuditTime";
			int result = SqlMapperManager.Instance.Delete(stmtId, auditTime);
			return result;
		}
		
		/// <summary>Implements <see cref="INewsDocDao.DeleteByModifyUserID"/></summary>
		public int DeleteByModifyUserID(Int64? modifyUserID) {
			String stmtId = "NewsDoc.DeleteByModifyUserID";
			int result = SqlMapperManager.Instance.Delete(stmtId, modifyUserID);
			return result;
		}
		
		/// <summary>Implements <see cref="INewsDocDao.DeleteByModifyTime"/></summary>
		public int DeleteByModifyTime(DateTime? modifyTime) {
			String stmtId = "NewsDoc.DeleteByModifyTime";
			int result = SqlMapperManager.Instance.Delete(stmtId, modifyTime);
			return result;
		}
		
		/// <summary>Implements <see cref="INewsDocDao.DeleteByModifyUserIP"/></summary>
		public int DeleteByModifyUserIP(String modifyUserIP) {
			String stmtId = "NewsDoc.DeleteByModifyUserIP";
			int result = SqlMapperManager.Instance.Delete(stmtId, modifyUserIP);
			return result;
		}
		
		/// <summary>Implements <see cref="INewsDocDao.DeleteByIsAuditing"/></summary>
		public int DeleteByIsAuditing(Int16 isAuditing) {
			String stmtId = "NewsDoc.DeleteByIsAuditing";
			int result = SqlMapperManager.Instance.Delete(stmtId, isAuditing);
			return result;
		}
		
		/// <summary>Implements <see cref="INewsDocDao.DeleteByIsDelete"/></summary>
		public int DeleteByIsDelete(Int16 isDelete) {
			String stmtId = "NewsDoc.DeleteByIsDelete";
			int result = SqlMapperManager.Instance.Delete(stmtId, isDelete);
			return result;
		}
		
		/// <summary>Implements <see cref="INewsDocDao.Reload"/></summary>
		public void Reload(NewsDoc obj) {
			if (obj == null) throw new ArgumentNullException("obj");
			String stmtId = "NewsDoc.Find";
			SqlMapperManager.Instance.QueryForObject<NewsDoc>(stmtId, obj, obj);
		}
		
	}

}