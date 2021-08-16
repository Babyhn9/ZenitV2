using ZenitV2.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZenitV2.Models
{
    public class DirectAngleCordinateModel
    {
        public string Name { get; set; }
        public string X { get; set; }
        public string Y { get; set; }
        public DegreeData DirectAngle { get; set; }
        /// <summary>
        /// Сближение мередианов
        /// </summary>
        public DegreeData Meredian { get; set; }
        public DegreeData Azimut { get; set; }
    }

    public class DirectAngleCordinateData
    {
        public string Name { get; set; }
        public double        X { get; set; }
        public double Y { get; set; }
        public DegreeData DirectAngle { get; set; }
        /// <summary>
        /// Сближение мередианов
        /// </summary>
        public DegreeData Meredian { get; set; }
        public DegreeData Azimut { get; set; }
    }
}
