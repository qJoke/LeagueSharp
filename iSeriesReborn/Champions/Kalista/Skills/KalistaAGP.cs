using iSeriesReborn.Utility;
using LeagueSharp;
using LeagueSharp.Common;

namespace iSeriesReborn.Champions.Kalista.Skills
{
    class KalistaAGP
    {
        //TODO Actually test this.
        internal static void OnGapclose(ActiveGapcloser gapcloser)
        {
            var spells = Variables.CurrentChampion.GetSpells();
            if (spells[SpellSlot.Q].IsReady())
            {
                var senderEndPoint = gapcloser.End;
                var mousePosition = Game.CursorPos;
                spells[SpellSlot.Q].Cast(senderEndPoint);
                LeagueSharp.Common.Utility.DelayAction.Add(260, () =>
                {
                    ObjectManager.Player.IssueOrder(GameObjectOrder.MoveTo, mousePosition);
                });
            }
        }
    }
}
