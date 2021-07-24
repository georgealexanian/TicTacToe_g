using Extensions;
using Logic;
using Logic.Managers;
using UnityEngine;
using UnityEngine.UI;

namespace UI.GameScene.GameView
{
    public class BoardCell : MonoBehaviour
    {
        [SerializeField] private Image cellImage;

        public PlayerMark _cellMarkType { get; private set; }


        public void OnCellClick()
        {
            GetComponent<Button>().interactable = false; 
            
            switch (GameManager.Instance.GameTurn)
            {
                case GameTurn.Player:
                    _cellMarkType = GameManager.Instance.PlayerMark;
                    break;
                case GameTurn.Opponent:
                    _cellMarkType = GameManager.Instance.OpponentMark;
                    break;
            }
            
            GameManager.Instance.SwitchGameTurn();
            
            MarkCell();
        }


        private void MarkCell()
        {
            switch (_cellMarkType)
            {
                case PlayerMark.Crosses:
                    break;
                case PlayerMark.Noughts:
                    break;
            }
        }
    }
}
