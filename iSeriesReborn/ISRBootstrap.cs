using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DZLib.Logging;
using iSeriesReborn.Utility.Entities;

namespace iSeriesReborn
{
    class ISRBootstrap
    {
        public static void OnLoad()
        {
            LogVariables.AssemblyName = "iSeriesReborn";
            LogHelper.OnLoad();
            GameObjects.Initialize();

            AssemblyLoader.OnLoad();
        }
    }
}
