using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using System.IO;
using CMS.Domain;
using CMS.Service;
using CMS.CommonLib.Utils;
using System.Web.Script.Serialization;

namespace CMS.AdminUI.Controllers
{
    public class FileStoreController : BaseController
    {
        //
        // GET: /FileStore/
        readonly string _rootPath = ConfigurationManager.AppSettings["UploadPath"];
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult IndexDialog()
        {
            return View();
        }

        #region ajax加载目录
        /// <summary>
        /// ajax加载目录列表
        /// </summary>
        /// <param name="localPath">当前路径</param>
        /// <param name="rows">行</param>
        /// <param name="page">页码</param>
        /// <returns></returns>
        public ActionResult AjaxJson(string localPath,int rows,int page)
        {
            int total;
            List<BrowseFoldInfo> foldList = LoadfoldList(localPath, rows, page, out total);
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string strJson = serializer.Serialize(foldList);
            string ab = "{\"total\":" + total + ",\"rows\":" + strJson + "}";
            return Content(ab);
        }
        #endregion

        #region 加载指定目录所有文件和文件夹
        private List<BrowseFoldInfo> LoadfoldList(string foldpath, int rows, int currentPage, out int rcount)
        {
            List<BrowseFoldInfo> efis = new List<BrowseFoldInfo>();
            //string proPath = Server.MapPath(rootPath);
            string proPath = _rootPath;
            if (!string.IsNullOrEmpty(foldpath)) {
                proPath =_rootPath+foldpath;
                currentPage = 1;
            }
            string webUrl = ConfigurationManager.AppSettings["WebUrl"];
            string[] folds = Directory.GetDirectories(proPath);
            foreach (string fold in folds)
            {
                BrowseFoldInfo efi = new BrowseFoldInfo();
                DirectoryInfo di = new DirectoryInfo(fold);
                efi.ExtName = "";
                efi.FileType = 0;
                efi.Filename = di.Name;
                if (proPath.Contains("\\"))
                {
                    efi.LocalPath = di.FullName.Replace(_rootPath, "").Replace("\\", "\\\\");
                }
                else
                {
                    efi.LocalPath = di.FullName.Replace(_rootPath, "");
                }
                efi.UploadDate = di.LastWriteTime;
                efi.FileURL = (webUrl + efi.LocalPath).Replace("\\\\", "/");
                efis.Add(efi);
            }
            string[] files = Directory.GetFiles(proPath);
            
            foreach (string fold in files)
            {
                BrowseFoldInfo efi = new BrowseFoldInfo();
                FileInfo fi = new FileInfo(fold);
                efi.ExtName = fi.Extension;
                efi.FileType = 1;
                if (proPath.Contains("\\"))
                {
                    efi.LocalPath = fi.FullName.Replace(_rootPath, "").Replace("\\", "\\\\");
                }
                else
                {
                    efi.LocalPath = fi.FullName.Replace(_rootPath, "");
                }
                efi.Filesize = (fi.Length / 1024).ToString(CultureInfo.InvariantCulture);
                efi.UploadDate = fi.LastWriteTime;
                efi.Filename = fi.Name;
                efi.FileURL = (webUrl + efi.LocalPath).Replace("\\\\","/");
                efis.Add(efi);
            }
            rcount = efis.Count;
            int starrecode = (currentPage - 1) * rows;
            int endrecode = currentPage * rows;
            if (rcount < endrecode)
            {
                endrecode = rcount;
            }
            List<BrowseFoldInfo> efispage = new List<BrowseFoldInfo>();
            for (int i = starrecode; i < endrecode; i++)
            {
                BrowseFoldInfo efi;
                efi = efis[i];
                efispage.Add(efi);
            }
            return efispage;
        }
        #endregion

        #region 新建文件夹
        /// <summary>
        /// 新建文件夹
        /// </summary>
        /// <param name="newFoldName"></param>
        /// <returns></returns>
        public ActionResult AddFold(string newFoldName)
        {
            string newFilePath = newFoldName;
            if (newFoldName.Contains("根目录"))
            {
                newFilePath = newFoldName.Replace("根目录/", "");
            }
            newFilePath = _rootPath + newFilePath;
            DirectoryInfo info = Directory.CreateDirectory(newFilePath);
            if (info.Exists)
            {
                return Content("success");
            }
            return Content("false");
        }
        #endregion

        public ActionResult ReName(string ty,string newpath, string oldpath)
        {
            string newPath = _rootPath + newpath.Replace("/","\\");
            string oldPath = _rootPath + oldpath.Replace("/", "\\");
            string imgurl = ConfigurationManager.AppSettings["WebUrl"] + newpath;
            string prePath = oldPath.Substring(0, oldPath.LastIndexOf('\\'));
            string newFold = newPath.Substring(oldPath.LastIndexOf('\\')+1);
            //表示文件
            if (ty == "2")
            {
                FileInfo fi = new FileInfo(oldPath);
                if (fi.Exists)
                {
                    string[] prefiles = Directory.GetFiles(prePath);
                    foreach (string pre in prefiles)
                    {
                        FileInfo file = new FileInfo(pre);
                        if (file.Name.ToLower() == newFold.ToLower().Trim())
                        {
                            return Json("{\"result\":\"existed\"}");
                        }
                    }
                    fi.MoveTo(newPath);
                    FileStoreService.RenameFile(newpath, oldpath, imgurl);
                    return Json("{\"result\":\"ok\"}");
                }
            }
            else
            {
                string[] files = Directory.GetFiles(oldPath);
                string[] folds = Directory.GetDirectories(oldPath);
                if (files.Length > 0 || folds.Length > 0)
                {
                    return Json("{\"result\":\"exist\"}");
                }
                string[] prefold = Directory.GetDirectories(prePath);
                foreach (string fold in prefold)
                {
                    DirectoryInfo di = new DirectoryInfo(fold);
                    if (di.Name.ToLower().Trim() == newFold.ToLower().Trim())
                    {
                        return Json("{\"result\":\"existed\"}");
                    }
                }
                Directory.Move(oldPath, newPath);
            }
            return Json("{\"result\":\"ok\"}");
        }

        public ActionResult DelFile(string ty,string localpath)
        {
            string delPath = _rootPath + localpath;        
            if (ty == "1")
            {
                string[] files = Directory.GetFiles(delPath);
                string[] folds = Directory.GetDirectories(delPath);
                if (files.Length > 0 || folds.Length > 0)
                {
                    return Json("{\"result\":\"exist\"}");
                }
                Directory.Delete(delPath);
            }
            else
            {
                FileInfo fi = new FileInfo(delPath);
                if (fi.Exists)
                {
                    fi.Delete();
                    FileStoreService.DelFileDetail(localpath);
                }
            }
            return Json("{\"result\":\"ok\"}");
        }


        public ActionResult GetFileDetail(string localpath)
        {
           IList<FileStore> fileList= FileStoreService.GetFileDetail(localpath);
           string result = "{";
           foreach (FileStore fs in fileList)
           {
               UserInfo user= new UserService().GetUserInfoByID(fs.CreateUserID);
               result += "\"Url\":\"" + fs.URL + "\",\"CreateTime\":\"" + fs.CreateTime + "\",\"UserName\":\"" + user.UserName + "\",\"PublicTime\":\"" + fs.PublicTime + "\"}";
           }
           return Json(result);
        }

        //修改发布时间
        public ActionResult UpdateTime(string localpath, string publicTime)
        {
            FileStoreService.UpdatePublicTime( publicTime,localpath);
            return Content("success");
        }

        #region 上传文件
        [HttpPost]
        public ActionResult UploadFile(string curpath)
        {
            string webUrl = ConfigurationManager.AppSettings["WebUrl"];
            string[] extNameList = ConfigurationManager.AppSettings["FileExtname"].Split(',');
            if (curpath.Contains("根目录"))
            {
                curpath = curpath.Replace("根目录", "");
            }
            string returnstr = "{\"result\":\"ok\"}"; 
            for (int iFile = 0; iFile < Request.Files.Count; iFile++)
            {
                bool checkTrue = false;
                FileStore storeFile = new FileStore();
                HttpPostedFileBase postedFile = Request.Files[iFile];
                if (postedFile==null)
                {
                    continue;
                }
                storeFile.ExtName = postedFile.FileName.Substring(postedFile.FileName.LastIndexOf('.'));

                string extnamel = storeFile.ExtName.ToLower();

                //check ext filename
                foreach(string item in extNameList)
                {
                    if (item == extnamel)
                    {
                        checkTrue = true;
                    }
                }
                if (checkTrue == false)
                {
                    returnstr = "{\"result\":\"error\"}";
                    return Json(returnstr);
                }

                if (!string.IsNullOrEmpty(curpath))
                {
                    storeFile.LocalPath = curpath + "/" + postedFile.FileName;
                }
                else
                {
                    storeFile.LocalPath = curpath +postedFile.FileName;
                }
                storeFile.FileSize = (postedFile.ContentLength / 1024);
                storeFile.URL = webUrl +storeFile.LocalPath; 
                storeFile.CreateTime = DateTime.Now;
                storeFile.Md5 = Utils.MD5(postedFile.FileName);
                storeFile.CreateUserID = UserID;
                if (System.IO.File.Exists(_rootPath+storeFile.LocalPath))
                {
                    System.IO.File.Delete(_rootPath + storeFile.LocalPath);
                }
                postedFile.SaveAs(_rootPath + storeFile.LocalPath);
                FileStoreService.InsertFile(storeFile);
            }
            return Json(returnstr);
        }
        #endregion

        #region send mail
        public ActionResult SendMail(string fileUrl, string mailTo, string title, string content)
        {
            FileStore fs=FileStoreService.GetFileByUrl(fileUrl);
            string lastDown = "";
            download_password dp = new download_password
            {
                send_time = DateTime.Now,
                send_userid = Convert.ToInt32(UserID.ToString(CultureInfo.InvariantCulture))
            };
            if (!string.IsNullOrEmpty(fs.URL)||fs.ID>0)
            {
                Random rd=new Random ();
                string pwd = Utils.MD5(fs.URL + fs.ID + rd.Next());
                dp.FileID = Convert.ToInt32(fs.ID.ToString(CultureInfo.InvariantCulture));
                dp.password = pwd;
                //todo 要修改的
                lastDown = "<br/><br/>------------------<a href=\"http://www.beijing-dentsu.com/down/index/" + fs.ID + "?pwd=" + pwd + "\" target=\"_blank\">下载地址</a>";
                DownService.Insert(dp);
            }
            string[] adminMail = ConfigurationManager.AppSettings["AdminMail"].Split(',');
            string smtp = ConfigurationManager.AppSettings["SmtpHost"];
            string[] mailTolist = mailTo.Split(',');
             string sendMsg="";
            foreach (string to in mailTolist)
            {
                sendMsg = Utils.SendMails(smtp, adminMail[0], adminMail[1], adminMail[0], to, content + lastDown, title);
            }
            return Content(sendMsg);
        }
        #endregion

        /// <summary>
        /// 取得文件名称
        /// </summary>
        /// <returns></returns>
        protected static string GetFileName(string sFileName)
        {
            string sValue = "";
            if (sFileName != "")
            {
                if (sFileName.IndexOf("\\", StringComparison.Ordinal) > 0)
                {
                    int j = sFileName.LastIndexOf("\\", StringComparison.Ordinal);
                    if (j > 0)
                        sValue = sFileName.Substring(j + 1);
                }
                else
                {
                    sValue = sFileName;
                }
            }

            return sValue;
        }

    }
}
