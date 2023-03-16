using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Generic class to create singleton of any ScriptableObject type
/// </summary>
/// <typeparam name="T"></typeparam>
public class ScriptableSingleton<T> : ScriptableObject where T : ScriptableObject
{
    private static T _instance = null;

    public static T Instance
    {

        get
        {
            if (_instance == null)
            {
                T[] results = Resources.FindObjectsOfTypeAll<T>();

                if (results.Length == 0)
                {
                    Debug.LogError("Singleton Scriptable Object: results length is 0 of " + typeof(T).ToString());
                    return null;
                }
                if (results.Length > 1)
                {
                    Debug.LogError("Singleton Scriptable Object: results is greater than 1 of " + typeof(T).ToString());
                    return null;
                }

                _instance = results[0];
                _instance.hideFlags = HideFlags.DontUnloadUnusedAsset;

            }
            return _instance;
        }


    }

}