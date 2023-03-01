using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerAbility : MonoBehaviour
{
    public virtual float baseCooldown
    {
        get { return 1f;}
    }
    [HideInInspector] public float cooldownTimer = 0f;
    public abstract void Activate();
    public abstract void UpdateCooldown(float time);
    public bool IsReady() 
    {
        return (cooldownTimer <= 0f);
    }
}
