using LeagueSharp;
using LeagueSharp.Common;
using SoloVayne.Utility;

namespace SoloVayne.Modules.Condemn
{
    class ActivatorModule : ISOLOModule
    {
        private Items.Item BOTRK = new Items.Item((int) ItemId.Blade_of_the_Ruined_King, 450f);
        private Items.Item Youmuu = new Items.Item((int)ItemId.Youmuus_Ghostblade);
        private Items.Item Cutlass = new Items.Item((int)ItemId.Bilgewater_Cutlass, 450f);

 
        public void OnLoad()
        {

        }

        public bool ShouldGetExecuted()
        {
            return ((BOTRK.IsOwned() && BOTRK.IsReady()) 
                || (Youmuu.IsOwned() && Youmuu.IsReady()) 
                || (Cutlass.IsOwned() && Cutlass.IsReady()))
                && (Variables.Orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.Combo || ObjectManager.Player.HealthPercent < 10);
        }

        public ModuleType GetModuleType()
        {
            return ModuleType.OnUpdate;
        }

        public void OnExecute()
        {
            var target = Variables.Orbwalker.GetTarget();

            if (target is Obj_AI_Hero && target.IsValidTarget(Orbwalking.GetRealAutoAttackRange(target) + 125f))
            {
                if (target.IsValidTarget(450f))
                {
                    var targetHealth = target.HealthPercent;
                    var myHealth = ObjectManager.Player.HealthPercent;

                    if (myHealth < 50 && targetHealth > 20 && (BOTRK.IsOwned() && BOTRK.IsReady()))
                    {
                        BOTRK.Cast(target as Obj_AI_Hero);
                    }

                    if (targetHealth < 65 && (Cutlass.IsOwned() && Cutlass.IsReady()))
                    {
                        Cutlass.Cast(target as Obj_AI_Hero);
                    }
                }

                if (Youmuu.IsOwned() && Youmuu.IsReady())
                {
                    Youmuu.Cast();
                }
            }
        }
    }

    enum ItemType
    {
        OnAfterAA, OnUpdate
    }
}
