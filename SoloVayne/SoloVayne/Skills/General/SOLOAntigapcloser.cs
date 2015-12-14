using DZLib.Logging;
using LeagueSharp;
using LeagueSharp.Common;
using SoloVayne.Skills.Tumble;
using SoloVayne.Utility;
using SOLOVayne.Utility.General;
using ActiveGapcloser = SOLOVayne.Utility.General.ActiveGapcloser;

namespace SoloVayne.Skills.General
{
    class SOLOAntigapcloser
    {
        public SOLOAntigapcloser()
        {
            CustomAntiGapcloser.OnEnemyGapcloser += OnEnemyGapcloser;
            Interrupter2.OnInterruptableTarget += OnInterruptable;
        }

        private void OnInterruptable(Obj_AI_Hero sender, Interrupter2.InterruptableTargetEventArgs args)
        {
            var interrupterEnabled = MenuExtensions.GetItemValue<bool>("solo.vayne.misc.miscellaneous.interrupter");

            if (!interrupterEnabled
                || !Variables.spells[SpellSlot.E].IsReady()
                || !sender.IsValidTarget())
            {
                return;
            }

            if (args.DangerLevel == Interrupter2.DangerLevel.High)
            {
                Variables.spells[SpellSlot.E].Cast(sender);
            }
        }

        private void OnEnemyGapcloser(ActiveGapcloser gapcloser)
        {
            var antigapcloserEnabled = MenuExtensions.GetItemValue<bool>("solo.vayne.misc.miscellaneous.antigapcloser");
            var antigapcloserMode =
                MenuExtensions.GetItemValue<StringList>("solo.vayne.misc.miscellaneous.gapcloser.mode").SelectedIndex;
            var endPosition = gapcloser.End;

            if (!antigapcloserEnabled || !Variables.spells[SpellSlot.E].IsReady() || !gapcloser.Sender.IsValidTarget() ||
                ObjectManager.Player.Distance(endPosition) > 370)
            {
                return;
            }

            //Smart
            var ShouldBeRepelled = CustomAntiGapcloser.SpellShouldBeRepelledOnSmartMode(gapcloser.SData.Name);

            if (ShouldBeRepelled)
            {
                Variables.spells[SpellSlot.E].Cast(gapcloser.Sender);
            }
            else
            {
                //Use Q
                var extendedPosition = ObjectManager.Player.ServerPosition.Extend(endPosition, -300f);
                if (!extendedPosition.UnderTurret(true) &&
                    !(extendedPosition.CountEnemiesInRange(400f) >= 2 && extendedPosition.CountAlliesInRange(400f) < 3))
                {
                    Variables.spells[SpellSlot.Q].Cast(extendedPosition);
                }
            }
        }
    }
    
}
