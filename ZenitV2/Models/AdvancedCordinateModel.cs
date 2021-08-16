using ZenitV2.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZenitV2.Models
{
    public class AdvancedCordinateModel : CordinateModel
    {
        public DegreeData L_L0 { get; set; }
        public DegreeData Ynn { get; set; }
        public double TgL_L0 { get; set; }
        public double SinB { get; set; }
        public double TgY { get; set; }
    }
}
