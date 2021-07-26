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
                undoButton.interactable = GameManager.Instance.OpponentType != OpponentType.Computer;
            };

            GameManager.Instance.DrawAction += () =>
            {
                gameFinishedWindow.OpenWindow();
                gameFinishedWindow.Init(true, "");
                hintButton.interactable = true;
                undoButton.interactable = GameManager.Instance.OpponentType != OpponentType.Computer;
            };

            ModeController.OnOpponentAction += () =>
            {
                hintButton.interactable = false;
                undoButton.interactable = false;
            };
            
            ModeController.OnPlayerAction += () =>
            {
                hintButton.interactable = true;
                undoButton.interactable = GameManager.Instance.OpponentType != OpponentType.Computer;
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
            if (GameManager.Instance.GameTurn == GameTurn.Opponent)
            {
                hintButton.interactable = false;
                undoButton.interactable = false;
            }
        }


        public void UndoButton()
        {
            GameManager.Instance.UndoRequested();
        }
    }
}
