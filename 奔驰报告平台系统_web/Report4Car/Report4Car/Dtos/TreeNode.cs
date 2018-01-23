using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Report4Car.Dtos
{
    public class TreeNode
    {
        public string Value { get; set; }
        public string Name { get; set; }
        public List<TreeNode> Nodes { get; set; }
    }
}