using UnityEngine;

namespace UI.GameScene.Windows.GameFinishedWindow
{
    public class GameFinishedWindow : BaseWindow<GameFinishedWindow>
    {
        public void Init()
        {
            
        }


        public void RestartGameButton()
        {
            CloseWindow();
        }
    }
}
