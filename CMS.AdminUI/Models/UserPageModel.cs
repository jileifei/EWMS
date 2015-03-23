using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Webdiyer.WebControls.Mvc;
using CMS.Domain;

namespace CMS.AdminUI.Models
{
    public class UserPageModel
    {
        public PagedList<UserInfo> PageList
        {
            get;
            set;
        }

        public UserInfo UserEntity
        {
            get;
            set;
        }
    }
}