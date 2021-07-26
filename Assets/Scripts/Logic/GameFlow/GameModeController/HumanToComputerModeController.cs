using System.Collections.Generic;
using System.Linq;
using Logic.Managers;
using UI.GameScene.GameView;

namespace Logic.GameFlow.GameModeController
{
    public class HumanToComputerModeController : ModeController
    {
        public HumanToComputerModeController(GameManager gameManager) : base(gameManager)
        {
            GameMan.HintPositionAction += HintReceived;
        }
        ~HumanToComputerModeController()
        {
            GameMan.HintPositionAction -= HintReceived;
        }
        
        
        public override void OpponentAction()
        {
            base.OpponentAction();

            List<BoardCell> tempList;
            for (var i = 0; i < GameMan.GridSize; i++)
            {
                var pos = i + 1;
                tempList = GameMan.CurrentGameCells.FindAll(cell => cell.BoardCellPosition.x == pos);
                GameMan.CheckHintPositions(tempList, PlayerMark.Crosses);
                tempList = GameMan.CurrentGameCells.FindAll(cell => cell.BoardCellPosition.y == pos);
                GameMan.CheckHintPositions(tempList, PlayerMark.Crosses);
                tempList = GameMan.GetDiagonalCells(true);
                GameMan.CheckHintPositions(tempList, PlayerMark.Crosses);
                tempList = GameMan.GetDiagonalCells(false);
                GameMan.CheckHintPositions(tempList, PlayerMark.Crosses);
            } 
            GameMan.HintRequested();
        }
        
        
        private void HintReceived(BoardCellPosition hintPos)
        {
            if (GameMan.GameTurn == GameTurn.Opponent)
            {
                var cell = GameMan.CurrentGameCells
                    .SingleOrDefault(x => x.BoardCellPosition.x == hintPos.x && x.BoardCellPosition.y == hintPos.y);
                if (cell != null)
                {
                    cell.OnCellClick();
                }
            }
        }
        
        
        public override void PlayerAction()
        {
            base.PlayerAction();
        }
    }
}
