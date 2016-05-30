using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;

namespace DZAIO_Reborn.Helpers.Positioning
{
    static class PositioningHelper
    {
        internal static bool IsSafe(this Vector3 Position)
        {
            if ((Position.UnderTurret(true) 
                && !ObjectManager.Player.UnderTurret(true)) 
                || (PositioningVariables.EnemiesClose.Count() > 1 && DZAIOGeometry.GetEnemyPoints().Contains(Position.To2D())))
            {
                return false;
            }

            return true;

        }
    }
}
