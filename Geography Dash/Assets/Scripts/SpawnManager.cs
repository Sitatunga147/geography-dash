using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject freezeObject;
    public GameObject glitchObject;
    public GameObject speedObject;
    public GameObject slowObject;
    public GameObject scramblerObject;
    public GameObject frozenObject;
    public GameObject finderObject;
    public GameObject player;
    public GameObject player2;
    public GameObject beaconObject;
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
        Instantiate(frozenObject, player.transform.position, frozenObject.transform.rotation);
    }
    public void frozenPlayer2()
    {
        Instantiate(frozenObject, player2.transform.position, frozenObject.transform.rotation);
    }
    public void freeze()
    {
        if(GameObject.FindGameObjectsWithTag("Freeze").Length == 1)
            Instantiate(freezeObject, new Vector3(Random.Range(lowerX, upperX), Random.Range(lowerY, upperY), 0), freezeObject.transform.rotation);
    }
    public void speed()
    {
        Instantiate(speedObject, new Vector3(Random.Range(lowerX, upperX), Random.Range(lowerY, upperY), 0), speedObject.transform.rotation);
    }
    public void slow()
    {
        Instantiate(slowObject, new Vector3(Random.Range(lowerX, upperX), Random.Range(lowerY, upperY), 0), slowObject.transform.rotation);
    }
    public void scrambledEggs()
    {
        Instantiate(scramblerObject, new Vector3(Random.Range(lowerX, upperX), Random.Range(lowerY, upperY), 0), scramblerObject.transform.rotation);
    }

    public void glitcher()
    {
        Instantiate(glitchObject, new Vector3(Random.Range(lowerX, upperX), Random.Range(lowerY, upperY), 0), glitchObject.transform.rotation);
    }

    public void loadFinder()
    {
        Instantiate(finderObject, new Vector3(Random.Range(lowerX, upperX), Random.Range(lowerY, upperY), 0), finderObject.transform.rotation);
    }

    public void setColors(GameObject[] objArray, Color color)
    {
        for (int i = 0; i < objArray.Length; i++)
        {
            GameObject theOneRingToRuleThemAll = objArray[i];
            theOneRingToRuleThemAll.GetComponent<Renderer>().material.color = color;
        }
    }

    public void findCountry(GameObject country)
    {
        Instantiate(beaconObject, new Vector3(country.transform.position.x + Random.Range(-.25f,.25f),country.transform.position.y + Random.Range(-.25f, .25f), 0), country.transform.rotation);
    }
}
