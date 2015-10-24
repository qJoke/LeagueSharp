using System;
using System.Collections.Generic;
using DZAwarenessAIO.Utility.Logs;
using DZAwarenessAIO.Utility.MenuUtility;
using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;

namespace DZAwarenessAIO.Modules.WardTracker
{
    class WardTrackerVariables
    {
        public static Dictionary<WardType, float> wardDurations = new Dictionary<WardType, float>()
        {
            { WardType.Green, 90 },
            { WardType.Trinket, 60 },
            { WardType.Pink, float.MaxValue }
        };

        public static List<WardTypeWrapper> wrapperTypes = new List<WardTypeWrapper>
        {
            new WardTypeWrapper
            {
                ObjectName = "YellowTrinket",
                SpellName = "TrinketTotemLvl1",
                WardType = WardType.Trinket,
                WardVisionRange = 1100
            },


        };

        public static List<Ward> detectedWards = new List<Ward>();

    }

    class Ward
    {
        public Vector3 Position { get; set; }

        public float startTick { get; set; }

        public WardTypeWrapper WardTypeW { get; set; }

        public Render.Rectangle RectangleObject { get; set; }

        public Render.Text TextObject { get; set; }

        public Ward()
        {
            CreateRenderObjects();
        }

        public void CreateRenderObjects()
        {
            RectangleObject = new Render.Rectangle((int) Drawing.WorldToScreen(Position).X, (int) Drawing.WorldToScreen(Position).Y, 65, 65, SharpDX.Color.Red)
            {
                VisibleCondition = sender => Render.OnScreen(Drawing.WorldToScreen(Position)) && MenuExtensions.GetItemValue<bool>("dz191.dza.ward.track")
            };
            RectangleObject.Add(0);

            TextObject = new Render.Text((int)Drawing.WorldToScreen(Position).X, (int)Drawing.WorldToScreen(Position).Y, "", 12, new ColorBGRA(255, 255, 255, 255))
            {
                VisibleCondition = sender => Render.OnScreen(Drawing.WorldToScreen(Position)) && MenuExtensions.GetItemValue<bool>("dz191.dza.ward.track"),
                TextUpdate = () => (Game.Time < startTick + WardTypeW.WardDuration) ? (Utils.FormatTime((startTick + WardTypeW.WardDuration) / 1000f)) : string.Empty
            };
            TextObject.Add(0);
        }

        public void RemoveRenderObjects()
        {
            RectangleObject.Remove();
            TextObject.Remove();
        }
    }

    class WardTypeWrapper
    {
        public string ObjectName { get; set; }

        public string SpellName { get; set; }

        public float WardVisionRange { get; set; }

        public WardType WardType { get; set; }

        public float WardDuration
        {
            get
            {
                try
                {
                    float val;
                    WardTrackerVariables.wardDurations.TryGetValue(WardType, out val);
                    return val;
                }
                catch (NullReferenceException ex)
                {
                    LogHelper.AddToLog(new LogItem("WardTracker_Var", ex, LogSeverity.Error));
                }

                return 0;
            }
        }
    }


    enum WardType
    {
        Trinket, Pink, Green
    }
}
