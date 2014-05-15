using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DarkEmpire
{
    public class Item
    {
        public string Name;
        public float Value;
        public float Quantity;
        public float Power;
        public String description;
        public Item()
        {

        }

        public Item(string n, float v, float q, float p, String d)
        {
            Name = n;
            Value = v;
            Quantity = q;
            Power = p;
            description = d;
        }       
    }
}
