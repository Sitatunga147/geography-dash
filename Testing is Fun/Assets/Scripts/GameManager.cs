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
    public TMP_InputField scoreGoal;
    public static int score=0;
    public bool yes;
    private static bool freeze = true, speed = true, slow = true, scrambledEggs = true, find = true;
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
            StartCoroutine(wait(num));
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
        }
    }
    IEnumerator wait(int numOfPowerUp)
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
        if (!scrambledEggs)
            scrambledEggs = true;
        else
            scrambledEggs = false;
    }
    public void Finder()
    {
        if (find)
            find = false;
        else find = true;
    }


    public void StartScene()
    {
        score = int.Parse(scoreGoal.text);
        SceneManager.LoadScene(1);
    }

    public void Home()
    {
        SceneManager.LoadScene(0);
    }    
}
