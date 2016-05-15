using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeagueSharp;

namespace DZAIO_Reborn.Core.ChampionHelper
{
    class ChampionLoader
    {
        public static void LoadChampion()
        {
            if (Variables.ChampList.ContainsKey(ObjectManager.Player.ChampionName))
            {
                Variables.CurrentChampion = Variables.ChampList[ObjectManager.Player.ChampionName].Invoke();
                Variables.CurrentChampion.OnLoad(Variables.AssemblyMenu);
                Variables.CurrentChampion.RegisterEvents();
                Game.PrintChat("<b><font color='#FF0000'>[DZAIO: Reborn] </font></b><font color='#FFFFFF'>Loaded</font> <b><font color='#FF0000'>{0}</font></b> plugin! <font color='#FFFFFF'> Have fun! </font>", ObjectManager.Player.ChampionName);
            }
        }
    }
}
