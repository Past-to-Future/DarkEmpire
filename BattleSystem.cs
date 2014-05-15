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
    { //
        TmxMap battlemap;
        public static Texture2D pixel = new Texture2D(Game1.instance.GraphicsDevice, 1, 1); //create 1x1 pixel texture
        Texture2D bigGear, smallGear;
        public static SpriteFont battleText; //placeholder if text is different than menu (menu currently used)
        SpriteBatch spriteBatch = Game1.instance.SpriteBatch;
        public bool activeBattle; //Displays the battlefield
        static List<int[]> battleQueue = new List<int[]>(); //loads attacks onto timer, if timer runs out they move to attackQueue
        static int[] setAttack;
        public Thread battleThread, attackThread;
        String status = "Status"; //using the size of the word 'status' as scaling, weird but keeps stuff constant
        Vector2 statusSize;
        public Texture2D background;
        bool paused = true; //player needs to input an attack because a character is eligible to attack
        bool doBattlepause = false; //semophore to handle the two threads
        bool selectEnemy = false, selectItem = false; //to know when player wants to select specifically the enemy to attack
        bool[] npcAttackSet = new bool[3];
        static int heroTurn = 0; //which hero gets to attack now
        static int Enemyselection = 0; //who we gonna kill with our attacks
        static int battleMenuSelection = 0; //determines if we are using an attack, item, or waiting.

        Texture2D[] characterPortrait = new Texture2D[8];
        
        static Random rand = new Random();

        /*Trying to reduce computations on the menu*/
        /*Probably won't make a difference.*/
        static float Width = Game1.instance.Width;
        static float Height = Game1.instance.Height;

        float pctH_08 = Height * .08f;
        float pctH_09 = Height * .09f;
        float pctH_10 = Height * .10f;
        float pctW_05 = Width * .05f;
        float pctW_06 = Width * .06f;
        float pctW_08 = Width * .08f;
        float pctW_09 = Width * .09f;
        float pctW_10 = Width * .10f;
        float pctW_20 = Width * .20f;
        float pctW_90 = Width * .90f;

        int width;
        int height;
        Texture2D texture;

        public BattleSystem()
        {
            activeBattle = false;
        }

        public void initialize()
        {
            characterPortrait[0] = Game1.instance.Content.Load<Texture2D>("CharacterPortrait/Maxum");
            characterPortrait[1] = Game1.instance.Content.Load<Texture2D>("CharacterPortrait/Aurelia");
            characterPortrait[2] = Game1.instance.Content.Load<Texture2D>("CharacterPortrait/Jasmine");
            characterPortrait[3] = Game1.instance.Content.Load<Texture2D>("CharacterPortrait/Vlad");

            background = Game1.instance.Content.Load<Texture2D>("battle_background");
            pixel.SetData(new[] { Color.White }); //make it white so we can color it
            battlemap = new TmxMap("Content\\MapData/battleMap.tmx");
            battleText = Game1.instance.Content.Load<SpriteFont>("BattleSystemFont"); //cannot edit in mono, import the .spritefont included into a dummy xna project and edit, bring .xnb back over
            battleThread = new Thread(new ThreadStart(DoBattle));
            attackThread = new Thread(new ThreadStart(DoAttack));
            battleThread.Start();
            attackThread.Start();
            statusSize = battleText.MeasureString(status);

            bigGear = Game1.instance.Content.Load<Texture2D>("bigGear");
            smallGear = Game1.instance.Content.Load<Texture2D>("smallGear");

            width = (int)(Width * 0.65f);
            height = (int)(Height * .20f);

            texture = new Texture2D(Game1.instance.GraphicsDevice, width, height, false, SurfaceFormat.Color);
            Color[] color = new Color[width * height];
            List<Color> backgroundColors = new List<Color>();
            List<Color> borderColors = new List<Color>();
            borderColors.Add(Color.White);
            backgroundColors.Add(Color.SkyBlue);

            int borderThickness = 5;
            int borderShadow = 1;
            int borderRadius = 12;
            int initialShadowIntensity = 10;
            int finalShadowIntensity = 15;
            for (int x = 0; x < texture.Width; x++)
            {
                for (int y = 0; y < texture.Height; y++)
                {
                    switch (backgroundColors.Count)
                    {
                        case 4:
                            Color leftColor0 = Color.Lerp(backgroundColors[0], backgroundColors[1], ((float)y / (width - 1)));
                            Color rightColor0 = Color.Lerp(backgroundColors[2], backgroundColors[3], ((float)y / (height - 1)));
                            color[x + width * y] = Color.Lerp(leftColor0, rightColor0, ((float)x / (width - 1)));
                            break;
                        case 3:
                            Color leftColor1 = Color.Lerp(backgroundColors[0], backgroundColors[1], ((float)y / (width - 1)));
                            Color rightColor1 = Color.Lerp(backgroundColors[1], backgroundColors[2], ((float)y / (height - 1)));
                            color[x + width * y] = Color.Lerp(leftColor1, rightColor1, ((float)x / (width - 1)));
                            break;
                        case 2:
                            color[x + width * y] = Color.Lerp(backgroundColors[0], backgroundColors[1], ((float)x / (width - 1)));
                            break;
                        default:
                            color[x + width * y] = backgroundColors[0];
                            break;
                    }
                    color[x + width * y] = ColorBorder(x, y, width, height, borderThickness, borderRadius, borderShadow, color[x + width * y], borderColors, initialShadowIntensity, finalShadowIntensity);
                }
            }

            texture.SetData<Color>(color);

        }

        public void TriplePunch()
        {
            KeyboardInput.hitInstance.Stop();
            KeyboardInput.hitInstance.Play();
            Thread.Sleep(200);
            KeyboardInput.hitInstance.Stop();
            KeyboardInput.hitInstance.Play();
            Thread.Sleep(200);
            KeyboardInput.hitInstance.Stop();
            KeyboardInput.hitInstance.Play();
            HeroParty.theEnemy[setAttack[3]].stat.health -= 1.0f;
            return;
        }

        public void Waiting()
        {
            Thread.Sleep(250);
            setAttack = null;
        }

        public void PlayerMoving(float dist)
        {
            float moving = 25f * 0.16f / dist;
            HeroParty.theHero[setAttack[2]].battleFrame = (HeroParty.theHero[setAttack[2]].battleFrame + 1) % 2;
            HeroParty.theHero[setAttack[2]].changeBattleStance();
            HeroParty.theHero[setAttack[2]].position = Vector2.Lerp(HeroParty.theHero[setAttack[2]].position, HeroParty.theEnemy[setAttack[3]].position, moving);
            return;
        }

        public void CheckDeath()
        {
            if (HeroParty.theEnemy[setAttack[3]].stat.health <= 0.0f)
            {
                HeroParty.theEnemy[setAttack[3]].stat.health = 0.0f; //did he die
            }
        }

        public void ResetSpritePosition()
        {
            if (setAttack[2] == 0)
                HeroParty.theHero[setAttack[2]].position = new Vector2(pctW_20, Height * .2f);
            else if (setAttack[2] == 1)
                HeroParty.theHero[setAttack[2]].position = new Vector2(pctW_10, Height * .4f);
            else
                HeroParty.theHero[setAttack[2]].position = new Vector2(pctW_20, Height * .6f);
            HeroParty.theHero[setAttack[2]].battleFrame = 0;
            HeroParty.theHero[setAttack[2]].changeBattleStance();
        }

        private void DoAttack()
        {
            while (true)
            {
                if (setAttack != null && doBattlepause)
                {

                    if (setAttack[0] == 1)
                    {
                        Waiting();
                    }
                    else
                    {
                        float dist = Vector2.Distance(HeroParty.theHero[setAttack[2]].position, HeroParty.theEnemy[setAttack[3]].position);
                        if (dist > 64)
                        {
                            PlayerMoving(dist); 
                        }
                        else
                        {
                            TriplePunch();
                            CheckDeath();
                            Thread.Sleep(1250);
                            ResetSpritePosition();
                            Thread.Sleep(250);
                            setAttack = null;
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
                            setAttack = battleQueue[i];
                            battleQueue.Remove(battleQueue[i]);
                            doBattlepause = true;
                        }
                    }
                }
                Thread.Sleep(25);
            }
        }

        static float rotation_1 = 0.0f;
        static float rotation_2 = 0.0f;
        static float rotation_3 = 0.0f;
        static float rotation_4 = 0.0f;
        static bool spin = false;
        static bool spin_1 = false;
        static int turn = 0;
        static int turn_1 = 0;

        public void draw()
        {
            Game1.instance.GraphicsDevice.Clear(Color.White);
            Rectangle screenRect = new Rectangle(0, 0, Game1.instance.GraphicsDevice.Viewport.Width, Game1.instance.GraphicsDevice.Viewport.Height);
            spriteBatch.Draw(background, screenRect, Color.White);
            spriteBatch.Draw(texture, new Vector2(Width * 0.05f, Height * .78f), new Rectangle(0, 0, (int)(Width * 0.65f), (int)(Height * .20f)), Color.White);

            if (selectEnemy)
            {
                shadowText(spriteBatch, "->", HeroParty.theEnemy[Enemyselection].position - new Vector2(pctW_05, 0), statusSize * 1.5f);
            }

            if (selectItem)
            {
                for (int i = 0; i < HeroParty.itemList.Count; i++)
                {
                    if (i < 5)
                    {
                        shadowText(spriteBatch, HeroParty.itemList[i].Name, new Vector2(Width * .09f + i / 3 * Width * 0.21f, Height * .8f + (i % 3) * Height * 0.05f), statusSize * 2.0f);
                        shadowText(spriteBatch, HeroParty.itemList[i].Quantity.ToString(), new Vector2(Width * .22f + i / 3 * Width * 0.21f, Height * .8f + (i % 3) * Height * 0.05f), statusSize * 2.0f);
                    }
                    else if(i == 5)
                        shadowText(spriteBatch, "More", new Vector2(Width * .09f + i / 3 * Width * 0.21f, Height * .8f + (i % 3) * Height * 0.05f), statusSize * 2.0f);
                }
                shadowText(spriteBatch, HeroParty.itemList[0].description, new Vector2(Width * .09f + 6 / 3 * Width * 0.21f, Height * .85f + (6 % 3) * Height * 0.05f), statusSize * 1.5f);

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
                    else if (battleMenuSelection == 4)
                        selectItem = !selectItem;
                    else if (battleMenuSelection == 1)
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
                    battleMenuSelection = (battleMenuSelection + 1) % 6;
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
                    battleMenuSelection = (battleMenuSelection - 1) % 6;
                    if (battleMenuSelection < 0)
                        battleMenuSelection = 0;
                }
            }

            else if (KeyboardInput.inputstate.IsKeyPressed(Keys.Right, null, out KeyboardInput.controlIndex))
            {
                if (paused && selectItem == false)
                {
                    battleMenuSelection = (battleMenuSelection - 1) % 6;
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

            if (!npcAttackSet[0])
            {
                heroTurn = 0;
                paused = true;
            }
            else if (!npcAttackSet[1])
            {
                heroTurn = 1;
                paused = true;
            }
            else if (!npcAttackSet[2])
            {
                heroTurn = 2;
                paused = true;
            }

            if (npcAttackSet[0] && npcAttackSet[1] && npcAttackSet[2])
                paused = false;

            if (!selectItem)
            {
                shadowText(spriteBatch, "Attack", new Vector2(pctW_09, Height * .8f), statusSize * 1.0f);
                shadowText(spriteBatch, "Wait", new Vector2(pctW_09, Height * .90f), statusSize * 1.0f);
                shadowText(spriteBatch, "Magic", new Vector2(2.5f * pctW_08 + pctW_09, Height * .8f), statusSize * 1.0f);
                shadowText(spriteBatch, "Skill", new Vector2(2.5f * pctW_08 + pctW_09, Height * .9f), statusSize * 1.0f);
                shadowText(spriteBatch, "Item", new Vector2(5.0f * pctW_08 + pctW_09, Height * .8f), statusSize * 1.0f);
                shadowText(spriteBatch, "Wait Again", new Vector2(6.0f * pctW_08, Height * .9f), statusSize * 1.0f);
            }
            if(battleMenuSelection % 2 == 0)
                shadowText(spriteBatch, "->", new Vector2(pctW_08 * (battleMenuSelection/2) * 2.5f + pctW_06, Height * .8f), statusSize * 1.5f);
            else
                shadowText(spriteBatch, "->", new Vector2(pctW_08 * (battleMenuSelection / 2) * 2.5f + pctW_06, Height * .9f), statusSize * 1.5f);

            for (int i = 0; i < 3; i++)
            {
                float HealthBarX = pctW_08 + Width * .27f * i;
                if(i==heroTurn)
                    spriteBatch.Draw(characterPortrait[i], new Vector2(HealthBarX - 70f,Game1.instance.Height * .025f), null , new Color(0,1,0,0.90f), 0.0f, Vector2.Zero, HeroParty.theHero[i].scale, SpriteEffects.None, 0.0f);
                else 
                    spriteBatch.Draw(characterPortrait[i], new Vector2(HealthBarX - 70f, Game1.instance.Height * .025f), null, Color.White, 0.0f, Vector2.Zero, HeroParty.theHero[i].scale, SpriteEffects.None, 0.0f);
                //[Names at the Top]
                shadowText(spriteBatch, HeroParty.theHero[i].name, new Vector2(HealthBarX, Game1.instance.Height * .025f), statusSize);

                //[Solid health bars]
                spriteBatch.Draw(Menu.pixel, new Vector2(HealthBarX, pctH_10), new Rectangle(0, 0, (int)(HeroParty.theHero[i].stat.HealthPercent() * Game1.instance.Width * 0.16f), 25), Color.Green);
                spriteBatch.Draw(Menu.pixel, new Vector2(HealthBarX + Width * 0.16f * HeroParty.theHero[i].stat.HealthPercent(), pctH_10), new Rectangle(0, 0, (int)(Width * 0.16f * (1.0f - HeroParty.theHero[i].stat.HealthPercent())), 25), Color.Red);

                //[Outline of health bars]
                spriteBatch.Draw(Menu.pixel, new Vector2(HealthBarX, pctH_10), new Rectangle(0, 0, (int)(Width * 0.16f), 2), Color.Black); //top
                spriteBatch.Draw(Menu.pixel, new Vector2(HealthBarX, pctH_10), new Rectangle(0, 0, 2, 25), Color.Black); //left
                spriteBatch.Draw(Menu.pixel, new Vector2(HealthBarX + Width * 0.16f - 2, pctH_10), new Rectangle(0, 0, 2, 25), Color.Black); //right
                spriteBatch.Draw(Menu.pixel, new Vector2(HealthBarX, pctH_10 + 25 - 2), new Rectangle(0, 0, (int)(Width * 0.16f), 2), Color.Black); //bottom

                //[Draw Hero Sprites]
                spriteBatch.Draw(PlayingState.instance.battleSprite, HeroParty.theHero[i].position, HeroParty.theHero[i].battleRect, Color.White, 0.0f, Vector2.Zero, HeroParty.theHero[i].scale, SpriteEffects.FlipHorizontally, 0.0f);

            }

            //[Draw battle bar]
            spriteBatch.Draw(Menu.pixel, new Vector2(Width * .01f, pctH_10*2), new Rectangle(0, 0, (int)(Width * 0.025f), (int)(Height * .70f)), new Color(0,0,0,0.24f));//Color.White); //top

            for (int i = 0; i < battleQueue.Count; i++)
            {
                int[] attack = battleQueue[i];
                if (attack[0] == 0)
                {
                    shadowText(spriteBatch, "Melee:" + i, new Vector2(Width * .01f, Height * .90f - (float)attack[1] / 5000f * Game1.instance.Height * .70f), statusSize * 3.0f);
                }
                else if (attack[0] == 1)
                {
                    shadowText(spriteBatch, "Wait:" + i, new Vector2(Width * .01f, Height * .90f - (float)attack[1] / 5000f * Game1.instance.Height * .70f), statusSize * 3.0f);
                }
            }

            //[Draw the Enemy Sprites]
            for (int i = 0; i < 5; i++)
            {
                if (HeroParty.theEnemy[i].stat.health != 0.0f)
                {
                    spriteBatch.Draw(PlayingState.instance.npcSprite, HeroParty.theEnemy[i].position, HeroParty.theEnemy[i].rect, Color.White, 0.0f, Vector2.Zero, HeroParty.theEnemy[i].scale, SpriteEffects.None, 0.0f);
                    HeroParty.theEnemy[i].DrawHealthAboveCharacter();
                }
            }

            //[Spin the gear manually] 
            if (KeyboardInput.inputstate.IsKeyPressed(Keys.Q, null, out KeyboardInput.controlIndex))
            {
                spin = !spin;
                spin_1 = !spin_1;
            }

            if (rotationRemaining > 0)
            {
                if (rotationRemaining > 7)
                    sDir = 1;
                else
                    sDir = -1;
                rotateWheel();
                if (!spin)
                {
                    spin = true;
                    spin_1 = true;
                    rotationRemaining -= 1;
                }
            }
            else
            {
                spin = false;
                spin_1 = false;
                rotation_1 = 0;
                rotation_2 = 0;
                rotation_3 = 0;
                rotation_4 = 0;
                turn = 0;
                turn_1 = 0;
            }
            spriteBatch.Draw(smallGear, new Vector2(Width, (Height / 2) + 310), null, Color.White, rotation_4, new Vector2(smallGear.Width / 2, smallGear.Height / 2), 1.0f, SpriteEffects.None, 0.0f);
            spriteBatch.Draw(bigGear, new Vector2(Width, (Height / 2) - 4), null, Color.White, rotation_2, new Vector2(bigGear.Width / 2, bigGear.Height / 2), 1.0f, SpriteEffects.None, 0.0f);

            for (int i = 0; i < battleQueue.Count; i++)
            {
                float xposition = 0;
                float yposition = -bigGear.Height /2 + 32;
                float rotationOrigin = (float)Math.PI * (i - 6) / 8f + (rotation_2);
                float xrotation = xposition * (float)Math.Cos(-rotationOrigin) + yposition * (float)Math.Sin(-rotationOrigin);
                float yrotation = (float)-Math.Sin(-rotationOrigin) * xposition + yposition * (float)Math.Cos(-rotationOrigin);

                spriteBatch.Draw(characterPortrait[battleQueue[i][2]], new Vector2(Width + xrotation, Height / 2 + yrotation - 4), null, Color.White, rotationOrigin, new Vector2(characterPortrait[0].Width / 2, characterPortrait[0].Height / 2), 0.5f, SpriteEffects.None, 0.0f);
            }            
        }

        static int rotationRemaining = 0;
        static int sDir = 1;

        public static void AddAttack(int attackNumber)
        {
            int[] attack = new int[4];
            attack[0] = attackNumber; //which skill
            attack[1] = rand.Next(5000); //time remaining
            attack[2] = heroTurn; //which npc doing the attack
            attack[3] = Enemyselection; //which enemy to hit
            battleQueue.Add(attack);

            for (int i = 0; i < battleQueue.Count-1; i++)
            {
                int[] dummy = new int[4];
                dummy = battleQueue[i];
                if (dummy[1] > battleQueue[i + 1][1])
                {
                    battleQueue[i] = battleQueue[i + 1];
                    battleQueue[i + 1] = dummy;
                }
            }
            rotationRemaining = 14;
            spin = !spin;
            spin_1 = !spin_1;
        }

        public void shadowText(SpriteBatch spriteBatch, String text, Vector2 position, Vector2 statusSize)
        {
            spriteBatch.DrawString(Menu.menuText, text, position + new Vector2(1, 1), Color.White, 0.0f, new Vector2(0, 0), new Vector2(pctW_08 / statusSize.X, pctW_08 / statusSize.X), SpriteEffects.None, 0.0f);
            spriteBatch.DrawString(Menu.menuText, text, position + new Vector2(1, -1), Color.White, 0.0f, new Vector2(0, 0), new Vector2(pctW_08 / statusSize.X, pctW_08 / statusSize.X), SpriteEffects.None, 0.0f);
            spriteBatch.DrawString(Menu.menuText, text, position + new Vector2(-1, 1), Color.White, 0.0f, new Vector2(0, 0), new Vector2(pctW_08 / statusSize.X, pctW_08 / statusSize.X), SpriteEffects.None, 0.0f);
            spriteBatch.DrawString(Menu.menuText, text, position + new Vector2(-1, -1), Color.White, 0.0f, new Vector2(0, 0), new Vector2(pctW_08 / statusSize.X, pctW_08 / statusSize.X), SpriteEffects.None, 0.0f);

            spriteBatch.DrawString(Menu.menuText, text, position, Color.Black, 0.0f, new Vector2(0, 0), new Vector2(pctW_08 / statusSize.X, pctW_08 / statusSize.X), SpriteEffects.None, 0.0f);
        }


        public static void rotateWheel()
        {
            if (spin == true)
            {
                //rotation_3 = rotation_1.ToString() + "\n" + (rotation_2 + 0.5f).ToString() + "\n";
                if (turn == 0)
                {
                    if (rotation_1 >= 0.362f)
                    {
                        rotation_1 = 0.0f;
                        // rotation_3 += rotation_2.ToString() + "\n";
                        turn = turn + 1;
                        spin = false;
                    }
                }

                if (turn == 1)
                {
                    if (rotation_1 >= 0.362f)
                    {
                        rotation_1 = 0.0f;
                        // rotation_3 += rotation_2.ToString() + "\n";
                        turn = turn + 1;
                        spin = false;
                    }
                }

                if (turn == 2)
                {
                    if (rotation_1 >= 0.382f)
                    {
                        rotation_1 = 0.0f;
                        //rotation_3 += rotation_2.ToString() + "\n";
                        turn = turn + 1;
                        spin = false;
                    }
                }

                if (turn == 3)
                {
                    if (rotation_1 >= 0.382f)
                    {
                        rotation_1 = 0.0f;
                        //rotation_3 += rotation_2.ToString() + "\n";
                        turn = turn + 1;
                        spin = false;
                    }
                }

                if (turn == 4)
                {
                    if (rotation_1 >= 0.378f)
                    {
                        rotation_1 = 0.0f;
                        //rotation_3 += rotation_2.ToString() + "\n";
                        turn = turn + 1;
                        spin = false;
                    }
                }

                if (turn == 5)
                {
                    if (rotation_1 >= 0.382f)
                    {
                        rotation_1 = 0.0f;
                        //rotation_3 += rotation_2.ToString() + "\n";
                        turn = turn + 1;
                        spin = false;
                    }
                }

                if (turn == 6)
                {
                    if (rotation_1 >= 0.38f)
                    {
                        rotation_1 = 0.0f;
                        // rotation_3 += rotation_2.ToString() + "\n";
                        turn = turn + 1;
                        spin = false;
                    }
                }

                if (turn == 7)
                {
                    if (rotation_1 >= 0.4f)
                    {
                        rotation_1 = 0.0f;
                        // rotation_3 += rotation_2.ToString() + "\n";
                        turn = turn + 1;
                        spin = false;
                    }
                }

                if (turn == 8)
                {
                    if (rotation_1 >= 0.38f)
                    {
                        rotation_1 = 0.0f;
                        //rotation_3 += rotation_2.ToString() + "\n";
                        turn = turn + 1;
                        spin = false;
                    }
                }

                if (turn == 9)
                {
                    if (rotation_1 >= 0.4f)
                    {
                        rotation_1 = 0.0f;
                        //rotation_3 += rotation_2.ToString() + "\n";
                        turn = turn + 1;
                        spin = false;
                    }
                }

                if (turn == 10)
                {
                    if (rotation_1 >= 0.4f)
                    {
                        rotation_1 = 0.0f;
                        // rotation_3 += rotation_2.ToString() + "\n";
                        turn = turn + 1;
                        spin = false;
                    }
                }

                if (turn == 11)
                {
                    if (rotation_1 >= 0.4f)
                    {
                        rotation_1 = 0.0f;
                        //rotation_3 += rotation_2.ToString() + "\n";
                        turn = turn + 1;
                        spin = false;
                    }
                }

                if (turn == 12)
                {
                    if (rotation_1 >= 0.38f)
                    {
                        rotation_1 = 0.0f;
                        // rotation_3 += rotation_2.ToString() + "\n";
                        turn = turn + 1;
                        spin = false;
                    }
                }

                if (turn == 13)
                {
                    if (rotation_1 >= 0.4f)
                    {
                        rotation_1 = 0.0f;
                        // rotation_3 += rotation_2.ToString() + "\n";
                        turn = turn + 1;
                        spin = false;
                    }
                }

                if (turn == 14)
                {
                    if (rotation_1 >= 0.38f)
                    {
                        rotation_1 = 0.0f;
                        // rotation_3 += rotation_2.ToString() + "\n";
                        turn = turn + 1;
                        spin = false;
                    }
                }

                if (turn == 15)
                {
                    if (rotation_1 >= 0.4f)
                    {
                        rotation_1 = 0.0f;
                        //rotation_3 += rotation_2.ToString() + "\n";
                        turn = 0;
                        rotation_2 = 0.0f;
                        spin = false;
                    }
                }
                rotation_1 = rotation_1 + 0.02f;
                rotation_2 = rotation_2 + sDir * 0.02f;
                rotation_4 = rotation_4 - sDir * (16*.02f/6f);
            }

            /*
            if (spin_1 == true)
            {
                //rotation_3 = rotation_1.ToString() + "\n" + (rotation_2 + 0.5f).ToString() + "\n";
                if (turn_1 == 0)
                {
                    if (rotation_3 >= 0.362f)
                    {
                        rotation_3 = 0.0f;
                        // rotation_3 += rotation_2.ToString() + "\n";
                        turn_1 = turn_1 + 1;
                        spin_1 = false;
                    }
                    else
                        rotation_4 = rotation_4 + 0.054f;
                }

                if (turn_1 == 1)
                {
                    if (rotation_3 >= 0.362f)
                    {
                        rotation_3 = 0.0f;
                        // rotation_3 += rotation_2.ToString() + "\n";
                        turn_1 = turn_1 + 1;
                        spin_1 = false;
                    }
                    else
                        rotation_4 = rotation_4 + 0.052f;
                }

                if (turn_1 == 2)
                {
                    if (rotation_3 >= 0.382f)
                    {
                        rotation_3 = 0.0f;
                        //rotation_3 += rotation_2.ToString() + "\n";
                        turn_1 = turn_1 + 1;
                        spin_1 = false;
                    }
                    else
                        rotation_4 = rotation_4 + 0.052f;
                }

                if (turn_1 == 3)
                {
                    if (rotation_3 >= 0.382f)
                    {
                        rotation_3 = 0.0f;
                        //rotation_3 += rotation_2.ToString() + "\n";
                        turn_1 = turn_1 + 1;
                        spin_1 = false;
                    }
                    else
                        rotation_4 = rotation_4 + 0.052f;
                }

                if (turn_1 == 4)
                {
                    if (rotation_3 >= 0.378f)
                    {
                        rotation_3 = 0.0f;
                        //rotation_3 += rotation_2.ToString() + "\n";
                        turn_1 = turn_1 + 1;
                        spin_1 = false;
                    }
                    else
                        rotation_4 = rotation_4 + 0.052f;
                }

                if (turn_1 == 5)
                {
                    if (rotation_3 >= 0.382f)
                    {
                        rotation_3 = 0.0f;
                        //rotation_3 += rotation_2.ToString() + "\n";
                        turn_1 = turn_1 + 1;
                        spin_1 = false;
                    }
                    else
                        rotation_4 = rotation_4 + 0.053f;
                }

                if (turn_1 == 6)
                {
                    if (rotation_3 >= 0.38f)
                    {
                        rotation_3 = 0.0f;
                        // rotation_3 += rotation_2.ToString() + "\n";
                        turn_1 = turn_1 + 1;
                        spin_1 = false;
                    }
                    else
                        rotation_4 = rotation_4 + 0.053f;
                }

                if (turn_1 == 7)
                {
                    if (rotation_3 >= 0.4f)
                    {
                        rotation_3 = 0.0f;
                        // rotation_3 += rotation_2.ToString() + "\n";
                        turn_1 = turn_1 + 1;
                        spin_1 = false;
                    }
                    else
                        rotation_4 = rotation_4 + 0.053f;
                }

                if (turn_1 == 8)
                {
                    if (rotation_3 >= 0.38f)
                    {
                        rotation_3 = 0.0f;
                        //rotation_3 += rotation_2.ToString() + "\n";
                        turn_1 = turn_1 + 1;
                        spin_1 = false;
                    }
                    else
                        rotation_4 = rotation_4 + 0.052f;
                }

                if (turn_1 == 9)
                {
                    if (rotation_3 >= 0.4f)
                    {
                        rotation_3 = 0.0f;
                        //rotation_3 += rotation_2.ToString() + "\n";
                        turn_1 = turn_1 + 1;
                        spin_1 = false;
                    }
                    else
                        rotation_4 = rotation_4 + 0.055f;
                }

                if (turn_1 == 10)
                {
                    if (rotation_3 >= 0.4f)
                    {
                        rotation_3 = 0.0f;
                        // rotation_3 += rotation_2.ToString() + "\n";
                        turn_1 = turn_1 + 1;
                        spin_1 = false;
                    }
                    else
                        rotation_4 = rotation_4 + 0.055f;
                }

                if (turn_1 == 11)
                {
                    if (rotation_3 >= 0.4f)
                    {
                        rotation_3 = 0.0f;
                        //rotation_3 += rotation_2.ToString() + "\n";
                        turn_1 = turn_1 + 1;
                        spin_1 = false;
                    }
                    else
                        rotation_4 = rotation_4 + 0.055f;
                }

                if (turn_1 == 12)
                {
                    if (rotation_3 >= 0.38f)
                    {
                        rotation_3 = 0.0f;
                        // rotation_3 += rotation_2.ToString() + "\n";
                        turn_1 = turn_1 + 1;
                        spin_1 = false;
                    }
                    else
                        rotation_4 = rotation_4 + 0.054f;
                }

                if (turn_1 == 13)
                {
                    if (rotation_3 >= 0.4f)
                    {
                        rotation_3 = 0.0f;
                        // rotation_3 += rotation_2.ToString() + "\n";
                        turn_1 = turn_1 + 1;
                        spin_1 = false;
                    }
                    else
                        rotation_4 = rotation_4 + 0.055f;
                }

                if (turn_1 == 14)
                {
                    if (rotation_3 >= 0.38f)
                    {
                        rotation_3 = 0.0f;
                        // rotation_3 += rotation_2.ToString() + "\n";
                        turn_1 = turn_1 + 1;
                        spin_1 = false;
                    }
                    else
                        rotation_4 = rotation_4 + 0.055f;
                }

                if (turn_1 == 15)
                {
                    if (rotation_3 >= 0.4f)
                    {
                        rotation_3 = 0.0f;
                        //rotation_3 += rotation_2.ToString() + "\n";
                        turn_1 = 0;
                        rotation_4 = 0.0f;
                        spin_1 = false;
                    }
                    else
                        rotation_4 = rotation_4 + 0.055f;
                }
                rotation_3 = rotation_3 - sDir*0.02f;
            }*/
        }

        private Color ColorBorder(int x, int y, int width, int height, int borderThickness, int borderRadius, int borderShadow, Color initialColor, List<Color> borderColors, float initialShadowIntensity, float finalShadowIntensity)
        {
            Rectangle internalRectangle = new Rectangle((borderThickness + borderRadius), (borderThickness + borderRadius), width - 2 * (borderThickness + borderRadius), height - 2 * (borderThickness + borderRadius));

            if (internalRectangle.Contains(x, y)) return initialColor;

            Vector2 origin = Vector2.Zero;
            Vector2 point = new Vector2(x, y);

            if (x < borderThickness + borderRadius)
            {
                if (y < borderRadius + borderThickness)
                    origin = new Vector2(borderRadius + borderThickness, borderRadius + borderThickness);
                else if (y > height - (borderRadius + borderThickness))
                    origin = new Vector2(borderRadius + borderThickness, height - (borderRadius + borderThickness));
                else
                    origin = new Vector2(borderRadius + borderThickness, y);
            }
            else if (x > width - (borderRadius + borderThickness))
            {
                if (y < borderRadius + borderThickness)
                    origin = new Vector2(width - (borderRadius + borderThickness), borderRadius + borderThickness);
                else if (y > height - (borderRadius + borderThickness))
                    origin = new Vector2(width - (borderRadius + borderThickness), height - (borderRadius + borderThickness));
                else
                    origin = new Vector2(width - (borderRadius + borderThickness), y);
            }
            else
            {
                if (y < borderRadius + borderThickness)
                    origin = new Vector2(x, borderRadius + borderThickness);
                else if (y > height - (borderRadius + borderThickness))
                    origin = new Vector2(x, height - (borderRadius + borderThickness));
            }

            if (!origin.Equals(Vector2.Zero))
            {
                float distance = Vector2.Distance(point, origin);

                if (distance > borderRadius + borderThickness + 1)
                {
                    return Color.Transparent;
                }
                else if (distance > borderRadius + 1)
                {
                    if (borderColors.Count > 2)
                    {
                        float modNum = distance - borderRadius;

                        if (modNum < borderThickness / 2)
                        {
                            return Color.Lerp(borderColors[2], borderColors[1], (float)((modNum) / (borderThickness / 2.0)));
                        }
                        else
                        {
                            return Color.Lerp(borderColors[1], borderColors[0], (float)((modNum - (borderThickness / 2.0)) / (borderThickness / 2.0)));
                        }
                    }


                    if (borderColors.Count > 0)
                        return borderColors[0];
                }
                else if (distance > borderRadius - borderShadow + 1)
                {
                    float mod = (distance - (borderRadius - borderShadow)) / borderShadow;
                    float shadowDiff = initialShadowIntensity - finalShadowIntensity;
                    return DarkenColor(initialColor, ((shadowDiff * mod) + finalShadowIntensity));
                }
            }

            return initialColor;
        }

        private Color DarkenColor(Color color, float shadowIntensity)
        {
            return Color.Lerp(color, Color.Black, shadowIntensity);
        }
    }
}