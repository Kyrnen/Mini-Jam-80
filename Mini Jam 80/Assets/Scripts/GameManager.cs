using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public bool ownsCat;

    public bool hasDeuce;
    public Animator litterboxAnim;
    bool activeButtons = false;

    public Text currentCatName;
    public Text currentCatAge;
    public Image catProfileImage;

    //These shouldn't be changed ever
    [SerializeField] GameObject catPrefab;
    [SerializeField] GameObject heartPrefab;
    [SerializeField] GameObject litterbox;
    public GameObject foodBowl;
    public GameObject toy;

    public Sprite[] kittenSprites;
    public Sprite[] catSprites;
    public Sprite[] effects;

    [SerializeField] Sprite emptyHeart;
    [SerializeField] Sprite halfHeart;
    [SerializeField] Sprite fullHeart;

    [SerializeField] Button food;
    [SerializeField] Button stats;
    [SerializeField] Button play;
    [SerializeField] Button clean;
    [SerializeField] Button care;

    [SerializeField] Button prevCat;
    [SerializeField] Button nextCat;


    public GameObject gameCanvas;
    [SerializeField] GameObject statsPanel;
    public GameObject healthHeartMeter;
    public GameObject happyHeartMeter;

    int damageRate = 5;
    bool hygeneDamage = false;
    [SerializeField] List<GameObject> healthHeartList;
    [SerializeField] List<GameObject> happyHeartList;

    public List<Cat> cats;
    public int timeBetweenEvents = 500;
    bool eventOccurred = false;

    // Start is called before the first frame update
    void Start()
    {
        cats = new List<Cat>();
        EnableInteraction();
        hasDeuce = false;
    }

    void SpawnCat()
    {
        Instantiate(catPrefab);
        ownsCat = true;
        GameObject cat = GameObject.FindGameObjectWithTag("Cat");
        cat.transform.SetParent(gameCanvas.transform, false);
        cat.transform.SetSiblingIndex(1);
        Cat c = cat.GetComponent<Cat>();
        cats.Add(c);
        c.gm = this;

        activeButtons = true;
        EnableInteraction();
    }

    public void ShowStats(int i)
    {   
        if (cats.Count() > 0)
        {
            statsPanel.SetActive(true);
            if (cats.Count() > 1)
            {
                if (i == 0)
                {
                    prevCat.gameObject.SetActive(false);
                    nextCat.gameObject.SetActive(true);
                }
                else if (i == 1)
                {
                    prevCat.gameObject.SetActive(true);
                    nextCat.gameObject.SetActive(false);
                }
            }
            else //(cats.Count() == 1)
            {
                prevCat.gameObject.SetActive(false);
                nextCat.gameObject.SetActive(false);
            }

            UpdateShownStats(i);
            activeButtons = false;
            EnableInteraction();
            food.enabled = false;
            stats.enabled = true;
        }
            
    }

    public void ExitStats()
    {
        statsPanel.SetActive(false);
        activeButtons = true;
        EnableInteraction();
    }

    void UpdateShownStats(int i)
    {
        currentCatName.text = cats[i].catName;
        catProfileImage.sprite = cats[i].catSprite.sprite;
        currentCatAge.text = cats[i].ageText;
        UpdateHeartsinProfile(cats[i]);
    }

    void UpdateHeartsinProfile(Cat c)
    {
        if (c.isBaby)
        {
            RemoveHearts();
        }
        else
        {
            if (healthHeartList.Count() == 3 && happyHeartList.Count() == 3)
            {
                AddHearts();
            }
        }
        UpdateHealthHearts(c);
        UpdateHappyHearts(c);
    }

    void UpdateHeartList(Cat c, float heartCounter, List<GameObject> heartList)
    {
        switch (heartCounter)
        {
            case 0f:
                heartList[0].GetComponent<Image>().sprite = emptyHeart;
                heartList[1].GetComponent<Image>().sprite = emptyHeart;
                heartList[2].GetComponent<Image>().sprite = emptyHeart;
                if (!c.isBaby)
                {
                    heartList[3].GetComponent<Image>().sprite = emptyHeart;
                }
                break;
            case .5f:
                heartList[0].GetComponent<Image>().sprite = halfHeart;
                heartList[1].GetComponent<Image>().sprite = emptyHeart;
                heartList[2].GetComponent<Image>().sprite = emptyHeart;
                if (!c.isBaby)
                {
                    heartList[3].GetComponent<Image>().sprite = emptyHeart;
                }
                break;
            case 1f:
                heartList[0].GetComponent<Image>().sprite = fullHeart;
                heartList[1].GetComponent<Image>().sprite = emptyHeart;
                heartList[2].GetComponent<Image>().sprite = emptyHeart;
                if (!c.isBaby)
                {
                    heartList[3].GetComponent<Image>().sprite = emptyHeart;
                }
                break;
            case 1.5f:
                heartList[0].GetComponent<Image>().sprite = fullHeart;
                heartList[1].GetComponent<Image>().sprite = halfHeart;
                heartList[2].GetComponent<Image>().sprite = emptyHeart;
                if (!c.isBaby)
                {
                    heartList[3].GetComponent<Image>().sprite = emptyHeart;
                }
                break;
            case 2f:
                heartList[0].GetComponent<Image>().sprite = fullHeart;
                heartList[1].GetComponent<Image>().sprite = fullHeart;
                heartList[2].GetComponent<Image>().sprite = emptyHeart;
                if (!c.isBaby)
                {
                    heartList[3].GetComponent<Image>().sprite = emptyHeart;
                }
                break;
            case 2.5f:
                heartList[0].GetComponent<Image>().sprite = fullHeart;
                heartList[1].GetComponent<Image>().sprite = fullHeart;
                heartList[2].GetComponent<Image>().sprite = halfHeart;
                if (!c.isBaby)
                {
                    heartList[3].GetComponent<Image>().sprite = emptyHeart;
                }
                break;
            case 3f:
                healthHeartList[0].GetComponent<Image>().sprite = fullHeart;
                healthHeartList[1].GetComponent<Image>().sprite = fullHeart;
                healthHeartList[2].GetComponent<Image>().sprite = fullHeart;
                if (!c.isBaby)
                {
                    healthHeartList[3].GetComponent<Image>().sprite = emptyHeart;
                }
                break;
            case 3.5f:
                if (!c.isBaby)
                {
                    healthHeartList[0].GetComponent<Image>().sprite = fullHeart;
                    healthHeartList[1].GetComponent<Image>().sprite = fullHeart;
                    healthHeartList[2].GetComponent<Image>().sprite = fullHeart;
                    healthHeartList[3].GetComponent<Image>().sprite = halfHeart;
                }
                break;
            case 4f:
                if (!c.isBaby)
                {
                    healthHeartList[0].GetComponent<Image>().sprite = fullHeart;
                    healthHeartList[1].GetComponent<Image>().sprite = fullHeart;
                    healthHeartList[2].GetComponent<Image>().sprite = fullHeart;
                    healthHeartList[3].GetComponent<Image>().sprite = halfHeart;
                }
                break;
        }

    }

    void UpdateHealthHearts(Cat c)
    {
        UpdateHeartList(c, c.healthHeartCounter, healthHeartList);
    }

    void UpdateHappyHearts(Cat c)
    {
        UpdateHeartList(c, c.happyHeartCounter, happyHeartList);
    }

    void AddHearts()
    {
        GameObject[] newHearts = {Instantiate(heartPrefab), Instantiate(heartPrefab)};

        newHearts[0].transform.SetParent(healthHeartMeter.transform, false);
        newHearts[1].transform.SetParent(happyHeartMeter.transform, false);

        healthHeartList.Add(newHearts[0]);
        happyHeartList.Add(newHearts[1]);
    }

    void RemoveHearts()
    {
        if (healthHeartList.Count() == 4 && happyHeartList.Count() == 4)
        {
            GameObject healthHeartToRemove = healthHeartList[3];
            GameObject happyHeartToRemove = happyHeartList[3];
            
            healthHeartList.RemoveAt(3);
            happyHeartList.RemoveAt(3);

            Destroy(healthHeartToRemove);
            Destroy(happyHeartToRemove);
        }
    }

    public void Feed()
    {
        if (!ownsCat)
        {
            Debug.Log("Cat Spawns");
            //update all the necessary things with cat coming to life
            SpawnCat();
        }
        else
        {
            List<Cat> catsLeaving = new List<Cat>();

            foreach (Cat c in cats)
            {
                StartCoroutine(c.FeedCats());
                //check if the cat is still healthy. if not, then indicate this
                c.CheckHealthState();

                Debug.Log(c.isHealthy + " " + c.health);
                if (!c.isHealthy && c.health <= 0)
                {
                    Debug.Log("Cat is leaving. You suck.");
                    catsLeaving.Add(c);
                }
            }

            CheckForCatLeaves(catsLeaving);
        }
    }

    void CheckForCatLeaves(List<Cat> leaving)
    {
        foreach (Cat c in leaving)
        {
            cats.Remove(c);
            Destroy(c.gameObject);
        }

        if(cats.Count() == 0)
        {
            ownsCat = false;
        }
    }

    public void Play()
    {
        if(ownsCat)
        {
            foreach (Cat c in cats)
            {
                StartCoroutine(c.Play());
            }
        }
    }

    public void Clean()
    {
        if(ownsCat)
        {
            if(hasDeuce)
            {
                foreach (Cat c in cats)
                {
                    c.ApplyCleanEffect();
                }
                //clean out litter box
                hasDeuce = false;

                StartCoroutine(CleanUpAnimation());
            }
        }
    }

    IEnumerator CleanUpAnimation()
    {
        litterboxAnim.SetBool("isDirty", false);
        litterboxAnim.SetBool("cleaning", true);
        yield return new WaitForSeconds(1);
        litterboxAnim.SetBool("cleaning", false);
    }

    void CheckCleanliness()
    {
        if(hasDeuce)
        {
            litterboxAnim.SetBool("isDirty", true);
            foreach (Cat c in cats)
            {
                if (c.hygenic)
                {
                    c.hygenic = false;
                    hygeneDamage = true;
                }
                else
                {
                    if (hygeneDamage)
                    {
                        StartCoroutine(ReduceHealthOverTime(c));
                    }
                }
            }

        }
        else
        {
            foreach(Cat c in cats)
            {
                c.hygenic = true;
            }
        }
    }

    IEnumerator ReduceHealthOverTime(Cat c)
    {
        hygeneDamage = false;
        yield return new WaitForSeconds(damageRate);
        c.IncrementCleanliness(-5);
        hygeneDamage = true;
        Debug.Log("health is being decreased over time since there is poo here");
    }

    public void Care()
    {
        if(ownsCat)
        {
            List<Cat> sickCatList = new List<Cat>();

            foreach(Cat c in cats)
            {
                if (!c.isHealthy)
                {
                    sickCatList.Add(c);
                }
            }
            if (sickCatList.Count() > 0)
            {
                foreach (Cat sick in sickCatList)
                {
                    sick.ApplyMedicineCorrectly();
                }
            }
            else
            {
                foreach (Cat c in cats)
                {
                    c.ApplyMedicineIncorrectly();
                }
            }
        }
    }

    void BecomeAdult()
    {
        foreach(Cat c in cats)
        {
            if(c.isBaby && c.CheckHeartStatusFull())
            {
                c.SetAgeToAdult();
                AddHearts();
                UpdateHeartsinProfile(c);
                Debug.Log("My baby is an adult now");
            }
        }
    }

    void BringBaby()
    {
        if(cats.Count() < 2)
        {
            if(!cats[0].isBaby && cats[0].CheckHeartStatusFull())
            {
                SpawnCat();
            }
        }
    }

    void LeaveHome()
    {
        if (cats.Count() == 2)
        {
            Cat adult = cats[0];
            Cat baby = cats[1];

            List<Cat> leaving = new List<Cat>();
            if(baby.health >= 40 && baby.happiness >=40)
            {
                leaving.Add(adult);
            }
            CheckForCatLeaves(leaving);
        }
    }

    public void EnableInteraction()
    {
        if (activeButtons)
        {
            food.enabled = true;
            stats.interactable = true;
            play.interactable = true;
            clean.interactable = true;
            care.interactable = true;
        }
        else
        {
            stats.interactable = false;
            play.interactable = false;
            clean.interactable = false;
            care.interactable = false;
        }
    }


    IEnumerator ImplementEvent()
    {
        if (!eventOccurred)
        {
            eventOccurred = true;
            yield return new WaitForSeconds(timeBetweenEvents);
            int randomEvent = Random.Range(0, 5);
            foreach(Cat c in cats)
            { 

                switch (randomEvent)
                {
                    case 0:
                        hasDeuce = true;
                        break;
                    case 1:
                        if (!cats[0].isHealthy)
                        {
                            hasDeuce = true;
                        }
                        else
                        {
                            c.catAnim.SetBool("Walking", true);
                            yield return new WaitForSeconds(10);
                            c.catAnim.SetBool("Walking", false);
                        }
                        break;
                    case 2:
                        c.tiredScale = c.maxForAge;
                        break;
                    case 3:
                        c.catAnim.SetBool("Walking", true);
                        yield return new WaitForSeconds(10);
                        c.catAnim.SetBool("Walking", false);
                        break;
                    case 4:
                        //cat randomly gets sick 1/5 chance
                        c.isHealthy = false;
                        c.ChangeStateSick(true);
                        break;
                }
            }
            eventOccurred = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (cats.Count() > 0)
        {
            StartCoroutine(ImplementEvent());
            CheckCleanliness();
            BecomeAdult();
            if (!cats[0].isBaby)
            {
                BringBaby();
                LeaveHome();
            }
        }
        
    }

}
