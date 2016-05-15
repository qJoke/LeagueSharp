using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DZAIO_Reborn.Core;

namespace DZAIO_Reborn
{
    class Program
    {
        static void Main(string[] args)
        {
            Variables.BootstrapInstance = new Bootstrap();
            Variables.BootstrapInstance.Initialize();
        }
    }
}
