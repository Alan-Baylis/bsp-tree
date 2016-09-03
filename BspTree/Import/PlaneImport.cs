using BspTree.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BspTree.Import
{
    public class PlaneImport : Plane
    {
        public new List<PointImport> Points { get; set; }
    }
}
