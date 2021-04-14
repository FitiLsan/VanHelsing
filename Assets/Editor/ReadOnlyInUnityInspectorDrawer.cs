using UnityEditor;
using UnityEngine;


namespace BeastHunter
{
    [CustomPropertyDrawer(typeof(ReadOnlyInUnityInspectorAttribute))]
    public class ReadOnlyInUnityInspectorDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            GUI.enabled = false;
            EditorGUI.PropertyField(position, property, label, true);
            GUI.enabled = true;
        }
    }
}
