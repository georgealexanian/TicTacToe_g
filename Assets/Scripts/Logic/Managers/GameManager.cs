using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Extensions;
using Logic.GameFlow.GameModeController;
using UI.GameScene.GameView;
using UI.GameScene.Windows.GameFinishedWindow;
using UI.GameScene.Windows.SettingsWindow;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Logic.Managers
{
    public class GameManager : BaseManager<GameManager>
    {
        public int GridSize { get; set; } = 3;
        public OpponentType OpponentType { get; set; } = OpponentType.LocalHuman;
        public PlayerMark PlayerMark { get; private set; } = PlayerMark.Crosses;
        public PlayerMark OpponentMark { get; private set; } = PlayerMark.Noughts;
        public GameTurn GameTurn { get; private set; } = GameTurn.Player;
        public DifficultyLevel GameDifficulty
        {
            get => _gameDifficulty;
            set
            {
                _gameDifficulty = value;
                DifficultyLevelAction?.Invoke();
            }
        }

        public DateTime GameStartTime { get; private set; }
        public List<BoardCell> CurrentGameCells { get; private set; } = new List<BoardCell>();

        public readonly List<BoardCell> CachedCells = new List<BoardCell>();
        public Action<List<BoardCellPosition>> VictoryAction;
        public Action DrawAction;
        public Action<BoardCellPosition> HintPositionAction;
        public Action<BoardCellPosition> UndoAction;
        public Action RestartGameAction;
        public Action DifficultyLevelAction;

        private readonly Stack<BoardCellPosition> _trackedSteps  = new Stack<BoardCellPosition>();
        private HintPositionInfo _hintPosition = new HintPositionInfo();
        private ModeController _modeController;
        private bool _isWon;
        private bool _nextStep;
        private DifficultyLevel _gameDifficulty = DifficultyLevel.Easy;
        
        
        public override void Initialize()
        {
            WindowsManager.Instance.WindowClosedCallBack += OnWindowClosed;
            GameTurn = GameTurn.Player; 
            ChooseModeController();
        }

        
        public void GameStarting()
        {
            GameStartTime = DateTime.UtcNow;
            CurrentGameCells = CachedCells.Take(GridSize * GridSize).ToList();
            GameTurn = GameTurn.Player;
            _trackedSteps.Clear();
            _nextStep = false;
        }
        

        public void UndoRequested()
        {
            GameTurn = GameTurn == GameTurn.Player ? GameTurn.Opponent : GameTurn.Player;
            UndoAction?.Invoke(_trackedSteps.Pop());
        }


        public void HintRequested(PlayerMark hintAgainst, int upperRandomValue)
        {
            CheckHintPositions(hintAgainst, upperRandomValue);
            HintPositionAction?.Invoke(_hintPosition.pos);
        }


        private void SwitchGameTurn()
        {
            GameTurn = GameTurn.NextEnumElement(1);
        }


        private void OnWindowClosed(string winName)
        {
            if (winName.Equals(nameof(SettingsWindow)))
            {
                CurrentGameCells = CachedCells.Take(GridSize * GridSize).ToList();
                ChooseModeController();
                
            }
            else if (winName.Equals(nameof(GameFinishedWindow)))
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
                    _modeController = new HumanToHumanModeController(this);
                    break;
                case OpponentType.Computer:
                    _modeController = new HumanToComputerModeController(this);
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
            
            for (var i = 0; i < GridSize; i++)
            {
                var pos = i + 1;
                CheckColumnsOrRows(cell => cell.BoardCellPosition.x == pos);
                CheckColumnsOrRows(cell => cell.BoardCellPosition.y == pos);
            } 
            CheckDiagonals(true);
            CheckDiagonals(false);
            
            _isWon = false;
        }


        private void CheckColumnsOrRows(Predicate<BoardCell> predicate)
        {
            if (_isWon)
            {
                return;
            }
            
            List<BoardCell> tempList = new List<BoardCell>();
            tempList = CurrentGameCells.FindAll(predicate);

            if (tempList.Count == 0)
            {
                return;
            }

            CheckVictory(tempList);
        }


        private void CheckDiagonals(bool leftToRight)
        {
            if (_isWon)
            {
                return;
            }

            List<BoardCell> tempList = new List<BoardCell>();
            if (leftToRight)
            {
                tempList = GetDiagonalCells(true);
            }
            else
            {
                tempList = GetDiagonalCells(false);
                _nextStep = true;
            }
            
            CheckVictory(tempList);
        }


        private List<BoardCell> GetDiagonalCells(bool leftToRight)
        {
            if (leftToRight)
            {
                return CurrentGameCells.FindAll(x => x.BoardCellPosition.x == x.BoardCellPosition.y);
            }
            else
            {
                var tempList = new List<BoardCell>();
                for (int i = 0; i < GridSize; i++)
                {
                    var cellToAdd = CurrentGameCells
                        .Find(cell => cell.BoardCellPosition.x == GridSize - i && cell.BoardCellPosition.y == i + 1);
                    tempList.Add(cellToAdd);
                }
                return tempList;
            }
        }


        private void CheckVictory(List<BoardCell> tempList)
        {
            if (tempList.Exists(x => x.CellMarkType == PlayerMark.Unknown))
            {
                // CheckHintPositions(tempList, PlayerMark.Noughts, 1);
                if (_nextStep)
                {
                    NextStepAction();
                }
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
            if (!CurrentGameCells.Exists(x => x.CellMarkType == PlayerMark.Unknown))
            {
                DrawAction?.Invoke();
                _trackedSteps.Clear();
                GameTurn = GameTurn.Player;
            }
            else
            {
                //continuing the game since no Draw or Victory has been achieved
                if (_nextStep)
                {
                    NextStepAction();
                }
            }
        }


        private void NextStepAction()
        {
            _nextStep = false;
            switch (GameTurn)
            {
                case GameTurn.Opponent:
                    _modeController.OpponentAction();
                    Debug.Log("opponent action");
                    break;
                case GameTurn.Player:
                    _modeController.PlayerAction();
                    Debug.Log("player action");
                    break;
            }
        }

        
        private void CheckHintPositions(PlayerMark hintAgainst, int upperRandomValue)
        {
            List<BoardCell> tempList;
            for (var i = 0; i < GridSize; i++)
            {
                var pos = i + 1;
                tempList = CurrentGameCells.FindAll(cell => cell.BoardCellPosition.x == pos);
                FindHintPos();
                tempList = CurrentGameCells.FindAll(cell => cell.BoardCellPosition.y == pos);
                FindHintPos();
            }
            
            tempList = GetDiagonalCells(true);
            FindHintPos();
            tempList = GetDiagonalCells(false);
            FindHintPos();

            void FindHintPos()
            {
                if (_hintPosition.isWinningPos)
                {
                    return;
                }
                if (!tempList.Exists(x => x.CellMarkType == hintAgainst) && tempList.FindAll(x => x.CellMarkType == hintAgainst.NextEnumElement(1)).Count == GridSize - 1)
                {
                    if (Random.Range(0, upperRandomValue) == 0)
                    {
                        ChoosePotentialWinningPosition(tempList);
                    }
                    else
                    {
                        ChooseRandomUnoccupiedPosition();
                    }
                }
                else
                {
                    ChooseRandomUnoccupiedPosition();
                }
            }
        }


        private void ChoosePotentialWinningPosition(List<BoardCell> tempList)
        {
            if (tempList.Exists(x => x.CellMarkType == PlayerMark.Unknown))
            {
                var winningHintPos = tempList.FirstOrDefault(x => x.CellMarkType == PlayerMark.Unknown);
                if (winningHintPos != null)
                {
                    _hintPosition.pos = winningHintPos.BoardCellPosition;
                    _hintPosition.isWinningPos = true;
                }
            }
        }
        

        private void ChooseRandomUnoccupiedPosition()
        {
            _hintPosition.pos = CurrentGameCells.First(x => x.CellMarkType == PlayerMark.Unknown).BoardCellPosition;
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
