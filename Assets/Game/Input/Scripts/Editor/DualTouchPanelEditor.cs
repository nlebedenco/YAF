using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(DualTouchPanel), true)]
[CanEditMultipleObjects]
public class DualTouchPanelEditor : Editor
{
    #region Properties

    private SerializedProperty dragLeftProperty;
    private SerializedProperty dragRightProperty;
    private SerializedProperty tapEnabledProperty;
    private SerializedProperty tapProperty;
    private SerializedProperty dragRangeProperty;

    #endregion

    #region Labels

    private GUIContent dragLeftLabel;
    private GUIContent dragRightLabel;
    private GUIContent tapEnabledLabel;
    private GUIContent tapLabel;
    private GUIContent dragRangeLabel;

    #endregion

    #region Editor

    void OnEnable()
    {
        // Properties 
        dragLeftProperty = serializedObject.FindProperty("dragLeft");
        dragRightProperty = serializedObject.FindProperty("dragRight");
        tapEnabledProperty = serializedObject.FindProperty("tapEnabled");
        tapProperty = serializedObject.FindProperty("tap");
        dragRangeProperty = serializedObject.FindProperty("dragRange");

        // Labels
        dragLeftLabel = new GUIContent("Left Settings");
        dragRightLabel = new GUIContent("Right Settings");
        tapEnabledLabel = new GUIContent("Tap Enabled");
        tapLabel = new GUIContent("Tap Settings");
        dragRangeLabel = new GUIContent("Drag Range");
    }

    void OnDisable()
    {

    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUI.BeginDisabledGroup(Application.isPlaying);

        EditorGUILayout.PropertyField(dragLeftProperty, dragLeftLabel, true);
        EditorGUILayout.PropertyField(dragRightProperty, dragRightLabel, true);

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
