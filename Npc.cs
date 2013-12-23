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
        public Vector2 position;
        public bool powerup = false;
        public int frame;
        public float Scale;

        public Npc(int ID, int dFace, Vector2 pos, float scale = 1.0f)
        {
            Scale = scale;
            frame = 1;
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
    }
}
