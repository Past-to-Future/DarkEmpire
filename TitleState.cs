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

namespace DarkEmpire
{
    public class TitleState : State, IDisposable
    {
        private SpriteBatch _spriteBatch;
        private Texture2D _titleBackground;
        private SpriteFont _spriteFont;

        public override void Initialize()
        {
            _spriteBatch = new SpriteBatch(Game.GraphicsDevice);
        }

        public override void LoadContent()
        {
            _titleBackground = Game.Content.Load<Texture2D>("background");
            _spriteFont = Game.Content.Load<SpriteFont>("console");
        }

        public override void Update(GameTime gameTime)
        {
            PlayerIndex _controllerIndex;

            if (Game.InputState.IsButtonPressed(Buttons.Start, null, out _controllerIndex) || Game.InputState.IsKeyPressed(Keys.Space, null, out _controllerIndex))
            {
                Game.ActivePlayerIndex = _controllerIndex;
                if (!Guide.IsVisible)
                {
                    StorageDevice.BeginShowSelector(new AsyncCallback(StorageDeviceSelected), null);
                }

            }
        }

        public void StorageDeviceSelected(IAsyncResult result)
        {
            StartGame();
        }

        public void StartGame()
        {
             Game.StateManager.ActiveState = Game.StateManager.GetState(typeof(PlayingState));
        }

        public override void Draw(GameTime gameTime)
        {
            Game.GraphicsDevice.Clear(Color.Black);
            Rectangle screenRect = new Rectangle(0, 0, _game.GraphicsDevice.Viewport.Width, _game.GraphicsDevice.Viewport.Height);

            _spriteBatch.Begin();
            _spriteBatch.Draw(_titleBackground, screenRect, Color.White);
            _spriteBatch.DrawString(_spriteFont, "Dark Empire\n Press Space Bar", new Vector2(_game.GraphicsDevice.Viewport.Width * 0.15f, _game.GraphicsDevice.Viewport.Height * .35f), Color.White, 0.0f, new Vector2(0,0), 5f, SpriteEffects.None, 0.0f);
            _spriteBatch.End();
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
