using System;
using UI.GameScene.Windows.SettingsWindow;
using UnityEngine;

namespace UI.GameScene
{
    public class GameSceneCanvasController : MonoBehaviour
    {
        [SerializeField] private SettingsWindow settingsWindow;
        [SerializeField] private GameView.GameView gameView;


        private void Awake()
        {
            gameView.Init();
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
