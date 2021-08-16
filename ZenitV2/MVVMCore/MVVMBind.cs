using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZenitV2.MVVMCore
{
    public class MVVMBind
    {
        public Type WindowType { get; set; }
        public Type ViewModelType { get; set; }

        public MVVMBind(Type windowType, Type viewModelType)
        {
            WindowType = windowType;
            ViewModelType = viewModelType;
        }
    }
}
