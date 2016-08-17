using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(MouseTrackerPanel), true)]
[CanEditMultipleObjects]
public class MouseTrackerPanelEditor : Editor
{
    #region Properties

    private SerializedProperty dragProperty;
    private SerializedProperty tapEnabledProperty;
    private SerializedProperty tapProperty;
    private SerializedProperty dragRangeProperty;

    #endregion

    #region Labels

    private GUIContent dragLabel;
    private GUIContent tapEnabledLabel;
    private GUIContent tapLabel;
    private GUIContent dragRangeLabel;

    #endregion

    #region Editor

    void OnEnable()
    {
        // Properties 
        dragProperty = serializedObject.FindProperty("drag");
        tapEnabledProperty = serializedObject.FindProperty("tapEnabled");
        tapProperty = serializedObject.FindProperty("tap");
        dragRangeProperty = serializedObject.FindProperty("dragRange");

        // Labels
        dragLabel = new GUIContent("Drag");
        tapEnabledLabel = new GUIContent("Tap Enabled");
        tapLabel = new GUIContent("Tap Settings");
        dragRangeLabel = new GUIContent("Drag Range");
    }

    void OnDisable()
    {

    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        serializedObject.Update();

        EditorGUI.BeginDisabledGroup(Application.isPlaying);

        EditorGUILayout.PropertyField(dragProperty, dragLabel, true);

        EditorGUILayout.PropertyField(tapEnabledProperty, tapEnabledLabel);
        if (tapEnabledProperty.boolValue)
        {
            EditorGUI.indentLevel += 1;
            EditorGUILayout.PropertyField(tapProperty, tapLabel, true);
            EditorGUI.indentLevel -= 1;
        }

        EditorGUI.EndDisabledGroup();

        EditorGUILayout.PropertyField(dragRangeProperty, dragRangeLabel);

        serializedObject.ApplyModifiedProperties();
    }

    #endregion

}
