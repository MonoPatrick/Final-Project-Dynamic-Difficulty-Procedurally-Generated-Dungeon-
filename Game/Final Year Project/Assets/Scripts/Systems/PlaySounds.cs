using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySounds : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip footstepClip;
    public AudioClip attackingClip;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void playfootstep()
    {
        

        audioSource.PlayOneShot(footstepClip);

        float randomNumber = Random.Range(0.05f, 0.1f);
        audioSource.volume = randomNumber;
        randomNumber = Random.Range(0.8f, 1.3f);
        audioSource.pitch = randomNumber;   
    }
    public void playAttack()
    {
        

        audioSource.PlayOneShot(attackingClip);
        float randomNumber = Random.Range(0.05f, 0.1f);
        audioSource.volume = randomNumber;

        randomNumber = Random.Range(0.8f, 1.3f);
        audioSource.pitch = randomNumber;
    }
}
