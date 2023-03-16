using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Event handler to handle audio events
/// </summary>
public class AudioEventHandler : Singleton<AudioEventHandler>
{
    public delegate void onPlaySFX(int index);
    public static event onPlaySFX OnPlaySFX;

    public delegate void onPlayBGM(int index, bool isLoop = true);
    public static event onPlayBGM OnPlayBGM;

    public delegate void onPlayEngineSound(bool play, bool isLoop = true);
    public static event onPlayEngineSound OnPlayEngineSound;


    /// <summary>
    /// Used for playing a bgm sound
    /// </summary>
    /// <param name="inx"></param>
    /// <param name="isLoop"></param>
    public void PlayBGM(int inx, bool isLoop = true)
    {
        OnPlayBGM?.Invoke(inx, isLoop);
    }

    /// <summary>
    ///Used to play SFX sounds
    /// </summary>
    /// <param name="inx"></param>
    public void PlaySFX(int inx)
    {
        OnPlaySFX?.Invoke(inx);
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="state"></param>
    /// <param name="looping"></param>
    public void PlayEngineSound(bool state, bool looping = true)
    {
        OnPlayEngineSound?.Invoke(state, looping);
    }
}