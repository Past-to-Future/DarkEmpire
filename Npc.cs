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

        public int Strength = 1;
        public int Vitality = 1;
        public int Perception = 1;
        public int Agility = 1;
        public int Intelligence = 1;
        public int Charisma = 1;
        public int AP = 1;
        public int Level = 1;
        public int XP = 1;

        public Npc(int ID, int dFace, Vector2 pos, float s = 1.0f, float role = 0)
        {
            if (role == 0)//scrub role
            {
                Strength = 1;
                Vitality = 1;
                Perception = 1;
                Agility = 1;
                Intelligence = 1;
                Charisma = 1;
                Level = 1;
                XP = 1;
            }

            else if(role == 1) //cheater role
            {
                Strength = 5;
                Vitality = 5;
                Perception = 5;
                Agility = 5;
                Intelligence = 5;
                Charisma = 5;
                Level = 1;
                XP = 1;
            }

            health = Level * (7+2*Vitality); //using this like a percentage

            scale = s;
            frame = 1;
            position = pos;
            directionFace = dFace; //# equals row on sprite sheet
            characterID = ID;
            partyPosition = 1;
            name = "dummy";
            //Has to be set to spritesheet and coordinated with artist
            rect = new Rectangle((characterID - 1) % 4 * 180 + (frame) * 60, characterID / 5 * 240 + (directionFace - 1) % 4 * 60, 60, 60);
        }

        public void setPartyPosition(int position)
        {
            partyPosition = position;
        }

        public void changeDirection(int dFace)
        {
            directionFace = dFace;
            rect = new Rectangle((characterID - 1) % 4 * 180 + (frame) * 60, characterID / 5 * 240 + (directionFace - 1) % 4 * 60, 60, 60);
        }

        Random rand = new Random();

        public void draw()
        {
            if (!powerup)
                spriteBatch.Draw(PlayingState.instance.npcSprite, position, rect, Color.White);
            else
                spriteBatch.Draw(PlayingState.instance.npcSprite, position, rect, new Color(rand.Next(255), rand.Next(255), rand.Next(255)));
        }


        public void DrawHealthAboveCharacter(int thicknessOfBorder = 2)
        {

            //Green = Health, Red = Lost Health
            //Bar has been set to be drawn above the sprite by 10% of the scaled height of the sprite 
            spriteBatch.Draw(pixel, new Vector2(position.X, position.Y - 3.2f * scale), new Rectangle(0, 0, (int)(32 * scale * HealthPercent() ), 10), Color.Green);
            spriteBatch.Draw(pixel, new Vector2(position.X + 32 * scale * HealthPercent(), position.Y - 3.2f * scale), new Rectangle(0, 0, (int)(32 * scale * (1.0f - health / MaxHealth())), 10), Color.Red);

            spriteBatch.Draw(pixel, new Vector2(position.X, position.Y - 3.2f * scale), new Rectangle(0, 0, (int)(32 * scale), thicknessOfBorder), Color.Black); //top
            spriteBatch.Draw(pixel, new Vector2(position.X, position.Y - 3.2f * scale), new Rectangle(0, 0, thicknessOfBorder, 10), Color.Black); //left
            spriteBatch.Draw(pixel, new Vector2(position.X + 32 * scale - thicknessOfBorder, position.Y - 3.2f * scale), new Rectangle(0, 0, thicknessOfBorder, 10), Color.Black); //right
            spriteBatch.Draw(pixel, new Vector2(position.X, position.Y - 3.2f * scale + 10 - thicknessOfBorder), new Rectangle(0, 0, (int)(32 * scale), thicknessOfBorder), Color.Black); //bottom
        }

        public float MaxHealth()
        {
            return Level * (7 + 2 * Vitality);
        }

        public float HealthPercent()
        {
            return health / MaxHealth();
        }

        public float XPPercent()
        {
            return XP/200f * (float)Math.Pow(1.5f, Level - 1);
        }

        public float XPtoLvl()
        {
            return 200f * (float)Math.Pow(1.5f, Level - 1);
        }
    }
}
