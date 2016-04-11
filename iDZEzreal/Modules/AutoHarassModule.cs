
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DZLib.Modules;

namespace iDZEzreal.Modules
{
    class AutoHarassModule : IModule
    {
        public void OnLoad()
        {
            Console.WriteLine("Auto Harass Module Loaded.");
        }

        public bool ShouldGetExecuted()
        {
            return false;
        }

        public ModuleType GetModuleType()
        {
            return ModuleType.OnUpdate;
        }

        public void OnExecute()
        {
           
        }

        public string GetName()
        {
            return "Auto Harass";
        }
    }
}
