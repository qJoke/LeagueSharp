using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DZLib.Modules;

namespace iDZEzreal.Modules
{
    public class AutoQModule : IModule
    {
        public void OnLoad()
        {
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
    }
}