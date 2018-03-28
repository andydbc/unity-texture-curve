using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TextureCurve))]
public class TextureCurveEditor : Editor
{
    SerializedProperty _redCurve;
    SerializedProperty _greenCurve;
    SerializedProperty _blueCurve;
    SerializedProperty _alphaCurve;

    SerializedProperty _resolution;
    SerializedProperty _wrapMode;
    SerializedProperty _filterMode;

    int _texureMinRes = 64;
    int _texureMaxRes = 1024;
    
    List<int> _textureResolutions = new List<int>();
    List<GUIContent> _textureResolutionsNames = new List<GUIContent>();

    void OnEnable()
    {
        _redCurve = serializedObject.FindProperty("_red");
        _greenCurve = serializedObject.FindProperty("_green");
        _blueCurve = serializedObject.FindProperty("_blue");
        _alphaCurve = serializedObject.FindProperty("_alpha");

        _wrapMode = serializedObject.FindProperty("_wrapMode");
        _filterMode = serializedObject.FindProperty("_filterMode");

        _resolution = serializedObject.FindProperty("_resolution");

        _textureResolutions.Clear();
        _textureResolutionsNames.Clear();

        for (int i = _texureMinRes; i <= _texureMaxRes; i *= 2)
        {
            _textureResolutions.Add(i);
            _textureResolutionsNames.Add(new GUIContent(i.ToString()));
        }
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUI.BeginChangeCheck();

        EditorGUILayout.PropertyField(_redCurve);
        EditorGUILayout.PropertyField(_greenCurve);
        EditorGUILayout.PropertyField(_blueCurve);
        EditorGUILayout.PropertyField(_alphaCurve);

        EditorGUILayout.IntPopup(_resolution, _textureResolutionsNames.ToArray(), _textureResolutions.ToArray());

        EditorGUILayout.PropertyField(_wrapMode);
        EditorGUILayout.PropertyField(_filterMode);

        var rebuild = EditorGUI.EndChangeCheck();

        serializedObject.ApplyModifiedProperties();

        if (rebuild)
            ((TextureCurve)target).Bake();
    }

    [MenuItem("Assets/Create/Texture Curve")]
    static void CreateTextureCurveAsset()
    {
        var path = AssetDatabase.GetAssetPath(Selection.activeObject);
        if (string.IsNullOrEmpty(path))
            path = "Assets";
        else if (Path.GetExtension(path) != "")
            path = path.Replace(Path.GetFileName(AssetDatabase.GetAssetPath(Selection.activeObject)), "");

        var assetPathName = AssetDatabase.GenerateUniqueAssetPath(path + "/TextureCurve.asset");
        var asset = ScriptableObject.CreateInstance<TextureCurve>();

        AssetDatabase.CreateAsset(asset, assetPathName);
        AssetDatabase.AddObjectToAsset(asset.Texture, asset);

        AssetDatabase.SaveAssets();

        EditorUtility.FocusProjectWindow();
        Selection.activeObject = asset;
    }
}
