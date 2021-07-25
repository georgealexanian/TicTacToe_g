using TMPro;
using UnityEngine;

namespace UI.GameScene.Windows.GameFinishedWindow
{
    public class GameFinishedWindow : BaseWindow<GameFinishedWindow>
    {
        [SerializeField] private TextMeshProUGUI winnerInfo;
        
        
        public void Init(bool isDraw, string winnerName)
        {
            if (isDraw)
            {
                winnerInfo.text = "DRAW! No winner.";
            }
            else
            {
                winnerInfo.text = $"The {winnerName} has won this game!";
            }
        }


        public void RestartGameButton()
        {
            CloseWindow();
        }
    }
}
