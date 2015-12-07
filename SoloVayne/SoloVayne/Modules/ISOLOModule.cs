using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoloVayne.Modules
{
    interface ISOLOModule
    {
        void OnLoad();

        bool ShouldGetExecuted();

        ModuleType GetModuleType();

        void OnExecute();
    }
}
