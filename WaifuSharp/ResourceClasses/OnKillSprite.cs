using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeagueSharp.Common;
using WaifuSharp.Enums;

namespace WaifuSharp.ResourceClasses
{
    class OnKillSprite
    {
        public Render.Sprite Sprite { get; set; }

        public ResourcePriority PicPriority { get; set; }

        public bool PlayCondition
        {
            get { return true; }
        }
    }
}
