using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Logic.Managers;
using UnityEditor;
using UnityEngine;
using UnityEngine.U2D;

namespace Editor
{
    public class GameGraphicsBuilderEditorWindow : EditorWindow
    {
        private List<string> _chosenAssetPaths = new List<string>();
        private string _bundleFolderName = "UiArtBundle";
        private Sprite _backGroundSprite;
        private Sprite _XMarkSprite;
        private Sprite _0MarkSprite;

        [MenuItem ("Window/Game Graphics Builder Window")]
        public static void  ShowWindow () {
            GetWindow(typeof(GameGraphicsBuilderEditorWindow));
        }
    
        private void OnGUI () {
            GUILayout.Label ("Asset bundle name", EditorStyles.boldLabel);
            _bundleFolderName = EditorGUILayout.TextField ("Name", _bundleFolderName);
            
            AddImageAssetLoader("Background: ", ref _backGroundSprite);
            AddImageAssetLoader("X Sprite: ", ref _XMarkSprite);
            AddImageAssetLoader("0 Sprite: ", ref _0MarkSprite);
            
            if (GUILayout.Button("Build Bundles"))
            {
                BuildButtonAction();
            }
        }


        private void AddImageAssetLoader(string label, ref Sprite sprite)
        {
            sprite = (Sprite) EditorGUILayout.ObjectField(
                new GUIContent(label), 
                sprite,
                typeof(Sprite), 
                false);
            var path = AssetDatabase.GetAssetPath(sprite);
            if (sprite != null)
            {
                AddAssetPath(path, sprite.name);
            }
        }


        private void AddAssetPath(string path, string spriteName)
        {
            if (!_chosenAssetPaths.Contains(path) && !path.Equals(""))
            {
                if (_chosenAssetPaths.Exists(x => x.Contains(spriteName)))
                {
                    var indexToRemove = _chosenAssetPaths.FindIndex(x => x.Contains(spriteName));
                    _chosenAssetPaths.RemoveAt(indexToRemove);
                }
                _chosenAssetPaths.Add(path);
            }
        }


        private async void BuildButtonAction()
        {
            var path = $"Assets/BundledAssets/{_bundleFolderName}";
            if (!AssetDatabase.IsValidFolder(path))
            {
                AssetDatabase.CreateFolder("Assets/BundledAssets", _bundleFolderName);
            }
            else
            {
                var info = new DirectoryInfo(path);
                var fileInfo = info.GetFiles();
                if (fileInfo.Length > 0)
                {
                    Debug.LogError("Please choose a different Asset Bundle name since the current name already exists");
                    return;
                }
            }
            
            MoveAssets();
            await Task.Delay(1000);
            BuildAllAssetBundles();
            
            Close();
        }


        private void MoveAssets()
        {
            foreach (var path in _chosenAssetPaths)
            {
                // Sprite sprite = (Sprite) AssetDatabase.LoadAssetAtPath(path, typeof(Sprite));
                var newPath = $"Assets/BundledAssets/{_bundleFolderName}/{FullSpriteName(path)}";
                var moveResult = AssetDatabase.ValidateMoveAsset(path, newPath);
                if (moveResult == "")
                {
                    AssetDatabase.MoveAsset(path, newPath);
                }
                AssetImporter.GetAtPath(newPath).SetAssetBundleNameAndVariant(_bundleFolderName, "");
                PlayerPrefs.SetString(AssetBundleManager.AssetBundleNameSaveKey, _bundleFolderName);
            }
        }


        private string FullSpriteName(string filePath)
        {
            string substring = filePath.Split('/').Last();
            return substring;
        }
        
        
        private void BuildAllAssetBundles()
        {
            string assetBundleDirectory = $"Assets/StreamingAssets";
            if (!Directory.Exists(Application.streamingAssetsPath))
            {
                Directory.CreateDirectory(assetBundleDirectory);
            } 
            BuildPipeline.BuildAssetBundles(
                assetBundleDirectory, 
                BuildAssetBundleOptions.None, 
                EditorUserBuildSettings.activeBuildTarget);
        }
    }
}
