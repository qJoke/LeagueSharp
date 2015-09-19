using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;
using WaifuSharp.Enums;
using WaifuSharp.ResourceClasses;
using WaifuSharp.WaifuHelper;

namespace WaifuSharp.WaifuSelector
{
    class WaifuSelector
    {
        private static string LeagueSharpAppData
        {
            get
            {
                return Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                    "LS" + Environment.UserName.GetHashCode().ToString("X"));
            }
        }

        public static String AssemblyDir
        {
            get { return Path.Combine(LeagueSharpAppData, "WaifuSharp"); }
        }

        public static String WaifusDir
        {
            get { return Path.Combine(AssemblyDir, "WaifusDir"); }
        }

        public static List<Waifu> Waifus = new List<Waifu>();
 
        public static void OnLoad()
        {
            CheckAndCreateDirectories();
            LoadWaifus();
            LoadMenu();
        }

        #region Waifus Loading
        private static void LoadWaifus()
        {
            foreach (var directory in Directory.GetDirectories(WaifusDir))
            {
                var currentWaifu = new Waifu
                {
                    Name = directory
                };
                string[] content = Directory.GetFiles(directory);
                foreach (var file in content)
                {
                    LoadContentToWaifu(file, currentWaifu);
                }
                Waifus.Add(currentWaifu);
            }
        }

        private static void LoadContentToWaifu(String FilePath, Waifu currentWaifu)
        {
            var array = FilePath.Split(Path.DirectorySeparatorChar);
            var fileName = array.Last().ToLower();

            if (fileName.Contains("onkill"))
            {
                OnKillLoad(fileName, FilePath, currentWaifu);
            }

            if (fileName.Contains("ondeath"))
            {
                OnDeathLoad(fileName, currentWaifu);
            }
        }

        #region OnKill / OnDeath Load
        public static void OnKillLoad(String fileName, String filePath, Waifu currentWaifu)
        {
            var priority = fileName.ToLower().Replace("onkill", "");

            if (priority.Contains("single"))
            {
                var currentSprite = GetSpriteFromFile(filePath);
                if (currentSprite != null)
                {
                    currentWaifu.OnKillPics.Add(new OnKillSprite
                    {
                        PicPriority = ResourcePriority.SingleKill,
                        Sprite = GetSpriteFromFile(filePath)
                    });
                }
            }

            if (priority.Contains("double"))
            {
                var currentSprite = GetSpriteFromFile(filePath);
                if (currentSprite != null)
                {
                    currentWaifu.OnKillPics.Add(new OnKillSprite
                    {
                        PicPriority = ResourcePriority.DoubleKill,
                        Sprite = GetSpriteFromFile(filePath)
                    });
                }
            }

            if (priority.Contains("triple"))
            {
                var currentSprite = GetSpriteFromFile(filePath);
                if (currentSprite != null)
                {
                    currentWaifu.OnKillPics.Add(new OnKillSprite
                    {
                        PicPriority = ResourcePriority.TripleKill,
                        Sprite = GetSpriteFromFile(filePath)
                    });
                }
            }

            if (priority.Contains("quadra"))
            {
                var currentSprite = GetSpriteFromFile(filePath);
                if (currentSprite != null)
                {
                    currentWaifu.OnKillPics.Add(new OnKillSprite
                    {
                        PicPriority = ResourcePriority.QuadraKill,
                        Sprite = GetSpriteFromFile(filePath)
                    });
                }
            }

            if (priority.Contains("penta"))
            {
                var currentSprite = GetSpriteFromFile(filePath);
                if (currentSprite != null)
                {
                    currentWaifu.OnKillPics.Add(new OnKillSprite
                    {
                        PicPriority = ResourcePriority.PentaKill,
                        Sprite = GetSpriteFromFile(filePath)
                    });
                }
            }
        }
        #endregion

        public static void OnDeathLoad(String fileName, Waifu currentWaifu)
        {

        }
        #endregion

        #region Menu Creation
        private static void LoadMenu()
        {
            
        }
        #endregion

        #region Utility Methods
        private static Render.Sprite GetSpriteFromFile(string filePath)
        {
            var sprite = new Render.Sprite(filePath, Vector2.Zero);
            return sprite;
        }

        private static void CheckAndCreateDirectories()
        {
            if (!Directory.Exists(AssemblyDir))
            {
                Directory.CreateDirectory(AssemblyDir);
            }

            if (!Directory.Exists(WaifusDir))
            {
                Directory.CreateDirectory(WaifusDir);
            }
        }
        #endregion

    }
}
