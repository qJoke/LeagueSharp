// <copyright file="AIOVariables.cs" company="LeagueSharp">
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

namespace SDKAIO.Utility
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using LeagueSharp;
    using LeagueSharp.SDK.Core.UI.IMenu;

    /// <summary>
    /// The AIO Variables class:
    /// This class will contains the global variables we will need to use. These are, for example:
    /// - An instance of the Menu
    /// - A list of the Champions.
    /// - An instance of the currently loaded champion.
    /// - An instance of the Champion's spell dictionary for it to be accessible globally
    /// </summary>
    internal class AIOVariables
    {
        /// <summary>
        /// Gets or sets the assembly main menu.
        /// </summary>
        /// <value>
        /// The menu that will be shown when pressing shift.
        /// </value>
        public static Menu AssemblyMenu { get; internal set; } = new Menu($"SDKAIO: {ObjectManager.Player.ChampionName}", "sdkaio", true);
    }
}
