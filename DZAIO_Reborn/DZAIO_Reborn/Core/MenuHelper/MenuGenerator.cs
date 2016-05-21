using LeagueSharp.Common;
using TargetSelector = LeagueSharp.Common.TargetSelector;

namespace DZAIO_Reborn.Core.MenuHelper
{
    class MenuGenerator
    {
        public static void GenerateMenu()
        {
            var rootMenu = Variables.AssemblyMenu;
            TargetSelector.AddToMenu(rootMenu);
        }
    }
}
