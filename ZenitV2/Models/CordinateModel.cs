using ZenitV2.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZenitV2.Models
{
    public class CordinateModel
    {
        public string Name { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }
        public DegreeData B { get; set; }
        public DegreeData L { get; set; }
        public double H { get; set; }
    }
}
