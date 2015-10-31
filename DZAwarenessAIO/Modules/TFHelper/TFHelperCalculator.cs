using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DZAwarenessAIO.Modules.TFHelper
{
    class TFHelperCalculator
    {
        public static float GetAllyStrength()
        {
            return 0.99f;
        }

        public static float GetEnemyStrength()
        {
            return 0.01f;
        }

        public static string GetText()
        {
            return string.Format("{0}v{1}: {2} will win", TFHelperVariables.AlliesClose.Count(), TFHelperVariables.EnemiesClose.Count(), GetAllyStrength() > GetEnemyStrength() ? "Ally" : "Enemy");
        }
    }
}
