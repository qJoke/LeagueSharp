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
using DZAIO_Reborn.Plugins.Interface;
using DZLib.Core;
using DZLib.Menu;
using DZLib.MenuExtensions;
using DZLib.Positioning;
using LeagueSharp;
using LeagueSharp.Common;
using SPrediction;

namespace DZAIO_Reborn.Plugins.Champions.Diana
{
    class Diana : IChampion
    {
        private Dictionary<SpellSlot, Spell> spells => Variables.Spells;
 
        public void OnLoad(Menu menu)
        {
            var comboMenu = new Menu(ObjectManager.Player.ChampionName + ": Combo", "dzaio.champion.diana.combo");
            {
                comboMenu.AddModeMenu(ModesMenuExtensions.Mode.Combo, new[] { SpellSlot.Q, SpellSlot.W, SpellSlot.E, SpellSlot.R }, new[] { true, true, true, true });
                comboMenu.AddBool("dzaio.champion.diana.combo.qr", "Use QR Combo", true);
                menu.AddSubMenu(comboMenu);
            }

            var mixedMenu = new Menu(ObjectManager.Player.ChampionName + ": Mixed", "dzaio.champion.diana.harrass");
            {
                mixedMenu.AddModeMenu(ModesMenuExtensions.Mode.Harrass, new[] { SpellSlot.Q, SpellSlot.W, SpellSlot.E }, new[] { true, true, true });
                mixedMenu.AddSlider("dzaio.champion.diana.mixed.mana", "Min Mana % for Harass", 30, 0, 100);
                menu.AddSubMenu(mixedMenu);
            }

            var farmMenu = new Menu(ObjectManager.Player.ChampionName + ": Farm", "dzaio.champion.diana.farm");
            {
                farmMenu.AddModeMenu(ModesMenuExtensions.Mode.Laneclear, new[] { SpellSlot.Q, SpellSlot.W, SpellSlot.E }, new[] { true, true, true });

                farmMenu.AddSlider("dzaio.champion.diana.farm.w.min", "Min Minions for W", 2, 1, 6);
                farmMenu.AddSlider("dzaio.champion.diana.farm.mana", "Min Mana % for Farm", 30, 0, 100);
                menu.AddSubMenu(farmMenu);
            }

            var extraMenu = new Menu(ObjectManager.Player.ChampionName + ": Extra", "dzaio.champion.diana.extra");
            {
                extraMenu.AddBool("dzaio.champion.diana.extra.interrupter", "Interrupter (E)", true);
                extraMenu.AddBool("dzaio.champion.diana.extra.antigapcloser", "Antigapcloser (E)", true);
                extraMenu.AddBool("dzaio.champion.diana.extra.autoQ", "Auto Q Stunned / Rooted", true);
                extraMenu.AddBool("dzaio.champion.diana.extra.autoQKS", "Auto Q KS", true);
            }

            Variables.Spells[SpellSlot.Q].SetSkillshot(0.25f, 185f, 1620f, false, SkillshotType.SkillshotCircle);
        }

        public void RegisterEvents()
        {
            DZInterrupter.OnInterruptableTarget += OnInterrupter;
            DZAntigapcloser.OnEnemyGapcloser += OnGapcloser;
        }

        private void OnGapcloser(DZLib.Core.ActiveGapcloser gapcloser)
        {
            if (Variables.AssemblyMenu.GetItemValue<bool>("dzaio.champion.diana.extra.antigapcloser")
                && ObjectManager.Player.ManaPercent > 20
                && gapcloser.End.Distance(ObjectManager.Player.ServerPosition) < spells[SpellSlot.E].Range
                && gapcloser.Sender.IsValidTarget(Variables.Spells[SpellSlot.E].Range)
                && Variables.Spells[SpellSlot.E].IsReady())
            {
                Variables.Spells[SpellSlot.E].Cast();
            }
        }

        private void OnInterrupter(Obj_AI_Hero sender, DZInterrupter.InterruptableTargetEventArgs args)
        {
            if (Variables.AssemblyMenu.GetItemValue<bool>("dzaio.champion.diana.extra.interrupter")
                && ObjectManager.Player.ManaPercent > 20
                && args.DangerLevel > DZInterrupter.DangerLevel.Medium
                && sender.IsValidTarget(Variables.Spells[SpellSlot.E].Range)
                && Variables.Spells[SpellSlot.E].IsReady())
            {
                Variables.Spells[SpellSlot.E].Cast();
            }
        }
        public Dictionary<SpellSlot, Spell> GetSpells()
        {
            return new Dictionary<SpellSlot, Spell>
                      {
                            { SpellSlot.Q, new Spell(SpellSlot.Q, 830f) },
                            { SpellSlot.W, new Spell(SpellSlot.W, 250f) },
                            { SpellSlot.E, new Spell(SpellSlot.E, 450f) },
                            { SpellSlot.R, new Spell(SpellSlot.R, 800f) }
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
            var target = TargetSelector.GetTarget(Variables.Spells[SpellSlot.Q].Range, TargetSelector.DamageType.Magical);

            if (target.IsValidTarget() && Variables.Spells[SpellSlot.Q].IsEnabledAndReady(ModesMenuExtensions.Mode.Combo))
            {
                var prediction = SPrediction.ArcPrediction.GetPrediction(target, Variables.Spells[SpellSlot.Q].Width,
                    Variables.Spells[SpellSlot.Q].Delay, Variables.Spells[SpellSlot.Q].Speed,
                    Variables.Spells[SpellSlot.Q].Range, false);

                if (prediction.HitChance > HitChance.Medium)
                {
                    var endPosition = prediction.CastPosition;

                    Variables.Spells[SpellSlot.Q].Cast(endPosition);
                }
            }

            if (Variables.AssemblyMenu.GetItemValue<bool>("dzaio.champion.diana.combo.qr"))
            {
                var killableTarget =
                   HeroManager.Enemies.FirstOrDefault(enemy => Variables.Spells[SpellSlot.R].IsKillable(enemy) 
                           && enemy.IsValidTarget(Variables.Spells[SpellSlot.R].Range * 2f));

                if (killableTarget != null)
                {
                    QR(killableTarget);
                }
            }

            if (target.IsValidTarget() && Variables.Spells[SpellSlot.R].IsEnabledAndReady(ModesMenuExtensions.Mode.Combo))
            {
                if (HasQBuff(target) && !target.UnderTurret(true) && target.ServerPosition.IsSafe())
                {
                    Variables.Spells[SpellSlot.R].Cast(target);
                }
            }

            if (Variables.Spells[SpellSlot.W].IsEnabledAndReady(ModesMenuExtensions.Mode.Combo))
            {
                if (!target.IsValidTarget(Orbwalking.GetRealAutoAttackRange(ObjectManager.Player))
                    && !ObjectManager.Player.IsDashing())
                {
                    Variables.Spells[SpellSlot.W].Cast(); ;
                }
            }

            if (Variables.Spells[SpellSlot.E].IsEnabledAndReady(ModesMenuExtensions.Mode.Combo))
            {
                if (!target.IsValidTarget(Variables.Spells[SpellSlot.E].Range)
                    && !ObjectManager.Player.IsDashing())
                {
                    Variables.Spells[SpellSlot.E].Cast(); ;
                }
            }


        }

        public void OnMixed()
        {
            if (ObjectManager.Player.ManaPercent <
                Variables.AssemblyMenu.GetItemValue<Slider>("dzaio.champion.diana.mixed.mana").Value)
            {
                return;
            }
            var target = TargetSelector.GetTarget(Variables.Spells[SpellSlot.Q].Range, TargetSelector.DamageType.Magical);

            if (target.IsValidTarget() && Variables.Spells[SpellSlot.Q].IsEnabledAndReady(ModesMenuExtensions.Mode.Combo))
            {
                var prediction = SPrediction.ArcPrediction.GetPrediction(target, Variables.Spells[SpellSlot.Q].Width,
                    Variables.Spells[SpellSlot.Q].Delay, Variables.Spells[SpellSlot.Q].Speed,
                    Variables.Spells[SpellSlot.Q].Range, false);

                if (prediction.HitChance > HitChance.Medium)
                {
                    var endPosition = prediction.CastPosition;

                    Variables.Spells[SpellSlot.Q].Cast(endPosition);
                }
            }

            if (Variables.Spells[SpellSlot.W].IsEnabledAndReady(ModesMenuExtensions.Mode.Combo))
            {
                if (!target.IsValidTarget(Orbwalking.GetRealAutoAttackRange(ObjectManager.Player))
                    && !ObjectManager.Player.IsDashing())
                {
                    Variables.Spells[SpellSlot.W].Cast(); ;
                }
            }

            if (Variables.Spells[SpellSlot.E].IsEnabledAndReady(ModesMenuExtensions.Mode.Combo))
            {
                if (!target.IsValidTarget(Variables.Spells[SpellSlot.E].Range)
                    && !ObjectManager.Player.IsDashing())
                {
                    Variables.Spells[SpellSlot.E].Cast(); ;
                }
            }
        }

        public void OnLastHit()
        { }

        public void OnLaneclear()
        {
            if (ObjectManager.Player.ManaPercent <
                Variables.AssemblyMenu.GetItemValue<Slider>("dzaio.champion.diana.farm.mana").Value)
            {
                return;
            }

            var minions = MinionManager.GetMinions(ObjectManager.Player.ServerPosition, Variables.Spells[SpellSlot.Q].Range, MinionTypes.All,  MinionTeam.NotAlly);

            var qMinGroups = minions.FindAll(m => m.IsValidTarget(Variables.Spells[SpellSlot.Q].Range));
            var qMinion = qMinGroups.Find(m => m.IsValidTarget());

            if (Variables.Spells[SpellSlot.W].IsEnabledAndReady(ModesMenuExtensions.Mode.Laneclear)
                && spells[SpellSlot.Q].GetCircularFarmLocation(minions).MinionsHit >= 2)
            {
                spells[SpellSlot.Q].Cast(qMinion);
            }

            if (Variables.Spells[SpellSlot.W].IsEnabledAndReady(ModesMenuExtensions.Mode.Laneclear)
                && spells[SpellSlot.W].GetCircularFarmLocation(minions).MinionsHit >= 2)
            {
                spells[SpellSlot.W].Cast();
            }

            if (Variables.Spells[SpellSlot.E].IsEnabledAndReady(ModesMenuExtensions.Mode.Laneclear) && qMinion.IsValidTarget(200f)
                && spells[SpellSlot.E].GetCircularFarmLocation(minions).MinionsHit >= 3)
            {
                spells[SpellSlot.E].Cast();
            }
        }

        public void QR(Obj_AI_Hero target)
        {
            if (!target.IsValidTarget(Variables.Spells[SpellSlot.R].Range))
            {
                return;
            }

            if (Variables.Spells[SpellSlot.Q].IsReady() && Variables.Spells[SpellSlot.R].IsReady())
            {
                var targetMinion = MinionManager.GetMinions(ObjectManager.Player.ServerPosition, Variables.Spells[SpellSlot.R].Range).OrderBy(x => x.Distance(target))
                        .FirstOrDefault(minion => !Variables.Spells[SpellSlot.Q].IsKillable(minion));

                if (targetMinion.IsValidTarget())
                {
                    Variables.Spells[SpellSlot.Q].Cast(targetMinion);
                    if (HasQBuff(targetMinion))
                    {
                        Variables.Spells[SpellSlot.R].Cast(targetMinion);
                    }
                }
            }
        }

        public double GetComboDamage(Obj_AI_Hero Target)
        {
            var IgniteDamage = ObjectManager.Player.GetSummonerSpellDamage(Target, Damage.SummonerSpell.Ignite);
            return ObjectManager.Player.GetComboDamage(Target,
                new[] {SpellSlot.Q, SpellSlot.W, SpellSlot.E, SpellSlot.R}) + IgniteDamage;
        }

        private static bool HasQBuff(Obj_AI_Base target)
        {
            return target.HasBuff("dianamoonlight");
        }

    }
}
