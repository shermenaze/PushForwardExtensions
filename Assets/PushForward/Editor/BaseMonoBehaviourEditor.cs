
using UnityEditor;
using UnityEngine;

[CanEditMultipleObjects, CustomEditor(typeof(BaseMonoBehaviour), true)]
public class BaseMonoBehaviourEditor : Editor
{
    public override void OnInspectorGUI()
    {
        this.serializedObject.Update();

        SerializedProperty propertyIterator = this.serializedObject.GetIterator();
        bool enterChildren = true;
        while (propertyIterator.NextVisible(enterChildren))
        {
            enterChildren = false;

            this.OnPropertyGUI(propertyIterator);
        }

        this.serializedObject.ApplyModifiedProperties();
    }

    public void OnPropertyGUI(SerializedProperty property)
    {
        this.OnPropertyGUI(property, new GUIContent(property.displayName, property.tooltip));
    }

    public void OnPropertyGUI(SerializedProperty property, GUIContent label)
    {
        BaseMonoBehaviour targetMonoBehaviour = this.target as BaseMonoBehaviour;

        EditorGUI.BeginChangeCheck();

        this.DrawProperty(property, label);

        if (EditorGUI.EndChangeCheck())
        {
            property.serializedObject.ApplyModifiedProperties();
            property.serializedObject.Update();
            targetMonoBehaviour.OnValidateProperty(property.name);
        }
    }

    public void OnPropertyGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        BaseMonoBehaviour targetMonoBehaviour = this.target as BaseMonoBehaviour;

        EditorGUI.BeginChangeCheck();

        this.DrawProperty(position, property, label);

        if (EditorGUI.EndChangeCheck())
        {
            property.serializedObject.ApplyModifiedProperties();
            property.serializedObject.Update();
            targetMonoBehaviour.OnValidateProperty(property.name);
        }
    }

    public virtual Rect GetPropertyControlRect(SerializedProperty property, GUIContent label, params GUILayoutOption[] options)
    {
        return EditorGUILayout.GetControlRect(!string.IsNullOrEmpty(label.text), EditorGUI.GetPropertyHeight(property, label, true), options);
    }

    protected virtual void DrawProperty(SerializedProperty property, GUIContent label, params GUILayoutOption[] options)
    {
        Rect position = this.GetPropertyControlRect(property, label, options);
        this.DrawProperty(position, property, label);
    }
 
    protected virtual void DrawProperty(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.PropertyField(position, property, label);
    }
}
