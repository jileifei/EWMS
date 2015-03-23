using System;
using System.Collections.Generic;
using CMS.DataAccess;
using CMS.Domain;
using CMS.DataAccess.Interface;
using System.Collections;

namespace CMS.Service
{
    public class FeedBackService
    {
        /// <summary>
        /// 所有留言信息
        /// </summary>
        /// <returns></returns>
        public static IList<FeedBack> FindAll()
        {
            IFeedBackDao feedbackService = CastleContext.Instance.GetService<IFeedBackDao>();
            IList<FeedBack> commentList = feedbackService.FindAll();
            return commentList;
        }

        /// <summary>
        /// 根据留言类型查询
        /// </summary>
        /// <returns></returns>
        public static IList<FeedBack> FindByMTStatus(Int32 messageType,Int32 status )
        {
            IFeedBackDao feedbackService = CastleContext.Instance.GetService<IFeedBackDao>();
            IList<FeedBack> commentList = feedbackService.FindByMTStatus(Convert.ToInt32(messageType), status);
            return commentList;
        }

        public static IList<FeedBack> FindByMessageType(Int32 messageType)
        {
            IFeedBackDao feedbackService = CastleContext.Instance.GetService<IFeedBackDao>();
            IList<FeedBack> commentList = feedbackService.FindByMessageType(messageType);
            return commentList;
        }

        public static void Insert(FeedBack messageType)
        {
            IFeedBackDao feedbackService = CastleContext.Instance.GetService<IFeedBackDao>();
            feedbackService.Insert(messageType);
        }

        //public static IList<FeedBack> QuickFindAll(int pageSize, int pageNumber)
        //{
        //    IFeedBackDao feedbackService = CastleContext.Instance.GetService<IFeedBackDao>();
        //    IList<FeedBack> commentList = feedbackService.QuickFindAll(pageSize, pageNumber);
        //    return commentList;
        //}
        /// <summary>
        /// 删除信息，是假删
        /// </summary>
        /// <param name="obj"></param>
        public static int Delete(FeedBack obj)
        {
            IFeedBackDao feedbackService = CastleContext.Instance.GetService<IFeedBackDao>();
            int result = feedbackService.Delete(obj);
            return result;
        }

        /// <summary>
        /// 审核成功
        /// </summary>
        /// <param name="ht"></param>
        /// <returns></returns>
        public static int AuditComment(Hashtable ht)
        {
            IFeedBackDao feedbackService = CastleContext.Instance.GetService<IFeedBackDao>();
            return feedbackService.UpdateStatus(ht);
        }

        /// <summary>
        /// 回复信息
        /// </summary>
        /// <param name="commentInfo"></param>
        /// <returns></returns>
        public static int ReplayContent(FeedBack commentInfo)
        {
            IFeedBackDao feedbackService = CastleContext.Instance.GetService<IFeedBackDao>();
            return feedbackService.UpdateReplayContent(commentInfo);
        }

        /// <summary>
        /// 删除回复
        /// </summary>
        /// <param name="commentInfo"></param>
        /// <returns></returns>
        public static int DeleteReplay(FeedBack commentInfo)
        {
            IFeedBackDao feedbackService = CastleContext.Instance.GetService<IFeedBackDao>();
            return feedbackService.UpdateReplayContent(commentInfo);
        }

        public static IList<FeedBack> FindAllByStatus(Int32 status)
        {
            IFeedBackDao feedbackService = CastleContext.Instance.GetService<IFeedBackDao>();
            IList<FeedBack> list = feedbackService.FindByStatus(status);
            return list;
        }
    }
}
