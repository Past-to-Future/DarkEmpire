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
        public float health;
        public float Scale;
        public Vector2 position;
        public bool powerup = false;
        Texture2D pixel = new Texture2D((GraphicsDevice)Game1.Instance.Services.GetService(typeof(GraphicsDevice)), 1, 1); //create 1x1 pixel texture

        public Npc(int ID, int dFace, Vector2 pos, float scale = 1.0f)
        {
            Scale = scale;
            frame = 1;
            health = 1.0f; //using this like a percentage
            position = pos;
            directionFace = dFace; //# equals row on sprite sheet
            characterID = ID;

            //Has to be set to spritesheet and coordinated with artist
            rect = new Rectangle((characterID - 1) % 4 * 32 * 3 + (frame) * 32, characterID / 5 * 32 * 4 + (directionFace - 1) % 4 * 32, 32, 32);
        }

        public void changeDirection(int dFace)
        {
            directionFace = dFace;
            rect = new Rectangle((characterID - 1) % 4 * 32 * 3 + (frame) * 32, characterID / 5 * 32 * 4 + (directionFace - 1) % 4 * 32, 32, 32);
        }


        public void DrawHealth()
        {
            SpriteBatch spriteBatch = (SpriteBatch)Game1.Instance.Services.GetService(typeof(SpriteBatch));
            pixel.SetData(new[] { Color.White }); //make it white so we can color it

            //Green = Health, Red = Lost Health
            //Bar has been set to be drawn above the sprite by 10% of the scaled height of the sprite 
            spriteBatch.Draw(pixel, new Vector2(position.X, position.Y - 0.1f * 32 * Scale), new Rectangle(0, 0, (int)(32 * Scale * health), 10), Color.Green);
            spriteBatch.Draw(pixel, new Vector2(position.X + 32 * Scale * health, position.Y - 0.1f * 32 * Scale), new Rectangle(0, 0, (int)(32 * Scale * (1.0f - health)), 10), Color.Red);
        }

        public void DrawHealthWithOutline(int thicknessOfBorder = 2) //DrawHealthWithOutline() = default 2  , DrawHealthWithOutline(5) for override
        {
            SpriteBatch spriteBatch = (SpriteBatch)Game1.Instance.Services.GetService(typeof(SpriteBatch));
            pixel.SetData(new[] { Color.White }); //make it white so we can color it

            //Draw Without outline
            spriteBatch.Draw(pixel, new Vector2(position.X, position.Y - 0.1f * 32 * Scale), new Rectangle(0, 0, (int)(32 * Scale * health), 10), Color.Green);
            spriteBatch.Draw(pixel, new Vector2(position.X + 32 * Scale * health, position.Y - 0.1f * 32 * Scale), new Rectangle(0, 0, (int)(32 * Scale * (1.0f - health)), 10), Color.Red);

            //Draw Outline
            spriteBatch.Draw(pixel, new Vector2(position.X, position.Y - 0.1f * 32 * Scale), new Rectangle(0, 0, (int)(32 * Scale), thicknessOfBorder), Color.Black); //top
            spriteBatch.Draw(pixel, new Vector2(position.X, position.Y - 0.1f * 32 * Scale), new Rectangle(0, 0, thicknessOfBorder, 10), Color.Black); //left
            spriteBatch.Draw(pixel, new Vector2(position.X + 32 * Scale - thicknessOfBorder, position.Y - 0.1f * 32 * Scale), new Rectangle(0, 0, thicknessOfBorder, 10), Color.Black); //right
            spriteBatch.Draw(pixel, new Vector2(position.X, position.Y - 0.1f * 32 * Scale + 10 - thicknessOfBorder), new Rectangle(0, 0, (int)(32 * Scale), thicknessOfBorder), Color.Black); //bottom
        }

    }
}
