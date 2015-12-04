using System;
using LeagueSharp.Common;
using SoloVayne.Utility;

namespace SoloVayne
{
    class Program
    {
        static void Main(string[] args)
        {
            CustomEvents.Game.OnGameLoad += OnLoad;
        }

        private static void OnLoad(EventArgs args)
        {
            Variables.Instance = new SOLOBootstrap();
        }
    }
}
