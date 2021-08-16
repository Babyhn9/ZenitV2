using ZenitV2.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZenitV2.Data
{
    public class DegreeData
    {
        public double Degree { get; set; }
        public double Min { get; set; }
        public double Sec { get; set; }


        public override bool Equals(object obj)
        {
            if (obj is DegreeData degree)
                return
                 Degree == degree.Degree &&
                       Min == degree.Min &&
                       Sec == degree.Sec;

            return false;

        }

        public override string ToString()
        {
            return $"{Degree}{Constants.DegreeSymbol}{Min}\'{Sec}";
        }

    }
}
