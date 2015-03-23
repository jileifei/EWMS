using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CMS.Domain;

namespace CMS.DataAccess.Interface
{
    public partial interface IDownload_Password
    {
        void Insert(download_password obj);

        download_password Find(int fileID, string pwd);
    }
}
