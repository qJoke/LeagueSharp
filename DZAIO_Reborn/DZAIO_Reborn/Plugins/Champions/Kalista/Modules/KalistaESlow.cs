using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DZAIO_Reborn.Core;
using DZAIO_Reborn.Helpers;
using DZAIO_Reborn.Helpers.Modules;
using LeagueSharp;
using LeagueSharp.Common;

namespace DZAIO_Reborn.Plugins.Champions.Kalista.Modules
{
    class KalistaESlow : IModule
    {
        private float LastECastTime = 0f;

        public void OnLoad()
        {

        }

        public bool ShouldGetExecuted()
        {
            return Variables.AssemblyMenu.GetItemValue<bool>("dzaio.champion.kalista.kalista.autoESlow") &&
                   Variables.Spells[SpellSlot.E].IsReady();

        }

        public DZAIOEnums.ModuleType GetModuleType()
        {
            return DZAIOEnums.ModuleType.OnUpdate;
        }

        public void OnExecute()
        {
           
        }
    }
}
