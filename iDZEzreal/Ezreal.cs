using System;
using System.Linq;
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
                case Orbwalking.OrbwalkingMode.LaneClear:
                    OnLaneclear();
                    break;
            }
            OnUpdateFunctions();
        }

        private static void OnUpdateFunctions()
        {
            //TODO AutoHarass
            foreach (var hero in
                HeroManager.Enemies.Where(
                    x =>
                        x.IsValidTarget(Variables.Spells[SpellSlot.Q].Range) &&
                        Variables.Spells[SpellSlot.Q].GetDamage(x) > x.Health)
                    .Where(hero => Variables.Spells[SpellSlot.Q].IsReady()))
            {
                Variables.Spells[SpellSlot.Q].Cast(hero);
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
                }
            }
            //W
            if (Variables.Menu.Item("ezreal.combo.w").GetValue<bool>() && Variables.Spells[SpellSlot.W].IsReady())
            {
                var target = TargetSelector.GetTarget(Variables.Spells[SpellSlot.W].Range,
                    TargetSelector.DamageType.Magical);

                if (target.IsValidTarget(Variables.Spells[SpellSlot.W].Range) &&
                    ObjectManager.Player.Distance(target) <= Variables.Spells[SpellSlot.W].Range)
                {
                    Variables.Spells[SpellSlot.W].CastIfHitchanceEquals(target,
                        target.IsMoving ? HitChance.Medium : MenuGenerator.GetHitchance());
                }
            }

            if (Variables.Menu.Item("ezreal.combo.r").GetValue<bool>() && Variables.Spells[SpellSlot.R].IsReady())
            {
                var target = TargetSelector.GetTarget(2500f, TargetSelector.DamageType.Physical);

                if (target.IsValidTarget(Variables.Spells[SpellSlot.R].Range)
                    && CanExecuteTarget(target)
                    && ObjectManager.Player.Distance(target) >= Orbwalking.GetRealAutoAttackRange(null)*0.80f
                    &&
                    !(target.Health + 5 <
                      ObjectManager.Player.GetAutoAttackDamage(target)*2 +
                      Variables.Spells[SpellSlot.Q].GetDamage(target)))
                {
                    Variables.Spells[SpellSlot.R].CastIfHitchanceEquals(
                        target, target.IsMoving ? HitChance.VeryHigh : HitChance.High);
                }

                var rPrediction = Variables.Spells[SpellSlot.R].GetPrediction(target);
                if (rPrediction.AoeTargetsHitCount >= Variables.Menu.Item("ezreal.combo.r.min").GetValue<Slider>().Value)
                {
                    Variables.Spells[SpellSlot.R].Cast(rPrediction.CastPosition);
                }
            }
        }

        /// <summary>
        ///     Sheen checking
        /// </summary>
        /// <returns>
        ///     The <see cref="bool" />.
        /// </returns>
        public static bool HasSheen()
        {
            return Variables.Menu.Item("ezreal.misc.sheen").GetValue<bool>() && ObjectManager.Player.HasBuff("sheen");
        }

        private static void OnMixed()
        {
        }

        private static void OnLaneclear()
        {
        }

        private static bool CanExecuteTarget(Obj_AI_Hero target)
        {
            double damage = 1f;

            var prediction = Variables.Spells[SpellSlot.R].GetPrediction(target);
            var count = prediction.CollisionObjects.Count;

            damage += ObjectManager.Player.GetSpellDamage(target, SpellSlot.R);

            if (count >= 7)
            {
                damage = damage*.3;
            }
            else if (count != 0)
            {
                damage = damage*(10 - count/10);
            }

            return damage > target.Health + 10;
        }
    }
}