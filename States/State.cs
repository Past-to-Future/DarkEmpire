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
    public abstract class State
    {
        public DarkEmpireGame _game;

        public State()
        {
        }

        public DarkEmpireGame Game
        {
            get { return _game; }
            set { _game = value; }
        }

        public abstract void Initialize();
        public abstract void LoadContent();
        public abstract void Update(GameTime gameTime);
        public abstract void Draw(GameTime gameTime);

    }
}
