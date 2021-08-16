using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZenitV2.Utils
{
    public class AttachToWindowAttribute : Attribute
    {
        public string WinName { get; set; }
        public AttachToWindowAttribute(Type type)
        {
            WinName = type.Name;
        }
    }
}
