using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDZLee.Modes
{
    internal class Insec : IMode
    {
        public bool GetRunCondition()
        {
            //TODO When we have the actual menu name put that here
            return false;
        }

        public string GetModeName()
        {
            return "Insec Combo";
        }

        public void Run()
        {

        }
    }
}
