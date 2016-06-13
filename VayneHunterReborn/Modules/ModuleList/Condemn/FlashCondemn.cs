using System;
using System.Linq;
using LeagueSharp;
using LeagueSharp.Common;
using VayneHunter_Reborn.Modules.ModuleHelpers;
using VayneHunter_Reborn.Skills.Tumble.VHRQ;
using VayneHunter_Reborn.Utility;
using VayneHunter_Reborn.Utility.MenuUtility;

namespace VayneHunter_Reborn.Modules.ModuleList.Condemn
{
    class FlashCondemn : IModule
    {
        private static Spell E
        {
            get { return Variables.spells[SpellSlot.E]; }
        }

        private static Spell Flash
        {
            get { return new Spell(ObjectManager.Player.GetSpellSlot("SummonerFlash"), 425f); }
        }

        public void OnLoad()
        {
        }

        public bool ShouldGetExecuted()
        {
            return MenuExtensions.GetItemValue<KeyBind>("dz191.vhr.misc.condemn.flashcondemn").Active &&
                   Variables.spells[SpellSlot.E].IsReady() && Flash.Slot != SpellSlot.Unknown && Flash.IsReady();
        }

        public ModuleType GetModuleType()
        {
            return ModuleType.OnUpdate; //idk why thiis wwas on after attack m8 pls
        }

        public void OnExecute()
        {
            var pushDistance = 450;

            foreach (var target in HeroManager.Enemies.Where(en => en.IsValidTarget(E.Range) && !en.IsDashing()))
            {
                var flashPosition = ObjectManager.Player.ServerPosition.Extend(Game.CursorPos, Flash.Range);

                var prediction = Variables.spells[SpellSlot.E].GetPrediction(target);

                if (prediction.Hitchance >= HitChance.VeryHigh)
                {
                    var endPosition = prediction.UnitPosition.Extend(flashPosition, -pushDistance);
                    if (endPosition.IsWall())
                    {
                        Variables.LastCondemnFlashTime = Environment.TickCount;
                        E.CastOnUnit(target);
                        Flash.Cast(flashPosition);
                    }
                    else
                    {
                        //It's not a wall.
                        var step = pushDistance / 5f;
                        for (float i = 0; i < pushDistance; i += step)
                        {
                            var endPositionEx = prediction.UnitPosition.Extend(flashPosition, -i);
                            if (endPositionEx.IsWall())
                            {
                                Variables.LastCondemnFlashTime = Environment.TickCount;
                                E.CastOnUnit(target);
                                Flash.Cast(flashPosition);
                                return;
                            }
                        }
                    }
                }

            }
        }
    }
}
