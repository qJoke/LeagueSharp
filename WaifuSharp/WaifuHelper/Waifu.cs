using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeagueSharp.Common;
using SharpDX.Multimedia;
using WaifuSharp.ResourceClasses;

namespace WaifuSharp.WaifuHelper
{
    class Waifu
    {
        public String Name { get; set; }

        public List<OnKillSprite> OnKillPics { get; set; }

        public List<OnKillSound> OnKillSounds { get; set; } 

        public List<OnDeathSprite> OnDeathPics { get; set; }

        public bool CurrentLevel { get; set; }

    }
}
