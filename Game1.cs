#region Using Statements
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

#endregion

namespace DarkEmpire
{

    public class Game1 : Game
    {
        public GraphicsDeviceManager graphics;
        public SpriteBatch spriteBatch;
        TmxMap map, battlemap;
        Texture2D platformerTex;
        Texture2D npcSprite;
        Npc[] npc = new Npc[900];
        Npc[] theHero = new Npc[5];
        InputState inputstate;
        PlayerIndex controlIndex;
        int tileWidth; //width of a sprite tile
        int tileHeight; //height of a sprite tile
        int tileSpacing; //spacing between sprites on sprite sheet
        int tileInX; //max number of sprites along x-axis of sprite sheet
        static Game instance;
        int screenWidth, screenHeight; //size of screen in pixels

        public static Game Instance
        {
            get { return instance; }
        }

        public Game1()
            : base()
        {
            instance = this;
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            base.Initialize();

            this.IsFixedTimeStep = true;
            this.TargetElapsedTime = System.TimeSpan.FromMilliseconds(1000 / 30f); //30 fps 

            graphics.IsFullScreen = true;  //warning going full screen doesn't stretch the graphics
            screenHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height; //not everyone has the same resolution
            screenWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width; //not everyone has the same resolution

            graphics.PreferredBackBufferHeight = screenHeight;
            graphics.PreferredBackBufferWidth = screenWidth;
            graphics.ApplyChanges();

            inputstate = new InputState();


            map = new TmxMap("Content\\TestMap1.tmx");
            battlemap = new TmxMap("Content\\battleMap.tmx");
            platformerTex = Content.Load<Texture2D>("Platformer");
            npcSprite = Content.Load<Texture2D>("npc_sprite");

            for (int i = 1; i <= 899; i++)
                npc[i] = new Npc(i % 9, 1, new Vector2(i * 2, i + rand.Next(-100, 400)));

            theHero[0] = new Npc(1, 3, new Vector2(screenWidth * 0.2f, screenHeight * .1f), 5.0f);
            theHero[1] = new Npc(2, 3, new Vector2(screenWidth * 0.3f, screenHeight * .3f), 5.0f);
            theHero[2] = new Npc(3, 3, new Vector2(screenWidth * 0.1f, screenHeight * .5f), 5.0f);

            tileWidth = map.TileWidth;
            tileHeight = map.TileHeight;
            tileSpacing = map.Tilesets[0].Spacing;
            tileInX = (int)map.Tilesets[0].Image.Width / tileWidth - 1;

            Debug.WriteLine("Version: " + map.Version);
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            this.Services.AddService(typeof(SpriteBatch), spriteBatch); //idk if this was a bad idea, never tried before
            this.Services.AddService(typeof(GraphicsDevice), GraphicsDevice); //trying to avoid passing these over and over through functions
        }

        protected override void UnloadContent()
        {

        }

        Random rand = new Random();
        bool powerup = false;
        bool battleSystem = false;
        protected override void Update(GameTime gameTime)
        {
            inputstate.Update(gameTime);

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            int ymove = 0;
            int xmove = 0;

            if (inputstate.IsKeyPressed(Keys.P, null, out controlIndex))
            {
                powerup = !powerup;
            }

            if (inputstate.IsKeyPressed(Keys.B, null, out controlIndex))
            {
                battleSystem = !battleSystem;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Down))
            {
                ymove = 1;
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.Up))
            {
                ymove = -1;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                xmove = 1;
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                xmove = -1;
            }

            for (int i = 1; i <= 899; i++)
            {
                npc[i].frame = (npc[i].frame + 1) % 3;

                if (ymove > 0)
                    npc[i].changeDirection(1);
                if (ymove < 0)
                    npc[i].changeDirection(4);
                if (ymove == 0 && xmove > 0)
                    npc[i].changeDirection(3);
                if (ymove == 0 && xmove < 0)
                    npc[i].changeDirection(2);

                npc[i].position += new Vector2(xmove, ymove);

                if (npc[i].position.X >= screenWidth)
                    npc[i].position.X = 0;
                if (npc[i].position.X < 0)
                    npc[i].position.X = screenWidth - 32;
                if (npc[i].position.Y >= screenHeight)
                    npc[i].position.Y = 0;
                if (npc[i].position.Y < 0)
                    npc[i].position.Y = screenHeight - 32;

            }


            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();

            //If layers are going to be used, need to check which order they draw in, this may do it backwards (easy fix)
            foreach (TmxLayer layer in map.Layers)
            {
                foreach (TmxLayerTile tile in layer.Tiles)
                {
                    //Rec stores the section of the sprite sheet only containing the texture of the sprite we want
                    Rectangle rec = new Rectangle((tile.Gid - 1) % tileInX * tileWidth + (tile.Gid - 1) % tileInX * tileSpacing, tile.Gid / tileInX * tileHeight + tile.Gid / tileInX * tileSpacing, tileWidth, tileHeight);
                    spriteBatch.Draw(platformerTex, new Vector2(tile.X * 70, tile.Y * 70), rec, Color.White);
                }
            }

            for (int i = 1; i <= 899; i++)
            {
                if (!powerup)
                    spriteBatch.Draw(npcSprite, npc[i].position, npc[i].rect, Color.White);
                else
                    spriteBatch.Draw(npcSprite, npc[i].position, npc[i].rect, new Color(rand.Next(255), rand.Next(255), rand.Next(255)));
            }

            spriteBatch.End();

            //Gonna just erase the screen and draw a battle system for now for testing.
            // All this code could be moved into better classes...battlesystem.cs and npc.cs
            if (battleSystem)
            {
                GraphicsDevice.Clear(Color.White);//new Color(rand.Next(255), rand.Next(255), rand.Next(255)));
                spriteBatch.Begin();

                foreach (TmxLayer layer in battlemap.Layers)
                {
                    foreach (TmxLayerTile tile in layer.Tiles)
                    {
                        Rectangle rec = new Rectangle((tile.Gid - 1) % tileInX * tileWidth + (tile.Gid - 1) % tileInX * tileSpacing, tile.Gid / tileInX * tileHeight + tile.Gid / tileInX * tileSpacing, tileWidth, tileHeight);
                        spriteBatch.Draw(platformerTex, new Vector2(tile.X * 70, tile.Y * 70), rec, Color.White);
                    }
                }
                for (int i = 0; i < 3; i++)
                {
                    spriteBatch.Draw(npcSprite, theHero[i].position, theHero[i].rect, Color.White, 0.0f, Vector2.Zero, theHero[i].Scale, SpriteEffects.None, 0.0f);
                    theHero[i].health = (float)rand.NextDouble(); //lets see the health change in real time...
                    theHero[i].DrawHealthWithOutline();
                }
                spriteBatch.End();

            }

            base.Draw(gameTime);
        }


    }
}
