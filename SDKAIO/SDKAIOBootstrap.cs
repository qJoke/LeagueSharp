// <copyright file="SDKAIOBootstrap.cs" company="LeagueSharp">
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
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using global::SDKAIO.Menu;

    /// <summary>
    /// The bootstrap class of the assembly. This will be used to init all the various components.
    /// </summary>
    internal class SDKAIOBootstrap
    {
        /// <summary>
        /// Gets the MenuGenerator instance.
        /// </summary>
        /// <value>
        /// The instance of the menu generator.
        /// </value>
        public MenuGenerator MenuGenerator { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SDKAIOBootstrap"/> class.
        /// </summary>
        public SDKAIOBootstrap()
        {
            this.MenuGenerator = new MenuGenerator();
        }
    }
}
