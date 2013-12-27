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
        public static GraphicsDeviceManager graphics;
        public static SpriteBatch spriteBatch;
        public static int screenWidth, screenHeight; //size of screen in pixels
        public static Game instance;
        public static Texture2D platformerTex;
        public static int tileWidth; //width of a sprite tile
        public static int tileHeight; //height of a sprite tile
        public static int tileSpacing; //spacing between sprites on sprite sheet
        public static int tileInX; //max number of sprites along x-axis of sprite sheet
        public static Npc[] theHero = new Npc[5];
        public static Texture2D npcSprite;
        public static Npc[] npc = new Npc[900];
        public static BattleSystem battlesystem;

        TmxMap map;

        InputState inputstate;
        PlayerIndex controlIndex;

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
            screenWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width; 

            graphics.PreferredBackBufferHeight = screenHeight;
            graphics.PreferredBackBufferWidth = screenWidth;
            graphics.ApplyChanges();
            inputstate = new InputState();
            battlesystem = new BattleSystem();
            battlesystem.initialize();

            map = new TmxMap("Content\\TestMap1.tmx");
            platformerTex = Content.Load<Texture2D>("Platformer");
            npcSprite = Content.Load<Texture2D>("npc_sprite");

            for (int i = 1; i <= 899; i++)
                npc[i] = new Npc(i % 9, 1, new Vector2(i * 2, i + rand.Next(-100, 400)));

            theHero[0] = new Npc(1, 3, new Vector2(screenWidth * 0.2f, screenHeight * .1f), 5.0f);
            theHero[1] = new Npc(2, 3, new Vector2(screenWidth * 0.3f, screenHeight * .3f), 5.0f);
            theHero[2] = new Npc(3, 3, new Vector2(screenWidth * 0.1f, screenHeight * .5f), 5.0f);
            
            theHero[0].health = 0.25f;
            theHero[1].health = 0.5f;
            theHero[2].health = 0.75f;

            theHero[0].setPartyPosition(0);
            theHero[1].setPartyPosition(1);
            theHero[2].setPartyPosition(2);

            theHero[0].name = "Sir Francis of Normandy";
            theHero[1].name = "Gandalf the White";
            theHero[2].name = "Frosty the Snowman";

            tileWidth = map.TileWidth;
            tileHeight = map.TileHeight;
            tileSpacing = map.Tilesets[0].Spacing;
            tileInX = (int)map.Tilesets[0].Image.Width / tileWidth - 1;
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        protected override void UnloadContent()
        {

        }

        Random rand = new Random();
        bool powerup = false;
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
                battlesystem.activeBattle = !battlesystem.activeBattle;
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
            if (battlesystem.activeBattle)
            {
                battlesystem.draw();
            }

            base.Draw(gameTime);
        }


    }
}
