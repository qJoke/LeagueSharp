using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;

namespace DZOrianna.Utility.Ball
{
    static class Helper
    {

        public static IEnumerable<List<Vector2>> GetCombinations(IReadOnlyCollection<Vector2> allValues)
        {
            var collection = new List<List<Vector2>>();
            for (var counter = 0; counter < (1 << allValues.Count); ++counter)
            {
                var combination = allValues.Where((t, i) => (counter & (1 << i)) == 0).ToList();

                collection.Add(combination);
            }

            return collection;
        }

        public static T GetItemValue<T>(string item)
        {
            return Variables.AssemblyMenu.Item(item).GetValue<T>();
        }

        public static List<Obj_AI_Hero> getEHits(Vector3 to)
        {
            return HeroManager.Enemies
                .Where(enemy => enemy.IsValidTarget(Variables.spells[SpellSlot.E].Range * 1.5f))
                .Where(enemyHero => Variables.spells[SpellSlot.E].WillHit(enemyHero, to))
                .ToList();
        }
    }
}
