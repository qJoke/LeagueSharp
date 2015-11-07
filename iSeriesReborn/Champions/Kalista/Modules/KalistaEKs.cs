using System;
using System.Collections.Generic;
using System.Linq;
using DZLib.Logging;
using iSeriesReborn.Champions.Kalista.Skills;
using iSeriesReborn.Utility;
using iSeriesReborn.Utility.ModuleHelper;
using LeagueSharp;
using LeagueSharp.Common;

namespace iSeriesReborn.Champions.Kalista.Modules
{
    class KalistaEKs : IModule
    {
        private float LastCastTime = 0f;

        public string GetName()
        {
            return "Kalista_EKS";
        }

        public ModuleType GetModuleType()
        {
            return ModuleType.OnUpdate;
        }

        public bool ShouldRun()
        {
            return Variables.spells[SpellSlot.E].IsReady() &&
                   MenuExtensions.GetItemValue<bool>("iseriesr.kalista.misc.kse");
        }

        public void Run()
        {
            var killableRendTarget = HeroManager.Enemies.FirstOrDefault(KalistaE.CanBeRendKilled);

            if (killableRendTarget != null && (Environment.TickCount - LastCastTime > 250))
            {
                Variables.spells[SpellSlot.E].Cast();
                LastCastTime = Environment.TickCount;
            }

        }
    }
}
