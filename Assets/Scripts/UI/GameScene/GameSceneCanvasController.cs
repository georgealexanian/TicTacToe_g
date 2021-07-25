using Logic.Managers;
using UI.GameScene.Windows.SettingsWindow;
using UnityEngine;
using UnityEngine.UI;

namespace UI.GameScene
{
    public class GameSceneCanvasController : MonoBehaviour
    {
        [SerializeField] private Image background;
        [SerializeField] private SettingsWindow settingsWindow;
        [SerializeField] private GameView.GameView gameView;


        private void Awake()
        {
            gameView.Init();
            background.sprite = AssetBundleManager.Instance.RetrieveAssetFromBundle<Sprite>("Background");
        }
        

        public void SettingsButton()
        {
            settingsWindow.OpenWindow((winName) =>
            {
                gameView.Init();
            });
        }
    }
}
