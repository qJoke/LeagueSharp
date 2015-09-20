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

        public static int X
        {
            get { return WaifuSharp.Menu.Item("waifusharp.options.x").GetValue<Slider>().Value; }
        }

        public static int Y
        {
            get { return WaifuSharp.Menu.Item("waifusharp.options.y").GetValue<Slider>().Value; }
        }

        public static float Scale
        {
            get { return WaifuSharp.Menu.Item("waifusharp.options.scale").GetValue<Slider>().Value / 100f; }
        }

        private static Render.Sprite CurrentSprite;

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
                        sprite.Sprite.Scale = new Vector2(Scale, Scale);
                        sprite.Sprite.VisibleCondition = delegate
                        {
                            return sprite.IsDrawing;
                        };
                        sprite.Sprite.Position = new Vector2(X, Y);
                        sprite.Sprite.PositionUpdate += () => new Vector2(X, Y);
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
            var menuItem = WaifuSharp.Menu.Item("waifusharp.options.waifus");

            if (menuItem != null)
            {
                return GetWaifuByName(menuItem.GetValue<StringList>().SelectedValue);
            }

            return Waifus.FirstOrDefault();
        }

        private static Waifu GetWaifuByName(String name)
        {
            return Waifus.FirstOrDefault(w => w.Name.ToLower() == name.ToLower());
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
                Game.PrintChat(string.Format("<b><font color='#FF0000'>Waifu#:</font></b> Loaded <b><font color='#7A6EFF'>{0}</font></b>", currentWaifu.Name));

                Waifus.Add(currentWaifu);
            }
        }

        private static void LoadContentToWaifu(String FilePath, String DirName, Waifu currentWaifu)
        {
            var array = FilePath.Split(Path.DirectorySeparatorChar);
            var fileName = array.Last();
            if (fileName.ToLower().Contains("onkill"))
            {
                OnKillLoad(fileName, FilePath, DirName, currentWaifu);
            }else if (fileName.ToLower().Contains("ondeath"))
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

            var OptionsMenu = new Menu("Waifu# Options","waifusharp.options");
            {
                var stringListContainer = new MenuItem("waifusharp.options.waifus", "Current Waifu: ");
                var WaifuNames = Waifus.Select(w => w.Name);
                stringListContainer.SetValue(new StringList(WaifuNames.ToArray()));
                OptionsMenu.AddItem(stringListContainer);

                OptionsMenu.AddItem(
                    new MenuItem("waifusharp.options.x", "X Coordinate").SetValue(
                        new Slider(200, 0, Drawing.Direct3DDevice.Viewport.Width)));
                OptionsMenu.AddItem(
                    new MenuItem("waifusharp.options.y", "Y Coordinate").SetValue(
                        new Slider(200, 0, Drawing.Direct3DDevice.Viewport.Height)));
                OptionsMenu.AddItem(new MenuItem("waifusharp.options.scale", "Image Scale %").SetValue(new Slider(50))).ValueChanged +=
                    (sender, args) =>
                    {
                        if (CurrentSprite != null)
                        {
                            CurrentSprite.Scale = new Vector2(Scale, Scale);
                        } 
                    };
                OptionsMenu.AddItem(new MenuItem("waifusharp.options.testwaifu", "Show Test Waifu").SetValue(false))
                    .ValueChanged += (sender, args) =>
                    {
                        var waifu = GetCurrentWaifu();
                        if (waifu != null)
                        {
                            var sprite = waifu.OnKillPics[new Random().Next(0, waifu.OnKillPics.Count())];
                            if (sprite != null)
                            {
                                if (args.GetNewValue<bool>())
                                {
                                    InitKillSprite(sprite);
                                }
                                else
                                {
                                    if (CurrentSprite != null)
                                    {
                                        IsDrawing = false;
                                        CurrentSprite.Visible = false;
                                        CurrentSprite.Remove();
                                    }
                                    
                                }
                            }
                        }
                    };
                WaifuSharp.Menu.AddSubMenu(OptionsMenu);
            }
            
            WaifuSharp.Menu.Item("waifusharp.options.testwaifu").SetValue(false);

            WaifuSharp.Menu.AddToMainMenu();
        }
        #endregion

        #region Utility Methods

        private static void InitKillSprite(OnKillSprite sprite)
        {
            if (IsDrawing)
            {
                return;
            }

            IsDrawing = true;
            sprite.IsDrawing = true;
            sprite.Sprite.Visible = true;
            sprite.Sprite.Scale = new Vector2(Scale, Scale);
            sprite.Sprite.VisibleCondition = delegate
            { return sprite.IsDrawing; };
            sprite.Sprite.Position = new Vector2(X, Y);
            sprite.Sprite.PositionUpdate += () => new Vector2(X, Y);
            CurrentSprite = sprite.Sprite;
            sprite.Sprite.Add();
        }

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
