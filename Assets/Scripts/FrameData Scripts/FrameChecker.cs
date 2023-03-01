// Based on code provided by Nahuel Gladstein
// https://www.gamedeveloper.com/design/frame-specific-attacks-in-unity
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IFrameCheckHandler
{
    void onActiveFrameStart();
    void onActiveFrameEnd();
    void onAttackCancelFrameStart();
    void onAttackCancelFrameEnd();
    void onAllCancelFrameStart();
    void onAllCancelFrameEnd();
    void onLastFrameStart();
    void onLastFrameEnd();
    void updateMe(float time);
}

[System.Serializable]
public class FrameChecker
{
    // frames the hitbox is out
    public int activeFrameStart;
    public int activeFrameEnd;
    // frames we can cancel into next attack of combo; specify 0, 0 for last attack of combo
    public int attackCancelFrameStart;
    public int attackCancelFrameEnd;
    // frames we can do any action except walk/run (jump, roll, block, attack, ability)
    // note: attacking during this window should reset to perform the first hit of the combo
    public int allCancelFrameStart;
    public int allCancelFrameEnd;
    private int totalFrames;

    private IFrameCheckHandler _frameCheckHandler;
    private FrameParser _frameParser;
    private bool _checkedActiveFrameStart;
    private bool _checkedActiveFrameEnd;
    private bool _checkedAttackCancelFrameStart;
    private bool _checkedAttackCancelFrameEnd;
    private bool _checkedAllCancelFrameStart;
    private bool _checkedAllCancelFrameEnd;
    private bool _lastFrame;

    public void initialize(IFrameCheckHandler frameCheckHandler, FrameParser frameParser)
    {
        _frameCheckHandler = frameCheckHandler;
        _frameParser = frameParser;
        totalFrames = frameParser.getTotalFrames();
        initCheck();
    }

    public void initCheck()
    {
        _checkedActiveFrameStart = false;
        _checkedActiveFrameEnd = false;
        _checkedAttackCancelFrameStart = false;
        _checkedAttackCancelFrameEnd = false;
        _checkedAllCancelFrameStart = false;
        _checkedAllCancelFrameEnd = false;
        _lastFrame = false;
    }

    public void checkFrames()
    {
        if (_lastFrame)
        {
            _lastFrame = false;
            _frameCheckHandler.onLastFrameEnd();
        }

        // ditch inactive parser
        if (!_frameParser.isActive()) { return; }

        // check active frame range
        if (!_checkedActiveFrameStart && _frameParser.isOnOrPastFrame(activeFrameStart))
        {
            _frameCheckHandler.onActiveFrameStart();
            _checkedActiveFrameStart = true;
        }
        else if (!_checkedActiveFrameEnd && _frameParser.isOnOrPastFrame(activeFrameEnd))
        {
            _frameCheckHandler.onActiveFrameEnd();
            _checkedActiveFrameEnd = true;
        }

        // check attack-cancel frame range
        if (!_checkedAttackCancelFrameStart && _frameParser.isOnOrPastFrame(attackCancelFrameStart))
        {
            _frameCheckHandler.onAttackCancelFrameStart();
            _checkedAttackCancelFrameStart = true;
        }
        else if (!_checkedAttackCancelFrameEnd && _frameParser.isOnOrPastFrame(attackCancelFrameEnd))
        {
            _frameCheckHandler.onAttackCancelFrameEnd();
            _checkedAttackCancelFrameEnd = true;
        }

        // check all-cancel frame range
        if (!_checkedAllCancelFrameStart && _frameParser.isOnOrPastFrame(allCancelFrameStart))
        {
            _frameCheckHandler.onAllCancelFrameStart();
            _checkedAllCancelFrameStart = true;
        }
        else if (!_checkedAllCancelFrameEnd && _frameParser.isOnOrPastFrame(allCancelFrameEnd))
        {
            _frameCheckHandler.onAllCancelFrameEnd();
            _checkedAllCancelFrameEnd = true;
        }

        // check last frame
        if (!_lastFrame && _frameParser.isOnLastFrame())
        {
            _frameCheckHandler.onLastFrameStart();
            _lastFrame = true;
        }
    }
}