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
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;


namespace DarkEmpire
{
    public class KeyboardInput
    {
        public static InputState inputstate;
        public static PlayerIndex controlIndex;
        public static SoundEffect soundEngine, hit;
        public static SoundEffectInstance soundEngineInstance, hitInstance;
        Song heroTheme;
        int characterSelection1 = 0;
        int characterSelection2 = 0;

        public KeyboardInput()
        {

        }

        public void initialize()
        {
            inputstate = new InputState();
            soundEngine = Game1.instance.Content.Load<SoundEffect>("misc_menu");
            hit = Game1.instance.Content.Load<SoundEffect>("hit28");
            soundEngineInstance = soundEngine.CreateInstance();
            hitInstance = hit.CreateInstance();
            heroTheme = Game1.instance.Content.Load<Song>("Heroes Theme_0.wav");
            //MediaPlayer.Play(heroTheme);
        }

        int count = 0;
        int xmove = 0, ymove = 0;
        public void Update(GameTime gameTime)
        {
            inputstate.Update(gameTime);


            ymove = 0;
            xmove = 0;

            if (inputstate.IsKeyPressed(Keys.R, null, out controlIndex))
            {
                int _width = 896;
                int _height = 504;
                float ratio = _width / _height;
                Game1.instance.Graphics.PreferredBackBufferHeight = (int)_height;
                Game1.instance.Graphics.PreferredBackBufferWidth = (int)_width;
                Game1.instance.Graphics.ApplyChanges();
            }

            if (inputstate.IsKeyPressed(Keys.T, null, out controlIndex))
            {
                int screenHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
                int screenWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
                Game1.instance.Graphics.PreferredBackBufferHeight = screenHeight;
                Game1.instance.Graphics.PreferredBackBufferWidth = screenWidth;
                //Game1.instance.Graphics.IsFullScreen = true;
                Game1.instance.Graphics.ApplyChanges();
            }

            if (inputstate.IsKeyPressed(Keys.U, null, out controlIndex))
            {
                //Game1.instance.Graphics.IsFullScreen = false;
                int screenHeight = 504;
                int screenWidth = 896;
                Game1.instance.Graphics.PreferredBackBufferHeight = screenHeight;
                Game1.instance.Graphics.PreferredBackBufferWidth = screenWidth;
                Game1.instance.Graphics.ApplyChanges();
            }

            if (inputstate.IsKeyPressed(Keys.P, null, out controlIndex))
            {
                PlayingState.powerup = !PlayingState.powerup;
            }

            if (inputstate.IsKeyPressed(Keys.B, null, out controlIndex))
            {
                PlayingState.battlesystem.activeBattle = !PlayingState.battlesystem.activeBattle;
            }

            if (inputstate.IsKeyPressed(Keys.M, null, out controlIndex))
            {
                PlayingState.menu.activeMenu = !PlayingState.menu.activeMenu;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Down))
            {
                ymove = 1;
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.Up))
            {
                ymove = -1;
            }
            else
                ymove = 0;
            if (Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                xmove = 1;
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                xmove = -1;
            }
            count++;

            if (inputstate.IsKeyPressed(Keys.Enter, null, out controlIndex))
            {
                if (PlayingState.menu.characterSelection == 10)
                    PlayingState.menu.selectCharacter = false;
                else if (PlayingState.menu.menuSelection < 7 && !PlayingState.menu.selectCharacter)
                {
                    PlayingState.menu.selectCharacter = true;
                    characterSelection1 = 0;
                    characterSelection2 = 0;
                }

                else if (PlayingState.menu.selectCharacter)
                {
                    if(characterSelection1 == 0)
                        characterSelection1 = PlayingState.menu.characterSelection;
                    else if(characterSelection2 == 0)
                        characterSelection2 = PlayingState.menu.characterSelection;
                    if (characterSelection1 != 0 && characterSelection2 != 0)
                    {
                        HeroParty.swap(characterSelection1, characterSelection2);
                        characterSelection1 = 0;
                        characterSelection2 = 0;
                    }
                }

            }

            if (inputstate.IsKeyPressed(Keys.Back, null, out controlIndex))
            {
                PlayingState.menu.selectCharacter = false;
            }

            if (PlayingState.menu.activeMenu == false)
            {
                if (count >= 5)
                {
                    count = 0;
                    for (int i = 1; i <= 100; i++)
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

                        PlayingState.npc[i].position += new Vector2(18*xmove, 18*ymove);

                        if (PlayingState.npc[i].position.X >= Game1.instance.Width)
                            PlayingState.npc[i].position.X = 0;
                        if (PlayingState.npc[i].position.X < 0)
                            PlayingState.npc[i].position.X = Game1.instance.Width - 60;
                        if (PlayingState.npc[i].position.Y >= Game1.instance.Height)
                            PlayingState.npc[i].position.Y = 0;
                        if (PlayingState.npc[i].position.Y < 0)
                            PlayingState.npc[i].position.Y = Game1.instance.Height - 60;

                    }
                }
            }

            else
            {
                /*Menu is active*/


                /*Pick character*/

                if (PlayingState.menu.selectCharacter && xmove != 0)
                {
                    if (count >= 3)
                    {
                        count = 0;
                        soundEngineInstance.Stop();
                        soundEngineInstance.Play();
                        PlayingState.menu.characterSelection += xmove;
                        if (PlayingState.menu.characterSelection > 3)
                            PlayingState.menu.characterSelection = 1;
                        else if (PlayingState.menu.characterSelection < 1)
                            PlayingState.menu.characterSelection = 3;
                    }
                }
                /*Toggle between pick character and back button*/
                if (PlayingState.menu.selectCharacter && ymove != 0)
                {
                    if (count >= 3)
                    {
                        count = 0;
                        soundEngineInstance.Stop();
                        soundEngineInstance.Play();
                        if (PlayingState.menu.characterSelection == 10)
                            PlayingState.menu.characterSelection = 1;
                        else
                            PlayingState.menu.characterSelection = 10;
                    }
                }

                
                /*Main menu options*/
                if (ymove != 0 && !PlayingState.menu.selectCharacter)
                {
                    if (count >= 2)
                    {
                        count = 0;
                        soundEngineInstance.Stop();
                        soundEngineInstance.Play();
                        PlayingState.menu.menuSelection += ymove;
                        if (PlayingState.menu.menuSelection < 1)
                            PlayingState.menu.menuSelection = 10;
                        if (PlayingState.menu.menuSelection > 10)
                            PlayingState.menu.menuSelection = 1;
                    }
                }
            }
        }
    
    }
}

