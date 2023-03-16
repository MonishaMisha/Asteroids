using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
/// <summary>
/// Custom property drawer to choose single layer in editor
/// </summary>
[CustomPropertyDrawer(typeof(LayerAttribute))]
public class LayerPropertyDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
     {
          property.intValue = EditorGUI.LayerField(position, label,  property.intValue);
     }
}
