using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class cardDescriptor : MonoBehaviour
{
    public GameObject Designer;

    public int manaCost;
    public string type;
    public string tribe;
    public string rarity;
    public string text;
    public string cardName;
    public Sprite art;
    //public float value; //refers to how "good" the card is considered to be
    public int attack;
    public int maxHealth;
    public int keywordNo;
    //need a vector for storing modifiers (eg. additional health/damage/additional text)
    public string ID; //is generated based on all card attributes so each card has a unique id it can be generated from if needed
    public float total_V;
    public float cost_V;
    public float OGBalance;
    public float Balance;
    public float CostRating;
    public float RarityRating;
    public float TypeRating;
    public float KeyTypeRating;
    public float KeyRating;
    public float EffectRating;
    public float MagRating;
    private GameObject Childtext;
    private GameObject Childart;
    private GameObject Childtokens;
    public bool hasPenalty;
    public DNA info;

    public Sprite[] borders;

    private void grabChildren()
    {
        Childtext = transform.GetChild(3).gameObject;
        Childart = transform.GetChild(2).gameObject;
        Childtokens = transform.GetChild(0).gameObject;
    }

    private void grabDesigner()
    {
        Designer = GameObject.Find("Designer");
    }

    public void Load(DNA cardInfo)
    {
        info = cardInfo;
        try
        {
            manaCost = Convert.ToInt32(cardInfo.getCost().lookup());
        }
        catch(FormatException e)
        {
            manaCost = -1;
        }
        try
        {
            attack = Convert.ToInt32(cardInfo.getAttack().lookup());
        }
        catch (FormatException e)
        {
            attack = -1;
        }
        try
        {
            maxHealth = Convert.ToInt32(cardInfo.getHealth().lookup());
        }
        catch (FormatException e)
        {
            maxHealth = -1;
        }
        try
        {
            keywordNo = Convert.ToInt32(cardInfo.getKeywordNo().lookup());
        }
        catch (FormatException e)
        {
            keywordNo = 0;
        }

        type = cardInfo.getType().lookup();
        tribe = cardInfo.getTribe().lookup();
        rarity = cardInfo.getRarity().lookup();
        ID = cardInfo.getSequence();
        text = ReadText(keywordNo, cardInfo.lookupKeywordFirst(true), cardInfo.lookupKeywordSecond(true), cardInfo.lookupKeywordThird(true));
        total_V = cardInfo.total_V;
        cost_V = cardInfo.cost_V;
        Balance = cardInfo.getScore();
        CostRating = cardInfo.getRatingCost();
        RarityRating = cardInfo.getRatingRarity();
        TypeRating = cardInfo.getRatingType();
        KeyTypeRating = cardInfo.getRatingKeyType();
        KeyRating = cardInfo.getRatingKey();
        EffectRating = cardInfo.getRatingEffect();
        MagRating = cardInfo.getRatingMag();
        hasPenalty = cardInfo.penalty();
        OGBalance = total_V - cost_V;

        grabChildren();
        grabDesigner();
        grabArt();
        grabName(cardInfo);

        Childtext.transform.GetChild(0).GetComponent<TextMesh>().text = attack.ToString();
        Childtext.transform.GetChild(1).GetComponent<TextMesh>().text = maxHealth.ToString();
        Childtext.transform.GetChild(3).GetComponent<TextMesh>().text = manaCost.ToString();
        Childtext.transform.GetChild(4).GetComponent<TextMeshPro>().text = text;
        Childtext.transform.GetChild(5).GetComponent<TextMesh>().text = (tribe=="None") ? "":tribe;

        if(type=="Minion"||type=="Weapon")
        {
            Childtext.transform.GetChild(0).gameObject.SetActive(true);
            Childtext.transform.GetChild(1).gameObject.SetActive(true);
            if(type=="Minion") Childtext.transform.GetChild(5).gameObject.SetActive(true);

            Childtokens.transform.GetChild(3).gameObject.SetActive(true);
            if (type == "Minion") Childtokens.transform.GetChild(4).gameObject.SetActive(true);
            else if (type == "Weapon") Childtokens.transform.GetChild(5).gameObject.SetActive(true);
        }
        else
        {
            Childtext.transform.GetChild(0).gameObject.SetActive(false);
            Childtext.transform.GetChild(1).gameObject.SetActive(false);
            Childtext.transform.GetChild(5).gameObject.SetActive(false);

            Childtokens.transform.GetChild(3).gameObject.SetActive(false);
            Childtokens.transform.GetChild(4).gameObject.SetActive(false);
            Childtokens.transform.GetChild(5).gameObject.SetActive(false);
        }
    }

    private string ReadText(int no, string first, string second, string third)
    {
        string text = "";
        if(no>=1)
        {
            text += first;
            if(no>=2)
            {
                text += "\n"+second;
                if(no>=3)
                {
                    text += "\n"+third;
                }
            }
        }
        return text;
    }


    private void grabArt()
    {
        art=Designer.GetComponent<ArtSelector>().pickArt(type, tribe);
        Childart.GetComponent<SpriteRenderer>().sprite = art;

        if (rarity == "Common") transform.GetChild(0).GetChild(2).gameObject.GetComponent<SpriteRenderer>().sprite = borders[0];
        else if (rarity == "Rare") transform.GetChild(0).GetChild(2).gameObject.GetComponent<SpriteRenderer>().sprite = borders[1];
        else if (rarity == "Epic") transform.GetChild(0).GetChild(2).gameObject.GetComponent<SpriteRenderer>().sprite = borders[2];
        else if (rarity == "Legendary") transform.GetChild(0).GetChild(2).gameObject.GetComponent<SpriteRenderer>().sprite = borders[3];

    }
    private void grabName(DNA cardinfo)
    {
        string effect="";
        if (keywordNo == 0) effect = "None";
        else
        {
            int r = UnityEngine.Random.Range(0, keywordNo - 1);
            if(r==0) effect = getEffect(cardinfo.getKeywordFirst(), (type == "Spell"));
            if (r == 1) effect = getEffect(cardinfo.getKeywordSecond(), (type == "Spell"));
            if (r == 2) effect = getEffect(cardinfo.getKeywordThird(), (type == "Spell"));
        }
        cardName = Designer.GetComponent<nameSelector>().pickName(art, type, effect);
        Childtext.transform.GetChild(2).GetComponent<TextMesh>().text = cardName;
    }
    private string getEffect(List<Gene> key, bool isSpell)
    {
        string effect="";
        if(isSpell || key[0].lookup() == "Dynamic")
        {
            effect = key[7].lookup();
        }
        else if (key[0].lookup() == "Static")
        {
            effect = key[1].lookup();
        }
        


        return effect;
    }
}
