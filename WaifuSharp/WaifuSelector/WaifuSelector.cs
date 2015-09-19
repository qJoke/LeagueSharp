using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
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

        public static bool IsDrawing = false;

        public static void OnLoad()
        {
            CheckAndCreateDirectories();
            SaveDefaultWaifus();
            LoadWaifus();
            LoadMenu();
            Game.OnInput += Game_OnInput;
        }


        static void Game_OnInput(GameInputEventArgs args)
        {
            if (args.Input.StartsWith(".w"))
            {
                args.Process = false;
                if (IsDrawing)
                {
                    return;
                }
                var waifu = GetCurrentWaifu();
                if (waifu != null)
                {
                    var sprite = waifu.OnKillPics[new Random().Next(0, waifu.OnKillPics.Count())];
                    if (sprite != null)
                    {
                        IsDrawing = true;
                        sprite.IsDrawing = true;
                        sprite.Sprite.Visible = true;
                        sprite.Sprite.Scale = new Vector2(1.0f, 1.0f);
                        sprite.Sprite.VisibleCondition = delegate
                        {
                            return sprite.IsDrawing;
                        };
                        sprite.Sprite.Position = new Vector2(200, 200);
                        sprite.Sprite.Add();
                        Utility.DelayAction.Add(
                            3500, () =>
                            {
                                IsDrawing = false;
                                sprite.IsDrawing = true;
                                sprite.Sprite.Visible = false;
                                sprite.Sprite.Remove();
                            });
                    }
                }
            }
        }

        private static Waifu GetCurrentWaifu()
        {
            return Waifus.FirstOrDefault();
        }
        #region Waifus Loading
        private static void LoadWaifus()
        {
            foreach (var directory in Directory.GetDirectories(WaifusDir))
            {
                var array = directory.Split(Path.DirectorySeparatorChar);
                var waifuName = array.Last();
                var currentWaifu = new Waifu
                {
                    Name = waifuName
                };

                foreach (var d2 in Directory.GetDirectories(directory))
                {
                    var a2 = d2.Split(Path.DirectorySeparatorChar);
                    var d2Name = a2.Last();

                    if (!IsInt(d2Name))
                    {
                        continue;
                    }

                    string[] content = Directory.GetFiles(d2);

                    foreach (var file in content)
                    {
                        LoadContentToWaifu(file, d2Name, currentWaifu);
                    }
                }
                Console.WriteLine(@"Loaded {0}, with {1} pics", currentWaifu.Name, currentWaifu.OnKillPics.Count);

                Waifus.Add(currentWaifu);
            }
        }

        private static void LoadContentToWaifu(String FilePath, String DirName, Waifu currentWaifu)
        {
            var array = FilePath.Split(Path.DirectorySeparatorChar);
            var fileName = array.Last();
            Console.WriteLine(fileName);
            if (fileName.ToLower().Contains("onkill"))
            {
                OnKillLoad(fileName, FilePath, DirName, currentWaifu);
            }

            if (fileName.ToLower().Contains("ondeath"))
            {
                OnDeathLoad(fileName, currentWaifu);
            }
        }

        #region OnKill / OnDeath Load
        private static void OnKillLoad(String fileName, String filePath, String DirName, Waifu currentWaifu)
        {
            var MinWaifuLevel = Int32.Parse(DirName);


            var priority = fileName.Replace("onkill", "");

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
            }else if (priority.Contains("double"))
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
            }else if (priority.Contains("triple"))
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
            }else if (priority.Contains("quadra"))
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
            }else if (priority.Contains("penta"))
            {
                var currentSprite = GetSpriteFromFile(filePath);
                if (currentSprite != null)
                {
                    currentWaifu.OnKillPics.Add(
                        new OnKillSprite
                        {
                            PicPriority = ResourcePriority.PentaKill,
                            Sprite = GetSpriteFromFile(filePath)
                        });
                }
            }
            else
            {
                var currentSprite = GetSpriteFromFile(filePath);
                if (currentSprite != null)
                {
                    currentWaifu.OnKillPics.Add(
                        new OnKillSprite
                        {
                            PicPriority = ResourcePriority.Random,
                            Sprite = GetSpriteFromFile(filePath)
                        });
                }
            }
        }

        private static void OnDeathLoad(String fileName, Waifu currentWaifu)
        {

        }

        #endregion

        
        #endregion

        #region Menu Creation
        private static void LoadMenu()
        {
            
        }
        #endregion

        #region Utility Methods
        private static void SaveDefaultWaifus()
        {

        }

        private static Render.Sprite GetSpriteFromFile(string filePath)
        {
            var sprite = new Render.Sprite(filePath, Vector2.Zero);
            return sprite;
        }
        private static bool IsInt(string sVal)
        {
            return sVal.Select(c => (int)c).All(iN => (iN <= 57) && (iN >= 48));
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
