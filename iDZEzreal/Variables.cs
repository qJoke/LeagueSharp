using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeagueSharp;
using LeagueSharp.Common;

namespace iDZEzreal
{
    class Variables
    {
        public static Menu Menu { get; set; }

        public static Orbwalking.Orbwalker Orbwalker { get; set; }

        public static readonly Dictionary<SpellSlot, Spell> Spells = new Dictionary<SpellSlot, Spell>()
        {
            { SpellSlot.Q, new Spell(SpellSlot.Q) },
            { SpellSlot.W, new Spell(SpellSlot.W) },
            { SpellSlot.E, new Spell(SpellSlot.E) },
            { SpellSlot.R, new Spell(SpellSlot.R) }
        };
    }
}
