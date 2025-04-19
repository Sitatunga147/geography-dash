using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Player2Controller player2ControllerScript;
    private SpawnManager spawnManagerScript;
    private GameManager gameManagerScript;
    public GameObject[] countries;

    public AudioClip correctAudio;
    public AudioClip speedAudio;
    public AudioClip slowAudio;
    public AudioClip scrambleAudio;
    public AudioClip freezeAudio;
    public AudioClip finderAudio;
    public AudioClip glitchAudio;
    private AudioSource p2AudioSource;

    public float horizontalInput;
    public float verticalInput;
    private float speed = 3;

    public int countryI;
    public int score = 0;
    
    public int scrambleDuration;
    public bool isScrambled = false;
    public bool doSoundEffects;
    private float speedBoost = 2;
    private int speedDuration = 5;
    private bool isFrozen = false;
    private int freezeDuration = 4;
    private bool isGlitched = false;
    private int glitchSpeedometer = 0;
    private int glitchDuration = 5;

    private float latitudeBound = 10;
    private float longitudeBound = 5;

    // Start is called before the first frame update
    void Start()
    {
        player2ControllerScript = GameObject.Find("Player 2").GetComponent<Player2Controller>();
        spawnManagerScript = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        gameManagerScript = GameObject.Find("GameManager").GetComponent<GameManager>();

        doSoundEffects = GameManager.doSoundEffects;

        countryI = Random.Range(0, countries.Length);
        p2AudioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isFrozen)
        {
            horizontalInput = Input.GetAxisRaw("Horizontal");
            verticalInput = Input.GetAxisRaw("Vertical");

            if (isGlitched)
            {
                if (Random.Range(0, 2) == 0)
                {
                    glitchSpeedometer++;
                    speed += speedBoost;
                }
                else
                {
                    glitchSpeedometer--;
                    speed -= speedBoost;
                }
            }

            if (isScrambled)
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
        if(collider.gameObject == countries[countryI])
        {
            if (doSoundEffects)
            {
                p2AudioSource.PlayOneShot(correctAudio, 1.0f);
            } 

            score += 1;
            countryI = Random.Range(0, countries.Length);
        }
        if(collider.gameObject.CompareTag("ScrambledEggs"))
        {
            if (doSoundEffects)
            {
                p2AudioSource.PlayOneShot(scrambleAudio, 2.5f);
            }

            player2ControllerScript.scramblered();
            Destroy(collider.gameObject);
            StartCoroutine(waitForEggs());
        }
        if (collider.gameObject.CompareTag("Boost"))
        {
            if (doSoundEffects)
            {
                p2AudioSource.PlayOneShot(speedAudio, 1.0f);
            }

            speed += speedBoost;
            Destroy(collider.gameObject);
            StartCoroutine(waitForBoost());
        }
        if (collider.gameObject.CompareTag("Slow"))
        {
            if (doSoundEffects)
            {
                p2AudioSource.PlayOneShot(slowAudio, 5.0f);
            }

            player2ControllerScript.Slow();
            Destroy(collider.gameObject);
        }
        if (collider.gameObject.CompareTag("Freeze"))
        {
            if (doSoundEffects)
            {
                p2AudioSource.PlayOneShot(freezeAudio, 1.0f);
            }

            player2ControllerScript.freeze();
            Destroy(collider.gameObject);
        }
        if (collider.gameObject.CompareTag("Finder"))
        {
            if (doSoundEffects)
            {
                p2AudioSource.PlayOneShot(finderAudio, 1.0f);
            }

            searchAndRescue();
            Destroy(collider.gameObject);
        }
        if (collider.gameObject.CompareTag("Glitch"))
        {
            if (doSoundEffects)
            {
                p2AudioSource.PlayOneShot(glitchAudio, 1.0f);
            }

            player2ControllerScript.Glitch();
            Destroy(collider.gameObject);
        }
    }

    public void freeze()
    {
        StartCoroutine(Freezing());
    }

    IEnumerator Freezing()
    {
        isFrozen = true;
        spawnManagerScript.frozenPlayer();
        yield return new WaitForSeconds(freezeDuration);
        isFrozen = false;
    }

    public void slow()
    {
        speed -= speedBoost;
        StartCoroutine(waitForSlow());
    }

    IEnumerator waitForBoost()
    {
        yield return new WaitForSeconds(speedDuration);
        speed -= speedBoost;
    }

    IEnumerator waitForSlow()
    {
        yield return new WaitForSeconds(speedDuration);
        speed += speedBoost;
    }

    IEnumerator waitForEggs()
    {
        yield return new WaitForSeconds(scrambleDuration);
        player2ControllerScript.scrambled = false;

    }

    public void scramblered()
    {
        isScrambled = true;
    }

    public void glitch()
    {
        StartCoroutine(deBugger());
        isGlitched = true;
    }

    IEnumerator deBugger()
    {
        yield return new WaitForSeconds(glitchDuration);
        isGlitched = false;
        speed -= glitchSpeedometer * speedBoost;
        glitchSpeedometer = 0;
    }
    public void searchAndRescue()
    {
        spawnManagerScript.findCountry(countries[countryI]);
    }
}
