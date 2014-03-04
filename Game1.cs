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
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        TmxMap map, battlemap;
        Texture2D platformerTex;
        Texture2D npcSprite;
        Texture2D battleline;
        Rectangle battleline_rec = new Rectangle(350, 720, 700, 150);
        Texture2D option_box;
        Texture2D actor_box;
        Texture2D actor_box_full;
        Texture2D actor1_box;
        Texture2D currency_box;
        
        
        private SpriteFont font;
        //SpriteFont font = Content.Load<SpriteFont>("courier");
        Npc[] npc = new Npc[900];
        Npc[] theHero = new Npc[6];
        InputState inputstate;
        PlayerIndex controlIndex;
        int tileWidth; //width of a sprite tile
        int tileHeight; //height of a sprite tile
        int tileSpacing; //spacing between sprites on sprite sheet
        int tileInX; //max number of sprites along x-axis of sprite sheet

        int screenWidth, screenHeight, pscreenWidth, pscreenHeight, cscreenWidth, cscreenHeight; //size of screen in pixels

        public Game1()
            : base()
        {
            graphics = new GraphicsDeviceManager(this);
            
            graphics.PreferredBackBufferWidth = 896;
            graphics.PreferredBackBufferHeight = 504;

            
            //float asp_ratio_screen = (float)graphics.PreferredBackBufferWidth / (float)graphics.PreferredBackBufferHeight;
            graphics.IsFullScreen = false;
            screenHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            screenWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
        //    pscreenHeight = graphics.PreferredBackBufferHeight;
        //    pscreenWidth = graphics.PreferredBackBufferWidth;
         //   cscreenHeight = graphics.PreferredBackBufferHeight;
          //  cscreenWidth = graphics.PreferredBackBufferWidth; 

            graphics.ApplyChanges();

            
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            base.Initialize();

            this.IsFixedTimeStep = true;
            this.TargetElapsedTime = System.TimeSpan.FromMilliseconds(1000 / 30f);

           // graphics.IsFullScreen = false;  //warning going full screen doesn't stretch the graphics
            
            //screenHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height; //not everyone has the same resolution
            //screenWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width; //not everyone has the same resolution



            //graphics.PreferredBackBufferWidth = 896;
            //graphics.PreferredBackBufferHeight = 1000;
            //graphics.IsFullScreen = false;
            //graphics.ApplyChanges();

            inputstate = new InputState();


            map = new TmxMap("Content\\TestMap1.tmx");
            battlemap = new TmxMap("Content\\battleMap.tmx");
            platformerTex = Content.Load<Texture2D>("Platformer");
            npcSprite = Content.Load<Texture2D>("npc_sprite");
            battleline = Content.Load<Texture2D>("Line Dark Empire");
            option_box = Content.Load<Texture2D>("box");
            actor_box = Content.Load<Texture2D>("box1");
            actor_box_full = Content.Load<Texture2D>("box1");
            actor1_box = Content.Load<Texture2D>("box3");
            currency_box = Content.Load<Texture2D>("box2");
           // text_box = Content.Load<Texture2D>("text");
            font = Content.Load<SpriteFont>("text");
            //font = Content.Load<SpriteFont>("spinwerad");
            
            for(int i = 1; i <= 899; i ++)
                npc[i] = new Npc(i%9, 1, new Vector2(i*2,i+rand.Next(-100,400)));

            theHero[0] = new Npc(1, 3, new Vector2(screenWidth * 0.2f, screenHeight * .1f), 5.0f);
            theHero[1] = new Npc(2, 3, new Vector2(screenWidth * 0.3f, screenHeight * .3f), 5.0f);
            theHero[2] = new Npc(3, 3, new Vector2(screenWidth * 0.1f, screenHeight * .5f), 5.0f);
            theHero[3] = new Npc(1, 2, new Vector2(screenWidth * 0.5f, screenHeight * .1f), 5.0f);
            theHero[4] = new Npc(2, 2, new Vector2(screenWidth * 0.6f, screenHeight * .3f), 5.0f);
            theHero[5] = new Npc(3, 2, new Vector2(screenWidth * 0.4f, screenHeight * .5f), 5.0f);

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
        }

        protected override void UnloadContent()
        {

        }

        Random rand = new Random();
        bool powerup = false;
        bool battleSystem = false;
        bool menu = false;
        protected override void Update(GameTime gameTime)
        {
            inputstate.Update(gameTime);

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            int ymove = 0;
            int xmove = 0;

            if (inputstate.IsKeyPressed(Keys.M, null, out controlIndex))
            {
                menu = !menu;
            }

            if (inputstate.IsKeyPressed(Keys.P, null, out controlIndex))
            {
                powerup = !powerup;
            }

            if (inputstate.IsKeyPressed(Keys.B, null, out controlIndex))
            {
                battleSystem = !battleSystem;
            }

            if (inputstate.IsKeyPressed(Keys.F, null, out controlIndex))
            {
                graphics.IsFullScreen = !graphics.IsFullScreen;
               // graphics.ToggleFullScreen();
                
             //   if (graphics.IsFullScreen==true)
             //  {
             //     graphics.PreferredBackBufferWidth = 1920;
             //     graphics.PreferredBackBufferHeight = 1080;
                  
                    
            //    }
//
             //   if (graphics.IsFullScreen == false)
            //    {
            //        graphics.PreferredBackBufferWidth = 896;
            //        graphics.PreferredBackBufferHeight = 504;
                    
             //   }
                graphics.ApplyChanges();
                    //string[] lines = { "First line", "Second line"};
                   // lines[0] = "Window Width = " + screenWidth;
                   // lines[1] = "Window Height = " + screenHeight;
                   // System.IO.File.WriteAllLines(@"C:\Users\anast_000\Desktop\Resolution3.txt", lines);
                
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

        private Rectangle transform_graphics(Rectangle rect, int screenW, int screenH) {

            string[] lines = { "First line", "Second line", "Third line", "Fourth Line", "FIFTH", "SIXTH","Seventh", "Eighth","Ninth","Tenth", "Eleventh", "Twelveth", "13","14", "15", "16" };

            float asp_ratio_actor_coordinates = (float)rect.X / (float)rect.Y;
            float asp_ratio_actor_dimentions = (float)rect.Width / (float)rect.Height;
            float asp_ratio_sc_act_coordinates = (float)896 / (float)rect.X;
            float asp_ratio_sc_act_dimentions = (float)896 / (float)rect.Width;


           // lines[12] = "Previous Width = " +rect.Width.ToString();
           // lines[13] = "Previous Height = " + rect.Height.ToString();
           // lines[14] = "Previous X = " +rect.X.ToString();
           // lines[15] = "Previous Y = "+ rect.Y.ToString();

            Rectangle rect2 = new Rectangle(0,0,0,0);

            rect2.Width = (int)((float)screenW / asp_ratio_sc_act_dimentions);
            rect2.Height = (int)((float)rect2.Width / asp_ratio_actor_dimentions);
            rect2.X = (int)((float)screenW / asp_ratio_sc_act_coordinates);
            rect2.Y = (int)((float)rect2.X / asp_ratio_actor_coordinates);

            lines[0] = "Width = " + rect.Width.ToString();
            lines[1] = "Height = " + rect.Height.ToString();
            lines[2] = "X = " + rect.X.ToString();
            lines[3] = "Y = " + rect.Y.ToString();
            lines[4] = "Previous Window Width = " + screenW;
            lines[5] = "Previous Window Height = " + screenH;
            lines[6] = "asp_ratio_actor_coordinates = rect.X / rect.Y = " + asp_ratio_actor_coordinates.ToString();
            lines[7] = "asp_ratio_actor_dimentions = rect.Width / rect.Height = " + asp_ratio_actor_dimentions.ToString();
            lines[8] = "asp_ratio_sc_act_coordinates = screenW / rect.X = " + asp_ratio_sc_act_coordinates.ToString();
            lines[9] = "asp_ratio_sc_act_dimentions = screenW / rect.Width = " + asp_ratio_sc_act_dimentions.ToString();
            lines[10] = "Current Window Width = " + screenW;
            lines[11] = "Current Window Height = " + screenH;
            

            System.IO.File.WriteAllLines(@"C:\Users\anast_000\Desktop\Resolution_tranform.txt", lines);

            return rect2;
        
        }
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);
            
            spriteBatch.Begin();

          //  If layers are going to be used, need to check which order they draw in, this may do it backwards (easy fix)
            foreach (TmxLayer layer in map.Layers){
                foreach (TmxLayerTile tile in layer.Tiles)
                {
                   //Rec stores the section of the sprite sheet only containing the texture of the sprite we want
                    Rectangle rec = new Rectangle((tile.Gid - 1) % tileInX * tileWidth + (tile.Gid - 1) % tileInX * tileSpacing, tile.Gid / tileInX * tileHeight + tile.Gid / tileInX * tileSpacing, tileWidth, tileHeight);
                   spriteBatch.Draw(platformerTex, new Vector2(tile.X*70, tile.Y*70), rec, Color.White);
               }
            }

            for (int i = 1; i <= 899; i++)
            {
                if(!powerup)
                    spriteBatch.Draw(npcSprite, npc[i].position, npc[i].rect, Color.White);
                else
                    spriteBatch.Draw(npcSprite, npc[i].position, npc[i].rect, new Color(rand.Next(255), rand.Next(255), rand.Next(255)));
            }

            spriteBatch.End();

            //Gonna just erase the screen and draw a battle system for now for testing.
            if (battleSystem)
            {
                GraphicsDevice.Clear(Color.White);
                new Color(rand.Next(255), rand.Next(255), rand.Next(255));
                spriteBatch.Begin();

                foreach (TmxLayer layer in battlemap.Layers)
                {
                    foreach (TmxLayerTile tile in layer.Tiles)
                    {
                        Rectangle rec = new Rectangle((tile.Gid - 1) % tileInX * tileWidth + (tile.Gid - 1) % tileInX * tileSpacing, tile.Gid / tileInX * tileHeight + tile.Gid / tileInX * tileSpacing, tileWidth, tileHeight);
                        spriteBatch.Draw(platformerTex, new Vector2(tile.X * 70, tile.Y * 70), rec, Color.White);
                    }
                }
               for (int i = 0; i < 6; i++)
                {
                    spriteBatch.Draw(npcSprite, theHero[i].position, theHero[i].rect, Color.White, 0.0f, Vector2.Zero, theHero[i].Scale, SpriteEffects.None, 0.0f);
                    
                }

               spriteBatch.Draw(battleline, battleline_rec, Color.White);
                spriteBatch.End();

            }

            if (menu)
            {
                GraphicsDevice.Clear(Color.CornflowerBlue);
                


                
                //graphics.PreferredBackBufferHeight = 504;
               // graphics.IsFullScreen = false;

                //graphics.PreferredBackBufferWidth = 896;
                //graphics.PreferredBackBufferHeight = 504;

                

                Rectangle option_box_rec = new Rectangle(0, 0, 98, 220);
                Rectangle actor_box_rec = new Rectangle(448, 50, 200, 150);
                Rectangle actor_box_rec_full = transform_graphics(actor_box_rec, 1920,1080);
                Rectangle actor1_box_rec = new Rectangle(600, 50, 800, 600);
                Rectangle currency_box_rec = new Rectangle(700, 700, 401, 92);

                
               // float asp_ratio_actor_coordinates = (float)actor_box_rec.X / (float)actor_box_rec.Y;
               // float asp_ratio_actor_dimentions = (float)actor_box_rec.Width / (float)actor_box_rec.Height;
              //  float asp_ratio_sc_act_coordinates = (float)graphics.PreferredBackBufferWidth / (float)actor_box_rec.X;
               // float asp_ratio_sc_act_dimentions = (float)graphics.PreferredBackBufferWidth / (float)actor_box_rec.Width;

                if (graphics.IsFullScreen == false)
                {
                    //string[] lines = { "First line", "Second line", "Third line", "Fourth Line", "dde", "fsre" };
                    actor_box_rec.X = 448;
                    actor_box_rec.Y=50;
                    actor_box_rec.Width=200;
                    actor_box_rec.Height=150;
                    spriteBatch.Begin();
                    //Rectangle currency_box_rec = new Rectangle(50, 700, 200, 50);
                    //Rectangle text_box_rec = new Rectangle(95, 60, 100, 40);
                //    spriteBatch.Draw(option_box, option_box_rec, Color.White);
                    spriteBatch.Draw(actor_box, actor_box_rec, Color.White);
                    spriteBatch.Draw(currency_box, currency_box_rec, Color.White);
                    //spriteBatch.Draw(text_box, text_box_rec, Color.White);
                    //spriteBatch.Draw(currency_box, new Vector2(screenWidth * 0.026f, screenHeight * 0.63f), currency_box_rec, Color.White, 0.0f, Vector2.Zero, 0.5f, SpriteEffects.None, 0.0f);

                    //spriteBatch.Draw(currency_box, new Vector2(screenWidth * 0.026f, screenHeight * 0.56f), currency_box_rec, Color.White, 0.0f, Vector2.Zero, 0.5f, SpriteEffects.None, 0.0f);
                     //spriteBatch.Draw(option_box, new Vector2(screenWidth * 0.026f, screenHeight * 0.046f), option_box_rec, Color.White, 0.0f, Vector2.Zero, 2.04f, SpriteEffects.None, 0.0f);
                    // spriteBatch.Draw(actor1_box, new Vector2(screenWidth * 0.182f, screenHeight * 0.046f), actor1_box_rec, Color.White, 0.0f, Vector2.Zero, 1.001f, SpriteEffects.None, 0.0f);


                 //   lines[0] = "Width = " + actor_box_rec.Width.ToString();
                //    lines[1] = "Height = " + actor_box_rec.Height.ToString();
                //    lines[2] = "X = " + actor_box_rec.X.ToString();
                 //   lines[3] = "Y = " + actor_box_rec.Y.ToString();
                  //  lines[4] = "Window Width = " + screenWidth;
                 //   lines[5] = "Window Height = " + screenHeight;
                  //  System.IO.File.WriteAllLines(@"C:\Users\anast_000\Desktop\Resolution1.txt", lines);
               //     spriteBatch.DrawString(font, "Status", new Vector2(100, 80), Color.Black);
                //    spriteBatch.DrawString(font, "Items", new Vector2(100, 140), Color.Black);
                //    spriteBatch.DrawString(font, "Equipment", new Vector2(100, 200), Color.Black);
                //    spriteBatch.DrawString(font, "Magic", new Vector2(100, 260), Color.Black);
                //    spriteBatch.DrawString(font, "Abilities", new Vector2(100, 320), Color.Black);
                 //   spriteBatch.DrawString(font, "Options", new Vector2(100, 380), Color.Black);
                    spriteBatch.End();

                }
                
                else {
                   string[] lines = { "First line", "Second line", "Third line","Fourth Line","dde","fsre" };

                 //  actor_box_rec=transform_graphics(actor_box_rec, pscreenWidth,pscreenHeight, cscreenWidth,cscreenHeight);
                    
                    spriteBatch.Begin();
                    spriteBatch.Draw(actor_box_full, actor_box_rec_full, Color.White);
                    //spriteBatch.Draw(currency_box, currency_box_rec, Color.White);
                    spriteBatch.End();

                    
                  //  lines[0]="Width = " +actor_box_rec.Width.ToString();
                  //  lines[1] = "Height = "+actor_box_rec.Height.ToString();
                  //  lines[2] = "X = " + actor_box_rec.X.ToString();
                  //  lines[3] = "Y = " + actor_box_rec.Y.ToString();
                  //  lines[4] = "Window Width = " + screenWidth;
                  //  lines[5] = "Window Height = " + screenHeight;
                  //  System.IO.File.WriteAllLines(@"C:\Users\anast_000\Desktop\Resolution.txt", lines);
                }
            }

            base.Draw(gameTime);
        }


    }
}
