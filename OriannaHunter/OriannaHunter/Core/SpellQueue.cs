using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeagueSharp;
using LeagueSharp.Common;

namespace OriannaHunter.Core
{
    class SpellQueue
    {
        //Needs to be done before Ballmanager
        private static int GameTickCount => (int) (Game.Time*1000f);
        private static int LastCommandTimeTick = 0;
        public static bool CanRun => !IsBusy;

        public static bool IsBusy
        {
            get
            {
                var busy = LastCommandTimeTick > 0 && LastCommandTimeTick + Game.Ping + 200 - GameTickCount > 0 
                    || (ObjectManager.Player.IsChannelingImportantSpell() || ObjectManager.Player.Spellbook.IsCharging || ObjectManager.Player.Spellbook.IsCastingSpell);
                IsBusy = busy;
                return busy;
            }
            private set
            {
                if (!value)
                {
                    LastCommandTimeTick = 0;
                }
            }
        }

        public static void OnLoad()
        {
            Obj_AI_Base.OnDoCast += OnDoCast;
            Spellbook.OnStopCast += OnStopCast;
            Spellbook.OnCastSpell += OnCast;
        }

        private static void OnCast(Spellbook sender, SpellbookCastSpellEventArgs args)
        {
            if (sender?.Owner != null && sender.Owner.IsMe)
            {
                switch (args.Slot)
                {
                    case SpellSlot.Q:
                    case SpellSlot.W:
                    case SpellSlot.E:
                    case SpellSlot.R:
                        if (CanRun)
                        {
                            LastCommandTimeTick = GameTickCount;
                        }
                        else
                        {
                            args.Process = false;
                        }
                        break;
                }
            }
        }

        private static void OnStopCast(Spellbook sender, SpellbookStopCastEventArgs args)
        {
            if (sender?.Owner != null && sender.Owner.IsMe)
            {
                IsBusy = false;
            }
        }

        private static void OnDoCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (sender != null && sender.IsMe && !Orbwalking.IsAutoAttack(args.SData.Name))
            {
                IsBusy = false;
            }
        }
    }
}
