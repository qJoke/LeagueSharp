using System;
using System.Linq;
using DZLib.Core;
using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;
using VayneHunter_Reborn.External;
using VayneHunter_Reborn.Skills.Tumble.VHRQ;
using VayneHunter_Reborn.Utility;
using VayneHunter_Reborn.Utility.MenuUtility;
using ActiveGapcloser = VayneHunter_Reborn.External.ActiveGapcloser;
using GapcloserType = VayneHunter_Reborn.External.GapcloserType;

namespace VayneHunter_Reborn.Skills.Condemn
{
    class InterrupterGapcloser
    {
        static float flashQRange = 450 + 300f;

        public static void OnLoad()
        {
            Interrupter2.OnInterruptableTarget += OnInterruptableTarget;
            DZAntigapcloserVHR.OnEnemyGapcloser += OnEnemyGapcloser;
            Obj_AI_Base.OnProcessSpellCast += Obj_AI_Base_OnProcessSpellCast;
            GameObject.OnCreate += GameObject_OnCreate;
        }

        private static void OnEnemyGapcloser(ActiveGapcloser gapcloser, SpellSlot slot)
        {
            if (ObjectManager.Player.Position.Distance(gapcloser.End) > 365f) return;
           
            SpellSlot FlashSlot = ObjectManager.Player.GetSpellSlot("summonerflash");

            if (MenuExtensions.GetItemValue<bool>("dz191.vhr.misc.general.antigp")  &&
                gapcloser.Sender.IsEnemy && gapcloser.SkillType == GapcloserType.Targeted &&
                gapcloser.Sender.ChampionName == "Alistar" && Variables.spells[SpellSlot.E].IsReady())
            {
                if(FlashSlot.IsReady()
                    && ObjectManager.Player.GetEnemiesInRange(1500f).Count >= 3 && ObjectManager.Player.HealthPercent< 40)
                {
                    ObjectManager.Player.Spellbook.CastSpell(FlashSlot, GetSelectedPosition(gapcloser.Sender.Position, 450, 0));
                    Variables.spells[SpellSlot.E].CastOnUnit(gapcloser.Sender);
                    return;
                }
            }


            if (MenuExtensions.GetItemValue<bool>("dz191.vhr.misc.general.antigp") && Variables.spells[slot].IsReady())
            {
                LeagueSharp.Common.Utility.DelayAction.Add(MenuExtensions.GetItemValue<Slider>("dz191.vhr.misc.general.antigpdelay").Value,
                    () =>
                    {
                        if (gapcloser.Sender.IsValidTarget(Variables.spells[SpellSlot.E].Range) && 
                            MenuExtensions.GetItemValue<bool>("dz191.vhr.misc.general.antigp") 
                            && (Variables.spells[slot].IsReady()))
                        {
                            switch (slot)
                            {
                                case SpellSlot.Q:
                                    var senderPos = gapcloser.End;
                                    var backOut = ObjectManager.Player.ServerPosition.Extend(senderPos, 300f);
                                    if (backOut.IsSafe())
                                    {
                                        if (gapcloser.Start.Distance(ObjectManager.Player.ServerPosition) >
                                            gapcloser.End.Distance(ObjectManager.Player.ServerPosition))
                                        {
                                            Variables.spells[SpellSlot.Q].Cast(backOut);
                                        }
                                    }

                                    break;

                                case SpellSlot.E:
                                    if (gapcloser.Start.Distance(ObjectManager.Player.ServerPosition) >
                                        gapcloser.End.Distance(ObjectManager.Player.ServerPosition))
                                    {
                                        Variables.spells[SpellSlot.E].CastOnUnit(gapcloser.Sender);
                                    }
                                    break;
                            }
                        }
                    });
            }
        }

        private static void OnInterruptableTarget(Obj_AI_Hero sender, Interrupter2.InterruptableTargetEventArgs args)
        {
            if (MenuExtensions.GetItemValue<bool>("dz191.vhr.misc.general.interrupt"))
            {
                if (args.DangerLevel == Interrupter2.DangerLevel.High && sender.IsValidTarget(Variables.spells[SpellSlot.E].Range))
                {
                    Variables.spells[SpellSlot.E].CastOnUnit(sender);
                }
            }
        }

        private static void Obj_AI_Base_OnProcessSpellCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {

            if (sender is Obj_AI_Hero)
            {
                var s2 = (Obj_AI_Hero)sender;
                if (s2.IsValidTarget() && s2.ChampionName == "Pantheon" && s2.GetSpellSlot(args.SData.Name) == SpellSlot.W)
                {
                    if (MenuExtensions.GetItemValue<bool>("dz191.vhr.misc.general.antigp") && args.Target.IsMe && Variables.spells[SpellSlot.E].IsReady())
                    {
                        if (s2.IsValidTarget(Variables.spells[SpellSlot.E].Range))
                        {
                            Variables.spells[SpellSlot.E].CastOnUnit(s2);
                        }
                    }
                }
            }

        }

        private static void GameObject_OnCreate(GameObject sender, EventArgs args)
        {
            if (MenuExtensions.GetItemValue<bool>("dz191.vhr.misc.general.antigp") && Variables.spells[SpellSlot.E].IsReady())
            {
                if (sender.IsEnemy && sender.Name == "Rengar_LeapSound.troy")
                {
                    var rengarEntity = HeroManager.Enemies.Find(h => h.ChampionName.Equals("Rengar") && h.IsValidTarget(Variables.spells[SpellSlot.E].Range));
                    if (rengarEntity != null)
                    {
                        Variables.spells[SpellSlot.E].CastOnUnit(rengarEntity);
                    }
                }
            }
        }

        public static Vector3 GetSelectedPosition(Vector3 pos, float range, int type)
        {
            switch (type)
            {
                case 0: // backwards
                    return ObjectManager.Player.ServerPosition.Extend(pos, -range);
                case 1: // teammates
                    var teammate = ObjectManager.Player.GetAlliesInRange(1500f).FirstOrDefault();
                    return ObjectManager.Player.Position.Extend(teammate?.Position ?? Game.CursorPos, range);
                case 2: // turret
                    var closestTurret =
                        ObjectManager.Get<Obj_AI_Turret>()
                            .FirstOrDefault(x => x.IsAlly && x.Health > 1 && x.Distance(ObjectManager.Player) < 1500f);
                    return ObjectManager.Player.Position.Extend(closestTurret?.Position ?? Game.CursorPos, range);
            }

            return ObjectManager.Player.ServerPosition.Extend(pos, -range);
        }
    }
}
