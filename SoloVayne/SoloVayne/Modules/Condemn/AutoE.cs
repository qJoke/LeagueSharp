using DZLib.Logging;
using LeagueSharp;
using LeagueSharp.Common;
using SoloVayne.Skills.Condemn;
using SoloVayne.Utility;

namespace SoloVayne.Modules.Condemn
{
    class AutoE : ISOLOModule
    {
        private CondemnLogicProvider MyProvider = new CondemnLogicProvider();

        public void OnLoad()
        {

        }

        public bool ShouldGetExecuted()
        {
            return MenuExtensions.GetItemValue<bool>("solo.vayne.misc.condemn.autoe") &&
                   Variables.spells[SpellSlot.E].IsReady() && (Variables.Orbwalker.ActiveMode != Orbwalking.OrbwalkingMode.Combo);
        }

        public ModuleType GetModuleType()
        {
            return ModuleType.OnUpdate;
        }

        public void OnExecute()
        {
            var target = MyProvider.GetTarget();
            if (target.IsValidTarget())
            {
                Variables.spells[SpellSlot.E].Cast(target);
            }
        }
    }
}
