using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
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
            AddAssetPath(AssetDatabase.GetAssetPath(sprite));
        }


        private void AddAssetPath(string path)
        {
            if (!_chosenAssetPaths.Contains(path) && !path.Equals(""))
            {
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
            
            MoveAssets();
            await Task.Delay(1000);
            BuildAllAssetBundles();
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
