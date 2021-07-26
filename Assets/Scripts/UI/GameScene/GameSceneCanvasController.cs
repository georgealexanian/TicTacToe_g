using Extensions;
using Logic;
using Logic.GameFlow;
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
        [SerializeField] private Button hintButton;
        [SerializeField] private Button undoButton;
        

        private void Awake()
        {
            gameView.Init();
            background.sprite = AssetBundleManager.Instance.RetrieveAssetFromBundle<Sprite>("Background");
            
            GameManager.Instance.VictoryAction += (cells) =>
            {
                gameFinishedWindow.OpenWindow();
                gameFinishedWindow.Init(
                    false, 
                    GameManager.Instance.GameTurn.NextEnumElement(1).ToString());
                hintButton.interactable = true;
                undoButton.interactable = true;
            };

            GameManager.Instance.DrawAction += () =>
            {
                gameFinishedWindow.OpenWindow();
                gameFinishedWindow.Init(true, "");
                hintButton.interactable = true;
                undoButton.interactable = true;
            };

            ModeController.OpponentActionCompleted += () =>
            {
                hintButton.interactable = false;
                undoButton.interactable = false;
            };
            
            ModeController.PlayerActionCompleted += () =>
            {
                hintButton.interactable = true;
                undoButton.interactable = true;
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


        public void UndoButton()
        {
            GameManager.Instance.UndoRequested();
        }
    }
}
