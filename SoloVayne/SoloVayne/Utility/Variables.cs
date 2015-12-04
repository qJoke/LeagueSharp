using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeagueSharp;
using LeagueSharp.Common;
using SoloVayne.Skills;
using SoloVayne.Skills.Tumble;
using SoloVayne.Utility.Enums;

namespace SoloVayne.Utility
{
    class Variables
    {
        /// <summary>
        /// Gets or sets the menu.
        /// </summary>
        /// <value>
        /// The menu.
        /// </value>
        public static Menu Menu { get; set; }

        /// <summary>
        /// Gets or sets the instance of the assembly.
        /// </summary>
        /// <value>
        /// The instance.
        /// </value>
        public static SOLOBootstrap Instance { get; set; }

        /// <summary>
        /// Gets or sets the orbwalker.
        /// </summary>
        /// <value>
        /// The orbwalker.
        /// </value>
        public static Orbwalking.Orbwalker Orbwalker { get; set; }

        /// <summary>
        /// The spells dictionary
        /// </summary>
        public static Dictionary<SpellSlot, Spell> spells = new Dictionary<SpellSlot, Spell>()
        {
            { SpellSlot.Q, new Spell(SpellSlot.Q) },
            { SpellSlot.W, new Spell(SpellSlot.W) },
            { SpellSlot.E, new Spell(SpellSlot.E, 590f) },
            { SpellSlot.R, new Spell(SpellSlot.R) }
        };

        /// <summary>
        /// The skills dictionary
        /// </summary>
        public static List<Skill> skills = new List<Skill>()
        {
            new Tumble(),
        };
    }
}
