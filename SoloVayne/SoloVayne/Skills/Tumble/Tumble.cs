using System.Linq;
using DZLib.Logging;
using iSeriesReborn.Utility.MenuUtility;
using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;
using SoloVayne.Utility;
using SoloVayne.Utility.Entities;
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
                if (targetHero.IsValidTarget())
                {
                    //If the Harass mode is agressive and the target has 1 W Stacks + 1 for current AA then we Q for the third.
                    if (Variables.Orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.Mixed
                        && targetHero.GetWBuff().Count != 1
                        && MenuExtensions.GetItemValue<StringList>("solo.vayne.mixed.mode").SelectedIndex == 1)
                    {
                          return;
                    }

                    //If they are autoattacking or winding up and Harass mode is passive we AA them then Q backwards.
                    if (Variables.Orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.Mixed
                        && MenuExtensions.GetItemValue<StringList>("solo.vayne.mixed.mode").SelectedIndex == 0
                        && (targetHero.IsWindingUp || targetHero.Spellbook.IsAutoAttacking) 
                        && Orbwalking.IsAutoAttack(targetHero.LastCastedSpellName()) 
                        && targetHero.LastCastedSpellTarget().IsValid<Obj_AI_Minion>())
                    {
                        var backwardsPosition = ObjectManager.Player.ServerPosition.Extend(targetHero.ServerPosition, -300f);
                        if (backwardsPosition.IsSafe())
                        {
                            CastTumble(backwardsPosition, targetHero);
                        }

                        return;
                    }

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
        }

        public void ExecuteFarm(Obj_AI_Base target)
        {
            if (Variables.spells[SpellSlot.Q].IsEnabledAndReady())
            {
                var currentTarget = target;

                if (currentTarget is Obj_AI_Minion)
                {
                    if (GameObjects.JungleLarge.Contains(currentTarget) || GameObjects.JungleLegendary.Contains(currentTarget))
                    {
                        //It's a jungle minion, so we Q sideways.
                        var sidewaysPosition =
                            (ObjectManager.Player.ServerPosition.To2D() + 300f * ObjectManager.Player.Direction.To2D())
                                .To3D();
                        if (sidewaysPosition.IsSafe())
                        {
                            CastQ(sidewaysPosition);
                            return;
                        }
                    }

                    var minionsInRange = MinionManager.GetMinions(ObjectManager.Player.ServerPosition, ObjectManager.Player.AttackRange + 65)
                        .Where(m => m.Health + 5 <= ObjectManager.Player.GetAutoAttackDamage(m) + Variables.spells[SpellSlot.Q].GetDamage(m))
                        .ToList();

                    if (minionsInRange.Count() > 1)
                    {
                        var firstMinion = minionsInRange.OrderBy(m => m.HealthPercent).First();
                        var afterTumblePosition = ObjectManager.Player.ServerPosition.Extend(Game.CursorPos, 300f);
                        if (afterTumblePosition.Distance(firstMinion.ServerPosition) <= Orbwalking.GetRealAutoAttackRange(null) 
                            && afterTumblePosition.IsSafe())
                        {
                            CastTumble(Game.CursorPos, firstMinion);
                            Variables.Orbwalker.ForceTarget(firstMinion);
                        }
                    }
                }
            }
        }

        private void CastTumble(Vector3 Position, Obj_AI_Base target)
        {
            var WallQPosition = TumbleHelper.GetQBurstModePosition();
            if (WallQPosition != null && ObjectManager.Player.ServerPosition.IsSafeEx() && !(ObjectManager.Player.ServerPosition.UnderTurret(true)))
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
