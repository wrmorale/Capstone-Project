using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IFrameCheckHandler 
{
    void onActiveFrameStart();
    void onActiveFrameEnd();
    void onLastFrameStart();
    void onLastFrameEnd();
}

[System.Serializable]
public class FrameChecker 
{
    public int activeFrameStart;
    public int activeFrameEnd;
    public int totalFrames;

    private IFrameCheckHandler _frameCheckHandler;
    private FrameParser _frameParser;
    private bool _checkedActiveFrameStart;
    private bool _checkedActiveFrameEnd;
    private bool _lastFrame;

    public void initialize(IFrameCheckHandler frameCheckHandler, FrameParser frameParser)
    {
        _frameCheckHandler = frameCheckHandler;
        _frameParser = frameParser;
        totalFrames = frameParser.getTotalFrames();
        initCheck();
    }

    public void initCheck() {
        _checkedActiveFrameStart = false;
        _checkedActiveFrameEnd = false;
        _lastFrame = false;
    }

    public void checkFrames() 
    {
        if (_lastFrame) 
        {
            _lastFrame = false;
            _frameCheckHandler.onLastFrameEnd();
        }
        if (!_frameParser.isActive()) { return; }
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

        if (!_lastFrame && _frameParser.isOnLastFrame()) 
        {
            _frameCheckHandler.onLastFrameStart();
            _lastFrame = true;
        }
    }
}