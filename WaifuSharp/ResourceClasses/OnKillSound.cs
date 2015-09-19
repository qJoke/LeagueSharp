using SharpDX.Multimedia;
using WaifuSharp.Enums;

namespace WaifuSharp.ResourceClasses
{
    class OnKillSound
    {
        public SoundStream SoundStream { get; set; }

        public ResourcePriority SoundPriority { get; set; }

        public int MinWaifuLevel { get; set; }

        public bool PlayCondition
        {
            get { return true; }
        }

        public bool IsDrawing { get; set; }
    }
}
