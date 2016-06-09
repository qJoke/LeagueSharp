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
            return ModuleType.OnAfterAA;
        }

        public void OnExecute()
        {
            var pushDistance = MenuExtensions.GetItemValue<Slider>("dz191.vhr.misc.condemn.pushdistance").Value - 25;


            foreach (var target in HeroManager.Enemies.Where(en => en.IsValidTarget(E.Range) && !en.IsDashing()))
            {
                var canFlashBehind = ObjectManager.Player.Distance(target) <
                                     Flash.Range - ObjectManager.Player.BoundingRadius;
                var flashPosition = ObjectManager.Player.ServerPosition.Extend(target.ServerPosition, Flash.Range);

                if (!canFlashBehind || !flashPosition.IsSafe())
                {
                    return;
                }

                var Prediction = Variables.spells[SpellSlot.E].GetPrediction(target);

                if (Prediction.Hitchance >= HitChance.VeryHigh)
                {
                    var endPosition = Prediction.UnitPosition.Extend(flashPosition, -pushDistance);
                    if (endPosition.IsWall())
                    {
                        Flash.Cast(flashPosition);
                        E.CastOnUnit(target);
                    }
                    else
                    {
                        //It's not a wall.
                        var step = pushDistance / 5f;
                        for (float i = 0; i < pushDistance; i += step)
                        {
                            var endPositionEx = Prediction.UnitPosition.Extend(flashPosition, -i);
                            if (endPositionEx.IsWall())
                            {
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
