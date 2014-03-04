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
using System.Threading;
using System.Collections;
namespace DarkEmpire
{
    public class BattleSystem
    {
        public static Texture2D pixel = new Texture2D(Game1.instance.GraphicsDevice, 1, 1); //create 1x1 pixel texture
        public static SpriteFont battleText;
        SpriteBatch spriteBatch = Game1.instance.SpriteBatch;
        public bool activeBattle;
        static List<int[]> battleQueue = new List<int[]>();
        TmxMap battlemap;
        public Thread battleThread;
        public Thread attackThread;
        String status = "Status";
        Vector2 statusSize;
        bool paused = true;
        int[] saveAttack = null;
        static int heroTurn = 0;

        public BattleSystem()
        {
            activeBattle = false;
        }

         public void initialize()
        {
            
            pixel.SetData(new[] { Color.White }); //make it white so we can color it
            battlemap = new TmxMap("Content\\battleMap.tmx");
            battleText = Game1.instance.Content.Load<SpriteFont>("BattleSystemFont"); //cannot edit in mono, import the .spritefont included into a dummy xna project and edit, bring .xnb back over
            battleThread =  new Thread(new ThreadStart(DoBattle));
            attackThread = new Thread(new ThreadStart(DoAttack));
            battleThread.Start();
            attackThread.Start();
            statusSize = battleText.MeasureString(status);

        }

        Random rand = new Random();
        float moving = 0f;

        private void DoAttack()
         {
             while (true)
             {
                 /*Character needs to show attack animation*/
                 if (saveAttack != null)
                 {
                     float dist = Vector2.Distance(HeroParty.theHero[saveAttack[2]].position, HeroParty.theEnemy[saveAttack[3]].position);
                     if (dist > 64)
                     {
                         moving += 4 * 0.16f / dist;
                         HeroParty.theHero[saveAttack[2]].position = Vector2.Lerp(HeroParty.theHero[saveAttack[2]].position, HeroParty.theEnemy[saveAttack[3]].position, moving);
                     }
                     else
                     {
                         KeyboardInput.hitInstance.Stop();
                         KeyboardInput.hitInstance.Play();
                         Thread.Sleep(200);
                         KeyboardInput.hitInstance.Stop();
                         KeyboardInput.hitInstance.Play();
                         Thread.Sleep(200);
                         KeyboardInput.hitInstance.Stop();
                         KeyboardInput.hitInstance.Play();

                         Thread.Sleep(1500);

                         if (saveAttack[2] == 0)
                             HeroParty.theHero[saveAttack[2]].position = new Vector2(Game1.instance.Width * 0.2f, Game1.instance.Height * .2f);
                         else if (saveAttack[2] == 1)
                             HeroParty.theHero[saveAttack[2]].position = new Vector2(Game1.instance.Width * 0.1f, Game1.instance.Height * .4f);
                         else
                             HeroParty.theHero[saveAttack[2]].position = new Vector2(Game1.instance.Width * 0.2f, Game1.instance.Height * .6f);
                         saveAttack = null;
                     }
                 }
                 
                 Thread.Sleep(50);
             }
         }

       
        private void DoBattle()
        {
            while (true)
            {
                if (paused)
                {

                }
                else
                {
                    for (int i = 0; i < battleQueue.Count; i++)
                    {
                        battleQueue[i][1] -= 100;
                        if (battleQueue[i][1] <= 0)
                        {
                            saveAttack = battleQueue[i];
                            battleQueue.Remove(battleQueue[i]);
                            paused = true;
                        }
                    }
                }
                Thread.Sleep(100);
            }
        }

        public void draw()
        {
            Game1.instance.GraphicsDevice.Clear(Color.White);//new Color(rand.Next(255), rand.Next(255), rand.Next(255)));
            foreach (TmxLayer layer in battlemap.Layers)
            {
                foreach (TmxLayerTile tile in layer.Tiles)
                {
                    Rectangle rec = new Rectangle((tile.Gid - 1) % Level.tileInX * Level.tileWidth + (tile.Gid - 1) % Level.tileInX * Level.tileSpacing, tile.Gid / Level.tileInX * Level.tileHeight + tile.Gid / Level.tileInX * Level.tileSpacing, Level.tileWidth, Level.tileHeight);
                    spriteBatch.Draw(PlayingState.level.platformerTex, new Vector2(tile.X * 70, tile.Y * 70), rec, Color.White);
                }
            }

            /*Menu paused waiting for user input*/
            if (paused && saveAttack == null)
            {

                shadowText(spriteBatch, "->", new Vector2(Game1.instance.Width * .05f , Game1.instance.Height * .8f), statusSize * 1.5f);
                shadowText(spriteBatch, "->", new Vector2(Game1.instance.Width * .05f + Game1.instance.Width * .30f*heroTurn, Game1.instance.Height * .025f), statusSize);

                if (KeyboardInput.inputstate.IsKeyPressed(Keys.Enter, null, out KeyboardInput.controlIndex))
                {
                    AddAttack();
                    paused = false;
                    heroTurn += 1;
                    if (heroTurn == 3)
                        heroTurn = 0;
                }
            }

            shadowText(spriteBatch, "Attack", new Vector2(Game1.instance.Width * .08f, Game1.instance.Height * .8f), statusSize*1.5f);
            shadowText(spriteBatch, "Item", new Vector2(Game1.instance.Width * .08f, Game1.instance.Height * .85f), statusSize*1.5f);
            shadowText(spriteBatch, "Wait", new Vector2(Game1.instance.Width * .08f, Game1.instance.Height * .90f), statusSize*1.5f);


            /* Names at the top*/
            shadowText(spriteBatch, HeroParty.theHero[0].name, new Vector2(Game1.instance.Width * .08f, Game1.instance.Height * .025f), statusSize);
            shadowText(spriteBatch, HeroParty.theHero[1].name, new Vector2(Game1.instance.Width * .38f, Game1.instance.Height * .025f), statusSize);
            shadowText(spriteBatch, HeroParty.theHero[2].name, new Vector2(Game1.instance.Width * .68f, Game1.instance.Height * .025f), statusSize);

            /*Hero One Health Bar*/
            //[Solid health bars]
            spriteBatch.Draw(Menu.pixel, new Vector2(Game1.instance.Width * .08f, Game1.instance.Height * .10f), new Rectangle(0, 0, (int)(HeroParty.theHero[0].health * Game1.instance.Width * 0.16f + 1), 25), Color.Green);
            spriteBatch.Draw(Menu.pixel, new Vector2(Game1.instance.Width * .08f + Game1.instance.Width * 0.16f * HeroParty.theHero[0].health, Game1.instance.Height * .10f), new Rectangle(0, 0, (int)(Game1.instance.Width * 0.16f * (1.0f - HeroParty.theHero[0].health)), 25), Color.Red);

            //[Outline of health bars]
            spriteBatch.Draw(Menu.pixel, new Vector2(Game1.instance.Width * .08f, Game1.instance.Height * .10f), new Rectangle(0, 0, (int)(Game1.instance.Width * 0.16f), 2), Color.Black); //top
            spriteBatch.Draw(Menu.pixel, new Vector2(Game1.instance.Width * .08f, Game1.instance.Height * .10f), new Rectangle(0, 0, 2, 25), Color.Black); //left
            spriteBatch.Draw(Menu.pixel, new Vector2(Game1.instance.Width * .08f + Game1.instance.Width * 0.16f - 2, Game1.instance.Height * .10f), new Rectangle(0, 0, 2, 25), Color.Black); //right
            spriteBatch.Draw(Menu.pixel, new Vector2(Game1.instance.Width * .08f, Game1.instance.Height * .10f + 25 - 2), new Rectangle(0, 0, (int)(Game1.instance.Width * 0.16f), 2), Color.Black); //bottom

            /*Hero Two Health Bar*/
            //[Solid health bars]
            spriteBatch.Draw(Menu.pixel, new Vector2(Game1.instance.Width * .38f, Game1.instance.Height * .10f), new Rectangle(0, 0, (int)(HeroParty.theHero[0].health * Game1.instance.Width * 0.16f + 1), 25), Color.Green);
            spriteBatch.Draw(Menu.pixel, new Vector2(Game1.instance.Width * .38f + Game1.instance.Width * 0.16f * HeroParty.theHero[0].health, Game1.instance.Height * .10f), new Rectangle(0, 0, (int)(Game1.instance.Width * 0.16f * (1.0f - HeroParty.theHero[0].health)), 25), Color.Red);

            //[Outline of health bars]
            spriteBatch.Draw(Menu.pixel, new Vector2(Game1.instance.Width * .38f, Game1.instance.Height * .10f), new Rectangle(0, 0, (int)(Game1.instance.Width * 0.16f), 2), Color.Black); //top
            spriteBatch.Draw(Menu.pixel, new Vector2(Game1.instance.Width * .38f, Game1.instance.Height * .10f), new Rectangle(0, 0, 2, 25), Color.Black); //left
            spriteBatch.Draw(Menu.pixel, new Vector2(Game1.instance.Width * .38f + Game1.instance.Width * 0.16f - 2, Game1.instance.Height * .10f), new Rectangle(0, 0, 2, 25), Color.Black); //right
            spriteBatch.Draw(Menu.pixel, new Vector2(Game1.instance.Width * .38f, Game1.instance.Height * .10f + 25 - 2), new Rectangle(0, 0, (int)(Game1.instance.Width * 0.16f), 2), Color.Black); //bottom

            /*Hero Three Health Bar*/
            //[Solid health bars]
            spriteBatch.Draw(Menu.pixel, new Vector2(Game1.instance.Width * .68f, Game1.instance.Height * .10f), new Rectangle(0, 0, (int)(HeroParty.theHero[0].health * Game1.instance.Width * 0.16f + 1), 25), Color.Green);
            spriteBatch.Draw(Menu.pixel, new Vector2(Game1.instance.Width * .68f + Game1.instance.Width * 0.16f * HeroParty.theHero[0].health, Game1.instance.Height * .10f), new Rectangle(0, 0, (int)(Game1.instance.Width * 0.16f * (1.0f - HeroParty.theHero[0].health)), 25), Color.Red);

            //[Outline of health bars]
            spriteBatch.Draw(Menu.pixel, new Vector2(Game1.instance.Width * .68f, Game1.instance.Height * .10f), new Rectangle(0, 0, (int)(Game1.instance.Width * 0.16f), 2), Color.Black); //top
            spriteBatch.Draw(Menu.pixel, new Vector2(Game1.instance.Width * .68f, Game1.instance.Height * .10f), new Rectangle(0, 0, 2, 25), Color.Black); //left
            spriteBatch.Draw(Menu.pixel, new Vector2(Game1.instance.Width * .68f + Game1.instance.Width * 0.16f - 2, Game1.instance.Height * .10f), new Rectangle(0, 0, 2, 25), Color.Black); //right
            spriteBatch.Draw(Menu.pixel, new Vector2(Game1.instance.Width * .68f, Game1.instance.Height * .10f + 25 - 2), new Rectangle(0, 0, (int)(Game1.instance.Width * 0.16f), 2), Color.Black); //bottom

            spriteBatch.Draw(PlayingState.instance.npcSprite, HeroParty.theHero[0].position, HeroParty.theHero[0].rect, Color.White, 0.0f, Vector2.Zero, HeroParty.theHero[0].scale, SpriteEffects.None, 0.0f);
            spriteBatch.Draw(PlayingState.instance.npcSprite, HeroParty.theEnemy[0].position, HeroParty.theEnemy[0].rect, Color.White, 0.0f, Vector2.Zero, HeroParty.theEnemy[0].scale, SpriteEffects.None, 0.0f);

            /*Draw battle bar*/
            spriteBatch.Draw(Menu.pixel, new Vector2(Game1.instance.Width * .9f, Game1.instance.Height * .10f), new Rectangle(0, 0, (int)(Game1.instance.Width * 0.025f), (int)(Game1.instance.Height * 0.8f)), Color.White); //top

            for (int i = 0; i < battleQueue.Count; i++)
            {
                int[] attack = battleQueue[i];
                if (attack[0] == 0)
                {
                   shadowText(spriteBatch, "Melee:" + i, new Vector2(Game1.instance.Width * .9f, Game1.instance.Height * .90f - (float)attack[1] / 5000f * Game1.instance.Height * .80f), statusSize * 3.0f);
                }
            }

            /*Draw the Hero Sprites*/

            for (int i = 0; i < 3; i++)
            {
                spriteBatch.Draw(PlayingState.instance.npcSprite, HeroParty.theHero[i].position, HeroParty.theHero[i].rect, Color.White, 0.0f, Vector2.Zero, HeroParty.theHero[i].scale, SpriteEffects.None, 0.0f);
            }

            /* Draw the Enemy Sprites*/

            for (int i = 0; i < 5; i++)
            {
                spriteBatch.Draw(PlayingState.instance.npcSprite, HeroParty.theEnemy[i].position, HeroParty.theEnemy[i].rect, Color.White, 0.0f, Vector2.Zero, HeroParty.theEnemy[i].scale, SpriteEffects.None, 0.0f);
                HeroParty.theEnemy[i].DrawHealthAboveCharacter();
            }

        }

        public static void AddAttack()
        {
            int[] attack = new int[4];
            attack[0] = 0; //which skill
            attack[1] = 1000; //time remaining
            attack[2] = heroTurn; //which npc doing the attack
            attack[3] = 1; //which enemy to hit
            battleQueue.Add(attack);
        }

        public void shadowText(SpriteBatch spriteBatch, String text, Vector2 position, Vector2 statusSize)
        {
            spriteBatch.DrawString(Menu.menuText, text, position + new Vector2(1, 1), Color.White, 0.0f, new Vector2(0, 0), new Vector2(Game1.instance.Width * 0.08f / statusSize.X, Game1.instance.Width * 0.08f / statusSize.X), SpriteEffects.None, 0.0f);
            spriteBatch.DrawString(Menu.menuText, text, position + new Vector2(1, -1), Color.White, 0.0f, new Vector2(0, 0), new Vector2(Game1.instance.Width * 0.08f / statusSize.X, Game1.instance.Width * 0.08f / statusSize.X), SpriteEffects.None, 0.0f);
            spriteBatch.DrawString(Menu.menuText, text, position + new Vector2(-1, 1), Color.White, 0.0f, new Vector2(0, 0), new Vector2(Game1.instance.Width * 0.08f / statusSize.X, Game1.instance.Width * 0.08f / statusSize.X), SpriteEffects.None, 0.0f);
            spriteBatch.DrawString(Menu.menuText, text, position + new Vector2(-1, -1), Color.White, 0.0f, new Vector2(0, 0), new Vector2(Game1.instance.Width * 0.08f / statusSize.X, Game1.instance.Width * 0.08f / statusSize.X), SpriteEffects.None, 0.0f);
            
            spriteBatch.DrawString(Menu.menuText, text, position, Color.Black, 0.0f, new Vector2(0, 0), new Vector2(Game1.instance.Width * 0.08f / statusSize.X, Game1.instance.Width * 0.08f / statusSize.X), SpriteEffects.None, 0.0f);
        }
    }
}
