using System.Collections.Generic;
using Logic.Managers;
using UI.GameScene.GameView;
using UnityEngine;

namespace Logic.GameFlow.GameDifficultyController
{
    public interface IDifficultyController
    {
        public void RequestHintPosition(PlayerMark hintAgainst);
    }


    public class EasyDifficulty : IDifficultyController
    {
        private const int UpperRandomValue = 4;
        
        public void RequestHintPosition(PlayerMark hintAgainst)
        {
            GameManager.Instance.HintRequested(hintAgainst, UpperRandomValue);
        }
    }
    
    
    public class MediumDifficulty : IDifficultyController
    {
        private const int UpperRandomValue = 2;

        public void RequestHintPosition(PlayerMark hintAgainst)
        {
            GameManager.Instance.HintRequested(hintAgainst, UpperRandomValue);
        }
    }
    
    
    public class HardDifficulty : IDifficultyController
    {
        private const int UpperRandomValue = 1;
        
        public void RequestHintPosition(PlayerMark hintAgainst)
        {
            GameManager.Instance.HintRequested(hintAgainst, UpperRandomValue);
        }
    }
}
