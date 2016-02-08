using System;
using System.Linq;
using System.Runtime.CompilerServices;
using DZOrianna.Utility;
using DZOrianna.Utility.Ball;
using LeagueSharp;
using LeagueSharp.Common;

namespace DZOrianna
{
    class Orianna
    {
        internal static void OnLoad()
        {
            CommandQueue.InitEvents();
            Variables.BallManager = new PetHandler();
            Variables.BallManager.OnLoad();
            Variables.spells[SpellSlot.Q].SetSkillshot(0f, 110f, 1425f, false, SkillshotType.SkillshotLine);
            Variables.spells[SpellSlot.E].SetSkillshot(0.25f, 80f, 1700f, true, SkillshotType.SkillshotLine);

            Game.OnUpdate += OnUpdate;
            Spellbook.OnCastSpell += OnCastSpell;
            Drawing.OnDraw += OnDraw;
        }

        private static void OnDraw(EventArgs args)
        {
 	        Render.Circle.DrawCircle(Variables.BallManager.BallPosition, 65, System.Drawing.Color.Red);
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

                    break;
            }
        }

        private static void OnCombo()
        {
            var qTarget = TargetSelector.GetTarget(Variables.spells[SpellSlot.Q].Range / 1.5f, TargetSelector.DamageType.Magical);

            if (Variables.AssemblyMenu.Item("dz191.orianna.combo.q").GetValue<bool>() && Variables.spells[SpellSlot.Q].IsReady())
            {
                if (qTarget.IsValidTarget())
                {
                    var qPrediction = Prediction.GetPrediction(
                        qTarget,
                        (int)
                            (Variables.BallManager.BallPosition.Distance(qTarget.ServerPosition) /
                             Variables.spells[SpellSlot.Q].Speed + Variables.spells[SpellSlot.Q].Delay +
                             (Game.Ping / 2f) / 1000f + 0.125f));

                        Variables.BallManager.ProcessCommand(new Command()
                        {
                            SpellCommand = Commands.Q,
                            Where = qPrediction.UnitPosition
                        });
                }
            }

            if (Variables.AssemblyMenu.Item("dz191.orianna.combo.w").GetValue<bool>() && Variables.spells[SpellSlot.W].IsReady())
            {
                var ballPosition = Variables.BallManager.BallPosition;
                var minWEnemies = Variables.AssemblyMenu.Item("dz191.orianna.combo.minw").GetValue<Slider>().Value;
                if (ObjectManager.Player.CountEnemiesInRange(Variables.spells[SpellSlot.Q].Range + 250f) > 1)
                {
                    if (ballPosition.CountEnemiesInRange(Variables.spells[SpellSlot.W].Range) >= minWEnemies)
                    {
                        Variables.BallManager.ProcessCommand(new Command() { SpellCommand = Commands.W, });
                    }
                }
                else
                {
                    if (ballPosition.CountEnemiesInRange(Variables.spells[SpellSlot.W].Range) > 0)
                    {
                        Variables.BallManager.ProcessCommand(new Command() { SpellCommand = Commands.W, });
                    }
                }
                
            }

            if (Variables.AssemblyMenu.Item("dz191.orianna.combo.r").GetValue<bool>() &&
                Variables.spells[SpellSlot.R].IsReady())
            {
                if (ObjectManager.Player.CountEnemiesInRange(Variables.spells[SpellSlot.Q].Range + 250f) > 1 && ObjectManager.Player.CountEnemiesInRange(Variables.spells[SpellSlot.Q].Range + 250f) > 1)
                {
                    var enemyHeroesPositions = HeroManager.Enemies.Select(hero => hero.Position.To2D()).ToList();

                    var Groups = Helper.GetCombinations(enemyHeroesPositions);

                    foreach (var group in Groups)
                    {
                        if (group.Count >=
                            Variables.AssemblyMenu.Item("dz191.orianna.combo.minr").GetValue<Slider>().Value)
                        {
                            var MEC_Circle = MEC.GetMec(group);
                            if (Variables.spells[SpellSlot.Q].IsReady() &&
                                MEC_Circle.Center.Distance(ObjectManager.Player) <= Variables.spells[SpellSlot.Q].Range &&
                                MEC_Circle.Radius <= Variables.spells[SpellSlot.R].Range
                                && MEC_Circle.Center.To3D().CountEnemiesInRange(
                                                    Variables.spells[SpellSlot.R].Range) >= Variables.AssemblyMenu.Item("dz191.orianna.combo.minr").GetValue<Slider>().Value)
                            {
                                Variables.spells[SpellSlot.Q].Cast(MEC_Circle.Center.To3D());
                                LeagueSharp.Common.Utility.DelayAction.Add(
                                    (int)
                                        (Variables.BallManager.BallPosition.Distance(MEC_Circle.Center.To3D()) /
                                         Variables.spells[SpellSlot.Q].Speed * 1000 +
                                         Variables.spells[SpellSlot.Q].Delay * 1000 + Game.Ping / 2f + 125f), () =>
                                         {
                                               if (
                                                Variables.BallManager.BallPosition.CountEnemiesInRange(
                                                    Variables.spells[SpellSlot.R].Range) >= Variables.AssemblyMenu.Item("dz191.orianna.combo.minr").GetValue<Slider>().Value)
                                                {
                                                         Variables.spells[SpellSlot.R].Cast();
                                                }
                                         }
                                        );
                            }
                        }
                    }
                }
                else
                {
                    var target = TargetSelector.GetTarget(Variables.spells[SpellSlot.Q].Range / 1.2f, TargetSelector.DamageType.Magical);

                    if (target.Health + 10f <
                        ObjectManager.Player.GetComboDamage(target, new[] { SpellSlot.Q, SpellSlot.W, SpellSlot.R })
                        && !(target.Health + 10f <
                        ObjectManager.Player.GetComboDamage(target, new[] { SpellSlot.Q, SpellSlot.W})))
                    {
                        var rPosition = target.ServerPosition.Extend(
                            ObjectManager.Player.ServerPosition, Variables.spells[SpellSlot.R].Range / 2f);

                        if (Variables.spells[SpellSlot.Q].IsReady() &&
                            rPosition.Distance(ObjectManager.Player.ServerPosition) <=
                            Variables.spells[SpellSlot.Q].Range)
                        {
                            Variables.spells[SpellSlot.Q].Cast(rPosition);
                            LeagueSharp.Common.Utility.DelayAction.Add(
                                (int)
                                    (Variables.BallManager.BallPosition.Distance(rPosition) /
                                     Variables.spells[SpellSlot.Q].Speed * 1000 +
                                     Variables.spells[SpellSlot.Q].Delay * 1000 + Game.Ping / 2f + 125f), () =>
                                     {
                                         if (
                                             Variables.BallManager.BallPosition.CountEnemiesInRange(
                                                 Variables.spells[SpellSlot.R].Range) > 0)
                                         {
                                                                                      Variables.spells[SpellSlot.R].Cast();
                                         }
                                     });
                        }
                    }
                }
                
            }

            
            if (Variables.AssemblyMenu.Item("dz191.orianna.combo.e").GetValue<bool>() &&
                Variables.spells[SpellSlot.E].IsReady())
            {
                var eTarget = qTarget;
                //Determine the ally to shield with E or me.
                if (ObjectManager.Player.Health <= Helper.GetItemValue<Slider>("dz191.orianna.misc.e.percent").Value 
                    && ObjectManager.Player.CountEnemiesInRange(1100f) > 1)
                {
                    //If we're low life the ball always goes to us.
                    Variables.spells[SpellSlot.E].Cast(ObjectManager.Player);
                    return;
                }

                var lhAllies =
                    HeroManager.Allies.Where(
                        m =>
                            m.HealthPercent < Helper.GetItemValue<Slider>("dz191.orianna.misc.e.percent").Value &&
                            Helper.GetItemValue<bool>($"dz191.orianna.misc.e.shield.{m.ChampionName}") &&
                            m.CountEnemiesInRange(425f) > 1).ToList();

                if (lhAllies.Any())
                {
                    Variables.spells[SpellSlot.E].Cast(lhAllies.OrderBy(m => m.Health).FirstOrDefault());
                    return;
                }

                if (Helper.GetItemValue<bool>("dz191.orianna.misc.e.damage"))
                {
                    foreach (var ally in HeroManager.Allies.Where(ally => ally.IsValidTarget(Variables.spells[SpellSlot.E].Range, false)))
                    {
                        var eHits = Helper.getEHits(ally.ServerPosition);
                        if (eHits.Count() > 2)
                        {
                            Variables.spells[SpellSlot.E].Cast(ally);
                            return;
                        }
                    }
                }
            }


        }

        private static void OnMixed()
        {
            var qTarget = TargetSelector.GetTarget(Variables.spells[SpellSlot.Q].Range / 1.5f, TargetSelector.DamageType.Magical);

            if (Variables.AssemblyMenu.Item("dz191.orianna.mixed.q").GetValue<bool>() && Variables.spells[SpellSlot.Q].IsReady())
            {
                if (qTarget.IsValidTarget())
                {
                    Variables.BallManager.ProcessCommand(new Command()
                    {
                        SpellCommand = Commands.Q,
                        Unit = qTarget
                    });
                }
            }

            if (Variables.AssemblyMenu.Item("dz191.orianna.mixed.w").GetValue<bool>() && Variables.spells[SpellSlot.W].IsReady())
            {
                var ballPosition = Variables.BallManager.BallPosition;
                var minWEnemies = Variables.AssemblyMenu.Item("dz191.orianna.mixed.minw").GetValue<Slider>().Value;

                if (ballPosition.CountEnemiesInRange(Variables.spells[SpellSlot.W].Range) >= minWEnemies)
                {
                    Variables.BallManager.ProcessCommand(new Command()
                    {
                        SpellCommand = Commands.W,
                    });
                }
            }

             if (Variables.AssemblyMenu.Item("dz191.orianna.mixed.e").GetValue<bool>() &&
                Variables.spells[SpellSlot.E].IsReady())
            {
                var eTarget = qTarget;
                //Determine the ally to shield with E or me.
                if (ObjectManager.Player.Health <= Helper.GetItemValue<Slider>("dz191.orianna.misc.e.percent").Value 
                    && ObjectManager.Player.CountEnemiesInRange(1100f) > 1)
                {
                    //If we're low life the ball always goes to us.
                    Variables.spells[SpellSlot.E].Cast(ObjectManager.Player);
                    return;
                }

                var lhAllies =
                    HeroManager.Allies.Where(
                        m =>
                            m.HealthPercent < Helper.GetItemValue<Slider>("dz191.orianna.misc.e.percent").Value &&
                            Helper.GetItemValue<bool>($"dz191.orianna.misc.e.shield.{m.ChampionName}") &&
                            m.CountEnemiesInRange(425f) > 1).ToList();

                if (lhAllies.Any())
                {
                    Variables.spells[SpellSlot.E].Cast(lhAllies.OrderBy(m => m.Health).FirstOrDefault());
                    return;
                }

                if (Helper.GetItemValue<bool>("dz191.orianna.misc.e.damage"))
                {
                    foreach (var ally in HeroManager.Allies.Where(ally => ally.IsValidTarget(Variables.spells[SpellSlot.E].Range, false)))
                    {
                        var eHits = Helper.getEHits(ally.ServerPosition);
                        if (eHits.Count() > 2)
                        {
                            Variables.spells[SpellSlot.E].Cast(ally);
                            return;
                        }
                    }
                }
            }
        }

        private static void OnCastSpell(Spellbook sender, SpellbookCastSpellEventArgs args)
        {
            if (Helper.GetItemValue<bool>("dz191.orianna.misc.r.block") && sender.Owner.IsMe && args.Slot == SpellSlot.R)
            {
                var ballPosition = Variables.BallManager.BallPosition;
                if (ballPosition.CountEnemiesInRange(Variables.spells[SpellSlot.R].Range + 110f) < 1)
                {
                    args.Process = false;   
                }
            }
        }

    }
}
