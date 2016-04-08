using System;
using LeagueSharp;
using LeagueSharp.Common;
using iDZEzreal.MenuHelper;

namespace iDZEzreal
{
    internal class Ezreal
    {
        public static void OnLoad()
        {
            Console.WriteLine("Loaded Ezreal");
            LoadSpells();
            LoadEvents();
        }

        private static void LoadSpells()
        {
            Variables.Spells[SpellSlot.Q].SetSkillshot(0.25f, 60f, 2000f, true, SkillshotType.SkillshotLine);
            Variables.Spells[SpellSlot.W].SetSkillshot(0.25f, 80f, 2000f, false, SkillshotType.SkillshotLine);
            Variables.Spells[SpellSlot.R].SetSkillshot(1f, 160f, 2000f, false, SkillshotType.SkillshotLine);
        }

        private static void LoadEvents()
        {
            Game.OnUpdate += OnUpdate;
        }

        private static void OnUpdate(EventArgs args)
        {
            switch (Variables.Orbwalker.ActiveMode)
            {
                case Orbwalking.OrbwalkingMode.Combo:
                    OnCombo();
                    break;
                case Orbwalking.OrbwalkingMode.Mixed:
                    OnMixed();
                    break;
            }
        }

        private static void OnCombo()
        {
            //Q
            if (Variables.Menu.Item("ezreal.combo.q").GetValue<bool>() && Variables.Spells[SpellSlot.Q].IsReady())
            {
                var target = TargetSelector.GetTarget(Variables.Spells[SpellSlot.Q].Range,
                    TargetSelector.DamageType.Physical);

                if (target.IsValidTarget(Variables.Spells[SpellSlot.Q].Range) &&
                    ObjectManager.Player.Distance(target) <= Variables.Spells[SpellSlot.Q].Range)
                {
                    Variables.Spells[SpellSlot.Q].CastIfHitchanceEquals(target,
                        target.IsMoving ? HitChance.Medium : MenuGenerator.GetHitchance());
                    Console.WriteLine("Rekt with Q");
                }
            }
        }

        private static void OnMixed()
        {
        }
    }
}