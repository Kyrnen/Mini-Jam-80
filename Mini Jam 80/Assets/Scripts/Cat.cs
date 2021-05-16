using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Cat : MonoBehaviour
{
    public GameManager gm;
    //Stats
    //Age
    public bool isBaby;

    //Healthy
    public bool isHealthy = true;
    public int health;
    int maxHealth;

    //Happy
    bool isPlayful = true;
    bool isHappy = true;
    public int happiness;
    int maxHappy;

    //Hungry
    bool isFull = false;
    int fullness;
    int maxFullMeter;

    //Sleep
    bool hasEnergy = true;
    int tiredScale;
    bool isSleeping = false;
    
    //Care
    bool hasDeuce = true;
    int cleanliness;

    int kittyMax = 150;
    int catMax = 200;
    int maxForAge;
    int low = 20;


    public void FeedCats()
    {
        //place food
        Debug.Log("Cat is being Fed");
        if (fullness <= maxForAge)
        {
            Debug.Log("Cat is hungry. Cat is eat. +20");
            fullness += 20;
            Debug.Log(fullness);
        }
        else
        {
            Debug.Log("Cat is being overfed. -10 health. - 10 happy");
            IncrementHealth(-10);
            IncrementHappiness(-10);
        }
    }

    public void Play()
    {
        Debug.Log("Playing with cat");
        if(!hasEnergy)
        {
            if (isPlayful)
            {
                IncrementHappiness(10);
                IncrementTired(15);
            }
            else
            {
                IncrementHappiness(15);
                IncrementTired(15);
            }
        }
        else
        {
            IncrementHappiness(-8);
            //lock play maybe
            //next command will be sleep
        }

    }

    public void ApplyCleanEffect()
    {
        Debug.Log("Picking up poo");
        IncrementCleanliness(10);
    }

    public void ApplyMedicineCorrectly()
    {
        Debug.Log("Feeding medicine");
        IncrementHealth(10);
        if (health >= low + 30)
        {
            isHealthy = true;
            //deactivate sick conditions
        }
    }

    public void ApplyMedicineIncorrectly()
    {
        Debug.Log("Don't feed cat medicine if not sick.");
        IncrementHappiness(-5);
        IncrementHealth(-3);
    }


    void IncrementStat(int stat, int by, bool r)
    {
        if (stat <= low && !r)
        {
            r = true;
            //SetSickCondition, poo rate goes up, playful rate goes down, tired meter increases
            Debug.Log("Cat is sick. Cat requires Healing");
        }
        else if (stat <= 0)
        {
            Debug.Log("Cat is leaving. You suck.");
        }
        else
        {
            stat += by;
            if (stat >= low + 30)
            {
                r = false;
                //Turn off Sick condition
            }
        }
    }

    public void IncrementHealth(int i)
    {
        IncrementStat(health, i, isHealthy);
    }

    public void IncrementHappiness(int i)
    {
        IncrementStat(happiness, i, isHappy);
    }

   public void IncrementTired(int i)
    {
        IncrementStat(tiredScale, i, hasEnergy);
    }

    public void IncrementCleanliness(int i)
    {
        IncrementHealth(5);
        IncrementHappiness(5);
    }

    
    public void Pet()
    {
        happiness += 1;
    }

    public void UpdateHealth()
    {
        //if the health and happiness of cat reach a certain threshold for a certain amount of time, then the cat is considered healthy
        health += 5;
    }

    public void UpdateCat()
    {
        //update stat boxes appropriately
    }

    // Start is called before the first frame update
    void Start()
    {
        if(isBaby) 
         maxForAge = kittyMax; 
        else 
         maxForAge = catMax;

        //for debug
        happiness = fullness = health = 30;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
