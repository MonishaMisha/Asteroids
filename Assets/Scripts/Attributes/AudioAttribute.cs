using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AudioType
{
    BGM,
    SFX
}

public class AudioAttribute : PropertyAttribute
{
    public bool IsCustomAudio;
    public AudioType audioType;
    public string target;
    public AudioAttribute(string targetName, AudioType audioType)
    {
        this.target = targetName;
        this.audioType = audioType;
    }
    public AudioAttribute( AudioType audioType)
    {
        IsCustomAudio = true;
        this.audioType = audioType;
    }
}
