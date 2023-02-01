// Code provided by Nahuel Gladstein
// https://www.gamedeveloper.com/design/frame-specific-attacks-in-unity
namespace Extensions
{
    using System;
    using UnityEngine;
    public static class AnimatorExtension
    {
        public static bool isPlayingOnLayer(this Animator animator, int fullPathHash, int layer)
        {
            return animator.GetCurrentAnimatorStateInfo(layer).fullPathHash == fullPathHash;
        }

        public static double normalizedTime(this Animator animator, System.Int32 layer)
        {
            double time = animator.GetCurrentAnimatorStateInfo(layer).normalizedTime;
            return time > 1 ? 1 : time;
        }
    }
}
