using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DarkEmpire
{
    public class Weapon
    {
        public string Name;
        public float Sharpness;
        public float Inertia;
        public float Hardness;
        public float Stability;
        public float Reach;
        public float Critical;
        public float Tier;

        public Weapon()
        {

        }

        public Weapon(string n, float s, float i, float h, float st, float r, float c, int t = 1)
        {
            Tier = t; 
            Name = n;
            Sharpness = s;
            Inertia = i;
            Hardness = h;
            Stability = st;
            Reach = r;
            Critical = c;
        }        
    }
}
