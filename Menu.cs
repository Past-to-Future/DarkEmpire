﻿using System;
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
using Microsoft.Xna.Framework.Audio;
namespace DarkEmpire
{
    public class Menu
    {
        public static Texture2D pixel = new Texture2D(Game1.instance.GraphicsDevice, 1, 1); //create 1x1 pixel texture
        public bool activeMenu, selectCharacter;
        public int menuSelection, characterSelection;
        public Texture2D background, box_background, white_background, backbone;
        public Texture2D[] c = new Texture2D[3];
        public static SpriteFont menuText;


        static float Width = Game1.instance.Width;
        static float Height = Game1.instance.Height;
        float pctW_07 = Width * .07f;
        float pctW_08 = Width * .08f;
        float pctW_36 = Width * .36f;
        float seperation = .0625f; //distance between main menu boxes

        public Menu()
        {
            activeMenu = false;
            selectCharacter = false;
            menuSelection = 1;
            characterSelection = 1;
        }

        public void Initialize()
        {
            background = Game1.instance.Content.Load<Texture2D>("background");
            box_background = Game1.instance.Content.Load<Texture2D>("light backbone");
            white_background = Game1.instance.Content.Load<Texture2D>("background white");
            backbone = Game1.instance.Content.Load<Texture2D>("backbone");
            menuText = Game1.instance.Content.Load<SpriteFont>("console");
            c[0] = Game1.instance.Content.Load<Texture2D>("Maxum");
            c[1] = Game1.instance.Content.Load<Texture2D>("Jasmine");
            c[2] = Game1.instance.Content.Load<Texture2D>("WizardGirl");
            pixel.SetData(new[] { Color.White }); //make it white so we can color it
        }

  
   
        public void Draw()
        {
            SpriteBatch spriteBatch = Game1.instance.SpriteBatch;
            Rectangle screenRect = new Rectangle(0, 0, Game1.instance.GraphicsDevice.Viewport.Width, Game1.instance.GraphicsDevice.Viewport.Height);
            spriteBatch.Draw(white_background, screenRect, Color.White);
            spriteBatch.Draw(background, screenRect, Color.White);
            spriteBatch.Draw(box_background, screenRect, new Color(Color.White, 0.9f));

            String status = "Status";
            Vector2 statusSize = menuText.MeasureString(status);

            /*Normal menu options*/
            if(menuSelection <10)
                spriteBatch.Draw(pixel, new Vector2(Game1.instance.Width * 0.06f, Game1.instance.Height * (.1825f+(menuSelection-1)*0.063492f)), new Rectangle(0, 0, (int)(Game1.instance.Width * 0.17f), (int)(Game1.instance.Height * 0.0476f)), new Color(Color.Green, 0.1f));
            /*Back Button*/
            if(menuSelection == 10)
                spriteBatch.Draw(pixel, new Vector2(Game1.instance.Width * 0.06f, Game1.instance.Height * (.1825f + 10.6f * 0.063492f)), new Rectangle(0, 0, (int)(Game1.instance.Width * 0.17f), (int)(Game1.instance.Height * 0.0476f)), new Color(Color.Green, 0.1f));

            /*Character selection*/
            if(selectCharacter && characterSelection !=10)
                spriteBatch.Draw(pixel, new Vector2(Game1.instance.Width * (.3484f + (characterSelection - 1) * .2036f), Game1.instance.Height * .1824f), new Rectangle(0, 0, (int)(Game1.instance.Width * .19375f), (int)(Game1.instance.Height * .716666f)), new Color(Color.White, 0.1f));
            /*Back button during character selection*/
            else if(selectCharacter && characterSelection == 10)
                spriteBatch.Draw(pixel, new Vector2(Game1.instance.Width * 0.06f, Game1.instance.Height * (.1825f + 10.6f * 0.063492f)), new Rectangle(0, 0, (int)(Game1.instance.Width * 0.17f), (int)(Game1.instance.Height * 0.0476f)), new Color(Color.Green, 0.1f));


            /* Main Menu Boxes*/

            shadowText(spriteBatch, "Status", new Vector2(pctW_07, Height * .18f), statusSize);
            shadowText(spriteBatch, "Item", new Vector2(pctW_07, Height * (.18f + seperation)), statusSize);
            shadowText(spriteBatch, "Equipment", new Vector2(pctW_07, Height * (.18f + 2 * seperation)), statusSize);
            shadowText(spriteBatch, "Magic", new Vector2(pctW_07, Height * (.18f + 3 * seperation)), statusSize);
            shadowText(spriteBatch, "Abilities", new Vector2(pctW_07, Height * (.18f + 4 * seperation)), statusSize);
            shadowText(spriteBatch, "Formation", new Vector2(pctW_07, Height * (.18f + 5 * seperation)), statusSize);
            shadowText(spriteBatch, "Back", new Vector2(pctW_07, Height * (.18f + 10.65f * seperation)), statusSize);
   
            
            /*Character Portraits*/
            for (int i = 0; i < 3; i++)
            {
                spriteBatch.Draw(c[HeroParty.theHero[i].characterID - 1], new Vector2(Width * .35f + Width*.20f * i, Height * .16f), new Rectangle(0, 0, c[i].Width, c[i].Height), Color.White, 0.0f, Vector2.Zero, new Vector2(Width * 0.20f / c[i].Width, Height * 0.60f / c[i].Height), SpriteEffects.None, 0.0f);
                shadowText(spriteBatch, HeroParty.theHero[i].name, new Vector2(pctW_36 + Width * .20f * i, Game1.instance.Height * .685f), statusSize);

                //[Solid health bars]
                spriteBatch.Draw(pixel, new Vector2(pctW_36 + Width * .20f * i, Height * .76f), new Rectangle(0, 0, (int)(HeroParty.theHero[0].HealthPercent() * Width * 0.16f), 25), Color.Green);
                spriteBatch.Draw(pixel, new Vector2(pctW_36 + Width * .20f * i + Width * 0.16f * HeroParty.theHero[0].HealthPercent(), Height * .76f), new Rectangle(0, 0, (int)(Width * 0.16f * (1.0f - HeroParty.theHero[0].HealthPercent())), 25), Color.Red);

                //[Outline of health bars]
                spriteBatch.Draw(pixel, new Vector2(pctW_36 + Width * .20f * i, Height * .76f), new Rectangle(0, 0, (int)(Width * 0.16f), 2), Color.Black); //top
                spriteBatch.Draw(pixel, new Vector2(pctW_36 + Width * .20f * i, Height * .76f), new Rectangle(0, 0, 2, 25), Color.Black); //left
                spriteBatch.Draw(pixel, new Vector2(pctW_36 + Width * .20f * i + Width * 0.16f - 2, Game1.instance.Height * .76f), new Rectangle(0, 0, 2, 25), Color.Black); //right
                spriteBatch.Draw(pixel, new Vector2(pctW_36 + Width * .20f * i, Height * .76f + 25 - 2), new Rectangle(0, 0, (int)(Width * 0.16f), 2), Color.Black); //bottom

                //[Solid XP bars]
                spriteBatch.Draw(pixel, new Vector2(pctW_36 + Width * .20f * i, Height * .83f), new Rectangle(0, 0, (int)(HeroParty.theHero[i].XPPercent() * Width * 0.16f + 1), 25), Color.Green);
                spriteBatch.Draw(pixel, new Vector2(pctW_36 + Width * .20f * i + Width * 0.16f * HeroParty.theHero[i].XPPercent(), Height * .83f), new Rectangle(0, 0, (int)(Width * 0.16f * (1.0f - HeroParty.theHero[i].XPPercent())), 25), Color.Red);

                //[Outline of XP bars]
                spriteBatch.Draw(pixel, new Vector2(pctW_36 + Width * .20f * i, Height * .83f), new Rectangle(0, 0, (int)(Width * 0.16f), 2), Color.Black); //top
                spriteBatch.Draw(pixel, new Vector2(pctW_36 + Width * .20f * i, Height * .83f), new Rectangle(0, 0, 2, 25), Color.Black); //left
                spriteBatch.Draw(pixel, new Vector2(pctW_36 + Width * .20f * i + Width * 0.16f - 2, Height * .83f), new Rectangle(0, 0, 2, 25), Color.Black); //right
                spriteBatch.Draw(pixel, new Vector2(pctW_36 + Width * .20f * i, Height * .83f + 25 - 2), new Rectangle(0, 0, (int)(Width * 0.16f), 2), Color.Black); //bottom

                shadowText(spriteBatch, "XP", new Vector2(Width * .365f + Width * .20f * i, Height * .81f), statusSize * 1.5f);
                shadowText(spriteBatch, "HP", new Vector2(Width * .365f + Width * .20f * i, Height * .74f), statusSize * 1.5f);

                shadowText(spriteBatch, HeroParty.theHero[i].XP.ToString() + "/" + HeroParty.theHero[i].XPtoLvl(), new Vector2(Width * .45f + Width * .20f * i, Height * .81f), statusSize * 1.5f);
                shadowText(spriteBatch, HeroParty.theHero[i].health.ToString() + "/" + HeroParty.theHero[i].MaxHealth().ToString(), new Vector2(Width * .45f + Width * .20f * i, Height * .74f), statusSize * 1.5f);

            }

           
            spriteBatch.Draw(backbone, screenRect, Color.White);

        }

        public void shadowText(SpriteBatch spriteBatch, String text, Vector2 position, Vector2 statusSize)
        {
            spriteBatch.DrawString(menuText, text, position + new Vector2(1, 1), Color.White, 0.0f, new Vector2(0, 0), new Vector2(pctW_08 / statusSize.X, pctW_08 / statusSize.X), SpriteEffects.None, 0.0f);
            spriteBatch.DrawString(menuText, text, position + new Vector2(1, -1), Color.White, 0.0f, new Vector2(0, 0), new Vector2(pctW_08 / statusSize.X, pctW_08 / statusSize.X), SpriteEffects.None, 0.0f);
            spriteBatch.DrawString(menuText, text, position + new Vector2(-1, 1), Color.White, 0.0f, new Vector2(0, 0), new Vector2(pctW_08 / statusSize.X, pctW_08 / statusSize.X), SpriteEffects.None, 0.0f);
            spriteBatch.DrawString(menuText, text, position + new Vector2(-1, -1), Color.White, 0.0f, new Vector2(0, 0), new Vector2(pctW_08 / statusSize.X, pctW_08 / statusSize.X), SpriteEffects.None, 0.0f);

            spriteBatch.DrawString(menuText, text, position, Color.Black, 0.0f, new Vector2(0, 0), new Vector2(pctW_08 / statusSize.X, pctW_08 / statusSize.X), SpriteEffects.None, 0.0f);
        }
    }
}
