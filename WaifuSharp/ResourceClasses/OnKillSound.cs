using SharpDX.Multimedia;
using WaifuSharp.Enums;

namespace WaifuSharp.ResourceClasses
{
    class OnKillSound
    {
        public SoundStream SoundStream { get; set; }

        public ResourcePriority SoundPriority { get; set; }

        public bool PlayCondition
        {
            get { return true; }
        }
    }
}
