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
            { WardType.Green, 60 * 3 * 1000},
            { WardType.Trinket, 60 * 1000},
            { WardType.TrinketUpgrade, 60 * 3 * 1000},
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
            new WardTypeWrapper
            {
                ObjectName = "YellowTrinketUpgrade",
                SpellName = "TrinketTotemLvl2",
                WardType = WardType.TrinketUpgrade,
                WardVisionRange = 1100
            },
            new WardTypeWrapper
            {
                ObjectName = "SightWard",
                SpellName = "TrinketTotemLvl3",
                WardType = WardType.Green,
                WardVisionRange = 1100
            },
            new WardTypeWrapper
            {
                ObjectName = "SightWard",
                SpellName = "SightWard",
                WardType = WardType.Green,
                WardVisionRange = 1100
            },
            new WardTypeWrapper
            {
                ObjectName = "SightWard",
                SpellName = "ItemGhostWard",
                WardType = WardType.Green,
                WardVisionRange = 1100
            },

            new WardTypeWrapper
            {
                ObjectName = "VisionWard",
                SpellName = "VisionWard",
                WardType = WardType.Pink,
                WardVisionRange = 1100
            },
            new WardTypeWrapper
            {
                ObjectName = "VisionWard",
                SpellName = "TrinketTotemLvl3B",
                WardType = WardType.Pink,
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

        public Render.Text TextObject { get; set; }

        public Ward()
        {
            CreateRenderObjects();
        }

        public void CreateRenderObjects()
        {
            TextObject = new Render.Text((int)Drawing.WorldToScreen(Position).X, (int)Drawing.WorldToScreen(Position).Y, "", 17, new ColorBGRA(255, 255, 255, 255))
            {
                VisibleCondition = sender => Render.OnScreen(Drawing.WorldToScreen(Position)) && MenuExtensions.GetItemValue<bool>("dz191.dza.ward.track"),
                PositionUpdate = () => new Vector2(Drawing.WorldToScreen(Position).X, Drawing.WorldToScreen(Position).Y + 12),
                TextUpdate = () => (Environment.TickCount < startTick + WardTypeW.WardDuration && WardTypeW.WardDuration < float.MaxValue) ? (Utils.FormatTime(Math.Abs(Environment.TickCount - (startTick + WardTypeW.WardDuration)) / 1000f)) : string.Empty
            };
            TextObject.Add(0);
        }

        public void RemoveRenderObjects()
        {
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
        Trinket, TrinketUpgrade, Pink, Green
    }
}
