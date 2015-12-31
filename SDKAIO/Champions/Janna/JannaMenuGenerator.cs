// <copyright file="JannaMenuGenerator.cs" company="LeagueSharp">
//    Copyright (c) 2015 LeagueSharp.
// 
//    This program is free software: you can redistribute it and/or modify
//    it under the terms of the GNU General Public License as published by
//    the Free Software Foundation, either version 3 of the License, or
//    (at your option) any later version.
// 
//    This program is distributed in the hope that it will be useful,
//    but WITHOUT ANY WARRANTY; without even the implied warranty of
//    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//    GNU General Public License for more details.
// 
//    You should have received a copy of the GNU General Public License
//    along with this program.  If not, see http://www.gnu.org/licenses/
// </copyright>

namespace SDKAIO.Champions.Janna
{
    using System.Linq;

    using LeagueSharp;
    using LeagueSharp.SDK.Core.UI.IMenu;
    using LeagueSharp.SDK.Core.UI.IMenu.Values;

    using SharpDX;

    /// <summary>
    /// This class generates the Menu for the champion Janna
    /// </summary>
    class JannaMenuGenerator : IMenuGenerator
    {
        public void LoadToMenu(Menu Menu)
        {
            var comboMenu = new Menu("sdkaio.janna.combo", "[Janna] Combo", false, ObjectManager.Player.ChampionName);
            {
                comboMenu.Add(new MenuBool("UseQ", "Use Q", true));
                comboMenu.Add(new MenuColor("test", "Test Colour Picker", Color.Red));

                comboMenu.Add(new MenuBool("UseW", "Use W", true));
                comboMenu.Add(new MenuBool("UseE", "Use E", true));
                comboMenu.Add(new MenuBool("UseR", "Use R", true));
                comboMenu.Add(new MenuSlider("RMinEnemies", "R Minimum Enemies", 2, 1, 5));
                comboMenu.Add(new MenuList<string>("RMode", "R Mode", new[] {"To Allies", "To Towers", "Both"}));
                Menu.Add(comboMenu);
            }

            var harassMenu = new Menu("sdkaio.janna.hybrid", "[Janna] Hybrid", false, ObjectManager.Player.ChampionName);
            {
                harassMenu.Add(new MenuBool("UseQ", "Use Q", true));
                harassMenu.Add(new MenuBool("UseW", "Use W", true));
                harassMenu.Add(new MenuBool("UseE", "Use E", true));
                Menu.Add(harassMenu);
            }


        }
    }
}
