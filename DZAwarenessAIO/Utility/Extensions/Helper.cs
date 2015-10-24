using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;

namespace DZAwarenessAIO.Utility.Extensions
{
    /// <summary>
    /// The Helper Class
    /// </summary>
    class Helper
    {
        public static Vector3[] GetPolygonVertices(Vector3 centerPosition, int VerticesNumbers, float polygonHalfDiagonal, float baseAngle)
        {
            var PolygonPoints = new List<Vector3>();
            var currentAngle = baseAngle;
            var RotationStep = 360f / VerticesNumbers;

            for (var i = baseAngle; i <= baseAngle + 360f; i += RotationStep)
            {
                PolygonPoints.Add(ConvertToPosition(centerPosition, currentAngle, polygonHalfDiagonal));
                currentAngle += RotationStep;
            }

            return PolygonPoints.ToArray();
        }

        private static Vector3 ConvertToPosition(Vector3 from, float rotation, float polygonHalfDiagonal)
        {
            var angle = LeagueSharp.Common.Geometry.DegreeToRadian(rotation);
            return new Vector2(
                (int)(Math.Cos(angle) * polygonHalfDiagonal + from.X),
                (int)(Math.Sin(-angle) * polygonHalfDiagonal + from.Y)).To3D();
        }
    }
}
