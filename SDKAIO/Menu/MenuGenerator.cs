// <copyright file="MenuGenerator.cs" company="LeagueSharp">
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

namespace SDKAIO.Menu
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using global::SDKAIO.Utility;

    using LeagueSharp.SDK.Core.UI.IMenu;

    /// <summary>
    /// The MenuGenerator class. This generates a Menu for the assembly and adds this menu to the root menu.
    /// </summary>
    internal class MenuGenerator
    {
        /// <summary>
        /// The Root Menu we will attach our Champion Menu to.
        /// </summary>
        private Menu RootMenu;

        /// <summary>
        /// Initializes a new instance of the <see cref="MenuGenerator"/> class.
        /// </summary>
        public MenuGenerator()
        {
            this.RootMenu = AIOVariables.AssemblyMenu;           
        }

        /// <summary>
        /// Initializes this instance. Specifically, loads the menu of the champion (if present) into the Root Menu.
        /// </summary>
        public void Init()
        {
            
        }
    }
}
