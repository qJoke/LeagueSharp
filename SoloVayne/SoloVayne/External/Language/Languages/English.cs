using System.Collections.Generic;

namespace SoloVayne.External.Language.Languages
{
    class English : ILanguage
    {
        public string GetLanguage()
        {
            return "English (English - US)";
        }

        public string GetLocalizedString(string name)
        {
            var dictionary = GetTranslationDictionary();
            if (dictionary.ContainsKey(name) && dictionary[name] != null)
            {
                return dictionary[name];
            }

            return "";
        }

        public Dictionary<string, string> GetTranslationDictionary()
        {
            var Translations = new Dictionary<string, string>()
            {
                {"solo.vayne.mixed.mode", "Harass Mode"},
                {"solo.vayne.laneclear.condemn.jungle", "Condemn Jungle Mobs"},
                {"solo.vayne.misc.tumble.noqintoenemies", "Don't Q into enemies"},
                {"solo.vayne.misc.tumble.qks", "Q for Killsteal"},
                {"solo.vayne.misc.tumble.smartQ", "Use SOLO Vayne Q Logic"},
                {"solo.vayne.misc.condemn.autoe", "Auto E"},
                {"solo.vayne.misc.condemn.current", "Only E Current Target"},
                {"solo.vayne.misc.condemn.save", "SOLO: Save Me"},
                {"solo.vayne.misc.miscellaneous.antigapcloser", "Antigapcloser"},
                {"solo.vayne.misc.miscellaneous.interrupter", "Interrupter"},
            };

            return Translations;
        }
    }
}
