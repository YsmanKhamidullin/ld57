#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

    [CustomPropertyDrawer(typeof(TexturePropertyAttribute))]
    public class TextureDrawer : PropertyDrawer
    {
        Texture2D texture;
        TexturePropertyAttribute propertyAttribute;
        float width = 0f;
        float height = 0f;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            var indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;
            propertyAttribute = attribute as TexturePropertyAttribute;
            texture = (Texture2D)property.objectReferenceValue;
            if (texture)
            {
                var textureRect = CalculateTextureRect(position);
                EditorGUI.DrawPreviewTexture(textureRect, texture);
            }
            else
            {
                var textureRect = new Rect(position.x, position.y, EditorGUIUtility.singleLineHeight,
                    EditorGUIUtility.singleLineHeight);
                texture = (Texture2D)EditorGUI.ObjectField(textureRect, "Attach Texture", texture, typeof(Texture2D),
                    false);
            }

            EditorGUI.indentLevel = indent;
            EditorGUI.EndProperty();
        }

        private Rect CalculateTextureRect(Rect position)
        {
            var maxWidth = 600;
            var maxHeight = 600;

            var aspectRatio = (float)texture.width / texture.height;

            width = 0f;
            height = 0f;
            if (texture.width > maxWidth || texture.height > maxHeight)
            {
                if (aspectRatio > 1)
                {
                    width = maxWidth;
                    height = (float)(maxWidth / aspectRatio);
                }
                else
                {
                    height = maxHeight;
                    width = (float)(maxHeight * aspectRatio);
                }
            }

            var textureRect = new Rect(position.x, position.y, width, height);
            return textureRect;
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return texture ? height : EditorGUIUtility.singleLineHeight * 6;
        }
    }
#endif