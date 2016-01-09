using System;
using System.Collections.Generic;
using System.Linq;
using iSeriesReborn.Utility;
using iSeriesReborn.Utility.MenuUtility;
using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;
using SoloVayne.Skills.Tumble;

namespace iSeriesReborn.Champions.Lucian.Skills
{
    class Combo
    {

        internal static void ExecuteComboLogic(GameObjectProcessSpellCastEventArgs args)
        {
            if (args.Target is Obj_AI_Hero)
            {
                var qReady = Variables.spells[SpellSlot.Q].IsEnabledAndReady();
                var wReady = Variables.spells[SpellSlot.W].IsEnabledAndReady();
                var eReady = Variables.spells[SpellSlot.E].IsEnabledAndReady();
                var target_ex = args.Target as Obj_AI_Hero;

                if (qReady)
                {
                    ExtendedQ();
                }

                if (qReady && (target_ex.Distance(ObjectManager.Player) < Variables.spells[SpellSlot.Q].Range - 65f + 0.25f * target_ex.MoveSpeed) && !LucianHooks.HasPassive)
                {
                    Variables.spells[SpellSlot.Q].CastOnUnit(target_ex);
                    LeagueSharp.Common.Utility.DelayAction.Add((int)(250 + Game.Ping / 2f + ObjectManager.Player.AttackCastDelay + 560f),
                        () =>
                        {
                            ExecuteComboLogic(args);
                        });
                    TargetSelector.SetTarget(args.Target as Obj_AI_Hero);
                }

                if (wReady 
                    && !qReady 
                    && target_ex.IsValidTarget(Variables.spells[SpellSlot.W].Range) 
                    && !LucianHooks.HasPassive)
                {
                    Variables.spells[SpellSlot.W].Cast(Variables.spells[SpellSlot.W].GetPrediction(target_ex).CastPosition);
                    LeagueSharp.Common.Utility.DelayAction.Add((int)(250 + Game.Ping / 2f + ObjectManager.Player.AttackCastDelay + 560f), () =>
                    {
                        ExecuteComboLogic(args);
                    });
                    TargetSelector.SetTarget(args.Target as Obj_AI_Hero);
                }

                if (eReady 
                    && target_ex.IsValidTarget(Variables.spells[SpellSlot.Q].Range + 300f + 65) 
                    && !LucianHooks.HasPassive 
                    && !ObjectManager.Player.IsWindingUp)
                {
                    var eProvider = new EPositionProvider();;
                    var eEndPosition = eProvider.GetEPosition();
                    if (eEndPosition != Vector3.Zero && eEndPosition.Distance(target_ex.ServerPosition) < Orbwalking.GetRealAutoAttackRange(target_ex))
                    {
                        Variables.spells[SpellSlot.E].Cast(eEndPosition);
                        LeagueSharp.Common.Utility.DelayAction.Add((int)(250 + Game.Ping / 2f + ObjectManager.Player.AttackCastDelay + 560f),
                            () =>
                            {
                                Orbwalking.ResetAutoAttackTimer();
                                ExecuteComboLogic(args);
                            });
                        TargetSelector.SetTarget(args.Target as Obj_AI_Hero);
                    }
                }
            }
        }

        private static void ExtendedQ()
        {
            foreach (var collisionMinion in from target in ObjectManager.Player.GetEnemiesInRange(Variables.qExtended.Range)
                                            let position = new List<Vector2> { target.Position.To2D() }
                                            select
                                                Variables.qExtended.GetCollision(
                                                    ObjectManager.Player.Position.To2D(),
                                                    position)
                                                .FirstOrDefault(
                                                    minion =>
                                                    Variables.spells[SpellSlot.Q].CanCast(minion)
                                                    && Variables.spells[SpellSlot.Q].IsInRange(minion)
                                                    && checkLine(
                                                        ObjectManager.Player.Position,
                                                        minion.Position,
                                                        target.ServerPosition) && checkDistance(target, minion)
                                                    && target.Distance(ObjectManager.Player) > minion.Distance(ObjectManager.Player)
                                                    && ObjectManager.Player.Distance(minion) + minion.Distance(target)
                                                    <= ObjectManager.Player.Distance(target) + 10f)
                                                into collisionMinion
                                                where collisionMinion != null
                                                select collisionMinion)
            {
                Variables.spells[SpellSlot.Q].CastOnUnit(collisionMinion);
            }
        }

        /// <summary>
        ///     Distance check
        ///     Credits Pastel!
        /// </summary>
        private static readonly Func<Obj_AI_Hero, Obj_AI_Base, bool> checkDistance =
            (champ, minion) =>
            Math.Abs(
                champ.Distance(ObjectManager.Player) - (minion.Distance(ObjectManager.Player) + minion.Distance(champ)))
            <= 2;

        /// <summary>
        ///     Line check, credits pastel!
        /// </summary>
        private static readonly Func<Vector3, Vector3, Vector3, bool> checkLine =
            (v1, v2, v3) =>
            Math.Abs((v1.X * v2.Y) + (v1.Y * v3.X) + (v2.X * v3.Y) - (v1.Y * v2.X) - (v1.X * v3.Y) - (v2.Y * v3.X))
            <= 20000;
    }
}
