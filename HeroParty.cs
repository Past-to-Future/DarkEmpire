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
        public static Npc[] theEnemy;
        SpriteBatch spriteBatch = Game1.instance.SpriteBatch;

        public HeroParty()
        {
            theHero = new Npc[5];
            theHero[0] = new Npc(1, 3, new Vector2(Game1.instance.Width * 0.2f, Game1.instance.Height * .2f), 1.0f);
            theHero[1] = new Npc(2, 3, new Vector2(Game1.instance.Width * 0.1f, Game1.instance.Height * .4f), 1.0f);
            theHero[2] = new Npc(3, 3, new Vector2(Game1.instance.Width * 0.2f, Game1.instance.Height * .6f), 1.0f);

            theHero[0].setPartyPosition(0);
            theHero[1].setPartyPosition(1);
            theHero[2].setPartyPosition(2);

            theHero[0].name = "Maxum";
            theHero[1].name = "Aurelia";
            theHero[2].name = "Jasmine";

            theEnemy = new Npc[5];
            theEnemy[0] = new Npc(5, 2, new Vector2(Game1.instance.Width * 0.6f, Game1.instance.Height * .3f), 1.0f);
            theEnemy[1] = new Npc(6, 2, new Vector2(Game1.instance.Width * 0.6f, Game1.instance.Height * .5f), 1.0f);
            theEnemy[2] = new Npc(7, 2, new Vector2(Game1.instance.Width * 0.7f, Game1.instance.Height * .2f), 1.0f);
            theEnemy[3] = new Npc(5, 2, new Vector2(Game1.instance.Width * 0.7f, Game1.instance.Height * .4f), 1.0f);
            theEnemy[4] = new Npc(6, 2, new Vector2(Game1.instance.Width * 0.7f, Game1.instance.Height * .6f), 1.0f);

            theEnemy[0].setPartyPosition(0);
            theEnemy[1].setPartyPosition(1);
            theEnemy[2].setPartyPosition(2);
            theEnemy[3].setPartyPosition(3);
            theEnemy[4].setPartyPosition(4);

            theEnemy[0].name = "Evil Villian";
            theEnemy[1].name = "Evil Enemy";
            theEnemy[2].name = "Dark Enemy";
            theEnemy[3].name = "Dark Villian";
            theEnemy[4].name = "Good Guy";
        }



        public static void swap(int hero1, int hero2)
        {
            Npc dummy = theHero[hero1-1];
            theHero[hero1-1] = theHero[hero2-1];
            theHero[hero2-1] = dummy;
        }
    }
}
