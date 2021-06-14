using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class DNA
{
    private const float error_V = 10000.0f; //penalty applied for faulty genes
    private float Score = 0.0f;
    public int spliceNo = 4;
    public Gene type, cost, attack, health, tribe, rarity, keywordNo;
    //List structure: keyword, target, scope, allegiance, effect
    public List<Gene> KeywordFirst;
    public List<Gene> KeywordSecond;
    public List<Gene> KeywordThird;
    //private Gene keyTypeFirst, keyTypeSecond, keyTypeThird;
    private bool hasPenalty;

    public float total_V, cost_V;

    private float TypeRating, CostRating, RarityRating, KeyTypeRating, KeyRating, EffectRating, MagRating, StatsRating;

    private bool typeFaulty, costFaulty, attackFaulty, healthFaulty, tribeFaulty, rarityFaulty, keywordNoFaulty, KeywordFirstFaulty, KeywordSecondFaulty, KeywordThirdFaulty;
    //private List<bool> KeywordFirstFaults, KeywordSecondFaults, KeywordThirdFaults;

    private string Sequence; //should be length 

    private List<string> cardType = new List<string>() { "Minion", "Spell", "Weapon" }; //, Hero }; //quest is a type of spell

    private List<string> cardCost = new List<string>() { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" ,"10" };

    private List<string> cardAttack = new List<string>() { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15" };//, "16", "17", "18", "19", "20" };

    private List<string> cardHealth = new List<string>() { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15", "16" };//, "17", "18", "19", "20" };

    //private List<string> cardClass = new List<string>() { "Neutral", "Mage", "Warrior", "Priest", "Shaman", "Paladin", "Rogue", "Hunter", "Warlock", "Druid" };

    private List<string> cardRarity = new List<string>() { "Common", "Rare", "Epic", "Legendary" };

    private List<string> cardTribe = new List<string>() { "None", "Mech", "Beast", "Demon", "Pirate", "Elemental", "All" };

    private List<string> cardKeywordNo = new List<string>() { /*"0",*/ "1", "2", "3" };


    private List<string> keyTypes = new List<string>() { "Static", "Dynamic" }; //spell is not included as if the card is of cardType "Spell" then is only uses the spell list and ignores this gene list

    private List<string> StaticKeywords = new List<string>() { "Taunt", "Stealth", "Charge", "Divine Shield", "Windfury", "Spell Damage", "Overload", "Elusive", "Poisonous", "Lifesteal", "Reborn", "Rush", "Forgetful", "Mega-Windfury", "Can't Attack", "Cleave" }; //15

    private Dictionary<string, float> StaticCost = new Dictionary<string, float>() { { "Taunt", 0.2f }, { "Stealth", 1.0f }, { "Charge", 0.5f }, { "Divine Shield", 2.0f }, { "Windfury", 1.5f }, { "Spell Damage", 1.0f }, { "Overload", -1.0f }, { "Elusive", 1.0f }, { "Poisonous", 2.0f }, { "Lifesteal", 1.0f }, { "Reborn", 1.0f }, { "Rush", 0.3f }, { "Forgetful", -1.5f }, { "Mega-Windfury", 3.5f }, { "Can't Attack", -2.0f }, { "Cleave", 0.5f } };
    
    private List<string> DynamicKeywords = new List<string>() { "Battlecry", "Deathrattle", "Combo", "Battlecry And Deathrattle", /*"Choose One",*/ "End Of Turn", "Start Of Turn", "When Damaged", /*"Discover", "Passive",*/ "Discarded" }; //8

    private Dictionary<string, float> DynamicCost = new Dictionary<string, float>() { { "Battlecry", 1.0f }, { "Deathrattle", 0.8f }, { "Combo", 0.9f }, { "Battlecry And Deathrattle", 2.0f }, { "Choose One", 1.2f }, { "End Of Turn", 1.5f }, { "Start Of Turn", 1.2f }, { "When Damaged", 1.8f }, { "Discover", 1.0f }, { "Discarded", 0.9f }, { "Passive", 1.0f } }; //11

    private List<string> SpellKeywords = new List<string>() { "Spell", "Combo", /*"Choose One", "Discover"/*, "Secret"*/ }; //2

    private Dictionary<string, float> SpellCost = new Dictionary<string, float>() { { "Spell", 1.0f }, { "Combo", 0.9f }, { "Choose One", 1.5f }, { "Discover", 1.0f } }; 



    private List<string> Targets = new List<string>() { "Hero", "Minion", "Character", /*"Hand", "Deck",*/ "Weapon", /*"Spell",*/ "Self", /*"Neighbours"*/ };

    private Dictionary<string, float> TargetCost = new Dictionary<string, float>() { { "Hero", 1.0f }, { "Minion", 1.0f }, { "Character", 0.8f }, { "Hand", 1.5f }, { "Deck", 1.0f }, { "Weapon", 1.0f }, { "Spell", 1.0f }, { "Self", 1.0f }, { "Neighbours", 1.0f } };

    private List<string> Scope = new List<string>() { "All", "Single", "Double", "Triple" };

    private Dictionary<string, float> ScopeCost = new Dictionary<string, float>() { { "All", 2.5f }, { "Single", 1.0f }, { "Double", 1.6f }, { "Triple", 2.0f } };

    private List<string> Allegiance = new List<string>() { "Friendly", "Enemy", "Any" };

    private Dictionary<string, float> AllegianceCost = new Dictionary<string, float>() { { "Friendly", 1.0f }, { "Enemy", 1.0f }, { "Any", 1.1f }};

    private List<string> Effects = new List<string>() { "Damage", "Heal", "Destroy", "Silence", "Freeze", "Discard", "Draw", "Armour", "Mind Control", "Return", "Shuffle", "Summon", "Buff" };//13

    private Dictionary<string, float> EffectCost = new Dictionary<string, float>() { { "Damage", 0.6f }, { "Heal", 0.15f }, { "Destroy", 5.0f }, { "Silence", 2.0f }, { "Freeze", 0.8f }, { "Discard", -0.8f }, { "Draw", 1.5f }, { "Armour", 0.2f }, { "Mind Control", 10.0f }, { "Return", 2.0f }, { "Shuffle", 3.0f }, { "Summon", 0.40f }, { "Buff", 0.50f } };

    private List<string> Magnitude = new List<string>() { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15", "16" };

    //private List<string> Trigger = new List<string>() { "Damage" };

    //private Dictionary<string, float> TriggerCost = new Dictionary<string, float>() { { "Damage", 0.6f } };


    //Magnitude is it's own cost i.e. magnitude of 1 has a cost of 1

    //control/validation information
    //private bool hasDynamic; //whether the card has a dynamic keyword
    //private int keywordNo; //number of keywords

    public DNA()
    {
        hasPenalty = false;
        type = new Gene(cardType);
        cost = new Gene(cardCost);
        attack = new Gene(cardAttack);
        health = new Gene(cardHealth);
        tribe = new Gene(cardTribe);
        rarity = new Gene(cardRarity);
        keywordNo = new Gene(cardKeywordNo);
        //key type dictates whether to use static or dynamic keyword (spells always use spell keywords)
        KeywordFirst = new List<Gene>() { new Gene(keyTypes), new Gene(StaticKeywords), new Gene(DynamicKeywords), new Gene(SpellKeywords), new Gene(Targets), new Gene(Scope), new Gene(Allegiance), new Gene(Effects), new Gene(Magnitude), new Gene(Magnitude) }; 
        KeywordSecond = new List<Gene>() { new Gene(keyTypes), new Gene(StaticKeywords), new Gene(DynamicKeywords), new Gene(SpellKeywords), new Gene(Targets), new Gene(Scope), new Gene(Allegiance), new Gene(Effects), new Gene(Magnitude), new Gene(Magnitude) };
        KeywordThird = new List<Gene>() { new Gene(keyTypes), new Gene(StaticKeywords), new Gene(DynamicKeywords), new Gene(SpellKeywords), new Gene(Targets), new Gene(Scope), new Gene(Allegiance), new Gene(Effects), new Gene(Magnitude), new Gene(Magnitude) };
        assembleSequence();
    }

    public DNA(string sequence)
    {
        hasPenalty = false;
        KeywordFirst = new List<Gene>();
        KeywordSecond = new List<Gene>();
        KeywordThird = new List<Gene>();

        string g = "";
        int currentGene = 0;
        if(sequence.Length!=(37*4))
        {
            Debug.LogError("SEQUENCE WRONG LENGTH: EXPECTED "+(34*4).ToString()+" RECIEVED " + sequence.Length.ToString());
        }

        for (int b = 1; b <= sequence.Length; b++) //start at 1 so factors of 4 are always last part of gene
        {
            //currentGene = b / 4; //gen length is always 4 and casting to int always rounds down
            bool geneEnd = (b % 4 == 0 ? true : false);
            g += sequence[b-1];
            if (geneEnd)
            {
                switch (currentGene)
                {
                    case 0: //type
                        type = new Gene(cardType, g);
                        g = "";
                        break;

                    case 1: //cost
                        cost = new Gene(cardCost, g);
                        g = "";
                        break;

                    case 2: //attack
                        attack = new Gene(cardAttack, g);
                        g = "";
                        break;

                    case 3: //health
                        health = new Gene(cardHealth, g);
                        g = "";
                        break;

                    case 4: //rarity
                        rarity = new Gene(cardRarity, g);
                        g = "";
                        break;

                    case 5: //tribe
                        tribe = new Gene(cardTribe, g);
                        g = "";
                        break;

                    case 6: //keywordNo
                        keywordNo = new Gene(cardKeywordNo, g);
                        g = "";
                        break;

                    //KeywordFirst Stuff
                    case 7: //keyType
                        KeywordFirst.Add(new Gene(keyTypes, g));
                        g = "";
                        break;

                    case 8: //static keyword
                        KeywordFirst.Add( new Gene(StaticKeywords, g));
                        g = "";
                        break;

                    case 9: //dynamic keyword
                        KeywordFirst.Add(new Gene(DynamicKeywords, g));
                        g = "";
                        break;

                    case 10: //spell keyword
                        KeywordFirst.Add(new Gene(SpellKeywords, g));
                        g = "";
                        break;

                    case 11: //target
                        KeywordFirst.Add(new Gene(Targets, g));
                        g = "";
                        break;

                    case 12: //scope
                        KeywordFirst.Add(new Gene(Scope, g));
                        g = "";
                        break;

                    case 13: //allegiance
                        KeywordFirst.Add(new Gene(Allegiance, g));
                        g = "";
                        break;

                    case 14: //effect
                        KeywordFirst.Add(new Gene(Effects, g));
                        g = "";
                        break;

                    case 15: //magnitude
                        KeywordFirst.Add(new Gene(Magnitude, g));
                        g = "";
                        break;

                    case 16: //magnitude2
                        KeywordFirst.Add(new Gene(Magnitude, g));
                        g = "";
                        break;

                    //KeywordSecond Stuff
                    case 17: //keyType
                        KeywordSecond.Add(new Gene(keyTypes, g));
                        g = "";
                        break;

                    case 18: //static keyword
                        KeywordSecond.Add(new Gene(StaticKeywords, g));
                        g = "";
                        break;

                    case 19: //dynamic keyword
                        KeywordSecond.Add(new Gene(DynamicKeywords, g));
                        g = "";
                        break;

                    case 20: //spell keyword
                        KeywordSecond.Add(new Gene(SpellKeywords, g));
                        g = "";
                        break;

                    case 21: //target
                        KeywordSecond.Add(new Gene(Targets, g));
                        g = "";
                        break;

                    case 22: //scope
                        KeywordSecond.Add(new Gene(Scope, g));
                        g = "";
                        break;

                    case 23: //allegiance
                        KeywordSecond.Add(new Gene(Allegiance, g));
                        g = "";
                        break;

                    case 24: //effect
                        KeywordSecond.Add(new Gene(Effects, g));
                        g = "";
                        break;

                    case 25: //magnitude
                        KeywordSecond.Add(new Gene(Magnitude, g));
                        g = "";
                        break;

                    case 26: //magnitude2
                        KeywordSecond.Add(new Gene(Magnitude, g));
                        g = "";
                        break;

                    //KeywordThird Stuff
                    case 27: //keyType
                        KeywordThird.Add(new Gene(keyTypes, g));//0
                        g = "";
                        break;

                    case 28: //static keyword
                        KeywordThird.Add(new Gene(StaticKeywords, g));//1
                        g = "";
                        break;

                    case 29: //dynamic keyword
                        KeywordThird.Add(new Gene(DynamicKeywords, g));//2
                        g = "";
                        break;

                    case 30: //spell keyword
                        KeywordThird.Add(new Gene(SpellKeywords, g));//3
                        g = "";
                        break;

                    case 31: //target
                        KeywordThird.Add(new Gene(Targets, g));//4
                        g = "";
                        break;

                    case 32: //scope
                        KeywordThird.Add(new Gene(Scope, g));//5
                        g = "";
                        break;

                    case 33: //allegiance
                        KeywordThird.Add(new Gene(Allegiance, g));//6
                        g = "";
                        break;

                    case 34: //effect
                        KeywordThird.Add(new Gene(Effects, g));//7
                        g = "";
                        break;

                    case 35: //magnitude
                        KeywordThird.Add(new Gene(Magnitude, g));//8
                        g = "";
                        break;

                    case 36: //magnitude2
                        KeywordThird.Add(new Gene(Magnitude, g));//8
                        g = "";
                        break;
                }
                currentGene++;
            }
        }
        Sequence = sequence;
    }

    public string getSequence()
    {
        return Sequence;
    }

    public Gene getType()
    {
        return type;
    }

    public Gene getCost()
    {
        return cost;
    }

    public Gene getAttack()
    {
        return attack;
    }

    public Gene getHealth()
    {
        return health;
    }

    public Gene getTribe()
    {
        return tribe;
    }

    public Gene getRarity()
    {
        return rarity;
    }

    public Gene getKeywordNo()
    {
        return keywordNo;
    }

    public List<Gene> getKeywordFirst()
    {
        return KeywordFirst;
    }

    public List<Gene> getKeywordSecond()
    {
        return KeywordSecond;
    }

    public List<Gene> getKeywordThird()
    {
        return KeywordThird;
    }

    private string sanitizeKeyword(List<Gene> Keyword)
    {
        string k = "";
        

        if (Keyword[0].lookup() == "Static" && type.lookup() != "Spell")
        {
            k += Keyword[1].lookup();
            if (Keyword[1].lookup() == "Spell Damage") k += " +" + Keyword[8].lookup();
            else if (Keyword[1].lookup() == "Overload") k += ": (" + Keyword[8].lookup()+")";
        }
        else if (Keyword[0].lookup() == "Dynamic" || type.lookup() == "Spell")
        {
            if (type.lookup() == "Spell" && Keyword[3].lookup() != "Spell") k += Keyword[3].lookup() + ": "; //the keyword Spell does not need to be printed for card type spell
            else if (Keyword[0].lookup() == "Dynamic" && type.lookup() != "Spell") k += Keyword[2].lookup() + ": ";

            string bkw = checkBonusKey();//bonus keyword for summoned minion(s)/buffs

            //generate text based on effect first

            switch (Keyword[7].lookup())
            {
                case "Damage": //nice and compact if statements
                    k += "Deal " + Keyword[8].lookup() + " damage to ";
                    if (Keyword[4].lookup() == "Self") k += "this " + type.lookup() + ".";
                    else
                    {
                        if (Keyword[5].lookup() == "Single" && Keyword[6].lookup() == "Enemy") k += "an ";
                        else if (Keyword[5].lookup() == "Single") k += "a ";
                        else if (Keyword[5].lookup() == "Double") k += "2 random ";
                        else if (Keyword[5].lookup() == "Triple") k += "3 random ";
                        else if (Keyword[5].lookup() == "All") k += "all ";
                        //"Any" does not need text added
                        if (Keyword[6].lookup() == "Friendly") k += "friendly ";
                        else if (Keyword[6].lookup() == "Enemy") k += "enemy ";
                        k += Keyword[4].lookup().ToLower() + (Keyword[5].lookup() != "Single" && (Keyword[4].lookup() == "Hero") ? "e" : "") + ((Keyword[5].lookup() == "Single") ? "" : "s") + ".";
                    }

                    
                    break;

                case "Heal":
                    k += "Restore " + Keyword[8].lookup() + " health to ";
                    if (Keyword[4].lookup() == "Self") k += "this " + type.lookup() + ".";
                    else
                    {
                        if (Keyword[5].lookup() == "Single" && Keyword[6].lookup() == "Enemy") k += "an ";
                        else if (Keyword[5].lookup() == "Single") k += "a ";
                        else if (Keyword[5].lookup() == "Double") k += "2 random ";
                        else if (Keyword[5].lookup() == "Triple") k += "3 random ";
                        else if (Keyword[5].lookup() == "All") k += "all ";
                        //"Any" does not need text added
                        if (Keyword[6].lookup() == "Friendly") k += "friendly ";
                        else if (Keyword[6].lookup() == "Enemy") k += "enemy ";

                        k += Keyword[4].lookup().ToLower() + (Keyword[5].lookup() != "Single" && (Keyword[4].lookup() == "Hero") ? "e" : "") + ((Keyword[5].lookup() == "Single") ? "" : "s") + ".";
                    }
                    break;

                case "Destroy":
                    k += "Destroy ";
                    if (Keyword[4].lookup() == "Self") k += "this " + type.lookup() + ".";
                    else
                    {
                        if (Keyword[5].lookup() == "Single" && Keyword[6].lookup() == "Enemy") k += "an ";
                        else if (Keyword[5].lookup() == "Single") k += "a ";
                        else if (Keyword[5].lookup() == "Double") k += "2 random ";
                        else if (Keyword[5].lookup() == "Triple") k += "3 random ";
                        else if (Keyword[5].lookup() == "All") k += "all ";
                        //"Any" does not need text added
                        if (Keyword[6].lookup() == "Friendly") k += "friendly ";
                        else if (Keyword[6].lookup() == "Enemy") k += "enemy ";

                        k += Keyword[4].lookup().ToLower() + (Keyword[5].lookup() != "Single" && (Keyword[4].lookup() == "Hero") ? "e" : "") + ((Keyword[5].lookup() == "Single") ? "" : "s") + ".";
                    }
                    break;

                case "Silence":
                    k += "Silence ";
                    if (Keyword[4].lookup() == "Self") k += "this " + type.lookup() + ".";
                    else
                    {
                        if (Keyword[5].lookup() == "Single" && Keyword[6].lookup() == "Enemy") k += "an ";
                        else if (Keyword[5].lookup() == "Single") k += "a ";
                        else if (Keyword[5].lookup() == "Double") k += "2 random ";
                        else if (Keyword[5].lookup() == "Triple") k += "3 random ";
                        else if (Keyword[5].lookup() == "All") k += "all ";
                        //"Any" does not need text added
                        if (Keyword[6].lookup() == "Friendly") k += "friendly ";
                        else if (Keyword[6].lookup() == "Enemy") k += "enemy ";

                        k += Keyword[4].lookup().ToLower() + (Keyword[5].lookup() != "Single" && (Keyword[4].lookup() == "Hero") ? "e" : "") + ((Keyword[5].lookup() == "Single") ? "" : "s") + ".";
                    }
                    break;

                case "Freeze":
                    k += "Freeze ";
                    if (Keyword[4].lookup() == "Self") k += "this " + type.lookup() + ".";
                    else
                    {
                        if (Keyword[5].lookup() == "Single" && Keyword[6].lookup() == "Enemy") k += "an ";
                        else if (Keyword[5].lookup() == "Single") k += "a ";
                        else if (Keyword[5].lookup() == "Double") k += "2 random ";
                        else if (Keyword[5].lookup() == "Triple") k += "3 random ";
                        else if (Keyword[5].lookup() == "All") k += "all ";
                        //"Any" does not need text added
                        if (Keyword[6].lookup() == "Friendly") k += "friendly ";
                        else if (Keyword[6].lookup() == "Enemy") k += "enemy ";

                        k += Keyword[4].lookup().ToLower() + (Keyword[5].lookup() != "Single" && (Keyword[4].lookup() == "Hero") ? "e" : "") + ((Keyword[5].lookup() == "Single") ? "" : "s") + ".";
                    }
                    break;

                case "Discard":
                    k += "Discard " + (Keyword[8].lookup() == "1" ? "a" : Keyword[8].lookup()) + " random card" + ((Keyword[8].lookup() == "1") ? "" : "s") + ".";
                    break;

                case "Draw":
                    if (Keyword[6].lookup() == "Enemy") k += "Your opponent draws ";
                    else if (Keyword[6].lookup() == "Friendly") k += "Draw ";
                    else if (Keyword[6].lookup() == "Any") k += "Each player draws ";
                    k += (Keyword[8].lookup() == "1" ? "a" : Keyword[8].lookup()) + " card" + ((Keyword[8].lookup() == "1") ? "" : "s") + ".";
                    break;

                case "Armour":
                    k += "Gain " + Keyword[8].lookup() + " armour" + ".";
                    break;

                case "Mind Control":
                    if (Keyword[6].lookup() == "Friendly") k += "Give control of ";
                    else k += "Take control of ";
                    if (Keyword[4].lookup() == "Self") k += "this " + type.lookup() + ".";
                    else
                    {
                        if (Keyword[5].lookup() == "Single" && Keyword[6].lookup() == "Enemy") k += "an ";
                        else if (Keyword[5].lookup() == "Single") k += "a ";
                        else if (Keyword[5].lookup() == "Double") k += "2 random ";
                        else if (Keyword[5].lookup() == "Triple") k += "3 random ";
                        else if (Keyword[5].lookup() == "All") k += "all ";
                        //"Any" does not need text added
                        if (Keyword[6].lookup() == "Friendly") k += "friendly ";
                        else if (Keyword[6].lookup() == "Enemy") k += "enemy ";

                        k += Keyword[4].lookup().ToLower() + (Keyword[5].lookup() != "Single" && (Keyword[4].lookup() == "Hero") ? "e" : "") + ((Keyword[5].lookup() == "Single") ? "" : "s") + ".";
                    }
                    break;

                case "Return":
                    k += "Return ";
                    if (Keyword[4].lookup() == "Self") k += "this " + type.lookup() + " to your hand.";
                    else
                    {
                        if (Keyword[5].lookup() == "Single" && Keyword[6].lookup() == "Enemy") k += "an ";
                        else if (Keyword[5].lookup() == "Single") k += "a ";
                        else if (Keyword[5].lookup() == "Double") k += "2 random ";
                        else if (Keyword[5].lookup() == "Triple") k += "3 random ";
                        else if (Keyword[5].lookup() == "All") k += "all ";
                        //"Any" does not need text added
                        if (Keyword[6].lookup() == "Friendly") k += "friendly ";
                        else if (Keyword[6].lookup() == "Enemy") k += "enemy ";

                        k += Keyword[4].lookup().ToLower() + (Keyword[5].lookup() != "Single" && (Keyword[4].lookup() == "Hero") ? "e" : "") + ((Keyword[5].lookup() == "Single") ? "" : "s") + " to ";
                        if (Keyword[6].lookup() == "Any") k += "their owner's ";
                        else if (Keyword[6].lookup() == "Friendly") k += "your ";
                        else if (Keyword[6].lookup() == "Enemy") k += "your opponent's ";
                        k += "hand.";

                    }
                    break;

                case "Shuffle":
                    k += "Shuffle ";
                    if (Keyword[4].lookup() == "Self") k += "this " + type.lookup() + " into your deck.";
                    else
                    {
                        if (Keyword[5].lookup() == "Single" && Keyword[6].lookup() == "Enemy") k += "an ";
                        else if (Keyword[5].lookup() == "Single") k += "a ";
                        else if (Keyword[5].lookup() == "Double") k += "2 random ";
                        else if (Keyword[5].lookup() == "Triple") k += "3 random ";
                        else if (Keyword[5].lookup() == "All") k += "all ";
                        //"Any" does not need text added
                        if (Keyword[6].lookup() == "Friendly") k += "friendly ";
                        else if (Keyword[6].lookup() == "Enemy") k += "enemy ";

                        k += Keyword[4].lookup().ToLower() + (Keyword[5].lookup() != "Single" && (Keyword[4].lookup() == "Hero") ? "e" : "") + ((Keyword[5].lookup() == "Single") ? "" : "s") + " into ";
                        if (Keyword[6].lookup() == "Any") k += "their owner's ";
                        else if (Keyword[6].lookup() == "Friendly") k += "your ";
                        else if (Keyword[6].lookup() == "Enemy") k += "your opponent's ";
                        k += "deck.";

                    }
                    break;

                case "Summon":
                    if (Keyword[5].lookup() == "All")
                    {
                        k += "Fill ";
                        if (Keyword[6].lookup() == "Friendly") k += "your ";
                        if (Keyword[6].lookup() == "Enemy") k += "your opponents ";
                        if (Keyword[6].lookup() == "Any") k += "both players ";
                        k += "board with " + Keyword[8].lookup() + "/" + Keyword[9].lookup() + " minion" + ((Keyword[5].lookup() == "Single") ? "" : "s");
                        
                    }
                    else
                    {
                        k += "Summon ";
                        if (Keyword[5].lookup() == "Single") k += "a ";
                        else if (Keyword[5].lookup() == "Double") k += "two ";
                        else if (Keyword[5].lookup() == "Triple") k += "three ";
                        k += Keyword[8].lookup() + "/" + Keyword[9].lookup() + " minion" + ((Keyword[5].lookup() == "Single") ? "" : "s"); 
                        if (bkw != "") k+=" with "+ bkw;
                        if (Keyword[6].lookup() == "Enemy") k += " for your opponent";
                        else if (Keyword[6].lookup() == "Any") k += " for both players";
                    }
                    
                    k += ".";
                    break;

                case "Buff":
                    k += "Give ";
                    if (Keyword[5].lookup() == "Single") k += "a ";
                    else if (Keyword[5].lookup() == "Double") k += "two random ";
                    else if (Keyword[5].lookup() == "Triple") k += "three random ";
                    else if (Keyword[5].lookup() == "All") k += "all ";

                    if (Keyword[6].lookup() == "Friendly") k += "friendly ";
                    else if (Keyword[6].lookup() == "Enemy") k += "enemy ";

                    k += "minion"+ ((Keyword[5].lookup() == "Single") ? "" : "s") + " +" + Keyword[8].lookup() + "/+" + Keyword[9].lookup();
                    if (bkw != "") k += " and " + bkw;
                    k += ".";
                    break;

            }

        }


        return k;
    }

    private string checkBonusKey()
    {
        int keyNo;
        string key = "";

        try
        {
            keyNo = int.Parse(keywordNo.lookup());
        }
        catch
        {
            keyNo = 0;
        }

        if (keyNo == 1)
        {
            if(KeywordSecond[0].lookup()=="Static")
            {
                key = KeywordSecond[1].lookup();
            }
            else if (KeywordThird[0].lookup() == "Static")
            {
                key = KeywordThird[1].lookup();
            }
        }
        else if (keyNo==2)
        {
            if (KeywordThird[0].lookup() == "Static")
            {
                key = KeywordThird[1].lookup();
            }
        }


        return key;
    }

    public string lookupKeywordFirst(bool sanitize = false)
    {
        string k = "";
        if(sanitize)
        {
            k = sanitizeKeyword(KeywordFirst);

        }
        else 
        {
            for (int i = 0; i < KeywordFirst.Count; i++)
            {
                k += KeywordFirst[i].lookup() + " ";
            }
        }
        return k;
    }

    public string lookupKeywordSecond(bool sanitize = false)
    {
        string k = "";
        if (sanitize)
        {
            k = sanitizeKeyword(KeywordSecond);

        }
        else
        {
            for (int i = 0; i < KeywordSecond.Count; i++)
            {
                k += KeywordSecond[i].lookup() + " ";
            }
        }
        return k;
    }

    public string lookupKeywordThird(bool sanitize = false)
    {
        string k = "";
        if (sanitize)
        {
            k = sanitizeKeyword(KeywordThird);

        }
        else
        {
            
            for (int i = 0; i < KeywordThird.Count; i++)
            {
                k += KeywordThird[i].lookup() + " ";
            }
        }
        return k; 
    }

    public float getScore()
    {
        return Score;
    }

    public void alterScore(float f) //+=
    {
        Score += f;
    }

    public void setScore(float f)
    {
        Score = f;
    }

    public void assembleSequence()
    {
        Sequence = type.getGene().ToString() + cost.getGene().ToString() + attack.getGene().ToString() + health.getGene().ToString() + rarity.getGene().ToString() + tribe.getGene().ToString() + keywordNo.getGene().ToString();
        Sequence += assembleKeywordSeq(KeywordFirst);
        Sequence += assembleKeywordSeq(KeywordSecond);
        Sequence += assembleKeywordSeq(KeywordThird);
    }

    public string assembleKeywordSeq(List<Gene> kw)
    {
        //textSeq = (string)keyword + (string)target + (string)scope + (string)allegiance + (string)effect;
        string seq = "";
        for(int i=0; i<kw.Count; i++)
        {
            seq += kw[i].getGene().ToString();
        }
        //Debug.Log(kw.Count.ToString() + " " + seq);
        return seq;
    }

    private bool checkDuplicateKeywords()
    {
        //bool duplicates = false;
        int keyNo = (!keywordNoFaulty) ? -int.Parse(keywordNo.lookup()) : 0; //if the keyword number is faulty assume 0 keywords
        if (keyNo>1)
        {
            if(KeywordFirst[0].lookup()== KeywordSecond[0].lookup()) //if keyword type is the same check if the keyword is the same
            {
                if(KeywordFirst[0].lookup() =="Static" && KeywordFirst[1].lookup() == KeywordSecond[1].lookup())
                {
                    return true;
                }
                else if (KeywordFirst[0].lookup() == "Dynamic" && KeywordFirst[2].lookup() == KeywordSecond[2].lookup())
                {
                    return true;
                }
            }



            if(keyNo > 2)
            {
                if (KeywordFirst[0].lookup() == KeywordThird[0].lookup()) //if keyword type is the same check if the keyword is the same
                {
                    if (KeywordFirst[0].lookup() == "Static" && KeywordFirst[1].lookup() == KeywordThird[1].lookup())
                    {
                        return true;
                    }
                    else if (KeywordFirst[0].lookup() == "Dynamic" && KeywordFirst[2].lookup() == KeywordThird[2].lookup())
                    {
                        return true;
                    }
                }

                if (KeywordThird[0].lookup() == KeywordSecond[0].lookup()) //if keyword type is the same check if the keyword is the same
                {
                    if (KeywordThird[0].lookup() == "Static" && KeywordThird[1].lookup() == KeywordSecond[1].lookup())
                    {
                        return true;
                    }
                    else if (KeywordThird[0].lookup() == "Dynamic" && KeywordThird[2].lookup() == KeywordSecond[2].lookup())
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }

    public string breed(string dna)
    {
        List<int> splicePoints = new List<int>();
        string otherSEQ = dna;

        if (otherSEQ.Length != Sequence.Length)
        {
            Debug.LogError("ERROR: DNA SEQUENCE LENGTHS DO NOT MATCH " + Sequence.Length.ToString() + " " + otherSEQ.Length.ToString());
            Debug.LogError(Sequence);
            Debug.LogError(otherSEQ);
        }

        for (int i=0; i<spliceNo; i++) //pick splice locations
        {
            bool valid = false;
            int count = 0;
            int point=0;
            while (!valid)
            {
                if (splicePoints == null) valid = true;
                point = UnityEngine.Random.Range(0, otherSEQ.Length-1);
                if (!splicePoints.Contains(point)) //make sure its not already been selected
                {
                    valid = true;
                }
                count++;
                if(count>=100) break;  //prevent infinite loop edgecase
            }
            splicePoints.Add(point);
        }
        splicePoints.Add(Sequence.Length);
        //Debug.Log("Splice Locations Picked");

        splicePoints.Sort(); //sort points so they're iterated through in the right order
        string newSEQ = "";
        int spliceIndex = 0; //index for splice points list
        bool useOther = false;
        for (int b =0; b< Sequence.Length;b++)
        {
            try
            {
                if (b > splicePoints[spliceIndex])
                {
                    spliceIndex += 1;
                    useOther = !useOther;
                }
                if (b <= splicePoints[spliceIndex])
                {
                    if (useOther)
                    {
                        newSEQ += otherSEQ[b];
                    }
                    else
                    {
                        newSEQ += Sequence[b];
                    }
                }

                else
                {
                    Debug.LogError("ERROR: DNA ASSEMBLY ERROR");
                }
            }
            catch 
            {
                Debug.Log("spliceIndex: " + spliceIndex.ToString());
                Debug.Log("splicePoints Length: " + splicePoints.Count.ToString());
                Debug.Log("b: " + b.ToString());
                Debug.Log("otherSEQ Length: " + otherSEQ.Length.ToString());
                Debug.Log("Sequence Length: " + Sequence.Length.ToString());
            }
        }
        //Debug.Log("Spliced Sequence Reassembled");
        //Debug.Log("SEQ: "+Sequence.Length.ToString()+" oth: "+ otherSEQ.Length.ToString()+" new: "+ newSEQ.Length.ToString());
        return newSEQ;
    }

    public void Mutate()
    {
        type.Mutate();
        cost.Mutate();
        attack.Mutate();
        health.Mutate();
        tribe.Mutate();
        rarity.Mutate();
        keywordNo.Mutate();
        //key type dictates whether to use static or dynamic keyword (spells always use spell keywords)
        for(int i =0; i<KeywordFirst.Count;i++)
        {
            
            try
            {
                KeywordFirst[i].Mutate();
                KeywordSecond[i].Mutate();
                KeywordThird[i].Mutate();
            }
            catch (ArgumentOutOfRangeException e)
            {
                Debug.Log(i.ToString());
                Debug.Log("First: " + KeywordFirst.Count.ToString() + " Second: " + KeywordSecond.Count.ToString() + " Third: " + KeywordThird.Count.ToString());
                Debug.Log(e);
            }

        }
        assembleSequence();
    }

    public void evaluate() //evaluates DNA and assigns score
    {
        hasPenalty = false;
        checkFaulty(); //check to see if any genes are faulty (undefined binary strings) and sets appropriate vars

        if (isFaulty()) //if the type is faulty the card is useless
        {
            Score = error_V; //give really high score (high score is bad)
            hasPenalty = true;
        }
        else
        {

            float statCost = 0.5f;
            //if (type.lookup() == "Weapon") statCost = 0.5f;

            bool spell = false;
            if (type.lookup() == "Spell") spell = true;


            

            //if it's not faulty lookup the value, else use error value
            cost_V = ((!costFaulty) ? int.Parse(cost.lookup()) : 0.0f);
            //check to see if dynamic effect is negative and also subtract from the value

            float attack_V = 0.0f;
            float health_V = 0.0f;

            if (spell != true)
            {
                attack_V = (!attackFaulty) ? int.Parse(attack.lookup()) : 0.0f;
                health_V = (!healthFaulty) ? int.Parse(health.lookup()) : 0.0f;
            }

            float stats_V = 0.0f;
            if(type.lookup()=="Minion") stats_V = (attack_V + health_V) * statCost;
            else if (type.lookup() == "Weapon") stats_V = (attack_V * health_V) * statCost;

            float tribe_V = (tribe.lookup() != "None") ? 0.25f : 0.0f; //doesn't matter if faulty as faulty is not equal to no tribe
            tribe_V = (tribe.lookup() == "All") ? 0.5f : tribe_V; 

            float keyOne_V = 0.0f;
            float keyTwo_V = 0.0f;
            float keyThree_V = 0.0f;
            //some assumptions can be made in the place of faulty genes
            int keyNo = (!keywordNoFaulty) ? int.Parse(keywordNo.lookup()) : 0; //if the keyword number is faulty assume 0 keywords
            if (keyNo >= 1) //only gather value if it's being used 
            {
                if (!KeywordFirstFaulty) keyOne_V = evalKeyword(KeywordFirst, 1, spell);
                else if (KeywordFirstFaulty) keyOne_V = 0.0f; //hasPenalty = true;
                if(!hasPenalty) hasPenalty = evalPenalty(KeywordFirst);
            }
            
            if (keyNo >= 2)
            {
                if (!KeywordSecondFaulty) keyTwo_V = evalKeyword(KeywordSecond, 2, spell);
                else if (KeywordSecondFaulty) keyTwo_V = 0.0f; //hasPenalty = true;
                if (!hasPenalty) hasPenalty = evalPenalty(KeywordSecond);
            }
            if (keyNo >= 3)
            {
                if (!KeywordThirdFaulty) keyThree_V = evalKeyword(KeywordThird, 3, spell);
                else if (KeywordThirdFaulty) keyThree_V = 0.0f; //hasPenalty = true;
                if (!hasPenalty) hasPenalty = evalPenalty(KeywordThird);
            }
            total_V = stats_V + tribe_V + keyOne_V + keyTwo_V + keyThree_V;

            if (checkDuplicateKeywords() && !hasPenalty) hasPenalty = true;
            if (type.lookup() == "Spell" && keywordNo.lookup() == "0" && !hasPenalty) hasPenalty = true;

            
            if(hasPenalty)
            {
                Score = error_V;
            }
            
            else
            {
                //Score = Math.Abs(1.0f - (total_V / (cost_V /** 2.0f*/))); //Scores converging towards 1 are the best scores so difference between them and 1 serves as score
                Score = total_V - cost_V;
            }

            
            //Score = Math.Abs(((cost_V * 2.0f) - total_V)); 

            
        }
    }

    private float evalKeyword(List<Gene> key, int Number, bool isSpell=false)
    {
        //number refers to whether this this the first, second or third keyword, not the number of keywords being used
        float score = 0.0f;
        //Keyword { keyTypes, StaticKeywords, DynamicKeywords, SpellKeywords, Targets, Scope, Allegiance, Effects, Magnitude };
        string cardType = type.lookup();
        string kWordType = key[0].lookup();
        string kWord ="";
        string target = key[4].lookup();
        string scope = key[5].lookup();
        string allegiance = key[6].lookup();
        string effect = key[7].lookup();
        int magnitude = int.Parse(key[8].lookup());
        int magnitude2 = int.Parse(key[9].lookup());
        bool positive = isPositive(effect);

        if (kWordType == "Static") //if keyword is static
        {
            kWord = key[1].lookup();
                
            if (kWord == "Spell Damage" || kWord == "Overload") //these static keywords are influenced by magnitude
            {
                if (kWord == "Spell Damage" && magnitude>4)
                {
                    score += StaticCost[kWord] * (magnitude*(magnitude/2.0f));
                }
                else
                {
                    score += StaticCost[kWord] * magnitude;//+= since polarity is stored in dictionary
                }
                if (kWord == "Overload" && magnitude > 10)
                {
                    hasPenalty = true; //can't overload more than 10 mana crystals (players have a max of 10 mana crystals)
                }
            }
            else if (kWord == "Charge" || kWord == "Rush" || kWord=="Cleave")
            {
                score += StaticCost[kWord] * float.Parse(attack.lookup());
            }
            else if (kWord == "Taunt")
            {
                score += StaticCost[kWord] * float.Parse(health.lookup());
            }
            else
            {
                score += StaticCost[kWord];
            }

            


            return score;
        }
        else if (kWordType == "Dynamic" || isSpell) //if keyword is dynamic
        {
            if (kWordType == "Dynamic" && !isSpell)
            {
                kWord = key[2].lookup();
            }
            else if (isSpell)
            {
                kWord = key[3].lookup();
            }

            if(effect=="Draw")
            {
                score += EffectCost[effect] * (float) Math.Pow(magnitude, 1.5);
                //if (magnitude > 1) positive = true; //forcing the enemy to draw multiple cards can be a benefit (see coldlight oracle)
            }
            else if(effect=="Summon")
            {
                score += (magnitude+magnitude2) * EffectCost[effect];
            }
            else if (effect == "Buff")
            {
                score += (magnitude + magnitude2) * EffectCost[effect];
            }
            else if (effect == "Damage" || effect == "Heal" || effect == "Armour" || effect == "Discard")
            {
                score += EffectCost[effect] * magnitude;
            }
            else
            {
                score += EffectCost[effect];
            }
            if(effect == "Buff" || effect == "Summon")
            {
                string bkw = checkBonusKey();//bonus keyword for summoned minion(s)/buffs
                if (bkw!="")
                {
                    if (bkw == "Charge" || bkw == "Rush")
                    {
                        score += StaticCost[bkw] * float.Parse(attack.lookup());
                    }
                    else 
                    { 
                        score += StaticCost[bkw];
                    }
                }
            }

            if(effect!="Draw" || effect != "Armour" || effect != "Discard" || effect != "Summon" || effect != "Buff")
            {
                score *= TargetCost[target];
            }
            

            if(target!="Self" && effect != "Draw" && effect != "Armour" && effect != "Discard") score *= ScopeCost[scope];

            if(isSpell)
            {
                score *= SpellCost[kWord];
            }
            else
            {
                score *= DynamicCost[kWord];
            }

            if (target == "Self") allegiance = "Friendly";

            if (((positive && allegiance=="Enemy") || (!positive && allegiance=="Friendly")) && (kWord!="Discarded" || effect != "Armour" || effect != "Draw")) //minions with negative discard effects shouldn't benefit from lower score and forcing enemy to draw cards can be beneficial (mill decks)
            {
                score *= -1.0f; //inverts score
            }

           
        }
        

        return score;
    }

    public bool penalty() { return hasPenalty; }
    private bool evalPenalty(List<Gene> key)//nonsensical text punishment
    {
        string cardType = type.lookup();
        string kWordType = key[0].lookup();
        string kWord = "";
        string target = key[4].lookup();
        string scope = key[5].lookup();
        string allegiance = key[6].lookup();
        string effect = key[7].lookup();
        int magnitude = int.Parse(key[8].lookup());
        bool positive = isPositive(effect);
        int hp = int.Parse(health.lookup());
        int atk = int.Parse(attack.lookup());
        int keyNo = int.Parse(keywordNo.lookup());

        if (cardType=="Spell")
        {
            kWord = key[3].lookup();
        }
        else if (kWordType == "Static") //if keyword is static
        {
            kWord = key[1].lookup();
        }
        else if (kWordType == "Dynamic") //if keyword is static
        {
            kWord = key[2].lookup();
        }

        bool p = false;

        if (effect == "Heal" && (target == "Weapon" || (target == "Self" && cardType == "Weapon") || (target == "Self" && (kWord == "Deathrattle" || kWord == "Battlecry and Deathrattle")))) p = true; //can't heal weapons and can't heal itself after death
        else if (effect == "Damage" && (target == "Weapon" || (target == "Self" && cardType == "Weapon") || (target == "Self" && (kWord == "Deathrattle" || kWord == "Battlecry and Deathrattle")))) p = true; //can't damage weapons and can't damage itself after death
        else if (effect == "Destroy" && (target == "Character" || target == "Hero")) p = true; //destroying heroes way to powerful (results in instant loss/victory)
        else if (effect == "Return" && (target == "Character" || target == "Hero")) p = true; //cant return heroes to hand
        else if (effect == "Shuffle" && (target == "Character" || target == "Hero")) p = true; //cant shuffle heroes into deck
        else if (cardType == "Spell" && target == "Self") p = true; //spell cant target itself
        else if ((target == "Hero" || target == "Weapon") && (scope == "Triple" || ((scope == "Double" || scope == "All") && (allegiance == "Friendly" || allegiance == "Enemy")))) p = true; //can't target 3 heroes/weapons or multiple friendly or enemy heroes/weapons
        else if (effect == "Damage" && target == "Self" && magnitude >= hp) p = true; //can't deal amount of damage to itself that would kill it
        else if ((effect == "Discard" || effect == "Draw") && magnitude > 10) p = true; //max hand size is 10
        else if ((target == "Hero" || target == "Character") && (effect == "Silence" || effect == "Mind Control")) p = true; //cannot silence or mind control heroes
        else if (target == "Weapon" && effect == "Freeze") p = true; //cannot freeze weapons (you freeze heroes to prevent them attacking with weapons
        else if (target == "Self" && (effect == "Silence" || effect == "Mind Control")) p = true; //cannot silence or mind control itself
        else if (allegiance == "Enemy" && effect == "Buff") p = true; //buffing enemy minions too tricky to balance (buff might not go off if there are no enemy minions which could result in minion being too strong but is also really bad if there are enemy minions)
        else if (cardType == "Spell" && kWord == "Combo" && keyNo == 1) p = true; //spell cant have only combo effect
        else if (cardType == "Spell" && !positive && keyNo == 1) p = true; //spell cant have only a negative effect
        else if (kWord == "Discarded" && target == "Self") p = true; //can't target itself after being discarded (same as death)
        else if (cardType == "Weapon" && atk == 0) p = true; //weapons can't deal 0 damage
        else if (cardType == "Weapon" && (kWord == "Charge" || kWord == "Rush" || kWord == "Stealth")) p = true;
        return p;
    }
    public bool isPositive(string key)
    {
        //positive is things you want to happen to you
        //negative is things you want to happen to your opponent
        List<string> EffectsPositive = new List<string>() { "Heal", "Draw", "Armour", "Silence", "Buff", "Summon" };
        List<string> EffectsNegative = new List<string>() { "Damage", "Destroy", "Freeze", "Discard", "Mind Control", "Return", "Shuffle" };

        if(EffectsPositive.Contains(key))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void setRatings(float cost, float rarity, float type, float keytype, float key, float eff, float mag, float stats)
    {
        CostRating = cost;
        RarityRating = rarity;
        TypeRating = type;
        KeyTypeRating = keytype;
        KeyRating = key;
        EffectRating = eff;
        MagRating = mag;
        StatsRating = stats;
    }

    public float getRatingCost() { return CostRating; }
    public float getRatingRarity() { return RarityRating; }
    public float getRatingType() { return TypeRating; }

    public float getRatingKeyType() { return KeyTypeRating; }

    public float getRatingKey() { return KeyRating; }

    public float getRatingEffect() { return EffectRating; }

    public float getRatingMag() { return MagRating; }

    public float getRatingStats() { return StatsRating; }

    public void checkFaulty() //general check if any are faulty
    {
        typeFaulty = checkFaulty(type);
        costFaulty = checkFaulty(cost);
        attackFaulty = checkFaulty(attack);
        healthFaulty = checkFaulty(health);
        tribeFaulty = checkFaulty(tribe);
        rarityFaulty = checkFaulty(rarity);
        keywordNoFaulty = checkFaulty(keywordNo);

        KeywordFirstFaulty = checkFaulty(KeywordFirst);
        KeywordSecondFaulty = checkFaulty(KeywordSecond);
        KeywordThirdFaulty = checkFaulty(KeywordThird);
    }
    private bool checkFaulty(Gene g) //checks single gene
    {
        if (g.lookup() == "NA") return true;
        else return false;
    }
    private bool checkFaulty(List<Gene> gL) //checks gene list
    {
        //bool faulty = false;
        for (int i = 0; i < gL.Count; i++)
        {
            if(checkFaulty(gL[i]))
            {
                return true; //if it finds a fault the keyword is faulty
            }
        }
        return false; //if no faults are found the keyword is not faulty
    }

    private List<bool> getFaults(List<Gene> gL) //checks gene list
    {
        List<bool> faulty = new List<bool>();
        for (int i = 0; i < gL.Count; i++)
        {
            faulty.Add(checkFaulty(gL[i]));
        }

        return faulty;
    }

    public bool isFaulty() //checks if there are ANY faults
    {
        if(typeFaulty == false && costFaulty == false && attackFaulty == false && healthFaulty == false && tribeFaulty == false && rarityFaulty == false && keywordNoFaulty == false && KeywordFirstFaulty == false && KeywordSecondFaulty == false && KeywordThirdFaulty == false)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
}

public class Gene
{
    private Dictionary<string, string> ToBin;// = Compile_Dictionary(Compile_Keywords("Static"));
    private Dictionary<string, string> ToStr;// = Compile_Dictionary_Reverse(Compile_Keywords("Static"));
    public string text;
    private float mutateChance = 0.001f;
    private string gene;
    private int length = 4;

    public Gene(List<string> types, int size = 4)
    {
        ToBin = Compile_Dictionary(types);
        ToStr = Compile_Dictionary_Reverse(types);
        gene = ToBin[types[UnityEngine.Random.Range(0, types.Count)]];
        length = size;
        text = lookup();
    }


    public Gene(List<string> types, string code, int size = 4)
    {
        ToBin = Compile_Dictionary(types);
        ToStr = Compile_Dictionary_Reverse(types);
        gene = code;
        length = size;
        text = lookup();
    }


    Dictionary<string, string> Compile_Dictionary(List<string> S)
    {
        Dictionary<string, string> D = new Dictionary<string, string>();
        for (int i = 0; i < S.Count; i++)
        {
            D.Add(S[i], ConvertIntToBin(i, length));
        }
        return D;
    }

    Dictionary<string, string> Compile_Dictionary_Reverse(List<string> S)
    {
        Dictionary<string, string> D = new Dictionary<string, string>();
        for (int i = 0; i < S.Count; i++)
        {
            D.Add(ConvertIntToBin(i, length), S[i]);
        }
        return D;
    }

    public string ConvertIntToBin(int integer, int bits)
    {
        List<int> c = new List<int>();
        int maxBit = (int)Math.Pow(2, (bits - 1));
        int currentBit = maxBit;
        int integerTemp = integer;
        string binary = "";
        //Debug.Log("Bits: " + bits);
        for (int i = 1; i <= bits; i++)
        {
            if (currentBit <= integerTemp)
            {
                binary += "1";
                integerTemp -= currentBit;
            }
            else
            {
                binary += "0";
            }
            //Debug.Log("Binary: "+ binary);
            currentBit /= 2;

        }
        /*
        int bin = 0;
        try
        {
            bin = Convert.ToInt32(binary);
        }

        catch (FormatException e)
        {
            Debug.Log(e);
            Debug.Log(binary);
            Debug.Log(integer);
        }
        */

        return binary;
    }

    public void Mutate()
    {
        string g = gene.ToString();
        string newGene = "";
        for(int i =0; i< g.Length; i++)
        {
            if(UnityEngine.Random.Range(0.0f,1.0f)<=mutateChance)
            {
                if(g[i]=='0')
                {
                    newGene += '1';
                }
                else if(g[i]=='1')
                {
                    newGene += '0';
                }
                else
                {
                    Debug.Log("ERROR: NON-BINARY VALUE IN GENE SEQUENCE");
                }
            }
            else
            {
                newGene += g[i];
            }
        }
        gene = newGene;
    }

    public string getGene()
    {
        return gene;
    }


    public string lookup()
    {
        if(ToStr.ContainsKey(gene))
        {
            return ToStr[gene];
        }
        else
        {
            return "NA"; //Not Assigned
        }
    }

    public void Override(string s) //rewrites gene contents
    {
        gene = ToBin[s];
        text = lookup();
    }

    /*
    void BuildDictionaries()
    {
        StaticToBinary = Compile_Dictionary(Compile_Keywords("Static"));
        BinaryToStatic = Compile_Dictionary_Reverse(Compile_Keywords("Static"));

        DynamicToBinary = Compile_Dictionary(Compile_Keywords("Dynamic"));
        BinaryToDynamic = Compile_Dictionary_Reverse(Compile_Keywords("Dynamic"));

        SpellToBinary = Compile_Dictionary(Compile_Keywords("Spell"));
        BinaryToSpell = Compile_Dictionary_Reverse(Compile_Keywords("Spell"));

    }
    */
}