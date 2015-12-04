using System;
using DZLib.Logging;
using LeagueSharp;
using LeagueSharp.Common;
using SoloVayne.Utility;
using SoloVayne.Utility.Enums;

namespace SoloVayne
{
    class SOLOVayne
    {
        /**
         * TODO List
         * Safe enemies around check for Q into Wall
         * Don't aa while stealthed should be on I guess with 3 enemies, but if you have an ally near it shouldn't aa with 2. Maybe it should just always stealth.
         * 
         */

        public SOLOVayne()
        {
            Game.OnUpdate += OnUpdate;
            Obj_AI_Base.OnDoCast += OnDoCast;
        }

        private void OnDoCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            try
            {

                if (sender.IsMe && Orbwalking.IsAutoAttack(args.SData.Name))
                {
                    foreach (var skill in Variables.skills)
                    {
                        if (skill.GetSkillMode() == SkillMode.OnAfterAA)
                        {
                            skill.Execute();
                        }
                    }
                }
            }catch (Exception e)
            {
                LogHelper.AddToLog(new LogItem("OnDoCast", e, LogSeverity.Error));
            }
        }

        private void OnUpdate(EventArgs args)
        {
            try
            {
                if (ObjectManager.Player.IsDead)
                {
                    return;
                }

                foreach (var skill in Variables.skills)
                {
                    if (skill.GetSkillMode() == SkillMode.OnUpdate)
                    {
                        skill.Execute();
                    }
                }
            }
            catch (Exception e)
            {
                LogHelper.AddToLog(new LogItem("OnUpdate", e, LogSeverity.Error));
            }
        }
    }
}
