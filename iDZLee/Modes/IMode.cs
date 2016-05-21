using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDZLee.Modes
{
    interface IMode
    {
        bool GetRunCondition();

        string GetModeName();

        void Run();
    }
}
