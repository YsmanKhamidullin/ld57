#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.IO;
using Game.Core.VisualNovel;

public class CharacterSelectorWindow : EditorWindow
{
    private List<Character> characters;
    private Vector2 scrollPosition;
    private SerializedProperty property;

    public static void ShowWindow(SerializedProperty property)
    {
        CharacterSelectorWindow window = GetWindow<CharacterSelectorWindow>("Select Character");
        window.LoadCharacters("Assets/VisualNovel/Resources/00_Characters/");
        window.property = property;
    }

    private void LoadCharacters(string folderPath)
    {
        characters = new List<Character>();
        string[] characterPaths = Directory.GetFiles(folderPath, "*.prefab", SearchOption.AllDirectories);
        foreach (string characterPath in characterPaths)
        {
            Character character = AssetDatabase.LoadAssetAtPath<Character>(characterPath);
            if (character.Id == "")
            {
                continue;
            }

            if (character != null)
            {
                characters.Add(character);
            }
        }
    }

    private void OnGUI()
    {
        GUILayout.Label("Select a Character", EditorStyles.boldLabel);
        scrollPosition = GUILayout.BeginScrollView(scrollPosition);

        foreach (Character character in characters)
        {
            Texture texture = character.FullSprite == null
                ? new Texture2D(100, 100)
                : character.FullSprite.texture;
            var aspect = texture.width / (float)texture.height;
            GUIContent preview = new GUIContent(texture, character.Name);
            if (GUILayout.Button(preview, GUILayout.Width(300), GUILayout.Height(400 / aspect)))
            {
                property.objectReferenceValue = character; // Set the selected character
                property.serializedObject.ApplyModifiedProperties(); // Apply changes
                Close(); // Close the window
            }
        }

        GUILayout.EndScrollView();
    }
}
#endif