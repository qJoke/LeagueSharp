using System.Collections.Generic;

namespace SoloVayne.External.Language
{
    interface ILanguage
    {
        string GetLanguage();

        string GetLocalizedString(string name);

        Dictionary<string, string> GetTranslationDictionary();
    }
}
