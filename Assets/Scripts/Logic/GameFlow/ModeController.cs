using System;

namespace Logic.GameFlow
{
    public abstract class ModeController
    {
        public static Action OpponentActionCompleted;
        public static Action PlayerActionCompleted;
        
        
        public virtual void OpponentAction()
        {
            OpponentActionCompleted?.Invoke();
        }


        public virtual void PlayerAction()
        {
            PlayerActionCompleted?.Invoke();
        }
    }
}
