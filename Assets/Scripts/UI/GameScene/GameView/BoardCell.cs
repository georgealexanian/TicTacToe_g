using DG.Tweening;
using Logic;
using Logic.Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.GameScene.GameView
{
    public class BoardCell : MonoBehaviour
    {
        [SerializeField] private Image cellImage;
        [SerializeField] private TextMeshProUGUI cellPosition;
        
        private BoardCellPosition BoardCellPosition { get; set; }

        private Sprite _xMarkSprite;
        private Sprite _0MarkSprite;
        
        public PlayerMark _cellMarkType { get; private set; }


        public void Init()
        {
            _xMarkSprite = AssetBundleManager.Instance.RetrieveAssetFromBundle<Sprite>("XMark");
            _0MarkSprite = AssetBundleManager.Instance.RetrieveAssetFromBundle<Sprite>("0Mark");
        }


        public void CalculateBoardCellPosition(int index)
        {
            BoardCellPosition = GameManager.Instance.CalculateBoardCellPosition(index);
            cellPosition.text = $"x: {BoardCellPosition.x} | y: {BoardCellPosition.y}";
        }
        

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
                    cellImage.sprite = _xMarkSprite;
                    break;
                case PlayerMark.Noughts:
                    cellImage.sprite = _0MarkSprite;
                    break;
            }
            cellImage.DOFade(1, 0.5f);
        }
    }
}
