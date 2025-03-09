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
    public AudioClip correct1;
    private AudioSource playerAudio;
    public bool scrambled = false;
    public int scrambledEggCookTime;
    private int boost = 2;
    private int boostTime = 5;
    private bool isFroozen = false;
    private int freezeTime = 4;

    private float latitudeBound = 10;
    private float longitudeBound = 5;

    // Start is called before the first frame update
    void Start()
    {
        playerControllerScript = GameObject.Find("Player").GetComponent<PlayerController>();
        spawnManagerScript = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
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
        if (collider.gameObject == playerControllerScript.countries[index])
        {
            score += 1;
            playerAudio.PlayOneShot(correct1, 1.0f);
            index = Random.Range(0, playerControllerScript.countries.Length);
        }
        if (collider.gameObject.CompareTag("ScrambledEggs"))
        {
            playerControllerScript.scramblered();
            Destroy(collider.gameObject);
            StartCoroutine(waitForEggs());
        }
        if (collider.gameObject.CompareTag("Boost"))
        {
            speed += boost;
            Destroy(collider.gameObject);
            StartCoroutine(waitForBoost());
        }
        if (collider.gameObject.CompareTag("Slow"))
        {
            playerControllerScript.slow();
            Destroy(collider.gameObject);
        }
        if (collider.gameObject.CompareTag("Freeze"))
        {
            playerControllerScript.freeze();
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
}
