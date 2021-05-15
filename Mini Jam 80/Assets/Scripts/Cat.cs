using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cat : MonoBehaviour
{
    bool ownsCat = true;
    //Stats
    bool isBaby;
    bool isHungry = true;
    bool isPlayful = true;
    bool isHealthy = false;
    bool isTired = true;
    bool hasDeuce = true;

    public GameObject[] healthMarker;
    
    public int health;
    public int hunger;
    public int happiness;

    int currentHealth;
    int currentHunger;
    int currentHappiness;

    public void Feed()
    {
        //place food
        Debug.Log("Cat is being Fed");

        if (!ownsCat && isHungry)
        {
            Debug.Log("Cat Spawns");
            //update all the necessary things with cat coming to life
        }

        currentHunger += 5;
        Debug.Log(currentHunger);
    }

    public void Play()
    {
        Debug.Log("Playing with cat");
        if(ownsCat) currentHappiness += 5;
    }

    public void Clean()
    {
        if(hasDeuce)
        {
            Debug.Log("Picking up poo");
        }
    }

    public void Care()
    {
        if(!isHealthy)
        {
            Debug.Log("Feeding medicine");
            //after a certain amount of times...
            isHealthy = true;
        }
        else
        {
            Debug.Log("Don't feed cat medicine if not sick.");
            currentHappiness -= 5;
        }

    }

    public void Pet()
    {
        currentHappiness += 1;
    }

    public void UpdateHealth()
    {
        //if the health and happiness of cat reach a certain threshold for a certain amount of time, then the cat is considered healthy
        currentHealth += 5;
    }

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = currentHappiness = currentHunger = 1;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
