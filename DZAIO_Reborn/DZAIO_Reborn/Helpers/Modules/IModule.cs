using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DZAIO_Reborn.Helpers.Entity;

namespace DZAIO_Reborn.Helpers.Modules
{
    interface IModule
    {
        void OnLoad();

        bool ShouldGetExecuted();

        DZAIOEnums.ModuleType GetModuleType();

        void OnExecute();
    }
}
