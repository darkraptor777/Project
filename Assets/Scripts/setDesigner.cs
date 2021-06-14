using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;
using System.Diagnostics;
using Debug = UnityEngine.Debug;

//can flavour set towards certain keyword/feature by disabling the similarity penatly when scoring a card design

public class setDesigner : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject GenButton;
    public GameObject NextButton;
    public GameObject Q1;
    public GameObject Q2;
    public GameObject End;
    public GameObject ID;
    public GameObject cardPrefab;
    private GameObject reporter;
    public Set cardSet = null;
    public int Epochs = 1;
    public int setSize = 135;
    public const int reviewSetSize = 20;
    List<int> reviewCards;
    List<GameObject> Cards;
    List<Feedback> reviews;
    private int CCI = 0; //current card index

    public Stopwatch timer;

    void Start()
    {
        Cards = new List<GameObject>();
        reviews = new List<Feedback>();
        //cardSet = new Set(setSize);
        grabReporter();
        reviewCards = new List<int>();
        timer = new Stopwatch();
        
    }

    void grabReporter()
    {
        reporter = GameObject.Find("Reporter");
    }

    private void SelectReviewCards()
    {
        for(int i=0; i< reviewSetSize; i++)
        {
            bool valid = false;
            while(!valid)
            {
                int r = UnityEngine.Random.Range(0, setSize - 1);
                if(!cardSet.getCard(r).penalty())
                {
                    if(i==0)//no need to check for duplicates for first entry
                    {
                        reviewCards.Add(r);
                        valid = true;
                    }
                    else
                    {
                        if(!reviewCards.Contains(r))
                        {
                            reviewCards.Add(r);
                            valid = true;
                        }
                    }
                }
            }
        }
    }

    public void Generate()
    {
        //Test();
        
        CCI = 0;
        Cards = new List<GameObject>();
        timer.Start();
        cardSet = new Set(setSize);
        cardSet.Generate(Epochs);
        timer.Stop();
        Debug.Log("Time Elapsed During Generation: " + timer.ElapsedMilliseconds.ToString());
        //SelectReviewCards();
        //VisualiseReviewSet();
        Visualise();
        sendReport();
        reviews.Add(new Feedback(Cards[CCI].GetComponent<cardDescriptor>().info.getSequence()));
        GenButton.SetActive(false);
        //Q1.SetActive(true);
        
    }

    public void Test()
    {
        List<int> EpochsList = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 15, 20, 25, 30, 40, 50, 75, 100, 150, 200, 250 };
        List<int> SetSizeList = new List<int>() { 30, 45, 135, 245 };
        int repeats = 20;
        List<List<float>> MeanTime = new List<List<float>>();
        List<List<float>> MeanAverageBalance = new List<List<float>>();
        List<List<float>> MeanPenalised = new List<List<float>>();
        for (int k = 0; k < SetSizeList.Count; k++)
        {
            setSize = SetSizeList[k];
            MeanTime.Add(new List<float>());
            MeanAverageBalance.Add(new List<float>());
            MeanPenalised.Add(new List<float>());
            for (int i = 0; i < EpochsList.Count; i++)
            {
                float TotalTime = 0.0f;
                float TotalAverageBalance = 0.0f;
                float TotalPenalised = 0.0f;
                int c = 0;
                while(c<repeats) //have to use while loop so completely faulty sets can be ingored at low set sizes
                {
                    timer.Reset();
                    timer.Start();
                    cardSet = new Set(setSize);
                    cardSet.Generate(EpochsList[i]);
                    timer.Stop();
                    sendReport();
                    float avg = getAVGRating(); 
                    if (!float.IsNaN(avg)) //gets NaN if set contains no non-faulty/penalised cards
                    {
                        TotalTime += timer.ElapsedMilliseconds;
                        TotalAverageBalance += avg;
                        TotalPenalised += cardSet.getFaultyCount();
                        c += 1;
                    }
                }
                MeanTime[k].Add(TotalTime / repeats);
                MeanAverageBalance[k].Add(TotalAverageBalance / repeats);
                MeanPenalised[k].Add(TotalPenalised / repeats);
                Debug.LogError("Set Size: " + SetSizeList[k].ToString() + " Epochs: " + EpochsList[i].ToString() + " Time: " + MeanTime[k][i].ToString() + " Balance: " + MeanAverageBalance[k][i].ToString() + " Penalised: " + MeanPenalised[k][i].ToString());
            }
        }
        //Debug.Log("Time Elapsed During Generation: " + timer.ElapsedMilliseconds.ToString());

    }


    public void SubmitReview()
    {
        for(int i =0; i<reviewSetSize; i++)
        {
            reviews[i].writeReport();
        }
    }

    public void Reset()
    {
        for (int i = reviewSetSize - 1; i < 0; i--)
        {
            Destroy(Cards[i]);
        }
        End.SetActive(false);
        GenButton.SetActive(true);
        //Destroy(Cards);
        
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void Next()
    {
        Cards[CCI].SetActive(false);
        CCI += 1;
        //if (Cards[CCI].GetComponent<cardDescriptor>().info.hasPenalty) Next(); //skip penalised/faulty cards
        if(CCI>=reviewSetSize)
        {
            SubmitReview();
            NextButton.SetActive(false);
            End.SetActive(true);
        }
            
        else
        {
            reviews.Add(new Feedback(Cards[CCI].GetComponent<cardDescriptor>().info.getSequence()));
            NextButton.SetActive(false);
            Q1.SetActive(true);
        }
    }

    public void q1Answer(string Answer)
    {
        Debug.Log("Reading Q1 Answer");
        Q1.SetActive(false);
        Q2.SetActive(true);
        reviews[CCI].Q1(Answer);
        Q2.transform.GetChild(0).GetComponent<UnityEngine.UI.Text>().text = "Pie estimates the cost of this card should be adjusted by about "+Math.Floor((double)Cards[CCI].GetComponent<cardDescriptor>().OGBalance).ToString()+"~"+ Math.Ceiling((double)Cards[CCI].GetComponent<cardDescriptor>().OGBalance).ToString() + " mana. How much would you adjust it by?";
    }
    public void submitID()
    {
        Debug.Log("Reading ID");
        string id = ID.transform.GetChild(2).GetComponent<InputField>().text;
        Feedback.writeID(id);
        ID.SetActive(false);
        GenButton.SetActive(true);
    }
    public void q2Answer()
    {
        Debug.Log("Reading Q2 Answer");
        string q2Answer="";
        float q2f=0;
        bool isValid = false;
        try
        {
            q2Answer = Q2.transform.GetChild(2).GetComponent<InputField>().text;
            q2f = float.Parse(q2Answer);
            isValid = true;
        }
        catch
        {
            if(!Q2.transform.GetChild(3).gameObject.activeInHierarchy) //if warning text not already active
            {
                Q2.transform.GetChild(3).gameObject.SetActive(true); //make it active
            }
        }
        if (isValid)
        {
            Q2.transform.GetChild(3).gameObject.SetActive(false);
            Q2.SetActive(false);
            NextButton.SetActive(true);
            string est = Math.Floor(Cards[CCI].GetComponent<cardDescriptor>().OGBalance).ToString() + "~" + Math.Ceiling(Cards[CCI].GetComponent<cardDescriptor>().OGBalance).ToString();
            reviews[CCI].Q2(q2Answer, est);
            Q2.transform.GetChild(2).GetComponent<InputField>().text = "";
        }
    }

    public void Visualise()
    {
        List<float> posX = new List<float>() { -40.0f, -30.0f, -20.0f, -10.0f, 0.0f, 10.0f, 20.0f, 30.0f, 40.0f};
        float posY = 50.0f;
        float Z = 0.0f;
        for(int i=0;i<setSize;i++)
        {
            GameObject c = Instantiate(cardPrefab);
            Cards.Add(c);
            Cards[i].GetComponent<cardDescriptor>().Load(cardSet.getCard(i));
            Cards[i].transform.position = new Vector3(posX[i % 9], posY);
            Cards[i].SetActive(!Cards[i].GetComponent<cardDescriptor>().hasPenalty);
            if (i % 9 == 8) posY -= 10.0f;
            //Cards[i].transform.position = new Vector3(0.0f, 0.0f, Z);
            //Z++;
        }
    }
    public void VisualiseReviewSet()
    {

        float Z = 0.0f;
        for (int i = 0; i < reviewSetSize; i++)
        {
            GameObject c = Instantiate(cardPrefab);
            Cards.Add(c);
            Cards[i].GetComponent<cardDescriptor>().Load(cardSet.getCard(reviewCards[i]));
            //Cards[i].transform.position = new Vector3(posX[i % 9], posY);
            //if (i % 9 == 8) posY -= 10.0f;
            Cards[i].transform.position = new Vector3(0.0f, 0.0f, Z);
            Z++;
        }
    }

    private void sendReport()
    {
        setReporter rep = reporter.GetComponent<setReporter>();
        Dictionary<string, int> t = cardSet.getTally();
        rep.Faulty = cardSet.getFaultyCount();
        rep.Rarities= new float[] { t["Common"], t["Rare"], t["Epic"], t["Legendary"] };
        rep.Types = new float[] { t["MinionType"], t["SpellType"], t["WeaponType"] };
        rep.Costs = new float[] { t["0 Cost"], t["1 Cost"], t["2 Cost"], t["3 Cost"], t["4 Cost"], t["5 Cost"], t["6 Cost"], t["7 Cost"], t["8 Cost"], t["9 Cost"], t["10 Cost"]};
        rep.AverageRating = getAVGRating();
    }
    private float getAVGRating()
    {
        float total = 0.0f;
        float count = 0.0f;
        for (int i = 0; i < setSize; i++)
        {
            if (!cardSet.getCard(i).penalty())
            {
                float t = cardSet.getCard(i).total_V;
                float c = cardSet.getCard(i).cost_V;
                //Debug.LogWarning("t: " + t.ToString() + " c: " + c.ToString());
                total += t - c;
                count += 1.0f;
            }
        }
        if (count == 0.0f)
            return float.NaN;
        return total / count;
    }

}

public class Feedback
{
    string cardSeq;
    string Q1Response;
    string Q2Response;
    string EstimatedAdjustment;
    static string path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop) + "\\CardGen";
    public Feedback(string seq)
    {
        
        cardSeq = seq;
    }
    public void Q1(string q1)
    {
        Q1Response = q1;
    }
    public void Q2(string q2, string est)
    {
        Q2Response = q2;
        EstimatedAdjustment = est;
    }
    public static void writeID(string id)
    {
        var folder = Directory.CreateDirectory(path);
        StreamWriter filewriter = new StreamWriter(path+"\\report.txt", true);
        filewriter.WriteLine(id + "\n\n");
        filewriter.Close();
    }
    public void writeReport()
    {
        StreamWriter filewriter = new StreamWriter(path+ "\\report.txt", true);
        filewriter.WriteLine(cardSeq+ "\n"+ Q1Response+"\n"+ EstimatedAdjustment+ "   "+ Q2Response +"\n\n");
        filewriter.Close();
    }
}

public class Set
{
    private List<DNA> set;
    //private List<float> scores;
    int size;
    //Rarity % targets
    Dictionary<string, float> targetRarity = new Dictionary<string, float>() { { "Common", 0.3642f }, { "Rare", 0.2777f }, { "Epic", 0.1937f }, { "Legendary", 0.1644f } };
    //Mana Cost % targets
    Dictionary<string, float> targetManaCost = new Dictionary<string, float>() { { "0 Cost", 0.131f }, { "1 Cost", 0.1186f }, { "2 Cost", 0.1779f }, { "3 Cost", 0.1824f }, { "4 Cost", 0.1553f }, { "5 Cost", 0.1226f }, { "6 Cost", 0.0879f }, { "7 Cost", 0.0598f }, { "8 Cost", 0.0392f }, { "9 Cost", 0.0246f }, { "10 Cost", 0.0161f }, { ">10 Cost", 0.0025f } };
    //Card Type Targets
    Dictionary<string, float> targetType = new Dictionary<string, float>() { { "Minion", 0.6987f }, { "Spell", 0.2601f }, { "Weapon", 0.0327f }, { "Hero", 0.0086f } };
    //Keyword Type Targets
    Dictionary<string, float> targetKeyType = new Dictionary<string, float>() { { "None", 0.0159f }, { "Static", 0.1135f }, { "Dynamic", 0.7556f }, { "Both", 0.1150f } };


    Dictionary<string, int> tally;
    int Faulty = 0;

    public Set(int s)
    {
        size = s;
        set = new List<DNA>();
        for (int i = 0; i<size; i++)
        {
            set.Add(new DNA());
        }
        setupTally();
    }

    private void Evaluate()
    {
        //scores = new List<float>();
        for (int i = 0; i < size; i++)
        {
            set[i].evaluate();
            //scores.Add(set[i].getScore());
        }
        Diversify();
    }

    public void Generate(int e)
    {
        for(int i = 0; i < e; i++)
        {
            Debug.Log("Epoch: " + i.ToString());
            Evaluate();
            //Rebalance();
            AdvanceEpoch();
        }
        Evaluate();
        //Rebalance();
    }

    private void AdvanceEpoch()
    {
        int TourneySize = 5;
        List<DNA> newSet = new List<DNA>();
        for (int i = 0; i < size; i++)
        {
            int A = Tournament(TourneySize);
            int B = Tournament(TourneySize);

            newSet.Add(new DNA(set[A].breed(set[B].getSequence())));
            newSet[i].Mutate();
        }

        set = new List<DNA>(); //clear old set
        for (int i = 0; i < size; i++)
        {
            set.Add(newSet[i]); //move new set into old set
        }
    }

    /*
    private void Rebalance()
    {
        for (int i = 0; i < size; i++)
        {
            float balance = set[i].getScore();
            if (Math.Abs(balance) >= 1.0f) //if balance is off by more than 1 mana
            {
                int cost = int.Parse(set[i].getCost().lookup());
                if (cost + (int)balance < 0 || cost + (int)balance > 10) //if purely adjusting cost would result in erroneous mana cost
                {
                    int keyNo = int.Parse(set[i].getKeywordNo().lookup());
                    if (keyNo>=1)
                    {
                        string effect = set[i].getKeywordFirst()[7].lookup();
                        if (effect == "Damage" || effect == "Heal" || effect == "Armour" || effect == "Draw" || effect == "Discard")
                        {
                            
                        }
                    }

                    
                }
                else
                {
                    set[i].getCost().Override((cost + (int)balance).ToString());
                }
            }
        }


        Evaluate(); //re-evaluate set after changes
    }
    */
    private void Diversify()
    {
        setupTally();
        Tally();
        
        for (int i = 0; i <size; i++)
        {
            float s = set[i].getScore();
            //move towards 0
            if (s<0) set[i].setScore(s - comparisonRating(i)); 
            else set[i].setScore(s + comparisonRating(i));
        }
        
    }

    private float comparisonRating(int i)
    {
        float rating = 0.0f;
        int totalMatches = 0;
        int keywordNo;
        try
        {
            keywordNo = Convert.ToInt32(set[i].getKeywordNo().lookup());
        }
        catch (FormatException e)
        {
            keywordNo = 0;//catches "NA"
        }
        //int div = 0; //tracks number of non faulty cards
        if (!set[i].isFaulty())
        {
            float rateMulti = 5.0f;
            float costRate = ((tally[set[i].getCost().lookup() + " Cost"] / (float)size) - targetManaCost[set[i].getCost().lookup() + " Cost"]) * (rateMulti*3.0f);
            float RarityRate = ((tally[set[i].getRarity().lookup()] / (float)size) - targetRarity[set[i].getRarity().lookup()]) * (rateMulti*2.0f);
            float typeRate = ((tally[set[i].getType().lookup() + "Type"] / (float)size) - targetType[set[i].getType().lookup()]) * (rateMulti*2.0f);

            int minionCount = tally["MinionType"];

            float keytypeRate = ((tally[getKeyType(i)] / (float)size) - targetKeyType[getKeyType(i)]) * rateMulti;
            float keyRate = 0.0f;
            float effectRate = 0.0f;
            float magRate = 0.0f;
            float statsRate = 0.0f; //atk/hp combo to punish lots of minions having the same statline

            if (set[i].getType().lookup() == "Minion" || set[i].getType().lookup() == "Weapon")//don't look at spells
            {
                //Debug.Log("looking at keyrate");
                if (keywordNo >= 1) keyRate += tally[set[i].getKeywordFirst()[(set[i].getKeywordFirst()[0].lookup() == "Dynamic") ? 2 : 1].lookup()] / (float)minionCount;
                if (keywordNo >= 2) keyRate += tally[set[i].getKeywordSecond()[(set[i].getKeywordSecond()[0].lookup() == "Dynamic") ? 2 : 1].lookup()] / (float)minionCount;
                if (keywordNo >= 3) keyRate += tally[set[i].getKeywordThird()[(set[i].getKeywordThird()[0].lookup() == "Dynamic") ? 2 : 1].lookup()] / (float)minionCount;
                if (keywordNo != 0) keyRate /= keywordNo;
                keyRate *= rateMulti;
            }
            if (keywordNo >= 1 && set[i].getKeywordFirst()[0].lookup() == "Dynamic") { effectRate += tally[set[i].getKeywordFirst()[7].lookup()] / (float)minionCount; magRate += tally["Mag " + set[i].getKeywordFirst()[8].lookup()] / (float)minionCount; }
            if (keywordNo >= 2 && set[i].getKeywordSecond()[0].lookup() == "Dynamic") { effectRate += tally[set[i].getKeywordSecond()[7].lookup()] / (float)minionCount; magRate += tally["Mag " + set[i].getKeywordSecond()[8].lookup()] / (float)minionCount; }
            if (keywordNo >= 3 && set[i].getKeywordThird()[0].lookup() == "Dynamic") { effectRate += tally[set[i].getKeywordThird()[7].lookup()] / (float)minionCount; magRate += tally["Mag " + set[i].getKeywordThird()[8].lookup()] / (float)minionCount; }
            if (keywordNo != 0) { effectRate /= keywordNo; magRate /= keywordNo; }
            
            if (set[i].getType().lookup() == "Minion")
            {
                try
                {
                    statsRate += tally[set[i].getAttack().lookup() + " " + set[i].getHealth().lookup()] / (float)minionCount;
                }
                catch
                {
                    Debug.LogError("tally does not contain statline");
                    Debug.LogError(set[i].getAttack().lookup() + " " + set[i].getHealth().lookup());
                }
            }
            effectRate *= rateMulti;
            magRate *= rateMulti;
            statsRate *= rateMulti;

            set[i].setRatings(costRate, RarityRate, typeRate, keytypeRate, keyRate, effectRate, magRate, statsRate);
            rating += costRate;
            rating += RarityRate;
            rating += typeRate;
            rating += keytypeRate;
            rating += keyRate;
            rating += effectRate;
            rating += magRate;
            rating += statsRate;
        }
        //else rating = 1000.0f;
        return rating;
    }

    private void Tally() //count how many different card aspects are in the set (how many have certain keywords or scopes etc.)
    {
        int keywordNo;
        
        Faulty = 0;
        for (int i =0; i<size; i++)
        {
            if(!set[i].penalty())//if there are no faults in the card (don't tally faulty or penalised cards)
            {
                try
                {
                    keywordNo = Convert.ToInt32(set[i].getKeywordNo().lookup());
                }
                catch (FormatException e)
                {
                    keywordNo = 0;
                }
                try
                {
                    tally[set[i].getType().lookup() + "Type"] += 1;

                    tally[set[i].getCost().lookup() + " Cost"] += 1;
                    tally[set[i].getRarity().lookup()] += 1;
                    if (set[i].getType().lookup() == "Minion") tally[set[i].getTribe().lookup()] += 1;
                    if (keywordNo >= 1) TallyKeyword(set[i].getKeywordFirst(), (set[i].getType().lookup()=="Spell"));
                    if (keywordNo >= 2) TallyKeyword(set[i].getKeywordSecond(), (set[i].getType().lookup() == "Spell"));
                    if (keywordNo >= 3) TallyKeyword(set[i].getKeywordThird(), (set[i].getType().lookup() == "Spell"));

                    if (keywordNo >= 1 && set[i].getKeywordFirst()[0].lookup() == "Dynamic") { tally[set[i].getKeywordFirst()[7].lookup()] += 1; tally["Mag " + set[i].getKeywordFirst()[8].lookup()] += 1; }
                    if (keywordNo >= 2 && set[i].getKeywordSecond()[0].lookup() == "Dynamic") { tally[set[i].getKeywordSecond()[7].lookup()] += 1; tally["Mag " + set[i].getKeywordSecond()[8].lookup()] += 1; }
                    if (keywordNo >= 3 && set[i].getKeywordThird()[0].lookup() == "Dynamic") { tally[set[i].getKeywordThird()[7].lookup()] += 1; tally["Mag " + set[i].getKeywordThird()[8].lookup()] += 1; }
                }
                catch (KeyNotFoundException e)
                {
                    Debug.LogError(e);
                    Debug.LogError(set[i].getType().lookup() + "Type");
                    Debug.LogError(set[i].getCost().lookup() + " Cost");
                    Debug.LogError(set[i].getRarity().lookup());
                    Debug.LogError(set[i].getTribe().lookup());
                }
                if (set[i].getType().lookup() == "Minion")
                {
                    tally[getKeyType(i)] += 1;
                    int atk = 0;
                    int hp = 0;
                    try
                    {
                        atk = Convert.ToInt32(set[i].getAttack().lookup());
                        hp = Convert.ToInt32(set[i].getHealth().lookup());
                        string entry = atk.ToString() + " " + hp.ToString();
                        tally[entry] += 1;
                    }
                    catch
                    {
                        Debug.LogError("Tally encountered Atk/HP Error");
                    }
                }
                
            }
            else
            {
                //Debug.Log("Tally encountered invalid card");
                Faulty++;
            }
        }
        //displayTally();
    }

    private string getKeyType(int i)
    {
        List<Gene> first = set[i].getKeywordFirst();
        List<Gene> second = set[i].getKeywordSecond();
        List<Gene> third = set[i].getKeywordThird();
        int keywordNo;
        string kt = "";
        try
        {
            keywordNo = Convert.ToInt32(set[i].getKeywordNo().lookup());
        }
        catch (FormatException e)
        {
            keywordNo = 0;
        }
        if (keywordNo == 0)
        {
            kt = "Blank";
        }
        else if (keywordNo == 1)
        {
            kt = first[0].lookup();
        }
        else if (keywordNo == 2)
        {
            if (first[0].lookup() == second[0].lookup())
            {
                kt = first[0].lookup();
            }
            else
            {
                kt = "Both";
            }

        }
        else if (keywordNo == 3)
        {
            if (first[0].lookup() == second[0].lookup() && third[0].lookup() == second[0].lookup())//if all keyword types are the same tally as that type
            {
                kt = first[0].lookup();
            }
            else
            {
                kt = "Both";
            }
        }
        return kt;
    }

    private void TallyKeyword(List<Gene> keyword, bool isSpell)
    {
        if(keyword[0].lookup()=="Static")
        {
            tally[keyword[1].lookup()] += 1;
        }
        else
        {
            if(isSpell)
            {
                tally[keyword[3].lookup()] += 1;
            }
            else
            {
                tally[keyword[2].lookup()] += 1;
            }
            tally[keyword[7].lookup()] += 1;
        }
    }

 
    private void setupTally()
    {
        tally = new Dictionary<string, int>() {
            //card type
            { "MinionType", 0 },
            { "SpellType", 0 },
            { "WeaponType", 0 },
            //mana cost
            { "0 Cost", 0 },
            { "1 Cost", 0 },
            { "2 Cost", 0 },
            { "3 Cost", 0 },
            { "4 Cost", 0 },
            { "5 Cost", 0 },
            { "6 Cost", 0 },
            { "7 Cost", 0 },
            { "8 Cost", 0 },
            { "9 Cost", 0 },
            { "10 Cost", 0 },
            //rarity
            { "Common", 0 },
            { "Rare", 0 },
            { "Epic", 0 },
            { "Legendary", 0 },
            //tribes
            { "None", 0 },
            { "Mech", 0 },
            { "Beast", 0 },
            { "Demon", 0 },
            { "Pirate", 0 },
            { "Elemental", 0 },
            //number of keywords isn't counted since having multiple keywords increases the amount of seperate cards with similar features
            //keyword type (spell is accounted for as a card type
            { "Blank", 0 },
            { "Static", 0 },
            { "Dynamic", 0 },
            { "Both", 0 },
            //keywords
            { "Taunt", 0 },
            { "Stealth", 0 },
            { "Charge", 0 },
            { "Divine Shield", 0 },
            { "Windfury", 0 },
            { "Spell Damage", 0 },
            { "Overload", 0 },
            { "Elusive", 0 },
            { "Poisonous", 0 },
            { "Lifesteal", 0 },
            { "Reborn", 0 },
            { "Rush", 0 },
            { "Forgetful", 0 },
            { "Mega-Windfury", 0 },
            { "Can't Attack", 0 },
            { "Cleave", 0 },

            { "Battlecry", 0 },
            { "Deathrattle", 0 },
            { "Combo", 0 },
            { "Battlecry And Deathrattle", 0 },
            { "End Of Turn", 0 },
            { "Start Of Turn", 0 },
            { "When Damaged", 0 },
            { "Discarded", 0 },
            { "Spell", 0 }, //does not include combo spells
            
            //targets
            { "Hero", 0 },
            { "Minion", 0 },
            { "Character", 0 },
            { "Hand", 0 },
            { "Deck", 0 },
            { "Weapon", 0 },
            { "Self", 0 },
            //scopes
            { "All", 0 },
            { "Single", 0 },
            { "Double", 0 },
            { "Triple", 0 },
            //allegiance
            { "Friendly", 0 },
            { "Enemy", 0 },
            { "Any", 0 },
            //effect
            { "Damage", 0 },
            { "Heal", 0 },
            { "Destroy", 0 },
            { "Silence", 0 },
            { "Freeze", 0 },
            { "Discard", 0 },
            { "Draw", 0 },
            { "Armour", 0 },
            { "Mind Control", 0 },
            { "Return", 0 },
            { "Shuffle", 0 },
            { "Summon", 0 },
            { "Buff", 0 },
            //magnitudes
            { "Mag 1", 0 },
            { "Mag 2", 0 },
            { "Mag 3", 0 },
            { "Mag 4", 0 },
            { "Mag 5", 0 },
            { "Mag 6", 0 },
            { "Mag 7", 0 },
            { "Mag 8", 0 },
            { "Mag 9", 0 },
            { "Mag 10", 0 },
            { "Mag 11", 0 },
            { "Mag 12", 0 },
            { "Mag 13", 0 },
            { "Mag 14", 0 },
            { "Mag 15", 0 },
            { "Mag 16", 0 }
        };

        for (int hp = 1; hp <= 16; hp++)
        {
            for (int atk = 0; atk <= 15; atk++)
            {
                string entry = atk.ToString() + " " + hp.ToString();
                tally.Add(entry,0);
            }
        }
    }

    private void displayTally()
    {
        foreach (KeyValuePair<string, int> kvp in tally)
        {
            //textBox3.Text += ("Key = {0}, Value = {1}", kvp.Key, kvp.Value);
            Debug.Log("Key = " + kvp.Key + ", Value = " + kvp.Value);
        }
    }

    public Dictionary<string, int> getTally() { return tally; }

    public int getSize() { return size; }



    private int Tournament(int TourneySize) //returns index for selected DNA
    {
       
        List<int> participents = new List<int>();
        int partiCount = 0; //participent count

        while (partiCount<TourneySize)
        {
            int r = UnityEngine.Random.Range(0, size - 1);
            if(!participents.Contains(r))
            {
                participents.Add(r);
                partiCount++;
            }
        }

        int fittest = 0;
        float fitScore = 999;
        for (int i =0; i<TourneySize; i++)
        {
            int part = participents[i]; //participent
            float sc = Math.Abs(set[part].getScore()); //participents score
            if (sc < fitScore)
            {
                fittest = part;
                fitScore = Math.Abs(sc);
            }
        }
        return fittest;
    }

    public DNA getCard(int i)
    {
        return set[i];
    }

    public int getFaultyCount() { return Faulty; }
}