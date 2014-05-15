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
        public Rectangle rect, battleRect;
        public int characterID;
        public int directionFace;
        public int frame, battleFrame;
        public int partyPosition;
        public float scale;
        public String name;
        public Vector2 position;
        public bool powerup = false;
        public static Texture2D pixel = new Texture2D(Game1.instance.GraphicsDevice, 1, 1); //create 1x1 pixel texture
        SpriteBatch spriteBatch = Game1.instance.SpriteBatch;

        public Stats stat = new Stats();

        public Npc(int ID, int dFace, Vector2 pos, float s = 1.0f, int roleId = 0)
        {
            stat.setRole(roleId);
            scale = s;
            frame = 1;
            battleFrame = 1;
            position = pos;
            directionFace = dFace;
            characterID = ID;
            partyPosition = 1;
            name = "dummy";
            rect = new Rectangle((characterID - 1) % 4 * 180 + (frame) * 60, characterID / 5 * 240 + (directionFace - 1) % 4 * 60, 60, 60); //npc_sprite
            battleRect = new Rectangle((battleFrame-1) * 100 , (characterID-1) * 100, 100, 100); //battle_sprite
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

        public void changeBattleStance()
        {
            battleRect = new Rectangle(battleFrame * 100, (characterID - 1) * 100, 100, 100); //battle_sprit
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
            spriteBatch.Draw(pixel, new Vector2(position.X, position.Y - 3.2f * scale), new Rectangle(0, 0, (int)(32 * scale * stat.HealthPercent() ), 10), Color.Green);
            spriteBatch.Draw(pixel, new Vector2(position.X + 32 * scale * stat.HealthPercent(), position.Y - 3.2f * scale), new Rectangle(0, 0, (int)(32 * scale * (1.0f - stat.health / stat.MaxHealth)), 10), Color.Red);

            spriteBatch.Draw(pixel, new Vector2(position.X, position.Y - 3.2f * scale), new Rectangle(0, 0, (int)(32 * scale), thicknessOfBorder), Color.Black); //top
            spriteBatch.Draw(pixel, new Vector2(position.X, position.Y - 3.2f * scale), new Rectangle(0, 0, thicknessOfBorder, 10), Color.Black); //left
            spriteBatch.Draw(pixel, new Vector2(position.X + 32 * scale - thicknessOfBorder, position.Y - 3.2f * scale), new Rectangle(0, 0, thicknessOfBorder, 10), Color.Black); //right
            spriteBatch.Draw(pixel, new Vector2(position.X, position.Y - 3.2f * scale + 10 - thicknessOfBorder), new Rectangle(0, 0, (int)(32 * scale), thicknessOfBorder), Color.Black); //bottom
        }
    }
}
