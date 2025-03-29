using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Player2Controller player2ControllerScript;
    private SpawnManager spawnManagerScript;
    public float horizontalInput;
    public float verticalInput;
    private float speed = 3;
    public GameObject[] countries;
    public int index;
    public int score = 0;
    public AudioClip correct1;
    public AudioClip boostAudio;
    public AudioClip slowAudio;
    public AudioClip scrambleAudio;
    public AudioClip freezeAudio;
    private AudioSource playerAudio;
    public int scrambledEggCookTime;
    public bool scrambled = false;
    private float boost = 2;
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
        player2ControllerScript = GameObject.Find("Player 2").GetComponent<Player2Controller>();
        spawnManagerScript = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        index = Random.Range(0, countries.Length);
        playerAudio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isFroozen)
        {
            horizontalInput = Input.GetAxis("Horizontal");
            verticalInput = Input.GetAxis("Vertical");

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
                    transform.Translate(Vector2.right * Time.deltaTime * -horizontalInput * speed);
                if (horizontalInput > 0)
                    transform.Translate(Vector2.up * Time.deltaTime * horizontalInput * speed);
                if (verticalInput > 0)
                    transform.Translate(Vector2.down * Time.deltaTime * verticalInput * speed);
                if (verticalInput < 0)
                    transform.Translate(Vector2.left * Time.deltaTime * -verticalInput * speed);

            }
            else
            {
                transform.Translate(Vector2.right * Time.deltaTime * horizontalInput * speed);
                transform.Translate(Vector2.up * Time.deltaTime * verticalInput * speed);
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
        if(collider.gameObject == countries[index])
        {
            score += 1;
            playerAudio.PlayOneShot(correct1, 1.0f);
            index = Random.Range(0, countries.Length);
        }
        if(collider.gameObject.CompareTag("ScrambledEggs"))
        {
            playerAudio.PlayOneShot(scrambleAudio, 1.0f);
            player2ControllerScript.scramblered();
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
            player2ControllerScript.slow();
            Destroy(collider.gameObject);
        }
        if (collider.gameObject.CompareTag("Freeze"))
        {
            playerAudio.PlayOneShot(freezeAudio, 1.0f);
            player2ControllerScript.freeze();
            Destroy(collider.gameObject);
        }
        if (collider.gameObject.CompareTag("Finder"))
        {
            Destroy(collider.gameObject);
        }
        if (collider.gameObject.CompareTag("Glitch"))
        {
            player2ControllerScript.glitch();
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
        spawnManagerScript.frozenPlayer();
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
        player2ControllerScript.scrambled = false;

    }

    public void scramblered()
    {
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
}
