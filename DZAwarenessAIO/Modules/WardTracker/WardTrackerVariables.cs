using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX;

namespace DZAwarenessAIO.Modules.WardTracker
{
    class WardTrackerVariables
    {
        public static Dictionary<WardType, float> wardDurations = new Dictionary<WardType, float>()
        {
            { WardType.Green, 90 },
            { WardType.Trinket, 90 },
            { WardType.Pink, float.MaxValue }
        };

        public List<Ward> detectedWards = new List<Ward>();
    }

    class Ward
    {
        public Vector3 Position { get; set; }

        public int startTick { get; set; }

        public WardType WardType { get; set; }
    }

    enum WardType
    {
        Trinket, Pink, Green
    }
}
