using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public bool ownsCat;

    public bool hasDeuce;

    public GameObject catPrefab;
    public GameObject heartPrefab;

    public Canvas gameCanvas;
    public GameObject healthHeartMeter;
    public GameObject happyHeartMeter;

    public List<Cat> cats;
    // Start is called before the first frame update
    void Start()
    {
        cats = new List<Cat>();

        //if save file exists
        //SpawnCat();
       
    }

    void SpawnCat()
    {
        Instantiate(catPrefab);
        
        ownsCat = true;
        GameObject cat = GameObject.FindGameObjectWithTag("Cat");
        cat.transform.SetParent(gameCanvas.transform, false);
        cat.transform.SetAsFirstSibling();
        Cat c = cat.GetComponent<Cat>();
        cats.Add(c);

        //need to finish logic on this
        c.isBaby = true;

        Debug.Log(cats.Count());
    }

    void AddHearts()
    {
        GameObject[] newHearts = {Instantiate(heartPrefab), Instantiate(heartPrefab)};

        newHearts[0].transform.SetParent(healthHeartMeter.transform, false);
        newHearts[1].transform.SetParent(happyHeartMeter.transform, false);
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
            foreach (Cat c in cats)
            {
                c.FeedCats();
            }
        }
    }

    public void Play()
    {
        if(ownsCat)
        {
            foreach (Cat c in cats)
            {
                c.Play();
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
            }
            else
            {
                Debug.Log("Nothing to clean");
            }
        }
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
            }
            Debug.Log("My baby is an adult now");
        }
    }

    // Update is called once per frame
    void Update()
    {
        //For Debug
        if (Input.GetKeyDown(KeyCode.A))
        {
            Debug.Log("Pressed");
            BecomeAdult();
        }
    }

}
