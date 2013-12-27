using System;
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

        public BattleSystem()
        {
            activeBattle = false;
        }

        public void initialize()
        {
             pixel.SetData(new[] { Color.White }); //make it white so we can color it
             battlemap = new TmxMap("Content\\battleMap.tmx");
             battleText = Game1.instance.Content.Load<SpriteFont>("BattleSystemFont"); //cannot edit in mono, import the .spritefont included into a dummy xna project and edit, bring .xnb back over
        }

        public void DrawBackGroundRectangle()
        {
            SpriteBatch spriteBatch = Game1.spriteBatch;
            spriteBatch.Draw(BattleSystem.pixel, new Vector2(Game1.screenWidth * 0.5f, Game1.screenHeight * .75f - Game1.screenHeight * .05f), new Rectangle(0, 0, (int)(Game1.screenWidth * 0.425f), (int)(Game1.screenHeight * .20f)), Color.White);
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
