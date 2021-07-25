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
        private Image _cellBackground;
        private Button _button;

        private readonly Color32 _whiteColor = new Color32(255, 255, 255, 90);
        private readonly Color32 _greenColor = new Color32(24, 255, 0, 89);
        
        
        
        private void Awake()
        {
            _cellBackground = GetComponent<Image>();
            _button = GetComponent<Button>();
            _cellBackground.DOColor(_whiteColor, 0);
        }


        public void Init()
        {
            _xMarkSprite = AssetBundleManager.Instance.RetrieveAssetFromBundle<Sprite>("XMark");
            _0MarkSprite = AssetBundleManager.Instance.RetrieveAssetFromBundle<Sprite>("0Mark");
            
            GameManager.Instance.VictoryAction += VictoryAction;
            GameManager.Instance.HintPositionAction += HintPositionAction;
            GameManager.Instance.UndoAction += UndoAction;
        }


        public void ResetCell(bool alsoPosition)
        {
            _cellBackground.color = _whiteColor;
            cellImage.DOFade(0, 0.0f);
            cellImage.sprite = null;
            if (alsoPosition)
            {
                BoardCellPosition = new BoardCellPosition(0, 0);
            }
            CellMarkType = PlayerMark.Unknown;
            _button.interactable = true;
        }


        private void UndoAction(BoardCellPosition boardCellPosition)
        {
            if (BoardCellPosition.x == boardCellPosition.x && BoardCellPosition.y == boardCellPosition.y)
            {
                ResetCell(false);
            }
        }


        private void VictoryAction(List<BoardCellPosition> winningCellPositions)
        {
            if (winningCellPositions.Exists(cell => cell.x == BoardCellPosition.x && cell.y == BoardCellPosition.y))
            {
                _cellBackground.color = _greenColor;
            }
        }
        
        
        private void HintPositionAction(BoardCellPosition hintCellPosition)
        {
            if (hintCellPosition.x == BoardCellPosition.x && hintCellPosition.y == BoardCellPosition.y)
            {
                _button.interactable = false;
                _cellBackground.DOColor(_greenColor, 0.5f).OnComplete(() =>
                {
                    _cellBackground.DOColor(_whiteColor, 0.5f).OnComplete(() =>
                    {
                        _button.interactable = true;
                    });
                });
            }
        }


        public void CalculateBoardCellPosition(int index)
        {
            BoardCellPosition = GameManager.Instance.CalculateBoardCellPosition(index);
            cellPosition.text = $"x: {BoardCellPosition.x} | y: {BoardCellPosition.y}";
        }
        

        public void OnCellClick()
        {
            _button.interactable = false; 
            
            switch (GameManager.Instance.GameTurn)
            {
                case GameTurn.Player:
                    CellMarkType = GameManager.Instance.PlayerMark;
                    break;
                case GameTurn.Opponent:
                    CellMarkType = GameManager.Instance.OpponentMark;
                    break;
            }
            
            GameManager.Instance.StartCheckingVictory(BoardCellPosition);
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


        private void OnDestroy()
        {
            GameManager.Instance.VictoryAction -= VictoryAction;
            GameManager.Instance.HintPositionAction -= HintPositionAction;
            GameManager.Instance.UndoAction -= UndoAction;
        }
    }
}
