using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeagueSharp;
using LeagueSharp.Common;

namespace DZAwarenessAIO.Modules.TFHelper
{
    class TFHelperCalculator
    {
        public static float GetAllyStrength()
        {
            if (ObjectManager.Player.IsDead || (TFHelperVariables.AlliesClose.Any() && !TFHelperVariables.EnemiesClose.ToList().Any()))
            {
                return 0;
            }

            return 1 - GetEnemyStrength();
        }

        public static float GetEnemyStrength()
        {
            var allyDamage = TFHelperVariables.AlliesClose.ToList().Sum(s => GetHeroAvgDamage(s, TFHelperVariables.EnemiesClose.ToList()));
            var enemyDamage = TFHelperVariables.EnemiesClose.ToList().Sum(s => GetHeroAvgDamage(s, TFHelperVariables.AlliesClose.ToList()));

            if (enemyDamage <= 0 && allyDamage >= 0)
            {
                return 0;
            }

            if (ObjectManager.Player.IsDead || (TFHelperVariables.AlliesClose.Any() && !TFHelperVariables.EnemiesClose.ToList().Any()))
            {
                return 0;
            }

            return (enemyDamage / allyDamage) <= 1 ? (enemyDamage / allyDamage) : (allyDamage / enemyDamage);
        }

        public static string GetText()
        {
            if ((TFHelperVariables.AlliesClose.Any() && !TFHelperVariables.EnemiesClose.ToList().Any()))
            {
                return "No enemy around";
            }

            return !ObjectManager.Player.IsDead ? string.Format("{0}v{1}: {2} will win", TFHelperVariables.AlliesClose.Count(), TFHelperVariables.EnemiesClose.Count(), GetAllyStrength() > GetEnemyStrength() ? "Ally" : "Enemy") : string.Format("You kinda suck!");
        }

        public static float GetHeroAvgDamage(Obj_AI_Hero player, List<Obj_AI_Hero> Enemies)
        {
            var totalEnemies = Enemies.Count();
            if (totalEnemies == 0)
            {
                return -1;
            }
            var AADamage = Enemies.Aggregate(0, (current, s) => (int) (current + player.GetAutoAttackDamage(s)));
            var QDamage = Enemies.Aggregate(0, (current, s) => (int)(current + (player.GetSpell(SpellSlot.Q).IsReady() ? player.GetSpellDamage(s, SpellSlot.Q) : 0)));
            var WDamage = Enemies.Aggregate(0, (current, s) => (int)(current + (player.GetSpell(SpellSlot.W).IsReady() ? player.GetSpellDamage(s, SpellSlot.W) : 0)));
            var EDamage = Enemies.Aggregate(0, (current, s) => (int)(current + (player.GetSpell(SpellSlot.E).IsReady() ? player.GetSpellDamage(s, SpellSlot.E) : 0)));
            var RDamage = Enemies.Aggregate(0, (current, s) => (int)(current + (player.GetSpell(SpellSlot.R).IsReady() ? player.GetSpellDamage(s, SpellSlot.R) : 0)));

            var totalDamage = AADamage + QDamage + WDamage + EDamage + RDamage;

            return (float) totalDamage / totalEnemies;
        }
    }
}
