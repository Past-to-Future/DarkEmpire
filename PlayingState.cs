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
    class PlayingState : State, IDisposable
    {
        Texture2D _npcSprite;
        private SpriteBatch _spriteBatch;
        private SpriteFont _spriteFont;
        public static Npc[] npc = new Npc[900];
        public static BattleSystem battlesystem;
        public static Menu menu;
        public static bool powerup = false;
        public static  HeroParty heroParty;
        public static KeyboardInput keyboardInput;
        public static Level level;
        public static PlayingState instance;

        Random rand = new Random();

        public override void Initialize()
        {
            instance = this;
            _spriteBatch = Game1.instance.SpriteBatch;
            heroParty = new HeroParty();
            level = new Level("Level1");
            battlesystem = new BattleSystem();
            battlesystem.initialize();
            menu = new Menu();
            menu.Initialize();

            for (int i = 1; i <= 100; i++)
                npc[i] = new Npc(i % 4, 1, new Vector2(i * 2, i + rand.Next(-100, 400)));
        }

        public Texture2D npcSprite
        {
            get { return _npcSprite; }
            set { _npcSprite = value; }
        }

        public override void LoadContent()
        {
            _spriteFont = Game.Content.Load<SpriteFont>("console");
            _npcSprite = Game.Content.Load<Texture2D>("npc_sprite");
            keyboardInput = new KeyboardInput();
            keyboardInput.initialize();          
        }

        public override void Update(GameTime gameTime)
        {
            keyboardInput.Update(gameTime);

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                battlesystem.attackThread.Abort();
                battlesystem.battleThread.Abort();
                Game.Exit();
            }
        }

        public override void Draw(GameTime gameTime)
        {
            _game.GraphicsDevice.Clear(Color.CornflowerBlue);
            
            _spriteBatch.Begin();
            
            level.Draw();

            for (int i = 1; i <= 100; i++)
                npc[i].draw();

            _spriteBatch.End(); 

            //Gonna just erase the screen and draw a battle system for now for testing.
            //Clear must be done at the start of spritebatch. Therefore I had end the one above.
            if (battlesystem.activeBattle)
            {
                _spriteBatch.Begin(); 
                _game.GraphicsDevice.Clear(Color.CornflowerBlue); 
                battlesystem.draw();
                _spriteBatch.End();
            }

            if (menu.activeMenu)
            {
                _spriteBatch.Begin(); 
                menu.Draw();
                _spriteBatch.End();
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_spriteBatch != null)
                {
                    _spriteBatch.Dispose();
                }
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
    }
}
