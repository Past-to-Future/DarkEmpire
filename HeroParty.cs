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
    public class HeroParty
    {
        Npc[] theHero;
        Npc[] theEnemy;

        public HeroParty()
        {
            theHero = new Npc[5];
            theHero[0] = new Npc(1, 3, new Vector2(Game1.screenWidth * 0.2f, Game1.screenHeight * .1f), 5.0f);
            theHero[1] = new Npc(2, 3, new Vector2(Game1.screenWidth * 0.3f, Game1.screenHeight * .3f), 5.0f);
            theHero[2] = new Npc(3, 3, new Vector2(Game1.screenWidth * 0.1f, Game1.screenHeight * .5f), 5.0f);
 
            theHero[0].health = 0.25f;
            theHero[1].health = 0.5f;
            theHero[2].health = 0.75f;

            theHero[0].setPartyPosition(0);
            theHero[1].setPartyPosition(1);
            theHero[2].setPartyPosition(2);

            theHero[0].name = "Sir Francis of Normandy";
            theHero[1].name = "Gandalf the White";
            theHero[2].name = "Frosty the Snowman";

            theEnemy = new Npc[5];
            theEnemy[0] = new Npc(5, 2, new Vector2(Game1.screenWidth * 0.7f, Game1.screenHeight * .1f), 5.0f);
            theEnemy[1] = new Npc(6, 2, new Vector2(Game1.screenWidth * 0.6f, Game1.screenHeight * .3f), 5.0f);
            theEnemy[2] = new Npc(7, 2, new Vector2(Game1.screenWidth * 0.8f, Game1.screenHeight * .5f), 5.0f);

            theEnemy[0].health = 0.05f;
            theEnemy[1].health = 0.15f;
            theEnemy[2].health = 0.35f;

            theEnemy[0].setPartyPosition(0);
            theEnemy[1].setPartyPosition(1);
            theEnemy[2].setPartyPosition(2);

            theEnemy[0].name = "Darth Vader";
            theEnemy[1].name = "Sir Evilsalot";
            theEnemy[2].name = "Team Rocket";
        }

        public void draw()
        {
            SpriteBatch spriteBatch = Game1.spriteBatch;
            for (int i = 0; i < 3; i++)
            {
                spriteBatch.Draw(Game1.npcSprite, theHero[i].position, theHero[i].rect, Color.White, 0.0f, Vector2.Zero, theHero[i].scale, SpriteEffects.None, 0.0f);
                spriteBatch.Draw(Game1.npcSprite, theEnemy[i].position, theEnemy[i].rect, Color.White, 0.0f, Vector2.Zero, theEnemy[i].scale, SpriteEffects.None, 0.0f);
                // theHero[i].health = (float)rand.NextDouble(); //lets see the health change in real time...
                theHero[i].DrawHealthAboveWithOutline();
                theHero[i].DrawHealthCornerLeftWithOutline();
                theEnemy[i].DrawHealthAboveWithOutline();
                theEnemy[i].DrawHealthCornerWithOutline();
            }
        }
    }
}
