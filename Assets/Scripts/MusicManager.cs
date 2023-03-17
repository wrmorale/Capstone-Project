using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{

    AudioSource audioSource;
    public AudioClip battleMusic;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void playBattleMusic(){
        audioSource.PlayOneShot(battleMusic, 1.0F);
    }

}
