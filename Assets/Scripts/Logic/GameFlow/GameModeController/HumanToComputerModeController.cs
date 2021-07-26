using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Logic.GameFlow.GameDifficultyController;
using Logic.Managers;
using UI.GameScene.GameView;
using UnityEngine;

namespace Logic.GameFlow.GameModeController
{
    public class HumanToComputerModeController : ModeController
    {
        public HumanToComputerModeController(GameManager gameManager) : base(gameManager)
        {
            GameMan.HintPositionAction += HintReceived;
            GameMan.DifficultyLevelAction += DifficultyLevelAction;
            ChooseDifficultyController();
        }
        ~HumanToComputerModeController()
        {
            GameMan.HintPositionAction -= HintReceived;
            GameMan.DifficultyLevelAction -= DifficultyLevelAction;
        }


        private IDifficultyController _difficultyController;


        private void DifficultyLevelAction()
        {
            ChooseDifficultyController();
        }
        
        
        private void ChooseDifficultyController()
        {
            switch (GameMan.GameDifficulty)
            {
                case DifficultyLevel.Unknown: 
                case DifficultyLevel.Easy:
                    _difficultyController = new EasyDifficulty();
                    break;
                case DifficultyLevel.Medium:
                    _difficultyController = new MediumDifficulty();
                    break;
                case DifficultyLevel.Hard:
                    _difficultyController = new HardDifficulty();
                    break;
            }
        }
        
        
        public override async void OpponentAction()
        {
            base.OpponentAction();
            await Task.Delay(100);
            _difficultyController.RequestHintPosition(PlayerMark.Noughts);
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
