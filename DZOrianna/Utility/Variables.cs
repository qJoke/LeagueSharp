using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeagueSharp;
using LeagueSharp.Common;

namespace DZOrianna.Utility
{
    class Variables
    {
        /// <summary>
        /// The assembly menu
        /// </summary>
        public static Menu AssemblyMenu = new Menu("Orianna", "dz191.orianna", true);

        public static Orbwalking.Orbwalker Orbwalker { get; set; }

        /// <summary>
        /// The spells dictionary
        /// </summary>
        public static Dictionary<SpellSlot, Spell> spells = new Dictionary<SpellSlot, Spell>()
        {
            { SpellSlot.Q, new Spell(SpellSlot.Q, 825f) },
            { SpellSlot.W, new Spell(SpellSlot.W, 250f) },
            { SpellSlot.E, new Spell(SpellSlot.E, 1095f) },
            { SpellSlot.R, new Spell(SpellSlot.R, 370f) }
        };
    }
}
