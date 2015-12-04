using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeagueSharp;
using SoloVayne.Utility.Enums;

namespace SoloVayne.Skills.Tumble
{
    class Tumble : Skill
    {
        public Tumble() 
        {
            
        }

        public SkillMode GetSkillMode()
        {
            return SkillMode.OnAfterAA;
        }

        public void Execute()
        {
        }
    }
}
