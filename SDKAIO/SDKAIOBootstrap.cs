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
    using System.Linq;

    using global::SDKAIO.Utility;

    using LeagueSharp;
    using LeagueSharp.SDK.Core.Enumerations;
    using LeagueSharp.SDK.Core.UI.INotifications;
    using LeagueSharp.SDK.Core.Utils;

    using SharpDX;

    /// <summary>
    /// The bootstrap class of the assembly. This will be used to init all the various components.
    /// </summary>
    internal class SDKAIOBootstrap
    {
        /// <summary>
        /// Initializes the various components of the assembly.
        /// </summary>
        internal void Init()
        {
            try
            {
                if (AIOVariables.AIOInitalized)
                {
                    return;
                }
                
                var ChampionToLoad = ObjectManager.Player.ChampionName;
                if (AIOVariables.ChampList.ContainsKey(ChampionToLoad))
                {
                    AIOVariables.ChampList[ChampionToLoad]();
                    AIOVariables.AssemblyMenu.Attach();

                    Game.PrintChat(
                        $"<b><font color='#FF0000'>[SDKAIO]</font></b> {ChampionToLoad} Loaded! Good luck, have fun!");

                    Logging.Write()(LogLevel.Info, $"[SDKAIO] Loaded {ChampionToLoad} successfully!");

                    var loadedNotification = new Notification(
                        $"[SDKAIO] {ChampionToLoad} Loaded!",
                        $"{ChampionToLoad} was loaded!",
                        "Good luck, have fun!")
                                                 {
                                                     HeaderTextColor = Color.LightBlue,
                                                     BodyTextColor = Color.White,
                                                     Icon = NotificationIconType.Check,
                                                     IconFlash = true
                                                 };

                    Notifications.Add(loadedNotification);
                }

                AIOVariables.AIOInitalized = true;
            }
            catch
            {
                Logging.Write()(LogLevel.Error, "[SDKAIO] Failed to load the Bootstrap!");
            }
        }
    }
}
