using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject freezer;
    public GameObject bugger;
    public GameObject boost;
    public GameObject slowed;
    public GameObject scrambler;
    public GameObject frozen;
    public GameObject finder;
    public GameObject player;
    public GameObject player2;
    public GameObject beacon;
    public float lowerX;
    public float lowerY;
    public float upperX;
    public float upperY;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GameObject[] slows = GameObject.FindGameObjectsWithTag("Slow");
        GameObject[] speeds = GameObject.FindGameObjectsWithTag("Boost");
        GameObject[] scrambles = GameObject.FindGameObjectsWithTag("ScrambledEggs");
        setColors(scrambles, new Color(10, 0, 255));
        GameObject[] finders = GameObject.FindGameObjectsWithTag("Finder");
        GameObject[] bugging = GameObject.FindGameObjectsWithTag("Glitch");
    }
    public void frozenPlayer()
    {
        Instantiate(frozen, player.transform.position, frozen.transform.rotation);
    }
    public void frozenPlayer2()
    {
        Instantiate(frozen, player2.transform.position, frozen.transform.rotation);
    }
    public void freeze()
    {
        if(GameObject.FindGameObjectsWithTag("Freeze").Length == 1)
            Instantiate(freezer, new Vector3(Random.Range(lowerX, upperX), Random.Range(lowerY, upperY), 0), freezer.transform.rotation);
    }
    public void speed()
    {
        Instantiate(boost, new Vector3(Random.Range(lowerX, upperX), Random.Range(lowerY, upperY), 0), boost.transform.rotation);
    }
    public void slow()
    {
        Instantiate(slowed, new Vector3(Random.Range(lowerX, upperX), Random.Range(lowerY, upperY), 0), slowed.transform.rotation);
    }
    public void scrambledEggs()
    {
        Instantiate(scrambler, new Vector3(Random.Range(lowerX, upperX), Random.Range(lowerY, upperY), 0), scrambler.transform.rotation);
    }

    public void glitcher()
    {
        Instantiate(bugger, new Vector3(Random.Range(lowerX, upperX), Random.Range(lowerY, upperY), 0), bugger.transform.rotation);
    }

    public void loadFinder()
    {
        Instantiate(finder, new Vector3(Random.Range(lowerX, upperX), Random.Range(lowerY, upperY), 0), finder.transform.rotation);
    }

    public void setColors(GameObject[] stuffs, Color color)
    {
        for (int thingy = 0; thingy < stuffs.Length; thingy++)
        {
            GameObject theOneRingToRuleThemAll = stuffs[thingy];
            theOneRingToRuleThemAll.GetComponent<Renderer>().material.color = color;
        }
    }

    public void findCountry(GameObject country)
    {
        Instantiate(beacon, country.transform.position, country.transform.rotation);
    }
}
