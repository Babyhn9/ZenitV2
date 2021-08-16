using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZenitV2.Data
{
    public class WordInputData
    {
        public string Name { get; set; }
        /// <summary>
        /// X
        /// </summary>
        public double North { get; set; }
        /// <summary>
        /// Y
        /// </summary>
        public double East { get; set; }
        /// <summary>
        /// Z
        /// </summary>
        public double Height { get; set; }
    }
}
