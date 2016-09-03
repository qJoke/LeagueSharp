using System;
using DZLib.MenuExtensions;
using LeagueSharp;
using LeagueSharp.Common;

namespace RivenEShielder
{
    class EShielder
    {
        public static Menu Menu = new Menu("Riven E Shielder", "dz191.riveneshield", true);
        public static Spell E;

        public static void OnLoad(EventArgs args)
        {
            Menu.AddBool("dz191.riveneshield.enabled", "Enabled", true);

            var subMenu = new Menu("Spells", "dz191.riveneshield.spells");

            foreach (var hero in HeroManager.Enemies)
            {
                foreach (var Spell in hero.Spellbook.Spells)
                {
                    if (Spell.SData.TargettingType == SpellDataTargetType.Unit)
                    {
                        subMenu.AddBool(
                            string.Format("dz191.riveneshield.spells.{0}", Spell.SData.Name),
                            hero.ChampionName + " " + Spell.Slot.ToString());
                    }
                }
            }

            Menu.AddSubMenu(subMenu);

            E = new Spell(SpellSlot.E, 325f);
            Obj_AI_Base.OnProcessSpellCast += OnProcessSpellCast;

            Menu.AddToMainMenu();
        }

        private static void OnProcessSpellCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (!Menu.Item("dz191.riveneshield.enabled").GetValue<bool>())
            {
                return;
            }

            if (sender is Obj_AI_Hero && args.SData != null && !args.SData.IsAutoAttack() && sender.IsEnemy &&
                (args.Target != null && args.Target.IsMe) &&
                Menu.Item(string.Format("dz191.riveneshield.spells.{0}", args.SData.Name)).GetValue<bool>())
            {
                var extendedPosition = HeroManager.Player.ServerPosition.Extend(Game.CursorPos, E.Range);

                if (!extendedPosition.UnderTurret(true))
                {
                    E.Cast(extendedPosition);
                }
            }
        }
    }
}
