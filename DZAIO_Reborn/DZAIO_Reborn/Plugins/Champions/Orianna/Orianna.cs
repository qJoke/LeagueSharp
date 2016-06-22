using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DZAIO_Reborn.Core;
using DZAIO_Reborn.Helpers;
using DZAIO_Reborn.Helpers.Entity;
using DZAIO_Reborn.Helpers.Modules;
using DZAIO_Reborn.Helpers.Positioning;
using DZAIO_Reborn.Plugins.Champions.Orianna.BallManager;
using DZAIO_Reborn.Plugins.Champions.Veigar.Modules;
using DZAIO_Reborn.Plugins.Interface;
using DZLib.Core;
using DZLib.Menu;
using DZLib.MenuExtensions;
using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;
using SPrediction;

namespace DZAIO_Reborn.Plugins.Champions.Orianna
{
    class Orianna : IChampion
    {
        public PetManager BallManager;
 
        public void OnLoad(Menu menu)
        {
            var comboMenu = new Menu(ObjectManager.Player.ChampionName + ": Combo", "dzaio.champion.orianna.combo");
            {
                comboMenu.AddModeMenu(ModesMenuExtensions.Mode.Combo, new[] { SpellSlot.Q, SpellSlot.W, SpellSlot.E, SpellSlot.R }, new[] { true, true, true, true });
                //comboMenu.AddNoUltiMenu(false);
                menu.AddSubMenu(comboMenu);
            }

            var mixedMenu = new Menu(ObjectManager.Player.ChampionName + ": Mixed", "dzaio.champion.orianna.harrass");
            {
                mixedMenu.AddModeMenu(ModesMenuExtensions.Mode.Harrass, new[] { SpellSlot.Q, SpellSlot.W, SpellSlot.E }, new[] { true, true, true });
                mixedMenu.AddSlider("dzaio.champion.veigar.mixed.mana", "Min Mana % for Harass", 30, 0, 100);
                menu.AddSubMenu(mixedMenu);
            }

            var farmMenu = new Menu(ObjectManager.Player.ChampionName + ": Farm", "dzaio.champion.orianna.farm");
            {
                farmMenu.AddModeMenu(ModesMenuExtensions.Mode.Laneclear, new[] { SpellSlot.Q, SpellSlot.W }, new[] { true, true });

                farmMenu.AddBool("dzaio.champion.veigar.farm.w.kill", "Only Use W to kill Minions", false);
                farmMenu.AddSlider("dzaio.champion.veigar.farm.w.min", "Min Minions for W", 2, 1, 6);
                farmMenu.AddSlider("dzaio.champion.veigar.farm.mana", "Min Mana % for Farm", 30, 0, 100);
                menu.AddSubMenu(farmMenu);
            }

            var extraMenu = new Menu(ObjectManager.Player.ChampionName + ": Extra", "dzaio.champion.orianna.extra");
            {
                extraMenu.AddBool("dzaio.champion.veigar.extra.interrupter", "Interrupter (R)", true);
            }

            Variables.Spells[SpellSlot.Q].SetSkillshot(0f, 110f, 1425f, false, SkillshotType.SkillshotLine);
            Variables.Spells[SpellSlot.E].SetSkillshot(0.25f, 80f, 1700f, true, SkillshotType.SkillshotLine);
            CommandQueue.InitEvents();
            BallManager = new PetManager();
            BallManager.OnLoad();
        }

        public void RegisterEvents()
        {
            DZInterrupter.OnInterruptableTarget += OnInterrupter;
            DZAntigapcloser.OnEnemyGapcloser += OnGapcloser;
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
                                    { SpellSlot.Q, new Spell(SpellSlot.Q, 800f) },
                                    { SpellSlot.W, new Spell(SpellSlot.W, 1000f) },
                                    { SpellSlot.E, new Spell(SpellSlot.E, 700f) },
                                    { SpellSlot.R, new Spell(SpellSlot.R, 640f) }
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
            var qTarget = TargetSelector.GetTarget(Variables.Spells[SpellSlot.Q].Range / 1.5f, TargetSelector.DamageType.Magical);

            if (Variables.Spells[SpellSlot.Q].IsEnabledAndReady(ModesMenuExtensions.Mode.Combo) && qTarget.IsValidTarget())
            {
                var targetPrediction = LeagueSharp.Common.Prediction.GetPrediction(qTarget, 0.75f);

                if (ObjectManager.Player.HealthPercent >= 35)
                {
                    var enemyHeroesPositions = HeroManager.Enemies.Select(hero => hero.Position.To2D()).ToList();

                    var Groups = PositioningHelper.GetCombinations(enemyHeroesPositions);

                    foreach (var group in Groups)
                    {
                        if (group.Count >= 3)
                        {
                            var Circle = MEC.GetMec(group);

                            if (Circle.Center.To3D().CountEnemiesInRange(Variables.Spells[SpellSlot.Q].Range) >= 2 &&
                                Circle.Center.Distance(ObjectManager.Player) <= Variables.Spells[SpellSlot.Q].Range &&
                                Circle.Radius <= Variables.Spells[SpellSlot.Q].Width)
                            {
                                this.BallManager.ProcessCommand(new Command()
                                {
                                    SpellCommand = Commands.Q,
                                    Where = Circle.Center.To3D()
                                });
                                return;
                            }
                        }
                    }

                }

                this.BallManager.ProcessCommand(new Command()
                {
                    SpellCommand = Commands.Q,
                    Where = targetPrediction.UnitPosition
                });


            }

            if (Variables.Spells[SpellSlot.W].IsEnabledAndReady(ModesMenuExtensions.Mode.Combo) &&
                qTarget.IsValidTarget(Variables.Spells[SpellSlot.W].Range))
            {
                var ballLocation = this.BallManager.BallPosition;
                var minWEnemies = 2;

                if (ObjectManager.Player.CountEnemiesInRange(Variables.Spells[SpellSlot.Q].Range + 245f) >= 2)
                {
                    if (ballLocation.CountEnemiesInRange(Variables.Spells[SpellSlot.W].Range) >= minWEnemies)
                    {
                        this.BallManager.ProcessCommand(new Command() { SpellCommand = Commands.W, });
                    }
                }
                else
                {
                    if (ballLocation.CountEnemiesInRange(Variables.Spells[SpellSlot.W].Range) >= 1)
                    {
                        this.BallManager.ProcessCommand(new Command() { SpellCommand = Commands.W, });
                    }
                }
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
        public static List<Obj_AI_Hero> getEHits(Vector3 endPosition)
        {
            return HeroManager.Enemies
                .Where(enemy => enemy.IsValidTarget(Variables.Spells[SpellSlot.E].Range * 1.45f) && Variables.Spells[SpellSlot.E].WillHit(enemy, endPosition))
                .ToList();
        }
    }
}
