using DZLib.Logging;
using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;
using SoloVayne.Utility;
using SoloVayne.Utility.Enums;

namespace SoloVayne.Skills.Tumble
{
    class Tumble : Skill
    {
        public TumbleLogicProvider Provider;

        public Tumble() 
        {
            Provider = new TumbleLogicProvider();
        }

        public SkillMode GetSkillMode()
        {
            return SkillMode.OnAfterAA;
        }

        public void Execute(Obj_AI_Base target)
        {
            if (target is Obj_AI_Hero)
            {
                var targetHero = target as Obj_AI_Hero;

                if (MenuExtensions.GetItemValue<bool>("solo.vayne.misc.tumble.smartQ"))
                {
                    var position = Provider.GetSOLOVayneQPosition();
                    if (position != Vector3.Zero)
                    {
                        CastTumble(position, targetHero);
                    }
                }
                else
                {
                    var position = ObjectManager.Player.ServerPosition.Extend(Game.CursorPos, 300f);
                    if (position.IsSafe())
                    {
                        CastTumble(position, targetHero);
                    }
                }
            }
        }

        private void CastTumble(Vector3 Position, Obj_AI_Base target)
        {
            var WallQPosition = TumbleHelper.GetQBurstModePosition();
            if (WallQPosition != null && ObjectManager.Player.ServerPosition.IsSafeEx())
            {
                var V3WallQ = (Vector3) WallQPosition;
                CastQ(V3WallQ);
                return;
            }

            Variables.Orbwalker.ForceTarget(target);
            CastQ(Position);
        }

        private void CastQ(Vector3 Position)
        {
            //Orbwalking.ResetAutoAttackTimer();
            Variables.spells[SpellSlot.Q].Cast(Position);
        }
    }
}
