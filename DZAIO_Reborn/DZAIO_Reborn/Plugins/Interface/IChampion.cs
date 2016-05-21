using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeagueSharp;
using LeagueSharp.Common;

namespace DZAIO_Reborn.Plugins.Interface
{
    interface IChampion
    {
        void OnLoad(Menu menu);
        void RegisterEvents();
        Dictionary<SpellSlot, Spell> GetSpells();
        void OnTick();
        void OnCombo();
        void OnMixed();
        void OnLastHit();
        void OnLaneclear();
    }
}
