using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CMS.Domain
{
    public class PagerModel<T>
    {
        private int _pagesize = 20;
        public int PageSize { get { return _pagesize; } set { _pagesize = value; } }

        private int _currentpage = 1;
        public int CurrentPage { get { return _currentpage; } set { _currentpage = value; } }

        public int TotalRecords { get; set; }

        private IList<T> _itemlist = new List<T>(1);
        public IList<T> ItemList { get { return _itemlist; } set { _itemlist = value; } }
    }
}
