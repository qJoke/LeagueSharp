using System.Linq;
using LeagueSharp;
using LeagueSharp.Common;
using VayneHunter_Reborn.Modules.ModuleHelpers;
using VayneHunter_Reborn.Utility;
using VayneHunter_Reborn.Utility.Helpers;
using VayneHunter_Reborn.Utility.MenuUtility;

namespace VayneHunter_Reborn.Modules.ModuleList.Misc
{
    internal class Focus2WStacks : IModule
    {
        public void OnLoad() {}

        public bool ShouldGetExecuted()
        {
            return (MenuExtensions.GetItemValue<bool>("dz191.vhr.misc.general.specialfocus") &&
                    Variables.Orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.Combo) ||
                   (Variables.Orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.Mixed &&
                    Variables.Orbwalker.GetTarget() is Obj_AI_Hero);
        }

        public ModuleType GetModuleType()
        {
            return ModuleType.OnUpdate;
        }

        public void OnExecute()
        {
            if (Game.Time < 25 * 60 * 1000)
            {

            }
        }
    }
}
