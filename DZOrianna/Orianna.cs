using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DZOrianna.Utility;
using DZOrianna.Utility.Ball;
using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;

namespace DZOrianna
{
    class Orianna
    {
        internal static void OnLoad()
        {
            CommandQueue.InitEvents();
            Variables.BallManager = new PetHandler();
            Variables.spells[SpellSlot.Q].SetSkillshot(0f, 110f, 1425f, false, SkillshotType.SkillshotLine);

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

                    break;
            }
        }

        private static void OnCombo()
        {
            if (Variables.AssemblyMenu.Item("dz191.orianna.combo.q").GetValue<bool>() && Variables.spells[SpellSlot.Q].IsReady())
            {
                var qTarget = TargetSelector.GetTarget(Variables.spells[SpellSlot.Q].Range / 1.5f, TargetSelector.DamageType.Magical);

                if (qTarget.IsValidTarget())
                {
                    Variables.BallManager.ProcessCommand(new Command()
                    {
                        SpellCommand = Commands.Q,
                        Unit = qTarget
                    });
                }
            }

            if (Variables.AssemblyMenu.Item("dz191.orianna.combo.w").GetValue<bool>() && Variables.spells[SpellSlot.W].IsReady())
            {
                var ballPosition = Variables.BallManager.BallPosition;
                var minWEnemies = Variables.AssemblyMenu.Item("dz191.orianna.combo.minw").GetValue<Slider>().Value;

                if (ballPosition.CountEnemiesInRange(Variables.spells[SpellSlot.W].Range) >= minWEnemies)
                {
                    Variables.BallManager.ProcessCommand(new Command()
                    {
                        SpellCommand = Commands.W,
                    });
                }
            }

            if (Variables.AssemblyMenu.Item("dz191.orianna.combo.e").GetValue<bool>() &&
                Variables.spells[SpellSlot.E].IsReady())
            {
                //TODO Collision Check
            }

            if (Variables.AssemblyMenu.Item("dz191.orianna.combo.r").GetValue<bool>() &&
                Variables.spells[SpellSlot.R].IsReady())
            {
                if (ObjectManager.Player.CountEnemiesInRange(Variables.spells[SpellSlot.Q].Range) > 1)
                {
                    var enemyHeroesPositions = HeroManager.Enemies.Select(hero => hero.Position.To2D()).ToList();

                    var Groups = GetCombinations(enemyHeroesPositions);

                    foreach (var group in Groups)
                    {
                        if (group.Count >=
                            Variables.AssemblyMenu.Item("dz191.orianna.combo.minr").GetValue<Slider>().Value)
                        {
                            var MEC_Circle = MEC.GetMec(group);
                            if (Variables.spells[SpellSlot.Q].IsReady() &&
                                MEC_Circle.Center.Distance(ObjectManager.Player) <= Variables.spells[SpellSlot.Q].Range &&
                                MEC_Circle.Radius <= Variables.spells[SpellSlot.R].Range)
                            {
                                Variables.spells[SpellSlot.Q].Cast(MEC_Circle.Center.To3D());
                                LeagueSharp.Common.Utility.DelayAction.Add(
                                    (int)
                                        (Variables.BallManager.BallPosition.Distance(MEC_Circle.Center.To3D()) /
                                         Variables.spells[SpellSlot.Q].Speed * 1000 +
                                         Variables.spells[SpellSlot.Q].Delay * 1000 + Game.Ping / 2f + 125f),
                                    () => Variables.spells[SpellSlot.R].Cast());
                            }
                        }
                    }
                }
                else
                {
                    var target = TargetSelector.GetTarget(Variables.spells[SpellSlot.Q].Range / 1.2f, TargetSelector.DamageType.Magical);

                    if (target.Health + 10f <
                        ObjectManager.Player.GetComboDamage(target, new[] { SpellSlot.Q, SpellSlot.W, SpellSlot.R }))
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
                                     Variables.spells[SpellSlot.Q].Delay * 1000 + Game.Ping / 2f + 125f),
                                () => Variables.spells[SpellSlot.R].Cast());
                        }
                    }
                }
                
            }

        }

        private static void OnMixed()
        {
            
        }

        private static IEnumerable<List<Vector2>> GetCombinations(IReadOnlyCollection<Vector2> allValues)
        {
            var collection = new List<List<Vector2>>();
            for (var counter = 0; counter < (1 << allValues.Count); ++counter)
            {
                var combination = allValues.Where((t, i) => (counter & (1 << i)) == 0).ToList();

                collection.Add(combination);
            }

            return collection;
        }

    }
}
