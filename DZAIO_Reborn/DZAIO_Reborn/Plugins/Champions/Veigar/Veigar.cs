using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DZAIO_Reborn.Core;
using DZAIO_Reborn.Plugins.Interface;
using DZLib.Menu;
using DZLib.MenuExtensions;
using LeagueSharp;
using LeagueSharp.Common;

namespace DZAIO_Reborn.Plugins.Champions.Veigar
{
    class Veigar : IChampion
    {
        public void OnLoad(Menu menu)
        {
            var comboMenu = new Menu(ObjectManager.Player.ChampionName + ": Combo", "dzaio.champion.veigar.combo");
            {
                comboMenu.AddModeMenu(ModesMenuExtensions.Mode.Combo, new[] { SpellSlot.Q, SpellSlot.W, SpellSlot.E, SpellSlot.R }, new[] { true, true, true, true });
                //comboMenu.AddNoUltiMenu(false);
                menu.AddSubMenu(comboMenu);
            }

            var mixedMenu = new Menu(ObjectManager.Player.ChampionName + ": Mixed", "dzaio.champion.veigar.harrass");
            {
                mixedMenu.AddModeMenu(ModesMenuExtensions.Mode.Harrass, new[] { SpellSlot.Q, SpellSlot.W, SpellSlot.E }, new[] { true, true, true });
                mixedMenu.AddSlider("dzaio.champion.veigar.mixed.mana", "Min Mana % for Harass", 30, 0, 100);
                menu.AddSubMenu(mixedMenu);
            }

            var farmMenu = new Menu(ObjectManager.Player.ChampionName + ": Farm", "dzaio.champion.veigar.farm");
            {
                farmMenu.AddModeMenu(ModesMenuExtensions.Mode.Laneclear, new[] { SpellSlot.Q, SpellSlot.W }, new[] { true, true});

                farmMenu.AddSlider("dzaio.champion.veigar.farm.mana", "Min Mana % for Farm", 30, 0, 100);
                menu.AddSubMenu(farmMenu);
            }

            var extraMenu = new Menu(ObjectManager.Player.ChampionName + ": Extra", "dzaio.champion.veigar.extra");
            {
                extraMenu.AddBool("dzaio.champion.veigar.extra.interrupter", "Interrupter (E)", true);
                extraMenu.AddBool("dzaio.champion.veigar.extra.antigapcloser", "Antigapcloser (E)", true);

            }

            Variables.Spells[SpellSlot.Q].SetSkillshot(0.25f, 65f, 1900f, false, SkillshotType.SkillshotLine);
            Variables.Spells[SpellSlot.W].SetSkillshot(1.25f, 190f, 0, false, SkillshotType.SkillshotCircle);
            Variables.Spells[SpellSlot.E].SetSkillshot(0.5f, 335f, 0, false, SkillshotType.SkillshotCircle);
        }

        public void RegisterEvents()
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

        public void OnTick()
        {
            throw new NotImplementedException();
        }

        public void OnCombo()
        {
            throw new NotImplementedException();
        }

        public void OnMixed()
        {
            throw new NotImplementedException();
        }

        public void OnLastHit()
        {
            throw new NotImplementedException();
        }

        public void OnLaneclear()
        {
            throw new NotImplementedException();
        }
    }
}
