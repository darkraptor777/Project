using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtSelector : MonoBehaviour
{
    public Sprite[] Buildings;
    public Sprite[] Locations;
    public Sprite[] Weapons;
    public Sprite[] Staffs;
    public Sprite[] Armours;
    public Sprite[] Foods;
    public Sprite[] Potions;
    public Sprite[] Items;
    public Sprite[] Demons;
    public Sprite[] Beasts;
    public Sprite[] Aquatics;
    public Sprite[] Monsters;
    public Sprite[] Mechs;
    public Sprite[] Undeads;
    public Sprite[] Mages;
    public Sprite[] Soldiers;
    public Sprite[] Pirates;
    public Sprite[] Citizens;
    public Sprite[] Elementals;
    public Sprite[] OffensiveSpells;
    public Sprite[] DefensiveSpells;
    public Sprite[] MiscSpells;

    public Sprite Default;


    public Sprite pickArt(string type, string tribe)
    {
        Sprite art= Default;
        if(type=="Spell")
        {
            int r = Random.Range(0, 7);
            switch(r)
            {
                case (0):
                    art = Buildings[Random.Range(0, Buildings.Length - 1)];
                    break;
                case (1):
                    art = Locations[Random.Range(0, Locations.Length - 1)];
                    break;
                case (2):
                    art = Foods[Random.Range(0, Foods.Length - 1)];
                    break;
                case (3):
                    art = Potions[Random.Range(0, Potions.Length - 1)];
                    break;
                case (4):
                    art = Items[Random.Range(0, Items.Length - 1)];
                    break;
                case (5):
                    art = OffensiveSpells[Random.Range(0, OffensiveSpells.Length - 1)];
                    break;
                case (6):
                    art = DefensiveSpells[Random.Range(0, DefensiveSpells.Length - 1)];
                    break;
                case (7):
                    art = MiscSpells[Random.Range(0, MiscSpells.Length - 1)];
                    break;
            }
        }
        else if(type=="Weapon")
        {
            int r = Random.Range(0, 3);
            switch (r)
            {
                case (0):
                    art = Weapons[Random.Range(0, Weapons.Length - 1)];
                    break;
                case (1):
                    art = Staffs[Random.Range(0, Staffs.Length - 1)];
                    break;
                case (2):
                    art = Armours[Random.Range(0, Armours.Length - 1)];
                    break;
                case (3):
                    art = Items[Random.Range(0, Items.Length - 1)];
                    break;
            }
        }
        else if(type=="Minion")
        {
            if (tribe == "Mech")
            {
                art = Mechs[Random.Range(0, Mechs.Length - 1)];
            }
            else if (tribe == "Beast")
            {
                art = Beasts[Random.Range(0, Beasts.Length - 1)];
            }
            else if (tribe == "Demon")
            {
                art = Demons[Random.Range(0, Demons.Length - 1)];
            }
            else if (tribe == "Pirate")
            {
                art = Pirates[Random.Range(0, Pirates.Length - 1)];
            }
            else if (tribe == "Elemental")
            {
                art = Elementals[Random.Range(0, Elementals.Length - 1)];
            }

            else
            {
                int r = Random.Range(0, 5);
                switch(r)
                {
                    case (0):
                        art = Aquatics[Random.Range(0, Aquatics.Length - 1)];
                        break;
                    case (1):
                        art = Monsters[Random.Range(0, Monsters.Length - 1)];
                        break;
                    case (2):
                        art = Undeads[Random.Range(0, Undeads.Length - 1)];
                        break;
                    case (3):
                        art = Mages[Random.Range(0, Mages.Length - 1)];
                        break;
                    case (4):
                        art = Soldiers[Random.Range(0, Soldiers.Length - 1)];
                        break;
                    case (5):
                        art = Citizens[Random.Range(0, Citizens.Length - 1)];
                        break;
 
                }
            }
        }
        return art;
    }
}
