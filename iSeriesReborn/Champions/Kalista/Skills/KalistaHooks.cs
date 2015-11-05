using System;
using DZLib.Logging;
using LeagueSharp;
using LeagueSharp.Common;

namespace iSeriesReborn.Champions.Kalista.Skills
{
    class KalistaHooks
    {

        internal static void OnIssueOrder(Obj_AI_Base sender, GameObjectIssueOrderEventArgs args)
        {
            if (sender.IsMe && args.Order == GameObjectOrder.MoveTo)
            {
                Console.WriteLine(@"Movement Order issued!");
            }
        }

        internal static void OnProcessSpellCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            try
            {
                if (sender.IsMe && args.SData.Name.Equals("KalistaExpungeWrapper"))
                {
                    LeagueSharp.Common.Utility.DelayAction.Add(0x7D, Orbwalking.ResetAutoAttackTimer);
                }
            }
            catch
            {
                LogHelper.AddToLog(new LogItem("Kalista_ProcessSpellCast", "Error: Failed to reset AA", LogSeverity.Error));
            }
        }

        internal static void OnGapclose(ActiveGapcloser gapcloser)
        {
           //
        }
    }
}
