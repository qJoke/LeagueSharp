using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DZOrianna.Utility;

namespace DZOrianna
{
    class DZOriannaBootstrap
    {
        internal static void Init()
        {
            MenuGenerator.OnLoad();
            Orianna.OnLoad();
        }
    }
}
