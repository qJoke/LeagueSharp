using System;
using System.Drawing;
using LeagueSharp.Common;
using VayneHunter_Reborn.External;
using VayneHunter_Reborn.External.Cleanser;
using VayneHunter_Reborn.External.ProfileSelector;
using VayneHunter_Reborn.External.Translation;
using Activator = VayneHunter_Reborn.External.Activator.Activator;

namespace VayneHunter_Reborn.Utility.MenuUtility
{
    class MenuGenerator
    {
        public static void OnLoad()
        {
            var RootMenu = Variables.Menu;

            var OWMenu = new Menu("[VHR] Orbwalker", "dz191.vhr.orbwalker");
            {
                Variables.Orbwalker = new Orbwalking.Orbwalker(OWMenu);
                RootMenu.AddSubMenu(OWMenu);
            }

            var TSMenu = new Menu("[VHR] TS", "dz191.vhr.ts");
            {
                TargetSelector.AddToMenu(TSMenu);
                RootMenu.AddSubMenu(TSMenu);
            }

            var comboMenu = new Menu("[VHR] Combo", "dz191.vhr.combo");
            {
                var manaMenu = new Menu("Mana Manager", "dz191.vhr.combo.mm");
                {
                    manaMenu.AddManaLimiter(Enumerations.Skills.Q, Orbwalking.OrbwalkingMode.Combo);
                    manaMenu.AddManaLimiter(Enumerations.Skills.E, Orbwalking.OrbwalkingMode.Combo);
                    manaMenu.AddManaLimiter(Enumerations.Skills.R, Orbwalking.OrbwalkingMode.Combo);
                    
                    comboMenu.AddSubMenu(manaMenu);
                }

                comboMenu.AddSkill(Enumerations.Skills.Q, Orbwalking.OrbwalkingMode.Combo);
                comboMenu.AddSkill(Enumerations.Skills.E, Orbwalking.OrbwalkingMode.Combo);
                comboMenu.AddSkill(Enumerations.Skills.R, Orbwalking.OrbwalkingMode.Combo, false);
                
                comboMenu.AddSlider("dz191.vhr.combo.r.minenemies", "Min. R Enemies", new Tuple<int, int, int>(2, 1, 5));
                comboMenu.AddBool("dz191.vhr.combo.q.2wstacks","Only Q if 2W Stacks on Target");

                RootMenu.AddSubMenu(comboMenu);
            }

            var harassMenu = new Menu("[VHR] Harass", "dz191.vhr.mixed");
            {
                var manaMenu = new Menu("Mana Manager", "dz191.vhr.mixed.mm");
                {
                    manaMenu.AddManaLimiter(Enumerations.Skills.Q, Orbwalking.OrbwalkingMode.Mixed);
                    manaMenu.AddManaLimiter(Enumerations.Skills.E, Orbwalking.OrbwalkingMode.Mixed);
                    
                    harassMenu.AddSubMenu(manaMenu);
                }

                harassMenu.AddSkill(Enumerations.Skills.Q, Orbwalking.OrbwalkingMode.Mixed);
                harassMenu.AddSkill(Enumerations.Skills.E, Orbwalking.OrbwalkingMode.Mixed);

                harassMenu.AddBool("dz191.vhr.mixed.q.2wstacks", "Only Q if 2W Stacks on Target");
                harassMenu.AddBool("dz191.vhr.mixed.ethird", "Use E for Third Proc");

                RootMenu.AddSubMenu(harassMenu);
            }

            var farmMenu = new Menu("[VHR] Farm", "dz191.vhr.farm");
            {
                farmMenu.AddSkill(Enumerations.Skills.Q, Orbwalking.OrbwalkingMode.LaneClear);
                farmMenu.AddManaLimiter(Enumerations.Skills.Q, Orbwalking.OrbwalkingMode.LaneClear, 45, true);
                farmMenu.AddSkill(Enumerations.Skills.Q, Orbwalking.OrbwalkingMode.LastHit);
                farmMenu.AddManaLimiter(Enumerations.Skills.Q, Orbwalking.OrbwalkingMode.LastHit, 45, true);
                farmMenu.AddBool("dz191.vhr.farm.condemnjungle","Use E to condemn jungle mobs", true);
                farmMenu.AddBool("dz191.vhr.farm.qjungle","Use Q against jungle mobs", true);

                RootMenu.AddSubMenu(farmMenu);
            }

            var miscMenu = new Menu("[VHR] Misc", "dz191.vhr.misc");
            {
                var miscQMenu = new Menu("Misc - Q (Tumble)", "dz191.vhr.misc.tumble");
                {
                    miscQMenu.AddStringList("dz191.vhr.misc.condemn.qlogic", "Q Logic", new[] { "Reborn", "Normal", "Kite melees", "Kurisu"});
                    miscQMenu.AddBool("dz191.vhr.mixed.mirinQ", "Q to Wall when Possible (Mirin Mode)", true);
                    miscQMenu.AddBool("dz191.vhr.misc.tumble.smartq", "Try to QE when possible"); //Done
                    miscQMenu.AddKeybind("dz191.vhr.misc.tumble.noaastealthex", "Don't AA while stealthed", new Tuple<uint, KeyBindType>('K', KeyBindType.Toggle)); //Done
                    miscQMenu.AddBool("dz191.vhr.misc.tumble.noqenemies", "Don't Q into enemies"); //done
                    miscQMenu.AddBool("dz191.vhr.misc.tumble.dynamicqsafety", "Use dynamic Q Safety Distance"); //done
                    miscQMenu.AddBool("dz191.vhr.misc.tumble.qspam", "Ignore Q checks"); //Done
                    miscQMenu.AddBool("dz191.vhr.misc.tumble.qinrange", "Q For KS", true); //Done

                    miscQMenu.AddText("dz191.vhr.misc.tumble.walltumble.warning", "Click and hold Walltumble")
                        .SetFontStyle(FontStyle.Bold, SharpDX.Color.Red);
                    miscQMenu.AddText("dz191.vhr.misc.tumble.walltumble.warning.2",
                        "It will walk to the nearest Tumble spot and Tumble")
                        .SetFontStyle(FontStyle.Bold, SharpDX.Color.Red);
                    miscQMenu.AddKeybind("dz191.vhr.misc.tumble.walltumble", "Tumble Over Wall (WallTumble)",
                        new Tuple<uint, KeyBindType>('Y', KeyBindType.Press));

                    miscMenu.AddSubMenu(miscQMenu);
                }

                var miscEMenu = new Menu("Misc - E (Condemn)", "dz191.vhr.misc.condemn");
                {
                    miscEMenu.AddStringList("dz191.vhr.misc.condemn.condemnmethod", "Condemn Method",
                        new[] {"VH Revolution", "VH Reborn", "Marksman/Gosu", "Shine#"});

                    miscEMenu.AddSlider("dz191.vhr.misc.condemn.pushdistance", "E Push Distance",
                        new Tuple<int, int, int>(395, 350, 470));

                    miscEMenu.AddSlider("dz191.vhr.misc.condemn.accuracy", "Accuracy (Revolution Only)",
                        new Tuple<int, int, int>(33, 1, 100));

                    miscEMenu.AddItem(
                        new MenuItem("dz191.vhr.misc.condemn.enextauto", "E Next Auto").SetValue(
                            new KeyBind('T', KeyBindType.Toggle)));

                    miscEMenu.AddBool("dz191.vhr.misc.condemn.onlystuncurrent", "Only stun current target"); //done
                    miscEMenu.AddBool("dz191.vhr.misc.condemn.autoe", "Auto E"); //Done
                    miscEMenu.AddBool("dz191.vhr.misc.condemn.eks", "Smart E KS"); //Done
                    miscEMenu.AddSlider("dz191.vhr.misc.condemn.noeaa", "Don't E if Target can be killed in X AA",
                        new Tuple<int, int, int>(1, 0, 4)); //Done

                    miscEMenu.AddBool("dz191.vhr.misc.condemn.trinketbush", "Trinket Bush on Condemn", true);
                    miscEMenu.AddBool("dz191.vhr.misc.condemn.lowlifepeel", "Peel with E when low health");
                    miscEMenu.AddBool("dz191.vhr.misc.condemn.condemnflag", "Condemn to J4 flag", true);
                    miscEMenu.AddBool("dz191.vhr.misc.condemn.noeturret", "No E Under enemy turret");

                    miscMenu.AddSubMenu(miscEMenu);
                }

                var miscGeneralSubMenu = new Menu("Misc - General", "dz191.vhr.misc.general"); //Done
                {
                    miscGeneralSubMenu.AddBool("dz191.vhr.misc.general.antigp", "Anti Gapcloser");
                    miscGeneralSubMenu.AddBool("dz191.vhr.misc.general.interrupt", "Interrupter", true);
                    miscGeneralSubMenu.AddSlider("dz191.vhr.misc.general.antigpdelay", "Anti Gapcloser Delay (ms)",
                        new Tuple<int, int, int>(0, 0, 1000));
                        
                    miscGeneralSubMenu.AddBool("dz191.vhr.misc.general.specialfocus", "Focus targets with 2 W marks");
                    miscGeneralSubMenu.AddBool("dz191.vhr.misc.general.reveal", "Stealth Reveal (Pink Ward / Lens)");

                    miscGeneralSubMenu.AddBool("dz191.vhr.misc.general.disablemovement", "Disable Orbwalker Movement");
                    miscGeneralSubMenu.AddBool("dz191.vhr.misc.general.disableattk", "Disable Orbwalker Attack");

                    miscMenu.AddSubMenu(miscGeneralSubMenu);
                }

                RootMenu.AddSubMenu(miscMenu);
            }

            var drawMenu = new Menu("[VHR] Drawings", "dz191.vhr.draw");
            {
                drawMenu.AddBool("dz191.vhr.draw.spots", "Draw Spots", true);
                drawMenu.AddBool("dz191.vhr.draw.range", "Draw Enemy Ranges", true);
                drawMenu.AddBool("dz191.vhr.draw.qpos", "Reborn Q Position (Debug)");

                RootMenu.AddSubMenu(drawMenu);
            }

            CustomAntigapcloser.BuildMenu(RootMenu);
            Activator.LoadMenu();
            Cleanser.LoadMenu(RootMenu);
            ProfileSelector.OnLoad(RootMenu);
            TranslationInterface.OnLoad(RootMenu);

            RootMenu.AddToMainMenu();
        }
    }
}
