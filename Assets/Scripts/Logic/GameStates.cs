using UnityEngine;

namespace Logic
{
    public class GameStates : MonoBehaviour
    {
        public enum GameTurn
        {
            Unknown = 0,
            Player = 1,
            Opponent = 2
        }

        public enum OpponentType
        {
            Unknown = 0,
            LocalHuman = 1,
            RemoteHuman = 2,
            Computer = 3
        }
        
        public enum PlayerMark
        {
            Unknown = 0,
            Noughts = 1, 
            Crosses = 2
        }
    }
}
