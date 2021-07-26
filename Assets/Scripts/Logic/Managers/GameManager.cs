using System;
using System.Collections.Generic;
using System.Linq;
using Extensions;
using Logic.GameFlow;
using UI.GameScene.GameView;
using UI.GameScene.Windows.GameFinishedWindow;
using UI.GameScene.Windows.SettingsWindow;

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
        public DateTime GameStartTime { get; private set; }

        public readonly List<BoardCell> CachedCells = new List<BoardCell>();
        public Action<List<BoardCellPosition>> VictoryAction;
        public Action DrawAction;
        public Action<BoardCellPosition> HintPositionAction;
        public Action<BoardCellPosition> UndoAction;
        public Action RestartGameAction;

        private readonly Stack<BoardCellPosition> _trackedSteps  = new Stack<BoardCellPosition>();
        private HintPositionInfo _hintPosition = new HintPositionInfo();
        private List<BoardCell> _currentGameCells = new List<BoardCell>();
        private ModeController _modeController = new HumanToHumanModeController();
        private bool _isWon;
        
        
        public override void Initialize()
        {
            WindowsManager.Instance.WindowClosedCallBack += OnWindowClosed;
            GameTurn = GameTurn.Player; 
            ChooseModeController();
        }


        public void UndoRequested()
        {
            UndoAction?.Invoke(_trackedSteps.Pop());
            GameTurn = GameTurn == GameTurn.Player ? GameTurn.Opponent : GameTurn.Player;
        }


        public void HintRequested()
        {
            if (GameTurn == GameTurn.Player)
            {
                HintPositionAction?.Invoke(_hintPosition.pos);
            }
        }


        private void SwitchGameTurn()
        {
            GameTurn = GameTurn.NextEnumElement(1);
        }


        public void GameStarting()
        {
            GameStartTime = DateTime.UtcNow;
            _currentGameCells = CachedCells.Take(GridSize * GridSize).ToList();
            _trackedSteps.Clear();
        }


        private void OnWindowClosed(string winName)
        {
            if (winName.Equals(nameof(SettingsWindow)))
            {
                _currentGameCells = CachedCells.Take(GridSize * GridSize).ToList();
                ChooseModeController();
                
            }
            else if(winName.Equals(nameof(GameFinishedWindow)))
            {
                RestartGameAction?.Invoke();
            }
        }


        private void ChooseModeController()
        {
            switch (OpponentType)
            {
                case OpponentType.Unknown: 
                case OpponentType.LocalHuman:
                    _modeController = new HumanToHumanModeController();
                    break;
                case OpponentType.Computer:
                    _modeController = new HumanToComputerModeController();
                    break;
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


        public void StartCheckingVictory(BoardCellPosition chosenBoardCellPosition)
        {
            _trackedSteps.Push(chosenBoardCellPosition);
            SwitchGameTurn();

            _hintPosition.pos = new BoardCellPosition(0, 0);
            _hintPosition.isWinningPos = false;
            
            for (int i = 0; i < GridSize; i++)
            {
                var pos = i + 1;
                CheckVictoryInColumnsOrRows(cell => cell.BoardCellPosition.x == pos);
                CheckVictoryInColumnsOrRows(cell => cell.BoardCellPosition.y == pos);
                CheckVictoryInDiagonal(true);
                CheckVictoryInDiagonal(false);
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


        private void CheckVictoryInDiagonal(bool leftToRight)
        {
            if (_isWon)
            {
                return;
            }

            List<BoardCell> tempList = new List<BoardCell>();
            if (leftToRight)
            {
                tempList = _currentGameCells.FindAll(x => x.BoardCellPosition.x == x.BoardCellPosition.y);
            }
            else
            {
                for (int i = 0; i < GridSize; i++)
                {
                    var cellToAdd = _currentGameCells
                        .Find(cell => cell.BoardCellPosition.x == GridSize - i && cell.BoardCellPosition.y == i + 1);
                    tempList.Add(cellToAdd);
                }
            }
            
            CheckVictory(tempList);
        }


        private void CheckVictory(List<BoardCell> tempList)
        {
            if (tempList.Exists(x => x.CellMarkType == PlayerMark.Unknown))
            {
                CheckHintPositions(tempList);
                NextStepAction();
                return;
            }

            if (tempList.Exists(x => x.CellMarkType == PlayerMark.Crosses) && tempList.Exists(x => x.CellMarkType == PlayerMark.Noughts))
            {
                CheckDraw();
            }
            else
            {
                CompleteVictory(tempList);
            }
        }


        private void CompleteVictory(List<BoardCell> tempList)
        {
            _isWon = true;
            VictoryAction?.Invoke(tempList.Select(x => x.BoardCellPosition).ToList());
            _trackedSteps.Clear();
            GameTurn = GameTurn.Player;
        }


        private void CheckDraw()
        {
            if (!_currentGameCells.Exists(x => x.CellMarkType == PlayerMark.Unknown))
            {
                DrawAction?.Invoke();
                _trackedSteps.Clear();
                GameTurn = GameTurn.Player;
            }
            else
            {
                //continuing the game since no Draw or Victory has been achieved
                NextStepAction();
            }
        }


        private void NextStepAction()
        {
            switch (GameTurn)
            {
                case GameTurn.Opponent:
                    _modeController.OpponentAction();
                    break;
                case GameTurn.Player:
                    _modeController.PlayerAction();
                    break;
            }
        }
        

        private void CheckHintPositions(List<BoardCell> tempList)
        {
            if (_hintPosition.isWinningPos)
            {
                return;
            }
            
            if (!tempList.Exists(x => x.CellMarkType == PlayerMark.Noughts) && tempList.FindAll(x => x.CellMarkType == PlayerMark.Crosses).Count == GridSize - 1)
            {
                ChoosePotentialWinningPosition(tempList);
            }
            else
            {
                ChooseRandomUnoccupiedPosition();
            }
        }


        private void ChoosePotentialWinningPosition(List<BoardCell> tempList)
        {
            var winningHintPos = tempList.SingleOrDefault(x => x.CellMarkType == PlayerMark.Unknown);
            if (winningHintPos != null)
            {
                _hintPosition.pos = winningHintPos.BoardCellPosition;
                _hintPosition.isWinningPos = true;
            }
        }
        

        private void ChooseRandomUnoccupiedPosition()
        {
            _hintPosition.pos = _currentGameCells.First(x => x.CellMarkType == PlayerMark.Unknown).BoardCellPosition;
            _hintPosition.isWinningPos = false;
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
