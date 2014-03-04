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
            float seperation = .0625f;

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

            shadowText(spriteBatch, "Status", new Vector2(Game1.instance.Width * 0.07f, Game1.instance.Height * .18f), statusSize);
            shadowText(spriteBatch, "Item", new Vector2(Game1.instance.Width * 0.07f, Game1.instance.Height * (.18f + seperation)), statusSize);
            shadowText(spriteBatch, "Equipment", new Vector2(Game1.instance.Width * 0.07f, Game1.instance.Height * (.18f + 2 * seperation)), statusSize);
            shadowText(spriteBatch, "Magic", new Vector2(Game1.instance.Width * 0.07f, Game1.instance.Height * (.18f + 3 * seperation)), statusSize);
            shadowText(spriteBatch, "Abilities", new Vector2(Game1.instance.Width * 0.07f, Game1.instance.Height * (.18f + 4 * seperation)), statusSize);
            shadowText(spriteBatch, "Formation", new Vector2(Game1.instance.Width * 0.07f, Game1.instance.Height * (.18f + 5 * seperation)), statusSize);
            shadowText(spriteBatch, "Back", new Vector2(Game1.instance.Width * 0.07f, Game1.instance.Height * (.18f + 10.65f * seperation)), statusSize);
   
            
            /*Character Portraits*/
            spriteBatch.Draw(c[HeroParty.theHero[0].characterID - 1], new Vector2(Game1.instance.Width * .35f, Game1.instance.Height * .16f), new Rectangle(0, 0, c[0].Width, c[0].Height), Color.White, 0.0f, Vector2.Zero, new Vector2(Game1.instance.Width * 0.20f / c[0].Width, Game1.instance.Height * 0.60f / c[0].Height), SpriteEffects.None, 0.0f);
            spriteBatch.Draw(c[HeroParty.theHero[1].characterID-1], new Vector2(Game1.instance.Width * .55f, Game1.instance.Height * .16f), new Rectangle(0, 0, c[1].Width, c[1].Height), Color.White, 0.0f, Vector2.Zero, new Vector2(Game1.instance.Width * 0.20f / c[1].Width, Game1.instance.Height * 0.60f / c[1].Height), SpriteEffects.None, 0.0f);
            spriteBatch.Draw(c[HeroParty.theHero[2].characterID-1], new Vector2(Game1.instance.Width * .74f, Game1.instance.Height * .16f), new Rectangle(0, 0, c[2].Width, c[2].Height), Color.White, 0.0f, Vector2.Zero, new Vector2(Game1.instance.Width * 0.20f / c[2].Width, Game1.instance.Height * 0.60f / c[2].Height), SpriteEffects.None, 0.0f);


            /*Character 1*/
            shadowText(spriteBatch, HeroParty.theHero[0].name, new Vector2(Game1.instance.Width * .35f, Game1.instance.Height * .685f), statusSize);
           
            //[Solid health bars]
            spriteBatch.Draw(pixel, new Vector2(Game1.instance.Width * 0.35f, Game1.instance.Height * .83f), new Rectangle(0, 0, (int)(HeroParty.theHero[0].health * Game1.instance.Width * 0.16f+1), 25), Color.Green);
            spriteBatch.Draw(pixel, new Vector2(Game1.instance.Width * 0.35f + Game1.instance.Width * 0.16f * HeroParty.theHero[0].health, Game1.instance.Height * .83f), new Rectangle(0, 0, (int)(Game1.instance.Width * 0.16f * (1.0f - HeroParty.theHero[0].health)), 25), Color.Red);

            //[Outline of health bars]
            spriteBatch.Draw(pixel, new Vector2(Game1.instance.Width * 0.35f, Game1.instance.Height * .83f), new Rectangle(0, 0, (int)(Game1.instance.Width * 0.16f), 2), Color.Black); //top
            spriteBatch.Draw(pixel, new Vector2(Game1.instance.Width * 0.35f, Game1.instance.Height * .83f), new Rectangle(0, 0, 2, 25), Color.Black); //left
            spriteBatch.Draw(pixel, new Vector2(Game1.instance.Width * 0.35f + Game1.instance.Width * 0.16f - 2, Game1.instance.Height * .83f), new Rectangle(0, 0, 2, 25), Color.Black); //right
            spriteBatch.Draw(pixel, new Vector2(Game1.instance.Width * 0.35f, Game1.instance.Height * .83f + 25 - 2), new Rectangle(0, 0, (int)(Game1.instance.Width * 0.16f), 2), Color.Black); //bottom

            shadowText(spriteBatch, "HP", new Vector2(Game1.instance.Width * .355f, Game1.instance.Height * .81f), statusSize * 1.5f);
            shadowText(spriteBatch, (HeroParty.theHero[0].health * 100).ToString() + "/100", new Vector2(Game1.instance.Width * .45f, Game1.instance.Height * .81f), statusSize * 1.5f);
            
            //[Solid XP bars]
            spriteBatch.Draw(pixel, new Vector2(Game1.instance.Width * 0.35f, Game1.instance.Height * .76f), new Rectangle(0, 0, (int)(HeroParty.theHero[0].health * Game1.instance.Width * 0.16f + 1), 25), Color.Green);
            spriteBatch.Draw(pixel, new Vector2(Game1.instance.Width * 0.35f + Game1.instance.Width * 0.16f * HeroParty.theHero[0].health, Game1.instance.Height * .76f), new Rectangle(0, 0, (int)(Game1.instance.Width * 0.16f * (1.0f - HeroParty.theHero[0].health)), 25), Color.Red);

            //[Outline of XP bars]
            spriteBatch.Draw(pixel, new Vector2(Game1.instance.Width * 0.35f, Game1.instance.Height * .76f), new Rectangle(0, 0, (int)(Game1.instance.Width * 0.16f), 2), Color.Black); //top
            spriteBatch.Draw(pixel, new Vector2(Game1.instance.Width * 0.35f, Game1.instance.Height * .76f), new Rectangle(0, 0, 2, 25), Color.Black); //left
            spriteBatch.Draw(pixel, new Vector2(Game1.instance.Width * 0.35f + Game1.instance.Width * 0.16f - 2, Game1.instance.Height * .76f), new Rectangle(0, 0, 2, 25), Color.Black); //right
            spriteBatch.Draw(pixel, new Vector2(Game1.instance.Width * 0.35f, Game1.instance.Height * .76f + 25 - 2), new Rectangle(0, 0, (int)(Game1.instance.Width * 0.16f), 2), Color.Black); //bottom

            shadowText(spriteBatch, "XP", new Vector2(Game1.instance.Width * .355f, Game1.instance.Height * .74f), statusSize * 1.5f);
            shadowText(spriteBatch, (HeroParty.theHero[0].health * 100).ToString() + "/100", new Vector2(Game1.instance.Width * .45f, Game1.instance.Height * .74f), statusSize * 1.5f);

            
            /*Character 2*/
            shadowText(spriteBatch, HeroParty.theHero[1].name, new Vector2(Game1.instance.Width * .56f, Game1.instance.Height * .685f), statusSize);

            //[Solid health bars]
            spriteBatch.Draw(pixel, new Vector2(Game1.instance.Width * 0.56f, Game1.instance.Height * .83f), new Rectangle(0, 0, (int)(HeroParty.theHero[1].health * Game1.instance.Width * 0.16f + 1), 25), Color.Green);
            spriteBatch.Draw(pixel, new Vector2(Game1.instance.Width * 0.56f + Game1.instance.Width * 0.16f * HeroParty.theHero[1].health, Game1.instance.Height * .83f), new Rectangle(0, 0, (int)(Game1.instance.Width * 0.16f * (1.0f - HeroParty.theHero[1].health)), 25), Color.Red);

            //[Outline of health bars]
            spriteBatch.Draw(pixel, new Vector2(Game1.instance.Width * 0.56f, Game1.instance.Height * .83f), new Rectangle(0, 0, (int)(Game1.instance.Width * 0.16f), 2), Color.Black); //top
            spriteBatch.Draw(pixel, new Vector2(Game1.instance.Width * 0.56f, Game1.instance.Height * .83f), new Rectangle(0, 0, 2, 25), Color.Black); //left
            spriteBatch.Draw(pixel, new Vector2(Game1.instance.Width * 0.56f + Game1.instance.Width * 0.16f - 2, Game1.instance.Height * .83f), new Rectangle(0, 0, 2, 25), Color.Black); //right
            spriteBatch.Draw(pixel, new Vector2(Game1.instance.Width * 0.56f, Game1.instance.Height * .83f + 25 - 2), new Rectangle(0, 0, (int)(Game1.instance.Width * 0.16f), 2), Color.Black); //bottom

            shadowText(spriteBatch, "HP", new Vector2(Game1.instance.Width * .565f, Game1.instance.Height * .81f), statusSize * 1.5f);
            shadowText(spriteBatch, (HeroParty.theHero[1].health * 100).ToString() + "/100", new Vector2(Game1.instance.Width * .66f, Game1.instance.Height * .81f), statusSize * 1.5f);

            //[Solid XP bars]
            spriteBatch.Draw(pixel, new Vector2(Game1.instance.Width * 0.56f, Game1.instance.Height * .76f), new Rectangle(0, 0, (int)(HeroParty.theHero[1].health * Game1.instance.Width * 0.16f + 1), 25), Color.Green);
            spriteBatch.Draw(pixel, new Vector2(Game1.instance.Width * 0.56f + Game1.instance.Width * 0.16f * HeroParty.theHero[1].health, Game1.instance.Height * .76f), new Rectangle(0, 0, (int)(Game1.instance.Width * 0.16f * (1.0f - HeroParty.theHero[1].health)), 25), Color.Red);

            //[Outline of XP bars]
            spriteBatch.Draw(pixel, new Vector2(Game1.instance.Width * 0.56f, Game1.instance.Height * .76f), new Rectangle(0, 0, (int)(Game1.instance.Width * 0.16f), 2), Color.Black); //top
            spriteBatch.Draw(pixel, new Vector2(Game1.instance.Width * 0.56f, Game1.instance.Height * .76f), new Rectangle(0, 0, 2, 25), Color.Black); //left
            spriteBatch.Draw(pixel, new Vector2(Game1.instance.Width * 0.56f + Game1.instance.Width * 0.16f - 2, Game1.instance.Height * .76f), new Rectangle(0, 0, 2, 25), Color.Black); //right
            spriteBatch.Draw(pixel, new Vector2(Game1.instance.Width * 0.56f, Game1.instance.Height * .76f + 25 - 2), new Rectangle(0, 0, (int)(Game1.instance.Width * 0.16f), 2), Color.Black); //bottom

            shadowText(spriteBatch, "XP", new Vector2(Game1.instance.Width * .565f, Game1.instance.Height * .74f), statusSize*1.5f);
            shadowText(spriteBatch, (HeroParty.theHero[1].health * 100).ToString() + "/100", new Vector2(Game1.instance.Width * .66f, Game1.instance.Height * .74f), statusSize*1.5f);


            /*Character 3*/

            shadowText(spriteBatch, HeroParty.theHero[2].name, new Vector2(Game1.instance.Width * .76f, Game1.instance.Height * .685f), statusSize);
            //[Solid health bars]
            spriteBatch.Draw(pixel, new Vector2(Game1.instance.Width * 0.76f, Game1.instance.Height * .83f), new Rectangle(0, 0, (int)(HeroParty.theHero[2].health * Game1.instance.Width * 0.16f + 1), 25), Color.Green);
            spriteBatch.Draw(pixel, new Vector2(Game1.instance.Width * 0.76f + Game1.instance.Width * 0.16f * HeroParty.theHero[2].health, Game1.instance.Height * .83f), new Rectangle(0, 0, (int)(Game1.instance.Width * 0.16f * (1.0f - HeroParty.theHero[2].health)), 25), Color.Red);

            //[Outline of health bars]
            spriteBatch.Draw(pixel, new Vector2(Game1.instance.Width * 0.76f, Game1.instance.Height * .83f), new Rectangle(0, 0, (int)(Game1.instance.Width * 0.16f), 2), Color.Black); //top
            spriteBatch.Draw(pixel, new Vector2(Game1.instance.Width * 0.76f, Game1.instance.Height * .83f), new Rectangle(0, 0, 2, 25), Color.Black); //left
            spriteBatch.Draw(pixel, new Vector2(Game1.instance.Width * 0.76f + Game1.instance.Width * 0.16f - 2, Game1.instance.Height * .83f), new Rectangle(0, 0, 2, 25), Color.Black); //right
            spriteBatch.Draw(pixel, new Vector2(Game1.instance.Width * 0.76f, Game1.instance.Height * .83f + 25 - 2), new Rectangle(0, 0, (int)(Game1.instance.Width * 0.16f), 2), Color.Black); //bottom

            shadowText(spriteBatch, "HP", new Vector2(Game1.instance.Width * .765f, Game1.instance.Height * .81f), statusSize * 1.5f);
            shadowText(spriteBatch, (HeroParty.theHero[2].health * 100).ToString() + "/100", new Vector2(Game1.instance.Width * .86f, Game1.instance.Height * .81f), statusSize * 1.5f);

            //[Solid XP bars]
            spriteBatch.Draw(pixel, new Vector2(Game1.instance.Width * 0.76f, Game1.instance.Height * .76f), new Rectangle(0, 0, (int)(HeroParty.theHero[2].health * Game1.instance.Width * 0.16f + 1), 25), Color.Green);
            spriteBatch.Draw(pixel, new Vector2(Game1.instance.Width * 0.76f + Game1.instance.Width * 0.16f * HeroParty.theHero[2].health, Game1.instance.Height * .76f), new Rectangle(0, 0, (int)(Game1.instance.Width * 0.16f * (1.0f - HeroParty.theHero[2].health)), 25), Color.Red);

            //[Outline of XP bars]
            spriteBatch.Draw(pixel, new Vector2(Game1.instance.Width * 0.76f, Game1.instance.Height * .76f), new Rectangle(0, 0, (int)(Game1.instance.Width * 0.16f), 2), Color.Black); //top
            spriteBatch.Draw(pixel, new Vector2(Game1.instance.Width * 0.76f, Game1.instance.Height * .76f), new Rectangle(0, 0, 2, 25), Color.Black); //left
            spriteBatch.Draw(pixel, new Vector2(Game1.instance.Width * 0.76f + Game1.instance.Width * 0.16f - 2, Game1.instance.Height * .76f), new Rectangle(0, 0, 2, 25), Color.Black); //right
            spriteBatch.Draw(pixel, new Vector2(Game1.instance.Width * 0.76f, Game1.instance.Height * .76f + 25 - 2), new Rectangle(0, 0, (int)(Game1.instance.Width * 0.16f), 2), Color.Black); //bottom

            shadowText(spriteBatch, "XP", new Vector2(Game1.instance.Width * .765f, Game1.instance.Height * .74f), statusSize * 1.5f);
            shadowText(spriteBatch, (HeroParty.theHero[2].health * 100).ToString() + "/100", new Vector2(Game1.instance.Width * .86f, Game1.instance.Height * .74f), statusSize * 1.5f);
            spriteBatch.Draw(backbone, screenRect, Color.White);

        }

        public void shadowText(SpriteBatch spriteBatch, String text, Vector2 position, Vector2 statusSize)
        {
            spriteBatch.DrawString(menuText, text, position + new Vector2(1, 1), Color.White, 0.0f, new Vector2(0, 0), new Vector2(Game1.instance.Width * 0.08f / statusSize.X, Game1.instance.Width * 0.08f / statusSize.X), SpriteEffects.None, 0.0f);
            spriteBatch.DrawString(menuText, text, position + new Vector2(1, -1), Color.White, 0.0f, new Vector2(0, 0), new Vector2(Game1.instance.Width * 0.08f / statusSize.X, Game1.instance.Width * 0.08f / statusSize.X), SpriteEffects.None, 0.0f);
            spriteBatch.DrawString(menuText, text, position + new Vector2(-1, 1), Color.White, 0.0f, new Vector2(0, 0), new Vector2(Game1.instance.Width * 0.08f / statusSize.X, Game1.instance.Width * 0.08f / statusSize.X), SpriteEffects.None, 0.0f);
            spriteBatch.DrawString(menuText, text, position + new Vector2(-1, -1), Color.White, 0.0f, new Vector2(0, 0), new Vector2(Game1.instance.Width * 0.08f / statusSize.X, Game1.instance.Width * 0.08f / statusSize.X), SpriteEffects.None, 0.0f);
            spriteBatch.DrawString(menuText, text, position, Color.Black, 0.0f, new Vector2(0, 0), new Vector2(Game1.instance.Width * 0.08f / statusSize.X, Game1.instance.Width * 0.08f / statusSize.X), SpriteEffects.None, 0.0f);
        }
    }
}
