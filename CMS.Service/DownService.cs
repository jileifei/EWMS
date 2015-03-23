using System;
using CMS.Domain;
using CMS.DataAccess;
using CMS.DataAccess.Interface;

namespace CMS.Service
{
    public class DownService
    {
        public static download_password Find(Int32 fileID,string password)
        {
            IDownload_Password feedbackService = CastleContext.Instance.GetService<IDownload_Password>();
            download_password commentList = feedbackService.Find(fileID,password);
            return commentList;
        }

        public static void Insert(download_password db)
        {
            IDownload_Password feedbackService = CastleContext.Instance.GetService<IDownload_Password>();
            feedbackService.Insert(db);
        }
    }
}
