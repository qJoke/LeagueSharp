using System;
using System.Linq;
using DZLib.Core;
using DZLib.Modules;
using DZLib.Positioning;
using LeagueSharp;
using LeagueSharp.Common;
using iDZEzreal.MenuHelper;
using SPrediction;

namespace iDZEzreal
{
    internal class Ezreal
    {
        public static void OnLoad()
        {
            Console.WriteLine("Loaded Ezreal");
            LoadSpells();
            LoadEvents();
            LoadModules();
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
            Orbwalking.AfterAttack += OrbwalkingOnAfterAttack;
            DZAntigapcloser.OnEnemyGapcloser += OnEnemyGapcloser;
        }

        private static void OnEnemyGapcloser(DZLib.Core.ActiveGapcloser gapcloser)
        {
            if (!Variables.Menu.Item("ezreal.misc.gapcloser").GetValue<bool>())
            {
                return;
            }

            if (!gapcloser.Sender.IsEnemy || !(gapcloser.End.Distance(ObjectManager.Player.ServerPosition) < 350))
                return;

            var extendedPosition = ObjectManager.Player.ServerPosition.Extend(
                Game.CursorPos, Variables.Spells[SpellSlot.E].Range);
            if (extendedPosition.IsSafe(Variables.Spells[SpellSlot.E].Range) &&
                extendedPosition.CountAlliesInRange(650f) > 0)
            {
                Variables.Spells[SpellSlot.E].Cast(extendedPosition);
            }
        }

        private static void OrbwalkingOnAfterAttack(AttackableUnit unit, AttackableUnit target1)
        {
            if (!unit.IsMe || !(target1 is Obj_AI_Hero))
            {
                return;
            }

            var target = (Obj_AI_Hero) target1;
            switch (Variables.Orbwalker.ActiveMode)
            {
                case Orbwalking.OrbwalkingMode.Combo:
                    if (Variables.Menu.Item("ezreal.combo.q").GetValue<bool>() &&
                        Variables.Spells[SpellSlot.Q].IsReady() &&
                        target.IsValidTarget(Variables.Spells[SpellSlot.Q].Range))
                    {
                        Variables.Spells[SpellSlot.Q].SPredictionCast(target, HitChance.High);
                    }
                    if (Variables.Menu.Item("ezreal.combo.w").GetValue<bool>() &&
                        Variables.Spells[SpellSlot.W].IsReady() &&
                        target.IsValidTarget(Variables.Spells[SpellSlot.W].Range) 
                        && ObjectManager.Player.ManaPercent > 45)
                    {
                        Variables.Spells[SpellSlot.W].SPredictionCast(target, HitChance.High);
                    }
                    break;
                case Orbwalking.OrbwalkingMode.Mixed:
                    if (Variables.Menu.Item("ezreal.mixed.q").GetValue<bool>() &&
                        Variables.Spells[SpellSlot.Q].IsReady() &&
                        target.IsValidTarget(Variables.Spells[SpellSlot.Q].Range))
                    {
                        Variables.Spells[SpellSlot.Q].SPredictionCast(target, HitChance.High);
                    }
                    if (Variables.Menu.Item("ezreal.mixed.w").GetValue<bool>() &&
                        Variables.Spells[SpellSlot.W].IsReady() &&
                        target.IsValidTarget(Variables.Spells[SpellSlot.W].Range)
                        && ObjectManager.Player.ManaPercent > 45)
                    {
                        Variables.Spells[SpellSlot.W].SPredictionCast(target, HitChance.High);
                    }
                    break;
            }
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
                    OnFarm();
                    break;
            }
            OnUpdateFunctions();
        }

        private static void OnUpdateFunctions()
        {
            //TODO AutoHaras
            foreach (
                var module in
                    Variables.Modules.Where(
                        module =>
                            module.ShouldGetExecuted() && module.GetModuleType() == ModuleType.OnUpdate &&
                            Variables.Menu.Item("ezreal.modules." + module.GetName().ToLowerInvariant())
                                .GetValue<bool>()))
            {
                module.OnExecute();
            }
        }

        private static void LoadModules()
        {
            foreach (var module in Variables.Modules)
            {
                try
                {
                    module.OnLoad();
                }
                catch (Exception)
                {
                    Console.WriteLine("Failed to load module {0}", module.GetName());
                    throw;
                }
            }
        }

        private static void OnCombo()
        {
            //Q
            if (Variables.Menu.Item("ezreal.combo.q").GetValue<bool>() && Variables.Spells[SpellSlot.Q].IsReady())
            {
                var target = TargetSelector.GetTarget(Variables.Spells[SpellSlot.Q].Range,
                    TargetSelector.DamageType.Physical);

                if (target.IsValidTarget(Variables.Spells[SpellSlot.Q].Range))
                {
                    Variables.Spells[SpellSlot.Q].SPredictionCast(target, MenuGenerator.GetHitchance());
                }
            }

            //W
            if (Variables.Menu.Item("ezreal.combo.w").GetValue<bool>() && Variables.Spells[SpellSlot.W].IsReady())
            {
                var target = TargetSelector.GetTarget(Variables.Spells[SpellSlot.W].Range,
                    TargetSelector.DamageType.Magical);

                if (target.IsValidTarget(Variables.Spells[SpellSlot.W].Range))
                {
                    Variables.Spells[SpellSlot.W].SPredictionCast(target, MenuGenerator.GetHitchance());
                }
            }

            //R
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
                    Variables.Spells[SpellSlot.R].SPredictionCast(
                        target, target.IsMoving ? HitChance.VeryHigh : HitChance.High);
                }

                var rPrediction = Variables.Spells[SpellSlot.R].GetAoeSPrediction();
                if (rPrediction.HitCount >= Variables.Menu.Item("ezreal.combo.r.min").GetValue<Slider>().Value)
                {
                    Variables.Spells[SpellSlot.R].Cast(rPrediction.CastPosition);
                }
            }
        }

        private static void OnMixed()
        {
            if (ObjectManager.Player.ManaPercent < Variables.Menu.Item("ezreal.mixed.mana").GetValue<Slider>().Value)
                return;
            if (Variables.Menu.Item("ezreal.mixed.q").GetValue<bool>() && Variables.Spells[SpellSlot.Q].IsReady())
            {
                var target = TargetSelector.GetTarget(Variables.Spells[SpellSlot.Q].Range,
                    TargetSelector.DamageType.Physical);

                if (target.IsValidTarget(Variables.Spells[SpellSlot.Q].Range))
                {
                    Variables.Spells[SpellSlot.Q].SPredictionCast(target, MenuGenerator.GetHitchance());
                }
            }

            if (Variables.Menu.Item("ezreal.mixed.w").GetValue<bool>() && Variables.Spells[SpellSlot.W].IsReady() &&
                ObjectManager.Player.ManaPercent > 45)
            {
                var target = TargetSelector.GetTarget(Variables.Spells[SpellSlot.W].Range,
                    TargetSelector.DamageType.Magical);

                if (target.IsValidTarget(Variables.Spells[SpellSlot.W].Range))
                {
                    Variables.Spells[SpellSlot.W].SPredictionCast(target,
                        target.IsMoving ? HitChance.Medium : MenuGenerator.GetHitchance());
                }
            }
        }

        private static void OnFarm()
        {
            if (!Variables.Menu.Item("ezreal.farm.q").GetValue<bool>() || !Variables.Spells[SpellSlot.Q].IsReady())
            {
                return;
            }

            var allMinions = MinionManager.GetMinions(ObjectManager.Player.ServerPosition,
                Variables.Spells[SpellSlot.Q].Range);
            var qMinion = allMinions.FirstOrDefault(x => x.IsValidTarget(Variables.Spells[SpellSlot.Q].Range));
            var minionHealth = HealthPrediction.GetHealthPrediction(qMinion,
                ((int)
                    (Variables.Spells[SpellSlot.Q].Delay +
                     (ObjectManager.Player.Distance(qMinion)/Variables.Spells[SpellSlot.Q].Speed)*1000f +
                     Game.Ping/2f)));
            if (qMinion != null)
            {
                
            
            switch (Variables.Orbwalker.ActiveMode)
            {
                case Orbwalking.OrbwalkingMode.LaneClear:
                    if (Variables.Spells[SpellSlot.Q].CanCast(qMinion))
                    {
                        if (Variables.Spells[SpellSlot.Q].GetDamage(qMinion) > qMinion.Health * 0.3f)
                        {
                            Variables.Spells[SpellSlot.Q].Cast(qMinion);
                        }
                    }
                    break;

                case Orbwalking.OrbwalkingMode.LastHit:
                    if (Variables.Spells[SpellSlot.Q].CanCast(qMinion))
                    {
                        if (qMinion.Health < Variables.Spells[SpellSlot.Q].GetDamage(qMinion))
                        {
                            if (Variables.Spells[SpellSlot.Q].GetDamage(qMinion) > minionHealth)
                            {
                                Variables.Spells[SpellSlot.Q].Cast(qMinion);
                            }
                        }
                    }
                    break;
            }
            }
        }

        private static bool CanExecuteTarget(Obj_AI_Base target)
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