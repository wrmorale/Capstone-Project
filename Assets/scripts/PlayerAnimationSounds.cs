using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationSounds : MonoBehaviour
{

    AudioSource animationSoundPlayer;
    public AudioClip runsfx;
    public AudioClip attack1sfx;
    public AudioClip attack2sfx;
    public AudioClip attack3sfx;


    // Start is called before the first frame update
    void Start()
    {
        animationSoundPlayer = GetComponent<AudioSource>();
    }

    private void playRunsfx(){
        animationSoundPlayer.PlayOneShot(runsfx, 1.0F);
    }

    private void playAttack1sfx(){
        animationSoundPlayer.PlayOneShot(attack1sfx, 1.0F);
    }

    private void playAttack2sfx(){
        animationSoundPlayer.PlayOneShot(attack2sfx, 1.0F);
    }

    private void playAttack3sfx(){
        animationSoundPlayer.PlayOneShot(attack3sfx, 1.0F);
    }

}
