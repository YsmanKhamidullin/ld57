#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(CharacterSelectorAttribute))]
public class CharacterSelectorDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        CharacterSelectorAttribute characterSelector = (CharacterSelectorAttribute)attribute;

        // Create a new button to open the character selection window
        if (GUI.Button(position, label.text))
        {
            // Open the character selection window
            CharacterSelectorWindow.ShowWindow(property);
        }
    }
}
#endif