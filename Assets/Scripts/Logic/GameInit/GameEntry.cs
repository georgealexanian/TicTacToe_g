using Logic.Managers;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameInit
{
    public class GameEntry : MonoBehaviour
    {
        private void Awake()
        {
            GameManager.Instance.Initialize();
            AudioManager.Instance.Initialize();
            WindowsManager.Instance.Initialize();
            AssetBundleManager.Instance.Initialize();
            
            SceneManager.LoadScene(Scenes.GameScene.ToString());
        }
    }
}

public enum Scenes
{
    Unknown = 0,
    LoadingScene = 1,
    GameScene = 2
}
