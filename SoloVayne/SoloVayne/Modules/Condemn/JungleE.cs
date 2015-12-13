using System.Linq;
using DZLib.Logging;
using LeagueSharp;
using LeagueSharp.Common;
using SoloVayne.Utility;
using SoloVayne.Utility.Entities;

namespace SoloVayne.Modules.Condemn
{
    class JungleE : ISOLOModule
    {
        public void OnLoad()
        {

        }

        public bool ShouldGetExecuted()
        {
            return MenuExtensions.GetItemValue<bool>("solo.vayne.laneclear.condemn.jungle") 
                && Variables.spells[SpellSlot.E].IsReady() 
                && (Variables.Orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.LaneClear)
                && ObjectManager.Player.ManaPercent >= 40;
        }

        public ModuleType GetModuleType()
        {
            return ModuleType.OnUpdate;
        }

        public void OnExecute()
        {
            var currentTarget = Variables.Orbwalker.GetTarget();

            if (currentTarget is Obj_AI_Minion && GameObjects.JungleLarge.Contains(currentTarget))
            {
                for (int i = 0; i < 450; i += 65)
                {
                    var endPos = currentTarget.Position.Extend(ObjectManager.Player.ServerPosition, -i);

                    if (endPos.IsWall())
                    {
                        Variables.spells[SpellSlot.E].Cast(currentTarget as Obj_AI_Base);
                        return;
                    }
                }
            }
        }
    }
}
