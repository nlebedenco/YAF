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

    private SerializedProperty splashGraphicProperty;
    private SerializedProperty fadeGraphicProperty;
    private SerializedProperty fadeDurationProperty;

    private SerializedProperty hudSceneNameProperty;
    private SerializedProperty menuSceneNameProperty;
    private SerializedProperty shopSceneNameProperty;
    private SerializedProperty creditsSceneNameProperty;

    private SerializedProperty levelSceneNameProperty;
    private ReorderableList levelList;

    #endregion

    #region Labels

    private GUIContent splashGraphicLabel;
    private GUIContent fadeGraphicLabel;
    private GUIContent fadeDurationLabel;

    private GUIContent hudSceneNameLabel;
    private GUIContent menuSceneNameLabel;
    private GUIContent shopSceneNameLabel;
    private GUIContent creditsSceneNameLabel;
    private GUIContent levelSceneLabel;

    #endregion

    #region Editor

    void OnEnable()
    {
        // Properties
        splashGraphicProperty = serializedObject.FindProperty("splashGraphic");
        fadeGraphicProperty = serializedObject.FindProperty("fadeGraphic");
        fadeDurationProperty = serializedObject.FindProperty("fadeDuration");

        hudSceneNameProperty = serializedObject.FindProperty("hudSceneName");
        menuSceneNameProperty = serializedObject.FindProperty("menuSceneName");
        shopSceneNameProperty = serializedObject.FindProperty("shopSceneName");
        creditsSceneNameProperty = serializedObject.FindProperty("creditsSceneName");
        levelSceneNameProperty = serializedObject.FindProperty("levelSceneName");

        levelList = new ReorderableList(serializedObject, levelSceneNameProperty);
        levelList.elementHeight = 16; // The list uses a default 16x16 icon. Other sizes make it stretch.

        levelList.drawHeaderCallback = LevelListDrawHeader;
        levelList.drawElementCallback = LevelListDrawChild;

        // Labels 

        splashGraphicLabel = new GUIContent("Splash Graphic");
        fadeGraphicLabel = new GUIContent("Fade Graphic");
        fadeDurationLabel = new GUIContent("Fade Duration");
        hudSceneNameLabel = new GUIContent("HUD");
        menuSceneNameLabel = new GUIContent("Menu");
        shopSceneNameLabel = new GUIContent("Shop");
        creditsSceneNameLabel = new GUIContent("Credits");
        levelSceneLabel = new GUIContent();

    }

    void OnDisable()
    {

    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUI.BeginDisabledGroup(Application.isPlaying);
        ShowSplash();
        ShowFade();
        ShowScenes();
        EditorGUI.EndDisabledGroup();

        serializedObject.ApplyModifiedProperties();
    }

    #endregion

    private void ShowSplash()
    {
        EditorGUILayout.PropertyField(splashGraphicProperty, splashGraphicLabel);
    }

    private void ShowFade()
    {
        EditorGUILayout.PropertyField(fadeGraphicProperty, fadeGraphicLabel);
        EditorGUILayout.PropertyField(fadeDurationProperty, fadeDurationLabel);
    }

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
        ShowScene(hudSceneNameProperty, hudSceneNameLabel);
        ShowScene(menuSceneNameProperty, menuSceneNameLabel);
        ShowScene(shopSceneNameProperty, shopSceneNameLabel);
        ShowScene(creditsSceneNameProperty, creditsSceneNameLabel);

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
        SerializedProperty element = levelSceneNameProperty.GetArrayElementAtIndex(index);
        levelSceneLabel.text = string.Format("{0}:", index);
        ShowScene(element, levelSceneLabel, r);
    }

    #endregion
}
