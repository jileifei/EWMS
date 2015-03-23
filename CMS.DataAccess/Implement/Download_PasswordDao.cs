using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CMS.Domain;
using System.Collections;
using CMS.DataAccess.Interface;

namespace CMS.DataAccess.Implement
{
    public class Download_PasswordDao :BaseDAO, IDownload_Password 
    {
        public void Insert(download_password obj)
        {
            if (obj == null) throw new ArgumentNullException("obj");
            String stmtId = "download_password.Insert";
            SqlMapperManager.Instance.Insert(stmtId, obj);
        }

        public download_password Find(int fileID, string pwd)
        {
            String stmtId = "download_password.Find";
            download_password obj = new download_password();
            obj.FileID = fileID;
            obj.password = pwd;
            download_password result = SqlMapperManager.Instance.QueryForObject<download_password>(stmtId, obj);
            return result;
        }
    }
}
