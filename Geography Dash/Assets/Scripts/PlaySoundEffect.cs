using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySoundEffect : MonoBehaviour
{
    public AudioClip sound;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        AudioSource audio = GetComponent<AudioSource>();
        audio.PlayOneShot(sound);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
