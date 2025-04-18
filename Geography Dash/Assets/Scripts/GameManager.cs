using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    private PlayerController playerControllerScript;
    private Player2Controller player2ControllerScript;
    private SpawnManager spawnManagerScript;
    public Object Freezer;
    public Object Frozen;
    public GameObject Boost;
    public TextMeshProUGUI countryText;
    public TextMeshProUGUI countryTextRed;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI scoreTextRed;
    public TextMeshProUGUI timerText;
    public TMP_InputField scoreStart;
    public TMP_InputField timerStart;
    public static int score=0;
    public static float timeConstraint = -1;
    public static float elapsedTime = -1;
    private static float startingTime;
    public bool timeUp = false;
    public bool yes;
    private static bool freeze = true, speed = true, slow = true, scrambledEggs = true, find = true, glitch = true;
    public static bool doSoundEffects = true;
    private int time = 5;
    private int powerupChoice;

    void Start()
    {
        if (yes)
        {
            int num = 0;
            playerControllerScript = GameObject.Find("Player").GetComponent<PlayerController>();
            player2ControllerScript = GameObject.Find("Player 2").GetComponent<Player2Controller>();
            spawnManagerScript = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
            if (freeze)
                num++;
            if (speed)
                num++;
            if (slow)
                num++;
            if (scrambledEggs)
                num++;
            if (find)
                num++;
            if (glitch)
                num++;
            StartCoroutine(Wait(num));
        }
     }

    void Update()
    {
        if (yes)
        {
            countryText.text = playerControllerScript.countries[playerControllerScript.index].name;
            countryTextRed.text = playerControllerScript.countries[player2ControllerScript.index].name;
            scoreText.text = "Score: " + playerControllerScript.score;
            scoreTextRed.text = "Score: " + player2ControllerScript.score;
            if (playerControllerScript.score >= score)
                SceneManager.LoadScene(2);
            if (player2ControllerScript.score >= score)
                SceneManager.LoadScene(3);
            if (timeUp && playerControllerScript.score >= player2ControllerScript.score)
                SceneManager.LoadScene(2);
            if (timeUp && player2ControllerScript.score > playerControllerScript.score)
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
    IEnumerator Wait(int numOfPowerUp)
    {
        while (numOfPowerUp>0)
        {
            yield return new WaitForSeconds(time);
            powerupChoice = (int)Random.Range(1, numOfPowerUp+ 1);
            if (!freeze)
                powerupChoice++;
            if (!speed && powerupChoice>1)
                powerupChoice++;
            if (!slow && powerupChoice>2)
                powerupChoice++;
            if (!scrambledEggs && powerupChoice>3)
                powerupChoice++;
            if (!find && powerupChoice > 4)
                powerupChoice++;
            if (powerupChoice == 1)
                spawnManagerScript.freeze();
            if (powerupChoice == 2)
                spawnManagerScript.speed();
            if (powerupChoice == 3)
                spawnManagerScript.slow();
            if (powerupChoice == 4)
                spawnManagerScript.scrambledEggs();
            if (powerupChoice == 5)
                spawnManagerScript.loadFinder();
            if (powerupChoice == 6)
                spawnManagerScript.glitcher();
        }
    }

    public void Freeze()
    {
        if (freeze)
            freeze = false;
        else
            freeze = true;
    }
    public void Fast()
    {
        if (speed)
            speed = false;
        else
            speed = true;
    }
    public void Slow()
    {
        if (slow)
            slow = false;
        else
            slow = true;
    }
    public void ScrambledEggs()
    {
        if (scrambledEggs)
            scrambledEggs = false;
        else
            scrambledEggs = true;
    }
    public void Finder()
    {
        if (find)
            find = false;
        else find = true;
    }
    public void Glitcher()
    {
        if (glitch)
            glitch = false;
        else glitch = true;
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
