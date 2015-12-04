using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using SoloVayne.Utility.Enums;

namespace SoloVayne.Skills
{
    interface Skill
    {
        SkillMode GetSkillMode();

        void Execute();
    }
}
