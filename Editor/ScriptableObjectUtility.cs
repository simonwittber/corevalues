using UnityEditor;
using UnityEngine;

namespace Dffrnt.CoreValues
{
    public static class ScriptableObjectUtility
    {
        public static U CreateInstance<U>(string defaultName, string key=null, string extension="asset") where U : ScriptableObject
        {
            var assetType = typeof(U).Name;
            if (key == null)
                key = $"_CreateAssetPath_{assetType}";
            else
                key = $"_CreateAssetPath_{key}";
            Debug.Log(key);
            var path = EditorPrefs.GetString(key, "Assets");
            Debug.Log(path);
            path = EditorUtility.SaveFilePanel($"Save {assetType} Asset", path, defaultName, extension);
            if (path != string.Empty)
            {
                var relativePath = $"Assets/{System.IO.Path.GetRelativePath("Assets", path)}";
                var savedPath = System.IO.Path.GetDirectoryName(relativePath);
                EditorPrefs.SetString(key, savedPath);
                var newAsset = ScriptableObject.CreateInstance<U>();
                AssetDatabase.CreateAsset(newAsset, relativePath);
                return newAsset;
            }
            return null;
        } 
    }
}