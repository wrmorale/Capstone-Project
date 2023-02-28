using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "Abilities/FeatherDuster")]
public class FeatherDusterAbility : PlayerAbility {
    public float projectileDamage = 1f;
    public float projectileLifetime = 1f;
    public Rigidbody projectile;
    public FrameParser throwClip;
    public FrameChecker throwFrameChecker;

    private FeatherDusterTriggerable launcher;

    public override void Initialize(GameObject obj)
    {
        launcher = obj.GetComponent<FeatherDusterTriggerable>();
        launcher.projectile = projectile;
        launcher.throwClip = throwClip;
        launcher.throwFrameChecker = throwFrameChecker;
        launcher.damage = projectileDamage;
    }

    public override void TriggerAbility() 
    {
        launcher.Launch();
    }
}
