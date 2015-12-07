using System;
using System.Linq;
using DZLib.Logging;
using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;
using SoloVayne.Skills.Tumble;
using SoloVayne.Utility;

namespace SoloVayne.Modules.Condemn
{
    class AutoR : ISOLOModule
    {
        private TumbleLogicProvider Provider;

        public void OnLoad()
        {
            Obj_AI_Base.OnDoCast += OnDoCast;
            Provider = new TumbleLogicProvider();
        }

        public bool ShouldGetExecuted()
        {
            return ObjectManager.Player.GetEnemiesInRange(900f).Count(en => en.IsValidTarget() && !(en.HealthPercent < 5)) >= 2;
        }

        public ModuleType GetModuleType()
        {
            return ModuleType.Other;
        }

        public void OnExecute() { }

        private void OnDoCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            try
            {
                if (sender.IsMe && args.Slot == SpellSlot.R)
                {
                    var QPosition = Provider.GetSOLOVayneQPosition();
                    if (QPosition != Vector3.Zero)
                    {
                        Variables.spells[SpellSlot.Q].Cast(QPosition);
                        return;
                    }

                    var secondaryQPosition = ObjectManager.Player.ServerPosition.Extend(Game.CursorPos, 300f);

                    if (!secondaryQPosition.UnderTurret(true))
                    {
                        Variables.spells[SpellSlot.Q].Cast(secondaryQPosition);
                    }
                }
            }
            catch (Exception e)
            {
                LogHelper.AddToLog(new LogItem("AutoQifR", e, LogSeverity.Error));
            }
            
        }
    }
}
