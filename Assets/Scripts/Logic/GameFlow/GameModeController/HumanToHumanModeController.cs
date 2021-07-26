using Logic.Managers;

namespace Logic.GameFlow.GameModeController
{
    public class HumanToHumanModeController : ModeController
    {
        public HumanToHumanModeController(GameManager gameManager) : base(gameManager)
        {
        }
        
        
        public override void OpponentAction()
        {
            base.OpponentAction();
        }
        
        
        public override void PlayerAction()
        {
            base.PlayerAction();
        }
    }
}
