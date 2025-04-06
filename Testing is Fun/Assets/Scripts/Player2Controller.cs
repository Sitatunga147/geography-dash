using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player2Controller : MonoBehaviour
{
    public float horizontalInput;
    public float verticalInput;
    private float speed = 3;
    public int index;
    public int score = 0;
    private PlayerController playerControllerScript;
    private SpawnManager spawnManagerScript;
    public AudioClip correctAudio;
    public AudioClip boostAudio;
    public AudioClip slowAudio;
    public AudioClip scrambleAudio;
    public AudioClip freezeAudio;
    public AudioClip finderAudio;
    public AudioClip buggerAudio;
    private AudioSource playerAudio;
    public bool scrambled = false;
    public int scrambledEggCookTime;
    private int boost = 2;
    private int boostTime = 5;
    private bool isFroozen = false;
    private int freezeTime = 4;
    private bool glitcher = false;
    private int spedometer = 0;
    private int buggingTime = 5;

    private float latitudeBound = 10;
    private float longitudeBound = 5;

    // Start is called before the first frame update
    void Start()
    {
        playerControllerScript = GameObject.Find("Player").GetComponent<PlayerController>();
        spawnManagerScript = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();

        correctAudio = playerControllerScript.correct1;
        boostAudio = playerControllerScript.boostAudio;
        slowAudio = playerControllerScript.slowAudio;
        scrambleAudio = playerControllerScript.scrambleAudio;
        freezeAudio = playerControllerScript.freezeAudio;
        playerAudio = GetComponent<AudioSource>();

        index = Random.Range(0, playerControllerScript.countries.Length);
    }

    // Update is called once per frame
    void Update()
    {
        if (!isFroozen)
        {
            horizontalInput = Input.GetAxis("Fire1");
            verticalInput = Input.GetAxis("Fire2");

            if (glitcher)
            {
                if (Random.Range(0, 2) == 0)
                {
                    spedometer++;
                    speed += boost;
                }
                else
                {
                    spedometer--;
                    speed -= boost;
                }
            }
            if (scrambled)
            {
                if (horizontalInput < 0)
                    transform.Translate(Vector2.right * Time.smoothDeltaTime * -horizontalInput * speed);
                if (horizontalInput > 0)
                    transform.Translate(Vector2.up * Time.smoothDeltaTime * horizontalInput * speed);
                if (verticalInput > 0)
                    transform.Translate(Vector2.down * Time.smoothDeltaTime * verticalInput * speed);
                if (verticalInput < 0)
                    transform.Translate(Vector2.left * Time.smoothDeltaTime * -verticalInput * speed);

            }
            else
            {
                transform.Translate(Vector2.right * Time.smoothDeltaTime * horizontalInput * speed);
                transform.Translate(Vector2.up * Time.smoothDeltaTime * verticalInput * speed);
            }

            if (transform.position.x > latitudeBound)
            {
                transform.position = new Vector2(-latitudeBound + 1, transform.position.y);
            }
            if (transform.position.x < -latitudeBound)
            {
                transform.position = new Vector2(latitudeBound - 1, transform.position.y);
            }

            if (transform.position.y > longitudeBound)
            {
                transform.position = new Vector2(transform.position.x, -longitudeBound + 1);
            }
            if (transform.position.y < -longitudeBound)
            {
                transform.position = new Vector2(transform.position.x, longitudeBound - 1);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject == playerControllerScript.countries[index])
        {
            score += 1;
            playerAudio.PlayOneShot(correctAudio, 1.0f);
            index = Random.Range(0, playerControllerScript.countries.Length);
        }
        if (collider.gameObject.CompareTag("ScrambledEggs"))
        {
            playerAudio.PlayOneShot(scrambleAudio, 1.0f);
            playerControllerScript.scramblered();
            Destroy(collider.gameObject);
            StartCoroutine(waitForEggs());
        }
        if (collider.gameObject.CompareTag("Boost"))
        {
            playerAudio.PlayOneShot(boostAudio, 1.0f);
            speed += boost;
            Destroy(collider.gameObject);
            StartCoroutine(waitForBoost());
        }
        if (collider.gameObject.CompareTag("Slow"))
        {
            playerAudio.PlayOneShot(slowAudio, 1.0f);
            playerControllerScript.slow();
            Destroy(collider.gameObject);
        }
        if (collider.gameObject.CompareTag("Freeze"))
        {
            playerAudio.PlayOneShot(freezeAudio, 1.0f);
            playerControllerScript.freeze();
            Destroy(collider.gameObject);
        }
        if (collider.gameObject.CompareTag("Finder"))
        {
            playerAudio.PlayOneShot(finderAudio, 1.0f);
            searchAndRescue();
            Destroy(collider.gameObject);
        }
        if (collider.gameObject.CompareTag("Glitch"))
        {
            playerAudio.PlayOneShot(buggerAudio, 1.0f);
            playerControllerScript.glitch();
            Destroy(collider.gameObject);
        }
    }

    public void freeze()
    {
        StartCoroutine(Freezing());
    }

    IEnumerator Freezing()
    {
        isFroozen = true;
        spawnManagerScript.frozenPlayer2();
        yield return new WaitForSeconds(freezeTime);
        isFroozen = false;
    }

    public void slow()
    {
        speed -= boost;
        StartCoroutine(waitForSlow());
    }

    IEnumerator waitForBoost()
    {
        yield return new WaitForSeconds(boostTime);
        speed -= boost;
    }

    IEnumerator waitForSlow()
    {
        yield return new WaitForSeconds(boostTime);
        speed += boost;
    }

    IEnumerator waitForEggs()
    {
        yield return new WaitForSeconds(scrambledEggCookTime);
        playerControllerScript.scrambled = false;

    }

    public void scramblered()
    {
        Debug.Log("Wassup");
        scrambled = true;
    }

    public void glitch()
    {
        StartCoroutine(deBugger());
        glitcher = true;
    }

    IEnumerator deBugger()
    {
        yield return new WaitForSeconds(buggingTime);
        glitcher = false;
        speed -= spedometer * boost;
        spedometer = 0;
    }

    public void searchAndRescue()
    {
        spawnManagerScript.findCountry(playerControllerScript.countries[index]);
    }
}
