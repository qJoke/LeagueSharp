using System;
using iDZLee.Core;
using LeagueSharp.Common;

namespace iDZLee
{
    class Program
    {
        static void Main(string[] args)
        {
            CustomEvents.Game.OnGameLoad += OnLoad;
        }

        private static void OnLoad(EventArgs args)
        {
            Bootstrap.OnLoad();
        }
    }
}
