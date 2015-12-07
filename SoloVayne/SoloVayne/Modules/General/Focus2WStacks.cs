using System.Linq;
using DZLib.Logging;
using LeagueSharp;
using LeagueSharp.Common;
using SoloVayne.Skills.Condemn;
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
                    m => m.IsValidTarget(Orbwalking.GetRealAutoAttackRange(null) + 200) && m.Has2WStacks());
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
