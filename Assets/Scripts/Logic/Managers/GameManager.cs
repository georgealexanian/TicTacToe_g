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

        private List<BoardCell> _currentGameCells = new List<BoardCell>();
        public Action<List<BoardCellPosition>> VictoryCallBack;
        
        
        public override void Initialize()
        {
            WindowsManager.Instance.WindowClosedCallBack += OnSettingsWindowClosed;
            // PlayerMark = EnumExtensions.RandomEnumValue<PlayerMark>(); //should be implemented for PlayerAgainstComputer game
            // OpponentMark = PlayerMark.NextEnumElement(1);
            GameTurn = PlayerMark == PlayerMark.Crosses ? GameTurn.Player : GameTurn.Opponent;
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


        public void CheckVictory()
        {
            for (int i = 0; i < GridSize; i++)
            {
                var pos = i + 1;
                CheckVictoryInColumnsOrRows(cell => cell.BoardCellPosition.x == pos);
                CheckVictoryInColumnsOrRows(cell => cell.BoardCellPosition.y == pos);
            } 
            SwitchGameTurn();
        }


        private void CheckVictoryInColumnsOrRows(Predicate<BoardCell> predicate)
        {
            List<BoardCell> tempList = new List<BoardCell>();
            tempList = _currentGameCells.FindAll(predicate);

            if (tempList.Count == 0)
            {
                return;
            }
            
            if (tempList.Exists(x => x.CellMarkType == PlayerMark.Unknown) 
                 || (tempList.Exists(x => x.CellMarkType == PlayerMark.Crosses) 
                && tempList.Exists(x => x.CellMarkType == PlayerMark.Noughts)))
            {
                    
            }
            else
            {
                Debug.Log("Victory");
                VictoryCallBack?.Invoke(tempList.Select(x => x.BoardCellPosition).ToList());
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
}
