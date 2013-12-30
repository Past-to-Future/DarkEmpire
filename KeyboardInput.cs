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
    public class KeyboardInput
    {
        public InputState inputstate;
        PlayerIndex controlIndex;

        public KeyboardInput()
        {

        }

        public void initialize()
        {
            inputstate = new InputState();
        }

        public void Update(GameTime gameTime)
        {
            inputstate.Update(gameTime);

            int ymove = 0;
            int xmove = 0;

            if (inputstate.IsKeyPressed(Keys.P, null, out controlIndex))
            {
                PlayingState.powerup = !PlayingState.powerup;
            }

            if (inputstate.IsKeyPressed(Keys.B, null, out controlIndex))
            {
                PlayingState.battlesystem.activeBattle = !PlayingState.battlesystem.activeBattle;
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
                PlayingState.npc[i].frame = (PlayingState.npc[i].frame + 1) % 3;

                if (ymove > 0)
                    PlayingState.npc[i].changeDirection(1);
                if (ymove < 0)
                    PlayingState.npc[i].changeDirection(4);
                if (ymove == 0 && xmove > 0)
                    PlayingState.npc[i].changeDirection(3);
                if (ymove == 0 && xmove < 0)
                    PlayingState.npc[i].changeDirection(2);

                PlayingState.npc[i].position += new Vector2(xmove, ymove);

                if (PlayingState.npc[i].position.X >= Game1.instance.Width)
                    PlayingState.npc[i].position.X = 0;
                if (PlayingState.npc[i].position.X < 0)
                    PlayingState.npc[i].position.X = Game1.instance.Width - 32;
                if (PlayingState.npc[i].position.Y >= Game1.instance.Height)
                    PlayingState.npc[i].position.Y = 0;
                if (PlayingState.npc[i].position.Y < 0)
                    PlayingState.npc[i].position.Y = Game1.instance.Height - 32;

            }
        
        }
    }
}

