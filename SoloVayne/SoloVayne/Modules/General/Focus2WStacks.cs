using System.Linq;
using LeagueSharp;
using LeagueSharp.Common;
using SoloVayne.Utility;
using SoloVayne.Utility.Entities;

namespace SoloVayne.Modules.Condemn
{
    class Focus2WStacks : ISOLOModule
    {
        public void OnLoad()
        {

        }

        public bool ShouldGetExecuted()
        {
            return
                HeroManager.Enemies.Any(
                    m => m.IsValidTarget(Orbwalking.GetRealAutoAttackRange(null) + 200) && m.Has2WStacks()) 
                    && !HeroManager.Enemies.Any(m => m.IsValidTarget(Orbwalking.GetRealAutoAttackRange(null) + 200) && m.Health + 15 < ObjectManager.Player.GetAutoAttackDamage(m) * 3 + Variables.spells[SpellSlot.W].GetDamage(m));
        }

        public ModuleType GetModuleType()
        {
            return ModuleType.OnUpdate;
        }

        public void OnExecute()
        {
            TargetSelector.SetTarget(HeroManager.Enemies.FirstOrDefault(
                    m => m.IsValidTarget(Orbwalking.GetRealAutoAttackRange(null) + 200) && m.Has2WStacks()));
        }
    }
}
