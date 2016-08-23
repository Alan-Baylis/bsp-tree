using BspTree.Base;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BspTree.Import
{
    public class PointImport : Point
    {
        [JsonProperty("label")]
        public string Label { get; set; }
    }
}
