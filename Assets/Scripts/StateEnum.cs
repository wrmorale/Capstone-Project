using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace States
{
    public enum PlayerStates
    {
        Idle,
        Walking,
        Running,
        Jumping,
        Falling,
        Attacking,
        Blocking,
        AirAttacking,
        AirBlocking,
        Talking,
        Unknown,
    }
}