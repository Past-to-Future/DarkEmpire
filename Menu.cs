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
        public Texture2D background, c1, c2, c3;
        public SpriteFont menuText;

        public Menu()
        {
            activeMenu = false;
            selectCharacter = false;
            menuSelection = 1;
            characterSelection = 1;
        }

        public void Initialize()
        {
            background = Game1.instance.Content.Load<Texture2D>("Menu");
            menuText = Game1.instance.Content.Load<SpriteFont>("console");
            c1 = Game1.instance.Content.Load<Texture2D>("Maxum");
            c2 = Game1.instance.Content.Load<Texture2D>("Jasmine");
            c3 = Game1.instance.Content.Load<Texture2D>("WizardGirl");
            pixel.SetData(new[] { Color.White }); //make it white so we can color it
        }

  
   
        public void Draw()
        {
            SpriteBatch spriteBatch = Game1.instance.SpriteBatch;
            Rectangle screenRect = new Rectangle(0, 0, Game1.instance.GraphicsDevice.Viewport.Width, Game1.instance.GraphicsDevice.Viewport.Height);
            spriteBatch.Draw(background, screenRect, Color.White);
            float seperation = .0625f;

            String status = "Status";
            Vector2 statusSize = menuText.MeasureString(status);
            Vector2 statusSizename;

            spriteBatch.Draw(pixel, new Vector2(Game1.instance.Width * 0.06f, Game1.instance.Height * (.1825f+(menuSelection-1)*0.063492f)), new Rectangle(0, 0, (int)(Game1.instance.Width * 0.17f), (int)(Game1.instance.Height * 0.0476f)), new Color(Color.Green, 0.1f));

            if(selectCharacter)
                spriteBatch.Draw(pixel, new Vector2(Game1.instance.Width * (.3484f + (characterSelection - 1) * .2036f), Game1.instance.Height * .1824f), new Rectangle(0, 0, (int)(Game1.instance.Width * .19375f), (int)(Game1.instance.Height * .716666f)), new Color(Color.White, 0.1f));

            spriteBatch.DrawString(menuText, "Status", new Vector2(Game1.instance.Width * 0.07f, Game1.instance.Height * .175f), Color.Black, 0.0f, new Vector2(0, 0), Game1.instance.Width * 0.10f / statusSize.X, SpriteEffects.None, 0.0f);
            spriteBatch.DrawString(menuText, "Item", new Vector2(Game1.instance.Width * 0.07f, Game1.instance.Height * (.175f + seperation)), Color.Black, 0.0f, new Vector2(0, 0), Game1.instance.Width * 0.10f / statusSize.X, SpriteEffects.None, 0.0f);
            spriteBatch.DrawString(menuText, "Equipment", new Vector2(Game1.instance.Width * 0.07f, Game1.instance.Height * (.175f + seperation * 2)), Color.Black, 0.0f, new Vector2(0, 0), Game1.instance.Width * 0.10f / statusSize.X, SpriteEffects.None, 0.0f);
            spriteBatch.DrawString(menuText, "Magic", new Vector2(Game1.instance.Width * 0.07f, Game1.instance.Height * (.175f + seperation * 3)), Color.Black, 0.0f, new Vector2(0, 0), Game1.instance.Width * 0.10f / statusSize.X, SpriteEffects.None, 0.0f);
            spriteBatch.DrawString(menuText, "Abilities", new Vector2(Game1.instance.Width * 0.07f, Game1.instance.Height * (.175f + seperation * 4)), Color.Black, 0.0f, new Vector2(0, 0), Game1.instance.Width * 0.10f / statusSize.X, SpriteEffects.None, 0.0f);
            spriteBatch.DrawString(menuText, "Formation", new Vector2(Game1.instance.Width * 0.07f, Game1.instance.Height * (.175f + seperation * 5)), Color.Black, 0.0f, new Vector2(0, 0), Game1.instance.Width * 0.10f / statusSize.X, SpriteEffects.None, 0.0f);
            spriteBatch.Draw(c1, new Vector2(Game1.instance.Width * .35f, Game1.instance.Height * .15f), new Rectangle(0, 0, c1.Width, c1.Height), Color.White, 0.0f, Vector2.Zero, new Vector2(Game1.instance.Width*0.20f/c1.Width, Game1.instance.Height * 0.70f / c1.Height), SpriteEffects.None, 0.0f);
            spriteBatch.Draw(c2, new Vector2(Game1.instance.Width * .55f, Game1.instance.Height * .20f), new Rectangle(0, 0, c2.Width, c2.Height), Color.White, 0.0f, Vector2.Zero, new Vector2(Game1.instance.Width * 0.20f / c2.Width, Game1.instance.Height * 0.60f / c2.Height), SpriteEffects.None, 0.0f);
            spriteBatch.Draw(c3, new Vector2(Game1.instance.Width * .74f, Game1.instance.Height * .15f), new Rectangle(0, 0, c3.Width, c3.Height), Color.White, 0.0f, Vector2.Zero, new Vector2(Game1.instance.Width * 0.20f / c3.Width, Game1.instance.Height * 0.70f / c3.Height), SpriteEffects.None, 0.0f);

            statusSizename = menuText.MeasureString(HeroParty.theHero[0].name);
            spriteBatch.DrawString(menuText, HeroParty.theHero[0].name, new Vector2(Game1.instance.Width * .35f, Game1.instance.Height * .8f), Color.Black, 0.0f, new Vector2(0, 0), new Vector2(Game1.instance.Width * 0.15f / statusSizename.X, Game1.instance.Height * 0.10f / statusSizename.Y), SpriteEffects.None, 0.0f);

            statusSizename = menuText.MeasureString(HeroParty.theHero[1].name);
            spriteBatch.DrawString(menuText, HeroParty.theHero[1].name, new Vector2(Game1.instance.Width * .575f, Game1.instance.Height * .8f), Color.Black, 0.0f, new Vector2(0, 0), new Vector2(Game1.instance.Width * 0.15f / statusSizename.X, Game1.instance.Height * 0.10f / statusSizename.Y), SpriteEffects.None, 0.0f);

            statusSizename = menuText.MeasureString(HeroParty.theHero[2].name);
            spriteBatch.DrawString(menuText, HeroParty.theHero[2].name, new Vector2(Game1.instance.Width * .775f, Game1.instance.Height * .8f), Color.Black, 0.0f, new Vector2(0, 0), new Vector2(Game1.instance.Width * 0.15f / statusSizename.X, Game1.instance.Height * 0.10f / statusSizename.Y), SpriteEffects.None, 0.0f);

        }
    }
}
