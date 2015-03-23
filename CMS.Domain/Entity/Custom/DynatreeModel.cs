using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CMS.Domain
{
    public class DynatreeModel
    {
        public string title;
        public bool isFolder = false;
        public string key;
        public bool expand = false;
        public bool isLazy = false;
        public string url = "";
        public long ParentID = 0;

        public Boolean IsBrowse = false;
        public Boolean AddAuth = false;
        public Boolean EditAuth = false;
        public Boolean DelAuth = false;
        public Boolean AuditingAuth = false;

        public List<DynatreeModel> children = new List<DynatreeModel>(1);
    }
}
