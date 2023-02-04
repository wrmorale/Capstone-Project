using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using States;
using Extensions;

public class BroomAttackManager : MonoBehaviour
{
    private playerController player;
    void Awake()
    {
        player = gameObject.GetComponent<playerController>();
    }

    // This custom update function can be called every frame from the Update() in playerController.cs to reduce overhead.
    // Only call if the player's state is Attacking.
    void updateMe()
    {

    }
}