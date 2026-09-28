using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(RequiredAttribute))]
public class RequiredDrawer : PropertyDrawer
{
    private const float Spacing = 2f;
    private const float HelpBoxHeight = 38f;

    public override void OnGUI(
        Rect position,
        SerializedProperty property,
        GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        Rect fieldRect = new Rect(
            position.x,
            position.y,
            position.width,
            EditorGUIUtility.singleLineHeight
        );

        EditorGUI.PropertyField(
            fieldRect,
            property,
            label,
            true
        );

        if (IsMissingReference(property))
        {
            Rect warningRect = new Rect(
                position.x,
                fieldRect.yMax + Spacing,
                position.width,
                HelpBoxHeight
            );

            EditorGUI.HelpBox(
                warningRect,
                "Required: esta referencia es obligatoria.",
                MessageType.Error
            );
        }

        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(
        SerializedProperty property,
        GUIContent label)
    {
        float height = EditorGUI.GetPropertyHeight(
            property,
            label,
            true
        );

        if (IsMissingReference(property))
        {
            height += Spacing + HelpBoxHeight;
        }

        return height;
    }

    private bool IsMissingReference(SerializedProperty property)
    {
        if (property.propertyType != SerializedPropertyType.ObjectReference)
            return false;

        return property.objectReferenceValue == null;
    }
}