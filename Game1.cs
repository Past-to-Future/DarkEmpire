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
using OpenTK;

#endregion

namespace DarkEmpire
{

    public class Game1 : DarkEmpireGame
    {
        SpriteBatch spriteBatch;
        public static Game1 instance;

        public Game1()
            : base()
        {
            instance = this;
            Content.RootDirectory = "Content";
        }

        public SpriteBatch SpriteBatch
        {
            get { return spriteBatch; }
            set { spriteBatch = value; }
        }

        protected override void Initialize()
        {
            this.IsMouseVisible = true;
            this.IsFixedTimeStep = true;
            this.TargetElapsedTime = System.TimeSpan.FromMilliseconds(1000 / 30f); //30 fps 
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            TitleState _state = new TitleState();
            _state.Game = this;
            _state.Initialize();
            _state.LoadContent();
            StateManager.ActiveState = _state;
        }

        protected override void UnloadContent()
        {

        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
<<<<<<< HEAD
                Exit();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            base.Draw(gameTime);
        }
    }
}


        
        
        
        
        
        
        
        /*public static GraphicsDeviceManager graphics;
        public static SpriteBatch spriteBatch;
        public static int screenWidth, screenHeight; //size of screen in pixels
        public static Game instance;
        public static Texture2D platformerTex;
        public static int tileWidth; //width of a sprite tile
        public static int tileHeight; //height of a sprite tile
        public static int tileSpacing; //spacing between sprites on sprite sheet
        public static int tileInX; //max number of sprites along x-axis of sprite sheet
        public static Texture2D npcSprite;
        public static Npc[] npc = new Npc[900];
        public static BattleSystem battlesystem;
        public static bool powerup = false;
        public static HeroParty heroParty;
        Texture2D battleline;
        Rectangle battleline_req = new Rectangle(350, 720, 700, 150);
        Random rand = new Random();

        TmxMap map;
<<<<<<< HEAD
        KeyboardInput keyboardInput;
=======
        public static KeyboardInput keyboardInput;
        
>>>>>>> origin/Tim's-Branch

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

            heroParty = new HeroParty();

            keyboardInput = new KeyboardInput();
            keyboardInput.initialize();
            battlesystem = new BattleSystem();
            battlesystem.initialize();

            map = new TmxMap("Content\\TestMap1.tmx");
            platformerTex = Content.Load<Texture2D>("Platformer");
            npcSprite = Content.Load<Texture2D>("npc_sprite");

            for (int i = 1; i <= 899; i++)
                npc[i] = new Npc(i % 9, 1, new Vector2(i * 2, i + rand.Next(-100, 400)));


            battleline = Content.Load<Texture2D>("Line Dark Empire");
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

        protected override void Update(GameTime gameTime)
        {
            keyboardInput.Update(gameTime);

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
=======
>>>>>>> origin/Tim's-Branch
            {
                Exit();
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            base.Draw(gameTime);
        }
    }
<<<<<<< HEAD
<<<<<<< HEAD
}
=======
}*/

=======
}
>>>>>>> origin/Tim's-Branch
