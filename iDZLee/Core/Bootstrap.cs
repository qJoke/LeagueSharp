using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DZLib.Logging;
using iDZLee.Core.MenuHelper;

namespace iDZLee.Core
{
    class Bootstrap
    {
        internal static void OnLoad()
        {
            LogHelper.OnLoad();
            MenuGenerator.Generate();
            Lee.OnLoad();
        }
    }
}
