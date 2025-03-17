using System.IO;
using UnityEditor;
using UnityEngine;

namespace Dffrnt.CoreValues
{
    public static class ScriptableObjectUtility
    {
        public static U CreateInstance<U>(string defaultName, string key = null, string extension = "asset") where U : ScriptableObject
        {
            var assetType = typeof(U).Name;
            if (key == null)
                key = $"_CreateAssetPath_{assetType}";
            else
                key = $"_CreateAssetPath_{key}";
            var path = EditorPrefs.GetString(key, "Assets");
            path = EditorUtility.SaveFilePanel($"Save {assetType} Asset", path, defaultName, extension);
            if (path != string.Empty)
            {
                var relativePath = $"Assets/{Path.GetRelativePath("Assets", path)}";
                var savedPath = Path.GetDirectoryName(relativePath);
                EditorPrefs.SetString(key, savedPath);
                var newAsset = ScriptableObject.CreateInstance<U>();
                AssetDatabase.CreateAsset(newAsset, relativePath);
                return newAsset;
            }

            return null;
        }
    }
}