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

        public List<OnKillSprite> OnKillPics = new List<OnKillSprite>();

        public List<OnKillSound> OnKillSounds = new List<OnKillSound>();

        public List<OnDeathSprite> OnDeathPics = new List<OnDeathSprite>();

        public int CurrentLevel { get; set; }

        public int CurrentExp { get; set; }

        public bool IsDrawing { get; set; }
    }
}
