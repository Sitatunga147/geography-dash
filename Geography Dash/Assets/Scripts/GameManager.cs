using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    private PlayerController p1Controller;
    private Player2Controller p2Controller;
    private SpawnManager spawnManager;
    public Object Freezer;
    public Object Frozen;
    public GameObject Boost;
    public TextMeshProUGUI p1CountryText;
    public TextMeshProUGUI p2CountryText;
    public TextMeshProUGUI p1ScoreText;
    public TextMeshProUGUI p2ScoreText;
    public TextMeshProUGUI timerText;
    public TMP_InputField scoreStart;
    public TMP_InputField timerStart;
    public static int score=0;
    public static float timeConstraint = -1;
    public static float elapsedTime = -1;
    private static float startingTime;
    public bool timeUp = false;
    public bool readyForComponents;
    private static bool doFreeze, doSpeed, doSlow, doScrambler, doFinder, doGlitch;
    public static bool doSoundEffects = true;
    private int time = 5;
    private int powerupChoice;

    void Start()
    {
        if (SceneManager.GetActiveScene().name.Equals("HomeScene"))
        {
            doFreeze = true;
            doSpeed = true;
            doSlow = true;
            doScrambler = true;
            doFinder = true;
            doGlitch = true;
        }

        if (readyForComponents)
        {
            int numPowerups = 0;
            p1Controller = GameObject.Find("Player").GetComponent<PlayerController>();
            p2Controller = GameObject.Find("Player 2").GetComponent<Player2Controller>();
            spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
            if (doFreeze)
                numPowerups++;
            if (doSpeed)
                numPowerups++;
            if (doSlow)
                numPowerups++;
            if (doScrambler)
                numPowerups++;
            if (doFinder)
                numPowerups++;
            if (doGlitch)
                numPowerups++;
            StartCoroutine(Wait(numPowerups));
        }
     }

    void Update()
    {
        if (readyForComponents)
        {
            p1CountryText.text = p1Controller.countries[p1Controller.countryI].name;
            p2CountryText.text = p1Controller.countries[p2Controller.index].name;
            p1ScoreText.text = "Score: " + p1Controller.score;
            p2ScoreText.text = "Score: " + p2Controller.score;
            if (p1Controller.score >= score)
                SceneManager.LoadScene(2);
            if (p2Controller.score >= score)
                SceneManager.LoadScene(3);
            if (timeUp && p1Controller.score >= p2Controller.score)
                SceneManager.LoadScene(2);
            if (timeUp && p2Controller.score > p1Controller.score)
                SceneManager.LoadScene(3);

            if(timeConstraint == -1 && SceneManager.GetActiveScene().buildIndex == 1)
            {
                timerText.text = "";
            }
            else if(!timeUp && SceneManager.GetActiveScene().buildIndex == 1)
            {
                RunTimer();
            }
        }
    }
    IEnumerator Wait(int numPowerups)
    {
        while (numPowerups>0)
        {
            yield return new WaitForSeconds(time);
            powerupChoice = (int)Random.Range(1, numPowerups+ 1);
            if (!doFreeze)
                powerupChoice++;
            if (!doSpeed && powerupChoice>1)
                powerupChoice++;
            if (!doSlow && powerupChoice>2)
                powerupChoice++;
            if (!doScrambler && powerupChoice>3)
                powerupChoice++;
            if (!doFinder && powerupChoice > 4)
                powerupChoice++;
            if (powerupChoice == 1)
                spawnManager.freeze();
            if (powerupChoice == 2)
                spawnManager.speed();
            if (powerupChoice == 3)
                spawnManager.slow();
            if (powerupChoice == 4)
                spawnManager.scrambledEggs();
            if (powerupChoice == 5)
                spawnManager.loadFinder();
            if (powerupChoice == 6)
                spawnManager.glitcher();
        }
    }

    public void Freeze()
    {
        if (doFreeze)
            doFreeze = false;
        else
            doFreeze = true;
    }
    public void Fast()
    {
        if (doSpeed)
            doSpeed = false;
        else
            doSpeed = true;
    }
    public void Slow()
    {
        if (doSlow)
            doSlow = false;
        else
            doSlow = true;
    }
    public void ScrambledEggs()
    {
        if (doScrambler)
            doScrambler = false;
        else
            doScrambler = true;
    }
    public void Finder()
    {
        if (doFinder)
            doFinder = false;
        else doFinder = true;
    }
    public void Glitcher()
    {
        if (doGlitch)
            doGlitch = false;
        else doGlitch = true;
    }
    public void SoundEffects()
    {
        if (doSoundEffects)
        {
            doSoundEffects = false;
        }
        else
        {
            doSoundEffects = true;
        }
    }


    public void StartWithScore()
    {
        score = int.Parse(scoreStart.text);
        SceneManager.LoadScene(1);

        startingTime = Time.time;
    }

    public void StartWithTimer()
    {
        score = int.MaxValue;
        timeConstraint = float.Parse(timerStart.text);
        SceneManager.LoadScene(1);

        startingTime = Time.time;
    }

    private void RunTimer()
    {
        float deltaSeconds = Time.time - startingTime;
        if(deltaSeconds > timeConstraint*60)
        {
            timeUp = true;
        }

        int totalMins = (int)(timeConstraint);
        int totalSecs = (int)(60*timeConstraint);

        int secsLeft = (int)(totalSecs - deltaSeconds);

        int minsLeft = (int)(secsLeft / 60);
        int unroundedDispSecs = (int)((totalSecs - deltaSeconds)%60);

        if((totalSecs - deltaSeconds)%60 < 10)
        {
            timerText.text = (minsLeft).ToString() + ":0" + unroundedDispSecs.ToString();
        }
        else
        {
            timerText.text = (minsLeft).ToString() + ":" + unroundedDispSecs.ToString();
        }
        
    }

    public void Home()
    {
        SceneManager.LoadScene(0);
        timeConstraint = -1;
        score = 0;
    }    
}
