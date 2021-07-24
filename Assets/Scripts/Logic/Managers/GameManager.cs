using Extensions;

namespace Logic.Managers
{
    public class GameManager : BaseManager<GameManager>
    {
        public int GridSize { get; set; } = 3;
        public GameStates.OpponentType OpponentType { get; set; } = GameStates.OpponentType.LocalHuman;
        public GameStates.PlayerMark PlayerMark => EnumExtensions.RandomEnumValue<GameStates.PlayerMark>();
        public GameStates.DifficultyLevel GameDifficulty { get; set; } = GameStates.DifficultyLevel.Easy;

        
        public override void Initialize()
        {
            
        }
    }
}
