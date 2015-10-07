using System;
using LeagueSharp;
using LeagueSharp.Common;

namespace VayneHunter_Reborn.Utility.MenuUtility
{
    static class MenuExtensions
    {
        public static void AddBool(this Menu menu, string name, string displayName, bool defaultValue = false)
        {
            menu.AddItem(new MenuItem(name, displayName).SetValue(defaultValue));
        }

        public static void AddSlider(this Menu menu, string name, string displayName, Tuple<int, int, int> values)
        {
            menu.AddItem(new MenuItem(name, displayName).SetValue(new Slider(values.Item1, values.Item2, values.Item3)));
        }

        public static void AddKeybind(this Menu menu, string name, string displayName, Tuple<uint, KeyBindType> value)
        {
            menu.AddItem(new MenuItem(name, displayName).SetValue(new KeyBind(value.Item1, value.Item2)));
        }

        public static MenuItem AddText(this Menu menu, string name, string displayName)
        {
            return menu.AddItem(new MenuItem(name, displayName));
        }

        public static void AddStringList(this Menu menu, string name, string displayName, string[] value, int index = 0)
        {
            menu.AddItem(new MenuItem(name, displayName).SetValue(new StringList(value, index)));
        }

        public static void AddSkill(this Menu menu, Enumerations.Skills skill, Orbwalking.OrbwalkingMode mode, bool defValue = true)
        {
            var name = string.Format("dz191.vhr.{0}.use{1}", mode.ToString().ToLower(), skill.ToString().ToLower());
            var displayName = string.Format("Use {0}", skill);
            menu.AddItem(new MenuItem(name, displayName).SetValue(defValue));
        }

        public static void AddManaLimiter(this Menu menu, Enumerations.Skills skill, Orbwalking.OrbwalkingMode mode, int defMana = 0, bool displayMode = false)
        {
            var name = string.Format("dz191.vhr.{0}.mm.{1}.mana", mode.ToString().ToLower(), skill.ToString().ToLower());
            var displayName = displayMode ? string.Format("{0} Mana {1}", skill, mode) : string.Format("{0} Mana", skill);
            menu.AddItem(new MenuItem(name, displayName).SetValue(new Slider(defMana)));
        }

        public static bool IsEnabledAndReady(this Spell spell, Orbwalking.OrbwalkingMode mode, bool checkMana = true)
        {
            var name = string.Format("dz191.vhr.{0}.use{1}", mode.ToString().ToLower(), spell.Slot.ToString().ToLower());
            var mana = string.Format("dz191.vhr.{0}.mm.{1}.mana", mode.ToString().ToLower(), spell.Slot.ToString().ToLower());
            
            if(Variables.Menu.Item(name) != null && Variables.Menu.Item(mana) != null)
            {
                return spell.IsReady()
                    && GetItemValue<bool>(name)
                    && (!checkMana || (ObjectManager.Player.Mana >= GetItemValue<Slider>(mana).Value));
            }
            return false;
        }

        public static T GetItemValue<T>(string item)
        {
            return Variables.Menu.Item(item).GetValue<T>();
        }
    }
}
