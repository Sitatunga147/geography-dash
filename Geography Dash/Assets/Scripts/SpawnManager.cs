using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject FreezeObject;
    public GameObject GlitchObject;
    public GameObject SpeedObject;
    public GameObject SlowObject;
    public GameObject ScrambleObject;
    public GameObject FrozenObject;
    public GameObject FinderObject;
    public GameObject P1Object;
    public GameObject P2Object;
    public GameObject BeaconObject;
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
        Instantiate(FrozenObject, P1Object.transform.position, FrozenObject.transform.rotation);
    }
    public void frozenPlayer2()
    {
        Instantiate(FrozenObject, P2Object.transform.position, FrozenObject.transform.rotation);
    }
    public void freeze()
    {
        if(GameObject.FindGameObjectsWithTag("Freeze").Length == 1)
            Instantiate(FreezeObject, new Vector3(Random.Range(lowerX, upperX), Random.Range(lowerY, upperY), 0), FreezeObject.transform.rotation);
    }
    public void speed()
    {
        Instantiate(SpeedObject, new Vector3(Random.Range(lowerX, upperX), Random.Range(lowerY, upperY), 0), SpeedObject.transform.rotation);
    }
    public void slow()
    {
        Instantiate(SlowObject, new Vector3(Random.Range(lowerX, upperX), Random.Range(lowerY, upperY), 0), SlowObject.transform.rotation);
    }
    public void scrambledEggs()
    {
        Instantiate(ScrambleObject, new Vector3(Random.Range(lowerX, upperX), Random.Range(lowerY, upperY), 0), ScrambleObject.transform.rotation);
    }

    public void glitcher()
    {
        Instantiate(GlitchObject, new Vector3(Random.Range(lowerX, upperX), Random.Range(lowerY, upperY), 0), GlitchObject.transform.rotation);
    }

    public void loadFinder()
    {
        Instantiate(FinderObject, new Vector3(Random.Range(lowerX, upperX), Random.Range(lowerY, upperY), 0), FinderObject.transform.rotation);
    }

    public void setColors(GameObject[] objectArray, Color color)
    {
        for (int i = 0; i < objectArray.Length; i++)
        {
            GameObject theOneRingToRuleThemAll = objectArray[i];
            theOneRingToRuleThemAll.GetComponent<Renderer>().material.color = color;
        }
    }

    public void findCountry(GameObject country)
    {
        Instantiate(BeaconObject, new Vector3(country.transform.position.x + Random.Range(-.25f,.25f),country.transform.position.y + Random.Range(-.25f, .25f), 0), country.transform.rotation);
    }
}
