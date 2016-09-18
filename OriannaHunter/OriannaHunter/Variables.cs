using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeagueSharp;
using LeagueSharp.Common;

namespace OriannaHunter
{
    class Variables
    {
        public static LeagueSharp.Common.Menu AssemblyMenu { get; set; }

        public static Orbwalking.Orbwalker Orbwalker { get; set; }

        public static Dictionary<SpellSlot, Spell> spells = new Dictionary<SpellSlot, Spell>()
        {
            { SpellSlot.Q, new Spell(SpellSlot.Q, 825f) },
            { SpellSlot.W, new Spell(SpellSlot.W, 250f) },
            { SpellSlot.E, new Spell(SpellSlot.E, 1095f) },
            { SpellSlot.R, new Spell(SpellSlot.R, 370f) }
        };
    }
}
