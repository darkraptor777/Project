using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class nameSelector : MonoBehaviour
{
    // Start is called before the first frame update

    //defaults
    public string[] MinionDefault;
    public string[] WeaponDefault;
    public string[] SpellDefault;

    //Minion
    public string[] Taunt;
    public string[] Stealth;
    public string[] Charge;
    public string[] DivineShield;
    public string[] Windfury;
    public string[] SpellDmg;
    public string[] Overload;
    public string[] Elusive;
    public string[] Poisonous;
    public string[] Lifesteal;
    public string[] Reborn;
    public string[] Rush;
    public string[] Forgetful;
    

    //Weapon

    public string[] WeaponDamage;
    public string[] WeaponHeal;
    public string[] WeaponDestroy;
    public string[] WeaponSilence;
    public string[] WeaponFreeze;
    public string[] WeaponDiscard;
    public string[] WeaponDraw;
    public string[] WeaponArmour;
    public string[] WeaponMindControl;
    public string[] WeaponReturn;
    public string[] WeaponShuffle;


    //Spell

    public string[] SpellDamage;
    public string[] SpellHeal;
    public string[] SpellDestroy;
    public string[] SpellSilence;
    public string[] SpellFreeze;
    public string[] SpellDiscard;
    public string[] SpellDraw;
    public string[] SpellArmour;
    public string[] SpellMindControl;
    public string[] SpellReturn;
    public string[] SpellShuffle;

 

    //private Sprite Art;
    public string pickName(Sprite art, string type, string effect)
    {
        string name ="";

        if(type=="Minion")
        {
            if (effect == "Taunt") name += Taunt[Random.Range(0, Taunt.Length - 1)];
            else if (effect == "Stealth") name += Stealth[Random.Range(0, Stealth.Length - 1)];
            else if (effect == "Charge") name += Charge[Random.Range(0, Charge.Length - 1)];
            else if (effect == "DivineShield") name += DivineShield[Random.Range(0, DivineShield.Length - 1)];
            else if (effect == "Windfury") name += Windfury[Random.Range(0, Windfury.Length - 1)];
            else if (effect == "Spell Damage") name += SpellDmg[Random.Range(0, SpellDmg.Length - 1)];
            else if (effect == "Overload") name += Overload[Random.Range(0, Overload.Length - 1)];
            else if (effect == "Elusive") name += Elusive[Random.Range(0, Elusive.Length - 1)];
            else if (effect == "Poisonous") name += Poisonous[Random.Range(0, Poisonous.Length - 1)];
            else if (effect == "Lifesteal") name += Lifesteal[Random.Range(0, Lifesteal.Length - 1)];
            else if (effect == "Reborn") name += Reborn[Random.Range(0, Reborn.Length - 1)];
            else if (effect == "Rush") name += Rush[Random.Range(0, Rush.Length - 1)];
            else if (effect == "Forgetful") name += Forgetful[Random.Range(0, Forgetful.Length - 1)];
            else name += MinionDefault[Random.Range(0, MinionDefault.Length - 1)];
        }
        else if (type == "Weapon")
        {
            if (effect == "Damage") name += WeaponDamage[Random.Range(0, WeaponDamage.Length - 1)];
            else if (effect == "Heal") name += WeaponHeal[Random.Range(0, WeaponHeal.Length - 1)];
            else if (effect == "Destroy") name += WeaponDestroy[Random.Range(0, WeaponDestroy.Length - 1)];
            else if (effect == "Silence") name += WeaponSilence[Random.Range(0, WeaponSilence.Length - 1)];
            else if (effect == "Freeze") name += WeaponFreeze[Random.Range(0, WeaponFreeze.Length - 1)];
            else if (effect == "Discard") name += WeaponDiscard[Random.Range(0, WeaponDiscard.Length - 1)];
            else if (effect == "Draw") name += WeaponDraw[Random.Range(0, WeaponDraw.Length - 1)];
            else if (effect == "Armour") name += WeaponArmour[Random.Range(0, WeaponArmour.Length - 1)];
            else if (effect == "Mind Control") name += WeaponMindControl[Random.Range(0, WeaponMindControl.Length - 1)];
            else if (effect == "Return") name += WeaponReturn[Random.Range(0, WeaponReturn.Length - 1)];
            else if (effect == "Shuffle") name += WeaponShuffle[Random.Range(0, WeaponShuffle.Length - 1)];
            else name += WeaponDefault[Random.Range(0, WeaponDefault.Length - 1)];
        }
        else if (type == "Spell")
        {
            if (effect == "Damage") name += SpellDamage[Random.Range(0, SpellDamage.Length - 1)];
            else if (effect == "Heal") name += SpellHeal[Random.Range(0, SpellHeal.Length - 1)];
            else if (effect == "Destroy") name += SpellDestroy[Random.Range(0, SpellDestroy.Length - 1)];
            else if (effect == "Silence") name += SpellSilence[Random.Range(0, SpellSilence.Length - 1)];
            else if (effect == "Freeze") name += SpellFreeze[Random.Range(0, SpellFreeze.Length - 1)];
            else if (effect == "Discard") name += SpellDiscard[Random.Range(0, SpellDiscard.Length - 1)];
            else if (effect == "Draw") name += SpellDraw[Random.Range(0, SpellDraw.Length - 1)];
            else if (effect == "Armour") name += SpellArmour[Random.Range(0, SpellArmour.Length - 1)];
            else if (effect == "Mind Control") name += SpellMindControl[Random.Range(0, SpellMindControl.Length - 1)];
            else if (effect == "Return") name += SpellReturn[Random.Range(0, SpellReturn.Length - 1)];
            else if (effect == "Shuffle") name += SpellShuffle[Random.Range(0, SpellShuffle.Length - 1)];
            else name += SpellDefault[Random.Range(0, SpellDefault.Length - 1)];
        }

        name += " " + sanitizeName(art.name);
        //name.Replace()

        return name;
    }
    private string sanitizeName(string n)
    {
        string name = n;
        name = name.Replace("0", "");
        name = name.Replace("1", "");
        name = name.Replace("2", "");
        name = name.Replace("3", "");
        name = name.Replace("4", "");
        name = name.Replace("5", "");
        name = name.Replace("6", "");
        name = name.Replace("7", "");
        name = name.Replace("8", "");
        name = name.Replace("9", "");

        return name;
    }
}
