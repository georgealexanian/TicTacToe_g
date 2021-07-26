using System;
using Logic.Managers;

namespace Logic.GameFlow
{
    public abstract class ModeController
    {
        public static Action OnOpponentAction;
        public static Action OnPlayerAction;
        protected GameManager GameMan;


        protected ModeController(GameManager gameManager)
        {
            GameMan = gameManager;
        }


        public virtual void OpponentAction()
        {
            OnOpponentAction?.Invoke();
        }


        public virtual void PlayerAction()
        {
            OnPlayerAction?.Invoke();
        }
    }
}
