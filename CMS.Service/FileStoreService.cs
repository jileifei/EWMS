using System;
using System.Collections.Generic;
using System.Linq;
using CMS.DataAccess;
using CMS.Domain;
using CMS.DataAccess.Interface;

namespace CMS.Service
{
    public class FileStoreService
    {
        /// <summary>
        /// 保存文件
        /// </summary>
        /// <param name="obj"></param>
        public static void InsertFile(FileStore obj)
        {
            IFileStoreDao filestoreService = CastleContext.Instance.GetService<IFileStoreDao>();
            filestoreService.Insert(obj);
        }

        public static IList<FileStore> GetFileDetail(string localPath)
        {
            IFileStoreDao filestoreService = CastleContext.Instance.GetService<IFileStoreDao>();
            return filestoreService.FindByLocalPath(localPath);
        }

        public static int DelFileDetail(string localPath)
        {
            IFileStoreDao filestoreService = CastleContext.Instance.GetService<IFileStoreDao>();
            return filestoreService.DeleteByLocalPath(localPath);
        }

        public static void RenameFile(string newpath, string oldpath, string imgurl)
        {
            IFileStoreDao filestoreService = CastleContext.Instance.GetService<IFileStoreDao>();
            filestoreService.UpdateLocalPath(newpath,oldpath,imgurl);
        }

        public static void UpdatePublicTime(string publicTime, string localPath)
        {
            IFileStoreDao fileService = CastleContext.Instance.GetService<IFileStoreDao>();
            fileService.Update(publicTime, localPath);
        }

        public static FileStore GetFileByUrl(string url)
        {
            IFileStoreDao fileService = CastleContext.Instance.GetService<IFileStoreDao>();
            IList<FileStore> fileList = fileService.FindByURL(url);
            if (fileList.Count > 1)
            {
                return fileList.OrderBy(a => a.CreateTime).First();
            }
            if(fileList.Count==0)
            {
                return new FileStore();
            }
            return fileList.First();
        }

        public static FileStore GetFileByID(Int64 id)
        {
            IFileStoreDao fileService = CastleContext.Instance.GetService<IFileStoreDao>();
            FileStore fileList = fileService.Find(id);
            return fileList;
        }
    }
}
