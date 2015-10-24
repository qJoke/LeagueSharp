using System;
using System.Linq;
using LeagueSharp;
using LeagueSharp.Common;

namespace DZAwarenessAIO.Modules.WardTracker
{
    class WardDetector
    {
        public static void OnTick()
        {
            foreach (var s in WardTrackerVariables.detectedWards)
            {
                if (Game.Time > s.startTick + s.WardTypeW.WardDuration)
                {
                    s.RemoveRenderObjects();   
                }
            }

            WardTrackerVariables.detectedWards.RemoveAll(s => Game.Time > s.startTick + s.WardTypeW.WardDuration);
        }

        public static void OnProcessSpellCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
           //TODO See if I will use this eventually
        }

        public static void OnCreate(GameObject sender, EventArgs args)
        {
            if (sender is Obj_AI_Base)
            {
                var sender_ex = sender as Obj_AI_Base;
                var ward = WardTrackerVariables.wrapperTypes.FirstOrDefault(
                    w => w.ObjectName.ToLower().Equals(sender_ex.CharData.BaseSkinName.ToLower()));
                if (ward != null)
                {
                    var StartTick = Game.Time - (int)((sender_ex.MaxMana - sender_ex.Mana) * 1000);
                    
                    var AlreadyDetected =
                        WardTrackerVariables.detectedWards.FirstOrDefault(
                            w =>
                                w.Position.Distance(sender_ex.ServerPosition) < 125 &&
                                (Math.Abs(w.startTick - StartTick) < 800 || w.WardTypeW.WardType != WardType.Green ||
                                 w.WardTypeW.WardType != WardType.Trinket));
                    if (AlreadyDetected != null)
                    {
                        AlreadyDetected.RemoveRenderObjects();
                        WardTrackerVariables.detectedWards.RemoveAll(
                            w =>
                                w.Position.Distance(sender_ex.ServerPosition) < 125 &&
                                (Math.Abs(w.startTick - StartTick) < 800 || w.WardTypeW.WardType != WardType.Green ||
                                 w.WardTypeW.WardType != WardType.Trinket));
                    }
                    
                    WardTrackerVariables.detectedWards.Add(new Ward()
                    {
                        Position = sender_ex.ServerPosition,
                        startTick = StartTick,
                        WardTypeW = ward
                    });
                }
            }
        }
    }
}
