// <copyright file="Program.cs" company="LeagueSharp">
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

namespace SDKAIO
{
    using System;
    using global::SDKAIO.Utility;

    using LeagueSharp.SDK;

    /// <summary>
    /// The entry class of the assembly.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// The entry point of the assembly.
        /// </summary>
        /// <param name="args">The arguments passed to the assembly.</param>
        private static void Main(string[] args)
        {
            Events.OnLoad += OnLoad;
        }

        /// <summary>
        /// Called when the assembly is loaded. This can either be at the Game start or while the game is running.
        /// </summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private static void OnLoad(object sender, EventArgs e)
        {
            //Initialize a new instance of the SDKAIOBootstrap class which will initialize the other components and call its Init() method.
            AIOVariables.AIOBootstrap = new SDKAIOBootstrap();
            AIOVariables.AIOBootstrap.Init();
        }
    }
}
