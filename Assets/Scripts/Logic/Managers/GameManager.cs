using Extensions;
using UI.GameScene.Windows.SettingsWindow;
using Logic;
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
        
        
        
        public override void Initialize()
        {
            WindowsManager.Instance.WindowClosedCallBack += OnSettingsWindowClosed;
            PlayerMark = EnumExtensions.RandomEnumValue<PlayerMark>();
            OpponentMark = PlayerMark.NextEnumElement(1);
            GameTurn = PlayerMark == PlayerMark.Crosses ? GameTurn.Player : GameTurn.Opponent;
        }


        public void SwitchGameTurn()
        {
            GameTurn = GameTurn.NextEnumElement(1);
        }


        private void OnSettingsWindowClosed(string winName)
        {
            if (winName.Equals(nameof(SettingsWindow)))
            {
                
            }
        }
    }
}
