#if UNITY_EDITOR
using Game.Core.VisualNovel;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(IntSliderAttribute))]
public class IntSliderPropertyDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        IntSliderAttribute sliderAttribute = (IntSliderAttribute)attribute;

        if (property.propertyType == SerializedPropertyType.Integer)
        {
            // Create a rect for the slider
            Rect sliderRect = new Rect(position.x, position.y, position.width - 60, position.height);
            // Create a rect for the + button
            Rect plusButtonRect = new Rect(position.x + position.width - 50, position.y, 25, position.height);
            // Create a rect for the - button
            Rect minusButtonRect = new Rect(position.x + position.width - 25, position.y, 25, position.height);

            // Draw the slider
            property.intValue = EditorGUI.IntSlider(sliderRect, label, property.intValue, sliderAttribute.min, sliderAttribute.max);

            // Draw the + button
            if (GUI.Button(plusButtonRect, "+"))
            {
                property.intValue = Mathf.Min(property.intValue + 1, sliderAttribute.max);
            }

            // Draw the - button
            if (GUI.Button(minusButtonRect, "-"))
            {
                property.intValue = Mathf.Max(property.intValue - 1, sliderAttribute.min);
            }
        }
        else
        {
            EditorGUI.LabelField(position, label.text, "Use IntSlider with int.");
        }
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUIUtility.singleLineHeight;
    }
}
#endif
