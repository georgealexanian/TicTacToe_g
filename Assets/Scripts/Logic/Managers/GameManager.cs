using System;
using System.Collections.Generic;
using System.Linq;
using Extensions;
using UI.GameScene.GameView;
using UI.GameScene.Windows.SettingsWindow;
using UnityEngine;

namespace Logic.Managers
{
    public class GameManager : BaseManager<GameManager>
    {
        public int GridSize { get; set; } = 3;
        public OpponentType OpponentType { get; set; } = OpponentType.LocalHuman;
        public PlayerMark PlayerMark { get; private set; } = PlayerMark.Crosses;
        public PlayerMark OpponentMark { get; private set; } = PlayerMark.Noughts;
        public GameTurn GameTurn { get; private set; } = GameTurn.Player;
        public DifficultyLevel GameDifficulty { get; set; } = DifficultyLevel.Easy;

        public readonly List<BoardCell> CachedCells = new List<BoardCell>();
        public Action<List<BoardCellPosition>> VictoryCallBack;
        public Action DrawCallBack;
        public Action<BoardCellPosition> HintPositionCallBack;

        private HintPositionInfo hintPosition = new HintPositionInfo();
        private List<BoardCell> _currentGameCells = new List<BoardCell>();
        private bool _isWon;
        
        
        
        
        public override void Initialize()
        {
            WindowsManager.Instance.WindowClosedCallBack += OnSettingsWindowClosed;
            // PlayerMark = EnumExtensions.RandomEnumValue<PlayerMark>(); //no longer needed! decided the player to be Crosses
            // OpponentMark = PlayerMark.NextEnumElement(1); //no longer needed! decided the player to be Noughts
            GameTurn = GameTurn.Player; // PlayerMark == PlayerMark.Crosses ? GameTurn.Player : GameTurn.Opponent;
        }


        public void HintRequested()
        {
            if (GameTurn == GameTurn.Player)
            {
                HintPositionCallBack?.Invoke(hintPosition.pos);
            }
        }


        private void SwitchGameTurn()
        {
            GameTurn = GameTurn.NextEnumElement(1);
        }


        public void GameStarting()
        {
            _currentGameCells = CachedCells.Take(GridSize * GridSize).ToList();
        }


        private void OnSettingsWindowClosed(string winName)
        {
            if (winName.Equals(nameof(SettingsWindow)))
            {
                _currentGameCells = CachedCells.Take(GridSize * GridSize).ToList();
            }
        }


        public BoardCellPosition CalculateBoardCellPosition(int cellIndex)
        {
            if (cellIndex >= GridSize * GridSize)
            {
                return new BoardCellPosition(0, 0);
            }
            var x = cellIndex - (cellIndex / GridSize) * GridSize + 1;
            var y = (cellIndex / GridSize) + 1;
            return new BoardCellPosition(x, y);
        }


        public void StartCheckingVictory()
        {
            SwitchGameTurn();

            hintPosition.pos = new BoardCellPosition(0, 0);
            hintPosition.isWinningPos = false;
            
            for (int i = 0; i < GridSize; i++)
            {
                var pos = i + 1;
                CheckVictoryInColumnsOrRows(cell => cell.BoardCellPosition.x == pos);
                CheckVictoryInColumnsOrRows(cell => cell.BoardCellPosition.y == pos);
                CheckVictoryInDiagonalLeftToRight();
                CheckVictoryInDiagonalRightToLeft();
            } 
            
            _isWon = false;
        }


        private void CheckVictoryInColumnsOrRows(Predicate<BoardCell> predicate)
        {
            if (_isWon)
            {
                return;
            }
            
            List<BoardCell> tempList = new List<BoardCell>();
            tempList = _currentGameCells.FindAll(predicate);

            if (tempList.Count == 0)
            {
                return;
            }

            CheckVictory(tempList);
        }


        private void CheckVictoryInDiagonalLeftToRight()
        {
            if (_isWon)
            {
                return;
            }
            
            List<BoardCell> tempList = new List<BoardCell>();
            tempList = _currentGameCells.FindAll(x => x.BoardCellPosition.x == x.BoardCellPosition.y);
            CheckVictory(tempList);
        }


        private void CheckVictoryInDiagonalRightToLeft()
        {
            if (_isWon)
            {
                return;
            }

            List<BoardCell> tempList = new List<BoardCell>();
            for (int i = 0; i < GridSize; i++)
            {
                var cellToAdd = _currentGameCells
                    .Find(cell => cell.BoardCellPosition.x == GridSize - i && cell.BoardCellPosition.y == i + 1);
                tempList.Add(cellToAdd);
            }
            
            CheckVictory(tempList);
        }


        private void CheckVictory(List<BoardCell> tempList)
        {
            if (tempList.Exists(x => x.CellMarkType == PlayerMark.Unknown))
            {
                CheckHintPositions(tempList);
                return;
            }

            if (tempList.Exists(x => x.CellMarkType == PlayerMark.Crosses) && tempList.Exists(x => x.CellMarkType == PlayerMark.Noughts))
            {
                CheckDraw();
            }
            else
            {
                _isWon = true;
                VictoryCallBack?.Invoke(tempList.Select(x => x.BoardCellPosition).ToList());
                GameTurn = GameTurn.Player;
            }
        }


        private void CheckDraw()
        {
            if (!_currentGameCells.Exists(x => x.CellMarkType == PlayerMark.Unknown))
            {
                DrawCallBack?.Invoke();
                GameTurn = GameTurn.Player;
            }
        }


        private void CheckHintPositions(List<BoardCell> tempList)
        {
            if (hintPosition.isWinningPos)
            {
                return;
            }
            
            if (!tempList.Exists(x => x.CellMarkType == PlayerMark.Noughts) && tempList.FindAll(x => x.CellMarkType == PlayerMark.Crosses).Count == GridSize - 1)
            {
                var winningHintPos = tempList.SingleOrDefault(x => x.CellMarkType == PlayerMark.Unknown);
                if (winningHintPos != null)
                {
                    hintPosition.pos = winningHintPos.BoardCellPosition;
                    hintPosition.isWinningPos = true;
                }
            }
            else
            {
                hintPosition.pos = _currentGameCells.First(x => x.CellMarkType == PlayerMark.Unknown).BoardCellPosition;
                hintPosition.isWinningPos = false;
            }
        }
    }


    
    [Serializable]
    public struct BoardCellPosition
    {
        public int x;
        public int y;

        public BoardCellPosition(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }


    [Serializable]
    public struct HintPositionInfo
    {
        public BoardCellPosition pos;
        public bool isWinningPos;

        public HintPositionInfo(BoardCellPosition pos, bool isWinningPos)
        {
            this.pos = pos;
            this.isWinningPos = isWinningPos;
        }
    }
}
