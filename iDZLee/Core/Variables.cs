using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iDZLee.Modes;
using LeagueSharp;
using LeagueSharp.Common;

namespace iDZLee.Core
{
    class Variables
    {
        public static Menu Menu { get; set; } = new Menu("iDZLee", "idzlee", true);

        public static Orbwalking.Orbwalker Orbwalker { get; set; }

        public static Dictionary<Spells, Spell> spells = new Dictionary<Spells, Spell>
        {
            { Spells.Q, new Spell(SpellSlot.Q) },
            { Spells.W, new Spell(SpellSlot.W) },
            { Spells.E, new Spell(SpellSlot.E) },
            { Spells.R, new Spell(SpellSlot.R) },
            { Spells.Q2, new Spell(SpellSlot.Q) },
            { Spells.W2, new Spell(SpellSlot.W) },
            { Spells.E2, new Spell(SpellSlot.E) },
        };

        public static List<IMode> modes = new List<IMode>
        {
            new Combo(),
            new Mixed(),
            new Star(),
            new Insec()
        };
    }

    enum Spells
    {
        Q, W, E, R, Q2, W2, E2
    }
}
