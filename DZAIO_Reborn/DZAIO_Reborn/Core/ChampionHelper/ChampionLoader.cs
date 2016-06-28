using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeagueSharp;

namespace DZAIO_Reborn.Core.ChampionHelper
{
    class ChampionLoader
    {
         private static Random random = new Random();

        public static void LoadChampion()
        {
            if (Variables.ChampList.ContainsKey(ObjectManager.Player.ChampionName))
            {
                Variables.CurrentChampion = Variables.ChampList[ObjectManager.Player.ChampionName].Invoke();
                Variables.CurrentChampion.OnLoad(Variables.AssemblyMenu);
                Variables.CurrentChampion.RegisterEvents();
                Game.PrintChat("<b><font color='#FF0000'>[DZAIO: Reborn] </font></b><font color='#FFFFFF'>Loaded</font> <b><font color='#FF0000'>{0}</font></b> plugin!", ObjectManager.Player.ChampionName);

                var champString = "<b>Also try it with: </b>";

                foreach (var champKvp in Variables.ChampList.Where(n => n.Key != ObjectManager.Player.ChampionName))
                {
                    champString += "<b><font color='" + GetRandomColor() +"'>" + champKvp.Key + "</font></b><font color='#FFFFFF'>, </font>";
                }
                    
                Game.PrintChat(champString);
            }
        }

        public static string GetRandomColor()
        {
            var color = $"#{random.Next(0x1000000):X6}";

            return color;
        }
    }
}
