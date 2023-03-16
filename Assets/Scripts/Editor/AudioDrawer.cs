using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


/// <summary>
/// Custom Propert Drawer to draw audioclip selection dropdown GUI in the editor
/// </summary>

[CustomPropertyDrawer(typeof(AudioAttribute))]
public class AudioDrawer : PropertyDrawer
{
    string[] _choices ;
    int _choiceIndex = 0;
    Rect toggle;
    bool isDrawable;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        GUI.color = Color.white;
        EditorGUI.BeginProperty(position, label, property);
        EditorGUI.BeginChangeCheck();
        AudioAttribute audio = attribute as AudioAttribute;
        bool newState = true;
        bool oldState = GUI.enabled;
        if (audio.IsCustomAudio)
        {
            isDrawable = audio.IsCustomAudio;
        }
        else
        {
            SerializedProperty s_Property = property.serializedObject.FindProperty(audio.target);

           

            if (s_Property == null) Debug.LogWarning("[AudioDrawer] Invalid Property Name for Attribute.", property.serializedObject.targetObject);
            else newState = s_Property.boolValue != audio.IsCustomAudio;
            GUI.enabled = newState;

            isDrawable = s_Property.boolValue;
        }

        if (property.propertyType == SerializedPropertyType.Integer)
        {
            
            switch (audio.audioType)
            {
            

                case AudioType.BGM:
                    if (isDrawable)
                    {
                        DisPlayBGMNames(position, property, label);
                    }
                    else
                    {
                        property.intValue = -1;
                    }
                    
                    break;

                case AudioType.SFX:
                   
                    if (isDrawable)
                    {
                        DisPlaySFXNames(position, property, label);
                    }
                    else
                    {
                        property.intValue = -1;
                    }
                    break;
            }
        }

        if (EditorGUI.EndChangeCheck())
        {
            property.serializedObject.ApplyModifiedProperties();
            EditorUtility.SetDirty(property.serializedObject.targetObject);
        }
        EditorGUI.EndProperty();

        GUI.enabled = oldState;
    }



    void DisPlaySFXNames(Rect position, SerializedProperty property, GUIContent label)
    {

        _choices = AudioAssetCollection.Instance.GetSFXNames();
        if (_choices.Length > 0)
        {

            property.intValue = EditorGUI.Popup(position,label.text, property.intValue, _choices);
            _choiceIndex = property.intValue;

        }
        else
        {
            GUI.color = Color.red;
            GUI.Label(position, "Cannot Find SFX Audios, Please check the AudioAssetCollection");
            property.intValue = -1;
        }
    }
    void DisPlayBGMNames(Rect position, SerializedProperty property, GUIContent label)
    {

        _choices = AudioAssetCollection.Instance.GetBGMNames();
        if (_choices.Length > 0)
        {

            property.intValue = EditorGUI.Popup(position, label.text,property.intValue, _choices);
            _choiceIndex = property.intValue;

        }
        else
        {
            GUI.color = Color.red;
            GUI.Label(position, "Cannot Find BGM Audios, Please check the AudioAssetCollection");
            property.intValue = -1;
        }
    }
  
}