using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cardDescriptor : MonoBehaviour
{
    public enum cardType { Minion, Spell, Weapon, Hero }; //quest is a type of spell
    public cardType type;
    public enum cardClass { Neutral, Mage, Warrior, Priest, Shaman, Paladin, Rogue, Hunter, Warlock, Druid };
    public cardClass classType;
    public enum cardRarity { Common, Rare, Epic, Legendary };
    public cardRarity rarity;
    public enum cardTribe { None, Mech, Beast, Dragon, Elemental };
    public cardTribe tribe;
    public int manaCost;
    public string text;
    public string cardName;
    public Sprite art;
    public float value; //refers to how "good" the card is considered to be
    public int attack;
    public int maxHealth;
    //need a vector for storing modifiers (eg. additional health/damage/additional text)
    public string ID; //is generated based on all card attributes so each card has a unique id it can be generated from if needed

    
    // Start is called before the first frame update
    void Start()
    {

    }                            

    // Update is called once per frame
    void Update()
    {
        
    }
}
