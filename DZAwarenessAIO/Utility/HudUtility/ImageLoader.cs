using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using DZAwarenessAIO.Properties;
using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;
using Color = System.Drawing.Color;
using Rectangle = System.Drawing.Rectangle;

//Credits to TheSaltyWaffle - Universal Minimap Hack
//https://github.com/TheSaltyWaffle/LeagueSharp/blob/master/UniversalMinimapHack/ImageLoader.cs
//Ily Waffle

namespace DZAwarenessAIO.Utility.HudUtility
{
    public class ImageLoader
    {
        public static Dictionary<string, HeroHudImage> AddedHeroes = new Dictionary<string, HeroHudImage>();

        public static int ListCount = 1;

        public static Bitmap Load(string championName)
        {
            string cachedPath = GetCachedPath(championName);
            if (File.Exists(cachedPath))
            {
                return ChangeOpacity(new Bitmap(cachedPath));
            }
            var bitmap = Resources.ResourceManager.GetObject(championName + "_Square_0") as Bitmap;
            if (bitmap == null)
            {
                return ChangeOpacity(CreateFinalImage(Resources.empty));
            }
            Bitmap finalBitmap = CreateFinalImage(bitmap);
            finalBitmap.Save(cachedPath);
            return finalBitmap;
            //return ChangeOpacity(finalBitmap);
        }

        private static string GetCachedPath(string championName)
        {
            string path = Path.Combine(Variables.WorkingDir, "ImageCache");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            path = Path.Combine(path, Game.Version);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            return Path.Combine(path, championName + ".png");
        }

        private static Bitmap CreateFinalImage(Bitmap srcBitmap)
        {
            var img = new Bitmap(srcBitmap.Width, srcBitmap.Width);
            var cropRect = new Rectangle(0, 0, srcBitmap.Width, srcBitmap.Width);

            using (Bitmap sourceImage = srcBitmap)
            {
                using (Bitmap croppedImage = sourceImage.Clone(cropRect, sourceImage.PixelFormat))
                {
                    using (var tb = new TextureBrush(croppedImage))
                    {
                        using (Graphics g = Graphics.FromImage(img))
                        {
                            g.FillEllipse(tb, 0, 0, srcBitmap.Width, srcBitmap.Width);
                            var p = new Pen(Color.DarkRed, 5) { Alignment = PenAlignment.Inset };
                            g.DrawEllipse(p, 0, 0, srcBitmap.Width, srcBitmap.Width);
                        }
                    }
                }
            }
            srcBitmap.Dispose();
            return img;
        }

        private static Bitmap ChangeOpacity(Bitmap img)
        {
            var bmp = new Bitmap(img.Width, img.Height); // Determining Width and Height of Source Image
            Graphics graphics = Graphics.FromImage(bmp);
            var colormatrix = new ColorMatrix { Matrix33 = 33 };
            var imgAttribute = new ImageAttributes();
            imgAttribute.SetColorMatrix(colormatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
            graphics.DrawImage(
                img, new Rectangle(0, 0, bmp.Width, bmp.Height), 0, 0, img.Width, img.Height, GraphicsUnit.Pixel,
                imgAttribute);
            graphics.Dispose(); // Releasing all resource used by graphics
            img.Dispose();
            return bmp;
        }
    }

    public class HeroHudImage
    {
        public Render.Sprite HeroSprite { get; set; }

        public HeroHudImage(string name)
        {
            this.HeroSprite = new Render.Sprite(ImageLoader.Load(name), new Vector2(0, 0))
            {
                Scale = new Vector2(0.38f, 0.38f),
                Visible = true,
                VisibleCondition = delegate { return HudDisplay.ShouldBeVisible; },
            };

            //image.GrayScale();
            HeroSprite.Add(1);
        }
    }
}