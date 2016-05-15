using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DZAIO_Reborn.Core.ChampionHelper;
using DZAIO_Reborn.Core.MenuHelper;
using LeagueSharp;
using LeagueSharp.Common;
using LeagueSharp.SDK;

namespace DZAIO_Reborn.Core
{
    class Bootstrap
    {
        public void Initialize()
        {
            Events.OnLoad += OnLoad;
        }

        private void OnLoad(object sender, EventArgs e)
        {
            MenuGenerator.GenerateMenu();
            ChampionLoader.LoadChampion();
        }
    }
}
