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
        public static Npc[] theHero;
        Npc[] theEnemy;
        SpriteBatch spriteBatch = Game1.instance.SpriteBatch;

        public HeroParty()
        {
            theHero = new Npc[5];
            theHero[0] = new Npc(1, 3, new Vector2(Game1.instance.Width * 0.2f, Game1.instance.Height * .1f), 5.0f);
            theHero[1] = new Npc(2, 3, new Vector2(Game1.instance.Width * 0.3f, Game1.instance.Height * .3f), 5.0f);
            theHero[2] = new Npc(3, 3, new Vector2(Game1.instance.Width * 0.1f, Game1.instance.Height * .5f), 5.0f);
 
            theHero[0].health = 0.25f;
            theHero[1].health = 0.5f;
            theHero[2].health = 0.75f;

            theHero[0].setPartyPosition(0);
            theHero[1].setPartyPosition(1);
            theHero[2].setPartyPosition(2);

            theHero[0].name = "The Hero";
            theHero[1].name = "The Hero: Warrior";
            theHero[2].name = "The Hero: Wizard from the North";

            theEnemy = new Npc[5];
            theEnemy[0] = new Npc(5, 2, new Vector2(Game1.instance.Width * 0.7f, Game1.instance.Height * .1f), 5.0f);
            theEnemy[1] = new Npc(6, 2, new Vector2(Game1.instance.Width * 0.6f, Game1.instance.Height * .3f), 5.0f);
            theEnemy[2] = new Npc(7, 2, new Vector2(Game1.instance.Width * 0.8f, Game1.instance.Height * .5f), 5.0f);

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
            for (int i = 0; i < 3; i++)
            {
                spriteBatch.Draw(PlayingState.instance.npcSprite, theHero[i].position, theHero[i].rect, Color.White, 0.0f, Vector2.Zero, theHero[i].scale, SpriteEffects.None, 0.0f);
                spriteBatch.Draw(PlayingState.instance.npcSprite, theEnemy[i].position, theEnemy[i].rect, Color.White, 0.0f, Vector2.Zero, theEnemy[i].scale, SpriteEffects.None, 0.0f);

                theHero[i].DrawHealthAboveWithOutline();
                theHero[i].DrawHealthCornerLeftWithOutline();
                theEnemy[i].DrawHealthAboveWithOutline();
                theEnemy[i].DrawHealthCornerWithOutline();
            }
        }

        public void swap(Npc hero1, Npc hero2)
        {

        }
    }
}
