using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This script will play animations for all weapons and set attack cooldowns for each.

public class weaponController : MonoBehaviour
{
    public playerController pc;
    [SerializeField]
    public GameObject weapon1;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(pc.state == States.PlayerStates.Attacking){
            Debug.Log("showing weapon");
            weapon1.SetActive(true);
        }
        else{
            weapon1.SetActive(false);
        }
    }
}
