using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DarkEmpire
{
    public class WeaponInfo
    {
        public List<Weapon> wList = new List<Weapon>();

        public WeaponInfo()
        {
            wList.Add(new Weapon("Arming Sword",    2.00f,  1.2f,    2.40f, 3.00f,  36.00f, 1.50f));
            wList.Add(new Weapon("Viking Sword",    2.00f,  1.25f,	3.20f,  3.00f,  33.00f, 1.50f));
            wList.Add(new Weapon("Short Sword	",	2.50f,	0.70f,	2.40f,	4.00f,	31.00f,	2.00f));
            wList.Add(new Weapon("Rapier",	        2.50f,	1.10f,	2.40f,	4.00f,	36.00f,	2.50f));
            wList.Add(new Weapon("Sabre",	        2.00f,	1.70f,	2.40f,	3.00f,	38.00f,	1.50f));
            wList.Add(new Weapon("Artillery Sword",	2.00f,	0.80f,	2.40f,	3.00f,	19.00f,	1.50f));
            wList.Add(new Weapon("Long Sword",	    2.00f,	1.65f,	3.20f,	3.00f,	48.00f,	1.50f));
            wList.Add(new Weapon("Broadsword",	    2.00f,	1.20f,	2.40f,	3.00f,	37.00f,	1.50f));
            wList.Add(new Weapon("Claymore",	    2.00f,	3.10f,	3.20f,	3.00f,	45.00f,	1.50f));
            wList.Add(new Weapon("Zweihander",	    2.00f,	3.50f,	3.20f,	2.00f,	56.00f,	1.50f));
            wList.Add(new Weapon("Flamberge",	    1.50f,	3.25f,	3.20f,	2.00f,	50.00f,	1.50f));
            wList.Add(new Weapon("Backsword",	    2.00f,	1.15f,	2.40f,	3.00f,	33.00f,	1.50f));
            wList.Add(new Weapon("Katzbalger",	    2.00f,	1.10f,	2.40f,	4.00f,	25.00f,	1.50f));
            wList.Add(new Weapon("Falchion",	    2.00f,	1.00f,	3.20f,	3.00f,	31.00f,	1.50f));
            wList.Add(new Weapon("Cutlass",	        2.00f,	1.05f,	3.20f,	3.00f,	25.00f,	1.50f));
            wList.Add(new Weapon("Scimitar",	    2.50f,	0.75f,	3.20f,	3.00f,	31.00f,	1.50f));
            wList.Add(new Weapon("Baselard",	    2.00f,	0.30f,	2.40f,	4.00f,	9.00f,	1.50f));
            wList.Add(new Weapon("Cinquedea",	    2.00f,	1.30f,	2.40f,	3.00f,	18.00f,	2.50f));
            wList.Add(new Weapon("Ear Dagger",	    2.50f,	0.45f,	2.40f,	5.00f,	9.00f,	3.00f));
            wList.Add(new Weapon("Grosse Messer",	2.50f,	2.00f,	2.40f,	3.00f,	32.00f,	2.50f));
            wList.Add(new Weapon("Misericorde",	    2.50f,	0.25f,	2.40f,	5.00f,	12.00f,	2.50f));
            wList.Add(new Weapon("Rondel Dagger",	2.50f,	0.30f,	2.40f,	5.00f,	11.00f,	3.00f));
            wList.Add(new Weapon("Sgian",	        2.00f,	0.15f,	2.40f,	5.00f,	6.00f,	3.50f));
            wList.Add(new Weapon("Dirk",	        2.00f,	0.65f,	2.40f,	3.00f,	13.00f,	3.00f));
            wList.Add(new Weapon("Parrying Dagger",	1.50f,	0.45f,	2.40f,	5.00f,	13.00f,	2.00f));
            wList.Add(new Weapon("Seax",	        2.00f,	0.65f,	2.40f,	4.00f,	14.00f,	2.50f));
            wList.Add(new Weapon("Hungarian Axe",	1.50f,	1.55f,	3.20f,	1.00f,	34.00f,	1.50f));
            wList.Add(new Weapon("Bearded Axe",	    1.50f,	1.85f,	3.20f,	1.00f,	28.00f,	1.50f));
            wList.Add(new Weapon("Horseman's Axe",	1.50f,	1.55f,	4.00f,	1.00f,	17.00f,	1.50f));
            wList.Add(new Weapon("Mace",	        1.00f,	1.10f,	3.20f,	1.00f,	20.00f,	1.50f));
            wList.Add(new Weapon("Flanged Mace",	1.50f,	1.00f,	4.00f,	1.00f,	16.00f,	1.50f));
            wList.Add(new Weapon("Morning Star",	2.00f,	1.45f,	4.00f,	1.00f,	18.00f,	1.50f));
            wList.Add(new Weapon("Flail",	        2.00f,	1.50f,	4.00f,	1.00f,	32.00f,	1.50f));
            wList.Add(new Weapon("Shestopyor",	    1.50f,	2.00f,	3.20f,	1.00f,	18.00f,	1.50f));
            wList.Add(new Weapon("Hakapik",	        2.00f,	1.50f,	4.00f,	1.00f,	36.00f,	2.00f));
            wList.Add(new Weapon("War Hammer",	    1.00f,	1.15f,	3.20f,	1.00f,	18.00f,	1.50f));
            wList.Add(new Weapon("Flanged War Hammer",	1.50f,	1.15f,	4.00f,	1.00f,	22.00f,	1.50f));
            wList.Add(new Weapon("Horseman's Pick",	2.00f,	2.00f,	4.00f,	1.00f,	36.00f,	1.50f));
            wList.Add(new Weapon("Halberd",	        2.00f,	2.30f,	3.20f,	3.00f,	72.00f,	1.50f));
            wList.Add(new Weapon("Bardiche",	    2.00f,	3.40f,	3.20f,	2.00f,	53.00f,	1.50f));
            wList.Add(new Weapon("Spear",	        2.00f,	1.30f,	2.40f,	3.00f,	69.00f,	2.00f));
            wList.Add(new Weapon("Lance",	        1.00f,	3.10f,	3.20f,	2.00f,	85.00f,	2.00f));
            wList.Add(new Weapon("Glaive",	        2.00f,	3.00f,	3.20f,	3.00f,	77.00f,	1.50f));
            wList.Add(new Weapon("Lochaber Axe",	2.00f,	2.95f,	3.20f,	2.00f,	63.00f,	1.50f));
            wList.Add(new Weapon("Lucerne",	        1.50f,	4.60f,	4.00f,	1.00f,	79.00f,	1.50f));
            wList.Add(new Weapon("Becde Corbin",	2.00f,	2.40f,	3.20f,	1.00f,	59.00f,	1.50f));
            wList.Add(new Weapon("Partisan",	    2.00f,	2.40f,	3.20f,	3.00f,	73.00f,	1.50f));
            wList.Add(new Weapon("Pollaxe",	        2.00f,	3.25f,	4.00f,	3.00f,	70.00f,	1.50f));
            wList.Add(new Weapon("Bill",	        2.00f,	2.55f,	3.20f,	2.00f,	74.00f,	1.50f));
            wList.Add(new Weapon("Pike",	        2.00f,	5.00f,	3.20f,	1.00f,	120.00f,2.00f));
            wList.Add(new Weapon("Quarterstaff",	0.50f,	1.45f,	2.40f,	2.00f,	74.00f,	1.50f));
            wList.Add(new Weapon("Voulge",	        2.00f,	2.75f,	4.00f,	1.00f,	67.00f,	1.50f));

        }
    }
}
