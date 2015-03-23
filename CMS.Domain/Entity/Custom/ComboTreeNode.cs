using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CMS.Domain
{
    [Serializable]
    public class ComboTreeNode
    {
        public string id;
        public string text;
        public string state = "open";//closed
        public string url;
        public List<ComboTreeNode> children = new List<ComboTreeNode>(1);
    }
}
