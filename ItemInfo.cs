using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DarkEmpire
{
    public class ItemInfo
    {
        public List<Item> iList = new List<Item>();

        public ItemInfo()
        {
            iList.Add(new Item("Potion", 100.00f, 1, 10.00f, "Heals 10 hp"));
            iList.Add(new Item("Good Potion", 150.00f, 1, 25.00f, "Heals 25 hp"));
            iList.Add(new Item("Super Potion", 200.00f, 1, 50.00f, "Heals 50 hp"));
            iList.Add(new Item("Ultra Potion", 250.00f, 1, 100.00f, "Heals 100 hp"));
            iList.Add(new Item("Antidote", 50.00f, 1, 0.00f, "Cures Status Ailment"));
        }

        public int FindItemNumber(String itemName)
        {
            for(int i = 0; i < iList.Count; i++)
            {
                if(iList[i].Name == itemName)
                {
                    return i;
                }
            }
            return -1; //lol incoming crash
        }
    }
}
