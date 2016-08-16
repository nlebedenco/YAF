using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

using System.IO;
using System;

[CustomEditor(typeof(GameSystem), true)]
[CanEditMultipleObjects]
public class GameSystemEditor : Editor
{
    #region Static

    private static SceneAsset FindSceneInEditorBuildSettings(string name)
    {
        if (string.IsNullOrEmpty(name))
            return null;

        foreach (var editorScene in EditorBuildSettings.scenes)
        {
            if (Path.GetFileNameWithoutExtension(editorScene.path) == name)
                return AssetDatabase.LoadAssetAtPath(editorScene.path, typeof(SceneAsset)) as SceneAsset;
        }

        Debug.LogWarningFormat("Scene [{0}] cannot be used. You must first add this scene to the 'Scenes in the Build' in the project build settings.", name);
        return null;
    }

    #endregion

    #region Properties

    private SerializedProperty hudOverlayProperty;
    private SerializedProperty menuOverlayProperty;
    private SerializedProperty optionsOverlayProperty;
    private SerializedProperty shopSceneProperty;
    private SerializedProperty creditsSceneProperty;

    private SerializedProperty levelSceneProperty;
    private ReorderableList levelList;

    #endregion

    #region Labels

    private GUIContent hudOverlayLabel;
    private GUIContent menuOverlayLabel;
    private GUIContent optionsOverlayLabel;
    private GUIContent shopSceneLabel;
    private GUIContent creditsSceneLabel;
    private GUIContent levelSceneLabel;

    #endregion

    #region Editor

    void OnEnable()
    {
        hudOverlayLabel = new GUIContent("HUD Overlay");
        menuOverlayLabel = new GUIContent("Menu Overlay");
        optionsOverlayLabel = new GUIContent("Options Overlay");
        shopSceneLabel = new GUIContent("Shop Scene");
        creditsSceneLabel = new GUIContent("Credits Scene");
        levelSceneLabel = new GUIContent();

        hudOverlayProperty = serializedObject.FindProperty("hudOverlay");
        menuOverlayProperty = serializedObject.FindProperty("menuOverlay");
        optionsOverlayProperty = serializedObject.FindProperty("optionsOverlay");

        shopSceneProperty = serializedObject.FindProperty("shopScene");
        creditsSceneProperty = serializedObject.FindProperty("creditsScene");

        levelSceneProperty = serializedObject.FindProperty("levelScene");

        levelList = new ReorderableList(serializedObject, levelSceneProperty);
        levelList.elementHeight = 16; // The list uses a default 16x16 icon. Other sizes make it stretch.

        levelList.drawHeaderCallback = LevelListDrawHeader;
        levelList.drawElementCallback = LevelListDrawChild;
    }

    void OnDisable()
    {

    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        ShowScenes();

        serializedObject.ApplyModifiedProperties();
    }

    #endregion

    private void ShowScene(SerializedProperty property, GUIContent label, Rect? rect = null)
    {
        var obj = FindSceneInEditorBuildSettings(property.stringValue);
        EditorGUI.BeginChangeCheck();
        var scene = (rect == null) ? EditorGUILayout.ObjectField(label, obj, typeof(SceneAsset), false)
                                   : EditorGUI.ObjectField(rect.Value, label, obj, typeof(SceneAsset), false);
        if (EditorGUI.EndChangeCheck())
        {
            if (scene == null)
            {
                property.stringValue = string.Empty;
            }
            else
            {
                string value = scene.name;
                if (value != property.stringValue)
                {
                    if (FindSceneInEditorBuildSettings(value) != null)
                        property.stringValue = value;
                }
            }
        }
    }

    private void ShowScenes()
    {
        ShowScene(hudOverlayProperty, hudOverlayLabel);
        ShowScene(menuOverlayProperty, menuOverlayLabel);
        ShowScene(optionsOverlayProperty, optionsOverlayLabel);
        ShowScene(shopSceneProperty, shopSceneLabel);
        ShowScene(creditsSceneProperty, creditsSceneLabel);

        {
            float labelWidth = EditorGUIUtility.labelWidth;
            try
            {
                EditorGUIUtility.labelWidth = 30;
                levelList.DoLayoutList();
            }
            finally
            {
                EditorGUIUtility.labelWidth = labelWidth;
            }
        }       
    }

    #region Level List Callbacks

    private void LevelListDrawHeader(Rect headerRect)
    {
        GUI.Label(headerRect, "Levels");
    }

    private void LevelListDrawChild(Rect r, int index, bool isActive, bool isFocused)
    {
        SerializedProperty element = levelSceneProperty.GetArrayElementAtIndex(index);
        levelSceneLabel.text = string.Format("{0}:", index);
        ShowScene(element, levelSceneLabel, r);
    }

    #endregion
}
