using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Storage;
using DarkEmpire.Tiled;

namespace DarkEmpire
{
    public class Level
    {
        TmxMap _map;
        SpriteBatch spriteBatch = Game1.instance.SpriteBatch;
        Texture2D _platformerTex;
        public static int tileWidth;
        public static int tileHeight;
        public static int tileSpacing;
        public static int tileInX; //max number of sprites along x-axis of sprite sheet

        public Level(String levelName, String spriteName = "Platformer")
        {
            Npc.pixel.SetData(new[] { Color.White }); //make it white so we can color it
            _map = new TmxMap("Content\\" + levelName + ".tmx");
            _platformerTex = Game1.instance.Content.Load<Texture2D>(spriteName);
            tileWidth = _map.TileWidth;
            tileHeight = _map.TileHeight;
            tileSpacing = _map.Tilesets[0].Spacing;
            tileInX = (int)_map.Tilesets[0].Image.Width / tileWidth - 1; 
        }

        public Texture2D platformerTex
        {
            get { return _platformerTex; }
            set { _platformerTex = value; }
        }

        public void Draw()
        {
            foreach (TmxLayer layer in _map.Layers)
            {
                foreach (TmxLayerTile tile in layer.Tiles)
                {
                    Rectangle rec = new Rectangle((tile.Gid - 1) % tileInX * tileWidth + (tile.Gid - 1) % tileInX * tileSpacing, tile.Gid / tileInX * tileHeight + tile.Gid / tileInX * tileSpacing, tileWidth, tileHeight);
                    spriteBatch.Draw(_platformerTex, new Vector2(tile.X * 70 / 2, tile.Y * 70 / 2), rec, Color.White, 0.0f, new Vector2(0, 0), 0.5f, SpriteEffects.None, 0.0f);
                }
            }
        }

    }
}
