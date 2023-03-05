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

    public static class FloatExtension
    {
        public static float map(this float value, float from1, float to1, float from2, float to2) 
        {
            return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
        }
    }
}
