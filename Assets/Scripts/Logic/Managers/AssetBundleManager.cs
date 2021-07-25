using System.IO;
using UnityEngine;

namespace Logic.Managers
{
    public class AssetBundleManager : BaseManager<AssetBundleManager>
    {
        public const string AssetBundleNameSaveKey = "AssetBundleName";
        private AssetBundle _assetBundle;
        
        
        public override void Initialize()
        {
            LoadAssetBundle();
        }


        private void LoadAssetBundle()
        {
            var bundleName = PlayerPrefs.GetString(AssetBundleNameSaveKey); 
            _assetBundle = AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath, bundleName));
            if (_assetBundle == null)
            {
                Debug.LogError("Failed to load AssetBundle!");
            }
        }


        public T RetrieveAssetFromBundle<T>(string assetName) where T : Object
        {
            if (_assetBundle != null)
            {
                T asset = _assetBundle.LoadAsset<T>(assetName);
                return asset;
            }
            return null;
        }
    }
}
