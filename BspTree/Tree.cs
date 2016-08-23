using BspTree.Base;
using BspTree.Import;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BspTree
{
    public class Tree
    {
        public Plane Plane { get; set; }
        public Tree Left { get; set; }
        public Tree Right { get; set; }
        public bool IsEmpty { get; set; }
    }
}
