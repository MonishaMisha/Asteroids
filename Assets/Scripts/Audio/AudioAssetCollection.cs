using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

using System.Linq;

/// <summary>
/// Collection of audioclips and access options
/// </summary>
public class AudioAssetCollection : ScriptableSingleton<AudioAssetCollection>
{

    public AudioClip[] sfxS;
    public AudioClip[] bgmS;

    //Default parameters for easy access

    [Audio(AudioType.BGM)]
    public int BGMbeat1;
    [Audio(AudioType.BGM)]
    public int BGMbeat2;
    [Audio(AudioType.SFX)]
    public int EngineSound;


    /// <summary>
    /// To get SFX clip from asset collection 
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public AudioClip GetSFX(int index)
    {

        try
        {
            return sfxS[index];

        }
        catch
        {          
            Debug.LogError("Could not find the SFX audio clip, Please make sure the name is correct or the clip is referenced to AudioAssetCollection");
            return null;
        }
    }

    /// <summary>
    /// To get BGM clip from asset collection 
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public AudioClip GetBGM(int index)
    {

        try
        {
            return bgmS[index];

        }
        catch
        {
            Debug.LogError("Could not find the BGM audio clip, Please make sure the name is correct or the clip is referenced to AudioAssetCollection");
            return null;
        }
    }

    /// <summary>
    /// Get all audioclip names in the SFX collection
    /// </summary>
    /// <returns></returns>
    public string[] GetSFXNames()
    {if (sfxS.Length > 0)
        {
            return sfxS.Select(x => x.name).ToArray();
        }
        else
        {
            return new string[] { };
        }
    }
    /// <summary>
    /// Get all audioclip names in the BGM collection
    /// </summary>
    /// <returns></returns>
    public string[] GetBGMNames()
    {
        if (bgmS.Length > 0)
        {
            return bgmS.Select(x => x.name).ToArray();
        }
        else
        {
            return new string[] { };
        }
    }
  


#if UNITY_EDITOR
        [MenuItem("Audio/Create Audio Asset Collection")]
    public static void CreateAsset()
    {
        //creates a new singleton audio collection asset in the given path
        if (Instance == null)
        {
            if (AssetDatabase.IsValidFolder("Assets/ScriptableObjects/dasdasd"))
            {
                AssetDatabase.CreateAsset(new AudioAssetCollection(), "Assets/ScriptableObjects/Audio/AudioAssetCollection.asset");
                Debug.Log("Audio asset created at path : Assets/ScriptableObjects/Audio/AudioAssetCollection.asset");
            }
            else
            {
                Debug.LogError("<b>Audio asset creation failed</b> Directory is missing  : Assets/ScriptableObjects/Audio/");
            }
            
        }
        else
        {
            Debug.Log("AudioAssetCollection object already exists at path : Assets/ScriptableObjects/Audio/AudioAssetCollection.asset");
        }
    }
#endif

}
