using Extensions;
using Logic;
using Logic.Managers;
using UI.GameScene.Windows.GameFinishedWindow;
using UI.GameScene.Windows.SettingsWindow;
using UnityEngine;
using UnityEngine.UI;

namespace UI.GameScene
{
    public class GameSceneCanvasController : MonoBehaviour
    {
        [SerializeField] private Image background;
        [SerializeField] private SettingsWindow settingsWindow;
        [SerializeField] private GameFinishedWindow gameFinishedWindow;
        [SerializeField] private GameView.GameView gameView;


        private void Awake()
        {
            gameView.Init();
            background.sprite = AssetBundleManager.Instance.RetrieveAssetFromBundle<Sprite>("Background");
            
            GameManager.Instance.VictoryCallBack += (cells) =>
            {
                gameFinishedWindow.OpenWindow();
                gameFinishedWindow.Init(
                    false, 
                    GameManager.Instance.GameTurn.NextEnumElement(1).ToString());
            };

            GameManager.Instance.DrawCallBack += () =>
            {
                gameFinishedWindow.OpenWindow();
                gameFinishedWindow.Init(true, "");
            };

            SettingsButton();
        }
        

        public void SettingsButton()
        {
            settingsWindow.OpenWindow((winName) =>
            {
                gameView.Init();
            });
        }


        public void HintButton()
        {
            GameManager.Instance.HintRequested();
        }
        
    }
}
