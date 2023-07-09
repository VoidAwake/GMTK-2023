using UnityEditor;
using UnityEngine;

namespace Hawaiian.Utilities.Editor
{
    // From https://www.patrykgalach.com/2020/01/20/readonly-attribute-in-unity-editor/
    [CustomPropertyDrawer(typeof(ReadOnlyAttribute))]
    public class ReadOnlyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var previousGUIState = GUI.enabled;

            GUI.enabled = false;

            EditorGUI.PropertyField(position, property, label);

            GUI.enabled = previousGUIState;
        }
    }
}