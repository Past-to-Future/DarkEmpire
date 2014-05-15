using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DarkEmpire
{
    public class Stats
    {
        public int Strength = 1;
        public int Vitality = 1;
        public int Perception = 1;
        public int Agility = 1;
        public int Intelligence = 1;
        public int Charisma = 1;
        public int AP = 1;
        public int Level = 1;
        public int XP = 1;
        public float health;

        public float HealthPercent()
        {
            return health / MaxHealth; 
        }

        public float XPPercent()
        {
            return XP / 200f * (float)Math.Pow(1.5f, Level - 1); 
        }

        public float XPtoLvl()
        {
            return 200f * (float)Math.Pow(1.5f, Level - 1); 
        }

        public float MaxHealth
        {
            get { return Level * (7 + 2 * Vitality); }
        }

        public void setRole(float roleId)
        {
            if (roleId == 0)//scrub role
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

            else if (roleId == 1) //cheater role
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

            health = Level * (7 + 2 * Vitality); //using this like a percentage
        }
    }
}
