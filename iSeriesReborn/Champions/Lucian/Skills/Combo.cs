using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
                var target = args.Target as Obj_AI_Hero;

                if (qReady && (target.Distance(ObjectManager.Player) < Variables.spells[SpellSlot.Q].Range - 65f + 0.25f * target.MoveSpeed) && !LucianHooks.HasPassive)
                {
                    Variables.spells[SpellSlot.Q].CastOnUnit(target);
                    LeagueSharp.Common.Utility.DelayAction.Add((int)(250 + Game.Ping / 2f + ObjectManager.Player.AttackCastDelay + 560f),
                        () =>
                        {
                            ExecuteComboLogic(args);
                        });
                    TargetSelector.SetTarget(args.Target as Obj_AI_Hero);
                }

                if (wReady 
                    && !qReady 
                    && !eReady
                    && target.IsValidTarget(Variables.spells[SpellSlot.W].Range) 
                    && !LucianHooks.HasPassive)
                {
                    Variables.spells[SpellSlot.W].Cast(Variables.spells[SpellSlot.W].GetPrediction(target).CastPosition);
                    LeagueSharp.Common.Utility.DelayAction.Add((int)(250 + Game.Ping / 2f + ObjectManager.Player.AttackCastDelay + 560f), () =>
                    {
                        ExecuteComboLogic(args);
                    });
                    TargetSelector.SetTarget(args.Target as Obj_AI_Hero);
                }

                if (eReady && target.IsValidTarget(Variables.spells[SpellSlot.Q].Range + 300f + 65) && !LucianHooks.HasPassive)
                {
                    var eProvider = new EPositionProvider();;
                    var eEndPosition = eProvider.GetEPosition();
                    if (eEndPosition != Vector3.Zero && eEndPosition.Distance(target.ServerPosition) < Orbwalking.GetRealAutoAttackRange(target))
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

    }
}
