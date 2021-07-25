using System;
using System.Collections.Generic;
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
        
        public BoardCellPosition BoardCellPosition { get; private set; }
        public PlayerMark CellMarkType { get; private set; } = PlayerMark.Unknown;

        private Sprite _xMarkSprite;
        private Sprite _0MarkSprite;
        


        public void Init()
        {
            _xMarkSprite = AssetBundleManager.Instance.RetrieveAssetFromBundle<Sprite>("XMark");
            _0MarkSprite = AssetBundleManager.Instance.RetrieveAssetFromBundle<Sprite>("0Mark");
            GameManager.Instance.VictoryCallBack += VictoryCallBack;
        }


        public void Reset()
        {
            GetComponent<Image>().color = new Color32(255, 255, 255, 123);
            cellImage.DOFade(0, 0.0f);
            cellImage.sprite = null;
            BoardCellPosition = new BoardCellPosition(0, 0);
            CellMarkType = PlayerMark.Unknown;
        }


        private void VictoryCallBack(List<BoardCellPosition> winningCellPositions)
        {
            if (winningCellPositions.Exists(cell=> cell.x == BoardCellPosition.x && cell.y == BoardCellPosition.y))
            {
                GetComponent<Image>().color = new Color32(24, 255, 0, 89);
            }
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
                    CellMarkType = GameManager.Instance.PlayerMark;
                    break;
                case GameTurn.Opponent:
                    CellMarkType = GameManager.Instance.OpponentMark;
                    break;
            }
            
            GameManager.Instance.CheckVictory();
            MarkCell();
        }


        private void MarkCell()
        {
            switch (CellMarkType)
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
