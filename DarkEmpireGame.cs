using System;
using System.Collections.Generic;
using System.Linq;
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
    public class DarkEmpireGame : Game
    {
        private StateManager _stateManager;
        private InputState _inputState;
        private GraphicsDeviceManager _graphics;
        private PlayerIndex _activePlayerIndex;

        private StorageDevice _storageDevice;
        private float _width, _height;

        public DarkEmpireGame()
            : base()
        {
            _graphics = new GraphicsDeviceManager(this);

            _width = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width; //896;
            _height = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height; //504;
            float ratio = _width/_height; 
            _graphics.PreferredBackBufferHeight = (int)_height;
            _graphics.PreferredBackBufferWidth = (int)_width;
            _graphics.IsFullScreen = true;// false;
            _graphics.ApplyChanges();
            Content.RootDirectory = "Content";

            _stateManager = new StateManager(this);
            _inputState = new InputState();
        }

        public float Width
        {
            get { return _width; }
            set { _width = value; }
        }

        public float Height
        {
            get { return _height; }
            set { _height = value; }
        }

        public StorageDevice StorageDevice
        {
            get { return _storageDevice; }
            set { _storageDevice = value; }
        }

        public PlayerIndex ActivePlayerIndex
        {
            get { return _activePlayerIndex; }
            set { _activePlayerIndex = value; }
        }

        public StateManager StateManager
        {
            get { return _stateManager; }
        }

        public InputState InputState
        {
            get { return _inputState; }
        }

        public GraphicsDeviceManager Graphics
        {
            get { return _graphics; }
            set { _graphics = value; }
        }

        protected override void Initialize()
        {
            _stateManager.Initialize();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _stateManager.LoadContent();
            base.LoadContent();
        }


        protected override void Update(GameTime gameTime)
        {
            _inputState.Update(gameTime);
            _stateManager.Update(gameTime);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            _stateManager.Draw(gameTime);
            base.Draw(gameTime);
        }
    }
}
