using System.Collections.Generic;
using System.Linq;
using DZAIO_Reborn.Core;
using DZAIO_Reborn.Helpers;
using DZAIO_Reborn.Helpers.Modules;
using DZAIO_Reborn.Plugins.Interface;
using DZLib.Core;
using DZLib.Menu;
using DZLib.MenuExtensions;
using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;

namespace DZAIO_Reborn.Plugins.Champions.Bard
{
    class Bard : IChampion
    {
        public void OnLoad(Menu menu)
        {
            var comboMenu = new Menu(ObjectManager.Player.ChampionName + ": Combo", "dzaio.champion.bard.combo");
            {
                comboMenu.AddModeMenu(ModesMenuExtensions.Mode.Combo, new[] { SpellSlot.Q, SpellSlot.R }, new[] { true, true });
                comboMenu.AddSlider("dzaio.champion.bard.combo.r.min", "Min Enemies for R", 2, 1, 5);

                menu.AddSubMenu(comboMenu);
            }

            var mixedMenu = new Menu(ObjectManager.Player.ChampionName + ": Mixed", "dzaio.champion.bard.harrass");
            {
                mixedMenu.AddModeMenu(ModesMenuExtensions.Mode.Harrass, new[] { SpellSlot.Q}, new[] { true });
                mixedMenu.AddSlider("dzaio.champion.bard.mixed.mana", "Min Mana % for Harass", 30, 0, 100);
                menu.AddSubMenu(mixedMenu);
            }

            var farmMenu = new Menu(ObjectManager.Player.ChampionName + ": Farm", "dzaio.champion.bard.farm");
            {
                farmMenu.AddModeMenu(ModesMenuExtensions.Mode.Laneclear, new[] { SpellSlot.Q }, new[] { true });

                farmMenu.AddSlider("dzaio.champion.bard.farm.w.min", "Min Minions for Q", 2, 1, 6);
                farmMenu.AddSlider("dzaio.champion.bard.farm.mana", "Min Mana % for Farm", 30, 0, 100);
                menu.AddSubMenu(farmMenu);
            }

            var extraMenu = new Menu(ObjectManager.Player.ChampionName + ": Extra", "dzaio.champion.bard.extra");
            {
                extraMenu.AddBool("dzaio.champion.bard.extra.interrupter", "Interrupter (Q)", true);
                extraMenu.AddBool("dzaio.champion.bard.extra.antigapcloser", "Antigapcloser (Q)", true);
                extraMenu.AddBool("dzaio.champion.bard.extra.autoQ", "Auto Q Stunned / Rooted", true);
                extraMenu.AddBool("dzaio.champion.bard.extra.autoQKS", "Auto Q KS", true);
                extraMenu.AddBool("dzaio.champion.bard.extra.supportmode", "Support Mode", true);
            }

            Variables.Spells[SpellSlot.Q].SetSkillshot(0.25f, 100, 1600, false, SkillshotType.SkillshotLine);
            Variables.Spells[SpellSlot.E].SetSkillshot(0.25f, 60, 1200, true, SkillshotType.SkillshotLine);
        }

        public void RegisterEvents()
        {
            DZInterrupter.OnInterruptableTarget += OnInterrupter;
            DZAntigapcloser.OnEnemyGapcloser += OnGapcloser;
            Orbwalking.BeforeAttack += BeforeAttack;
        }

        private void BeforeAttack(Orbwalking.BeforeAttackEventArgs args)
        {

            if (args.Target.Type == GameObjectType.obj_AI_Minion
            && (Variables.Orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.Mixed)
            && Variables.AssemblyMenu.GetItemValue<bool>("dzaio.champion.bard.extra.supportmode"))
            {
                if (ObjectManager.Player.CountAlliesInRange(Variables.AssemblyMenu.GetItemValue<Slider>("dz191.bard.misc.attackMinionRange").Value) > 0)
                {
                    args.Process = false;
                }
            }
        }

        private void OnGapcloser(DZLib.Core.ActiveGapcloser gapcloser)
        {
            
        }

        private void OnInterrupter(Obj_AI_Hero sender, DZInterrupter.InterruptableTargetEventArgs args)
        {
            
        }
        public Dictionary<SpellSlot, Spell> GetSpells()
        {
            return new Dictionary<SpellSlot, Spell>
                      {
                            {SpellSlot.Q, new Spell(SpellSlot.Q, 950f)},
                            {SpellSlot.W, new Spell(SpellSlot.W, 945f)},
                            {SpellSlot.E, new Spell(SpellSlot.E, float.MaxValue)}
                      };
        }

        public List<IModule> GetModules()
        {
            return new List<IModule>()
            {

            };
        }

        public void OnTick()
        {

        }

        public void OnCombo()
        {
            var ComboTarget = TargetSelector.GetTarget(Variables.Spells[SpellSlot.Q].Range / 1.3f, TargetSelector.DamageType.Magical);

            if (Variables.Spells[SpellSlot.Q].IsEnabledAndReady(ModesMenuExtensions.Mode.Combo) && ComboTarget.IsValidTarget())
            {
                HandleQ(ComboTarget);
            }
        }

        public void OnMixed()
        {

        }

        public void OnLastHit()
        { }

        public void OnLaneclear()
        {

        }

        public void HandleQ(Obj_AI_Hero comboTarget)
        {
            var QPrediction = Variables.Spells[SpellSlot.Q].GetPrediction(comboTarget);

            if (QPrediction.Hitchance >= HitChance.High)
            {
                var QPushDistance = 250;
                var QAccuracy = 20;
                var PlayerPosition = ObjectManager.Player.ServerPosition;

                var BeamStartPositions = new List<Vector3>()
                    {
                        QPrediction.CastPosition,
                        QPrediction.UnitPosition,
                        comboTarget.ServerPosition,
                        comboTarget.Position
                    };

                if (comboTarget.IsDashing())
                {
                    BeamStartPositions.Add(comboTarget.GetDashInfo().EndPos.To3D());
                }

                var PositionsList = new List<Vector3>();
                var CollisionPositions = new List<Vector3>();

                foreach (var position in BeamStartPositions)
                {
                    var collisionableObjects = Variables.Spells[SpellSlot.Q].GetCollision(position.To2D(),
                        new List<Vector2>() { position.Extend(PlayerPosition, -QPushDistance).To2D() });

                    if (collisionableObjects.Any())
                    {
                        if (collisionableObjects.Any(h => h is Obj_AI_Hero) &&
                            (collisionableObjects.All(h => h.IsValidTarget())))
                        {
                            Variables.Spells[SpellSlot.Q].Cast(QPrediction.CastPosition);
                            break;
                        }

                        for (var i = 0; i < QPushDistance; i += (int)comboTarget.BoundingRadius)
                        {
                            CollisionPositions.Add(position.Extend(PlayerPosition, -i));
                        }
                    }

                    for (var i = 0; i < QPushDistance; i += (int)comboTarget.BoundingRadius)
                    {
                        PositionsList.Add(position.Extend(PlayerPosition, -i));
                    }
                }

                if (PositionsList.Any())
                {
                    //We don't want to divide by 0 Kappa
                    var WallNumber = PositionsList.Count(p => p.IsWall()) * 1.3f;
                    var CollisionPositionCount = CollisionPositions.Count;
                    var Percent = (WallNumber + CollisionPositionCount) / PositionsList.Count;
                    var AccuracyEx = QAccuracy / 100f;
                    if (Percent >= AccuracyEx)
                    {
                        Variables.Spells[SpellSlot.Q].Cast(QPrediction.CastPosition);
                    }

                }
            }
            else if (QPrediction.Hitchance == HitChance.Collision)
            {
                var QCollision = QPrediction.CollisionObjects;
                if (QCollision.Count == 1)
                {
                    Variables.Spells[SpellSlot.Q].Cast(QPrediction.CastPosition);
                }
            }
        }
    }
}
