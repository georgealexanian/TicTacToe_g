using DG.Tweening;
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

        private Sprite _XMarkSprite;
        private Sprite _0MarkSprite;
        
        public PlayerMark _cellMarkType { get; private set; }


        public void Init()
        {
            _XMarkSprite = AssetBundleManager.Instance.RetrieveAssetFromBundle<Sprite>("XMark");
            _0MarkSprite = AssetBundleManager.Instance.RetrieveAssetFromBundle<Sprite>("0Mark");
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
                    cellImage.sprite = _XMarkSprite;
                    break;
                case PlayerMark.Noughts:
                    cellImage.sprite = _0MarkSprite;
                    break;
            }
            cellImage.DOFade(1, 0.5f);
        }
    }
}
