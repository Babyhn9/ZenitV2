using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZenitV2.Utils
{
    public static class EventManager
    {
        public static event Action ChangeCountOfSymbols;

        public static void RaiseChangeCountOfSymbols() => ChangeCountOfSymbols?.Invoke();
    }
}
