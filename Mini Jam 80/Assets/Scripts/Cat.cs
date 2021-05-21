using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Cat : MonoBehaviour
{
    public Animator catAnim;
    public Animator emoteAnim;
    //Stats
    public string catName = "Sir Fluffles";
    
    public Image catSprite;
    public Image effect;
    int spriteNumber;
    public GameManager gm;

    //Age
    public bool isBaby = true;
    public string ageText = "Age: Kitten";

    //Healthy
    public bool isHealthy = true;
    public int health;
    public float healthHeartCounter;

    //Happy
    public int happiness;
    public float happyHeartCounter;

    //Hungry
    bool isFull = false;
    int fullness;

    //Sleep
    bool hasEnergy = true;
    public int tiredScale;
    bool isSleeping = false;
    int sleepTimer = 200;

    //Care
    public bool hygenic = true;

    const int kittyMax = 120;
    const int catMax = 160;
    
    public int maxForAge;
    public float maxHearts;
    int low = 10;

    bool statApplied = false;

    void Start()
    {
        isBaby = true;
        ageText = "Age: Kitten";

        if (isBaby)
        {
            maxForAge = kittyMax;
            spriteNumber = Random.Range(0, gm.kittenSprites.Count());
            catSprite.sprite = gm.kittenSprites[spriteNumber];

            catAnim.SetBool("isBaby", isBaby);
            catAnim.SetInteger("Fur", spriteNumber);
        }

        if(gm.cats.Count() > 1)
        {
            name = catName = "Sir Fluffles II";
        }
        else
        {
            name = catName = "Sir Fluffles";
        }

        healthHeartCounter = happyHeartCounter = 0;
        health = 60;
        happiness = 15;
        fullness = 5;
        tiredScale = 0;
        
    }


    // Update is called once per frame
    void Update()
    {
        CheckSleepingConditions();
        CheckHungry();
        StartCoroutine(StatsOverTime());
    }

    IEnumerator StatsOverTime()
    {
        if (!statApplied && !isSleeping)
        {
            statApplied = true;
            yield return new WaitForSeconds(200);
            IncrementHealth(-5);
            IncrementFull(-5);
            IncrementTired(5);
            statApplied = false;
        }
    }

    public IEnumerator FeedCats()
    {
        if (!isSleeping)
        {
            gm.foodBowl.SetActive(true);
            IncrementFull(30);

            catAnim.SetBool("Hungry", true);
            yield return new WaitForSeconds(3);
            catAnim.SetBool("Hungry", false);

            gm.foodBowl.SetActive(false);
        }
    }

    void CheckHungry()
    {
        if(fullness <= low)
        {
            catAnim.SetBool("Hungry", true);
        }
        else
        {
            catAnim.SetBool("Hungry", false);
        }
    }

    public void SetAgeToAdult()
    {
        isBaby = false;
        maxForAge = catMax;
        catSprite.sprite = gm.catSprites[spriteNumber];

        catAnim.SetBool("isBaby", false);
    }
    public bool CheckHeartStatusFull()
    {
        if (isBaby && happyHeartCounter == 3 && healthHeartCounter == 3)
        {
            return true;
        }
        else if (!isBaby && happyHeartCounter == 4f && healthHeartCounter == 4f)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

  

    public IEnumerator Play()
    {
        if(!isSleeping)
        {
            Debug.Log("Playing with cat");
            if (hasEnergy)
            {
                StartCoroutine(IncrementHappiness(15));
                IncrementTired(15);
            }
            else
            {
                StartCoroutine(IncrementHappiness(5));
                IncrementTired(15);
            }

            gm.toy.gameObject.SetActive(true);
            yield return new WaitForSeconds(2);
            gm.toy.gameObject.SetActive(false);
        }
    }

    public void IncrementTired(int i)
    {
        if (tiredScale + i > maxForAge)
        {
            tiredScale = maxForAge;
        }
        else
        {
            tiredScale += i;
        }
    }

    void CheckSleepingConditions()
    {
        if (tiredScale >= maxForAge)
        {
            tiredScale = maxForAge;
            hasEnergy = false;

            if (!isSleeping) //if cat is not already sleeping
            {
                isSleeping = true;
                StartCoroutine(WakeCat());
                Debug.Log("Cat is sleeping");
            }
            else
            {
                if (!effect.isActiveAndEnabled)
                {
                    effect.gameObject.SetActive(true);
                    effect.sprite = gm.effects[3];
                    emoteAnim.SetBool("isSleeping", true);
                }
            }
        }
    }

    IEnumerator WakeCat()
    {
        catAnim.SetBool("isSleeping", true);
        yield return new WaitForSeconds(sleepTimer);

        isSleeping = false;
        hasEnergy = true;
        tiredScale = 0;

        isFull = false;
        fullness = 15;

        catAnim.SetBool("isSleeping", false);
        emoteAnim.SetBool("isSleeping", false);
        effect.gameObject.SetActive(false);
    }

   
    public void ApplyCleanEffect()
    {
        if (gm.hasDeuce)
        {
            Debug.Log("Picking up poo");
            IncrementCleanliness(5);
        }
    }
    public void IncrementCleanliness(int i)
    {
        IncrementHealth(i);
        StartCoroutine(IncrementHappiness(i));
    }


    public void ApplyMedicineCorrectly()
    {
        Debug.Log("Feeding medicine");
        IncrementHealth(10);
        StartCoroutine(IncrementHappiness(3));
    }

    public void ApplyMedicineIncorrectly()
    {
        Debug.Log("Don't feed cat medicine if not sick.");
        StartCoroutine(IncrementHappiness(-5));
        IncrementHealth(-3);

    }

    void IncrementHealth(int i)
    {
        if (isHealthy)
        {
            if (health + i > maxForAge)
            {
                health = maxForAge;
            }
            else
            {
                health += i;
            }
        }
        else
        {
            if (health + i <= 0)
            {
                health = 0;
            }
        }

        healthHeartCounter = UpdateHeartCounter(healthHeartCounter, health);
    }

    public void CheckHealthState()
    {
        if (health <= low)
        {
            Debug.Log("Your cat isn't feeling too good. Please take care of your cat");
            ChangeStateSick(true);
        }
        else if (!isHealthy && health >= low + 30)
        {
            ChangeStateSick(false);
        }
    }

    public void ChangeStateSick(bool status)
    {
        isHealthy = !status;
        hasEnergy = !status;

        effect.gameObject.SetActive(status);
        if (status)
            effect.sprite = gm.effects[2];
        
        emoteAnim.SetBool("isSick", status);
    }

    

    float UpdateHeartCounter(float heartCounter, int stat)
    {
        if (stat < 20)
        {
            heartCounter = 0;
        }
        else if (stat >= 20 && stat < 40)
        {
            heartCounter = .5f;
        }
        else if (stat >= 40 && stat < 60)
        {
            heartCounter =1f;
        }
        else if (stat >= 60 && stat < 80)
        {
            heartCounter = 1.5f;
        }
        else if (stat >= 80 && stat < 100)
        {
            heartCounter  = 2f;
        }
        else if (stat >= 100 && stat < 120)
        {
            heartCounter = 2.5f;
        }
        else if (stat >= 120 && stat < 140)
        {
            heartCounter = 3f;
        }
        else if (stat >= 140 && stat < 160)
        {
            heartCounter = 3.5f;
        }
        else
        {
            heartCounter = 4f;
        }
        return heartCounter;
    }

    public IEnumerator IncrementHappiness(int i)
    {
        if (happiness + i >= maxForAge)
        {
            happiness = maxForAge;
        }
        else
        {
            happiness += i; 
        }

        if(i < 0)//happiness is being decremented
        {
            effect.gameObject.SetActive(true) ;
            effect.sprite = gm.effects[0];

            emoteAnim.SetBool("isAngry", true);
            yield return new WaitForSeconds(1.5f);//wait for a few seconds for animation to play
            emoteAnim.SetBool("isAngry", false);

            effect.gameObject.SetActive(false);
        }
        else if (i > 0)
        {
            effect.gameObject.SetActive(true);
            effect.sprite = gm.effects[1];

            emoteAnim.SetBool("isHappy", true);
            yield return new WaitForSeconds(1.5f);//wait for a few seconds for animation to play
            emoteAnim.SetBool("isHappy", false);

            effect.gameObject.SetActive(false);
        }

        happyHeartCounter = UpdateHeartCounter(happyHeartCounter, happiness);
    }



    void IncrementFull(int i)
    {
        if (fullness + i >= maxForAge)
        {
            if (!isFull)
            {  
                fullness = maxForAge;
                isFull = true;
            }
            else
            {
                Debug.Log("Cat is being overfed. -20 health. - 10 happy. Health: " + health + ", Happy: " + happiness);
                IncrementHealth(-20);
                StartCoroutine(IncrementHappiness(-10));
            }
        }
        else
        {
            fullness += i;
            IncrementHealth(10);
            StartCoroutine(IncrementHappiness(5));
        }
    }



    public void Pet()
    {
        happiness += 1;
    }

    // Start is called before the first frame update

}
