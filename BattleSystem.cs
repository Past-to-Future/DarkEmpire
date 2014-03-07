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
        TmxMap battlemap;
        public static Texture2D pixel = new Texture2D(Game1.instance.GraphicsDevice, 1, 1); //create 1x1 pixel texture
        public static SpriteFont battleText; //placeholder if text is different than menu (menu currently used)
        SpriteBatch spriteBatch = Game1.instance.SpriteBatch;
        public bool activeBattle; //Displays the battlefield
        static List<int[]> battleQueue = new List<int[]>(); //loads attacks onto timer, if timer runs out they move to attackQueue
        static List<int[]> attackQueue = new List<int[]>(); //responsible to show the attack
        public Thread battleThread, attackThread; 
        String status = "Status"; //using the size of the word 'status' as scaling, weird but keeps stuff constant
        Vector2 statusSize;

        bool paused = true; //player needs to input an attack because a character is eligible to attack
        bool doBattlepause = false; //semophore to handle the two threads
        bool selectEnemy = false, selectItem = false; //to know when player wants to select specifically the enemy to attack
        bool[] npcAttackSet = new bool[3];
        static int heroTurn = 0; //which hero gets to attack now
        static int Enemyselection = 0; //who we gonna kill with our attacks
        static int battleMenuSelection = 0; //determines if we are using an attack, item, or waiting.

        static Random rand = new Random();

        /*Trying to reduce computations on the menu*/
        /*Probably won't make a difference.*/
        static float Width = Game1.instance.Width;
        static float Height = Game1.instance.Height;

        float pctH_08 = Height * .08f;
        float pctH_10 = Height * .10f;
        float pctW_05 = Width * .05f;
        float pctW_08 = Width * .08f;
        float pctW_10 = Width * .10f;
        float pctW_20 = Width * .20f;
        float pctW_90 = Width * .90f;

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

        float moving = 0f;

        private void DoAttack()
         {
             while (true)
             {
                 if (attackQueue.Count > 0 && doBattlepause)
                 {

                     int i = 0;
                     int[] saveAttack = attackQueue[i];

                     if (saveAttack[0] == 1)
                     {
                         Thread.Sleep(250);
                         attackQueue.Remove(attackQueue[i]);
                     }
                     else
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
                             HeroParty.theEnemy[saveAttack[3]].health -= 1.0f;

                             if (HeroParty.theEnemy[saveAttack[3]].health <= 0.0f)
                             {
                                 HeroParty.theEnemy[saveAttack[3]].health = 0.0f; //did he die
                             }

                             Thread.Sleep(1250);

                             if (saveAttack[2] == 0)
                                 HeroParty.theHero[saveAttack[2]].position = new Vector2(pctW_20, Height * .2f);
                             else if (saveAttack[2] == 1)
                                 HeroParty.theHero[saveAttack[2]].position = new Vector2(pctW_10, Height * .4f);
                             else
                                 HeroParty.theHero[saveAttack[2]].position = new Vector2(pctW_20, Height * .6f);
                             Thread.Sleep(250);
                             attackQueue.Remove(attackQueue[i]);
                         }
                     }
                 }
                 else
                 {
                     doBattlepause = false;
                 }
                 Thread.Sleep(25);
             }
         }

       
        private void DoBattle()
        {
            while (true)
            {
                if (paused || doBattlepause)
                {
                    //doing nothing! herp derp
                }
                else
                {
                    if (battleQueue.Count == 0)
                        paused = true;

                    for (int i = 0; i < battleQueue.Count; i++)
                    {
                        battleQueue[i][1] -= 25;
                        if (battleQueue[i][1] <= 0)
                        {
                            attackQueue.Add(battleQueue[i]);
                            battleQueue.Remove(battleQueue[i]);
                            doBattlepause = true;
                        }
                    }
                }
                Thread.Sleep(25);
            }
        }

        public void draw()
        {
            Game1.instance.GraphicsDevice.Clear(Color.White);
            foreach (TmxLayer layer in battlemap.Layers)
            {
                foreach (TmxLayerTile tile in layer.Tiles)
                {
                    Rectangle rec = new Rectangle((tile.Gid - 1) % Level.tileInX * Level.tileWidth + (tile.Gid - 1) % Level.tileInX * Level.tileSpacing, tile.Gid / Level.tileInX * Level.tileHeight + tile.Gid / Level.tileInX * Level.tileSpacing, Level.tileWidth, Level.tileHeight);
                    spriteBatch.Draw(PlayingState.level.platformerTex, new Vector2(tile.X * 70/2, tile.Y * 70/2), rec, Color.White, 0.0f, new Vector2(0,0), 0.5f, SpriteEffects.None, 0.0f);
                }
            }


            shadowText(spriteBatch, "->", new Vector2(pctW_05, Height * .8f + Height * .05f * battleMenuSelection), statusSize * 1.5f);
            shadowText(spriteBatch, "->", new Vector2(pctW_05 + Width * .30f * heroTurn, Height * .025f), statusSize);

            if (selectEnemy)
            {
                shadowText(spriteBatch, "->", HeroParty.theEnemy[Enemyselection].position - new Vector2(pctW_05, 0), statusSize * 1.5f);
            }

            if (selectItem)
            {
                shadowText(spriteBatch, "Oak: Red! This isn't the time to use that yet!", new Vector2(Width * .15f, Height * .8f + Height * .05f * battleMenuSelection), statusSize * 2.0f);
            }

            if (KeyboardInput.inputstate.IsKeyPressed(Keys.Enter, null, out KeyboardInput.controlIndex))
            {
                if (paused)
                {
                    if (selectEnemy)
                    {
                        AddAttack(0);
                        selectEnemy = false;
                    }
                    else if (battleMenuSelection == 0)
                        selectEnemy = true;
                    else if (battleMenuSelection == 1)
                        selectItem = !selectItem;
                    else if (battleMenuSelection == 2)
                        AddAttack(1);

                    Enemyselection = 0;
                }
            }

            else if (KeyboardInput.inputstate.IsKeyPressed(Keys.Down, null, out KeyboardInput.controlIndex))
            {
                if (paused && selectEnemy)
                {
                    Enemyselection = (Enemyselection + 1) % 5;
                }
                else if (paused && selectItem == false)
                {
                    battleMenuSelection = (battleMenuSelection + 1) % 3 ;
                }
            }

            else if (KeyboardInput.inputstate.IsKeyPressed(Keys.Up, null, out KeyboardInput.controlIndex))
            {
                if (paused && selectEnemy)
                {
                    Enemyselection = (Enemyselection - 1) % 5;
                    if (Enemyselection < 0)
                        Enemyselection = 0;
                }
                else if (paused && selectItem == false)
                {
                    battleMenuSelection = (battleMenuSelection - 1) % 3;
                    if (battleMenuSelection < 0)
                        battleMenuSelection = 0;
                }
            }


            int[] checkList;

            npcAttackSet[0] = npcAttackSet[1] = npcAttackSet[2] = false;

            for (int i = 0; i < battleQueue.Count; i++)
            {
                checkList = battleQueue[i];

                if (checkList[2] == 0)
                    npcAttackSet[0] = true;
                else if (checkList[2] == 1)
                    npcAttackSet[1] = true;
                else if (checkList[2] == 2)
                    npcAttackSet[2] = true;
            }

            if (!npcAttackSet[2])
            {
                heroTurn = 2;
                paused = true;
            }
            else if (!npcAttackSet[1])
            {
                heroTurn = 1;
                paused = true;
            }
            else if (!npcAttackSet[0])
            {
                heroTurn = 0;
                paused = true;
            }

            if (npcAttackSet[0] && npcAttackSet[1] && npcAttackSet[2])
                paused = false;


            shadowText(spriteBatch, "Attack", new Vector2(pctW_08, Height * .8f), statusSize * 1.5f);
            shadowText(spriteBatch, "Item", new Vector2(pctW_08, Height * .85f), statusSize * 1.5f);
            shadowText(spriteBatch, "Wait", new Vector2(pctW_08, Height * .90f), statusSize * 1.5f);

            for (int i = 0; i < 3; i++)
            {
                float HealthBarX = pctW_08 + Width * .3f * i; 
                //[Names at the Top]
                shadowText(spriteBatch, HeroParty.theHero[i].name, new Vector2(HealthBarX, Game1.instance.Height * .025f), statusSize);
                
                //[Solid health bars]
                spriteBatch.Draw(Menu.pixel, new Vector2(HealthBarX, pctH_10), new Rectangle(0, 0, (int)(HeroParty.theHero[i].HealthPercent() * Game1.instance.Width * 0.16f + 1), 25), Color.Green);
                spriteBatch.Draw(Menu.pixel, new Vector2(HealthBarX + Width * 0.16f * HeroParty.theHero[i].HealthPercent(), pctH_10), new Rectangle(0, 0, (int)(Width * 0.16f * (1.0f - HeroParty.theHero[i].HealthPercent() )), 25), Color.Red);

                //[Outline of health bars]
                spriteBatch.Draw(Menu.pixel, new Vector2(HealthBarX, pctH_10), new Rectangle(0, 0, (int)(Width * 0.16f), 2), Color.Black); //top
                spriteBatch.Draw(Menu.pixel, new Vector2(HealthBarX, pctH_10), new Rectangle(0, 0, 2, 25), Color.Black); //left
                spriteBatch.Draw(Menu.pixel, new Vector2(HealthBarX + Width * 0.16f - 2, pctH_10), new Rectangle(0, 0, 2, 25), Color.Black); //right
                spriteBatch.Draw(Menu.pixel, new Vector2(HealthBarX, pctH_10 + 25 - 2), new Rectangle(0, 0, (int)(Width * 0.16f), 2), Color.Black); //bottom

                //[Draw Hero Sprites]
                spriteBatch.Draw(PlayingState.instance.npcSprite, HeroParty.theHero[i].position, HeroParty.theHero[i].rect, Color.White, 0.0f, Vector2.Zero, HeroParty.theHero[i].scale, SpriteEffects.None, 0.0f);

            }

            //[Draw battle bar]
            spriteBatch.Draw(Menu.pixel, new Vector2(pctW_90, pctH_10), new Rectangle(0, 0, (int)(Width * 0.025f), (int)(Height*.80f)), Color.White); //top

            for (int i = 0; i < battleQueue.Count; i++)
            {
                int[] attack = battleQueue[i];
                if (attack[0] == 0)
                {
                    shadowText(spriteBatch, "Melee:" + i, new Vector2(pctW_90, Height * .90f - (float)attack[1] / 5000f * Game1.instance.Height * .80f), statusSize * 3.0f);
                }
                else if (attack[0] == 1)
                {
                    shadowText(spriteBatch, "Wait:" + i, new Vector2(pctW_90, Height * .90f - (float)attack[1] / 5000f * Game1.instance.Height * .80f), statusSize * 3.0f);
                }
            }

            //[Draw the Enemy Sprites]
            for (int i = 0; i < 5; i++)
            {
                if (HeroParty.theEnemy[i].health != 0.0f)
                {
                    spriteBatch.Draw(PlayingState.instance.npcSprite, HeroParty.theEnemy[i].position, HeroParty.theEnemy[i].rect, Color.White, 0.0f, Vector2.Zero, HeroParty.theEnemy[i].scale, SpriteEffects.None, 0.0f);
                    HeroParty.theEnemy[i].DrawHealthAboveCharacter();
                }
            }

        }

        public static void AddAttack(int attackNumber)
        {
            int[] attack = new int[4];
            attack[0] = attackNumber; //which skill
            attack[1] = rand.Next(5000); //time remaining
            attack[2] = heroTurn; //which npc doing the attack
            attack[3] = Enemyselection; //which enemy to hit
            battleQueue.Add(attack);
        }

        public void shadowText(SpriteBatch spriteBatch, String text, Vector2 position, Vector2 statusSize)
        {
            spriteBatch.DrawString(Menu.menuText, text, position + new Vector2(1, 1), Color.White, 0.0f, new Vector2(0, 0), new Vector2(pctW_08 / statusSize.X, pctW_08 / statusSize.X), SpriteEffects.None, 0.0f);
            spriteBatch.DrawString(Menu.menuText, text, position + new Vector2(1, -1), Color.White, 0.0f, new Vector2(0, 0), new Vector2(pctW_08 / statusSize.X, pctW_08 / statusSize.X), SpriteEffects.None, 0.0f);
            spriteBatch.DrawString(Menu.menuText, text, position + new Vector2(-1, 1), Color.White, 0.0f, new Vector2(0, 0), new Vector2(pctW_08 / statusSize.X, pctW_08 / statusSize.X), SpriteEffects.None, 0.0f);
            spriteBatch.DrawString(Menu.menuText, text, position + new Vector2(-1, -1), Color.White, 0.0f, new Vector2(0, 0), new Vector2(pctW_08 / statusSize.X, pctW_08 / statusSize.X), SpriteEffects.None, 0.0f);

            spriteBatch.DrawString(Menu.menuText, text, position, Color.Black, 0.0f, new Vector2(0, 0), new Vector2(pctW_08 / statusSize.X, pctW_08 / statusSize.X), SpriteEffects.None, 0.0f);
        }
    }
}
