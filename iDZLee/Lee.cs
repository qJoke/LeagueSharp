using System;
using DZLib.Logging;
using iDZLee.Core;
using LeagueSharp;

namespace iDZLee
{
    class Lee
    {
        internal static void OnLoad()
        {
            Game.OnUpdate += OnUpdate;
        }

        private static void OnUpdate(EventArgs args)
        {
            try
            {
                foreach (var mode in Variables.modes)
                {
                    if (mode.GetRunCondition())
                    {
                        mode.Run();
                    }
                }
            }
            catch
            {
                LogHelper.AddToLog(new LogItem("OnTick", "Error during the OnTick"));
            }
        }
    }
}
