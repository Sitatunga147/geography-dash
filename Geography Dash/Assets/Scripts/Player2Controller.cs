using System.Collections;
using UnityEngine;

public class Player2Controller : MonoBehaviour
{
    public float horizontalInput;
    public float verticalInput;
    private float speed = 3;

    public int countryIndex;
    public int lastCountryIndex;
    public int score = 0;

    private PlayerController playerControllerScript;
    private SpawnManager spawnManagerScript;
    private GameManager gameManagerScript;
    private GameObject finder;
    public AudioClip correctAudio;
    public AudioClip speedAudio;
    public AudioClip slowAudio;
    public AudioClip scrambleAudio;
    public AudioClip freezeAudio;
    public AudioClip finderAudio;
    public AudioClip glitchAudio;
    private AudioSource playerAudio;

    public bool isScrambled = false;
    public bool doSoundEffects, doSinglePlayer;
    public int scrambleDuration;
    private int speedBoost = 2;
    private int speedDuration = 5;
    private bool isFrozen = false;
    private int freezeDuration = 4;
    private bool isGlitched = false;
    private int glitchSpeedometer = 0;
    private int glitchDuration = 5;

    private float latitudeBound = 10;
    private float longitudeBound = 5;
    
    private float AITargetX;
    private float AITargetY;

    // Start is called before the first frame update
    void Start()
    {
        playerControllerScript = GameObject.Find("Player").GetComponent<PlayerController>();
        spawnManagerScript = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();

        finder = GameObject.Find("Finder");

        doSoundEffects = GameManager.doSoundEffects;
        doSinglePlayer = GameManager.doSinglePlayer;

        correctAudio = playerControllerScript.correctAudio;
        speedAudio = playerControllerScript.speedAudio;
        slowAudio = playerControllerScript.slowAudio;
        scrambleAudio = playerControllerScript.scrambleAudio;
        freezeAudio = playerControllerScript.freezeAudio;
        playerAudio = GetComponent<AudioSource>();

        lastCountryIndex = -1;
        countryIndex = Random.Range(0, playerControllerScript.countries.Length);
    }

    // Update is called once per frame
    void Update()
    {
        if (!isFrozen)
        {
            if (doSinglePlayer) {
                speed = 2;
                GenerateAITarget();
                float cutoff = 0.5f;
            	if (AITargetX > transform.position.x) {
            	    horizontalInput = 1;
            	} else if (AITargetX < transform.position.x) {
            	    horizontalInput = -1;
            	} else {
            	    horizontalInput = 0;
            	}
                if (AITargetY > transform.position.y) {
            	    verticalInput = 1;
            	} else if (AITargetY < transform.position.y) {
            	    verticalInput = -1;
            	} else {
            	    verticalInput = 0;
            	}
            }
            else {
                horizontalInput = Input.GetAxisRaw("Fire1");
                verticalInput = Input.GetAxisRaw("Fire2");
            }

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
        if (collider.gameObject == playerControllerScript.countries[countryIndex])
        {
            if (doSoundEffects)
            {
                playerAudio.PlayOneShot(correctAudio, 1.0f);
            }
            score += 1;
            lastCountryIndex = countryIndex;
            while(countryIndex == lastCountryIndex)
            {
                countryIndex = Random.Range(0, playerControllerScript.countries.Length);
            }
        }
        if (collider.gameObject.CompareTag("ScrambledEggs"))
        {
            if (doSoundEffects)
            {
                playerAudio.PlayOneShot(scrambleAudio, 2.5f);
            }
            playerControllerScript.DoScrambler();
            Destroy(collider.gameObject);
            StartCoroutine(AwaitScramblerEnd());
        }
        if (collider.gameObject.CompareTag("Boost"))
        {
            if (doSoundEffects)
            {
                playerAudio.PlayOneShot(speedAudio, 1.0f);
            }
            speed += speedBoost;
            Destroy(collider.gameObject);
            StartCoroutine(AwaitSpeedEnd());
        }
        if (collider.gameObject.CompareTag("Slow"))
        {
            if (doSoundEffects)
            {
                playerAudio.PlayOneShot(slowAudio, 5.0f);
            }
            playerControllerScript.DoSlow();
            Destroy(collider.gameObject);
        }
        if (collider.gameObject.CompareTag("Freeze"))
        {
            if (doSoundEffects)
            {
                playerAudio.PlayOneShot(freezeAudio, 1.0f);
            }
            playerControllerScript.DoFreeze();
            Destroy(collider.gameObject);
        }
        if (collider.gameObject.CompareTag("Finder"))
        {
            if (doSoundEffects)
            {
                playerAudio.PlayOneShot(finderAudio, 1.0f);
            }
            DoFinder();
            Destroy(collider.gameObject);
        }
        if (collider.gameObject.CompareTag("Glitch"))
        {
            if (doSoundEffects)
            {
                playerAudio.PlayOneShot(glitchAudio, 1.0f);
            }
            playerControllerScript.DoGlitch();
            Destroy(collider.gameObject);
        }
    }

    public void DoFreeze()
    {
        StartCoroutine(Freezing());
    }

    IEnumerator Freezing()
    {
        isFrozen = true;
        spawnManagerScript.FreezeP2();
        yield return new WaitForSeconds(freezeDuration);
        isFrozen = false;
    }

    public void DoSlow()
    {
        speed -= speedBoost;
        StartCoroutine(AwaitSlowEnd());
    }

    IEnumerator AwaitSpeedEnd()
    {
        yield return new WaitForSeconds(speedDuration);
        speed -= speedBoost;
    }

    IEnumerator AwaitSlowEnd()
    {
        yield return new WaitForSeconds(speedDuration);
        speed += speedBoost;
    }

    IEnumerator AwaitScramblerEnd()
    {
        yield return new WaitForSeconds(scrambleDuration);
        playerControllerScript.isScrambled = false;

    }

    public void DoScrambler()
    {
        isScrambled = true;
    }

    public void Glitch()
    {
        StartCoroutine(AwaitGlitchEnd());
        isGlitched = true;
    }

    IEnumerator AwaitGlitchEnd()
    {
        yield return new WaitForSeconds(glitchDuration);
        isGlitched = false;
        speed -= glitchSpeedometer * speedBoost;
        glitchSpeedometer = 0;
    }

    public void DoFinder()
    {
        spawnManagerScript.InstantiateBeacon(playerControllerScript.countries[countryIndex]);
    }

    public void GenerateAITarget() {
        // needs to be worked on
        // AITargetOptionsX.Clear();
        // AITargetOptionsY.Clear();

        // // random location to keep things fair for player
        // AITargetOptionsX.Add(Random.Range(-9, 9));
        // AITargetOptionsY.Add(Random.Range(-3, 3));
        
        // // true location
        // // AITargetOptions.Add(new float[] [playerControllerScript.countries[index].transform.position.x, playerControllerScript.countries[index].transform.position.y]);

        // int targetIndex = Random.Range(0, AITargetOptionsX.Count);
        // AITargetX = AITargetOptionsX[targetIndex];
        // AITargetY = AITargetOptionsY[targetIndex];

        AITargetX = playerControllerScript.countries[countryIndex].transform.position.x;
        AITargetY = playerControllerScript.countries[countryIndex].transform.position.y;
    }
}
