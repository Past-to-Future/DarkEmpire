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
    public class Npc
    {
        public Rectangle rect;
        public int characterID;
        public int directionFace;
        public int frame;
        public int partyPosition;
        public float health;
        public float scale;
        public String name;
        public Vector2 position;
        public bool powerup = false;
        public static Texture2D pixel = new Texture2D(Game1.instance.GraphicsDevice, 1, 1); //create 1x1 pixel texture
        SpriteBatch spriteBatch = Game1.instance.SpriteBatch;

        public Npc(int ID, int dFace, Vector2 pos, float s = 1.0f)
        {
            scale = s;
            frame = 1;
            health = 1.0f; //using this like a percentage
            position = pos;
            directionFace = dFace; //# equals row on sprite sheet
            characterID = ID;
            partyPosition = 1;
            name = "dummy";
            //Has to be set to spritesheet and coordinated with artist
            rect = new Rectangle((characterID - 1) % 4 * 60 * 3 + (frame) * 60, characterID / 5 * 60 * 4 + (directionFace - 1) % 4 * 60, 60, 60);
        }

        public void setPartyPosition(int position)
        {
            partyPosition = position;
        }

        public void changeDirection(int dFace)
        {
            directionFace = dFace;
            rect = new Rectangle((characterID - 1) % 4 * 60 * 3 + (frame) * 60, characterID / 5 * 60 * 4 + (directionFace - 1) % 4 * 60, 60, 60);
        }

        Random rand = new Random();

        public void draw()
        {
            if (!powerup)
                spriteBatch.Draw(PlayingState.instance.npcSprite, position, rect, Color.White);
            else
                spriteBatch.Draw(PlayingState.instance.npcSprite, position, rect, new Color(rand.Next(255), rand.Next(255), rand.Next(255)));
        }


        public void DrawHealthAboveCharacter()
        {
            //Green = Health, Red = Lost Health
            //Bar has been set to be drawn above the sprite by 10% of the scaled height of the sprite 
            spriteBatch.Draw(pixel, new Vector2(position.X, position.Y - 0.1f * 32 * scale), new Rectangle(0, 0, (int)(32 * scale * health), 10), Color.Green);
            spriteBatch.Draw(pixel, new Vector2(position.X + 32 * scale * health, position.Y - 0.1f * 32 * scale), new Rectangle(0, 0, (int)(32 * scale * (1.0f - health)), 10), Color.Red);
        }

        public void DrawHealthAboveWithOutline(int thicknessOfBorder = 2) 
        {            
            //Draw Without Outline
            DrawHealthAboveCharacter(); 

            //Draw Outline
            spriteBatch.Draw(pixel, new Vector2(position.X, position.Y - 0.1f * 32 * scale), new Rectangle(0, 0, (int)(32 * scale), thicknessOfBorder), Color.Black); //top
            spriteBatch.Draw(pixel, new Vector2(position.X, position.Y - 0.1f * 32 * scale), new Rectangle(0, 0, thicknessOfBorder, 10), Color.Black); //left
            spriteBatch.Draw(pixel, new Vector2(position.X + 32 * scale - thicknessOfBorder, position.Y - 0.1f * 32 * scale), new Rectangle(0, 0, thicknessOfBorder, 10), Color.Black); //right
            spriteBatch.Draw(pixel, new Vector2(position.X, position.Y - 0.1f * 32 * scale + 10 - thicknessOfBorder), new Rectangle(0, 0, (int)(32 * scale), thicknessOfBorder), Color.Black); //bottom

        }

        public void DrawHealthCorner()
        {
            //Health Bar Botom Right Corner
            spriteBatch.Draw(pixel, new Vector2(Game1.instance.Width * 0.7f, Game1.instance.Height * .75f + Game1.instance.Height * .05f * partyPosition), new Rectangle(0, 0, (int)(health * Game1.instance.Width * 0.2f), 25), Color.Green);
            spriteBatch.Draw(pixel, new Vector2(Game1.instance.Width * 0.7f + Game1.instance.Width * 0.2f * health, Game1.instance.Height * .75f + Game1.instance.Height * .05f * partyPosition), new Rectangle(0, 0, (int)(Game1.instance.Width * 0.2f * (1.0f - health)), 25), Color.Red);
        }

        public void DrawHealthCornerWithOutline(int thicknessOfBorder = 2) 
        {
            //Draw Without Outline
            DrawHealthCorner();
            //Draw Outline
            spriteBatch.DrawString(BattleSystem.battleText, name, new Vector2(Game1.instance.Width * 0.525f, Game1.instance.Height * .75f + Game1.instance.Height * .05f * partyPosition), Color.Black);
            spriteBatch.Draw(pixel, new Vector2(Game1.instance.Width * 0.7f, Game1.instance.Height * .75f + Game1.instance.Height * .05f * partyPosition), new Rectangle(0, 0, (int)(Game1.instance.Width * 0.2f), thicknessOfBorder), Color.Black); //top
            spriteBatch.Draw(pixel, new Vector2(Game1.instance.Width * 0.7f, Game1.instance.Height * .75f + Game1.instance.Height * .05f * partyPosition), new Rectangle(0, 0, thicknessOfBorder, 25), Color.Black); //left
            spriteBatch.Draw(pixel, new Vector2(Game1.instance.Width * 0.7f + Game1.instance.Width * 0.2f - thicknessOfBorder, Game1.instance.Height * .75f + Game1.instance.Height * .05f * partyPosition), new Rectangle(0, 0, thicknessOfBorder, 25), Color.Black); //right
            spriteBatch.Draw(pixel, new Vector2(Game1.instance.Width * 0.7f, Game1.instance.Height * .75f + Game1.instance.Height * .05f * partyPosition + 25 - thicknessOfBorder), new Rectangle(0, 0, (int)(Game1.instance.Width * 0.2f), thicknessOfBorder), Color.Black); //bottom

        }

        public void DrawHealthCornerLeft()
        {
            //Health Bar Botom Right Corner
            spriteBatch.Draw(pixel, new Vector2(Game1.instance.Width * 0.25f, Game1.instance.Height * .75f + Game1.instance.Height * .05f * partyPosition), new Rectangle(0, 0, (int)(health * Game1.instance.Width * 0.2f), 25), Color.Green);
            spriteBatch.Draw(pixel, new Vector2(Game1.instance.Width * 0.25f + Game1.instance.Width * 0.2f * health, Game1.instance.Height * .75f + Game1.instance.Height * .05f * partyPosition), new Rectangle(0, 0, (int)(Game1.instance.Width * 0.2f * (1.0f - health)), 25), Color.Red);
        }

        public void DrawHealthCornerLeftWithOutline(int thicknessOfBorder = 2)
        {
            //Draw Without Outline
            DrawHealthCornerLeft();
            //Draw Outline
            spriteBatch.DrawString(BattleSystem.battleText, name, new Vector2(Game1.instance.Width * 0.075f, Game1.instance.Height * .75f + Game1.instance.Height * .05f * partyPosition), Color.Black);
            spriteBatch.Draw(pixel, new Vector2(Game1.instance.Width * 0.25f, Game1.instance.Height * .75f + Game1.instance.Height * .05f * partyPosition), new Rectangle(0, 0, (int)(Game1.instance.Width * 0.2f), thicknessOfBorder), Color.Black); //top
            spriteBatch.Draw(pixel, new Vector2(Game1.instance.Width * 0.25f, Game1.instance.Height * .75f + Game1.instance.Height * .05f * partyPosition), new Rectangle(0, 0, thicknessOfBorder, 25), Color.Black); //left
            spriteBatch.Draw(pixel, new Vector2(Game1.instance.Width * 0.25f + Game1.instance.Width * 0.2f - thicknessOfBorder, Game1.instance.Height * .75f + Game1.instance.Height * .05f * partyPosition), new Rectangle(0, 0, thicknessOfBorder, 25), Color.Black); //right
            spriteBatch.Draw(pixel, new Vector2(Game1.instance.Width * 0.25f, Game1.instance.Height * .75f + Game1.instance.Height * .05f * partyPosition + 25 - thicknessOfBorder), new Rectangle(0, 0, (int)(Game1.instance.Width * 0.2f), thicknessOfBorder), Color.Black); //bottom
        }

    }
}
