﻿using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;
using DarkEmpire.Tiled;
using System.IO;
using System.Reflection;
using System.Diagnostics;
namespace DarkEmpire
{
    public class BattleSystem
    {
        public static Texture2D pixel = new Texture2D(Game1.graphics.GraphicsDevice, 1, 1); //create 1x1 pixel texture
        TmxMap battlemap;
        public bool activeBattle;
        public static SpriteFont battleText;

        int width;
        int height;
        Texture2D texture;

        public BattleSystem()
        {
            activeBattle = false;
        }

        private Color ColorBorder(int x, int y, int width, int height, int borderThickness, int borderRadius, int borderShadow, Color initialColor, List<Color> borderColors, float initialShadowIntensity, float finalShadowIntensity)
        {
            Rectangle internalRectangle = new Rectangle((borderThickness + borderRadius), (borderThickness + borderRadius), width - 2 * (borderThickness + borderRadius), height - 2 * (borderThickness + borderRadius));

            if (internalRectangle.Contains(x, y)) return initialColor;

            Vector2 origin = Vector2.Zero;
            Vector2 point = new Vector2(x, y);

            if (x < borderThickness + borderRadius)
            {
                if (y < borderRadius + borderThickness)
                    origin = new Vector2(borderRadius + borderThickness, borderRadius + borderThickness);
                else if (y > height - (borderRadius + borderThickness))
                    origin = new Vector2(borderRadius + borderThickness, height - (borderRadius + borderThickness));
                else
                    origin = new Vector2(borderRadius + borderThickness, y);
            }
            else if (x > width - (borderRadius + borderThickness))
            {
                if (y < borderRadius + borderThickness)
                    origin = new Vector2(width - (borderRadius + borderThickness), borderRadius + borderThickness);
                else if (y > height - (borderRadius + borderThickness))
                    origin = new Vector2(width - (borderRadius + borderThickness), height - (borderRadius + borderThickness));
                else
                    origin = new Vector2(width - (borderRadius + borderThickness), y);
            }
            else
            {
                if (y < borderRadius + borderThickness)
                    origin = new Vector2(x, borderRadius + borderThickness);
                else if (y > height - (borderRadius + borderThickness))
                    origin = new Vector2(x, height - (borderRadius + borderThickness));
            }

            if (!origin.Equals(Vector2.Zero))
            {
                float distance = Vector2.Distance(point, origin);

                if (distance > borderRadius + borderThickness + 1)
                {
                    return Color.Transparent;
                }
                else if (distance > borderRadius + 1)
                {
                    if (borderColors.Count > 2)
                    {
                        float modNum = distance - borderRadius;

                        if (modNum < borderThickness / 2)
                        {
                            return Color.Lerp(borderColors[2], borderColors[1], (float)((modNum) / (borderThickness / 2.0)));
                        }
                        else
                        {
                            return Color.Lerp(borderColors[1], borderColors[0], (float)((modNum - (borderThickness / 2.0)) / (borderThickness / 2.0)));
                        }
                    }


                    if (borderColors.Count > 0)
                        return borderColors[0];
                }
                else if (distance > borderRadius - borderShadow + 1)
                {
                    float mod = (distance - (borderRadius - borderShadow)) / borderShadow;
                    float shadowDiff = initialShadowIntensity - finalShadowIntensity;
                    return DarkenColor(initialColor, ((shadowDiff * mod) + finalShadowIntensity));
                }
            }

            return initialColor;
        }

        private Color DarkenColor(Color color, float shadowIntensity)
        {
            return Color.Lerp(color, Color.Black, shadowIntensity);
        }

        public void initialize()
        {
            width = (int)(Game1.screenWidth * 0.425f);
            height = (int)(Game1.screenHeight * .20f);

             texture = new Texture2D(Game1.graphics.GraphicsDevice, width, height, false, SurfaceFormat.Color);
             Color[] color = new Color[width * height];
             List<Color> backgroundColors = new List<Color>();
             List<Color> borderColors = new List<Color>();
             borderColors.Add(Color.Purple);
             borderColors.Add(Color.Orange);
             backgroundColors.Add(Color.Red);
             backgroundColors.Add(Color.Blue);
             backgroundColors.Add(Color.Yellow);
             backgroundColors.Add(Color.Green);
             int borderThickness = 5;
             int borderShadow = 2;
             int borderRadius = 12;
             int initialShadowIntensity = 10;
             int finalShadowIntensity = 15;
             for (int x = 0; x < texture.Width; x++)
             {
                 for (int y = 0; y < texture.Height; y++)
                 {
                     switch (backgroundColors.Count)
                     {
                         case 4:
                             Color leftColor0 = Color.Lerp(backgroundColors[0], backgroundColors[1], ((float)y / (width - 1)));
                             Color rightColor0 = Color.Lerp(backgroundColors[2], backgroundColors[3], ((float)y / (height - 1)));
                             color[x + width * y] = Color.Lerp(leftColor0, rightColor0, ((float)x / (width - 1)));
                             break;
                         case 3:
                             Color leftColor1 = Color.Lerp(backgroundColors[0], backgroundColors[1], ((float)y / (width - 1)));
                             Color rightColor1 = Color.Lerp(backgroundColors[1], backgroundColors[2], ((float)y / (height - 1)));
                             color[x + width * y] = Color.Lerp(leftColor1, rightColor1, ((float)x / (width - 1)));
                             break;
                         case 2:
                             color[x + width * y] = Color.Lerp(backgroundColors[0], backgroundColors[1], ((float)x / (width - 1)));
                             break;
                         default:
                             color[x + width * y] = backgroundColors[0];
                             break;
                     }
                     color[x + width * y] = ColorBorder(x, y, width, height, borderThickness, borderRadius, borderShadow, color[x + width * y], borderColors, initialShadowIntensity, finalShadowIntensity);
                 }
             }

             texture.SetData<Color>(color);

             pixel.SetData(new[] { Color.White }); //make it white so we can color it
             battlemap = new TmxMap("Content\\battleMap.tmx");
             battleText = Game1.instance.Content.Load<SpriteFont>("BattleSystemFont"); //cannot edit in mono, import the .spritefont included into a dummy xna project and edit, bring .xnb back over
        }

        public void DrawBackGroundRectangle()
        {
            SpriteBatch spriteBatch = Game1.spriteBatch;
            spriteBatch.Draw(texture, new Vector2(Game1.screenWidth * 0.5f, Game1.screenHeight * .75f - Game1.screenHeight * .05f), new Rectangle(0, 0, (int)(Game1.screenWidth * 0.425f), (int)(Game1.screenHeight * .20f)), Color.White);
        }

        public void draw()
        {
            Game1.graphics.GraphicsDevice.Clear(Color.White);//new Color(rand.Next(255), rand.Next(255), rand.Next(255)));
            SpriteBatch spriteBatch = Game1.spriteBatch;

            spriteBatch.Begin();

            foreach (TmxLayer layer in battlemap.Layers)
            {
                foreach (TmxLayerTile tile in layer.Tiles)
                {
                    Rectangle rec = new Rectangle((tile.Gid - 1) % Game1.tileInX * Game1.tileWidth + (tile.Gid - 1) % Game1.tileInX * Game1.tileSpacing, tile.Gid / Game1.tileInX * Game1.tileHeight + tile.Gid / Game1.tileInX * Game1.tileSpacing, Game1.tileWidth, Game1.tileHeight);
                    spriteBatch.Draw(Game1.platformerTex, new Vector2(tile.X * 70, tile.Y * 70), rec, Color.White);
                }
            }
            
            DrawBackGroundRectangle();
            Game1.heroParty.draw();
            spriteBatch.End();
        }
    }
}