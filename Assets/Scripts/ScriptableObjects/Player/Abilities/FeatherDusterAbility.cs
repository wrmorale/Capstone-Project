using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "Abilities/FeatherDuster")]
public class FeatherDusterAbility : PlayerAbility {
    public int projectileDamage = 1;
    public float projectileRange = 50f;
    public Rigidbody projectile;

    private FeatherDusterTriggerable launcher;

    public override void Initialize(GameObject obj)
    {
        launcher = obj.GetComponent<FeatherDusterTriggerable>();
        launcher.projectile = projectile;
    }

    public override void TriggerAbility() 
    {
        launcher.Launch();
    }
}
