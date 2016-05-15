using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeagueSharp.Common;

namespace DZAIO_Reborn.Plugins.Interface
{
    interface IChampion
    {
        void OnLoad(Menu menu);
        void RegisterEvents();
        void SetUpSpells();
        void OnTick();
        void OnCombo();
        void OnMixed();
        void OnLastHit();
        void OnLaneclear();
    }
}
