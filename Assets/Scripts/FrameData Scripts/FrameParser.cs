using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Extensions;
// Based on code provided by Nahuel Gladstein
// https://www.gamedeveloper.com/design/frame-specific-attacks-in-unity
[System.Serializable]
public class FrameParser
{
    public Animator animator;
    public AnimationClip clip;
    public string animatorStateName;
    public int layerNumber;

    private string name;
    private int _totalFrames = 0;
    private int _animationFullNameHash;

    public void initialize()
    {
        _totalFrames = Mathf.RoundToInt(clip.length * clip.frameRate);

        if (animator.isActiveAndEnabled) 
        {
            name = animator.GetLayerName(layerNumber) + "." + animatorStateName;

            _animationFullNameHash = Animator.StringToHash(name);
        }
    }

    public bool isActive() 
    {
        return animator.isPlayingOnLayer(_animationFullNameHash, 0);
    }

    public double percentageOnFrame(int frameNumber) 
    {
        return (double)frameNumber / (double)_totalFrames;
    }

    public bool isOnOrPastFrame(int frameNumber) 
    {
        double percentage = animator.normalizedTime(layerNumber);
        return (percentage >= percentageOnFrame(frameNumber));
    }

    public bool isOnLastFrame() 
    {
        double percentage = animator.normalizedTime(layerNumber);
        return (percentage > percentageOnFrame(_totalFrames - 1));
    }

    public int getTotalFrames()
    {
        return _totalFrames;
    }

    public void play() 
    {
        animator.Play(name, 0);
    }
}